using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Other.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Other.Model.Config
{
    public class SettingConfig : IEntityTypeConfiguration<SettingEntity>
    {
        public void Configure(EntityTypeBuilder<SettingEntity> builder)
        {
            builder.ToTable("T_Settings");
            builder.Property(e => e.Key).IsRequired().HasMaxLength(256);
            builder.Property(e => e.Value).IsRequired().HasMaxLength(256);
            builder.Property(e => e.KeyPari).IsRequired().HasMaxLength(256);
        }
    }
}
