using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planeventbackend.Data;
using Planeventbackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planeventbackend.Controllers
{
    [ApiController]
    [Route("v1/event")]
    public class EventController : ControllerBase
    {
        // Get All Events
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<EventModel>>>
        Get([FromServices] DataContext context)
        {
            var events = await context.events.ToListAsync();
            return events;
        }

        // Get Event By Id
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<EventModel>>
        Get([FromServices] DataContext context, int id)
        {
            var resevent = await context.events
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);

            return resevent;
        }

        // Get Event By User
        [HttpGet]
        [Route("user/{id:int}")]
        public async Task<ActionResult<List<EventModel>>>
        GetByUser([FromServices] DataContext context, int id)
        {
            var events = await context
                .events
                .AsNoTracking()
                .Where(x => x.Userid == id)
                .ToListAsync(); 

            return events;
        }

        // Create Event
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<EventModel>>
        Post([FromServices] DataContext context, [FromBody] EventModel model)
        {
            if (ModelState.IsValid)
            {
                context.events.Add(model);
                await context.SaveChangesAsync();
                return model;
            } else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
