using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Admin.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Admin.Model.Config
{
    public class PermissionConfig : IEntityTypeConfiguration<PermissionEntity>
    {
        public void Configure(EntityTypeBuilder<PermissionEntity> builder)
        {
            builder.ToTable("T_Permissions");
            builder.Property(e => e.Description).IsRequired().HasMaxLength(64);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(32);
        }
    }
}
