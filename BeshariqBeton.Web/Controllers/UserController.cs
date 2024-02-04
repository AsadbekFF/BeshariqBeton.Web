using BeshariqBeton.BLL.Base;
using BeshariqBeton.BLL.Services;
using BeshariqBeton.Common.Entities;
using BeshariqBeton.Common.Enums;
using BeshariqBeton.Common.Security;
using BeshariqBeton.DAL.Infrastructure;
using BeshariqBeton.Web.Infrastructure;
using BeshariqBeton.Web.Infrastructure.Authentication;
using BeshariqBeton.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ILogger = BeshariqBeton.Common.Logging.ILogger;

namespace BeshariqBeton.Web.Controllers
{
    [PermissionAuthorize(StandardPermissionNames.ManageUsers)]
    public class UserController : CrudController<User, UserViewModel, int, MasterContext>
    {
        private readonly UserService _userService;
        private readonly PermissionService _permissionService;

        public UserController(ILogger logger, IMapper mapper, UserService service, PermissionService permissionService) : base(logger, mapper, service)
        {
            _userService = service;
            _permissionService = permissionService;
        }

        public override IActionResult Add()
        {
            var viewModel = PrepareUserForViewModel(null).Result;
            return View("AddEdit", viewModel);
        }

        public override async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.GetByIdNotTrackingAsync(id);
            var viewModel = await PrepareUserForViewModel(user);
            return View("AddEdit", viewModel);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("log-in")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("log-in")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userService.AuthenticateAsync(model.Username, model.Password);
                    await HttpContext.AuthenticateUser(user, model.RememberMe, default);
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while getting security parameters:", ex);
                    var result = HandleException(ex);
                    if (result != null)
                        return result;
                }
            }

            if (!string.IsNullOrWhiteSpace(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        protected override Task AfterMappingToEntity(User entity, UserViewModel viewModel)
        {
            entity.StandardPermissions.Clear();

            var allowedPermissions = viewModel.StandardPermissions.Where(x => x.Allow);
            entity.StandardPermissions.AddRange(allowedPermissions.Select(x => new UserStandardPermission
            {
                StandardPermissionId = x.Id
            }));

            return base.AfterMappingToEntity(entity, viewModel);
        }

        [HttpGet("log-out")]
        [AllowAnonymous]
        public async Task<IActionResult> LogOut()
        {
            if (User.IsAuthenticated())
                await HttpContext.SignOutUserAsync();

            return RedirectToAction("LogIn", "User");
        }

        private async Task<UserViewModel> PrepareUserForViewModel(User user)
        {
            var allPermissions = await _permissionService.GetStandardPermissions();
            var viewModel = user != null ? Mapper.Map<User, UserViewModel>(user) : new UserViewModel();

            var userPermissionIds = (await _userService.GetByIdNotTrackingAsync(User.GetUserId())).StandardPermissions.Select(p => p.StandardPermissionId);
            allPermissions = allPermissions.Where(p => userPermissionIds.Contains(p.Id)).ToList();

            IEnumerable<string> userPermissions;

            if (user == null)
            {
                userPermissions = StandardPermissionsProvider
                        .GetRoleDefaultPermissions()
                        .First(x => x.Role == UserRole.User).Permissions
                        .Select(x => x.Name);
            }
            else
            {
                userPermissions = user.StandardPermissions.Select(x => x.StandardPermission.Name);
            }

            viewModel.StandardPermissions = allPermissions.Select(x => new StandardPermissionViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Allow = userPermissions.Contains(x.Name)
            }).ToList();

            return viewModel;
        }
    }
}
