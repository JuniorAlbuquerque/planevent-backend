using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Planeventbackend.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Password { get; set; }
    }
}
