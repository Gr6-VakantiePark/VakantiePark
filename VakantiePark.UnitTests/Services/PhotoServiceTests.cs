using Moq;
using VakantiePark.Application.Interfaces;
using VakantiePark.Application.Services;
using VakantiePark.Domain.Entities;

namespace VakantiePark.UnitTests.Services
{
    public class PhotoServiceTests
    {
        private readonly Mock<IPhotoRepository> _repository = new();
        private readonly PhotoService _sut;

        public PhotoServiceTests()
        {
            _sut = new PhotoService(_repository.Object);
        }

        [Fact]
        public async Task GetFileAsync_Found_ReturnsFile()
        {
            _repository.Setup(r => r.GetWithFileAsync(3)).ReturnsAsync(new Photo
            {
                Id = 3,
                FileName = "berk4.jpg",
                ContentType = "image/jpeg",
                File = new PhotoFile { PhotoId = 3, Data = [1, 2, 3] },
            });

            var file = await _sut.GetFileAsync(3);

            Assert.Equal("image/jpeg", file.ContentType);
            Assert.Equal(new byte[] { 1, 2, 3 }, file.Data);
        }

        [Fact]
        public async Task GetFileAsync_NotFound_ThrowsKeyNotFoundException()
        {
            _repository.Setup(r => r.GetWithFileAsync(3)).ReturnsAsync((Photo?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.GetFileAsync(3));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetFileAsync_InvalidId_ThrowsArgumentException(int id)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetFileAsync(id));

            _repository.Verify(r => r.GetWithFileAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
