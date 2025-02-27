using ATechnologiesAssignment.Services.Services.IpGeolocationServices.Models;

namespace ATechnologiesAssignment.Services.Services.IpGeolocationServices.Exceptions
{
    public class IpApiErrorException : Exception
    {
        public IpApiErrorResponse ErrorResponse { get; set; }

        public IpApiErrorException(IpApiErrorResponse error)
        {
            ErrorResponse = error;
        }
    }
}
