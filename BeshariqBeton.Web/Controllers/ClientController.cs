using BeshariqBeton.BLL.Base;
using BeshariqBeton.BLL.Services;
using BeshariqBeton.Common.Entities;
using BeshariqBeton.Common.Security;
using BeshariqBeton.DAL.Infrastructure;
using BeshariqBeton.Web.Infrastructure;
using BeshariqBeton.Web.Infrastructure.Authentication;
using BeshariqBeton.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BeshariqBeton.Web.Controllers
{
    [PermissionAuthorize(StandardPermissionNames.ManageClients)]
    public class ClientController : CrudController<Client, ClientViewModel, int, MasterContext>
    {
        public ClientController(Common.Logging.ILogger logger, IMapper mapper, ClientService service) : base(logger, mapper, service)
        {
        }
    }
}
