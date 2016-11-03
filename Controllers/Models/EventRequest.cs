using System.Dynamic;

namespace WebApplication.Controllers.Models
{
    public class EventRequest
    {
        public string Type { get; set; }

        public ExpandoObject Data { get; set; }
    }
}