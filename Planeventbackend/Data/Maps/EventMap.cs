using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planeventbackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planeventbackend.Data.Maps
{
    public class EventMap : IEntityTypeConfiguration<EventModel>
    {
        public void Configure(EntityTypeBuilder<EventModel> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Userid).HasColumnName("Userid");
            builder.Property(t => t.Name).HasMaxLength(150);
            builder.Property(t => t.Description).HasMaxLength(150).IsRequired();
            builder.Property(t => t.Date).HasMaxLength(150).IsRequired();
            builder.Property(t => t.Local).HasMaxLength(150);
            builder.Property(t => t.Type).HasMaxLength(150);
            builder.ToTable("Events");
        }
    }
}
