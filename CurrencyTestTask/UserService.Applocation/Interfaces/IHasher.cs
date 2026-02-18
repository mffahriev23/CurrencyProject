namespace UserService.Application.Interfaces
{
    public interface IHasher
    {
        string GetHash(string text);

        bool VerifyText(string hashedText, string text);
    }
}
