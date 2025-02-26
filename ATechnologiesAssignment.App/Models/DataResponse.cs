namespace ATechnologiesAssignment.App.Models
{
    public class DataResponse<TData> : BaseResponse
    {
        public TData Data { get; set; }
    }
}
