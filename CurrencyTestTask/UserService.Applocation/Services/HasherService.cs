using Microsoft.AspNetCore.Identity;
using UserService.Application.Interfaces;

namespace UserService.Application.Services
{
    public class HasherService : IHasher
    {
        private readonly PasswordHasher<string> _passwordHasher = new();
        private const string _userName = "user_identity";

        public string GetHash(string password)
        {
            return _passwordHasher.HashPassword(_userName, password);
        }

        public bool VerifyText(string hashedPassword, string providedPassword)
        {
            PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(
                _userName,
                hashedPassword,
                providedPassword
            );

            return result == PasswordVerificationResult.Success;
        }
    }
}
