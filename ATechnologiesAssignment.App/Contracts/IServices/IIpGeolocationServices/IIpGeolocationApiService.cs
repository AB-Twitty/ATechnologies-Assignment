namespace ATechnologiesAssignment.App.Contracts.IServices.IIpGeolocationServices
{
    public interface IIpGeolocationApiService
    {
        Task<dynamic> GetGeolocationByIpAsync(string ip);
    }
}
