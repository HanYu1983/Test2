using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Specialized;
using System.Text;
using System;

namespace HanUtil{
	public class Native : MonoBehaviour {

		public HandleDebug handleDebug;
		public event Action<string, NameValueCollection> OnNativeCommand = delegate {};

		#if UNITY_IPHONE
		[DllImport ("__Internal")]
		static extern float _Command(string url);
		#endif

		public void Command(string queryString){
			handleDebug.Log ("callNative:" + queryString);
			#if UNITY_ANDROID
			try{
				using (AndroidJavaClass jo = new AndroidJavaClass("org.han.unity.UnityBinder")){
					jo.CallStatic("command", queryString);
				}
			}catch(System.Exception e){
				handleDebug.LogWarning("callNative Exception:"+e.Message);
			}
			#elif UNITY_IPHONE
			try{
			_Command (queryString);
			}catch(Exception e){
			handleDebug.LogWarning("callNative Exception:"+e.Message);
			}
			#endif
		}

		public void onNativeCommand(string queryString){
			handleDebug.Log ("onNativeCommand:"+queryString);
			var result = new NameValueCollection();
			ParseQueryString (queryString, Encoding.UTF8, result);
			var cmd = result.GetValues ("cmd")[0];
			handleDebug.Log ("onNativeCommand:"+cmd);
			OnNativeCommand (cmd, result);
		}
		// ?cmd=IAP.didReceiveResponse&response=%7B'products'%3A%5B%5D,%20'invalidProductIdentifiers'%3A%5Biap01,iap03,iap05,iap07,iap09,iap02,iap04,iap06,iap08%5D%7D
		public static void ParseQueryString(string query, Encoding encoding, NameValueCollection result){
			if (query.Length == 0)
				return;

			string decoded = WWW.UnEscapeURL(query, encoding);
			int decodedLength = decoded.Length;
			int namePos = 0;
			bool first = true;
			while (namePos <= decodedLength)
			{
				int valuePos = -1, valueEnd = -1;
				for (int q = namePos; q < decodedLength; q++)
				{
					if (valuePos == -1 && decoded[q] == '=')
					{
						valuePos = q + 1;
					}
					else if (decoded[q] == '&')
					{
						valueEnd = q;
						break;
					}
				}

				if (first)
				{
					first = false;
					if (decoded[namePos] == '?')
						namePos++;
				}

				string name, value;
				if (valuePos == -1)
				{
					name = null;
					valuePos = namePos;
				}
				else
				{
					name = WWW.UnEscapeURL(decoded.Substring(namePos, valuePos - namePos - 1), encoding);
				}
				if (valueEnd < 0)
				{
					namePos = -1;
					valueEnd = decoded.Length;
				}
				else
				{
					namePos = valueEnd + 1;
				}
				value = WWW.UnEscapeURL(decoded.Substring(valuePos, valueEnd - valuePos), encoding);
				result.Add(name, value);
				if (namePos == -1)
					break;
			}
		}
	}
}