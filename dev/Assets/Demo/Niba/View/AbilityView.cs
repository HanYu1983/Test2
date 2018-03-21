using System;
using UnityEngine;
using UnityEngine.UI;
using Common;
using System.Linq;
using HanRPGAPI;

namespace View
{
	public class AbilityView : MonoBehaviour
	{
		public GameObject txtsBasic, txtsFight, txtsExp;

		public Text Search(GameObject parent, string id){
			foreach (var i in parent.GetComponentsInChildren<Text>()) {
				if (i.name == id) {
					return i;
				}
			}
			throw new Exception ("沒有找到:"+id);
		}

		public void UpdateAbility(IModelGetter model, Place who_){
			if (who_ == Place.Storage) {
				Debug.LogWarning("倉庫中不計算能力");
				return;
			}
			var who = model.GetMapPlayer (who_);
			var oriBasic = BasicAbility.Default.Add(who.basicAbility);
			var basic = model.PlayerBasicAbility(who_);

			if (txtsBasic != null) {
				var offsetBasic = basic.Add (oriBasic.Negative);
				Search (txtsBasic, "str").text = string.Format ("力:{0}({1})", (int)basic.str, (int)offsetBasic.str);
				Search (txtsBasic, "vit").text = string.Format ("體:{0}({1})", (int)basic.vit, (int)offsetBasic.vit);
				Search (txtsBasic, "agi").text = string.Format ("敏:{0}({1})", (int)basic.agi, (int)offsetBasic.agi);
				Search (txtsBasic, "dex").text = string.Format ("技:{0}({1})", (int)basic.dex, (int)offsetBasic.dex);
				Search (txtsBasic, "int").text = string.Format ("知:{0}({1})", (int)basic.Int, (int)offsetBasic.Int);
				Search (txtsBasic, "luc").text = string.Format ("運:{0}({1})", (int)basic.luc, (int)offsetBasic.luc);
			}

			if (txtsFight != null) {
				var oriFight = basic.FightAbility;
				var fight = model.PlayerFightAbility (who_);
				var offsetFight = fight.Add (oriFight.Negative);
				Search (txtsFight, "hp").text = string.Format ("耐久:{0}({1})", (int)fight.hp, (int)offsetFight.hp);
				Search (txtsFight, "mp").text = string.Format ("魔力:{0}({1})", (int)fight.mp, (int)offsetFight.mp);
				Search (txtsFight, "atk").text = string.Format ("物攻:{0}({1})", (int)fight.atk, (int)offsetFight.atk);
				Search (txtsFight, "def").text = string.Format ("物防:{0}({1})", (int)fight.def, (int)offsetFight.def);
				Search (txtsFight, "matk").text = string.Format ("魔攻:{0}({1})", (int)fight.matk, (int)offsetFight.matk);
				Search (txtsFight, "mdef").text = string.Format ("魔防:{0}({1})", (int)fight.mdef, (int)offsetFight.mdef);
				Search (txtsFight, "accuracy").text = string.Format ("命中:{0}({1})", (int)fight.accuracy, (int)offsetFight.accuracy);
				Search (txtsFight, "dodge").text = string.Format ("閃避:{0}({1})", (int)fight.dodge, (int)offsetFight.dodge);
				Search (txtsFight, "critical").text = string.Format ("爆擊:{0}({1})", (int)fight.critical, (int)offsetFight.critical);
			}

			if (txtsExp != null) {
				for (var i = 0; i < ConfigSkillType.ID_COUNT; ++i) {
					var cfg = ConfigSkillType.Get (i);
					var id = cfg.ID;
					var v = who.Exp (id);
					Search (txtsExp, id).text = string.Format ("{0}:{1}", cfg.Name, v);
				}
			}
		}
	}
}

