using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;
using System.Linq;
using UnityEngine.UI;
using HanRPGAPI;

namespace View{
	public class SkillPopup : MonoBehaviour {
		public ListView skillListView;
		public SkillDataProvider skillDataProvider;
		public GameObject filterBtns;
		public Text txtSlotUse;

		public Toggle[] toggles;

		void Awake(){
			skillListView.DataProvider = skillDataProvider;
			toggles = filterBtns.GetComponentsInChildren<Toggle> ();
			// 直接改變isOn會自動觸發按下事件，而導致bug
			// 所以改為在場景設定
			/*foreach (var t in toggles) {
				t.isOn = false;
			}*/
		}

		public void UpdateUI(IModelGetter model){
			UpdateSkillList (model);
			UpdateSlotCount (model);
		}

		public void UpdateSlotCount(IModelGetter model){
			var who = model.GetMapPlayer (Common.Common.PlaceAt (model.PlayState));
			txtSlotUse.text = string.Format ("{0}/{1}", who.SkillSlotUsed, who.MaxSkillSlotCount);
		}

		static int FilterIDApply = 0;

		public void UpdateSkillList(IModelGetter model){
			IEnumerable<ConfigSkill> skills = null;

			if (CheckToggleValue (FilterIDApply)) {
				var who = model.GetMapPlayer (Common.Common.PlaceAt (model.PlayState));
				skills = who.skills.Select (ConfigSkill.Get);
			} else {
				skills = Enumerable.Range (0, ConfigSkill.ID_COUNT).Select (ConfigSkill.Get);
			}

			if (CheckToggleValue (1)) {
				skills = skills.Where (cfg => {
					return cfg.SkillTypeRequire.Contains(ConfigSkillType.ID_karate);
				});
			}

			if (CheckToggleValue (2)) {
				skills = skills.Where (cfg => {
					return cfg.SkillTypeRequire.Contains(ConfigSkillType.ID_fencingArt);
				});
			}

			var skillIds = skills.Select(cfg=>cfg.ID);
			skillDataProvider.Data = skillIds.ToList();
			skillListView.UpdateDataView (model);
			skillListView.CurrItemLabel (model, skillListView.offset);
		}

		public bool CheckToggleValue(int num){
			if (num >= toggles.Length) {
				Debug.LogWarning ("XXXX");
				return false;
			}
			return toggles [num].isOn;
		}

		public IEnumerator HandleCommand(IModelGetter model, string msg, object args, Action<Exception> callback){
			switch (msg) {
			case "click_skillPopup_active":
				{
					var idx = skillListView.LastSelectIndex;
					if (idx < 0) {
						yield break;
					}
					if (idx >= skillDataProvider.Data.Count) {
						callback(new Exception("idx >= skillDataProvider.Data.Count"));
						yield break;
					}
					var selectSkill = skillDataProvider.Data [idx];
					Common.Common.Notify ("skillPopup_active", selectSkill);
				}
				break;
			case "click_skillPopup_inactive":
				{
					var idx = skillListView.LastSelectIndex;
					if (idx < 0) {
						yield break;
					}
					if (idx >= skillDataProvider.Data.Count) {
						callback(new Exception("idx >= skillDataProvider.Data.Count")); 
						yield break;
					}
					var selectSkill = skillDataProvider.Data [idx];
					Common.Common.Notify ("skillPopup_inactive", selectSkill);
				}
				break;
			case "click_skillPopup_filter":
				// 如果點擊"已裝備"，要把索引重設為0，不然招式列表會看不見
				if (CheckToggleValue (FilterIDApply)) {
					skillListView.offset = 0;
				}
				UpdateSkillList (model);
				callback(null);
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