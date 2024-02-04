using BeshariqBeton.BLL.Base;
using BeshariqBeton.Common.Entities;
using BeshariqBeton.DAL.Infrastructure;
using System;
using System.Collections.Generic;
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
    }
}
