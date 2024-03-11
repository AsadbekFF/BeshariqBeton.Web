using BeshariqBeton.BLL.Base;
using BeshariqBeton.Common.Entities;
using BeshariqBeton.Common.Enums;
using BeshariqBeton.Common.Models;
using BeshariqBeton.Common.Models.Filters;
using BeshariqBeton.DAL.Infrastructure;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;

namespace BeshariqBeton.BLL.Services
{
    public class SaleService : BaseService<Sale, int, MasterContext>
    {
        private readonly DefaultParametersService _defaultParametersService;
        private readonly StorageService _storageService;

        public SaleService(MasterContext context, DefaultParametersService defaultParametersService, StorageService storageService) : base(context)
        {
            _defaultParametersService = defaultParametersService;
            _storageService = storageService;
        }

        public override Task<Items<Sale>> FilterAsync(string sort, string order, int limit, int offset)
        {
            return base.FilterAsync(sort, order, limit, offset, new string[] {nameof(Sale.Client)});
        }

        public async Task<Items<Sale>> FilterAsync(string sort, string order, int limit, int offset, string search, SaleFilter filter)
        {
            var predicate = PredicateBuilder.New<Sale>(true);

            if (!string.IsNullOrWhiteSpace(search))
            {
                predicate.Or(e => e.Client.Name.Contains(search));
            }

            if (filter.SaleFilterType == SaleFilterType.ByClient && filter.ClientId.HasValue)
            {
                predicate.And(s => s.ClientId == filter.ClientId);
            }

            if (filter.SaleFilterType == SaleFilterType.ByProduct && filter.ConcreteProductType.HasValue)
            {
                predicate.And(s => s.ConcreteProductType == filter.ConcreteProductType);
            }

            if (filter.From.HasValue)
            {
                predicate.And(s => s.ComeOutDateTime.Date >= filter.From.Value.Date);
            }

            if (filter.To.HasValue)
            {
                predicate.And(s => s.ComeOutDateTime.Date <= filter.To.Value.Date);
            }

            return await CustomFilterAsync(sort, order, limit, offset, predicate, new string[] {nameof(Sale.Client)}, null);
        }

        private async Task<Items<Sale>> CustomFilterAsync(string sort, string order, int limit, int offset, Expression<Func<Sale, bool>> filterExpression, string[] includedProperties, Expression<Func<Sale, Sale>> projectionExpression)
        {
            var items = Context.Set<Sale>().AsQueryable();

            // Included properties
            if (includedProperties != null)
                foreach (var includeProperty in includedProperties)
                    items = items.Include(includeProperty);

            if (filterExpression != null)
                items = items.Where(filterExpression);

            // Order
            if (!string.IsNullOrEmpty(sort))
                items = items.OrderBy(sort + " " + order);

            // Total items
            var total = await items.CountAsync();

            // Pagination
            if (limit > 0)
                items = items.Skip(offset).Take(limit);

            // Projection
            if (projectionExpression != null)
                items = items.Select(projectionExpression);

            var result = await items.ToListAsync();

            result.Add(new Sale
            {
                Client = new Client
                {
                    Name = "Umumiy narxi"
                },
                TotalPrice = items.Sum(s => s.TotalPrice),
            });

            // Pagination
            return new Items<Sale>
            {
                Rows = result,
                Total = total
            };
        }

        public override Task<Sale> GetByIdNotTrackingAsync(int id)
        {
            return base.GetByIdNotTrackingAsync(id, new string[] {nameof(Sale.Client)});
        }

        public override Task<Sale> GetByIdAsync(int id)
        {
            return base.GetByIdAsync(id, new string[] { nameof(Sale.Client) });
        }

        public override async Task AddAsync(Sale entity)
        {
            await _storageService.RemoveFromStorageAsync(entity);
            entity.TotalPrice = await GetTotalPrice(entity);

            await base.AddAsync(entity);
        }

        public override async Task UpdateAsync(Sale entity)
        {
            entity.TotalPrice = await GetTotalPrice(entity);

            var dbSale = await GetByIdNotTrackingAsync(entity.Id);

            if (dbSale.ConcreteProductType != entity.ConcreteProductType)
            {
                if (IsConcreteType(dbSale) && IsConcreteType(entity))
                    await _storageService.ChangeConcreteType(dbSale, entity);
                else if (!IsConcreteType(dbSale) && IsConcreteType(entity))
                    await _storageService.RemoveFromStorageAsync(entity);
                else if (IsConcreteType(dbSale) && !IsConcreteType(entity))
                    await _storageService.AddToStorageAsync(dbSale);
            }

            await base.UpdateAsync(entity);
        }

        public async Task<double> GetTotalPrice(Sale sale)
        {
            double productPrice = 0;
            switch (sale.ConcreteProductType)
            {
                case Common.Enums.ConcreteProductType.Concrete100:
                    productPrice = await GetTotalPriceConcrete100(sale);
                    break;
                case Common.Enums.ConcreteProductType.Concrete150:
                    productPrice = await GetTotalPriceConcrete150(sale);
                    break;
                case Common.Enums.ConcreteProductType.Concrete200:
                    productPrice = await GetTotalPriceConcrete200(sale);
                    break;
                case Common.Enums.ConcreteProductType.Concrete250:
                    productPrice = await GetTotalPriceConcrete250(sale);
                    break;
                case Common.Enums.ConcreteProductType.Concrete300:
                    productPrice = await GetTotalPriceConcrete300(sale);
                    break;
                case Common.Enums.ConcreteProductType.Concrete350:
                    productPrice = await GetTotalPriceConcrete350(sale);
                    break;
                case Common.Enums.ConcreteProductType.Concrete400:
                    productPrice = await GetTotalPriceConcrete400(sale);
                    break;
                case Common.Enums.ConcreteProductType.Sump:
                    productPrice = await GetSumpPrice(sale);
                    break;
                case Common.Enums.ConcreteProductType.Plate:
                    productPrice = await GetPlatePrice(sale);
                    break;
                case Common.Enums.ConcreteProductType.CinderBlock:
                    productPrice = await GetCinderPrice(sale);
                    break;
            }

            var distancePrice = await GetDistancePrice((await Context.Clients.FirstOrDefaultAsync(c => c.Id == sale.ClientId))?.DistanceKm ?? 0);

            var total = productPrice + distancePrice;

            return Math.Round(total, 2);
        }

        public async Task<List<Sale>> GetSalesInPeriod(int month, int year)
        {
            return await Context.Sales.Where(s => s.ComeOutDateTime.Month == month && s.ComeOutDateTime.Year == year).ToListAsync();
        }

        public async Task<List<Sale>> GetSalesInPeriod(int month, int year, int day)
        {
            return await Context.Sales.Where(s => s.ComeOutDateTime.Month == month && s.ComeOutDateTime.Year == year && s.ComeOutDateTime.Day == day).ToListAsync();
        }

        private async Task<double> GetTotalPriceConcrete100(Sale sale)
        {
            var concreteConsistancesParameters = await _defaultParametersService.GetConcreteConsistancesParametersAsync();

            return await GetConcreteConsistancePrice(concreteConsistancesParameters.СementWeightKg100,
                concreteConsistancesParameters.SandVolume100,
                concreteConsistancesParameters.ShabenVolume100,
                concreteConsistancesParameters.СhemicalKg100,
                sale.Count,
                sale.PaymentType == Common.Enums.PaymentType.Card);
        }

        private async Task<double> GetTotalPriceConcrete150(Sale sale)
        {
            var concreteConsistancesParameters = await _defaultParametersService.GetConcreteConsistancesParametersAsync();

            return await GetConcreteConsistancePrice(concreteConsistancesParameters.СementWeightKg150,
                concreteConsistancesParameters.SandVolume150,
                concreteConsistancesParameters.ShabenVolume150,
                concreteConsistancesParameters.СhemicalKg150,
                sale.Count,
                sale.PaymentType == Common.Enums.PaymentType.Card);
        }

        private async Task<double> GetTotalPriceConcrete200(Sale sale)
        {
            var concreteConsistancesParameters = await _defaultParametersService.GetConcreteConsistancesParametersAsync();

            return await GetConcreteConsistancePrice(concreteConsistancesParameters.СementWeightKg200,
                concreteConsistancesParameters.SandVolume200,
                concreteConsistancesParameters.ShabenVolume200,
                concreteConsistancesParameters.СhemicalKg200,
                sale.Count,
                sale.PaymentType == Common.Enums.PaymentType.Card);
        }

        private async Task<double> GetTotalPriceConcrete250(Sale sale)
        {
            var concreteConsistancesParameters = await _defaultParametersService.GetConcreteConsistancesParametersAsync();

            return await GetConcreteConsistancePrice(concreteConsistancesParameters.СementWeightKg250,
                concreteConsistancesParameters.SandVolume250,
                concreteConsistancesParameters.ShabenVolume250,
                concreteConsistancesParameters.СhemicalKg250,
                sale.Count,
                sale.PaymentType == Common.Enums.PaymentType.Card);
        }

        private async Task<double> GetTotalPriceConcrete300(Sale sale)
        {
            var concreteConsistancesParameters = await _defaultParametersService.GetConcreteConsistancesParametersAsync();

            return await GetConcreteConsistancePrice(concreteConsistancesParameters.СementWeightKg300,
                concreteConsistancesParameters.SandVolume300,
                concreteConsistancesParameters.ShabenVolume300,
                concreteConsistancesParameters.СhemicalKg300,
                sale.Count,
                sale.PaymentType == Common.Enums.PaymentType.Card);
        }

        private async Task<double> GetTotalPriceConcrete350(Sale sale)
        {
            var concreteConsistancesParameters = await _defaultParametersService.GetConcreteConsistancesParametersAsync();

            return await GetConcreteConsistancePrice(concreteConsistancesParameters.СementWeightKg350,
                concreteConsistancesParameters.SandVolume350,
                concreteConsistancesParameters.ShabenVolume350,
                concreteConsistancesParameters.СhemicalKg350,
                sale.Count,
                sale.PaymentType == Common.Enums.PaymentType.Card);
        }

        private async Task<double> GetTotalPriceConcrete400(Sale sale)
        {
            var concreteConsistancesParameters = await _defaultParametersService.GetConcreteConsistancesParametersAsync();

            return await GetConcreteConsistancePrice(concreteConsistancesParameters.СementWeightKg400,
                concreteConsistancesParameters.SandVolume400,
                concreteConsistancesParameters.ShabenVolume400,
                concreteConsistancesParameters.СhemicalKg400,
                sale.Count,
                sale.PaymentType == Common.Enums.PaymentType.Card);
        }

        private async Task<double> GetConcreteConsistancePrice(float cementWeight, float sandWeight, float shebenWeight, float chemicalWeight, int count, bool withNds)
        {
            var concreteConsistancesPricesParameters = await _defaultParametersService.GetConcreteConsistancesPricesParametersAsync();
            var pricesParameters = await _defaultParametersService.GetConcreteTypesPricesParametersAsync();
            var interestRate = pricesParameters.InterestRate;

            double cementPrice = cementWeight * count * concreteConsistancesPricesParameters.СementPriceKg;
            double sandCount = sandWeight * count * concreteConsistancesPricesParameters.SandPriceM3;
            double shebenPrice = shebenWeight * count * concreteConsistancesPricesParameters.ShabenPriceM3;
            double chemicalPrice = chemicalWeight * count * concreteConsistancesPricesParameters.СhemicalPriceKg;

            var total = cementPrice + sandCount + chemicalPrice + shebenPrice;

            if (withNds)
            {
                var nds = pricesParameters.NdsPercent;

                total = nds > 0 ? total + (total * ((double)nds / 100d)) : total;
            }

            return interestRate > 0 ? total + (total * ((double)interestRate / 100d)) : total;
        }

        private async Task<long> GetSumpPrice(Sale sale)
        {
            var sumpPiecesParameters = await _defaultParametersService.GetSumpPiecesPricesParametersAsync();

            var bottomPrice = sale.BottomCount.Value * sumpPiecesParameters.BottomPrice;
            var sump60Price = sale.Sump60Count.Value * sumpPiecesParameters.Sump60Price;
            var sump90Price = sale.Sump90Count.Value * sumpPiecesParameters.Sump90Price;
            var coverPrice = sale.CoverCount.Value * sumpPiecesParameters.CoverPrice;

            return bottomPrice + sump60Price + sump90Price + coverPrice;
        }

        private async Task<long> GetPlatePrice(Sale sale)
        {
            var concreteTypesParameters = await _defaultParametersService.GetConcreteTypesPricesParametersAsync();

            return concreteTypesParameters.PlatePrice * sale.Count;
        }

        private async Task<long> GetCinderPrice(Sale sale)
        {
            var concreteTypesParameters = await _defaultParametersService.GetConcreteTypesPricesParametersAsync();

            return concreteTypesParameters.CinderBlockPrice * sale.Count;
        }

        private async Task<long> GetDistancePrice(int distance)
        {
            var distancePriceParameters = await _defaultParametersService.GetDistancePriceParametersAsync();

            if (distance <= distancePriceParameters.InitialDistanceKm)
            {
                return distancePriceParameters.InitialDistancePrice;
            }

            long price = distancePriceParameters.InitialDistancePrice;
            distance -= distancePriceParameters.InitialDistanceKm;

            if (distancePriceParameters.AfterInitialDistanceKm <= 0)
            {
                return price;
            }

            do
            {
                price += distancePriceParameters.AfterInitialDistancePrice;
            }
            while ((distance -= distancePriceParameters.AfterInitialDistanceKm) >= 0);

            return price;
        }

        private bool IsConcreteType(Sale sale)
        {
            if (sale.ConcreteProductType == Common.Enums.ConcreteProductType.Sump ||
                sale.ConcreteProductType == Common.Enums.ConcreteProductType.Plate ||
                sale.ConcreteProductType == Common.Enums.ConcreteProductType.CinderBlock)
                return false;

            return true;
        }
    }
}
