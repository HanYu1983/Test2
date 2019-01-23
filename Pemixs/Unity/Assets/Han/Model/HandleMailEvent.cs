using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace Remix
{
	public class HandleMailEvent : MonoBehaviour
	{
		public DeviceData deviceData;
		public UserSettings userSettings;
		[Tooltip("使用本地端每日禮物。這個請打勾，可以節省後端資源")]
		public bool useLocalDailyGift;
		#region mail
		IEnumerable<RemixApi.Mail> mails = new List<RemixApi.Mail>();
		public IEnumerator LoadMail(IModel model, Action<Exception> after){
			try{
				mails = RemixApi.LoadMail (deviceData.DeviceID);
				if( useLocalDailyGift ){
					// 去除後端下載的每日禮物郵件
					var tmpMails = mails.Where(mail=>{
						return mail.IsDailyGift == false;
					});
					// 手動加入本地的每日禮物郵件
					if(model.IsAlreadyGetGift(DateTime.Now.Ticks) == false){
						var giftKey = string.Format ("Mg{0:00}", model.GetTopGiftId()+1);
						try{
							var gift = GiftDef.Get(giftKey);
							var manualGiftMail = new RemixApi.Mail();
							manualGiftMail.ID = System.Guid.NewGuid().ToString();
							manualGiftMail.Description = giftKey;
							// 這行的位置必須在設定完manualGiftMail.Description之後
							if(manualGiftMail.IsDailyGift == false){
								throw new UnityException("必須符合每日禮物的格式");
							}
							manualGiftMail.Gift = gift.Item;
							manualGiftMail.GiftCount = gift.Quantity;
							// 必須給予假值，不然會出現非預期的狀況
							manualGiftMail.Unread = true;
							manualGiftMail.Description = giftKey;
							tmpMails = tmpMails.Concat(Enumerable.Repeat(manualGiftMail,1));
						}catch(Exception e){
							Util.Instance.LogWarning("這天沒禮物:"+giftKey+":"+e.Message);
						}
					}
					// 替換掉
					mails = tmpMails;
				}
				after(null);
			} catch (Exception e){
				Debug.LogWarning ("無法取得郵件:"+e.Message);
				after (e);
			}
			yield return null;
		}

		public IEnumerable<RemixApi.Mail> Mails{
			get {
				if (mails == null) {
					throw new UnityException ("請先呼叫LoadMail");
				}
				return mails;
			}
		}

		public IEnumerator GetUserGotGiftCountInMonth(IModel model, Action<Exception, int> after){
			if (useLocalDailyGift) {
				after (null, model.GetTopGiftId ());
				yield break;
			}
			int count = 0;
			Exception e = null;
			try{
				count = RemixApi.GetUserGotGiftCountInMonth (deviceData.DeviceID);
			}catch(Exception ee){
				Debug.LogWarning ("無法取得禮物次數:"+ee.Message);
				e = ee;
			}
			yield return null;
			after (e, count);
		}

		public IEnumerator MarkMailRead(IModel model, string mailId, Action<Exception> after){
			if (useLocalDailyGift) {
				var giftMail = mails.Where (mail => {
					return mail.ID == mailId;
				}).FirstOrDefault ();
				if (giftMail == null) {
					Util.Instance.LogWarning ("no has mail:" + mailId);
					after (null);
					yield break;
				}
				if (giftMail.IsDailyGift) {
					// 這裡只記錄已拿取，拿取禮物的code在Main裡：尋找MarkMailRead的ref
					model.GetTodayGift ();
					// 直接去掉
					var tmpMail = mails.Where (mail => {
						return mail.ID != mailId;
					});
					mails = tmpMail;
					after (null);
					yield break;
				}
			}
			Exception ee = null;
			try{
				RemixApi.MarkMailRead (deviceData.DeviceID, mailId);
			}catch(Exception e){
				Debug.LogWarning ("無法標示已讀:" + e.Message);
				ee = e;
			}
			if (ee != null) {
				after (ee);
				yield return null;
			} else {
				yield return LoadMail (model, after);
			}
		}

		public IEnumerable<RemixApi.Mail> UnreadMails{
			get {
				if (mails == null) {
					throw new UnityException ("請先呼叫LoadMail");
				}
				return from m in mails
						where m.Unread == true
					select m;
			}
		}

		#endregion

		#region event
		IEnumerable<RemixApi.Event> events = new List<RemixApi.Event>();
		public IEnumerator LoadEvent(Action<Exception> after){
			after (null);
			// 先不必有事件功能
			/*try{
				events = RemixApi.LoadEvent ();
				after(null);
			}catch(Exception e){
				Debug.LogWarning ("無法取得事件:"+e.Message);
				after (e);
			}*/
			yield return null;
		}

		public IEnumerable<RemixApi.Event> Events {
			get {
				if (events == null) {
					throw new UnityException ("請先呼叫LoadEvent");
				}
				return events;
			}
		}

		public RemixApi.Event EventWithID(int id){
			var evt = Events.Where (e => e.ID == id).First ();
			if (evt == null) {
				throw new UnityException ("沒有這個事件，程式有誤，請檢查:"+id);
			}
			return evt;
		}

		public void MarkEventRead(int id){
			userSettings.MarkRead ("Event"+id);
			userSettings.Save ();
		}

		public static bool IsInSatSunDay(DateTime date){
			return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
		}

		public IEnumerable<RemixApi.Event> UnreadEvents {
			get {
				return from e in Events
						where userSettings.IsMarkRead ("Event" + e.ID) == false
					select e;
			}
		}
		#endregion
	}
}

