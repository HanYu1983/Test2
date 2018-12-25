using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Text;


public class TestAssetBundle : MonoBehaviour {
	public Transform anchor;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator DownloadAndCache (){
		// Wait for the Caching system to be ready
		while (!Caching.ready)
			yield return null;
		/*
		// Load the AssetBundle file from Cache if it exists with the same version or download and store it in the cache
		using(WWW www = WWW.LoadFromCacheOrDownload ("file:///Users/hanyu/Desktop/test/AssetBundles/runtime", 0)){
			yield return www;
			if (www.error != null)
				throw new Exception("WWW download had an error:" + www.error);
			AssetBundle bundle = www.assetBundle;

			var obj = Instantiate(bundle.LoadAsset("Assets/Resources/Output/SpineAnim/C04P3.prefab")) as GameObject;
			// Unload the AssetBundles compressed contents to conserve memory
			bundle.Unload(false);

			obj.SetActive (true);
			obj.transform.SetParent (anchor, false);
		} // memory is freed from the web stream (www.Dispose() gets called implicitly)
		*/
		var bundle = HttpRequest ("file:///Users/hanyu/Desktop/test/AssetBundles/runtime");
		var obj = Instantiate(bundle.LoadAsset("Assets/Resources/Output/SpineAnim/C04P3.prefab")) as GameObject;
		bundle.Unload(false);
		obj.SetActive (true);
		obj.transform.SetParent (anchor, false);
	}

	public static AssetBundle HttpRequest(string uri){
		WebRequest request = WebRequest.Create (uri);
		try{
			WebResponse response = request.GetResponse ();
			var dataStream = response.GetResponseStream ();
			BinaryReader reader = new BinaryReader (dataStream);
			byte[] payload = reader.ReadBytes((int)response.ContentLength);
			AssetBundle bundle = AssetBundle.LoadFromMemory (payload);
			reader.Close ();
			dataStream.Close ();
			response.Close ();
			return bundle;
		}catch(Exception e){
			throw new UnityException ("伺服器沒有響應:"+e.Message);
		}
	}
}
