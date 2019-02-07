using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Post.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Post.Model.Config
{
    public class PostStatusConfig : IEntityTypeConfiguration<PostStatusEntity>
    {
        public void Configure(EntityTypeBuilder<PostStatusEntity> builder)
        {
            builder.ToTable("T_PostStatuses");
            builder.Property(e => e.Description).IsRequired().HasMaxLength(64);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(32);
        }
    }
}
