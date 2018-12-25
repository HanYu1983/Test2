using System;
using System.Net;
using System.Text;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using LitJson;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using UnityEngine.Networking;
using System.Collections;
using UniRx;
using System.Collections.Specialized;

namespace Remix
{
	public class RemixApi
	{
		#if LOCAL_SERVER1
		public const string API_HOST = "http://localhost:8080";
		#elif GCM1
		public const string API_HOST = "http://104.197.72.117"; // google compute engine
		

#else
		public const string API_HOST = "http://localhost:8080"; // hicloud
		#endif

		// Authorization的Header不能使用，所以改用Authorization2
		const string AUTH_HEADER = "Authorization2";

		public struct DeviceInfo
		{
			public string ID;
			public string Name;
			public string InviteCode;
			public string TransferCode;
		}

		public static DeviceInfo GetDevice (string name)
		{
			DeviceInfo info;
            info.ID = "0";
			info.Name = "han";
            info.InviteCode = "0";
            info.TransferCode = "0";
            return info;
		}

		public static void UpdateName (string deviceId, string name)
		{
			var res = HttpRequest ("/api/device/" + deviceId + "/updateName", "name=" + WWW.EscapeURL (name));
			var jo = JsonMapper.ToObject (res);
			if (jo [0] != null) {
				throw new UnityException ((string)jo [0]);
			}
		}

		public static bool IsDeviceEnable (string deviceId, ref string msg)
		{
            return true;
		}

		public static void Save (string deviceId, string data)
		{
			var encodeBody = WWW.EscapeURL (data);
			var res = HttpRequest ("/api/device/" + deviceId + "/record", "data=" + encodeBody);
			var jo = JsonMapper.ToObject (res);
			if (jo [0] != null) {
				throw new UnityException ((string)jo [0]);
			}
		}

		public static string Load (string deviceId)
		{
			var res = HttpRequest ("/api/device/" + deviceId + "/record", null);
			var jo = JsonMapper.ToObject (res);
			if (jo [0] != null) {
				throw new UnityException ((string)jo [0]);
			}
			if (jo [1] == null) {
				throw new UnityException ("還沒建檔");
			}
			return (string)jo [1] ["json"];
		}

		public class Mail
		{
			public string ID;
			public bool Unread;
			public string Title;
			public string Description;
			public string Gift;
			public int GiftCount;

			public bool IsDailyGift {
				get {
					return Description.Contains ("Mg");
				}
			}

			public bool IsInviteGift {
				get {
					return Description.Contains ("invite gift");
				}
			}
		}

		public static IEnumerable<Mail> LoadMail (string deviceId)
		{
			var ret = new List<Mail> ();
			return ret;
		}

		public static int GetUserGotGiftCountInMonth (string deviceId)
		{
            return 0;
		}

		public static void MarkMailRead (string deviceId, string mailId)
		{
            
		}

		public class Event
		{
			public int ID;
			public int ConditionID;
			public string Title;
			public string Description;
		}

		public static IEnumerable<Event> LoadEvent ()
		{
			var ret = new List<Event> ();
			return ret;
		}

		public static bool CheckInvite (string deviceId, string inviteCode)
		{
            return false;
		}

		public static void Invite (string deviceId, string inviteCode)
		{
            return ;
		}

		public static int InviteCount (string deviceId)
		{
            return 0;
		}

		public static bool CheckTransfer (string deviceId, string transferCode, string transferName)
		{
            return false;
		}

		public static string Transfer (string deviceId, string transferCode, string transferName)
		{
			return "";
		}

		public static IEnumerator HttpRequestAsync (string uri, string encodeBody, Action<Exception, string> cb)
		{
			var url = API_HOST + uri;
			var auth = RemixApi.GetAuthorizationString (uri);
			var result = new Either<byte[]> ();
			yield return URLRequestAsync (result, url, auth, null);
			if (result.Exception != null) {
				cb (result.Exception, null);
				yield break;
			}
			var resultStr = System.Text.Encoding.UTF8.GetString (result.Ref);
			cb (null, resultStr);
		}

		public struct GashAuthResult
		{
			public string token;
			public string coid;
			public static GashAuthResult Empty;
		}

		public static IEnumerator GashNewOrder (string deviceId, float amount, string item, Action<Exception, GashAuthResult> cb)
		{
            yield return null;
            GashAuthResult result;
            result.token = "0";
            result.coid = "0";
            cb(null, result);
            /*
            var url = string.Format (
				          "/api/gash/device/{0}/newOrder/{1}/{2}", 
				          deviceId, amount, item
			          );
			yield return HttpRequestAsync (url, null, (e, res) => {
				if (e != null) {
					cb (e, GashAuthResult.Empty);
					return;
				}
				var jo = JsonMapper.ToObject (res);
				if (jo [0] != null) {
					cb (new UnityException ((string)jo [0]), GashAuthResult.Empty);
					return;
				}
				try {
					GashAuthResult result;
					result.token = (string)jo [1] ["token"];
					result.coid = (string)jo [1] ["coid"];
					cb (null, result);
				} catch (Exception e2) {
					cb (new UnityException ("data format is not valid, can not transfer", e2), GashAuthResult.Empty);
				}
			});
            */
		}

		public static IEnumerator GashQuery (string deviceId, Action<Exception> cb)
		{
            yield return null;
            cb(null);
            /*
			var url = string.Format (
				          "/api/gash/device/{0}/query", 
				          deviceId
			          );
			yield return HttpRequestAsync (url, null, (e, res) => {
				if (e != null) {
					cb (e);
					return;
				}
				var jo = JsonMapper.ToObject (res);
				if (jo [0] != null) {
					cb (new UnityException ((string)jo [0]));
					return;
				}
				cb (null);
			});
            */
		}

		public struct GashOrder
		{
			public string item;
			public string id;
		}

		public static IEnumerator GashGetItem (string deviceId, Action<Exception, List<GashOrder>> cb)
		{
            yield return null;
            var ret = new List<GashOrder>();
            cb(null, ret);
            /*
            var url = string.Format (
				          "/api/gash/device/{0}/getItem", 
				          deviceId
			          );
			yield return HttpRequestAsync (url, null, (e, res) => {
				if (e != null) {
					cb (e, null);
					return;
				}
				var jo = JsonMapper.ToObject (res);
				if (jo [0] != null) {
					cb (new UnityException ((string)jo [0]), null);
					return;
				}
				var ret = new List<GashOrder> ();
				for (var i = 0; i < jo [1].Count; ++i) {
					var joo = jo [1] [i];
					GashOrder order;
					order.id = (string)jo [1] [i] ["id"];
					order.item = (string)jo [1] [i] ["item"];
					ret.Add (order);
				}
				cb (null, ret);
			});
            */
		}

		public static IEnumerator GashMarkItem (string id, Action<Exception> cb)
		{
            yield return null;
            cb(null);
            /*
            var url = string.Format (
				         "/api/gash/markItem/{0}", 
				         id
			         );
			yield return HttpRequestAsync (url, null, (e, res) => {
				if (e != null) {
					cb (e);
					return;
				}
				var jo = JsonMapper.ToObject (res);
				if (jo [0] != null) {
					cb (new UnityException ((string)jo [0]));
					return;
				}
				cb (null);
			});
            */
		}

		public static string HttpRequest (string uri, string encodeBody)
		{
			var bytes = URLRequest (API_HOST + uri, RemixApi.GetAuthorizationString (uri), encodeBody);
			var result = System.Text.Encoding.UTF8.GetString (bytes);
			return result;
		}

		public static UnityWebRequest CreateURLRequest (string url, string auth, string encodeBody)
		{
			Util.Instance.Log (url);
			UnityWebRequest request = null;
			if (encodeBody != null) {
				if (encodeBody.Length > 0) {
					// 空字串呼叫這段可能會有以下錯誤
					// System.ArgumentException: Cannot create a data handler without payload data
					request = new UnityWebRequest (url, UnityWebRequest.kHttpVerbPOST);
					byte[] bytes = Encoding.UTF8.GetBytes (encodeBody);
					UploadHandlerRaw uH = new UploadHandlerRaw (bytes);
					uH.contentType = "application/x-www-form-urlencoded"; //this is ignored?
					request.uploadHandler = uH;
					request.downloadHandler = new DownloadHandlerBuffer ();
				} else {
					request = UnityWebRequest.Post (url, new WWWForm ());
				}
			} else {
				request = UnityWebRequest.Get (url);
			}
			request.SetRequestHeader (AUTH_HEADER, auth);
			return request;
		}

		public static byte[] URLRequest (string url, string auth, string encodeBody)
		{
			UnityWebRequest request = CreateURLRequest (url, auth, encodeBody);
			request.Send ();
			while (!request.isDone) {
				System.Threading.Thread.Sleep (TimeSpan.FromSeconds (0.3f));
			}
			if (request.isNetworkError) {
				throw new UnityException (request.error);
			}
			return request.downloadHandler.data;
		}

		public static string GetAuthorizationString (string uri)
		{
			var timestamp = GameConfig.ConvertToUnixTimestamp (DateTime.Now);
			var secret = "dcc8fb4d1bd446f5a7f7892736f97df4";
			var originStr = string.Format ("{0}\n{1}\n{2}", secret, timestamp, uri).ToLower ();

			MD5 md5 = new MD5CryptoServiceProvider ();
			var source = Encoding.Default.GetBytes (originStr);
			byte[] crypto = md5.ComputeHash (source);
			var sig = Convert.ToBase64String (crypto);

			var auth = string.Format ("sig={0}&timestamp={1}&uri={2}",
				           WWW.EscapeURL (sig),
				           timestamp,
				           WWW.EscapeURL (uri)
			           );
			return auth;
		}

		public static Texture2D GetPhoto (string photoPath, string photoName, bool useCache = true)
		{
			var fullPath = photoPath + photoName;
			Util.Instance.Log ("GetPhoto:" + fullPath);
			byte[] bytes = null;
			// 先讀取本地
			if (useCache) {
				var photoNameEscape = WWW.EscapeURL (photoName);
				var dir = GameConfig.LOCAL_PHOTOS_PATH;
				var localPath = Path.Combine (dir, photoNameEscape);
				bytes = LoadBytesFromLocal (localPath);
				if (bytes != null) {
					Debug.LogWarning ("本機bundle已存在:" + localPath);
					Texture2D tex = new Texture2D (2, 2);
					tex.LoadImage (bytes);
					return tex;
				}
			}
			{
				bytes = URLRequest (fullPath, GetAuthorizationString ("Photos"), null);
				// cache photo
				var photoNameEscape = WWW.EscapeURL (photoName);
				var dir = GameConfig.LOCAL_PHOTOS_PATH;
				if (Directory.Exists (dir) == false) {
					Directory.CreateDirectory (dir);
				}
				var localPath = Path.Combine (dir, photoNameEscape);
				SaveBundleToLocal (localPath, bytes);

				// Create a texture. Texture size does not matter, since
				// LoadImage will replace with with incoming image size.
				Texture2D tex = new Texture2D (2, 2);
				tex.LoadImage (bytes);
				return tex;
			}
		}

		public static bool CheckBundleUpdate (string bundlePath, string bundleName)
		{
			var manifestName = bundleName + ".manifest";
			var fullPath = bundlePath + manifestName;
			Util.Instance.Log ("LoadAssetBundleManifest:" + fullPath);

			byte[] bytes = null;
			string remoteCrc = null;
			string localCrc = null;
			string pattern = @"CRC:\s(?<crc>\d+)[\r\n]";
			// 先取得Remote CRC Code
			{
				bytes = URLRequest (fullPath, GetAuthorizationString ("AssetBundles"), null);
				var txt = Encoding.ASCII.GetString (bytes);
				Regex regex = new Regex (pattern, RegexOptions.IgnoreCase);
				MatchCollection matches = regex.Matches (txt);
				foreach (Match match in matches) {
					GroupCollection groups = match.Groups;
					remoteCrc = groups ["crc"].Value;
					break;
				}
			}

			// 必須要有CRC code
			if (remoteCrc == null) {
				throw new ShowMessageException ("crc code not found");
			}
			// 讀取本機CRC code
			var bundleNameEscape = WWW.EscapeURL (manifestName);
			var dir = GameConfig.LOCAL_ASSET_BUNDELS_PATH;
			var localPath = dir + bundleNameEscape;
			Util.Instance.Log ("LoadLocalAssetBundleManifest:" + localPath);

			var isExistLocalManifest = File.Exists (localPath);
			// 如果本機檔案不存在，就直接下載Package
			if (isExistLocalManifest == false) {
				Util.Instance.LogWarning ("本機檔案不存在，就直接下載Package:" + localPath);
				SaveBundleToLocal (localPath, bytes);
				return true;
			}
			// 讀取本機CRC code
			{
				var localBytes = LoadBytesFromLocal (localPath);
				var txt = Encoding.ASCII.GetString (localBytes);
				Regex regex = new Regex (pattern, RegexOptions.IgnoreCase);
				MatchCollection matches = regex.Matches (txt);
				foreach (Match match in matches) {
					GroupCollection groups = match.Groups;
					localCrc = groups ["crc"].Value;
					break;
				}
			}
			// 如果本機CRC code不存在，就直接下載Package
			if (localCrc == null) {
				Util.Instance.LogWarning ("本機CRC code不存在，就直接下載Package:" + manifestName);
				Util.Instance.Log ("SaveLocalAssetBundleManifest:" + localPath);
				SaveBundleToLocal (localPath, bytes);
				return true;
			}

			// 如果CRC code不一樣，就下載Package
			if (localCrc != remoteCrc) {
				Util.Instance.LogWarning ("CRC local/remote:" + localCrc + "/" + remoteCrc);
				Util.Instance.LogWarning ("CRC code不一樣，就直接下載Package:" + manifestName);
				Util.Instance.Log ("SaveLocalAssetBundleManifest:" + localPath);
				SaveBundleToLocal (localPath, bytes);
				return true;
			}
			return false;
		}

		// 這個方法可以用Thread
		public static byte[] LoadAssetBundleBytes (string bundlePath, string bundleName, bool useCache = true, bool withManifest = true)
		{
			var fullPath = bundlePath + bundleName;
			Util.Instance.Log ("LoadAssetBundle:" + fullPath);
			byte[] bytes = null;
			// 先讀取本地
			if (useCache) {
				var bundleNameEscape = WWW.EscapeURL (bundleName);
				var dir = GameConfig.LOCAL_ASSET_BUNDELS_PATH;
				var localPath = dir + bundleNameEscape;
				bytes = LoadBytesFromLocal (localPath);
				if (bytes != null) {
					Util.Instance.LogWarning ("本機bundle已存在:" + localPath);
					return bytes;
				}
			}
			if (withManifest) {
				bytes = URLRequest (fullPath + ".manifest", GetAuthorizationString ("AssetBundles"), null);
				var bundleNameEscape = WWW.EscapeURL (bundleName + ".manifest");
				var dir = GameConfig.LOCAL_ASSET_BUNDELS_PATH;
				var localPath = dir + bundleNameEscape;
				SaveBundleToLocal (localPath, bytes);
			}

			{
				bytes = URLRequest (fullPath, GetAuthorizationString ("AssetBundles"), null);
				var bundleNameEscape = WWW.EscapeURL (bundleName);
				var dir = GameConfig.LOCAL_ASSET_BUNDELS_PATH;
				if (Directory.Exists (dir) == false) {
					Directory.CreateDirectory (dir);
				}
				var localPath = dir + bundleNameEscape;
				SaveBundleToLocal (localPath, bytes);
				return bytes;
			}
		}

		public static byte[] LoadBytesFromLocal (string localPath)
		{
			var isExist = File.Exists (localPath);
			if (isExist) {
				var fileInfo = new System.IO.FileInfo (localPath);
				using (var fs = File.OpenRead (localPath)) {
					return GameConfig.GetContent (fs, fileInfo.Length);
				}
			}
			return null;
		}

		public static void DeleteBytesFromLocal (string localPath)
		{
			var isExist = File.Exists (localPath);
			if (isExist) {
				File.Delete (localPath);
			}
		}

		public static void SaveBundleToLocal (string localPath, byte[] bytes)
		{
			using (var fs = File.OpenWrite (localPath)) {
				fs.Write (bytes, 0, bytes.Length);
			}
		}

		#region async

		public class Either<T>
		{
			public Exception Exception{ get; set; }

			T refobj;

			public T Ref { 
				get {
					if (Exception != null) {
						throw Exception;
					}
					return refobj;
				}
				set {
					refobj = value;
				} 
			}

			public bool IsDone {
				get {
					return Exception != null || refobj != null;
				}
			}
		}

		public static IEnumerator URLRequestAsync (Either<byte[]> after, string uri, string auth, string encodeBody)
		{
			Util.Instance.Log (uri);
			UnityWebRequest request = null;
			if (encodeBody != null) {
				WWWForm form = new WWWForm ();
				var result = new NameValueCollection ();
				Native.ParseQueryString ("?" + encodeBody, Encoding.UTF8, result);
				foreach (var key in result.AllKeys) {
					var vs = result.GetValues (key);
					foreach (var v in vs) {
						form.AddField (key, v);
					}
				}
				request = UnityWebRequest.Post (uri, form);
			} else {
				request = UnityWebRequest.Get (uri);
			}
			request.SetRequestHeader (AUTH_HEADER, auth);
			yield return request.Send ();
			if (request.isNetworkError) {
				after.Exception = new ShowMessageException (request.error);
				yield break;
			}
			after.Ref = request.downloadHandler.data;
		}

		public static IEnumerator LoadAssetBundleBytesAsync (Either<byte[]> result, string bundlePath, string bundleName, bool useCache = true, bool withManifest = true)
		{
			yield return null;

			var fullPath = bundlePath + bundleName;
			Util.Instance.Log ("LoadAssetBundle:" + fullPath);
			byte[] bytes = null;
			// 先讀取本地
			if (useCache) {
				var bundleNameEscape = WWW.EscapeURL (bundleName);
				var dir = GameConfig.LOCAL_ASSET_BUNDELS_PATH;
				var localPath = dir + bundleNameEscape;
				bytes = LoadBytesFromLocal (localPath);
				if (bytes != null) {
					Util.Instance.LogWarning ("本機bundle已存在:" + localPath);
					result.Ref = bytes;
					yield break;
				}
			}
			if (withManifest) {
				var answer = new Either<byte[]> ();
				yield return URLRequestAsync (answer, fullPath + ".manifest", RemixApi.GetAuthorizationString (""), null);
				if (answer.Exception != null) {
					result.Exception = answer.Exception;
					yield break;
				}
				var bundleNameEscape = WWW.EscapeURL (bundleName + ".manifest");
				var dir = GameConfig.LOCAL_ASSET_BUNDELS_PATH;
				var localPath = dir + bundleNameEscape;
				SaveBundleToLocal (localPath, answer.Ref);
			}

			{
				var answer = new Either<byte[]> ();
				yield return URLRequestAsync (answer, fullPath, RemixApi.GetAuthorizationString (""), null);
				if (answer.Exception != null) {
					result.Exception = answer.Exception;
					yield break;
				}
				var bundleNameEscape = WWW.EscapeURL (bundleName);
				var dir = GameConfig.LOCAL_ASSET_BUNDELS_PATH;
				if (Directory.Exists (dir) == false) {
					Directory.CreateDirectory (dir);
				}
				var localPath = dir + bundleNameEscape;
				SaveBundleToLocal (localPath, answer.Ref);
				result.Ref = answer.Ref;
				yield break;
			}
		}

		#endregion
	}
}

