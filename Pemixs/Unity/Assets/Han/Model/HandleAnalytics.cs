using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Remix
{
	public class HandleAnalytics : MonoBehaviour
	{
		public Native native;

		public string trackerIdForIOS, trackerIdForAndroid;

		public void LogScreen(string title){
			var cmd = string.Format (
				"?cmd={0}&tracker={1}&screen={2}",
				"GoogleAnalytics.trackScreen",
				WWW.EscapeURL (UnitId),
				title
			);
			native.Command (cmd);
		}

		public void LogException(string desc, bool isFatal){
			
		}

		public void LogItem(string id, string name, string sku, string category, double price, long quantity){
			LogEvent ("IAP", "PurchaseOK", sku, 1);
		}

		public void LogEvent(string category, string action, string label, long value){
			var cmd = string.Format (
				"?cmd={0}&tracker={1}&category={2}&action={3}&label={4}&value={5}",
				"GoogleAnalytics.trackEvent",
				WWW.EscapeURL (UnitId),
				WWW.EscapeURL (category),
				WWW.EscapeURL (action),
				WWW.EscapeURL (label),
				value+""
			);
			native.Command (cmd);
		}

		public void LogTiming(string category, long interval, string variable, string label){
			var cmd = string.Format (
				"?cmd={0}&tracker={1}&category={2}&variable={3}&label={4}&value={5}",
				"GoogleAnalytics.trackTiming",
				WWW.EscapeURL (UnitId),
				WWW.EscapeURL (category),
				WWW.EscapeURL (variable),
				WWW.EscapeURL (label),
				interval+""
			);
			native.Command (cmd);
		}

		public long enterTime;

		public void EnterApplication(){
			enterTime = DateTime.Now.Ticks;
		}

		public void ExitApplication(string page){
			TimeSpan travelTime = DateTime.Now - new DateTime(enterTime);
			LogTiming ("EnterExitAppDuration", Convert.ToInt64(travelTime.TotalSeconds), page, "");
			LogEvent ("Player", "ExitPage", page, 0);
		}

		public void TrackUnlockCaptureCount(IModel model){
			LogEvent ("Player", "UnlockCaptureCount", "", model.EnableCaptures.Count());
		}
			
		public void TrackCurrentLevel(IModel model){
			// 記錄最後關卡
			var currmap = model.GetCurrentMapIdx();
			var currLevel = model.GetCurrentLevel (currmap).First ();
			LogEvent ("Player", "CurrentLevel", currLevel.MapIdx+"_"+currLevel.Idx, currLevel.Idx); 
		}

		public void TrackPlayCapture(IModel model){
			LogEvent ("Player", "TrackPlayCapture", model.EnableCaptures.Count()+"", 0);
		}

		public void TrackGetAdReward(){
			LogEvent ("Player", "GetAdReward", "undefined", 0);
		}

		string UnitId{
			get{
				#if UNITY_EDITOR
				return trackerIdForIOS;
				#elif UNITY_ANDROID
				return trackerIdForAndroid;
				#else
				return trackerIdForIOS;
				#endif
			}
		}

	}
}

