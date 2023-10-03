using System.Text.Json;

namespace YBS.Services.DataHandler.Responses
{
    public class APIException : Exception
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string ToJson ()
        {
            return JsonSerializer.Serialize(this);
        }
        public APIException(int StatusCode, string Message)
        {
            this.StatusCode = StatusCode;
            this.Message = Message;
        }
    }
}    
