using UnityEngine;
using UnityEngine.UI;
using YellowSquad.CashierSimulator.Gameplay.Useful;

namespace YellowSquad.CashierSimulator.Gameplay
{
    public class ShopStatsView : MonoBehaviour
    {
        [SerializeField] private StatsText _dayText;
        [SerializeField] private StatsText _satisfiedCustomersText;
        [SerializeField] private StatsText _costumersCheatedWithChangeText;
        [SerializeField] private StatsText _totalCostumersText;
        [SerializeField] private StatsText _mostPopularProductText;
        [SerializeField] private StatsText _biggestProfitProductText;
        [SerializeField] private StatsText _balanceText;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _closeButton;
        [SerializeField] private ProductList _productList;

        public bool Opened { get; private set; }

        private void Awake()
        {
            _canvasGroup.Disable();
        }

        private void OnEnable()
        {
            _closeButton.onClick.AddListener(OnCloseButtonClick);
        }

        private void OnDisable()
        {
            _closeButton.onClick.RemoveListener(OnCloseButtonClick);
        }

        public void Render(ShopStatsData stats, int day)
        {
            Opened = true;
            
            _dayText.Render(day.ToString());

            int satisfiedCustomers = stats.Costumers.TotalCount - stats.Costumers.CheatedWithChange;
            _satisfiedCustomersText.Render(satisfiedCustomers.ToString(), satisfiedCustomers > 0);
            _costumersCheatedWithChangeText.Render(stats.Costumers.CheatedWithChange.ToString(), stats.Costumers.CheatedWithChange > 0);
            _totalCostumersText.Render(stats.Costumers.TotalCount.ToString());
            
            _mostPopularProductText.Render(string.IsNullOrEmpty(stats.Products.MostPopularProductTag) ? "" 
                : _productList.FindInfoBy(stats.Products.MostPopularProductTag).RuName);
            _biggestProfitProductText.Render(string.IsNullOrEmpty(stats.Products.BiggestProfitProductTag) ? "" 
                : $"{_productList.FindInfoBy(stats.Products.BiggestProfitProductTag).RuName} (+{stats.Products.BiggestProfit.ToPriceTag()})");
            
            _balanceText.Render($"+{stats.TotalProfit.ToPriceTag()}", stats.TotalProfit.TotalCents > 0);
            
            _canvasGroup.Enable(0.2f);
        }

        private void OnCloseButtonClick()
        {
            if (Opened == false)
                return;
            
            _canvasGroup.Disable();
            Opened = false;
        }
    }
}