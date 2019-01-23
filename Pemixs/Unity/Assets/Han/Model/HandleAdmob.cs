using System;
using UnityEngine;
using System.Collections;
using System.Collections.Specialized;

namespace Remix
{
	public class HandleAdmob : MonoBehaviour
	{
		public event Action<Exception> OnInterstitialDidFailToReceiveAdWithError = delegate{};
		public event Action<Exception> OnAdViewDidFailToReceiveAdWithError = delegate{};
		public event Action OnInterstitialDidReceiveAd = delegate{};
		public event Action OnInterstitialDidDismissScreen = delegate{};

		public Native native;
		public bool enableBannerFeature;

		public string bannerUnitId;

		public void Init(){
			native.OnNativeCommand += OnNativeCommand;
		}

		public void LoadBanner(){
			if (enableBannerFeature == false) {
				return;
			}
			var cmd = string.Format (
				"?cmd={0}&unitId={1}&y={2}",
				"GoogleAds.loadBanner",
				WWW.EscapeURL (bannerUnitId),
				50
			);
			native.Command (cmd);
		}

		public bool IsBannerVisible{
			set{
				if (enableBannerFeature == false) {
					return;
				}
				var cmd = string.Format (
					"?cmd={0}&visible={1}",
					"GoogleAds.setBannerVisible",
					value ? 1 : 0
				);
				native.Command (cmd);
			}
		}

		public string rewardUnitId;

		public void LoadRewardAd(){
			var cmd = string.Format (
				"?cmd={0}&unitId={1}",
				"GoogleAds.loadInter",
				WWW.EscapeURL (rewardUnitId)
			);
			native.Command (cmd);

			#if UNITY_EDITOR
			Debug.LogWarning("AD出現");
			OnNativeCommand("GoogleAds.interstitialDidReceiveAd", null);
			#endif
		}

		public void ShowRewardAd(){
			var cmd = string.Format (
				"?cmd={0}",
				"GoogleAds.showInterstitial"
			);
			native.Command (cmd);

			#if UNITY_EDITOR
			Debug.LogWarning("AD關閉");
			OnNativeCommand("GoogleAds.interstitialDidDismissScreen", null);
			#endif
		}

		void OnNativeCommand(string cmd, NameValueCollection querys){
			switch (cmd) {
			case "GoogleAds.interstitialDidReceiveAd":
				OnInterstitialDidReceiveAd ();
				break;
			case "GoogleAds.interstitialDidFailToReceiveAdWithError":
				{
					var reason = querys.GetValues ("reason") [0];
					OnInterstitialDidFailToReceiveAdWithError (new UnityException (reason));
				}
				break;
			case "GoogleAds.adViewDidFailToReceiveAdWithError":
				{
					var reason = querys.GetValues ("reason") [0];
					OnAdViewDidFailToReceiveAdWithError (new UnityException (reason));
				}
				break;
			case "GoogleAds.interstitialDidDismissScreen":
				{
					OnInterstitialDidDismissScreen ();
				}
				break;
			case "":
				break;
			}
		}

		public const string TestUnitId = "ca-app-pub-3940256099942544/2934735716";
		public const string TestRewardId = "ca-app-pub-3940256099942544/4411468910";

		public void SetTestUnitId(){
			bannerUnitId = TestUnitId;
			rewardUnitId = TestRewardId;
		}
	}
}

