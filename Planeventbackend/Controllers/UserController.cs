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

        // Get User By Id
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<UserModel>>
        GetById([FromServices] DataContext context, int id)
        {
            var user = await context.users.FindAsync(id);
            return user;
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
            var user = await context.users.FirstOrDefaultAsync(
            u => u.Email == model.Email && u.Password == model.Password
                );

            if (user == null)
            {
                return BadRequest();
            }

            return user;
        }

        // Delete User
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<ActionResult>
        Get([FromServices] DataContext context, int id)
        {
            var user = await context.users.FindAsync(id);
            context.Entry(user).State = EntityState.Deleted;
            await context.SaveChangesAsync();

            return Ok(new
            {
                success = new
                {
                    message = "Usuário excluido"
                }
            });
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

                return Ok(new
                {
                    success = new
                    {
                        message = "Dados atualizados com sucesso"
                    }
                });
            } else
            {
                return BadRequest();
            }
        }
    }
}
