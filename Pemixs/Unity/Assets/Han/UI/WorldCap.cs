using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Remix
{
	public class WorldCap : MonoBehaviour
	{
		public List<string> mapIdx2ArrayIdx;
		public GameObject mapObject;
		public int numOfNode;

		public List<GameObject> ons, offs, cans, times, photos, cats;
		public List<Text> texts;

		public RectPosition rectPosition;
		public RectPosition RectPosition{ get { return rectPosition; } }

		void Awake(){
			mapIdx2ArrayIdx = GameConfig.MAP_IDXS;
			ons = new List<GameObject> ();
			offs = new List<GameObject> ();
			cans = new List<GameObject> ();
			times = new List<GameObject> ();
			photos = new List<GameObject> ();
			cats = new List<GameObject> ();

			foreach (var mapIdx in mapIdx2ArrayIdx) {
				var onsName = string.Format ("EP_{0}_on", mapIdx);
				var offsName = string.Format ("EP_{0}_off", mapIdx);
				ons.Add (mapObject.transform.Find (onsName).gameObject);
				offs.Add (mapObject.transform.Find (offsName).gameObject);

				for (var i = 0; i < numOfNode; ++i) {
					var cansName = string.Format ("EP_{0}_cp{1}_on", mapIdx, i+1);
					var timesName = string.Format ("EP_{0}_cp{1}_time", mapIdx, i+1);
					var photosName = string.Format ("EP_{0}_cp{1}_photo", mapIdx, i+1);
					var catsName = string.Format ("EP_{0}_cp{1}_cat", mapIdx, i+1);

					cans.Add (mapObject.transform.Find (cansName).gameObject);
					times.Add (mapObject.transform.Find (timesName).gameObject);
					photos.Add (mapObject.transform.Find (photosName).gameObject);
					cats.Add (mapObject.transform.Find (catsName).gameObject);
				}
			}

			for (var i = 0; i < mapObject.transform.childCount; ++i) {
				var child = mapObject.transform.GetChild (i);
				if (child.name.Contains ("Text_")) {
					var t = child.GetComponent<Text> ();
					if (t == null) {
						Debug.LogWarning ("沒有設定Text:"+child.name);
						continue;
					}
					texts.Add (t);
				}
			}
		}

		public void SetText(string key, string v){
			foreach(var text in texts){
				var isTarget = text.gameObject.name == key;
				if (isTarget) {
					text.text = v;
				}
			}
		}

		public void HideAllClear(){
			for (var i = 0; i < cans.Count; ++i) {
				cans [i].SetActive (false);
				times [i].SetActive (false);
				photos [i].SetActive (false);
				cats [i].SetActive (false);
			}
		}

		public void EnableCan(NodeKey key){
			var index = mapIdx2ArrayIdx.IndexOf(key.MapIdx)* 3 + key.Idx;
			if(index == -1){
				throw new UnityException ("沒有這張地圖:"+key.MapIdx);
			}
			cans [index].SetActive (true);
		}

		public void EnableWaitCapture(NodeKey key){
			var index = mapIdx2ArrayIdx.IndexOf(key.MapIdx)* 3 + key.Idx;
			if(index == -1){
				throw new UnityException ("沒有這張地圖:"+key.MapIdx);
			}
			times [index].SetActive (true);
		}

		public void EnableWaitGetPhoto(NodeKey key){
			var index = mapIdx2ArrayIdx.IndexOf(key.MapIdx)* 3 + key.Idx;
			if(index == -1){
				throw new UnityException ("沒有這張地圖:"+key.MapIdx);
			}
			photos [index].SetActive (true);
		}

		public void EnableWaitGetCat(NodeKey key){
			var index = mapIdx2ArrayIdx.IndexOf(key.MapIdx)* 3 + key.Idx;
			if(index == -1){
				throw new UnityException ("沒有這張地圖:"+key.MapIdx);
			}
			cats [index].SetActive (true);
		}

		public void Unlock(string mapIdx){
			var index = mapIdx2ArrayIdx.IndexOf(mapIdx);
			if(index == -1){
				throw new UnityException ("沒有這張地圖:"+mapIdx);
			}
			ons [index].SetActive (true);
		}
	}
}

