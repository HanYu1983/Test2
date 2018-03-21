using System;
using UnityEngine;
using Common;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace View
{
	public class SelectSkillPopup : MonoBehaviour
	{
		public ListView skillListView;
		public SkillDataProvider skillDataProvider;

		public ListView monsterListView;
		public MonsterDataProvider monsterDataProvider;

		void Awake(){
			skillListView.DataProvider = skillDataProvider;
			monsterListView.DataProvider = monsterDataProvider;
		}

		public List<string> Skills{
			set{
				skillDataProvider.Data = value;
			}
		}

		public List<int> MapObjectIds{
			set{
				monsterDataProvider.Data = value;
			}
		}

		Description work;
		public Description Work {
			set{
				var d = value;
				switch (d.description) {
				case Description.WorkSelectSkillForEnemy:
					{
						if (d.values.AllKeys.Contains ("skillIds") != false) {
							var skills = d.values.GetValues ("skillIds").ToList();
							Skills = skills;
						}
						var mapObjectId = int.Parse(d.values.Get ("mapObjectId"));
						MapObjectIds = new List<int>(){mapObjectId};
					}
					break;
				case Description.WorkUseSkillForEnemyAll:
					{
						var skillId = d.values.Get ("skillId");
						var mapObjectIds = d.values.GetValues ("mapObjectIds");
						Skills = new List<string> () {
							skillId
						};
						MapObjectIds = mapObjectIds.Select (int.Parse).ToList ();
					}
					break;
				default:
					throw new Exception ("錯誤的參數:" + d.description);
				}
				work = d;
			}
			get{
				return work;
			}
		}

		public void UpdateUI(IModelGetter model){
			if (skillDataProvider.Data != null) {
				skillListView.UpdateDataView (model);
			}
			if (monsterDataProvider.Data != null) {
				monsterListView.UpdateDataView (model);
			}
		}

		public IEnumerator HandleCommand(IModelGetter model, string msg, object args, Action<Exception> callback){
			switch (msg) {
			case "click_selectSkillPopup_ok":
				{
					var idx = skillListView.LastSelectIndex;
					if (idx == -1) {
						callback(new Exception ("你沒有選擇任何招式"));
						yield break;
					}
					var skill = skillDataProvider.Data [idx];
					work.values.Set ("skillId", skill);
					Common.Common.Notify ("selectSkillPopup_selectSkill", work);
				}
				break;
			default:
				{
					if (msg.Contains (skillListView.CommandPrefix)) {
						yield return skillListView.HandleCommand (model, msg, args, callback);
					}
				}
				break;
			}
			yield return null;
		}
	}
}

