using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Controllers.Models;
using WebApplication.Interfaces;

namespace WebApplication.Controllers
{
    [Route("v1/Events")]
    public class EventController : Controller
    {
        public IEnumerable<IProjection> Projections { get; set; }

        public EventController (IEnumerable<IProjection> projections)
        {
          Projections = projections;
        }

        [HttpPut]
        public IActionResult PutEvent([FromBody]EventRequest eventRequest)
        {
            var candidates = Projections.Where(p => p.CanExecute(eventRequest.Type));
            var newEvent = new WebApplication.Models.Event
            {
                Type = eventRequest.Type,
                Data = eventRequest.Data
            };

            var tasks = candidates.Select(c => c.ExecuteAsync(newEvent)).ToArray();
            Task.WaitAll(tasks);
            return Ok();
        }
    }
}
