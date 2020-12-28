using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Planeventbackend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Planeventbackend.Data.Maps
{
    public class UserMap : IEntityTypeConfiguration<UserModel>
    {
        public void Configure(EntityTypeBuilder<UserModel> builder)
        {
            builder.HasKey(t => t.Id);
            builder.Property(t => t.Name).HasMaxLength(150);
            builder.Property(t => t.Email).HasMaxLength(150).IsRequired();
            builder.Property(t => t.Password).HasMaxLength(150).IsRequired();
            builder.Property(t => t.Birthdate).HasMaxLength(150);
            builder.Property(t => t.Sex).HasMaxLength(150);
            builder.ToTable("Users");
        }
    }
}
