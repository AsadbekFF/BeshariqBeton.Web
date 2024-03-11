using BeshariqBeton.BLL.Base;
using BeshariqBeton.BLL.Services;
using BeshariqBeton.Common.Entities;
using BeshariqBeton.Common.Models.Filters;
using BeshariqBeton.Common.Security;
using BeshariqBeton.DAL.Infrastructure;
using BeshariqBeton.Web.Infrastructure;
using BeshariqBeton.Web.Infrastructure.Authentication;
using BeshariqBeton.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BeshariqBeton.Web.Controllers
{
    [PermissionAuthorize(StandardPermissionNames.ManageSales)]
    public class SaleController : CrudController<Sale, SaleViewModel, int, MasterContext>
    {
        private readonly SaleService _saleService;

        public SaleController(Common.Logging.ILogger logger, IMapper mapper, SaleService service) : base(logger, mapper, service)
        {
            _saleService = service;
        }

        [HttpGet("[action]")]
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public async Task<IActionResult> GetDataJsonSearch(string sort, string order, int limit, int offset, string search, SaleFilter filter)
        {
            var result = await _saleService.FilterAsync(sort, order, limit, offset, search, filter);
            return Json(new { total = result.Total, rows = result.Rows });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> GetTotalPrice(SaleViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var entity = Mapper.Map<SaleViewModel, Sale>(viewModel);

                viewModel.TotalPrice = await _saleService.GetTotalPrice(entity);
            }

            return View("AddEdit", viewModel);
        }
    }
}
