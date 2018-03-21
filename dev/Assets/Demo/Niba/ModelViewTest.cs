using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HanUtil;
using Common;
using System.Linq;
using View;
using HanRPGAPI;

namespace Model
{
	public class ModelViewTest : MonoBehaviour
	{
		public HandleDebug debug;
		public HanView hanView;
		public Model defaultModel;

		IView view;
		IModel model;

		void Awake(){
			model = defaultModel;
			view = hanView;
			view.ModelGetter = model;
		}

		void Start ()
		{
			model.NewGame ();
			StartCoroutine (TestAll());
		}

		IEnumerator TestAll(){
			//yield return TestNpcMission (model, view);
			yield return TestHomeStorage (model, view);
			yield return TestFusionView (model, view);
			yield return TestFight (model, view);
			yield return TestFusion (model, view);
			yield return TestShowInfo (model, view);
			yield return TestMap (model, view);
			yield return TestWeapon (model, view);
		}

		static IEnumerator TestNpcMission(IModel model, IView view){
			var ms = model.AvailableNpcMissions;
			var grass10Mission = ms
				.Select (ConfigNpcMission.Get)
				.Where (m => m.Dialog == "幫我拿10個草")
				.FirstOrDefault ();
			if (string.IsNullOrEmpty(grass10Mission.ID)) {
				foreach (var m in ms) {
					var cfg = ConfigNpcMission.Get (m);
					Debug.Log (cfg.Dialog);
				}
				throw new Exception ("必須有草任務");
			}
			Debug.Log ("領取草任務");
			model.AcceptMission (grass10Mission.ID);

			Debug.Log ("獲得草10個");
			var grass10 = new Item {
				prototype = ConfigItem.ID_grass,
				count = 10
			};
			model.AddItemToStorage (grass10, Place.Storage);

			Debug.Log ("判斷任務");
			var completedMs = model.CheckMissionStatus ();
			if (completedMs.Count == 0) {
				throw new Exception ("必須有過任務");
			}
			var hasGrass10 = completedMs.Exists (id => id == grass10Mission.ID);
			if (hasGrass10 == false) {
				throw new Exception ("必須過了草任務");
			}
			var rewards = completedMs.SelectMany (model.CompleteMission);
			foreach (var r in rewards) {
				Debug.LogError (r.prototype);
			}
			ms = model.AvailableNpcMissions;
			var wood10Mission = ms
				.Select (ConfigNpcMission.Get)
				.Where (m => m.Dialog == "幫我拿10個木")
				.FirstOrDefault ();
			if (string.IsNullOrEmpty(wood10Mission.ID)) {
				foreach (var m in ms) {
					var cfg = ConfigNpcMission.Get (m);
					Debug.Log (cfg.Dialog);
				}
				throw new Exception ("解完草任務必須有木任務");
			}
			yield return null;
		}

		static IEnumerator TestHomeStorage(IModel model, IView view){
			model.ClearStorage (Place.Storage);
			model.ClearStorage (Place.Pocket);
			model.ClearStorage (Place.Map);

			Debug.Log ("加入2個道具");
			Item item;
			item.count = 1;
			item.prototype = ConfigItem.ID_grass;
			model.AddItemToStorage (item, Place.Storage);

			item.prototype = ConfigItem.ID_woodSword;
			model.AddItemToStorage (item, Place.Storage);

			Debug.Log ("判斷道具是否存在");
			if (model.GetMapPlayer(Place.Storage).storage.Count != 2) {
				throw new Exception ("家裡必須有2個道具:"+model.GetMapPlayer(Place.Storage).storage.Count);
			}

			Exception e = null;
			yield return view.ShowInfo(Info.Storage, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			yield return new WaitForSeconds (1f);
			yield return view.HideInfo (Info.Storage);

			Debug.Log ("加入道具到口袋");
			item.prototype = ConfigItem.ID_woodSword;
			model.AddItemToStorage (item, Place.Pocket);
			if (model.GetMapPlayer(Place.Pocket).storage.Count != 1) {
				throw new Exception ("口袋必須有1個道具");
			}

			Debug.Log ("將口袋道具裝到身上");
			model.EquipWeapon (item, Place.Pocket, Place.Pocket);
			if (model.GetMapPlayer(Place.Pocket).storage.Count != 0) {
				throw new Exception ("裝備後口袋必須沒有道具");
			}

			Debug.Log ("直接從家裡裝裝備");
			item.prototype = ConfigItem.ID_woodSword;
			model.EquipWeapon (item, Place.Pocket, Place.Storage);
			if (model.GetMapPlayer(Place.Pocket).weapons.Count != 2) {
				throw new Exception ("裝備後裝備數量必須為2");
			}
			yield return view.ShowInfo(Info.Item, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			yield return new WaitForSeconds (1f);
			yield return view.HideInfo (Info.Item);

			yield return view.ShowInfo(Info.Storage, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			yield return new WaitForSeconds (1f);
			yield return view.HideInfo (Info.Storage);
		}

		static IEnumerator TestFusionView(IModel model, IView view){
			Item item;
			item.count = 1;
			for (var i = 0; i < ConfigItem.ID_COUNT; ++i) {
				item.prototype = ConfigItem.Get (i).ID;
				model.AddItemToStorage (item, Place.Map);
			}

			Exception e = null;
			yield return view.ShowInfo (Info.Fusion, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			yield return view.HideInfo (Info.Fusion);
		}

		static IEnumerator TestWeapon(IModel model, IView view){
			Exception e = null;
			model.NewMap (MapType.Unknown);
			model.EnterMap ();
			yield return view.ChangePage (Page.Game, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			Debug.Log ("先拆除所有裝備");
			foreach (var w in model.GetMapPlayer(Place.Map).weapons.ToList()) {
				model.UnequipWeapon (w, Place.Map, Place.Map);
			}
			Debug.Log ("先丟掉所有道具");
			model.ClearStorage (Place.Map);

			var fight = model.PlayerFightAbility(Place.Map);
			Debug.Log (fight);

			var weapon = HanRPGAPI.Item.Empty;
			weapon.prototype = ConfigItem.ID_woodSword;
			weapon.count = 1;

			model.AddItemToStorage (weapon, Place.Map);
			model.EquipWeapon (weapon, Place.Map, Place.Map);
			try{
				model.EquipWeapon (weapon, Place.Map, Place.Map);
				throw new Exception ("裝備沒有的裝備必須丟出例外");
			}catch(Exception e2){
				if (e2.Message.IndexOf ("無法裝備，請檢查:沒有那個道具") == -1) {
					throw new Exception ("裝備沒有的裝備必須丟出特定例外:"+e2.Message);
				}
			}
			model.AddItemToStorage (weapon, Place.Map);
			model.EquipWeapon (weapon, Place.Map, Place.Map);

			model.AddItemToStorage (weapon, Place.Map);
			try{
				model.EquipWeapon (weapon, Place.Map, Place.Map);
				throw new Exception ("裝備超過最大數量限制必須丟出例外");
			}catch(Exception e2){
				if (e2.Message.IndexOf ("無法裝備，請檢查:那個位置已經滿") == -1) {
					throw new Exception ("裝備超過最大數量限制必須丟出特定例外:"+e2.Message);
				}
			}
			weapon.prototype = ConfigItem.ID_grassKen;
			model.AddItemToStorage (weapon, Place.Map);
			model.EquipWeapon (weapon, Place.Map, Place.Map);

			fight = model.PlayerFightAbility(Place.Map);
			Debug.Log (fight);
			yield return view.ShowInfo (Info.Ability, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			yield return view.HideInfo(Info.Ability);
			model.ExitMap ();
		}

		static IEnumerator TestFight(IModel model, IView view){
			UnityEngine.Random.InitState (1);
			Exception e = null;
			model.NewMap (MapType.Unknown);
			model.EnterMap ();
			yield return view.ChangePage (Page.Game, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			yield return view.ShowInfo (Info.Map, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			model.MoveRight ();
			yield return view.ShowInfo (Info.Map, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			yield return view.ShowInfo (Info.Ability, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			yield return new WaitForSeconds (2f);
			yield return view.HideInfo (Info.Ability);

			yield return view.ShowInfo (Info.Work, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			yield return new WaitForSeconds (2f);

			var atkWork = model.Works.Where (w => {
				return w.description == Description.WorkAttack;
			}).FirstOrDefault ();
			if(atkWork.Equals(Description.Empty)){
				throw new Exception ("必須要有敵人可攻擊");
			}
			model.StartWork (atkWork);
			model.ApplyWork ();

			foreach (var result in model.WorkResults) {
				Debug.Log (result.description);
			}
			yield return view.ShowInfo (Info.WorkResult, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			yield return new WaitForSeconds (2f);
			yield return view.HideInfo (Info.WorkResult);
			model.ExitMap ();
		}

		static IEnumerator TestFusion(IModel model, IView view){
			yield return null;
			/*
			UnityEngine.Random.InitState (1);
			model.ClearStorage (Place.Map);

			Exception e = null;
			yield return model.NewMap (MapType.Unknown, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			yield return view.ChangePage (Page.Game, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			var item = Common.Item.Empty;
			item.prototype = ConfigItem.ID_feather;
			item.count = 5;
			model.AddItemToStorage (item, Place.Map);

			item.prototype = ConfigItem.ID_wood;
			item.count = 5;
			model.AddItemToStorage (item, Place.Map);

			var canFusionArrows = model.IsCanFusion (ConfigItem.ID_arrows, Place.Map);
			if (canFusionArrows > 0) {
				yield return view.ShowInfo (Info.Fusion, e2 => {
					e = e2;
				});
				if (e != null) {
					throw e;
				}
				throw new Exception ("現在必須不能合成箭矢，因為道具不該足夠:"+canFusionArrows);
			}

			item.prototype = ConfigItem.ID_gravel;
			item.count = 5;
			model.AddItemToStorage (item, Place.Map);

			foreach(var i in model.GetMapPlayer(Place.Map).storage){
				Debug.Log (i);
			}
			yield return view.ShowInfo (Info.Item, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			yield return new WaitForSeconds (2f);
			canFusionArrows = model.IsCanFusion (ConfigItem.ID_arrows, Place.Map);
			if (canFusionArrows <= 0) {
				throw new Exception ("必須能合成箭矢");
			}

			yield return view.HideInfo (Info.Item);

			Item twoArrow;
			twoArrow.prototype = ConfigItem.ID_arrows;
			twoArrow.count = 2;
			model.Fusion (twoArrow, Place.Map);
			foreach(var i in model.GetMapPlayer(Place.Map).storage){
				Debug.Log (i);
			}
			var arrows = model.GetMapPlayer(Place.Map).storage.Where(it=>{
				return it.prototype == ConfigItem.ID_arrows;
			}).FirstOrDefault();
			if (arrows.Equals (Common.Item.Empty)) {
				throw new Exception ("必須有箭矢");
			}
			if (arrows.count != 2) {
				throw new Exception ("箭矢必須有2個");
			}
			yield return view.ShowInfo (Info.Item, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			yield return new WaitForSeconds (2f);
			yield return view.HideInfo (Info.Item);
			*/
		}

		static IEnumerator TestShowInfo(IModel model, IView view){
			UnityEngine.Random.InitState (1);

			Exception e = null;
			model.NewMap (MapType.Unknown);
			yield return view.ChangePage (Page.Game, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			view.ShowInfo (Info.Work, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			yield return new WaitForSeconds (2f);
			model.EnterMap ();

			model.MoveRight ();
			var result = model.MoveResult;
			if(result.isMoveSuccess){
				yield return view.ShowInfo (Info.Map, e2 => {
					e = e2;
				});
				if (e != null) {
					throw e;
				}
				if (result.HasEvent) {
					yield return view.ShowInfo (Info.Event, e2 => {
						e = e2;
					});
					if (e != null) {
						throw e;
					}
					yield return new WaitForSeconds (2f);
					yield return view.HideInfo (Info.Event);
				}
			}
			model.ApplyMoveResult();
			model.ClearMoveResult();

			yield return view.ShowInfo (Info.Work, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			yield return new WaitForSeconds (2f);
			yield return view.HideInfo (Info.Work);

			yield return view.ShowInfo (Info.Item, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			model.ExitMap ();
		}

		static IEnumerator TestMap(IModel model, IView view){
			UnityEngine.Random.InitState (1);

			Exception e = null;
			model.NewMap (MapType.Unknown);
			if (e != null) {
				throw e;
			}
			yield return view.ChangePage (Page.Game, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			model.EnterMap ();
			Debug.Log ("向右移20步");
			for (var i = 0; i < 20; ++i) {
				model.MoveRight ();
				var result = model.MoveResult;
				if(result.isMoveSuccess){
					yield return view.ShowInfo (Info.Map, e2 => {
						e = e2;
					});
					if (e != null) {
						throw e;
					}
				}
				model.ClearMoveResult();
				yield return new WaitForSeconds (0f);
			}
			Debug.Log ("向下移20步");
			for (var i = 0; i < 20; ++i) {
				model.MoveDown ();
				var result = model.MoveResult;
				if(result.isMoveSuccess){
					yield return view.ShowInfo (Info.Map, e2 => {
						e = e2;
					});
					if (e != null) {
						throw e;
					}
				}
				model.ClearMoveResult();
				yield return new WaitForSeconds (0f);
			}
			Debug.Log ("目前位置:" + model.GetMapPlayer(Place.Map).position);
			var objs = model.MapObjectsAt (model.GetMapPlayer(Place.Map).position);
			foreach (var obj in objs) {
				Debug.Log ("有物件:" + obj.type);
			}
			var works = model.Works;
			Debug.Log ("取得目前工作數量:" + works.Count ());
			if (works.Count () > 0) {
				if (model.GetMapPlayer(Place.Map).IsWorking) {
					throw new Exception ("現在必須沒有工作在身");
				}

				var firstWork = works.First ();
				Debug.Log ("開始第一件工作，工作為:"+firstWork.description);
				model.StartWork (firstWork);

				if (model.GetMapPlayer(Place.Map).IsWorking == false) {
					throw new Exception ("現在必須有工作在身");
				}
				var finishedTime = new DateTime (model.GetMapPlayer(Place.Map).workFinishedTime);
				Debug.Log ("工作結束時間為:"+finishedTime);

				model.ApplyWork ();

				switch (firstWork.description) {
				case Description.WorkCollectResource:
					{
						var collectTargetId = int.Parse (firstWork.values.Get ("mapObjectId"));
						var target = model.MapObjects [collectTargetId];
						var targetInfo = model.ResourceInfos [target.infoKey];
						Debug.Log ("采集"+targetInfo.type+"結束");
						if (target.died == false) {
							throw new Exception ("采集完的物件必須死亡");
						}
						yield return view.ShowInfo (Info.Map, e2 => {
							e = e2;
						});
						if (e != null) {
							throw e;
						}
						foreach (var item in model.GetMapPlayer(Place.Map).storage) {
							Debug.Log ("擁有" + item.prototype +"/"+item.count);
						}
						yield return view.ShowInfo (Info.Item, e2 => {
							e = e2;
						});
						if (e != null) {
							throw e;
						}
						yield return new WaitForSeconds (2);
						yield return view.HideInfo(Info.Item);
					}
					break;
				case Description.WorkAttack:
					break;
				}
			}
			Debug.Log ("上移1格");
			model.MoveUp ();
			yield return view.ShowInfo (Info.Map, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			model.ClearMoveResult();

			Debug.Log ("直接完成第1個工作");
			model.StartWork (model.Works.First());
			model.ApplyWork ();
			yield return view.ShowInfo (Info.Map, e2 => {
				e = e2;
			});
			if (e != null) {
				throw e;
			}
			model.ExitMap ();
		}
	}
}

