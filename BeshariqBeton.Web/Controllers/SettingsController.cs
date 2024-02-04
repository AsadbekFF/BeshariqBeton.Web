using BeshariqBeton.BLL.Services;
using BeshariqBeton.Common.Models.Parameters;
using BeshariqBeton.Common.Security;
using BeshariqBeton.Web.Infrastructure;
using BeshariqBeton.Web.Infrastructure.Authentication;
using BeshariqBeton.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeshariqBeton.Web.Controllers
{
    [Authorize]
    [PermissionAuthorize(StandardPermissionNames.ManageSettings)]
    public class SettingsController : BaseController
    {
        private readonly DefaultParametersService _defaultParametersService;
        private readonly IMapper _mapper;

        public SettingsController(Common.Logging.ILogger logger, DefaultParametersService defaultParametersService, IMapper mapper) : base(logger)
        {
            _defaultParametersService = defaultParametersService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new PriceSettingsViewModel();

            try
            {
                var concreteConsistancesParameters = await _defaultParametersService.GetConcreteConsistancesParametersAsync();

                model.СementWeightKg100 = concreteConsistancesParameters.СementWeightKg100;
                model.SandVolume100 = concreteConsistancesParameters.SandVolume100;
                model.ShabenVolume100 = concreteConsistancesParameters.ShabenVolume100;
                model.СhemicalKg100 = concreteConsistancesParameters.СhemicalKg100;
                model.СementWeightKg150 = concreteConsistancesParameters.СementWeightKg150;
                model.SandVolume150 = concreteConsistancesParameters.SandVolume150;
                model.ShabenVolume150 = concreteConsistancesParameters.ShabenVolume150;
                model.СhemicalKg150 = concreteConsistancesParameters.СhemicalKg150;
                model.СementWeightKg200 = concreteConsistancesParameters.СementWeightKg200;
                model.SandVolume200 = concreteConsistancesParameters.SandVolume200;
                model.ShabenVolume200 = concreteConsistancesParameters.ShabenVolume200;
                model.СhemicalKg200 = concreteConsistancesParameters.СhemicalKg200;
                model.СementWeightKg250 = concreteConsistancesParameters.СementWeightKg250;
                model.SandVolume250 = concreteConsistancesParameters.SandVolume250;
                model.ShabenVolume250 = concreteConsistancesParameters.ShabenVolume250;
                model.СhemicalKg250 = concreteConsistancesParameters.СhemicalKg250;
                model.СementWeightKg300 = concreteConsistancesParameters.СementWeightKg300;
                model.SandVolume300 = concreteConsistancesParameters.SandVolume300;
                model.ShabenVolume300 = concreteConsistancesParameters.ShabenVolume300;
                model.СhemicalKg300 = concreteConsistancesParameters.СhemicalKg300;
                model.СementWeightKg350 = concreteConsistancesParameters.СementWeightKg350;
                model.SandVolume350 = concreteConsistancesParameters.SandVolume350;
                model.ShabenVolume350 = concreteConsistancesParameters.ShabenVolume350;
                model.СhemicalKg350 = concreteConsistancesParameters.СhemicalKg350;
                model.СementWeightKg400 = concreteConsistancesParameters.СementWeightKg400;
                model.SandVolume400 = concreteConsistancesParameters.SandVolume400;
                model.ShabenVolume400 = concreteConsistancesParameters.ShabenVolume400;
                model.СhemicalKg400 = concreteConsistancesParameters.СhemicalKg400;

                var concreteConsistancesPricesParameters = await _defaultParametersService.GetConcreteConsistancesPricesParametersAsync();

                model.СementPriceKg = concreteConsistancesPricesParameters.СementPriceKg;
                model.SandPriceM3 = concreteConsistancesPricesParameters.SandPriceM3;
                model.ShabenPriceM3 = concreteConsistancesPricesParameters.ShabenPriceM3;
                model.СhemicalPriceKg = concreteConsistancesPricesParameters.СhemicalPriceKg;

                var concreteTypesPrices = await _defaultParametersService.GetConcreteTypesPricesParametersAsync();

                model.PlatePrice = concreteTypesPrices.PlatePrice;
                model.CinderBlockPrice = concreteTypesPrices.CinderBlockPrice;
                model.InterestRate = concreteTypesPrices.InterestRate;

                var distancePriceParameters = await _defaultParametersService.GetDistancePriceParametersAsync();

                model.InitialDistanceKm = distancePriceParameters.InitialDistanceKm;
                model.InitialDistancePrice = distancePriceParameters.InitialDistancePrice;
                model.AfterInitialDistanceKm = distancePriceParameters.AfterInitialDistanceKm;
                model.AfterInitialDistancePrice = distancePriceParameters.AfterInitialDistancePrice;

                var sumpPiecesPricesParameters = await _defaultParametersService.GetSumpPiecesPricesParametersAsync();

                model.BottomPrice = sumpPiecesPricesParameters.BottomPrice;
                model.Sump60Price = sumpPiecesPricesParameters.Sump60Price;
                model.Sump90Price = sumpPiecesPricesParameters.Sump90Price;
                model.CoverPrice = sumpPiecesPricesParameters.CoverPrice;
            }
            catch (Exception ex)
            {
                Logger.Error("Error while getting security parameters:", ex);
                var result = HandleException(ex);
                if (result != null)
                    return result;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(PriceSettingsViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var concreteConsistancesParameters = _mapper.Map<PriceSettingsViewModel, ConcreteConsistancesParameters>(model);
                    await _defaultParametersService.SaveParametersAsync(concreteConsistancesParameters);

                    var concreteConsistancesPricesParameters = _mapper.Map<PriceSettingsViewModel, ConcreteConsistancesPricesParameters>(model);
                    await _defaultParametersService.SaveParametersAsync(concreteConsistancesPricesParameters);

                    var concreteTypesPricesParameters = _mapper.Map<PriceSettingsViewModel, ConcreteTypesPricesParameters>(model);
                    await _defaultParametersService.SaveParametersAsync(concreteTypesPricesParameters);

                    var distancePriceParameters = _mapper.Map<PriceSettingsViewModel,  DistancePriceParameters>(model);
                    await _defaultParametersService.SaveParametersAsync(distancePriceParameters);

                    var sumpPiecesParameters = _mapper.Map<PriceSettingsViewModel, SumpPiecesPricesParameters>(model);
                    await _defaultParametersService.SaveParametersAsync(sumpPiecesParameters);

                    ShowSuccessMessage();
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while getting security parameters:", ex);
                    var result = HandleException(ex);
                    if (result != null)
                        return result;
                }
            }

            return View(model);
        }
    }
}
