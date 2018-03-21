using System;
using Common;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using HanRPGAPI;

namespace View
{
	public class ItemDataProvider : MonoBehaviour, ListView.IDataProvider
	{
		public int DataCount{ 
			get{ 
				return data.Count;
			}
		}

		public void ShowData(IModelGetter model, GameObject ui, int idx){
			var modelItem = data [idx];
			var cfg = ConfigItem.Get (modelItem.prototype);

			var cnt = modelItem.count;
			var name = cfg.Name;
			var appendStr = "";
			switch (showMode) {
			case Mode.Equip:
				{
					if (cfg.Type == ConfigItemType.ID_weapon) {
						appendStr += "(" + cfg.Ability + ")";
					}
				}
				break;
			}

			var msg = string.Format ("{0}{1}{2}個", name, appendStr, cnt);
			ui.GetComponentInChildren<Text> ().text = msg;
			ui.SetActive (true);
		}

		public void ShowSelect (IModelGetter model, GameObject ui, int idx){
			if (idx <0 || idx >= DataCount) {
				ui.GetComponent<Text>().text = "你沒有選擇任何道具";
				return;
			}
			var item = data [idx];
			var cfg = ConfigItem.Get (item.prototype);
			ui.GetComponent<Text>().text = string.Format ("你選擇{0}", cfg.Name);
		}

		/// <summary>
		/// 顯示用的資料，在呼叫UpdateUI前要先設定
		/// </summary>
		List<Item> data;
		public List<Item> Data{
			set{
				data = value;
			}
			get{
				return data;
			}
		}

		public enum Mode{
			Normal, Equip
		}
		public Mode showMode;
		/// <summary>
		/// 列表文字顯示模式
		/// 當為Equip時會另外顯示裝備道具效果
		/// 在點擊到weapon時可以修改這個模式
		/// </summary>
		/// <value>The show mode.</value>
		public Mode ShowMode{
			set{
				showMode = value;
			}
		}
	}
}

