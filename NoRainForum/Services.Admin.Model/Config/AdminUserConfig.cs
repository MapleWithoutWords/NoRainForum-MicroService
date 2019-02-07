using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Admin.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Admin.Model.Config
{
    public class AdminUserConfig : IEntityTypeConfiguration<AdminUserEntity>
    {
        public void Configure(EntityTypeBuilder<AdminUserEntity> builder)
        {
            builder.ToTable("T_AdminUsers");
            builder.Property(e => e.Age).HasMaxLength(256);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(16);
            builder.Property(e => e.PasswordHash).IsRequired().HasMaxLength(128);
            builder.Property(e => e.PhoneNum).IsRequired().HasMaxLength(16);
            builder.Property(e => e.Salt).IsRequired().HasMaxLength(64);
        }
    }
}
