using ATechnologiesAssignment.Domain.Base;

namespace ATechnologiesAssignment.Domain.Entities
{
    public class TemporalBlockedCountry : BaseEntity
    {
        public string CountryCode { get; set; }
        public int DurationMinutes { get; set; }
    }
}
