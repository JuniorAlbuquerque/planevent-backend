using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Planeventbackend.Data;
using Planeventbackend.Models;
using Planeventbackend.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planeventbackend.Controllers
{
    public class Filter
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }

    }

    [ApiController]
    [Route("v1/event")]
    public class EventController : ControllerBase
    {
        public Message message = new Message();

        // Get All Events
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<EventModel>>>
        GetAll([FromServices] DataContext context)
        {
            var events = await context.Events.OrderBy(x => x.Date).ToListAsync();
            return events;
        }

        // Get Event By Id
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<EventModel>>
        GetById([FromServices] DataContext context, int id)
        {
            var resevent = await context.Events
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
                .Events
                .Where(x => x.Userid == id)
                .ToListAsync(); 

            return events;
        }

        // Search events by Name and Date
        [HttpGet]
        [Route("filter")]
        public async Task<ActionResult<List<EventModel>>>
        GetByName([FromServices] DataContext context, [FromQuery] Filter model)
        {
            if (model.Name != null)
            {
                var events = await context.Events.OrderBy(x => x.Date).Where(x => x.Name.ToUpper().Contains(model.Name.ToUpper())).ToListAsync();

                return events;
            }
            if (model.Date != null)
            {
                var events = await context.Events.OrderBy(x => x.Date).Where(x => x.Date == model.Date).ToListAsync();

                return events;
            }

            return BadRequest();
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
                    var query = await context.Events.FirstOrDefaultAsync(
                        x => x.Date == model.Date && x.Type == "Exclusivo"  
                    );

                    if (query != null)
                    {
                        return BadRequest(message.GetMessage("Error", "Evento exclusivo na mesma data já cadastrado"));
                    } 
                    else
                    {
                        context.Events.Add(model);
                        await context.SaveChangesAsync();
                        return model;
                    }
                } else
                {
                    context.Events.Add(model);
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
            var resevent = await context.Events.FindAsync(id);
            context.Entry(resevent).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return Ok(message.GetMessage("Success", "Evento excluido"));
        }

        // Update Event
        [HttpPut]
        [Route("")]
        public async Task<ActionResult>
        Put([FromServices] DataContext context, [FromBody] EventModel model)
        {
            if (ModelState.IsValid)
            {
                var ev = await context.Events.FindAsync(model.Id);
                ev.Name = model.Name;
                ev.Description = model.Description;
                ev.Date = model.Date;
                ev.Local = model.Local;
                ev.Type = model.Type;

                context.Entry(ev).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return Ok(message.GetMessage("Success", "Dados atualizados com sucesso"));
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
