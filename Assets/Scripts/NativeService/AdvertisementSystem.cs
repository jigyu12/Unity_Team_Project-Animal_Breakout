using GoogleMobileAds.Api;
using System;
using System.Collections.Generic;
using UnityCommunity.UnitySingleton;
using UnityEngine;

public class AdvertisementSystem : PersistentMonoSingleton<AdvertisementSystem>
{
    private BannerView bannerView;
    private RewardedAd rewardedAd;
    private InterstitialAd interstitialAd;

    private Action onGetReward;
    private Action onAdScreenClosed;
    private bool isWaitingRewardEarnedCallback;
    private bool isWaitingAdScreenClosedCallback;

#if UNITY_ANDROID
    //테스트 코드
    private string bannerAdUnitId = "ca-app-pub-3940256099942544/9214589741";
    private string rewardAdUnitId = "ca-app-pub-3940256099942544/5224354917";
    private string interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
    //진짜 광고코드
    //private string rewardAdUnitId = "ca-app-pub-9819783257891765/5740570359";
#elif UNITY_IPHONE
   private string bannerAdUnitId = "ca-app-pub-3940256099942544/2934735716"
    private string rewardAdUnitId = "ca-app-pub-3940256099942544/1712485313"
     private string interstitialAdUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
  private string bannerAdUnitId = "";
    private string rewardAdUnitId = "";
     private string interstitialAdUnitId = "";;
#endif

    public override void InitializeSingleton()
    {
        base.InitializeSingleton();

        MobileAds.Initialize(initializeStatus =>
        {
            Debug.Log("AdMob Initialize Success");
            //RequestBannerAdvertisement();
            RequestRewardAdvertisement();
            RequestInterstitialAdvertisement();
        });

        // When true all events raised by GoogleMobileAds will be raised
        // on the Unity main thread. The default value is false.
        //MobileAds.RaiseAdEventsOnUnityMainThread = true;
    }

    private void Update()
    {
        if (isWaitingRewardEarnedCallback)
        {
            OnGetRewardItem();
        }

        if (isWaitingAdScreenClosedCallback)
        {
            OnAdvertisementScreenClosed();
        }
    }

    public void RequestBannerAdvertisement()
    {
        // 이전 배너 파괴
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        AdSize adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        // 새 배너 생성
        bannerView = new BannerView(bannerAdUnitId, adaptiveSize, AdPosition.Bottom);

        RegisterBannerViewEventHandlers(bannerView);

        // 새로운 방식의 광고 요청 생성
        AdRequest request = new AdRequest();

        // 배너 광고 로드
        bannerView.LoadAd(request);
    }

    public void RequestRewardAdvertisement()
    {
        // Clean up the old ad before loading a new one.
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(rewardAdUnitId, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                rewardedAd = ad;
            });

    }

    //광고 보상 수령 액션, 광고창 끌때 액션, 광고창 끌때 타임스케일
    public void ShowRewardedAdvertisement(Action onGetReward, Action onAdScreenClosed, float timeScale = 0)
    {
        const string rewardMsg =
            "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            this.onGetReward = onGetReward;
            this.onAdScreenClosed = onAdScreenClosed;
            this.onAdScreenClosed += () => Time.timeScale = timeScale;

            RegisterRewardedAdEventHandlers(rewardedAd);
            //볼륨조절이 필요하다면 삽입
            //SetApplicationVolume()
            rewardedAd.Show((Reward reward) =>
            {
                // TODO: Reward the user.
                isWaitingRewardEarnedCallback = true;
                Debug.Log(String.Format(rewardMsg, reward.Type, reward.Amount));
            });
        }
    }


    private void RegisterBannerViewEventHandlers(BannerView ad)
    {
        // 이벤트 핸들러
        ad.OnBannerAdLoaded += () =>
        {
            Debug.Log("배너 광고 로드 성공");
        };

        ad.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.Log($"배너 광고 로드 실패: {error.GetMessage()}");
        };
    }

    private void RegisterRewardedAdEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
            RequestRewardAdvertisement();
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            isWaitingAdScreenClosedCallback = true;
            ad.Destroy();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }

    private void OnGetRewardItem()
    {
        isWaitingRewardEarnedCallback = false;
        onGetReward?.Invoke();
    }

    private void OnAdvertisementScreenClosed()
    {
        isWaitingAdScreenClosedCallback = false;
        onAdScreenClosed?.Invoke();
    }

    public void RequestInterstitialAdvertisement()
    {
        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(interstitialAdUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
            });
    }

    public void ShowInterstitialAdvertisement(Action onAdScreenClosed, float timeScale = 0)
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");

            this.onAdScreenClosed = onAdScreenClosed;
            this.onAdScreenClosed += () => Time.timeScale = timeScale;

            RegisterEventHandlers(interstitialAd);
            //볼륨조절이 필요하다면 삽입
            //SetApplicationVolume()
            interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    private void RegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
            RequestInterstitialAdvertisement();
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            isWaitingAdScreenClosedCallback = true;
            ad.Destroy();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
}
