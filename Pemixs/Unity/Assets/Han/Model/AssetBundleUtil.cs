using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace Remix {
	public class AssetBundleUtil {

		public static UnityEngine.Object LoadAsset(Dictionary<string,AssetBundle> bundlePool, string bundleName, string assetName, Type type){
			#if UNITY_EDITOR
			if (bundlePool == null) {
				// 全部改為小寫. AssetBundle的名稱設定中不支援大寫，而尋找AssetBundle名稱又是大小寫敏感的
				bundleName = bundleName.ToLower();
				string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundleAndAssetName (bundleName, assetName);
				if (assetPaths.Length == 0) {
					throw new UnityException ("asset not exist:" + bundleName + ";" + assetName);
				}
				var target = AssetDatabase.LoadAssetAtPath (assetPaths [0], type);
				return target;
			} 
			else 
			#endif
			{
				return bundlePool [bundleName].LoadAsset (assetName, type);
			}
		}
		
		public static void UnloadAssetBundle(Dictionary<string,AssetBundle> bundlePool){
			foreach (var bundle in bundlePool.Values) {
				bundle.Unload (false);
			}
			bundlePool.Clear ();
			// 這行非常重要，這會把assetBundle中讀入卻沒有到的資源釋放掉
			Resources.UnloadUnusedAssets ();
		}

		public static bool IsLocalAssetBundleExist(string bundleName){
			var bundleNameEscape = WWW.EscapeURL (bundleName);
			var localPath = GameConfig.LOCAL_ASSET_BUNDELS_PATH + bundleNameEscape;
			var isExist = File.Exists (localPath);
			return isExist;
		}

		// 會有這個方法純粹是為了Threading
		// 因為AssetBundle.LoadFromXXX只能在MainThread中執行
		// 所以由另一條Thread呼叫LoadAssetBundleBytesWithDependency將收集到的Bytes交給MainThread
		// 再由MainThread呼叫AssetBundle.LoadFromMemory
		// Download AssetBundles時畫面幾乎凍住，所以有了這個方法來改善
		public static byte[] LoadAssetBundleBytesWithDependency(string bundlePath, Dictionary<string,byte[]> bytesPool, AssetBundleManifest manifest, string bundleName){
			if (manifest != null) {
				var deps = manifest.GetAllDependencies (bundleName);
				foreach (var depBundleName in deps) {
					var hasDepBundle = bytesPool.ContainsKey (depBundleName);
					if (hasDepBundle == false) {
						LoadAssetBundleBytesWithDependency (bundlePath, bytesPool, manifest, depBundleName);
					}
				}
			}
			var hasBundle = bytesPool.ContainsKey (bundleName);
			if (hasBundle == false) {
				// 不需要更新的bundle才使用cache
				var useCache = RemixApi.CheckBundleUpdate (bundlePath, bundleName) == false;
				// 不需要另外下載manifest，因為在CheckBundleUpdate就下載過了
				var withManifest = false;
				var bytes = RemixApi.LoadAssetBundleBytes (bundlePath, bundleName, useCache, withManifest);
				bytesPool.Add (bundleName, bytes);
			}
			return bytesPool [bundleName];
		}

		// 而這個方法是給MainThread(or Coroutine)執行用的
		// 因為當中用到了AssetBundle.LoadFromXXX
		// 動態Download時呼叫，這個時候畫面凍住沒有關係
		public static AssetBundle LoadAssetBundleWithDependency(string bundlePath, Dictionary<string,AssetBundle> bundlePool, AssetBundleManifest manifest, string bundleName){
			if (manifest != null) {
				var deps = manifest.GetAllDependencies (bundleName);
				foreach (var depBundleName in deps) {
					var hasDepBundle = bundlePool.ContainsKey (depBundleName);
					if (hasDepBundle == false) {
						LoadAssetBundleWithDependency (bundlePath, bundlePool, manifest, depBundleName);
					}
				}
			}
			var hasBundle = bundlePool.ContainsKey (bundleName);
			if (hasBundle == false) {
				var useCache = true;
				// 一並連manifest一起下載
				var withManifest = true;
				var bytes = RemixApi.LoadAssetBundleBytes (bundlePath, bundleName, useCache, withManifest);
				var bundle = AssetBundle.LoadFromMemory (bytes);
				bundlePool.Add (bundleName, bundle);
			}
			return bundlePool [bundleName];
		}

		public static AssetBundleManifest LoadManifest(AssetBundle bundle){
			return bundle.LoadAsset ("AssetBundleManifest") as AssetBundleManifest;
		}

		#region async
		public static IEnumerator LoadAssetBundleBytesWithDependencyAsync(RemixApi.Either<int> answer, string bundlePath, Dictionary<string,byte[]> bytesPool, AssetBundleManifest manifest, string bundleName){
			if (manifest != null) {
				var deps = manifest.GetAllDependencies (bundleName);
				foreach (var depBundleName in deps) {
					var hasDepBundle = bytesPool.ContainsKey (depBundleName);
					if (hasDepBundle == false) {
						yield return LoadAssetBundleBytesWithDependencyAsync (answer, bundlePath, bytesPool, manifest, depBundleName);
					}
				}
			}
			var hasBundle = bytesPool.ContainsKey (bundleName);
			if (hasBundle == false) {
				var answer2 = new RemixApi.Either<byte[]> ();
				yield return RemixApi.LoadAssetBundleBytesAsync (answer2, bundlePath, bundleName, true, true);
				if (answer2.Exception != null) {
					answer.Exception = answer2.Exception;
					yield break;
				}
				bytesPool.Add (bundleName, answer2.Ref);
			}
		}
		#endregion
	}
}