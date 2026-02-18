namespace UserService.Domain.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public required string HashToken { get; set; }

        public Guid UserId { get; set; }

        public DateTime Expires { get; set; }

        public DateTime Created { get; set; }

        public bool IsRevoked { get; set; }

        public User? User { get; set; }
    }
}
