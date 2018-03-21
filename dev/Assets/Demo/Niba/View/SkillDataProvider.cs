using System;
using UnityEngine;
using Common;
using System.Collections.Generic;
using UnityEngine.UI;
using HanRPGAPI;

namespace View
{
	public class SkillDataProvider : MonoBehaviour, ListView.IDataProvider
	{
		public int DataCount{ 
			get{ 
				return data.Count;
			}
		}

		public void ShowData(IModelGetter model, GameObject ui, int idx){
			var modelItem = data [idx];
			var cfg = ConfigSkill.Get (modelItem);
			var conditionCfg = ConfigConditionType.Get (cfg.Condition);

			var who = model.GetMapPlayer (Common.Common.PlaceAt (model.PlayState));
			var isEquip = who.skills.Contains (cfg.ID);
			var btn = ui.GetComponent<Button> ();
			var colors = btn.colors;
			colors.normalColor = colors.highlightedColor = isEquip ? Color.yellow : Color.white;
			btn.colors = colors;

			ui.transform.Find ("txt_name").GetComponent<Text> ().text = cfg.Name;
			ui.transform.Find ("txt_des").GetComponent<Text> ().text = cfg.Effect;
			ui.transform.Find ("txt_condition").GetComponent<Text> ().text = conditionCfg.Name;
			ui.transform.Find ("txt_slotCount").GetComponent<Text> ().text = cfg.SlotCount+"";
			ui.SetActive (true);
		}

		public void ShowSelect (IModelGetter model, GameObject ui, int idx){
			if (idx <0 || idx >= DataCount) {
				ui.GetComponent<Text>().text = "你沒有選擇任何道具";
				return;
			}
			var item = data [idx];
			var cfg = ConfigSkill.Get (item);
			ui.GetComponent<Text>().text = string.Format ("你選擇{0}", cfg.Name);
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

