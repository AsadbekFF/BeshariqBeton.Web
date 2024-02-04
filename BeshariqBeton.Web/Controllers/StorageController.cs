using BeshariqBeton.BLL.Base;
using BeshariqBeton.BLL.Services;
using BeshariqBeton.Common.Entities;
using BeshariqBeton.Common.Models.Parameters;
using BeshariqBeton.Common.Security;
using BeshariqBeton.DAL.Infrastructure;
using BeshariqBeton.Web.Infrastructure;
using BeshariqBeton.Web.Infrastructure.Authentication;
using BeshariqBeton.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeshariqBeton.Web.Controllers
{
    [Authorize]
    [PermissionAuthorize(StandardPermissionNames.ManageStorage)]
    public class StorageController : BaseController
    {
        private readonly DefaultParametersService _defaultParametersService;
        private readonly IMapper _mapper;

        public StorageController(Common.Logging.ILogger logger, DefaultParametersService defaultParametersService, IMapper mapper) : base(logger)
        {
            _defaultParametersService = defaultParametersService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new StorageViewModel();
            try
            {
                var storageParameters = await _defaultParametersService.GetStorageParametersAsync();

                storageParameters.CementRemainKg = Math.Round(storageParameters.CementRemainKg, 2);
                storageParameters.SandRemainM3 = Math.Round(storageParameters.SandRemainM3, 2);
                storageParameters.ShebenRemainM3 = Math.Round(storageParameters.ShebenRemainM3, 2);
                storageParameters.ChemicalRemainKg = Math.Round(storageParameters.ChemicalRemainKg, 2);

                model = _mapper.Map<StorageParameters, StorageViewModel>(storageParameters);
            }
            catch (Exception ex)
            {
                Logger.Error("Error while getting storage parameters");
                var result = HandleException(ex);
                if (result != null)
                {
                    return result;
                }
            }
            

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(double? cementWeight, double? sandVolume, double? shebenVolume, double? chemicalWeight)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var storageParameters = await _defaultParametersService.GetStorageParametersAsync();
                    var entity = new StorageParameters
                    {
                        CementRemainKg = cementWeight ?? 0,
                        SandRemainM3 = sandVolume ?? 0,
                        ChemicalRemainKg = chemicalWeight ?? 0,
                        ShebenRemainM3 = shebenVolume ?? 0,
                    };

                    storageParameters.CementRemainKg += entity.CementRemainKg;
                    storageParameters.SandRemainM3 += entity.SandRemainM3;
                    storageParameters.ShebenRemainM3 += entity.ShebenRemainM3;
                    storageParameters.ChemicalRemainKg += entity.ChemicalRemainKg;

                    await _defaultParametersService.SaveParametersAsync(storageParameters);
                    ShowSuccessMessage();
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while getting storage parameters");
                    var result = HandleException(ex);
                    if (result != null)
                    {
                        return result;
                    }
                }
            }

            return RedirectToAction("Index");
        }
    }
}
