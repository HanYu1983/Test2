using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using UnityEngine.UI;

namespace View{
	public class MapDataProvider : MonoBehaviour, ListView.IDataProvider {
		
		public int DataCount{ 
			get{ 
				return data.Count;
			}
		}

		public void ShowData(IModelGetter model, GameObject ui, int idx){
			var mapType = data [idx];
			ui.GetComponentInChildren<Text> ().text = mapType.ToString();
			ui.SetActive (true);
		}

		public void ShowSelect (IModelGetter model, GameObject ui, int idx){
			var mapType = data [idx];
			switch (mapType) {
			case MapType.Random:
				ui.GetComponentInChildren<Text> ().text = "測試用隨機地圖";
				break;
			case MapType.Pattern:
				ui.GetComponentInChildren<Text> ().text = "自動成生";
				break;
			}
		}

		/// <summary>
		/// 顯示用的資料，在呼叫UpdateUI前要先設定
		/// </summary>
		List<MapType> data;
		public List<MapType> Data{
			set{
				data = value;
			}
			get{
				return data;
			}
		}
	}
}