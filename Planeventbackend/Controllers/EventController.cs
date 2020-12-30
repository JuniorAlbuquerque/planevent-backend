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
            var events = await context.events.OrderBy(x => x.Date).ToListAsync();
            return events;
        }

        // Get Event By Id
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<EventModel>>
        GetById([FromServices] DataContext context, int id)
        {
            var resevent = await context.events
                .AsNoTracking()
                .Include(x => x.UserEventModels)
                .ThenInclude(t => t.User)
                .FirstOrDefaultAsync(x => x.Id == id);

            return resevent;
        }

        // Get Event Created By User
        [HttpGet]
        [Route("user/{id:int}")]
        public async Task<ActionResult<List<EventModel>>>
        GetByUser([FromServices] DataContext context, int id)
        {
            var events = await context
                .events
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
                if (model.Type == "Exclusivo")
                {
                    var query = await context.events.FirstOrDefaultAsync(
                        x => x.Date == model.Date && x.Type == "Exclusivo"  
                    );

                    if (query != null)
                    {
                        return BadRequest("Evento exclusivo na mesma data");
                    } else
                    {
                        context.events.Add(model);
                        await context.SaveChangesAsync();
                        return model;
                    }
                } else
                {
                    context.events.Add(model);
                    await context.SaveChangesAsync();
                    return model;
                }
                
            } else
            {
                return BadRequest(ModelState);
            }
        }

        // Delete Event
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult>
        GetDelete([FromServices] DataContext context, int id)
        {
            var resevent = await context.events.FindAsync(id);
            context.Entry(resevent).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return Ok(new
            {
                success = new
                {
                    message = "Evento excluido"
                }
            });
        }

        // Update Event
        [HttpPut]
        [Route("")]
        public async Task<ActionResult>
        Put([FromServices] DataContext context, [FromBody] EventModel model)
        {
            if (ModelState.IsValid)
            {
                var ev = await context.events.FindAsync(model.Id);
                ev.Name = model.Name;
                ev.Description = model.Description;
                ev.Date = model.Date;
                ev.Local = model.Local;
                ev.Type = model.Type;

                context.Entry(ev).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return Ok(new
                {
                    success = new
                    {
                        message = "Dados atualizados com sucesso"
                    }
                });
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
