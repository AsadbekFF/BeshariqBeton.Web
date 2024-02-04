using BeshariqBeton.Common.Entities;
using BeshariqBeton.Common.Security;
using BeshariqBeton.DAL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.BLL.Services
{
    public class PermissionService
    {
        private readonly MasterContext _masterContext;

        public PermissionService(MasterContext masterContext)
        {
            _masterContext = masterContext;
        }

        public async Task<List<StandardPermission>> GetStandardPermissions()
        {
            var standardPermissions = await _masterContext.StandardPermissions.ToListAsync();

            // Sort the permissions from db based on the static list of permissions
            // from StandardPermissionsProvider so that they appear in a set order
            var sortedPermissions = StandardPermissionsProvider.GetAllPermissions()
                .Join(standardPermissions,
                    staticPermission => staticPermission.SystemName,
                    dbPermission => dbPermission.SystemName,
                    (staticPermission, dbPermission) => dbPermission);

            return sortedPermissions.ToList();
        }
    }
}
