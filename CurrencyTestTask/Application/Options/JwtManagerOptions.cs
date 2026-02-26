namespace Application.Options
{
    public record JwtManagerOptions
    {
        public string? Secret { get; init; }

        public int? ExpirationAccessTokenOnMinutes { get; init; }

        public int? ExpirationRefreshTokenOnDay { get; init; }
    }
}
