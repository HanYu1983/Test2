using System;
using UnityEngine;
using View;
using Model;
using System.Collections;
using System.Linq;
using HanRPGAPI;

namespace Common
{
	public class Controller : MonoBehaviour
	{
		public HanView hanView;
		public Model.Model defaultModel;

		public IView view;
		public IModel model;

		void Awake(){
			view = hanView;
			model = defaultModel;
			view.ModelGetter = model;
		}

		void Start(){
			Common.OnEvent += Common_OnEvent;
			StartCoroutine (view.ChangePage (Page.Title, e => {
				if (e != null) {
					HandleException (e);
				}
			}));
			/*Item item;
			item.count = 1;
			for (var i = 0; i < ConfigItem.ID_COUNT; ++i) {
				item.prototype = ConfigItem.Get (i).ID;
				model.AddItemToStorage (item, model.MapPlayer);
			}*/
		}

		void HandleException(Exception e){
			view.Alert (e.Message);
			Debug.LogError (e.Message);
			handleCommandCoroutine = null;
		}

		Coroutine handleCommandCoroutine;

		void Common_OnEvent (string msg, object args)
		{
			if (handleCommandCoroutine != null) {
				Debug.LogWarning ("上一次的動畫處理還沒完成:"+msg);
				return;
			}
			handleCommandCoroutine = StartCoroutine (HandleCommand (msg, args));
		}

		IEnumerator HandleCommand(string msg, object args){
			Debug.Log ("[Controller]:"+msg);
			Exception e = null;
			switch (msg) {
			case "selectMapPopup_selectMap":
				{
					yield return view.HideInfo (Info.SelectMap);

					var mt = (MapType)args;
					// 創新地圖
					model.NewMap (mt);
					// 再進入
					model.EnterMap ();
					yield return view.ChangePage (Page.Game, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
					yield return view.ShowInfo(Info.Map, e2=>{
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
				}
				break;
			case "selectSkillPopup_selectSkill":
				{
					var work = (Description)args;
					yield return view.HideInfo (Info.SelectSkill);
					try {
						model.StartWork (work);
						model.ApplyWork ();
					} catch (Exception e2) {
						e = e2;
					}
					if (e != null) {
						HandleException (e);
						yield break;
					}
					yield return view.ShowInfo (Info.WorkResult, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
					yield return view.ShowInfo (Info.Map, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
					if (model.GetMapPlayer (Place.Map).IsDied) {
						e = new Exception ("冒險者掛了");
					}
					if (e != null) {
						HandleException (e);
						yield break;
					}
					var missionOK = model.CheckMissionStatus ();
					if (missionOK.Count > 0) {
						view.Alert ("mission ok");
					}
					model.ClearMissionStatus ();
				}
				break;
			case "skillPopup_active":
				{
					if (model.PlayState == PlayState.Play) {
						e = new Exception ("冒險中不能修改招式");
					}
					if (e != null) {
						HandleException (e);
						yield break;
					}
					var skill = (string)args;
					try{
						model.EquipSkill(Common.PlaceAt (model.PlayState), skill);
					}catch(Exception e2){
						e = e2;
					}
					if (e != null) {
						HandleException (e);
						yield break;
					}
					yield return view.ShowInfo(Info.Skill, e2=>{
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
				}
				break;
			case "skillPopup_inactive":
				{
					if (model.PlayState == PlayState.Play) {
						e = new Exception ("冒險中不能修改招式");
					}
					if (e != null) {
						HandleException (e);
						yield break;
					}
					var skill = (string)args;
					try{
						model.UnequipSkill(Common.PlaceAt (model.PlayState), skill);
					}catch(Exception e2){
						e = e2;
					}
					if (e != null) {
						HandleException (e);
						yield break;
					}
					yield return view.ShowInfo(Info.Skill, e2=>{
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
				}
				break;
			case "click_title_newgame":
				{
					model.NewGame ();
					yield return OpenPage (Page.Home);
				}
				break;
			case "click_title_load":
				{
					if (model.LoadGame () == false) {
						HandleException (new Exception ("你沒有存檔"));
						yield break;
					}
					var nextPage = model.PlayState == PlayState.Play ?
						Page.Game : Page.Home;
					yield return view.ChangePage (nextPage, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
					if (nextPage == Page.Game) {
						yield return view.ShowInfo(Info.Map, e2=>{
							e = e2;
						});
						if (e != null) {
							HandleException (e);
							yield break;
						}
					}
				}
				break;
			case "fusionRequireView_ok":
				{
					var info = (object[])args;
					var fusionTarget = (Item)info [0];
					var who = (Place)info [1];
					try{
						model.Fusion (fusionTarget, who);
					}catch(Exception e2){
						HandleException(e2);
						yield break;
					}
				}
				break;
			case "itemPopup_move_item":
				{
					var info = (object[])args;
					var item = (Item)info [0];
					var whosStorage = (Place)info [1];
					// 如果現在是家裡箱子，就移動到口袋
					// 反之就相反
					var toStorage = whosStorage == Place.Pocket ?
						Place.Storage : Place.Pocket;
					try{
						model.MoveItem(whosStorage, toStorage, item);
					}catch(Exception e2){
						HandleException(e2);
						yield break;
					}
					var returnTo = 
						whosStorage == Place.Storage ? Info.Storage : Info.Item;
					yield return view.ShowInfo (returnTo, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
				}
				break;
			case "itemPopup_use_item":
				{
				}
				break;
			case "itemPopup_equip_item":
				{
					var info = (object[])args;
					var weapon = (Item)info [0];
					var whosStorage = (Place)info [1];
					try{
						model.EquipWeapon (weapon, whosStorage, whosStorage);
					}catch(Exception e2){
						e = e2;
					}
					if (e != null) {
						HandleException (e);
						yield break;
					}
					if (whosStorage == Place.Storage) {
						e = new Exception ("裝備時不可能在倉庫");
					}
					if (e != null) {
						HandleException (e);
						yield break;
					}
					yield return view.ShowInfo (Info.Item, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
					yield return view.HandleCommand (msg, args, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
				}
				break;
			case "itemPopup_unequip_item":
				{
					var info = (object[])args;
					var weapon = (Item)info [0];
					var whosStorage = (Place)info [1];
					try{
						model.UnequipWeapon (weapon, whosStorage, whosStorage);
					}catch(Exception e2){
						e = e2;
					}
					if (e != null) {
						HandleException (e);
						yield break;
					}
					if (whosStorage == Place.Storage) {
						e = new Exception ("裝備時不可能在倉庫");
					}
					if (e != null) {
						HandleException (e);
						yield break;
					}
					yield return view.ShowInfo (Info.Item, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
					yield return view.HandleCommand (msg, args, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
				}
				break;
			case "missionPopup_completeMission":
				{
					if (model.PlayState != PlayState.Home) {
						HandleException (new Exception ("在家裡才能完成任務"));
						yield break;
					}
					var id = (string)args;
					try{
						var rewards = model.CompleteMission (id);
						view.Alert(string.Join(",", rewards.Select(i=>i.ToString()).ToArray()));
					}catch(Exception e2){
						e = e2;
					}
					if (e != null) {
						HandleException (e);
						yield break;
					}
					yield return view.ShowInfo (Info.Mission, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
				}
				break;
			case "click_home_map":
				{
					yield return view.ShowInfo (Info.SelectMap, e2 => {
						e = e2;
					});
					/*
					// 創新地圖
					model.NewMap (MapType.Random);
					// 再進入
					model.EnterMap ();
					yield return view.ChangePage (Page.Game, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
					yield return view.ShowInfo(Info.Map, e2=>{
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
					*/
				}
				break;
			case "click_home_item":
				{
					yield return view.ShowInfo (Info.Storage, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
				}
				break;
			case "click_home_pocket":
				{
					yield return view.ShowInfo (Info.Item, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
				}
				break;
			case "click_home_fusion":
				{
					yield return view.ShowInfo (Info.Fusion, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
				}
				break;
			case "click_home_mission":
				{
					yield return view.ShowInfo (Info.Mission, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
				}
				break;
			case "click_home_skill":
				{
					yield return view.ShowInfo (Info.Skill, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
				}
				break;
			case "click_map_home":
				{
					yield return OpenPage (Page.Home);
					model.ExitMap ();
				}
				break;
			case "click_map_fusion":
				{
					yield return view.ShowInfo(Info.Fusion, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
				}
				break;
			case "click_map_down":
			case "click_map_left":
			case "click_map_right":
			case "click_map_up":
				{
					try{
						if (msg == "click_map_down") {
							model.MoveDown ();
						}
						if (msg == "click_map_left") {
							model.MoveLeft ();
						}
						if (msg == "click_map_right") {
							model.MoveRight ();
						}
						if (msg == "click_map_up") {
							model.MoveUp ();
						}
					}catch(Exception e2){
						e = e2;
					}
					if (e != null) {
						HandleException (e);
						yield break;
					}
					yield return HandleAfterMove ();
				}
				break;
			case "click_map_item":
				{
					yield return view.ShowInfo (Info.Item, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
				}
				break;
			case "click_map_ability":
				{
					yield return view.ShowInfo (Info.Ability, e2 => {
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
				}
				break;
				/*
			case "click_map_work_0":
			case "click_map_work_1":
			case "click_map_work_2":
			case "click_map_work_3":
			case "click_map_work_4":
			case "click_map_work_5":
			case "click_map_work_6":
				*/
			case "menuMap_work":
				{
					var selectWork = (Description)args;
					/*
					var idx = int.Parse(msg.Replace ("click_map_work_", ""));
					var selectWork = model.Works.ToList () [idx];
					*/
					if (selectWork.description == Description.WorkSelectSkillForEnemy ||
						selectWork.description == Description.WorkUseSkillForEnemyAll) {
						yield return view.ShowInfo (Info.SelectSkill, selectWork, e2 => {
							e = e2;
						});
						if (e != null) {
							HandleException (e);
							yield break;
						}
					} else {
						try {
							model.StartWork (selectWork);
							model.ApplyWork ();
						} catch (Exception e2) {
							e = e2;
						}
						if (e != null) {
							HandleException (e);
							yield break;
						}
						yield return view.ShowInfo (Info.WorkResult, e2 => {
							e = e2;
						});
						if (e != null) {
							HandleException (e);
							yield break;
						}
						yield return view.ShowInfo (Info.Map, e2 => {
							e = e2;
						});
						if (e != null) {
							HandleException (e);
							yield break;
						}
						if (model.GetMapPlayer (Place.Map).IsDied) {
							e = new Exception("冒險者掛了");
						}
						if (e != null) {
							HandleException (e);
							yield break;
						}
						var missionOK = model.CheckMissionStatus ();
						if (missionOK.Count > 0) {
							view.Alert ("mission ok");
						}
						model.ClearMissionStatus ();
					}
				}
				break;
			default:
				yield return view.HandleCommand (msg, args, e2=>{
					e = e2;
				});
				if (e != null) {
					HandleException (e);
					yield break;
				}
				break;
			}
			handleCommandCoroutine = null;
		}

		IEnumerator HandleAfterMove(){
			Exception e = null;
			var result = model.MoveResult;
			if (result.isMoveSuccess) {
				yield return view.ShowInfo(Info.Map, e2=>{
					e = e2;
				});
				if (e != null) {
					HandleException (e);
					yield break;
				}
				if (result.HasEvent) {
					yield return view.ShowInfo(Info.Event, e2=>{
						e = e2;
					});
					if (e != null) {
						HandleException (e);
						yield break;
					}
				}
			}
			model.ApplyMoveResult();
			model.ClearMoveResult ();
		}

		IEnumerator OpenPage(Page page){
			Exception e = null;
			yield return view.ChangePage (page, e2 => {
				e = e2;
			});
			if (e != null) {
				HandleException (e);
				yield break;
			}
			if (page == Page.Home) {
				// 自動領取任務
				foreach (var m in model.AvailableNpcMissions) {
					model.AcceptMission (m);
				}
                /*
				var misNot = model.CheckMissionNotification ();
				foreach (var m in misNot) {
					yield return view.MissionDialog (m);
					model.MarkMissionNotification (m);
				}
                */
			}
		}
	}
}

