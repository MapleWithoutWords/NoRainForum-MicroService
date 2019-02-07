using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Services.Admin.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Admin.Model.Config
{
    public class AdminUserRoleConfig : IEntityTypeConfiguration<AdminUserRoleEntity>
    {
        public void Configure(EntityTypeBuilder<AdminUserRoleEntity> builder)
        {
            builder.ToTable("T_AdminUserRoles");
            builder.HasOne(e => e.AdminUser).WithMany().HasForeignKey(e => e.AdminUserId).OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e => e.Role).WithMany().HasForeignKey(e => e.RoleId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
