using System.Collections.Generic;
using System.Linq;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class ShopStatsData
    {
        public ShopStatsData() : this(new CostumersData(), new ProductData()) { }

        private ShopStatsData(CostumersData costumers, ProductData products)
        {
            Costumers = costumers;
            Products = products;
        }
        
        public CostumersData Costumers { get; }
        public ProductData Products { get; }
        public Currency TotalProfit => Costumers.ProfitFromCheatingWithChange + Products.TotalProfit;
        
        public static ShopStatsData operator +(ShopStatsData first, ShopStatsData second)
        {
            return new ShopStatsData(first.Costumers + second.Costumers, first.Products + second.Products);
        }
        
        public class CostumersData
        {
            public int TotalCount { get; private set; }
            public int CheatedWithChange { get; private set; }
            public Currency ProfitFromCheatingWithChange { get; private set; }

            public void AddCostumer()
            {
                TotalCount += 1;
            }

            public void AddCheatedWithChange(Currency profit)
            {
                CheatedWithChange += 1;
                ProfitFromCheatingWithChange += profit;
            }
            
            public static CostumersData operator +(CostumersData first, CostumersData second)
            {
                return new CostumersData
                {
                    TotalCount = first.TotalCount + second.TotalCount,
                    CheatedWithChange = first.CheatedWithChange + second.CheatedWithChange,
                    ProfitFromCheatingWithChange = first.ProfitFromCheatingWithChange + second.ProfitFromCheatingWithChange
                };
            }
        }

        public class ProductData
        {
            private readonly Dictionary<string, ProductInfo> _productsInfo;

            public ProductData() : this(new Dictionary<string, ProductInfo>()) { }
            
            private ProductData(Dictionary<string, ProductInfo> productsInfo)
            {
                _productsInfo = productsInfo;
            }
            
            public string MostPopularProductTag => _productsInfo.Count == 0 ? "" : _productsInfo.Aggregate((fPair, sPair) => fPair.Key == sPair.Key ? fPair 
                : fPair.Value.SalesCount > sPair.Value.SalesCount ? fPair : sPair).Key;
            public string BiggestProfitProductTag => _productsInfo.Count == 0 ? "" : _productsInfo.Aggregate((fPair, sPair) => fPair.Key == sPair.Key ? fPair
                : fPair.Value.Profit.TotalCents > sPair.Value.Profit.TotalCents ? fPair : sPair).Key ?? string.Empty;
            public Currency BiggestProfit => _productsInfo.Count == 0 ? Currency.Zero :  _productsInfo[BiggestProfitProductTag].Profit;
            public Currency TotalProfit => _productsInfo.Sum(pair => pair.Value.Profit.TotalCents);

            public void Add(string productTag, Currency price)
            {
                _productsInfo.TryAdd(productTag, new ProductInfo());
                _productsInfo[productTag].SalesCount += 1;
                _productsInfo[productTag].Profit += price;
            }

            public static ProductData operator +(ProductData first, ProductData second)
            {
                var productsInfo = new Dictionary<string, ProductInfo>();

                foreach (var pair in first._productsInfo)
                {
                    productsInfo.TryAdd(pair.Key, new ProductInfo());
                    productsInfo[pair.Key] += pair.Value;
                }
                
                foreach (var pair in second._productsInfo)
                {
                    productsInfo.TryAdd(pair.Key, new ProductInfo());
                    productsInfo[pair.Key] += pair.Value;
                }

                return new ProductData(productsInfo);
            }

            private class ProductInfo
            {
                public int SalesCount;
                public Currency Profit;

                public static ProductInfo operator +(ProductInfo first, ProductInfo second)
                {
                    return new ProductInfo
                    {
                        SalesCount = first.SalesCount + second.SalesCount,
                        Profit = first.Profit + second.Profit
                    };
                }
            }
        }
    }
}