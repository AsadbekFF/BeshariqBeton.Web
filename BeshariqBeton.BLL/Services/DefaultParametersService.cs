using BeshariqBeton.Common.Entities;
using BeshariqBeton.Common.Models.Parameters;
using BeshariqBeton.Common.Utilities;
using BeshariqBeton.DAL.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.BLL.Services
{
    public class DefaultParametersService
    {
        private readonly MasterContext _context;

        public DefaultParametersService(MasterContext context)
        {
            _context = context;
        }

        public async Task<CompanyParameters> GetCompanyParametersAsync()
        {
            var parameters = new CompanyParameters
            {
                LogoPath = null
            };

            await SetParameters(parameters);
            return parameters;
        }

        public async Task<ConcreteConsistancesParameters> GetConcreteConsistancesParametersAsync()
        {
            var parameters = new ConcreteConsistancesParameters();

            await SetParameters(parameters);
            return parameters;
        }

        public async Task<ConcreteConsistancesPricesParameters> GetConcreteConsistancesPricesParametersAsync()
        {
            var parameters = new ConcreteConsistancesPricesParameters();

            await SetParameters(parameters);
            return parameters;
        }

        public async Task<SumpPiecesPricesParameters> GetSumpPiecesPricesParametersAsync()
        {
            var parameters = new SumpPiecesPricesParameters();

            await SetParameters(parameters);
            return parameters;
        }

        public async Task<ConcreteTypesPricesParameters> GetConcreteTypesPricesParametersAsync()
        {
            var parameters = new ConcreteTypesPricesParameters();

            await SetParameters(parameters);
            return parameters;
        }

        public async Task<StorageParameters> GetStorageParametersAsync()
        {
            var parameters = new StorageParameters();

            await SetParameters(parameters);
            return parameters;
        }

        public async Task<DistancePriceParameters> GetDistancePriceParametersAsync()
        {
            var parameters = new DistancePriceParameters();

            await SetParameters(parameters);
            return parameters;
        }

        public async Task SaveParametersAsync<T>(T parameters)
        {
            // Get properties
            var properties = typeof(T).GetProperties();

            var dbParameters = await
                _context.DefaultParameters
                    .AsTracking()
                    .Where(p => properties.Select(property => property.Name).Contains(p.Name))
                    .ToListAsync();

            foreach (var property in properties)
            {
                var parameter = dbParameters.FirstOrDefault(p => p.Name == property.Name);

                string value;

                // Enum - use underling number
                if (property.PropertyType.IsEnum)
                    value = property.GetValue(parameters).ToValue().ToString();
                // Simple data type
                else
                    value = Convert.ToString(property.GetValue(parameters));

                // Add
                if (parameter == null)
                    await _context.DefaultParameters.AddAsync(new DefaultParameter
                    {
                        Name = property.Name,
                        Value = value
                    });
                // Update
                else
                    parameter.Value = value;
            }

            await _context.SaveChangesAsync();
        }

        // Get all parameters from DB based on object properties fields.
        private async Task SetParameters<T>(T defaults)
        {
            // Get properties
            var properties = typeof(T).GetProperties();
            var parameters = await
                _context.DefaultParameters.Where(p => properties.Select(property => property.Name).Contains(p.Name))
                    .ToListAsync();

            foreach (var parameter in parameters)
            {
                var property = properties.First(p => p.Name == parameter.Name);

                object value = null;

                if (!string.IsNullOrEmpty(parameter.Value))
                {
                    var propertyType = property.PropertyType.Name.IndexOf("Nullable`", StringComparison.OrdinalIgnoreCase) == 0
                        ? property.PropertyType.GenericTypeArguments.FirstOrDefault()
                        : property.PropertyType;

                    // Timespan
                    if (propertyType == typeof(TimeSpan))
                        value = TimeSpan.Parse(parameter.Value);
                    // Enum
                    else if (propertyType?.IsEnum ?? false)
                        value = Enum.Parse(propertyType, parameter.Value);
                    // Simple data type
                    else
                        value = Convert.ChangeType(parameter.Value, propertyType);
                }

                property.SetValue(defaults, value, null);
            }
        }
    }
}
