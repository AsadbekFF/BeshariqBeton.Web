using BeshariqBeton.Common.Entities;
using BeshariqBeton.Common.Enums;
using BeshariqBeton.Common.Security;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.DAL.Infrastructure
{
    public class MasterContext : DbContext, IDataProtectionKeyContext
    {
        public DbSet<StandardPermission> StandardPermissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserStandardPermission> UsersStandardPermissions { get; set; }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; }

        public DbSet<DefaultParameter> DefaultParameters { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Storage> Storages { get; set; }
        public DbSet<Sale> Sales { get; set; }

        public MasterContext(DbContextOptions<MasterContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<UserStandardPermission>().HasKey(u => new { u.UserId, u.StandardPermissionId });
            modelBuilder.Entity<StandardPermission>().HasIndex(s => s.SystemName).IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public void Initialize()
        {
            // Users
            if (!Users.Any())
            {
                Users.Add(new User
                {
                    Username = "Muhammadjon25",
                    Password = "D#Fn{p6L",
                    Role = UserRole.Admin,
                    Created = DateTime.UtcNow,
                    UpdatedDateTime = DateTime.UtcNow,
                    PhoneNumber = "+998900644474"
                });
                SaveChanges();
            }

            #region Standard Permissions

            var standardPermissions = StandardPermissionsProvider.GetAllPermissions();
            var userDefaults = StandardPermissionsProvider.GetRoleDefaultPermissions()
                .Single(x => x.Role == UserRole.User);
            var administratorDefaults = StandardPermissionsProvider.GetRoleDefaultPermissions()
                .Single(x => x.Role == UserRole.Admin);
            var addedPermissions = new List<StandardPermission>();

            foreach (var standardPermission in standardPermissions)
            {
                var exists = StandardPermissions.Any(x => x.SystemName == standardPermission.SystemName);

                if (exists)
                    continue;

                var newStandardPermission = new StandardPermission()
                {
                    Name = standardPermission.Name,
                    SystemName = standardPermission.SystemName
                };

                StandardPermissions.Add(newStandardPermission);
                addedPermissions.Add(newStandardPermission);
            }

            if (addedPermissions.Any())
            {
                SaveChanges();

                // assign role defaults

                var sql = @"
INSERT INTO UsersStandardPermissions(UserId, StandardPermissionId)
SELECT Id, @p0
FROM Users
WHERE [Role] = @p1";

                foreach (var permission in addedPermissions)
                {
                    if (userDefaults.Permissions.Any(x => x.SystemName == permission.SystemName))
                    {
                        Database.ExecuteSqlRaw(sql, permission.Id, UserRole.User);
                    }

                    if (administratorDefaults.Permissions.Any(x => x.SystemName == permission.SystemName))
                    {
                        Database.ExecuteSqlRaw(sql, permission.Id, UserRole.Admin);
                    }
                }
            }

            #endregion
        }
    }
}
