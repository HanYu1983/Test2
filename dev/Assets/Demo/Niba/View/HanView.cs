using System;
using UnityEngine;
using Common;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using HanRPGAPI;

namespace View
{
	public class HanView : MonoBehaviour, IView
	{
		public ZUIManager mgr;
		public Menu menuTitle, menuHome, menuMap;
		public SideMenu menuInfo;
		public Popup itemPopup;
		public Popup abilityPopup;
		public Popup fusionPopup;
		public Popup missionPopup;
		public Popup skillPopup;
		public Popup selectSkillPopup;
		public Popup selectMapPopup;
		public HandleFungus handleFungus;

		IModelGetter model;
		public IModelGetter ModelGetter{ set{ model = value; } }
		public IEnumerator ChangePage(Page page, Action<Exception> callback){
			switch (page) {
			case Page.Title:
				{
					mgr.OpenMenu (menuTitle);
					callback (null);
				}
				break;
			case Page.Home:
				mgr.OpenMenu (menuHome);
				var home = menuHome.GetComponent<MenuHome> ();
				if (home == null) {
					throw new Exception ("你沒有加入MenuHome Component");
				}
				home.UpdateUI (model, Place.Pocket);
				callback (null);
				break;
			case Page.Game:
				mgr.OpenMenu (menuMap);
				callback (null);
				break;
			}
			yield return null;
		}

		public IEnumerator ShowInfo(Info info, object args, Action<Exception> callback){
			switch (info) {
			case Info.SelectSkill:
				{
					var popup = selectSkillPopup.GetComponent<SelectSkillPopup> ();
					if (popup == null) {
						throw new Exception ("你沒有加入SelectSkillPopup Component");
					}
					selectSkillPopup.ChangeVisibility (true);
					var d = (Description)args;
					popup.Work = d;
					popup.UpdateUI (model);
					callback (null);
				}
				break;
			default:
				yield return ShowInfo (info, callback);
				break;
			}
		}

		public IEnumerator ShowInfo(Info info, Action<Exception> callback){
			switch (info) {
			case Info.SelectMap:
				{
					var popup = selectMapPopup.GetComponent<SelectMapPopup> ();
					if (popup == null) {
						throw new Exception ("你沒有加入SelectSkillPopup Component");
					}
					selectMapPopup.ChangeVisibility (true);
					popup.UpdateUI (model);
					callback (null);
				}
				break;
			case Info.Skill:
				{
					var popup = skillPopup.GetComponent<SkillPopup> ();
					if (popup == null) {
						throw new Exception ("你沒有加入SkillPopup Component");
					}
					skillPopup.ChangeVisibility (true);
					popup.UpdateUI (model);
					callback (null);
				}
				break;
			case Info.Mission:
				{
					var popup = missionPopup.GetComponent<MissionPopup> ();
					if (popup == null) {
						throw new Exception ("你沒有加入MissionPopup Component");
					}
					missionPopup.ChangeVisibility (true);
					popup.UpdateMissionList (model);
					callback (null);
				}
				break;
			case Info.Storage:
				{
					var popup = itemPopup.GetComponent<ItemPopup2> ();
					if (popup == null) {
						throw new Exception ("你沒有加入ItemPopup Component");
					}
					// 先Open才會呼叫Awake
					itemPopup.ChangeVisibility(true);
					popup.Who = Place.Storage;
					popup.UpdateUI (model);
					callback (null);
				}
				break;
			case Info.Item:
				{
					var popup = itemPopup.GetComponent<ItemPopup2> ();
					if (popup == null) {
						throw new Exception ("你沒有加入ItemPopup Component");
					}
					// 先Open才會呼叫Awake
					itemPopup.ChangeVisibility(true);
					popup.Who = Common.Common.PlaceAt(model.PlayState);
					popup.UpdateUI (model);
					callback (null);
					break;
				}
			case Info.Map:
				{
					var map = mgr.CurActiveMenu.GetComponent<MenuMap> ();
					if (map == null) {
						callback (new Exception ("你沒有加入MenuMap Component"));
						yield break;
					}
					map.UpdateUI (model);
					callback (null);
				}
				break;
			case Info.Work:
				{
					var map = mgr.CurActiveMenu.GetComponent<MenuMap> ();
					if (map == null) {
						callback (new Exception ("你沒有加入MenuMap Component"));
						yield break;
					}
					map.UpdateWork (model);
					callback (null);
				}
				break;
			case Info.WorkResult:
				{
					var results = model.WorkResults;
					if (results == null) {
						Debug.LogWarning ("沒有workresults");
						callback (null);
						yield break;
					}
					var msg = string.Join("\n", results.Select (e => {
						Debug.LogWarning(e.description);
						switch (e.description) {
						case Description.EventLucklyFind:
							{
								var itemPrototype = e.values.Get("itemPrototype");
								var count = int.Parse(e.values.Get("count"));
								var config = ConfigItem.Get (itemPrototype);
								return "獲得item:" + config.Name + " 數量:" + count;
							}
						case Description.InfoCollectResource:
							{
								var items = e.values.GetValues("items").Select(JsonUtility.FromJson<Item>);
								return string.Format("你採集了{0}", string.Join(",", items.Select(i=>i.ToString()).ToArray()));
							}
						case Description.InfoAttack:
							{
								var mapObjectId = int.Parse (e.values.Get ("mapObjectId"));
								var mapObj = model.MapObjects [mapObjectId];
								var objInfo = model.MonsterInfos [mapObj.infoKey];
								var objCfg = ConfigMonster.Get (objInfo.type);
								var damage = float.Parse (e.values.Get ("damage"));
								var isCriHit = e.values.Get("isCriHit") == "1";
								return string.Format ("你攻擊{0}造成{1}{2}傷害", objCfg.Name, damage, isCriHit ? "暴擊":"");
							}
						case Description.InfoDodge:
							{
								var mapObjectId = int.Parse (e.values.Get ("mapObjectId"));
								var mapObj = model.MapObjects [mapObjectId];
								var objInfo = model.MonsterInfos [mapObj.infoKey];
								var objCfg = ConfigMonster.Get (objInfo.type);
								return string.Format ("你閃避了{0}的攻擊", objCfg.Name);
							}
						case Description.InfoMonsterAttack:
							{
								var mapObjectId = int.Parse (e.values.Get ("mapObjectId"));
								var mapObj = model.MapObjects [mapObjectId];
								var objInfo = model.MonsterInfos [mapObj.infoKey];
								var objCfg = ConfigMonster.Get (objInfo.type);
								var damage = int.Parse (e.values.Get ("damage"));
								return string.Format ("{0}對你造成{1}傷害", objCfg.Name, damage);
							}
						case Description.InfoMonsterDied:
							{
								var mapObjectId = int.Parse (e.values.Get ("mapObjectId"));
								var mapObj = model.MapObjects [mapObjectId];
								var objInfo = model.MonsterInfos [mapObj.infoKey];
								var objCfg = ConfigMonster.Get (objInfo.type);

								var show = "";
								show += string.Format("{0}死亡. ", objCfg.Name);
								var hasReward = e.values.AllKeys.Contains("rewards");
								if(hasReward){
									var rewards = e.values.GetValues("rewards").Select(JsonUtility.FromJson<Item>);
									show += string.Format("你獲得{0}", string.Join(",", rewards.Select(i=>i.ToString()).ToArray()));
								}
								return show;
							}
						case Description.InfoWeaponBroken:
							{
								var weapons = e.values.GetValues("items").Select(JsonUtility.FromJson<Item>);
								return string.Format("{0}壞掉了!", string.Join(",", weapons.Select(i=>i.ToString()).ToArray()));
							}
						case Description.InfoUseSkill:
							{
								var skills = e.values.GetValues("skills").Select(ConfigSkill.Get).Select(cfg=>cfg.Name);
								return string.Format("你使出了{0}!", string.Join(",", skills.ToArray()));
							}
						case Description.InfoMonsterDodge:
							{
								var mapObjectId = int.Parse (e.values.Get ("mapObjectId"));
								var mapObj = model.MapObjects [mapObjectId];
								var objInfo = model.MonsterInfos [mapObj.infoKey];
								var objCfg = ConfigMonster.Get (objInfo.type);
								return string.Format ("{0}閃過你的攻擊", objCfg.Name);
							}
						case Description.InfoMonsterEscape:
							{
								var mapObjectId = int.Parse (e.values.Get ("mapObjectId"));
								var mapObj = model.MapObjects [mapObjectId];
								var objInfo = model.MonsterInfos [mapObj.infoKey];
								var objCfg = ConfigMonster.Get (objInfo.type);
								return string.Format ("{0}逃走了", objCfg.Name);
							}
						case Description.InfoMonsterIdle:
							{
								var mapObjectId = int.Parse (e.values.Get ("mapObjectId"));
								var mapObj = model.MapObjects [mapObjectId];
								var objInfo = model.MonsterInfos [mapObj.infoKey];
								var objCfg = ConfigMonster.Get (objInfo.type);
								return string.Format ("{0}沒有理你", objCfg.Name);
							}
						default:
							throw new NotImplementedException(e.description);	
						}
					}).ToArray ());

					Alert (msg);
					callback (null);
				}
				break;
			case Info.Event:
				{
					var results = model.MoveResult;
					if (results.HasEvent == false) {
						yield break;
					}
					var msg = string.Join ("\n", results.events.Select (evt => {
						switch(evt.description){
						case Description.EventLucklyFind:
							{
								var itemPrototype = evt.values.Get("itemPrototype");
								var cnt = int.Parse(evt.values.Get("count"));
								var cfg = ConfigItem.Get(itemPrototype);
								return string.Format("你好運地發現了{0}{1}個", cfg.Name, cnt);
							}
						default:
							throw new NotImplementedException("沒實作的事件:"+evt.description);
						}
					}).ToArray ());
					Alert (msg);
					callback (null);
				}
				break;
			case Info.Ability:
				{
					var ability = abilityPopup.GetComponent<AbilityView> ();
					if (ability == null) {
						callback (new Exception ("你沒有加入AbilityPopup Component"));
						yield break;
					}
					abilityPopup.ChangeVisibility (true);
					ability.UpdateAbility (model, Place.Map);
				}
				break;
			case Info.Fusion:
				{
					var popup = fusionPopup.GetComponent<FusionPopup> ();
					if (popup == null) {
						callback (new Exception ("你沒有加入FusionPopup Component"));
						yield break;
					}
					fusionPopup.ChangeVisibility (true);
					popup.UpdateUI (model);
				}
				break;
			default:
				throw new NotImplementedException (info.ToString());
			}
			yield return null;
		}

		public IEnumerator HideInfo(Info page){
			switch (page)
			{
			case Info.Map:
			case Info.Work:
				{
					// nothing to do
				}
				break;
			case Info.Event:
				{
					yield return CloseMsgPopup ();
				}
				break;
			case Info.SelectMap:
				{
					selectMapPopup.ChangeVisibility (false);
				}
				break;
			case Info.SelectSkill:
				{
					selectSkillPopup.ChangeVisibility (false);
				}
				break;
			case Info.Skill:
				{
					skillPopup.ChangeVisibility (false);
				}
				break;
			case Info.Mission:
				{
					missionPopup.ChangeVisibility (false);
				}
				break;
			case Info.Item:
			case Info.Storage:
				{
					itemPopup.ChangeVisibility (false);
				}
				break;
			case Info.WorkResult:
				{
					yield return CloseMsgPopup ();
				}
				break;
			case Info.Ability:
				{
					abilityPopup.ChangeVisibility (false);
				}
				break;
			case Info.Fusion:
				{
					fusionPopup.ChangeVisibility (false);
				}
				break;
			default:
				throw new NotImplementedException (page.ToString());
			}
			yield return null;
		}

		public void Alert (string msg){
			var popup = OpenMsgPopup ();
			popup.Message = msg;
		}

		public IEnumerator HandleCommand(string msg, object args, Action<Exception> callback){
			switch (msg) {
			case "itemPopup_equip_item":
			case "itemPopup_unequip_item":
				if (menuHome.isActiveAndEnabled) {
					menuHome.GetComponent<MenuHome> ().UpdateUI (model, Place.Pocket);
				}
				break;
			case "click_selectSkillPopup":
				yield return HideInfo (Info.SelectSkill);
				callback (null);
				break;
			case "click_selectMapPopup_close":
				yield return HideInfo (Info.SelectMap);
				callback (null);
				break;
			case "click_skillPopup_close":
				yield return HideInfo (Info.Skill);
				callback (null);
				break;
			case "click_missionPopup_close":
				yield return HideInfo (Info.Mission);
				callback (null);
				break;
			case "click_fusionPopup_close":
				yield return HideInfo (Info.Fusion);
				callback (null);
				break;
			case "click_abilityPopup_close":
				yield return HideInfo (Info.Ability);
				callback (null);
				break;
			case "click_itemPopup_close":
				yield return HideInfo (Info.Item);
				callback (null);
				break;
			case "click_msgPopup_close":
				yield return CloseMsgPopup ();
				break;
			default:
				{
					if (msg.Contains ("click_menuMap")) {
						var popup = menuMap.GetComponent<MenuMap> ();
						if (popup == null) {
							callback (new Exception ("MenuMap is null"));
							yield break;
						}
						yield return popup.HandleCommand (model, msg, args, callback);
					}

					if (msg.Contains ("click_selectMapPopup")) {
						var popup = selectMapPopup.GetComponent<SelectMapPopup> ();
						if (popup == null) {
							callback (new Exception ("SelectMapPopup is null"));
							yield break;
						}
						yield return popup.HandleCommand (model, msg, args, callback);
					}


					if (msg.Contains ("click_selectSkillPopup")) {
						var popup = selectSkillPopup.GetComponent<SelectSkillPopup> ();
						if (popup == null) {
							callback (new Exception ("SelectSkillPopup is null"));
							yield break;
						}
						yield return popup.HandleCommand (model, msg, args, callback);
					}

					if (msg.Contains ("click_itemPopup")) {
						var popup = itemPopup.GetComponent<ItemPopup2> ();
						if (popup == null) {
							callback (new Exception ("ItemPopup2 is null"));
							yield break;
						}
						yield return popup.HandleCommand (model, msg, args, callback);
					}

					if (msg.Contains ("click_fusionPopup")) {
						var popup = fusionPopup.GetComponent<FusionPopup> ();
						if (popup == null) {
							callback (new Exception ("FusionPopup is null"));
							yield break;
						}
						yield return popup.HandleCommand (model, msg, args, callback);
					}

					if (msg.Contains ("click_missionPopup")) {
						var popup = missionPopup.GetComponent<MissionPopup> ();
						if (popup == null) {
							callback (new Exception ("MissionPopup is null"));
							yield break;
						}
						/*if (msg.Contains ("click_missionPopup_item_")) {
							var selectIdx = popup.CurrMissionIndex(msg);
							var mid = popup.CurrMissionData [selectIdx];
							yield return MissionDialog (mid);
						}*/
						yield return popup.HandleCommand (model, msg, args, callback);
					}

					if (msg.Contains ("click_skillPopup")) {
						var popup = skillPopup.GetComponent<SkillPopup> ();
						if (popup == null) {
							callback (new Exception ("SkillPopup is null"));
							yield break;
						}
						yield return popup.HandleCommand (model, msg, args, callback);
					}
				}
				break;
			}
			yield return null;
		}

		public IEnumerator MissionDialog(string mid){
			var cfg = ConfigNpcMission.Get (mid);
			var npcCfg = ConfigNpc.Get (cfg.Npc);
			var dialog = string.Format("{0}:{1}", npcCfg.Name, cfg.Dialog);
			yield return handleFungus.MissionDialog (dialog);
		}

		#region msg popup
		public Popup msgPopObject;
		public List<Popup> msgPops;
		public MessagePopup OpenMsgPopup(Transform parent = null){
			if(parent == null){
				parent = msgPopObject.transform.parent;
			}
			var popup = Instantiate (msgPopObject, parent, false);
			var info = popup.GetComponent<MessagePopup> ();
			if (info == null) {
				throw new Exception ("沒有加入MessagePopup Component");
			}
			msgPops.Add (popup);
			// 注意：
			// 初始化旗標必須全部重設，不然UIElement中會發生null pointer的情況
			// 可能是因為沒有按ZUI標準使用方式
			// 為了讓Popup可以同時出現多種，並一種可以重復出現，這個Popup是沒有透過ZUIManager.OpenPopup打開的
			popup.Initialized = false;
			foreach (var i in popup.AnimatedElements) {
				i.Initialized = false;
			}
			popup.ChangeVisibility (true);
			return info;
		}
		public MessagePopup GetTopMsgPopup(){
			return msgPops.Last ().GetComponent<MessagePopup>();
		}
		public IEnumerator CloseMsgPopup(){
			if (msgPops.Count == 0) {
				yield break;
			}
			var popup = msgPops.Last ();
			popup.ChangeVisibility (false);
			msgPops.Remove (popup);
			yield return new WaitForSeconds (0.5f);
			Destroy (popup.gameObject);
		}
		#endregion
	}
}

