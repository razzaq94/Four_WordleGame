using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public const string AdRemovedKey = "IsAdsRemoved";
    DateTime adStartingTime ;
    DateTime adFinishingTime ;
    public bool GetBooster = false;
    public bool IsAdsEnabled
    {
        get => PlayerPrefs.GetInt(AdRemovedKey, 0) == 0;
        set
        {
            PlayerPrefs.SetInt(AdRemovedKey, value ? 0 : 1);
        }
    }

    Action OnRewardedCallback = null;

    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = false;

    private string _gameId;

    [SerializeField] AdID _bannerID;
    [SerializeField] AdID _interstitialID;
    [SerializeField] AdID _rewardedVideoID;
    [SerializeField] BannerPosition _bannerPosition = BannerPosition.BOTTOM_CENTER;

    public static AdsManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        InitializeAds();
    }

    #region Initialize

    public void InitializeAds()
    {
        if (!IsAdsEnabled)
            return;

        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? _iOSGameId : _androidGameId;
        Advertisement.Initialize(_gameId, _testMode, this);
    }

    private void OnApplicationFocus(bool focus)
    {
            LoadBannerAd();
    }
    void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            Advertisement.Banner.Hide(true);

        }
        else
        {
            //if (Advertisement.Banner.isLoaded) 
            //    Advertisement.Banner.Hide(true);

            LoadBannerAd();
        }
    }


    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");

        if (PlayerPrefs.GetInt("HideBanner", 0) == 1)
        {

        }
        else
        {
        }
        //if (Advertisement.Banner.isLoaded) 
        //    Advertisement.Banner.Hide(true);

        LoadBannerAd();
        LoadInterstitialAd();
        LoadRewardedVideoAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }

    #endregion

    #region Banner Ads

    public void LoadBannerAd()
    {
        if (GlobalData.Instance.isPremium == false)
        {
            return;
        }


            if (!Advertisement.isInitialized)
            return;

        if (!IsAdsEnabled)
            return;

        if (Advertisement.Banner.isLoaded)
            Advertisement.Banner.Hide(true);

        print("Is Banner Loaded  : " + Advertisement.Banner.isLoaded);


        Advertisement.Banner.SetPosition(_bannerPosition);

        string _adUnitId = getAdID(AdType.Banner);

        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        Advertisement.Banner.Load(_adUnitId, options);
    }

    void OnBannerLoaded()
    {
        Debug.Log("Banner loaded");
        if (GlobalData.Instance.isPremium == false)
        {
            ShowBannerAd();
        }
    }

    void OnBannerError(string message)
    {
        Debug.Log($"Banner Error: {message}");
        // Optionally execute additional code, such as attempting to load another ad.
        //if (Advertisement.Banner.isLoaded) 
        //    Advertisement.Banner.Hide(true);

      //  LoadBannerAd();
        
    }

    public void ShowBannerAd()
    {
        if (!Advertisement.isInitialized)
            return;

        if (!IsAdsEnabled)
            return;

        string _adUnitId = getAdID(AdType.Banner);

        // Set up options to notify the SDK of show events:
        BannerOptions options = new BannerOptions
        {
            clickCallback = OnBannerClicked,
            hideCallback = OnBannerHidden,
            showCallback = OnBannerShown
        };

        // Show the loaded Banner Ad Unit:
        Advertisement.Banner.Show(_adUnitId, options);
    }

    public void HideBannerAd()
    {
        // Hide the banner:
        Advertisement.Banner.Hide();
    }

    void OnBannerClicked() { }
    void OnBannerShown() { }
    void OnBannerHidden() { }

    #endregion

    #region Interstitial Ads

    public void LoadInterstitialAd()
    {

        if (!Advertisement.isInitialized)
            return;

        if (!IsAdsEnabled)
            return;

        string _adUnitId = getAdID(AdType.Interstitial);
        if (_adUnitId == null)
        {
            Debug.Log("Ads Not supported on this platform");
            return;
        }

        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }

    public void ShowInterstitialAd()
    {
        if (!Advertisement.isInitialized)
            return;

        if (!IsAdsEnabled)
            return;

        string _adUnitId = getAdID(AdType.Interstitial);
        if (_adUnitId == null)
        {
            Debug.Log("Ads Not supported on this platform");
            return;
        }

        Debug.Log("Showing Ad: " + _adUnitId);
        if (Timer.Instance != null && GetBooster==true )
        {
            Timer.Instance.StopTimer(true);
            adStartingTime = System.DateTime.Now;
        }
        Advertisement.Show(_adUnitId, this);
    }

    #endregion

    #region Rewarded Video Ads

    public void LoadRewardedVideoAd()
    {
        //GameControl.instance.rewardedButton.interactable = false;
        if (!Advertisement.isInitialized)
            return;

        if (!IsAdsEnabled)
            return;

        rewardedVideoAdLoaded = false;
        string _adUnitId = getAdID(AdType.Rewarded);
        if (_adUnitId == null)
        {
            Debug.Log("Ads Not supported on this platform");
            return;
        }

        Debug.Log("Loading Ad: " + _adUnitId);
        Advertisement.Load(_adUnitId, this);
    }
    public bool IsRewardedVideoAdLoaded()
    {
        if (rewardedVideoAdLoaded)
            return true;
        else
            return false;

    }
    public void ShowRewardedVideoAd(Action onRewarded = null)
    {
        if (!Advertisement.isInitialized)
            return;

        if (!IsAdsEnabled)
            return;

        OnRewardedCallback = onRewarded;

        string _adUnitId = getAdID(AdType.Rewarded);
        if (_adUnitId == null)
        {
            Debug.Log("Ads Not supported on this platform");
            return;
        }

        Debug.Log("Showing Ad: " + _adUnitId);
        if (Timer.Instance != null && GetBooster == true)
        {
            Timer.Instance.StopTimer(true);
            adStartingTime = System.DateTime.Now;
        }
        Advertisement.Show(_adUnitId, this);
    }

    #endregion

    #region Callbacks

    // Implement Load Listener and Show Listener interface methods: 
    bool rewardedVideoAdLoaded;
    public void OnUnityAdsAdLoaded(string adUnitId)
    {
        // Optionally execute code if the Ad Unit successfully loads content.
        if (adUnitId == getAdID(AdType.Rewarded))
        {
            //print("Rewarded loaded");
            rewardedVideoAdLoaded = true;
            //GameControl.instance.rewardedButton.interactable = true;
            //UIManager.AdLoaded = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
    {
        Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.

        if (adUnitId == getAdID(AdType.Interstitial))
        {
            LoadInterstitialAd();
        }
        else if (adUnitId == getAdID(AdType.Rewarded))
        {

            LoadRewardedVideoAd();
        }
    }

    public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
        // Optionally execute code if the Ad Unit fails to show, such as loading another ad.

        if (adUnitId == getAdID(AdType.Interstitial))
        {
            LoadInterstitialAd();
        }
        else if (adUnitId == getAdID(AdType.Rewarded))
        {
            Time.timeScale = 1;
            LoadRewardedVideoAd();
        }
    }

    public void OnUnityAdsShowStart(string adUnitId)
    {
      //  Time.timeScale = 0;
        Time.timeScale = 1;
    }
    public void OnUnityAdsShowClick(string adUnitId) { }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            print("Ad is completed before..");
            if (Timer.Instance != null && GetBooster == true)
            {
                adFinishingTime = System.DateTime.Now;
                print(adFinishingTime + "  " + adStartingTime);
                TimeSpan timeDiff = adFinishingTime - adStartingTime;
                print(timeDiff);
                print("Current time before " + Timer.Instance.CurrentTime);

                Timer.Instance.CurrentTime -= (float)timeDiff.TotalSeconds;
                print("Current time after" + Timer.Instance.CurrentTime);
                timeDiff = TimeSpan.Zero;
                Timer.Instance.StopTimer(false);
                GetBooster = false;
            }
            if (GlobalData.Instance.isRevealBoosterAdShown)
            {
                GlobalData.Instance.isRevealBoosterAdShown = false;
                GlobalData.Instance.UpdateBoosterCount("Reveal", 1);
                UIManager.Instance.UpdateRevealUI(GlobalData.Instance.RevealBoosterCount);
            }
            else if (GlobalData.Instance.isEliminateBoosterAdShown)
            {
                GlobalData.Instance.isEliminateBoosterAdShown = false;
                GlobalData.Instance.UpdateBoosterCount("Eliminate", 1);
                UIManager.Instance.UpdateEliminateUI(GlobalData.Instance.EliminateBoosterCount);
            }
            if (adUnitId == getAdID(AdType.Rewarded))
            {
                if (OnRewardedCallback != null)
                {
                    print("Ad is completed..");
                      OnRewardedCallback.Invoke();
                   
                }
            }
        }

        if (adUnitId == getAdID(AdType.Interstitial))
        {
            LoadInterstitialAd();
        }
        else if (adUnitId == getAdID(AdType.Rewarded))
        {
            LoadRewardedVideoAd();
        }
        //if (StartMenu.instance != null)
        //    StartMenu.instance.checkMusic = false;

        Time.timeScale = 1;
    }

    #endregion

    #region Utility Functions

    string getAdID(AdType adType)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (adType == AdType.Banner)
                return _bannerID.IOS_ID;
            else if (adType == AdType.Interstitial)
                return _interstitialID.IOS_ID;
            else if (adType == AdType.Rewarded)
                return _rewardedVideoID.IOS_ID;
            else
                return null;
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            if (adType == AdType.Banner)
                return _bannerID.Android_ID;
            else if (adType == AdType.Interstitial)
                return _interstitialID.Android_ID;
            else if (adType == AdType.Rewarded)
                return _rewardedVideoID.Android_ID;
            else
                return null;
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.LinuxEditor)
        {
            if (adType == AdType.Banner)
                return _bannerID.Android_ID != null ? _bannerID.Android_ID : _bannerID.IOS_ID;
            else if (adType == AdType.Interstitial)
                return _interstitialID.Android_ID != null ? _interstitialID.Android_ID : _interstitialID.IOS_ID;
            else if (adType == AdType.Rewarded)
                return _rewardedVideoID.Android_ID != null ? _rewardedVideoID.Android_ID : _rewardedVideoID.IOS_ID;
            else
                return null;
        }
        else
        {
            return null;
        }
    }

    public void OnAdsRemoved()
    {
        HideBannerAd();
    }

    #endregion
}

[System.Serializable]
public class AdID
{
    public string Android_ID;
    public string IOS_ID;
}

[System.Serializable]
public enum AdType
{
    Banner, Interstitial, Rewarded
}