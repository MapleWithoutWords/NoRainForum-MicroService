using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Post.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Post.Model.Config
{
    public class PostCollectionConfig : IEntityTypeConfiguration<PostCollectionEntity>
    {
        public void Configure(EntityTypeBuilder<PostCollectionEntity> builder)
        {
            builder.ToTable("T_PostCollections");
            builder.HasOne(e => e.Post).WithMany().HasForeignKey(e => e.PostId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
