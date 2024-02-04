using BeshariqBeton.Common.Entities;
using BeshariqBeton.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.Common.Security
{
    public class StandardPermissionsProvider
    {
        public static readonly StandardPermission ManageUsers = new()
        {
            Name = "Foydalanuvchilarni boshqarish",
            SystemName = "ManageUsers"
        };

        public static readonly StandardPermission ManageSettings = new()
        {
            Name = "Sozlamalar",
            SystemName = "ManageSettings"
        };

        public static readonly StandardPermission ManageSales = new()
        {
            Name = "Sotuvni boshqarish",
            SystemName = "ManageSales"
        };

        public static readonly StandardPermission ManageStorage = new()
        {
            Name = "Skladni boshqarish",
            SystemName = "ManageStorage"
        };

        public static readonly StandardPermission ManageClients = new()
        {
            Name = "Klientlarni boshqarish",
            SystemName = "ManageClients"
        };

        public static List<StandardPermission> GetAllPermissions()
        {
            return new List<StandardPermission>
            {
                ManageUsers,
                ManageSettings,
                ManageSales,
                ManageStorage,
                ManageClients
            };
        }

        public static List<RoleDefaultPermission> GetRoleDefaultPermissions()
        {
            return new List<RoleDefaultPermission>
            {
                new() {
                    Role = UserRole.Admin,
                    Permissions = new List<StandardPermission>
                    {
                        ManageUsers,
                        ManageSettings,
                        ManageSales,
                        ManageStorage,
                        ManageClients
                    }
                },

                new() {
                    Role = UserRole.User,
                    Permissions = new List<StandardPermission>
                    {
                    }
                }
            };
        }
    }
}
