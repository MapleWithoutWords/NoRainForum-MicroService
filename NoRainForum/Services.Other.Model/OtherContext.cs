using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Other.Model.Config;
using Services.Other.Model.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Services.Other.Model
{
    public class OtherContext : DbContext
    {
        public OtherContext()
        {
            this.Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string conStr = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetSection("conStr").Value;
            optionsBuilder.UseMySql(conStr);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AppInfoConfig());
            modelBuilder.ApplyConfiguration(new SettingConfig());
        }
        public DbSet<AppInfoEntity> AppInfos { get; set; }
        public DbSet<SettingEntity> Settings { get; set; }
    }
}
