using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapTap.TapAd;


public class MobileAdManager
{
	public static MobileAdManager ins;

	long media_id = 1007826;
	string media_name = "晶石守护者";
	string media_key = "e6fNGJPMISPZdH2LFKbbJTiFvQ4rO5BrxhSi2z1IAUMg9J4OaTbAU1cImhu3Cxng";
	string channel = "TapTap";
	int _rewardAdId = 1006769;

	System.Action callBack;

    private TapRewardVideoAd _tapRewardAd;

	public static void Init(){
		ins = new MobileAdManager();
	}

	public void RequestPermission(){
		TapAdSdk.RequestPermissionIfNecessary();
	}

	public void InitAd(){
        if (TapAdSdk.IsInited)
        {
            return;
        }
		TapAdConfig config = new TapAdConfig.Builder()
                .MediaId(media_id)             // 必选参数，为 TapADN 注册的媒体 ID
                .MediaName(media_name)         // 必选参数，为 TapADN 注册的媒体名称
                .MediaKey(media_key)           // 必选参数，媒体密钥，可以在 TapADN 后台查看（用于传输数据的解密）
                .MediaVersion("1")                  // 必选参数，默认值 "1"
                .Channel(channel)             // 必选参数，渠道
                .Build();

		// FormalCustomControllerWrapper 为实现了 TapTap.TapAd.ICustomController 的类
		// onInitedCallback 为可选回调函数，类型是 System.Action，
		TapAdSdk.Init(config, new FormalCustomControllerWrapper(this), ()=>{
			LoadRewardedAd();
		});
	}

    private async void LoadRewardedAd()
    {
        if (_tapRewardAd != null)
        {
            _tapRewardAd.Dispose();
            _tapRewardAd = null;
        }
        // create AdRequest
        var request = new TapAdRequest.Builder()
            .SpaceId(_rewardAdId)
            .Extra1("{}")
            .RewardName("Reward")
            .RewardCount(1)
            .UserId("User")
            .Build();
        _tapRewardAd = new TapRewardVideoAd(request);
        _tapRewardAd.SetLoadListener(new RewardVideoAdLoadListener(this));
        _tapRewardAd.Load();
    }

  	public void ShowRewardedAd(System.Action callBack){
  		this.callBack = callBack;
        if (TapAdSdk.IsInited == false)
        {
            ToastBar.ShowMsg("TapAd 需要先初始化!");
            return;
        }
        if (_tapRewardAd != null)
        {
            _tapRewardAd.SetInteractionListener(new RewardVideoInteractionListener(this));
            _tapRewardAd.Show();
        }
        else
        {
            ToastBar.ShowMsg("未加载好视频,无法播放!");
        }
	}

    public sealed class RewardVideoAdLoadListener : IRewardVideoAdLoadListener
    {
        private readonly MobileAdManager context;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public RewardVideoAdLoadListener(MobileAdManager context)
        {
            this.context = context;
        }

        public void OnError(int code, string message)
        {
            message = message ?? "NULL";
            Debug.LogErrorFormat($"加载激励视频错误! 错误 code: {code} message: {message}");
        }

        public void OnRewardVideoAdCached(TapRewardVideoAd ad)
        {
            Debug.Log($"{ad.AdType}素材 Cached 完毕! ad != null: {(ad != null).ToString()} ");
        }
        
        public void OnRewardVideoAdLoad(TapRewardVideoAd ad)
        {
            Debug.Log($"{ad.AdType}素材 Load 完毕! ad != null: {(ad != null).ToString()} ");
        }
    }

    public sealed class RewardVideoInteractionListener : IRewardVideoInteractionListener
    {
        private readonly MobileAdManager context;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public RewardVideoInteractionListener(MobileAdManager context)
        {
            this.context = context;
        }

        public void OnAdShow(TapRewardVideoAd ad)
        {
            Debug.Log($"[Unity::AD] {ad.AdType} OnAdShow");
        }

        public void OnAdClose(TapRewardVideoAd ad)
        {
            Debug.Log($"[Unity::AD] {ad.AdType} OnAdClose");
            MobileAdManager.ins.callBack();
            MobileAdManager.ins.LoadRewardedAd();
        }

        public void OnVideoComplete(TapRewardVideoAd ad)
        {
            Debug.Log($"[Unity::AD] {ad.AdType} OnVideoComplete");
        }

        public void OnVideoError(TapRewardVideoAd ad)
        {
            Debug.Log($"[Unity::AD] {ad.AdType} OnVideoError");
        }

        public void OnRewardVerify(TapRewardVideoAd ad, bool rewardVerify, int rewardAmount, string rewardName, int code, string msg)
        {
            Debug.Log($"[Unity::AD] {ad.AdType} OnRewardVerify rewardVerify: {rewardVerify} rewardAmount: {rewardAmount} rewardName: {rewardName} code: {code} msg: {msg}");
        }

        public void OnSkippedVideo(TapRewardVideoAd ad)
        {
            Debug.Log($"[Unity::AD] {ad.AdType} OnSkippedVideo");
        }

        public void OnAdClick(TapRewardVideoAd ad)
        {
            Debug.Log($"[Unity::AD] {ad.AdType} OnAdClick");
            Debug.LogErrorFormat($"激励视频 点击");
        }
    }
    

    public sealed class FormalCustomControllerWrapper : ICustomController
    {
        private readonly MobileAdManager context;

        /// <summary>
        /// constructor bind with Java interface
        /// </summary>
        /// <param name="context"></param>
        public FormalCustomControllerWrapper(MobileAdManager context)
        {
            this.context = context;
        }

        public bool CanUseLocation => false;

        public TapAdLocation GetTapAdLocation => null;
        
        public bool CanUsePhoneState => true;
        public string GetDevImei => "";
        public bool CanUseWifiState => true;
        public bool CanUseWriteExternal => true;
        public string GetDevOaid => null;
        public bool Alist => true;
        public bool CanUseAndroidId => true;
        public CustomUser ProvideCustomer() => null;
    }
}

