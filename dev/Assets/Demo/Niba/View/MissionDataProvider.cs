using System;
using Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using HanRPGAPI;

namespace View
{
	public class MissionDataProvider : MonoBehaviour, ListView.IDataProvider
	{
		public ListView requireListView;
		public AbstractItemDataProvider requireItemDataProvider;

		public ListView rewardListView;
		public AbstractItemDataProvider rewardDataProvider;

		void Awake(){
			requireListView.DataProvider = requireItemDataProvider;
			rewardListView.DataProvider = rewardDataProvider;
		}

		public int DataCount{ 
			get{ 
				return data.Count;
			}
		}

		public void ShowData(IModelGetter model, GameObject ui, int idx){
			ui.transform.Find ("txt_npc").GetComponent<Text> ().text = "";
			ui.transform.Find ("txt_des").GetComponent<Text> ().text = "";

			var item = data [idx];
			var cfg = ConfigNpcMission.Get (item);
			if (cfg.Npc != null) {
				var npc = ConfigNpc.Get (cfg.Npc);
				ui.transform.Find ("txt_npc").GetComponent<Text> ().text = npc.Name;
			}
			ui.transform.Find ("txt_des").GetComponent<Text> ().text = cfg.Dialog;
			ui.SetActive (true);
		}

		public void ShowSelect (IModelGetter model, GameObject ui, int idx){
			if (idx <0 || idx >= DataCount) {
				return;
			}
			var item = data [idx];
			var cfg = ConfigNpcMission.Get (item);

			var requireItem = new List<AbstractItem> ();
			if (cfg.RequireItem != null) {
				requireItem.AddRange (HanRPGAPI.Alg.ParseAbstractItem (cfg.RequireItem));
			}
			if (cfg.RequireKill != null) {
				requireItem.AddRange (HanRPGAPI.Alg.ParseAbstractItem (cfg.RequireKill));
			}
			if (cfg.RequireStatus != null) {
				requireItem.AddRange (HanRPGAPI.Alg.ParseAbstractItem (cfg.RequireStatus));
			}
			requireItemDataProvider.Data = requireItem;
			rewardDataProvider.Data = HanRPGAPI.Alg.ParseAbstractItem (cfg.Reward).ToList ();

			requireListView.UpdateDataView (model);
			rewardListView.UpdateDataView (model);
		}

		/// <summary>
		/// 顯示用的資料，在呼叫UpdateUI前要先設定
		/// </summary>
		List<string> data;
		public List<string> Data{
			set{
				data = value;
			}
			get{
				return data;
			}
		}
	}
}

