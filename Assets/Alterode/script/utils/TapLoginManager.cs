using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using TapTap.Common;
using TapTap.Login;
using Honeti;
using TapTap.AntiAddiction;
using TapTap.AntiAddiction.Model;

public class TapLoginManager : MonoBehaviour
{
	static string CLIENT_ID = "3wwmg3bmbm5kkpuzcl";

	public static void Init(){
		TapLogin.Init(CLIENT_ID);
		//AntiAddictionUIKit.SetTestEnvironment(true);
	}

	public static async void CheckLogin(bool noAntiAddiction){
		try 
		{
		    var accesstoken = await TapLogin.GetAccessToken();
		    //Debug.Log("已登录");

			var profile = await TapLogin.GetProfile();
			GlobalData.tapUserProfile = profile;
			LoginDone(noAntiAddiction);

		} 
		catch (Exception e)
		{
		    Debug.Log("当前未登录");

		    Login(noAntiAddiction);
		}

		// 获取用户信息

		// 获取实时更新的用户信息
		//await TapLogin.FetchProfile();
	}

	public static async void Login(bool noAntiAddiction){
		try
		{
		    // 在 iOS、Android 系统下，会唤起 TapTap 客户端或以 WebView 方式进行登录
		    // 在 Windows、macOS 系统下显示二维码（默认）和跳转链接（需配置）
		    var accessToken = await TapLogin.Login();
		    Debug.Log($"TapTap 登录成功 accessToken: {accessToken.ToJson()}");
		}
		catch (Exception e)
		{
		    if (e is TapException tapError)  // using TapTap.Common
		    {
		        Debug.Log($"encounter exception:{tapError.code} message:{tapError.message}");
		        if (tapError.code == (int)TapErrorCode.ERROR_CODE_BIND_CANCEL) // 取消登录
		        {
		        	ToastBar.ShowMsg("需要登录以验证防沉迷系统");
		            Debug.Log("登录取消");
		        }
		    }
		}

		// 获取 TapTap Profile  可以获得当前用户的一些基本信息，例如名称、头像。
		var profile = await TapLogin.FetchProfile();
		GlobalData.tapUserProfile = profile;
		//Debug.Log($"TapTap 登录成功 profile: {profile.ToJson()}");

		LoginDone(noAntiAddiction);
	}

	public static void LoginDone(bool noAntiAddiction){
		Debug.Log("登录成功:"+GlobalData.tapUserProfile.name);
		
		if(!noAntiAddiction){
			AntiAddiction();
		}
		else{
        	LoadingMask.ins.LoadSceneAsync("MainScene");
		}
	}

	public static void AntiAddiction(){
		Action<int, string> callback = async (code, extra) => {
		      // 防沉迷回调
		     UnityEngine.Debug.LogFormat($"code: {code} error Message: {extra}");

		     if(code == 500){
        		LoadingMask.ins.LoadSceneAsync("MainScene");
		     }
		     else if(code == 1030 || code == 1050 || code == 1100){
		     	ToastBar.ShowMsg("受防沉迷系统限制，当前时间无法进行游戏");
		     }
		     else{
		     	ToastBar.ShowMsg("防沉迷验证失败(错误码:"+code+")");
		     }
		};

		AntiAddictionConfig config = new AntiAddictionConfig()
		{
		    gameId = CLIENT_ID,      // TapTap 开发者中心对应 Client ID
		    showSwitchAccount = true,      // 是否显示切换账号按钮
		    useAgeRange = true      // 是否使用年龄段信息
		};         
		//设置配置及回调，callback 为开发者实现的自定义防沉迷回调对象
		AntiAddictionUIKit.Init(config);
		AntiAddictionUIKit.SetAntiAddictionCallback(callback);
		AntiAddictionUIKit.StartupWithTapTap(GlobalData.tapUserProfile.unionid);
	}
}