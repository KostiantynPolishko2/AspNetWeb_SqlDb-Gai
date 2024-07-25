using System.ComponentModel.DataAnnotations.Schema;

namespace _20240723_SqlDb_Gai.Models
{
    [NotMapped]
    public class StatusCode
    {
        public int code { get; }
        public string? message { get; }

        public StatusCode(): this(102, "server executes the request") { }
        public StatusCode(int code, string message) { 
            this.code = code;
            this.message = message;
        }
    }
}
