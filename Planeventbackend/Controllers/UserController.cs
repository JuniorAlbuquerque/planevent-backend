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
    [Route("v1/user")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<UserModel>>>
        Get([FromServices] DataContext context)
        {
            var users = await context.users.ToListAsync();
            return users;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<UserModel>>
        Post([FromServices] DataContext context, [FromBody] UserModel model)
        {
            if(ModelState.IsValid)
            {
                context.users.Add(model);
                await context.SaveChangesAsync();
                return model;
            } else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<UserModel>>
        Post([FromServices] DataContext context, [FromBody] UserLogin model)
        {
            var users = await context.users.FirstOrDefaultAsync(
            u => u.Email == model.Email && u.Password == model.Password
                );

            if (users == null)
            {
                return BadRequest();
            }

            return users;
        }

    }
}
