using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Post.Model.Config;
using Services.Post.Model.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Services.Post.Model
{
    public class PostContext:DbContext
    {
        public PostContext()
        {
            this.Database.EnsureCreated();
        }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<PostCommentEntity> PostCommonts { get; set; }
        public DbSet<PostStatusEntity> PostStatuses { get; set; }
        public DbSet<PostTypeEntity> PostTypes { get; set; }
        public DbSet<PostCollectionEntity> PostCollections { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string conStr = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetSection("conStr").Value;
            optionsBuilder.UseMySql(conStr);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new PostConfig());
            modelBuilder.ApplyConfiguration(new PostCommontConfig());
            modelBuilder.ApplyConfiguration(new PostStatusConfig());
            modelBuilder.ApplyConfiguration(new PostTypeConfig());
            modelBuilder.ApplyConfiguration(new PostCollectionConfig());
        }
    }
}
