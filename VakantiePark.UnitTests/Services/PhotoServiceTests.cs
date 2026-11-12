using Moq;
using VakantiePark.Application.Interfaces;
using VakantiePark.Application.Services;
using VakantiePark.Domain.Entities;
using VakantiePark.Domain.Interfaces;

namespace VakantiePark.UnitTests.Services
{
    public class PhotoServiceTests
    {
        private readonly Mock<IPhotoRepository> _repository = new();
        private readonly PhotoService _sut;

        private static readonly Guid PhotoId = Guid.NewGuid();

        public PhotoServiceTests()
        {
            _sut = new PhotoService(_repository.Object);
        }

        [Fact]
        public async Task GetFileAsync_Found_ReturnsFile()
        {
            _repository.Setup(r => r.GetWithFileAsync(PhotoId)).ReturnsAsync(new Photo
            {
                Id = PhotoId,
                FileName = "berk4.jpg",
                ContentType = "image/jpeg",
                File = new PhotoFile { PhotoId = PhotoId, Data = [1, 2, 3] },
            });

            var file = await _sut.GetFileAsync(PhotoId);

            Assert.Equal("image/jpeg", file.ContentType);
            Assert.Equal(new byte[] { 1, 2, 3 }, file.Data);
        }

        [Fact]
        public async Task GetFileAsync_NotFound_ThrowsKeyNotFoundException()
        {
            _repository.Setup(r => r.GetWithFileAsync(PhotoId)).ReturnsAsync((Photo?)null);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.GetFileAsync(PhotoId));
        }

        [Fact]
        public async Task GetFileAsync_EmptyId_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetFileAsync(Guid.Empty));

            _repository.Verify(r => r.GetWithFileAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}
