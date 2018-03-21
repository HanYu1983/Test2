using System;
using UnityEngine;
using Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using HanRPGAPI;

namespace View
{
	public class MonsterDataProvider : MonoBehaviour, ListView.IDataProvider
	{
		public int DataCount{ 
			get{ 
				return data.Count;
			}
		}

		public void ShowData(IModelGetter model, GameObject ui, int idx){
			var mapObjectId = data [idx];
			var mapObject = model.MapObjects [mapObjectId];
			if (mapObject.type != MapObjectType.Monster) {
				throw new Exception ("XXXX");
			}
			var monsterInf = model.MonsterInfos [mapObject.infoKey];
			var cfg = ConfigMonster.Get (monsterInf.type);
			var msg = string.Format ("{0}", cfg.Name);
			ui.GetComponentInChildren<Text> ().text = msg;
			ui.SetActive (true);
		}

		public void ShowSelect (IModelGetter model, GameObject ui, int idx){
			
		}

		/// <summary>
		/// 顯示用的資料，在呼叫UpdateUI前要先設定
		/// </summary>
		List<int> data;
		public List<int> Data{
			set{
				data = value;
			}
			get{
				return data;
			}
		}
	}
}

