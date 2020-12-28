using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planeventbackend.Models
{
    public class UserEventModel
    {
        public int Userid { get; set; }

        public UserModel User { get; set; }

        public int Eventid { get; set; }

        public EventModel Event { get; set; }
    }
}
