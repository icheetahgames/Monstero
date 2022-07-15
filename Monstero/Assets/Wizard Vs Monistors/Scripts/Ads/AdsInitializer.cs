using UnityEngine.Advertisements;
using UnityEngine;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    string _androidGameId = "4375435";
    string _iOSGameId = "4375434";
    [SerializeField] bool _testMode = true;
    private string _gameId;

    [SerializeField] RewardedAdsButton _invokeBootBtnADs;
    [SerializeField] RewardedAdsButtonDoublicate _invokeDoublicateBtnADs;
    // [SerializeField] InterstitialAdsButton interstitialAdsButton;
    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameId
            : _androidGameId;
        Advertisement.Initialize(_gameId, _testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        _invokeBootBtnADs.LoadAd();
        _invokeDoublicateBtnADs.LoadAd();
 //      interstitialAdsButton.LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}
