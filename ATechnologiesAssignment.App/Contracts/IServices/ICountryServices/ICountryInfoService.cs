namespace ATechnologiesAssignment.App.Contracts.IServices.ICountryServices
{
    public interface ICountryInfoService
    {
        Task<string> GetCountryNameByCodeAsync(string countryCode);
    }
}
