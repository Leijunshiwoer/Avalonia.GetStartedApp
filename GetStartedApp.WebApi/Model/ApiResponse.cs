namespace GetStartedApp.WebApi.Model
{
    public class ApiResponse
    {
        public bool Status { get; set; }

        public string Message { get; set; } = string.Empty;

        public object? Data { get; set; }

        public static ApiResponse Success(object? data = null, string message = "操作成功")
        {
            return new ApiResponse
            {
                Status = true,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse Fail(string message = "操作失败", object? data = null)
        {
            return new ApiResponse
            {
                Status = false,
                Message = message,
                Data = data
            };
        }
    }
}
