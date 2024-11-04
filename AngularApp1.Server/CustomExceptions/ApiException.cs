namespace AngularApp1.Server.CustomExceptions
{
    public class ApiException:Exception
    {
        public int StatusCode { get; }
        public ApiException(int statusCode,string message):base(message) {
            this.StatusCode = statusCode;
        }
        public override string ToString() { 
            return $"{StatusCode}:{Message}";
        }
    }
}
