namespace ATechnologiesAssignment.App.Dtos.TemporalBlockedCountryDtos
{
    public class TemporalBlockedCountryDto
    {
        private string _countryCode;

        public required string CountryCode
        {
            get => _countryCode;
            set => _countryCode = value.ToUpper();
        }
        public int DurationMinutes { get; set; }
    }
}
