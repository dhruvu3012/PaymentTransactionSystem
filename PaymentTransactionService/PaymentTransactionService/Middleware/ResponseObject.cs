namespace PaymentTransactionService.Middleware
{
    public class ResponseObject
    {
        public bool isError { get; set; }
        public object? result { get; set; }
        public int statusCode { get; set; }
        public string? message { get; set; }

        public static ResponseObject Success(object result, int statusCode, string message = "")
        {
            return new ResponseObject
            {
                isError = false,
                result = result,
                statusCode = statusCode,
                message = message
            };
        }

        public static ResponseObject Error(int statusCode, string message)
        {
            return new ResponseObject
            {
                isError = true,
                result = null,
                statusCode = statusCode,
                message = message
            };
        }
    }
}
