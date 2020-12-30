using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Planeventbackend.Models
{
    public class UserEventModel
    {
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public int Userid { get; set; }

        public UserModel User { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public int Eventid { get; set; }

        public EventModel Event { get; set; }
    }
}
