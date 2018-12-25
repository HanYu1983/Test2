using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Remix
{
	public class WorldMap : MonoBehaviour
	{
		public List<string> mapIdx2ArrayIdx;
		public GameObject mapObject;
		public List<GameObject> ons, offs, easys, normals, hards;
		public List<Text> texts;

		public RectPosition rectPosition;
		public RectPosition RectPosition{ get { return rectPosition; } }

		void Awake(){
			mapIdx2ArrayIdx = GameConfig.MAP_IDXS;
			CollectGameObject ();
		}

		void CollectGameObject(){
			foreach (var mapIdx in mapIdx2ArrayIdx) {
				var onsName = string.Format ("EP_{0}_on", mapIdx);
				var offsName = string.Format ("EP_{0}_off", mapIdx);
				var easysName = string.Format ("EP_{0}_easy_clear", mapIdx);
				var normalsName = string.Format ("EP_{0}_normal_clear", mapIdx);
				var hardsName = string.Format ("EP_{0}_hard_clear", mapIdx);
				ons.Add (mapObject.transform.Find (onsName).gameObject);
				offs.Add (mapObject.transform.Find (offsName).gameObject);
				easys.Add (mapObject.transform.Find (easysName).gameObject);
				normals.Add (mapObject.transform.Find (normalsName).gameObject);
				hards.Add (mapObject.transform.Find (hardsName).gameObject);
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
			Text target = null;
			foreach(var text in texts){
				var isTarget = text.gameObject.name == key;
				if (isTarget) {
					target = text;
					break;
				}
			}
			if (target != null) {
				target.text = v;
			} else {
				Debug.LogWarning ("目標文字欄位沒有找到:"+key);
			}
		}

		public void HideAllClear(){
			for (var i = 0; i < easys.Count; ++i) {
				easys [i].SetActive (false);
				normals [i].SetActive (false);
				hards [i].SetActive (false);
			}
		}

		public void ClearEasy(string mapIdx){
			var index = mapIdx2ArrayIdx.IndexOf(mapIdx);
			if(index == -1){
				throw new UnityException ("沒有這張地圖:"+mapIdx);
			}
			easys [index].SetActive (true);
		}

		public void ClearNormal(string mapIdx){
			var index = mapIdx2ArrayIdx.IndexOf(mapIdx);
			if(index == -1){
				throw new UnityException ("沒有這張地圖:"+mapIdx);
			}
			normals [index].SetActive (true);
		}

		public void ClearHard(string mapIdx){
			var index = mapIdx2ArrayIdx.IndexOf(mapIdx);
			if(index == -1){
				throw new UnityException ("沒有這張地圖:"+mapIdx);
			}
			hards [index].SetActive (true);
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

