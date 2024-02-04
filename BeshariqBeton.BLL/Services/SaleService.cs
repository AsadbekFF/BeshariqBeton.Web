using BeshariqBeton.BLL.Base;
using BeshariqBeton.Common.Entities;
using BeshariqBeton.Common.Models;
using BeshariqBeton.Common.Models.Parameters;
using BeshariqBeton.DAL.Infrastructure;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Items<Sale>> FilterAsync(string sort, string order, int limit, int offset, string search)
        {
            var predicate = PredicateBuilder.New<Sale>(true);

            if (!string.IsNullOrWhiteSpace(search))
            {
                predicate.Or(e => e.Client.Name.Contains(search));
            }

            return await FilterAsync(sort, order, limit, offset, predicate, new string[] {nameof(Sale.Client)}, null);
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
            await _storageService.CheckAndSaveStorageForSale(entity);
            entity.TotalPrice = await GetTotalPrice(entity);

            await base.AddAsync(entity);
        }

        public override async Task UpdateAsync(Sale entity)
        {
            entity.TotalPrice = await GetTotalPrice(entity);

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

            var interestRate = (await _defaultParametersService.GetConcreteTypesPricesParametersAsync()).InterestRate;

            var total = productPrice + distancePrice;

            return Math.Round(interestRate > 0 ? total + (total * ((double)interestRate / 100d)) : total, 2);
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
                sale.Count);
        }

        private async Task<double> GetTotalPriceConcrete150(Sale sale)
        {
            var concreteConsistancesParameters = await _defaultParametersService.GetConcreteConsistancesParametersAsync();

            return await GetConcreteConsistancePrice(concreteConsistancesParameters.СementWeightKg150,
                concreteConsistancesParameters.SandVolume150,
                concreteConsistancesParameters.ShabenVolume150,
                concreteConsistancesParameters.СhemicalKg150,
                sale.Count);
        }

        private async Task<double> GetTotalPriceConcrete200(Sale sale)
        {
            var concreteConsistancesParameters = await _defaultParametersService.GetConcreteConsistancesParametersAsync();

            return await GetConcreteConsistancePrice(concreteConsistancesParameters.СementWeightKg200,
                concreteConsistancesParameters.SandVolume200,
                concreteConsistancesParameters.ShabenVolume200,
                concreteConsistancesParameters.СhemicalKg200,
                sale.Count);
        }

        private async Task<double> GetTotalPriceConcrete250(Sale sale)
        {
            var concreteConsistancesParameters = await _defaultParametersService.GetConcreteConsistancesParametersAsync();

            return await GetConcreteConsistancePrice(concreteConsistancesParameters.СementWeightKg250,
                concreteConsistancesParameters.SandVolume250,
                concreteConsistancesParameters.ShabenVolume250,
                concreteConsistancesParameters.СhemicalKg250,
                sale.Count);
        }

        private async Task<double> GetTotalPriceConcrete300(Sale sale)
        {
            var concreteConsistancesParameters = await _defaultParametersService.GetConcreteConsistancesParametersAsync();

            return await GetConcreteConsistancePrice(concreteConsistancesParameters.СementWeightKg300,
                concreteConsistancesParameters.SandVolume300,
                concreteConsistancesParameters.ShabenVolume300,
                concreteConsistancesParameters.СhemicalKg300,
                sale.Count);
        }

        private async Task<double> GetTotalPriceConcrete350(Sale sale)
        {
            var concreteConsistancesParameters = await _defaultParametersService.GetConcreteConsistancesParametersAsync();

            return await GetConcreteConsistancePrice(concreteConsistancesParameters.СementWeightKg350,
                concreteConsistancesParameters.SandVolume350,
                concreteConsistancesParameters.ShabenVolume350,
                concreteConsistancesParameters.СhemicalKg350,
                sale.Count);
        }

        private async Task<double> GetTotalPriceConcrete400(Sale sale)
        {
            var concreteConsistancesParameters = await _defaultParametersService.GetConcreteConsistancesParametersAsync();

            return await GetConcreteConsistancePrice(concreteConsistancesParameters.СementWeightKg400,
                concreteConsistancesParameters.SandVolume400,
                concreteConsistancesParameters.ShabenVolume400,
                concreteConsistancesParameters.СhemicalKg400,
                sale.Count);
        }

        private async Task<double> GetConcreteConsistancePrice(float cementWeight, float sandWeight, float shebenWeight, float chemicalWeight, int count)
        {
            var concreteConsistancesPricesParameters = await _defaultParametersService.GetConcreteConsistancesPricesParametersAsync();

            double cementPrice = cementWeight * count * concreteConsistancesPricesParameters.СementPriceKg;
            double sandCount = sandWeight * count * concreteConsistancesPricesParameters.SandPriceM3;
            double shebenPrice = shebenWeight * count * concreteConsistancesPricesParameters.ShabenPriceM3;
            double chemicalPrice = chemicalWeight * count * concreteConsistancesPricesParameters.СhemicalPriceKg;
            return (cementPrice + sandCount + chemicalPrice + shebenPrice);
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
    }
}
