﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Planeventbackend.Models
{
    public class EventModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Local { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Date { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Type { get; set; }

        public int Userid { get; set; }

        public UserModel Users { get; set; }

        public ICollection<UserEventModel> UserEventModels { get; set; }
    }
}
