using BeshariqBeton.BLL.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BeshariqBeton.Web.Utilities
{
    public class ListHelper
    {
        private readonly ClientService _clientService;

        public ListHelper(ClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task<IEnumerable<SelectListItem>> GetClientsAsync()
        {
            return (await _clientService.GetAllAsync()).OrderBy(c => c.Name).Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString(),
            });
        }
    }
}
