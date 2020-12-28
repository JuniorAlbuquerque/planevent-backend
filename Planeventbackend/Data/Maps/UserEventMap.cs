using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planeventbackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planeventbackend.Data.Maps
{
    public class UserEventMap : IEntityTypeConfiguration<UserEventModel>
    {
        public void Configure(EntityTypeBuilder<UserEventModel> builder)
        {
            builder.HasKey(t => new { t.Userid, t.Eventid });
        }
    }
}
