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
    public class Associate
    {
        public string Email { get; set; }
        public int EventId { get; set; }
    }

    [ApiController]
    [Route("v1/userevent")]
    public class UserEventController : ControllerBase
    {
        // Associate user to event
        [HttpPost]
        [Route("associate")]
        public async Task<ActionResult<UserEventModel>>
        Post([FromServices] DataContext context, [FromBody] Associate model )
        {
            var user = await context.users.FirstOrDefaultAsync(
                x => x.Email == model.Email 
            );

            if (user != null)
            {
                var isAssociate = await context.userEvents.FirstOrDefaultAsync(x => 
                    x.Userid == user.Id && x.Eventid == model.EventId
                );

                if (isAssociate != null)
                {
                    return BadRequest(new
                    {
                        error = new
                        {
                            message = "Participante já adicionado ao evento"
                        }
                    });
                }

                var insert = new UserEventModel { Userid = user.Id, Eventid = model.EventId };
                context.userEvents.Add(insert);
                await context.SaveChangesAsync();

                return Ok(new
                {
                    success = new
                    {
                        message = "Participante adicionado ao evento"
                    }
                });
            }

            else
            {
                return BadRequest(new
                {
                    error = new
                    {
                        message = "Participante não encontrado"
                    }
                });
            }
        }

        // Get actually Events
        [HttpGet]
        [Route("actually/{id:int}")]
        public async Task<ActionResult<List<EventModel>>>
        Get([FromServices] DataContext context, int id)
        {
            var atual = DateTime.Now.Date;

            var query =
                await (from a in context.events
                       join b in context.userEvents on a.Id equals b.Eventid
                       join c in context.users on b.Userid equals c.Id
                       where c.Id == id
                       where a.Date <= atual
                       select a).OrderBy(x => x.Date).ToListAsync();

            return query;
        }

        // Get Next Events
        [HttpGet]
        [Route("next/{id:int}")]
        public async Task<ActionResult<List<EventModel>>>
        GetNextEvents([FromServices] DataContext context, int id)
        {
            var atual = DateTime.Now.Date;

            var query =
                await (from a in context.events
                       join b in context.userEvents on a.Id equals b.Eventid
                       join c in context.users on b.Userid equals c.Id
                       where c.Id == id
                       where a.Date >= atual
                       select a).ToListAsync();

            return query;
        }
    }
}
