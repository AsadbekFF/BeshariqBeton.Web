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
    public class StorageService
    {
        private readonly DefaultParametersService _defaultParametersService;
        public StorageService(DefaultParametersService defaultParametersService)
        {
            _defaultParametersService = defaultParametersService;
        }

        public async Task CheckAndSaveStorageForSale(Sale sale)
        {
            double cementNeed = 0;
            double sandNeed = 0;
            double shebenNeed = 0;
            double chemicalNeed = 0;
            var consistanceParameters = await _defaultParametersService.GetConcreteConsistancesParametersAsync();

            switch(sale.ConcreteProductType)
            {
                case Common.Enums.ConcreteProductType.Concrete100:
                    cementNeed = consistanceParameters.СementWeightKg100;
                    sandNeed = consistanceParameters.SandVolume100;
                    shebenNeed = consistanceParameters.ShabenVolume100;
                    chemicalNeed = consistanceParameters.СhemicalKg100;
                    break;
                case Common.Enums.ConcreteProductType.Concrete150:
                    cementNeed = consistanceParameters.СementWeightKg150;
                    sandNeed = consistanceParameters.SandVolume150;
                    shebenNeed = consistanceParameters.ShabenVolume150;
                    chemicalNeed = consistanceParameters.СhemicalKg150;
                    break;
                case Common.Enums.ConcreteProductType.Concrete200:
                    cementNeed = consistanceParameters.СementWeightKg200;
                    sandNeed = consistanceParameters.SandVolume200;
                    shebenNeed = consistanceParameters.ShabenVolume200;
                    chemicalNeed = consistanceParameters.СhemicalKg200;
                    break;
                case Common.Enums.ConcreteProductType.Concrete250:
                    cementNeed = consistanceParameters.СementWeightKg250;
                    sandNeed = consistanceParameters.SandVolume250;
                    shebenNeed = consistanceParameters.ShabenVolume250;
                    chemicalNeed = consistanceParameters.СhemicalKg250;
                    break;
                case Common.Enums.ConcreteProductType.Concrete300:
                    cementNeed = consistanceParameters.СementWeightKg300;
                    sandNeed = consistanceParameters.SandVolume300;
                    shebenNeed = consistanceParameters.ShabenVolume300;
                    chemicalNeed = consistanceParameters.СhemicalKg300;
                    break;
                case Common.Enums.ConcreteProductType.Concrete350:
                    cementNeed = consistanceParameters.СementWeightKg350;
                    sandNeed = consistanceParameters.SandVolume350;
                    shebenNeed = consistanceParameters.ShabenVolume350;
                    chemicalNeed = consistanceParameters.СhemicalKg350;
                    break;
                case Common.Enums.ConcreteProductType.Concrete400:
                    cementNeed = consistanceParameters.СementWeightKg400;
                    sandNeed = consistanceParameters.SandVolume400;
                    shebenNeed = consistanceParameters.ShabenVolume400;
                    chemicalNeed = consistanceParameters.СhemicalKg400;
                    break;
            }

            cementNeed *= sale.Count;
            sandNeed *= sale.Count;
            chemicalNeed *= sale.Count;
            shebenNeed *= sale.Count;

            var errorMessage = new StringBuilder();
            var storageParameters = await _defaultParametersService.GetStorageParametersAsync();

            if (cementNeed > storageParameters.CementRemainKg)
            {
                errorMessage.AppendLine("Sement yetarlik emas.");
            }

            if (sandNeed > storageParameters.SandRemainM3)
            {
                errorMessage.AppendLine("Qum yetarlik emas.");
            }

            if (shebenNeed > storageParameters.ShebenRemainM3)
            {
                errorMessage.AppendLine("Sheben yetarlik emas.");
            }

            if (chemicalNeed > storageParameters.ChemicalRemainKg)
            {
                errorMessage.AppendLine("Ximikat yetarlik emas.");
            }

            if (errorMessage.Length > 0)
            {
                throw new BeshariqBeton.Common.Exceptions.BetonException(errorMessage.ToString());
            }
            else
            {
                storageParameters.CementRemainKg -= cementNeed;
                storageParameters.SandRemainM3 -= sandNeed;
                storageParameters.ShebenRemainM3 -= shebenNeed;
                storageParameters.ChemicalRemainKg -= chemicalNeed;

                await _defaultParametersService.SaveParametersAsync(storageParameters);
            }
        }
    }
}
