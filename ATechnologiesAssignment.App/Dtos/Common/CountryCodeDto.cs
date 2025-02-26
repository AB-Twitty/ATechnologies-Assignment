namespace ATechnologiesAssignment.App.Dtos.Common
{
    public class CountryCodeDto
    {
        private string _countryCode;

        public required string CountryCode
        {
            get => _countryCode;
            set => _countryCode = value.ToUpper();
        }
    }
}
