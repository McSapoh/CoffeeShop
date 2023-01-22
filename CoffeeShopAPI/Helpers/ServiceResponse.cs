namespace CoffeeShopAPI.Helpers
{
    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
        public object Data { get; set; }

        public ServiceResponse(bool success, string message, int status)
        {
            Success = success;
            Message = message;
            Status = status;
        }
        public ServiceResponse(int status, object data)
        {
            Status = status;
            Data = data;
        }
    }
}
