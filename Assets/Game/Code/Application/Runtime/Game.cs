using System.Collections;
using Lean.Localization;
using UnityEngine;
using YellowSquad.CashierSimulator.Gameplay;
using YellowSquad.CashierSimulator.Gameplay.Meta;
using YellowSquad.CashierSimulator.Gameplay.Useful;
using YellowSquad.CashierSimulator.UserInput;
using YellowSquad.GamePlatformSdk;

namespace YellowSquad.CashierSimulator.Application
{
    public class Game : MonoBehaviour
    {
        private const string LeaderboardName = "MainLeaderboard";

        private const string Ru = "Language/Russian";
        private const string En = "Language/English";

        [SerializeField] private Shop _shop;
        [SerializeField] private Wallet _wallet;
        [SerializeField] private ProductsInventory _productsInventory;
        [SerializeField] private PurchaseProductMenu _purchaseProductMenu;
        [SerializeField] private ShopUpgradeMenu _shopUpgradeMenu;
        [SerializeField] private GameTutorial _gameTutorial;
        [SerializeField] private InputRouter _inputRouter;
        [SerializeField] private GameSettings _settings;
        [SerializeField] private CanvasGroup _blackScreenCanvasGroup;

        private bool _needUpdate = true;

        private void Awake()
        {
#if !UNITY_EDITOR
            UnityEngine.Application.targetFrameRate = (int)Screen.currentResolution.refreshRateRatio.value;
#endif
        }

        private IEnumerator Start()
        {
            _inputRouter.SetActiveCursor(false);
            _blackScreenCanvasGroup.Enable();
            
            yield return GamePlatformSdkContext.Current.Initialize();
            
            LeanLocalization.SetCurrentLanguageAll(GamePlatformSdkContext.Current.Language == Language.Russian ? Ru : En);
            
            _blackScreenCanvasGroup.Disable(0.5f);
            
            _needUpdate = true;
            
            _productsInventory.Initialize(GamePlatformSdkContext.Current.Save);
            _shop.Initialize(GamePlatformSdkContext.Current.Save);
            _wallet.Initialize(GamePlatformSdkContext.Current.Save);
            
            yield return PlayTutorialIfNeed();

            bool firstIterationEnded = false;

            while (true)
            {
                if (GamePlatformSdkContext.Current.Save.HasKey(SaveConstants.ShopStatsSaveKey) == false || firstIterationEnded)
                {
                    _inputRouter.ResetCameraRotation();
                    _shop.StartDay();

                    yield return new WaitUntil(() => _shop.WorkIsDone);
                    yield return new WaitForSeconds(5);

                    while (_settings.Opened)
                        yield return new WaitForSeconds(1);

                    _needUpdate = false;
                    _inputRouter.SetActiveCursor(true);
                
                    _shop.DeactivateBoosts();
                    _shop.ShowStats();
                
                    UpdateLeaderboardScore();
                    GamePlatformSdkContext.Current.Save.Save();
                
                    yield return new WaitUntil(() => _shop.StatsShowing == false);
                    yield return GamePlatformSdkContext.Current.Advertisement.ShowInterstitial();
                }
                
                _needUpdate = false;
                _inputRouter.SetActiveCursor(true);

                _shopUpgradeMenu.Open();
                
                yield return new WaitForSeconds(0.3f);
                _inputRouter.ResetCameraRotation();

                while (_shopUpgradeMenu.Opened)
                {
                    yield return new WaitUntil(() => _shopUpgradeMenu.Opened == false);
                    _shop.SaveShopUpgrades();
                    GamePlatformSdkContext.Current.Save.Save();

                    if (_purchaseProductMenu.Opened == false) 
                        continue;
                    
                    yield return new WaitUntil(() => _purchaseProductMenu.Opened == false);
                    _shop.SaveProducts();
                    GamePlatformSdkContext.Current.Save.Save();
                }

                _inputRouter.SetActiveCursor(false);
                _needUpdate = true;

                firstIterationEnded = true;
            }
        }

        private void Update()
        {
            if (_needUpdate == false)
                return;
            
            _inputRouter.UpdateInput();
        }

        private IEnumerator PlayTutorialIfNeed()
        {
            if (GamePlatformSdkContext.Current.Save.HasKey(SaveConstants.GameTutorialSaveKey)) 
                yield break;
            
            _needUpdate = false;
                    
            _inputRouter.SetActiveCursor(true);
            yield return _gameTutorial.Play();
                
            GamePlatformSdkContext.Current.Save.SetString(SaveConstants.GameTutorialSaveKey, "Complete");
            GamePlatformSdkContext.Current.Save.Save();
                    
            _needUpdate = true;
        }

        private void UpdateLeaderboardScore()
        {
            int bestScore = GamePlatformSdkContext.Current.Save.GetLeaderboardScore(LeaderboardName);
            int currentScore = _shop.CurrentDay;

            if (currentScore <= bestScore)
                return;
            
            GamePlatformSdkContext.Current.Save.SetLeaderboardScore(LeaderboardName, currentScore);
        }
    }
}
