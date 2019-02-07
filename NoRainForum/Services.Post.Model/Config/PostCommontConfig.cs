using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Post.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Post.Model.Config
{
    public class PostCommontConfig : IEntityTypeConfiguration<PostCommentEntity>
    {
        public void Configure(EntityTypeBuilder<PostCommentEntity> builder)
        {
            builder.ToTable("T_PostCommonts");
            builder.Property(e => e.Content).IsRequired();
            builder.HasOne(e => e.Post).WithMany().HasForeignKey(e => e.PostId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
