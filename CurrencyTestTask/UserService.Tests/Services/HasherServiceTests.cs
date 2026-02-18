using UserService.Application.Interfaces;
using UserService.Application.Services;

namespace UserService.Tests.Services
{
    public class HasherServiceTests
    {
        private readonly IHasher _hasher = new HasherService();

        [Fact]
        public void GetHash_And_VerifyText_WithSamePassword_ShouldReturnTrue()
        {
            string password = "P@ssw0rd!";

            string hash = _hasher.GetHash(password);

            bool result = _hasher.VerifyText(hash, password);

            Assert.True(result);
        }

        [Fact]
        public void GetHash_And_VerifyText_WithDifferentPassword_ShouldReturnFalse()
        {
            string password = "P@ssw0rd!";
            string other = "OtherPassword";

            string hash = _hasher.GetHash(password);

            bool result = _hasher.VerifyText(hash, other);

            Assert.False(result);
        }

        [Fact]
        public void GetHash_ForDifferentPasswords_ShouldProduceDifferentHashes()
        {
            string password1 = "Password1";
            string password2 = "Password2";

            string hash1 = _hasher.GetHash(password1);
            string hash2 = _hasher.GetHash(password2);

            Assert.NotEqual(hash1, hash2);
        }
    }
}

