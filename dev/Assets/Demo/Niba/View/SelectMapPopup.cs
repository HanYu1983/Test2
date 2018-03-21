using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;

namespace View{
	public class SelectMapPopup : MonoBehaviour {
		public ListView mapListView;
		public MapDataProvider mapDataProvider;

		void Awake(){
			mapListView.DataProvider = mapDataProvider;
		}

		public void UpdateUI(IModelGetter model){
			mapDataProvider.Data = new List<MapType> () {
				MapType.Random, MapType.Pattern
			};
			mapListView.UpdateDataView (model);
		}

		public IEnumerator HandleCommand(IModelGetter model, string msg, object args, Action<Exception> callback){
			switch (msg) {
			case "click_selectMapPopup_ok":
				{
					var idx = mapListView.LastSelectIndex;
					if (idx == -1) {
						callback(new Exception ("你沒有選擇任何招式"));
						yield break;
					}
					var mapType = mapDataProvider.Data [idx];
					Common.Common.Notify ("selectMapPopup_selectMap", mapType);
				}
				break;
			default:
				{
					if (msg.Contains (mapListView.CommandPrefix)) {
						yield return mapListView.HandleCommand (model, msg, args, callback);
					}
				}
				break;
			}
			yield return null;
		}
	}
}