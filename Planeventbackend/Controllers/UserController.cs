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
        // Get All Users
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<UserModel>>>
        Get([FromServices] DataContext context)
        {
            var users = await context.users.ToListAsync();
            return users;
        }

        // Create User
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

        // Login User
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

        // Delete User
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult>
        Get([FromServices] DataContext context, int id)
        {
            var resevent = await context.events
               .AsNoTracking()
               .FirstOrDefaultAsync(x => x.Userid == id);

            context.Entry(resevent).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            var user = await context.users.FindAsync(id);
            context.Entry(user).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return Ok("Deletado");
        }

        // Update User
        [HttpPut]
        [Route("")]
        public async Task<ActionResult>
        Put([FromServices] DataContext context, [FromBody] UserModel model)
        {
            if(ModelState.IsValid)
            {
                var user = await context.users.FindAsync(model.Id);
                user.Name = model.Name;
                user.Email = model.Email;
                user.Password = model.Password;
                user.Birthdate = model.Birthdate;
                user.Sex = model.Sex;

                context.Entry(user).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return Ok("Atualizado com sucesso");
            } else
            {
                return BadRequest();
            }
        }
    }
}
