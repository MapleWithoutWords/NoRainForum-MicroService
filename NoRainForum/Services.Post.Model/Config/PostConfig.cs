using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Post.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Post.Model.Config
{
    public class PostConfig : IEntityTypeConfiguration<PostEntity>
    {
        public void Configure(EntityTypeBuilder<PostEntity> builder)
        {
            builder.ToTable("T_Posts");
            builder.Property(e => e.Content).IsRequired();
            builder.Property(e => e.Title).IsRequired().HasMaxLength(128);
            builder.HasOne(e => e.PostType).WithMany().HasForeignKey(e => e.PostTypeId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e => e.PostStatus).WithMany().HasForeignKey(e => e.PostStatusId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
