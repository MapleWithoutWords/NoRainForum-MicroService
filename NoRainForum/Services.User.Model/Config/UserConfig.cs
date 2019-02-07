using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.User.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.User.Model.Config
{
    public class UserConfig : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("T_Users");
            builder.Property(e => e.Autograph).IsRequired().HasMaxLength(64);
            builder.Property(e => e.City).IsRequired().HasMaxLength(16);
            builder.Property(e => e.Email).IsRequired().HasMaxLength(32);
            builder.Property(e => e.HeadImgSrc).IsRequired().HasMaxLength(256);
            builder.Property(e => e.NickName).IsRequired().HasMaxLength(16);
            builder.Property(e => e.PasswordHash).IsRequired().HasMaxLength(256);
            builder.Property(e => e.Salt).IsRequired().HasMaxLength(64);
        }
    }
}
