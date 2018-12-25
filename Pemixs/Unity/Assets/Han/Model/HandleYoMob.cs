using System;
using UnityEngine;
using System.Collections.Specialized;

namespace Remix
{
	public class HandleYoMob : MonoBehaviour
	{
		public event Action<Exception> OnShowFailed = delegate{};
		public event Action<Exception> OnADAwardFailed = delegate{};
		public event Action<Exception> OnException = delegate{};
		public event Action OnADAwardSuccess = delegate{};
		public event Action OnADClose = delegate{};

		public Native native;

		public string appId;
		public string sceneId;

		public void Init(){
			native.OnNativeCommand += OnNativeCommand;
			var cmd = string.Format (
				"?cmd={0}&appId={1}",
				"YoMob.setup",
				WWW.EscapeURL (appId)
			);
			native.Command (cmd);
		}

		public void ShowRewardAd(){
			var cmd = string.Format (
				"?cmd={0}&sceneId={1}",
				"YoMob.showAd",
				WWW.EscapeURL (sceneId)
			);
			native.Command (cmd);
		}

		void OnNativeCommand(string cmd, NameValueCollection querys){
			switch (cmd) {
			case "YoMob.onShowSuccess":
				break;
			case "YoMob.onShowFailed":
				{
					var reason = querys.GetValues ("reason") [0];
					OnShowFailed (new UnityException (reason));
				}
				break;
			case "YoMob.onADComplete":
				break;
			case "YoMob.onADClick":
				break;
			case "YoMob.onADClose":
				OnADClose ();
				break;
			case "YoMob.onADAwardSuccess":
				OnADAwardSuccess ();
				break;
			case "YoMob.onADAwardFailed":
				{
					var reason = querys.GetValues ("reason") [0];
					OnADAwardFailed (new UnityException (reason));
				}
				break;
			case "YoMob.onException":
				{
					var reason = querys.GetValues ("reason") [0];
					OnException (new UnityException (reason));
				}
				break;
			}
		}
	}
}

