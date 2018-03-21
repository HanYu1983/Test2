using System;
using UnityEngine;
using UnityEngine.UI;
using Common;
using System.Collections.Generic;
using HanRPGAPI;

namespace View
{
	public class WorkDataProvider : MonoBehaviour, ListView.IDataProvider
	{
		public int DataCount{ 
			get{ 
				return data.Count;
			}
		}

		public void ShowData(IModelGetter model, GameObject ui, int idx){
			var w = data [idx];
			var msg = "";
			switch (w.description) {
			case Description.WorkCollectResource:
				{
					var mapObjectId = int.Parse(w.values.Get("mapObjectId"));
					var mapObject = model.MapObjects[mapObjectId];
					var mapObjectInfo = model.ResourceInfos[mapObject.infoKey];
					var config = ConfigResource.Get(mapObjectInfo.type);
					msg = "采集[" + config.Name + "]";
				}
				break;
			case Description.WorkAttack:
				{
					var mapObjectId = int.Parse(w.values.Get("mapObjectId"));
					var mapObject = model.MapObjects[mapObjectId];
					var mapObjectInfo = model.MonsterInfos[mapObject.infoKey];
					var config = ConfigMonster.Get(mapObjectInfo.type);
					msg = "攻擊[" + config.Name + "]";
				}
				break;
			case Description.WorkSelectSkillForEnemy:
				{
					var mapObjectId = int.Parse(w.values.Get("mapObjectId")); 
					var mapObject = model.MapObjects[mapObjectId];
					var mapObjectInfo = model.MonsterInfos[mapObject.infoKey];
					var config = ConfigMonster.Get(mapObjectInfo.type);
					var skillIds = w.values.GetValues ("skillIds");
					msg = "招式[" + config.Name + "]";
				}
				break;
			case Description.WorkUseSkillForEnemyAll:
				{
					var skillId = w.values.Get ("skillId");
					var cfg = ConfigSkill.Get (skillId);
					msg = string.Format ("{0}攻擊", cfg.Name);
				}
				break;
			default:
				throw new NotImplementedException (w.description);
			}
			ui.GetComponentInChildren<Text> ().text = msg;
			ui.SetActive (true);
		}

		public void ShowSelect (IModelGetter model, GameObject ui, int idx){

		}

		/// <summary>
		/// 顯示用的資料，在呼叫UpdateUI前要先設定
		/// </summary>
		List<Description> data;
		public List<Description> Data{
			set{
				data = value;
			}
			get{
				return data;
			}
		}
	}
}

