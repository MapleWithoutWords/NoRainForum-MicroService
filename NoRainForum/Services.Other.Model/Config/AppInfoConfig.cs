using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Other.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Other.Model.Config
{
    public class AppInfoConfig : IEntityTypeConfiguration<AppInfoEntity>
    {
        public void Configure(EntityTypeBuilder<AppInfoEntity> builder)
        {
            builder.ToTable("T_AppInfos");
            builder.Property(e => e.AppKey).IsRequired().HasMaxLength(128);
            builder.Property(e => e.AppSecret).IsRequired().HasMaxLength(128);
            builder.Property(e => e.Email).IsRequired().HasMaxLength(32);
        }
    }
}
