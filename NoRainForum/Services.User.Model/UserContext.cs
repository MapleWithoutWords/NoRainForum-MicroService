using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.User.Model.Config;
using Services.User.Model.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Services.User.Model
{
    public class UserContext:DbContext
    {
        public UserContext()
        {
            this.Database.EnsureCreated();
        }
        public DbSet<UserEntity> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string conStr = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetSection("conStr").Value;
            optionsBuilder.UseMySql(conStr);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfig());
        }
    }
}
