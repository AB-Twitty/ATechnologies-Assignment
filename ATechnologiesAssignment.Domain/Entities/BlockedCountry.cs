using ATechnologiesAssignment.Domain.Base;

namespace ATechnologiesAssignment.Domain.Entities
{
    public class BlockedCountry : BaseEntity
    {
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
    }
}
