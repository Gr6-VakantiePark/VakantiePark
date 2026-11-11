using VakantiePark.Infrastructure.Security;

namespace VakantiePark.UnitTests.Security
{
    public class BCryptPasswordHasherTests
    {
        private readonly BCryptPasswordHasher _hasher = new();

        [Fact]
        public void Hash_DoesNotContainThePlainPassword()
        {
            string hash = _hasher.Hash("Zomer2027!");

            Assert.DoesNotContain("Zomer2027!", hash);
            Assert.StartsWith("$2", hash);
        }

        [Fact]
        public void Hash_GivesADifferentHashEachTime()
        {
            Assert.NotEqual(_hasher.Hash("Zomer2027!"), _hasher.Hash("Zomer2027!"));
        }

        [Fact]
        public void Verify_AcceptsTheCorrectPassword()
        {
            string hash = _hasher.Hash("Zomer2027!");

            Assert.True(_hasher.Verify("Zomer2027!", hash));
        }

        [Theory]
        [InlineData("zomer2027!")]
        [InlineData("Zomer2027")]
        [InlineData("")]
        public void Verify_RejectsAWrongPassword(string attempt)
        {
            string hash = _hasher.Hash("Zomer2027!");

            Assert.False(_hasher.Verify(attempt, hash));
        }

        [Fact]
        public void Verify_RejectsAnInvalidHash()
        {
            Assert.False(_hasher.Verify("Zomer2027!", "geen-hash"));
        }

        [Fact]
        public void Hash_RejectsAnEmptyPassword()
        {
            Assert.Throws<ArgumentException>(() => _hasher.Hash(string.Empty));
        }
    }
}
