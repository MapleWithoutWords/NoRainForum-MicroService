using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Post.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Post.Model.Config
{
    public class PostTypeConfig : IEntityTypeConfiguration<PostTypeEntity>
    {
        public void Configure(EntityTypeBuilder<PostTypeEntity> builder)
        {
            builder.ToTable("T_PostTypes");
            builder.Property(e => e.Description).IsRequired().HasMaxLength(64);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(32);
        }
    }
}
