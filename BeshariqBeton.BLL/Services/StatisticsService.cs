using BeshariqBeton.Common.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeshariqBeton.BLL.Services
{
    public class StatisticsService
    {
        private readonly string[] Months = new string[] { "Yanvar", "Fevral", "Mart", "Aprel", "May", "Iyun", "Iyul", "Avgust", "Sentabr", "Oktabr", "Noyabr", "Dekabr" };
        private readonly SaleService _saleService;

        public StatisticsService(SaleService saleService)
        {
            _saleService = saleService;
        }

        public async Task<Statistics> GetYearStatistics()
        {
            var result = new Statistics();
            var yearAgoDate = DateTime.Now.AddMonths(-11);
            List<string> months = new List<string>();

            var concrete100Dataset = new Datasets
            {
                Label = "Beton 100",
                BorderColor = "rgb(31, 35, 36)"
            };

            var concrete150Dataset = new Datasets
            {
                Label = "Beton 150",
                BorderColor = "rgb(144, 50, 21)"
            };

            var concrete200Dataset = new Datasets
            {
                Label = "Beton 200",
                BorderColor = "rgb(82, 76, 24)"
            };

            var concrete250Dataset = new Datasets
            {
                Label = "Beton 250",
                BorderColor = "rgb(46, 88, 29)"
            };

            var concrete300Dataset = new Datasets
            {
                Label = "Beton 300",
                BorderColor = "rgb(5, 78, 81)"
            };

            var concrete350Dataset = new Datasets
            {
                Label = "Beton 350",
                BorderColor = "rgb(4, 69, 121)"
            };

            var concrete400Dataset = new Datasets
            {
                Label = "Beton 400",
                BorderColor = "rgb(72, 24, 130)"
            };

            var totalDataset = new Datasets
            {
                Label = "Umumiy",
                BorderColor = "rgb(173, 37, 37)"
            };

            for (var i = Months.Length; i > 0; i--)
            {
                var salesInPeriod = await _saleService.GetSalesInPeriod(yearAgoDate.Month, yearAgoDate.Year);

                var concrete100Sales = salesInPeriod.Where(s => s.ConcreteProductType == Common.Enums.ConcreteProductType.Concrete100).Select(s => s.Count).Sum();
                concrete100Dataset.Data.Add(concrete100Sales);

                var concrete150Sales = salesInPeriod.Where(s => s.ConcreteProductType == Common.Enums.ConcreteProductType.Concrete150).Select(s => s.Count).Sum();
                concrete150Dataset.Data.Add(concrete150Sales);

                var concrete200Sales = salesInPeriod.Where(s => s.ConcreteProductType == Common.Enums.ConcreteProductType.Concrete200).Select(s => s.Count).Sum();
                concrete200Dataset.Data.Add(concrete200Sales);

                var concrete250Sales = salesInPeriod.Where(s => s.ConcreteProductType == Common.Enums.ConcreteProductType.Concrete250).Select(s => s.Count).Sum();
                concrete250Dataset.Data.Add(concrete250Sales);

                var concrete300Sales = salesInPeriod.Where(s => s.ConcreteProductType == Common.Enums.ConcreteProductType.Concrete300).Select(s => s.Count).Sum();
                concrete300Dataset.Data.Add(concrete300Sales);

                var concrete350Sales = salesInPeriod.Where(s => s.ConcreteProductType == Common.Enums.ConcreteProductType.Concrete350).Select(s => s.Count).Sum();
                concrete350Dataset.Data.Add(concrete350Sales);

                var concrete400Sales = salesInPeriod.Where(s => s.ConcreteProductType == Common.Enums.ConcreteProductType.Concrete400).Select(s => s.Count).Sum();
                concrete400Dataset.Data.Add(concrete400Sales);

                var totalSales = concrete100Sales + concrete150Sales + concrete200Sales + concrete250Sales + concrete300Sales + concrete350Sales + concrete400Sales;

                var month = $"{Months[yearAgoDate.Month - 1]}, {yearAgoDate.Year}";
                result.Labels.Add(month);

                yearAgoDate = yearAgoDate.AddMonths(1);
            }

            result.Datasets.Add(concrete100Dataset);
            result.Datasets.Add(concrete150Dataset);
            result.Datasets.Add(concrete200Dataset);
            result.Datasets.Add(concrete250Dataset);
            result.Datasets.Add(concrete300Dataset);
            result.Datasets.Add(concrete350Dataset);
            result.Datasets.Add(concrete400Dataset);

            return result;
        }

        public async Task<Statistics> GetDailyStatistics(int month, int year)
        {
            var result = new Statistics();
            var days = DateTime.DaysInMonth(year, month);

            var concrete100Dataset = new Datasets
            {
                Label = "Beton 100",
                BorderColor = "rgb(31, 35, 36)"
            };

            var concrete150Dataset = new Datasets
            {
                Label = "Beton 150",
                BorderColor = "rgb(144, 50, 21)"
            };

            var concrete200Dataset = new Datasets
            {
                Label = "Beton 200",
                BorderColor = "rgb(82, 76, 24)"
            };

            var concrete250Dataset = new Datasets
            {
                Label = "Beton 250",
                BorderColor = "rgb(46, 88, 29)"
            };

            var concrete300Dataset = new Datasets
            {
                Label = "Beton 300",
                BorderColor = "rgb(5, 78, 81)"
            };

            var concrete350Dataset = new Datasets
            {
                Label = "Beton 350",
                BorderColor = "rgb(4, 69, 121)"
            };

            var concrete400Dataset = new Datasets
            {
                Label = "Beton 400",
                BorderColor = "rgb(72, 24, 130)"
            };

            for (int i = 1; i <= days; i++)
            {
                var salesInPeriod = await _saleService.GetSalesInPeriod(month, year, i);

                var concrete100Sales = salesInPeriod.Where(s => s.ConcreteProductType == Common.Enums.ConcreteProductType.Concrete100).Select(s => s.Count).Sum();
                concrete100Dataset.Data.Add(concrete100Sales);

                var concrete150Sales = salesInPeriod.Where(s => s.ConcreteProductType == Common.Enums.ConcreteProductType.Concrete150).Select(s => s.Count).Sum();
                concrete150Dataset.Data.Add(concrete150Sales);

                var concrete200Sales = salesInPeriod.Where(s => s.ConcreteProductType == Common.Enums.ConcreteProductType.Concrete200).Select(s => s.Count).Sum();
                concrete200Dataset.Data.Add(concrete200Sales);

                var concrete250Sales = salesInPeriod.Where(s => s.ConcreteProductType == Common.Enums.ConcreteProductType.Concrete250).Select(s => s.Count).Sum();
                concrete250Dataset.Data.Add(concrete250Sales);

                var concrete300Sales = salesInPeriod.Where(s => s.ConcreteProductType == Common.Enums.ConcreteProductType.Concrete300).Select(s => s.Count).Sum();
                concrete300Dataset.Data.Add(concrete300Sales);

                var concrete350Sales = salesInPeriod.Where(s => s.ConcreteProductType == Common.Enums.ConcreteProductType.Concrete350).Select(s => s.Count).Sum();
                concrete350Dataset.Data.Add(concrete350Sales);

                var concrete400Sales = salesInPeriod.Where(s => s.ConcreteProductType == Common.Enums.ConcreteProductType.Concrete400).Select(s => s.Count).Sum();
                concrete400Dataset.Data.Add(concrete400Sales);

                result.Labels.Add(i.ToString());
            }

            result.Datasets.Add(concrete100Dataset);
            result.Datasets.Add(concrete150Dataset);
            result.Datasets.Add(concrete200Dataset);
            result.Datasets.Add(concrete250Dataset);
            result.Datasets.Add(concrete300Dataset);
            result.Datasets.Add(concrete350Dataset);
            result.Datasets.Add(concrete400Dataset);

            return result;
        }
    }
}
