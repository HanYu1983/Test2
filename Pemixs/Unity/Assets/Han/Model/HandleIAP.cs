using System;
using UnityEngine;
using System.Collections.Specialized;
using System.Collections;
using LitJson;
using System.Collections.Generic;
using System.Linq;

namespace Remix
{
	public class HandleIAP : MonoBehaviour
	{
		public Native native;

		public event Action<string, string> OnPurchaseOK = delegate{};
		public event Action<string, string> OnPurchaseFail = delegate{};
		public event Action<Exception> OnException = delegate{};
		public event Action<JsonData> OnReceviceResponse = delegate{};

		public void Init ()
		{
			#if UNITY_EDITOR
			#elif UNITY_ANDROID
			native.OnNativeCommand += AndroidOnNativeCommand;
			#else
			native.OnNativeCommand += IOSOnNativeCommand;
			#endif
		}

		#region ios iap

		const string IOS_IAP_SKU_PREFIX = "com.testhan.remix.";

		public void IOSRequestProductData ()
		{
			var skus = new string[IAPDefCht.ID_COUNT];
			for (var i = 0; i < IAPDefCht.ID_COUNT; ++i) {
				var def = IAPDefCht.Get (i);
				var sku = IOS_IAP_SKU_PREFIX + def.ID;
				skus [i] = "sku=" + WWW.EscapeURL (sku);
			}
			var skusQuery = string.Join ("&", skus);
			var cmd = "?cmd=IAP.requestProductData&" + skusQuery;
			native.Command (cmd);
		}

		public void IOSPurchase (string sku)
		{
			var cmd = "?cmd=IAP.purchase&sku=" + WWW.EscapeURL(IOS_IAP_SKU_PREFIX + sku);
			native.Command (cmd);
		}

		public void IOSRestoreCompletedTransactions(){
			var cmd = "?cmd=IAP.restoreCompletedTransactions";
			native.Command (cmd);
		}

		void IOSOnNativeCommand (string cmd, NameValueCollection querys)
		{
			switch (cmd) {
			case "IAP.didReceiveResponse":
				{
					var response = querys.GetValues ("response") [0];
					var json = JsonMapper.ToObject (response);
					OnReceviceResponse (json);
				}
				break;
			case "IAP.onTransactionFailed":
				{
					var sku = querys.GetValues ("sku") [0].Replace(IOS_IAP_SKU_PREFIX, "");
					var errcode = querys.GetValues ("errcode") [0];
					OnPurchaseFail (sku, errcode);
				}
				break;
			case "IAP.onTransactionPurchased":
			case "IAP.onTransactionRestored":
				{
					var sku = querys.GetValues ("sku") [0].Replace(IOS_IAP_SKU_PREFIX, "");
					var receipt = querys.GetValues ("receipt") [0];
					OnPurchaseOK (sku, receipt);
				}
				break;
			case "IAP.onException":
				{
					var reason = querys.GetValues ("reason") [0];
					OnException (new UnityException (reason));
				}
				break;
			case "":
				break;
			}
		}

		#endregion


		#region android iab

		public event Action OnAndroidServiceConnected = delegate{};
		public event Action<string, ActivityResult> OnAndroidActivityResult = delegate{};
		public event Action<Exception> OnAndroidException = delegate{};
		public event Action<string> OnAndroidConsumePurchaseOK = delegate{};
		public event Action<IEnumerable<JsonData>> OnAndroidGetSkuDetailsResult = delegate{};
		public event Action<AndroidPurchasesResult> OnAndroidGetPurchasesResult = delegate{};

		public void AndroidBindService ()
		{
			var cmd = "?cmd=IAB.bindService";
			native.Command (cmd);
		}

		public void AndroidGetBuyIntent (string sku)
		{
			var cmd = "?cmd=IAB.getBuyIntent&requestCode=0&developerPayload=&type=inapp&sku=" + sku;
			native.Command (cmd);
		}

		public void AndroidGetSkuDetails ()
		{
			var skus = new string[IAPDefCht.ID_COUNT];
			for (var i = 0; i < IAPDefCht.ID_COUNT; ++i) {
				var def = IAPDefCht.Get (i);
				skus [i] = "sku=" + def.ID;
			}
			var skusQuery = string.Join ("&", skus);
			var cmd = "?cmd=IAB.getSkuDetails&type=inapp&" + skusQuery;
			native.Command (cmd);
		}

		public void AndroidGetPhuchases (string continuationToken)
		{
			var cmd = "?cmd=IAB.getPurchases&type=inapp";
			if (continuationToken != null) {
				cmd += "&continuationToken=" + WWW.EscapeURL (continuationToken);
			}
			native.Command (cmd);
		}

		public void AndroidConsumePurchase (string sku, string purchaseToken)
		{
			var cmd = "?cmd=IAB.consumePurchase&sku=" + sku + "&purchaseToken=" + WWW.EscapeURL (purchaseToken);
			native.Command (cmd);
		}

		public class ActivityResult
		{
			public int RESPONSE_CODE{ get; set; }

			public PurchaseData INAPP_PURCHASE_DATA{ get; set; }

			public string INAPP_DATA_SIGNATURE{ get; set; }
		}

		public class PurchaseData
		{
			public string packageName{ get; set; }

			public string productId{ get; set; }

			public long purchaseTime{ get; set; }

			public int purchaseState{ get; set; }

			public string purchaseToken{ get; set; }
		}

		public class AndroidPurchasesResult
		{
			public List<string> INAPP_PURCHASE_ITEM_LIST{ get; set; }

			public List<PurchaseData> INAPP_PURCHASE_DATA_LIST{ get; set; }

			public List<string> INAPP_DATA_SIGNATURE_LIST{ get; set; }

			public string INAPP_CONTINUATION_TOKEN{ get; set; }
		}

		void AndroidOnNativeCommand (string cmd, NameValueCollection querys)
		{
			switch (cmd) {
			case "IAB.onServiceConnected":
				{
					OnAndroidServiceConnected ();
				}
				break;
			case "IAB.getPurchasesResult":
				{
					try {
						var jsonstr = querys.GetValues ("result") [0];
						Util.Instance.Log("getPurchasesResult:"+jsonstr);			
						var result = JsonMapper.ToObject<AndroidPurchasesResult> (jsonstr);
						OnAndroidGetPurchasesResult (result);
					} catch (Exception e) {
						OnAndroidException (e);
					}
				}
				break;
			case "IAB.getSkuDetailsResult":
				{
					// ignore
				}
				break;
			case "IAB.onActivityResult":
				{
					try {
						var requestCode = querys.GetValues ("requestCode") [0];
						var resultCode = querys.GetValues ("resultCode") [0];
						var data = querys.GetValues ("data") [0];
						Util.Instance.Log("onActivityResult:"+data);

						OnAndroidActivityResult (resultCode, JsonMapper.ToObject<ActivityResult> (data));
					} catch (Exception e) {
						OnAndroidException (e);
					}
				}
				break;
			case "IAB.consumePurchaseResult":
				{
					var sku = querys.GetValues ("sku") [0];
					OnAndroidConsumePurchaseOK (sku);
				}
				break;
			case "IAB.onException":
				{
					var reason = querys.GetValues ("reason") [0];
					OnAndroidException (new UnityException (reason));
				}
				break;
			case "":
				break;
			}
		}

		#endregion
	}
}

