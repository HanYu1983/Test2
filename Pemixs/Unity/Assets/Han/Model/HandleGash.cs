using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Remix
{
	public struct InvokeSDKResult {
		public string TOKEN;
		public string RCODE;
		public bool IsSuccess {
			get {
				return RCODE == "0000";
			}
		}
	}

	public class HandleGash : MonoBehaviour
	{
		public event Action<Exception, InvokeSDKResult> OnInvokeSDK = delegate{};

		public HandleDebug debug;
		public Native native;
		public DeviceData deviceData;

		public void Init(){
			native.OnNativeCommand += Native_OnNativeCommand;
		}

		void Native_OnNativeCommand (string cmd, System.Collections.Specialized.NameValueCollection querys)
		{
			switch (cmd) {
			case "Gash.invokeSDK.callback":
				{
					var json = querys.Get("json");
					Debug.Log (json);
					var result = JsonUtility.FromJson<InvokeSDKResult> (json);
					OnInvokeSDK (null, result);
				}
				break;
			}
		}

		public IEnumerator NewOrderAndInvokeSDK(float amount, string item, Action<Exception> cb){
			yield return RemixApi.GashNewOrder (deviceData.DeviceID, amount, item, (e, result) => {
				if(e!=null){
					cb(e);
					return;
				}
				var cmd = string.Format (
					"?cmd={0}&token={1}&coid={2}", 
					"Gash.invokeSDK",
					WWW.EscapeURL(result.token), WWW.EscapeURL(result.coid)
				);
				native.Command (cmd);
				cb(null);
			});
		}

		public IEnumerator QueryOrder(Action<Exception> cb){
			yield return RemixApi.GashQuery (deviceData.DeviceID, cb);
		}

		public IEnumerator QueryAndGetItem(Action<Exception, RemixApi.GashOrder> eachOrder, Action<Exception> cb){
			Exception e = null;
			yield return RemixApi.GashQuery (deviceData.DeviceID, (e2)=>{
				e = e2;
			});
			if (e != null) {
				cb (e);
				yield break;
			}
			var orders = new List<RemixApi.GashOrder>();
			yield return RemixApi.GashGetItem (deviceData.DeviceID, (e2, res)=>{
				e = e2;
				orders = res;
			});
			if (e != null) {
				cb (e);
				yield break;
			}
			foreach (var order in orders) {
				yield return RemixApi.GashMarkItem (order.id, (e2)=>{
					eachOrder (e2, order);
				});
			}
			cb (null);
		}
	}
}

