namespace ATechnologiesAssignment.Domain.Entities
{
    public class BlockedAttemptLog
    {
        public string Id { get; set; }
        public string IpAddress { get; set; }
        public DateTime Timestamp { get; set; }
        public string CountryCode { get; set; }
        public bool IsBlocked { get; set; }
        public string UserAgent { get; set; }
    }
}
