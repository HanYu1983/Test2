using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using UnityEngine.UI;
using System.Threading;
using System.Linq;
using UnityEngine.Networking;
using ICSharpCode.SharpZipLib.Zip;

namespace Remix
{
	public class HandleAssetBundle : MonoBehaviour
	{
		public string assetBundlePath;
		public string platform;
		public bool simulation;
		public List<string> loadedBundleName;
		public Dictionary<string, AssetBundle> bundlePool = new Dictionary<string, AssetBundle> ();

		void Awake(){
			assetBundlePath = RemixApi.API_HOST+"/AssetBundles/";
#if UNITY_IOS
            platform = "iOS";
#elif UNITY_ANDROID
            platform = "Android";
#else
            platform = "Windows";
#endif
        }

        public IEnumerator DownloadAssetBundle(string bundleName, Action<float> onProgress, Action<Exception> onDone){
			if (simulation) {
				if (onDone != null) {
					onDone (null);
				}
				yield break;
			}
			var fileNames = new List<string> (){ 
				bundleName, 
				bundleName+".manifest" 
			};
			foreach (var fileName in fileNames) {
				var fullPath = assetBundlePath + platform + "/" + fileName;
				UnityWebRequest request = RemixApi.CreateURLRequest (fullPath, RemixApi.GetAuthorizationString (""), null);
				// request.Send ();
                request.SendWebRequest();
				while (true) {
					if (request.isDone) {
						if (request.isNetworkError) {
							if (onDone != null) {
								onDone (new UnityException (request.error));
							}
							yield break;
						} else {
							break;
						}
					}
					if (onProgress != null) {
						var progress = request.downloadProgress;
						onProgress (progress);
					}
					yield return new WaitForSeconds (0.1f);
				}
				var localPath = GameConfig.LOCAL_ASSET_BUNDELS_PATH + WWW.EscapeURL (fileName);
				var bytes = request.downloadHandler.data;
				RemixApi.SaveBundleToLocal (localPath, bytes);
			}
			if (onDone != null) {
				onDone (null);
			}
		}

		public bool IsSimulation{ get { return simulation; } }

		public UnityEngine.Object LoadAsset(string path, Type type){
			var isAssetBundle = path.Contains (";");
			if (isAssetBundle) {
				var info = path.Split (';');
				var bundleName = info [0];
				var assetName = info [1];
				return LoadAsset (bundleName, assetName, type);
			} else {
				var ret = Resources.Load (path);
				return ret;
			}
		}

		public UnityEngine.Object LoadAsset(string bundleName, string assetName, Type type){
			Util.Instance.Log ("LoadAsset:"+bundleName+";"+assetName);
			if (simulation) {
				return AssetBundleUtil.LoadAsset (null, bundleName, assetName, type);
			}
			try{
				var fullPath = assetBundlePath + platform + "/";
				var manifestBundle = AssetBundleUtil.LoadAssetBundleWithDependency(fullPath, bundlePool, null, platform);
				var manifest = AssetBundleUtil.LoadManifest (manifestBundle);
				var bundle = AssetBundleUtil.LoadAssetBundleWithDependency (fullPath, bundlePool, manifest, bundleName);
				var obj = bundle.LoadAsset (assetName, type);
				// 使用AssetBundle載入的物件有可能會遺失材質，所以將材質重新加入一次
				ResetShader (obj);
				UpdateBundleName ();
				return obj;
			}catch(Exception e){
				throw new ShowMessageException (e.Message, e);
			}
		}

		public static void ResetShader(UnityEngine.Object obj){
			var listMat = new List<Material>();
			listMat.Clear();
			if (obj is Material){
				var m = obj as Material;
				listMat.Add(m);
			}else if (obj is GameObject){
				var go = obj as GameObject;
				var rends = go.GetComponentsInChildren<Renderer>();
				if (null != rends){
					foreach (var item in rends){
						var materialsArr = item.sharedMaterials;
						foreach (var m in materialsArr) {
							listMat.Add(m);
						}
					}
				}
				var imgs = go.GetComponentsInChildren<Image> ();
				if (null != imgs){
					foreach (var item in imgs){
						listMat.Add (item.material);
					}
				}

				for (int i = 0; i < listMat.Count; i++){
					var m = listMat[i];
					if (null == m) {
						continue;
					}
					var shaderName = m.shader.name;
					var newShader = Shader.Find(shaderName);
					if (newShader != null) {
						m.shader = newShader;
					}
				}
			}
		}

		public bool IsNeedDownloadAssetBundleAndSetDownloadFlag(){
			if (simulation) {
				return false;
			}
			// first assetbundle load in main thread
			var fullPath = assetBundlePath + platform + "/";
			var shouldDownload = RemixApi.CheckBundleUpdate (fullPath, platform);
			if (shouldDownload == false) {
				// 另外再檢查完整性
				var bytes = RemixApi.LoadAssetBundleBytes (fullPath, platform, false, false);
				try{
					var manifestBundle = AssetBundle.LoadFromMemory (bytes);
					manifestBundle.Unload(true);
				}catch(Exception){
					var fileName = GameConfig.LOCAL_ASSET_BUNDELS_PATH + WWW.EscapeURL(platform);
					Util.Instance.LogError ("檔案不完整，刪除:" + fileName);
					RemixApi.DeleteBytesFromLocal (fileName);
					return true;
				}
			}
			return shouldDownload;
		}

		// 第一次呼叫DownloadAllAssetBundlesAtFirstTimeZipFileVersion時checkFile必須為false
		public IEnumerator DownloadAllAssetBundlesAtFirstTimeZipFileVersion(bool checkFile, Action onPassCheck, Action<string, long, long> onProgress, Action<Exception> onDone){
			if (simulation) {
				onDone (null);
				yield break;
			}

			var savePath = GameConfig.LOCAL_TEMP_PATH + platform + ".zip";
			var fullPath = assetBundlePath + platform + ".zip";

			// 預設都要重新下載
			var needDownload = true;
			if (checkFile) {
				// 檔案存在代表下載到一半被中斷了
				// 反之則是成功下載並解壓完成
				// 注意：第一次使用這個方法時不該進入這裡(checkFile = true)
				needDownload = File.Exists (savePath);
			}

			if (needDownload == false) {
				File.Delete (savePath);
				onPassCheck ();
				onDone (null);
				yield break;
			}

			var requestAssetBundle = RemixApi.CreateURLRequest (fullPath, RemixApi.GetAuthorizationString ("AssetBundle"), null);
			requestAssetBundle.downloadHandler = new ToFileDownloadHandler (new byte[64 * 1024], savePath);
			requestAssetBundle.Send ();

			while (true) {
				onProgress ("下載中", (int)(requestAssetBundle.downloadProgress * 100), 100);
				if (requestAssetBundle.isDone == false) {
					yield return new WaitForSeconds (0.3f);
				} else {
					break;
				}
			}

			if (requestAssetBundle.isNetworkError) {
				onDone (new Exception (requestAssetBundle.error));
				yield break;
			}

			using (ZipInputStream s = new ZipInputStream(File.OpenRead(savePath))) {
				ZipEntry theEntry;
				var progress = 0;

				while ((theEntry = s.GetNextEntry()) != null) {
					onProgress ("解壓中", Math.Min(++progress, 499), 500);
					yield return null;
					if (theEntry.Name == String.Empty) {
						continue;
					}
					var distFileName = GameConfig.LOCAL_ASSET_BUNDELS_PATH + WWW.EscapeURL(theEntry.Name);
					using (FileStream streamWriter = File.Create(distFileName)) {
						byte[] data = new byte[2048];
						while (true) {
							int size = s.Read(data, 0, data.Length);
							if (size > 0) {
								streamWriter.Write(data, 0, size);
							} else {
								break;
							}
						}
					}
				}
			}
			File.Delete (savePath);
			onPassCheck ();
			onDone (null);
		}

		// 下載預設bundle
		// 注意：assetBundle中不能包含AnimatorComponent(正確來說是不能引用到Animator的檔案)，否則在實例化包含之物件時會crash!!!
		// 所以將有animator的prefab都移到本地resource中，就不包成assetBundle了
		public IEnumerator DownloadAllAssetBundlesAtFirstTime(LanguageText lt, int lang, bool checkFileDownloadOK, bool checkFileNeedUpdate, Action onPassCheck, Action<string, long, long> onProgress, Action<Exception> onDone){
			if (simulation) {
				onDone (null);
				yield break;
			}
			var skipNames = new List<string> () {
				"dynamic", 
				"event/event0", "event/event1", "event/event2", 
				"level/m01","level/m02","level/m03","level/m04","level/m05","level/m06","level/s01","level/s02",
				"cat/02","cat/03","cat/04","cat/05","cat/06","cat/07","cat/08","cat/09",
				"cat/10","cat/11","cat/12","cat/13","cat/14","cat/15","cat/16","cat/17","cat/18","cat/19",
				"cat/20","cat/21","cat/22","cat/23",
				"map/02","map/03","map/04","map/05","map/06","map/s1","map/s2",
			};

			var shouldDownloadNames = new HashSet<string> ();

			var fullPath = assetBundlePath + platform + "/";
			var bytes = RemixApi.LoadAssetBundleBytes (fullPath, platform);
			var manifestBundle = AssetBundle.LoadFromMemory (bytes);
			var manifest = AssetBundleUtil.LoadManifest (manifestBundle);
			var savePath = GameConfig.LOCAL_ASSET_BUNDELS_PATH;

			var requests = new List<UnityWebRequest> ();
			var bundleNames = manifest.GetAllAssetBundles ();

			for (var i = 0; i < bundleNames.Length; ++i) {
				var name = bundleNames [i];

				var skip = false;
				foreach (var skipName in skipNames) {
					if (name.Contains (skipName)) {
						skip = true;
						break;
					}
				}
				if (skip) {
					Util.Instance.LogWarning ("skip:" + name);
					continue;
				}
				var nameEscape = WWW.EscapeURL (name);

				if (checkFileDownloadOK) {
					onProgress (lt.GetDlgMessage(lang, "MesgText_D02"), i, bundleNames.Length);
					yield return null;

					// 檢查完整性
					var isPerfect = false;
					var bundleBytes = RemixApi.LoadBytesFromLocal (GameConfig.LOCAL_ASSET_BUNDELS_PATH + nameEscape);
					if (bundleBytes != null) {
						try {
							var bundle = AssetBundle.LoadFromMemory (bundleBytes);
							bundle.Unload (true);
							Resources.UnloadUnusedAssets ();
							isPerfect = true;
						} catch (Exception) {
							Util.Instance.LogError ("檔案不完整，重新下載:" + name);
						}
					}
					if (isPerfect) {
						Util.Instance.LogWarning ("此包已存在，略過:" + name);
						continue;
					}
				}
				shouldDownloadNames.Add (name);
			}
			// 記得要Unload掉，不然之後不小心又Load一個同樣的AssetBundle，程式就當了
			manifestBundle.Unload (true);

			if (checkFileNeedUpdate) {
				// 動態下載的部分在這裡有更新的機會
				var dir = new DirectoryInfo (GameConfig.LOCAL_ASSET_BUNDELS_PATH);
				var info = dir.GetFiles ("*.manifest");
				for(var i=0; i<info.Length; ++i){
					var f = info [i];
					var name = WWW.UnEscapeURL(f.Name.Replace (".manifest", ""));
					try{
						var isExpire = RemixApi.CheckBundleUpdate (fullPath, name);
						if (isExpire) {
							shouldDownloadNames.Add (name);
						}
					}catch(Exception e){
						Util.Instance.LogWarning (e.Message);
						// ignore
					}
					onProgress (lt.GetDlgMessage(lang, "MesgText_D03"), i, info.Length);
					yield return null;
				}
			}

			var isPassCheck = shouldDownloadNames.Count == 0;
			if (isPassCheck) {
				onPassCheck ();
				onDone (null);
				yield break;
			}

			foreach (var name in shouldDownloadNames) {
				var manifestName = name + ".manifest";
				var nameEscape = WWW.EscapeURL (name);

				var requestAssetBundle = RemixApi.CreateURLRequest (fullPath + name, RemixApi.GetAuthorizationString ("AssetBundle"), null);
				requestAssetBundle.downloadHandler = new ToFileDownloadHandler (new byte[64 * 1024], savePath + nameEscape);

				var requestAssetBundleManifest = RemixApi.CreateURLRequest (fullPath + manifestName, RemixApi.GetAuthorizationString ("AssetBundle"), null);
				requestAssetBundleManifest.downloadHandler = new ToFileDownloadHandler (new byte[64 * 1024], savePath + nameEscape + ".manifest");

				requests.Add (requestAssetBundle);
				requests.Add (requestAssetBundleManifest);
			}

			foreach (var request in requests) {
				request.Send ();
			}

			Exception hasError = null;
			var count = requests.Count ();
			while (true) {
				var doneCount = requests.Count (r => {
					var isCurrDone = r.isDone;
					if(isCurrDone){
						// 閒置太久進入背景的話，不會有error
						// 但檔案還是下載失敗
						// 卻不會跑到下面這行(不會有例外產生)
						// 這個檢查是最後保險
						if(r.isNetworkError){
							var handler = r.downloadHandler as ToFileDownloadHandler;
							handler.Cancel();
							hasError = new ShowMessageException(r.error);
						}
					}
					return isCurrDone;
				});
				if (hasError != null) {
					break;
				}
				var isDone = doneCount == count;
				if (isDone) {
					break;
				} else {
					onProgress (lt.GetDlgNote(lang, "LoadDlg"), doneCount, count);
					yield return new WaitForSeconds(1f);
				}
			}
			onDone (hasError);
		}

		public void Unload(){
			AssetBundleUtil.UnloadAssetBundle (bundlePool);
			UpdateBundleName ();
		}

		void UpdateBundleName(){
			loadedBundleName.Clear ();
			foreach (var key in bundlePool.Keys) {
				loadedBundleName.Add (key);
			}
		}

#region async
		public IEnumerator DownloadAllAssetBundlesAsync(Action<string, int, int> onProgress, Action<Exception> onDone){
			yield return null;

		}

		public IEnumerator LoadAssetAsync(RemixApi.Either<UnityEngine.Object> answer, string path, Type type){
			var isAssetBundle = path.Contains (";");
			if (isAssetBundle) {
				var info = path.Split (';');
				var bundleName = info [0];
				var assetName = info [1];
				yield return LoadAssetAsync (answer, bundleName, assetName, type);
			} else {
				var req = Resources.LoadAsync (path);
				yield return req;
				answer.Ref = req.asset;
				yield break;
			}
		}

		public IEnumerator LoadAssetAsync(RemixApi.Either<UnityEngine.Object> answer, string bundleName, string assetName, Type type){
			if (simulation) {
				var asset = AssetBundleUtil.LoadAsset (null, bundleName, assetName, type);
				answer.Ref = asset;
				yield break;
			}
			var fullPath = assetBundlePath + platform + "/";
			var checkException = new RemixApi.Either<int> ();
			var bundleBytesPool = new Dictionary<string,byte[]> ();

			if (bundlePool.ContainsKey (platform) == false) {
				yield return AssetBundleUtil.LoadAssetBundleBytesWithDependencyAsync(checkException, fullPath, bundleBytesPool, null, platform);
				if (checkException.Exception != null) {
					answer.Exception = checkException.Exception;
					yield break;
				}
				var req = AssetBundle.LoadFromMemoryAsync(bundleBytesPool[platform]);
				yield return req;
				var manifestBundle = req.assetBundle;
				bundlePool.Add (platform, manifestBundle);
			}
			var manifest = AssetBundleUtil.LoadManifest (bundlePool[platform]);

			if (bundlePool.ContainsKey (bundleName) == false) {
				yield return AssetBundleUtil.LoadAssetBundleBytesWithDependencyAsync (checkException, fullPath, bundleBytesPool, manifest, bundleName);
				if (checkException.Exception != null) {
					answer.Exception = checkException.Exception;
					yield break;
				}
				foreach (var name in bundleBytesPool.Keys) {
					var bytes = bundleBytesPool [name];
					var req = AssetBundle.LoadFromMemoryAsync(bytes);
					yield return req;
					var bundle = req.assetBundle;
					bundlePool.Add (name, bundle);
				}
			}
			var targetBundle = bundlePool[bundleName];
			var req2 = targetBundle.LoadAssetAsync(assetName, type);
			yield return req2;

			var obj = req2.asset;
			// 使用AssetBundle載入的物件有可能會遺失材質，所以將材質重新加入一次
			ResetShader (obj);
			UpdateBundleName ();
			answer.Ref = obj;
			yield break;
		}
#endregion
	}
}

