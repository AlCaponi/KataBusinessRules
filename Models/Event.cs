using System.Dynamic;
namespace WebApplication.Models
{
    public class Event
    {
        public string Type { get; set; }
        public ExpandoObject Data { get; set; }
    }
}