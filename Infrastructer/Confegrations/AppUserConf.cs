global using Core.Models;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using System;
global using System.Collections.Generic;
global using System.Text;

namespace Infrastructer.Confegrations
{
    public class AppUserConf : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.UserName).IsUnique();
            builder.Property(x => x.RefreshToken).IsRequired(false);
            builder.Property(x => x.RefreshTokenExpirationTime);
            builder.Property(x => x.UserName).HasMaxLength(50).IsRequired(true);
            builder.Property(x => x.Email).HasMaxLength(50).IsRequired(true);
            builder.Property(x => x.PhoneNumber).HasMaxLength(20).IsRequired(false);
            builder.Property(x => x.FullName).HasMaxLength(100).IsRequired(true);
        }

    }
}
