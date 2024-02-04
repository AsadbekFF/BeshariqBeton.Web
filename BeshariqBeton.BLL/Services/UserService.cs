using BeshariqBeton.BLL.Base;
using BeshariqBeton.Common.Entities;
using BeshariqBeton.Common.Enums;
using BeshariqBeton.Common.Exceptions;
using BeshariqBeton.DAL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.BLL.Services
{
    public class UserService : BaseService<User, int, MasterContext>
    {
        public UserService(MasterContext context) : base(context)
        {
        }

        public override async Task<User> GetByIdNotTrackingAsync(int id)
        {
            var user = await GetByIdNotTrackingAsync(id, new string[] { nameof(User.StandardPermissions) });

            if (user != null)
            {
                foreach(var userStandardPermission in user.StandardPermissions)
                {
                    userStandardPermission.StandardPermission = Context.StandardPermissions.FirstOrDefault(p => p.Id == userStandardPermission.StandardPermissionId);
                }
            }

            return user;
        }

        protected override async Task<IEnumerable<ValidationResult>> ValidateAsync(User user)
        {
            var result = new List<ValidationResult>();

            // Check duplicate username
            await CheckDuplicatesAsync(user, result, nameof(User.Username), "User with this username already exists.",
                u => u.Username == user.Username);

            return result;
        }

        public async Task<DateTime?> GetLastUpdatedTimeAsync(int id)
        {
            return await Context.Users.Where(u => u.Id == id).Select(u => u.UpdatedDateTime).FirstOrDefaultAsync();
        }

        public override async Task UpdateAsync(User entity)
        {
            await DoValidation(entity);
            var dbUser = await GetByIdAsync(entity.Id);

            if (dbUser != null)
            {
                var permissions = await Context.UsersStandardPermissions.Where(u => u.UserId == dbUser.Id).ToListAsync();
                Context.UsersStandardPermissions.RemoveRange(permissions);

                dbUser.Username = entity.Username;
                dbUser.FirstName = entity.FirstName;
                dbUser.Lastname = entity.Lastname;
                dbUser.Password = entity.Password;
                dbUser.PhoneNumber = entity.PhoneNumber;
                dbUser.StandardPermissions = entity.StandardPermissions;
            }

            await Context.SaveChangesAsync();
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            if (!(await Context.Users.AnyAsync(u => u.Username == username && u.Password == password)))
            {
                throw new ForbiddenException();
            }

            return await Context.Users.Include(u => u.StandardPermissions).ThenInclude(u => u.StandardPermission).FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
