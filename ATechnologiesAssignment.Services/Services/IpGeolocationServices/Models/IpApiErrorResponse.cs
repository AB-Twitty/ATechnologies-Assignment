namespace ATechnologiesAssignment.Services.Services.IpGeolocationServices.Models
{
    public class IpApiErrorResponse
    {
        public bool Success { get; set; }

        public class ErrorDetails
        {
            public int Code { get; set; }
            public string Type { get; set; }
            public string Info { get; set; }
        }

        public ErrorDetails Error { get; set; } = new ErrorDetails();
    }
}
