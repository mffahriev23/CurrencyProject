namespace Authorization.Options
{
    public record JwtManagerOptions
    {
        public string? Secret { get; init; }

        public int? ExpirationTimesOnMinutes { get; init; }
    }
}
