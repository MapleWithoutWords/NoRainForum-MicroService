using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Services.Admin.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Services.Admin.Model.Config;

namespace Services.Admin.Model
{
    public class AdminUserContext : DbContext
    {
        public AdminUserContext()
        {
            this.Database.EnsureCreated();
        }
        public DbSet<AdminUserEntity> AdminUsers { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        public DbSet<PermissionEntity> Permissions { get; set; }
        public DbSet<RolePermissionEntity> RolePermission { get; set; }
        public DbSet<AdminUserRoleEntity> AdminUserRoles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string conStr = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build().GetSection("conStr").Value;
            optionsBuilder.UseMySql(conStr);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AdminUserConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new PermissionConfig());
            modelBuilder.ApplyConfiguration(new RolePermissionConfig());
            modelBuilder.ApplyConfiguration(new AdminUserRoleConfig());
        }
    }
}
