using BeshariqBeton.BLL.Base;
using BeshariqBeton.Common.Entities;
using BeshariqBeton.DAL.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.BLL.Services
{
    public class ClientService : BaseService<Client, int, MasterContext>
    {
        public ClientService(MasterContext context) : base(context)
        {
        }

        protected override async Task<IEnumerable<ValidationResult>> ValidateAsync(Client entity)
        {
            var errors = new List<ValidationResult>();

            await CheckDuplicatesAsync(entity, errors, nameof(Client.Name), "Bunaqa ism tizimda bor.", c => c.Name == entity.Name);

            return errors;
        }
    }
}
