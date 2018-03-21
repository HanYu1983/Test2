using System;
using UnityEngine;
using UnityEngine.UI;
using Common;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using HanRPGAPI;

namespace View
{
	public class ItemPopup2 : MonoBehaviour
	{
		public ListView listView;
		public ItemDataProvider itemDataProvider;
		public AbilityView abilityView;
		public Button[] btns;

		void Awake(){
			listView.DataProvider = itemDataProvider;
		}
			
		public Place Who {
			get;
			set;
		}

		public void UpdateUI(IModelGetter model){
			var popup = this;
			popup.itemDataProvider.Data = model.GetMapPlayer (Who).storage;
			popup.listView.UpdateDataView (model);
			popup.listView.CurrItemLabel (model, popup.SelectIndex);
			popup.UpdateButtonLabel (model, Who);
			popup.abilityView.UpdateAbility(model, Who);
		}
		/// <summary>
		/// 更新按鈕文字
		/// 每次道具裝備或拆掉後呼叫
		/// </summary>
		/// <param name="model">Model.</param>
		void UpdateButtonLabel(IModelGetter model, Place who_){
			if (who_ == Place.Storage) {
				Debug.LogWarning ("倉庫中不顯示裝備");
				return;
			}
			var who = model.GetMapPlayer (who_);
			// 頭
			var btn_head = btns.Where (btn => {
				return btn.gameObject.name == "btn_head";
			}).FirstOrDefault();
			if (btn_head == null) {
				throw new Exception ("xxx");
			}
			btn_head.GetComponentInChildren<Text> ().text = "頭";
			var head = who.weapons.Where (item => {
				var cfg = ConfigItem.Get (item.prototype);
				return cfg.Position == ConfigWeaponPosition.ID_head;
			}).FirstOrDefault ();
			if (head.Equals (Item.Empty) == false) {
				var cfg = ConfigItem.Get(head.prototype);
				btn_head.GetComponentInChildren<Text> ().text = cfg.Name;
			}
			// 身
			var btn_body = btns.Where (btn => {
				return btn.gameObject.name == "btn_body";
			}).FirstOrDefault();
			if (btn_body == null) {
				throw new Exception ("xxx");
			}
			btn_body.GetComponentInChildren<Text> ().text = "身";
			var body = who.weapons.Where (item => {
				var cfg = ConfigItem.Get (item.prototype);
				return cfg.Position == ConfigWeaponPosition.ID_body;
			}).FirstOrDefault ();
			if (body.Equals (Item.Empty) == false) {
				var cfg = ConfigItem.Get(body.prototype);
				btn_body.GetComponentInChildren<Text> ().text = cfg.Name;
			}
			// 腳
			var btn_foot = btns.Where (btn => {
				return btn.gameObject.name == "btn_foot";
			}).FirstOrDefault();
			if (btn_foot == null) {
				throw new Exception ("xxx");
			}
			btn_foot.GetComponentInChildren<Text> ().text = "腳";
			var foot = who.weapons.Where (item => {
				var cfg = ConfigItem.Get (item.prototype);
				return cfg.Position == ConfigWeaponPosition.ID_foot;
			}).FirstOrDefault ();
			if (foot.Equals (Item.Empty) == false) {
				var cfg = ConfigItem.Get(foot.prototype);
				btn_foot.GetComponentInChildren<Text> ().text = cfg.Name;
			}
			// 左右手
			var leftHandBtn = btns.Where (btn => {
				return btn.gameObject.name == "btn_leftHand";
			}).FirstOrDefault();

			var rightHandBtn = btns.Where (btn => {
				return btn.gameObject.name == "btn_rightHand";
			}).FirstOrDefault();

			if (leftHandBtn == null) {
				throw new Exception ("xxx");
			}
			if (rightHandBtn == null) {
				throw new Exception ("xxx");
			}

			leftHandBtn.GetComponentInChildren<Text> ().text = "左";
			rightHandBtn.GetComponentInChildren<Text> ().text = "右";

			var handBtns = new Button[]{ leftHandBtn, rightHandBtn };
			var handWeapons = who.weapons.Where (item => {
				var cfg = ConfigItem.Get(item.prototype);
				return cfg.Position == ConfigWeaponPosition.ID_hand;
			}).ToList();

			if (handWeapons.Count () > 2) {
				throw new Exception ("手上武器應該不可能超過2個，請檢查程式:"+handWeapons.Count());
			}

			for (var i = 0; i < handWeapons.Count; ++i) {
				var btn = handBtns [i];
				var weapon = handWeapons [i];
				var cfg = ConfigItem.Get(weapon.prototype);
				btn.GetComponentInChildren<Text> ().text = cfg.Name;
			}
			// 三個配件
			var a1Btn = btns.Where (btn => {
				return btn.gameObject.name == "btn_a1";
			}).FirstOrDefault();
			var a2Btn = btns.Where (btn => {
				return btn.gameObject.name == "btn_a2";
			}).FirstOrDefault();
			var a3Btn = btns.Where (btn => {
				return btn.gameObject.name == "btn_a3";
			}).FirstOrDefault();

			if (a1Btn == null) {
				throw new Exception ("xxx");
			}
			if (a2Btn == null) {
				throw new Exception ("xxx");
			}
			if (a3Btn == null) {
				throw new Exception ("xxx");
			}

			a1Btn.GetComponentInChildren<Text> ().text = "配件1";
			a2Btn.GetComponentInChildren<Text> ().text = "配件2";
			a3Btn.GetComponentInChildren<Text> ().text = "配件3";

			var aWeapons = who.weapons.Where (item => {
				var cfg = ConfigItem.Get(item.prototype);
				return cfg.Position == ConfigWeaponPosition.ID_accessory;
			}).ToList();

			var aBtns = new Button[]{ a1Btn, a2Btn, a3Btn };
			for (var i = 0; i < aWeapons.Count; ++i) {
				if (aBtns [i] == null) {
					continue;
				}
				var btn = aBtns [i];
				var weapon = aWeapons [i];
				var cfg = ConfigItem.Get(weapon.prototype);
				btn.GetComponentInChildren<Text> ().text = cfg.Name;
			}
		}



		#region controller
		public IEnumerator HandleCommand(IModelGetter model, string msg, object args, Action<Exception> callback){
			switch (msg) {
			case "click_itemPopup_move":
				{
					if (Who == Place.Map) {
						callback (new Exception ("冒險時不能移動道具"));
						yield break;
					}
					if (IsSelectNothing) {
						callback (new Exception ("你沒有選擇任何道具"));
						yield break;
					}
					// 防呆處理
					if (itemDataProvider.DataCount == 0) {
						callback (new Exception ("你沒有選擇任何道具"));
						yield break;
					}
					if (SelectIndex >= itemDataProvider.Data.Count) {
						callback (new Exception ("索引超多數量:"+SelectIndex));
						yield break;
					}
					var item = itemDataProvider.Data[SelectIndex];
					var info = new object[] {
						item, Who
					};
					Common.Common.Notify ("itemPopup_move_item", info);
				}
				break;
			case "click_itemPopup_equip":
				{
					if (Who == Place.Storage) {
						callback(new Exception ("倉庫中不能裝備"));
						yield break;
					}
					if (IsSelectNothing) {
						callback (new Exception ("你沒有選擇任何道具"));
						yield break;
					}
					// 防呆處理
					// 果為連續按裝備時道具會減少
					if (itemDataProvider.DataCount == 0) {
						callback (new Exception ("你沒有選擇任何道具"));
						yield break;
					}
					if (SelectIndex >= itemDataProvider.DataCount) {
						ClearSelectIndex ();
						callback (new Exception ("你沒有選擇任何道具"));
						yield break;
					}
					var item = itemDataProvider.Data[SelectIndex];
					var info = new object[] {
						item, Who
					};
					Common.Common.Notify ("itemPopup_equip_item", info);
				}
				break;
			case "click_itemPopup_unequip":
				{
					if (Who == Place.Storage) {
						callback(new Exception ("倉庫中不能裝備"));
						yield break;
					}
					if (HasLastPosition == false) {
						callback (new Exception ("你沒有選擇任何部位"));
						yield break;
					}
					var pos = "";
					var idx = 0;
					LastPosition(ref pos, ref idx);

					var who = model.GetMapPlayer (Who);
					var item = who.weapons.Where (i => {
						var cfg = ConfigItem.Get(i.prototype);
						return cfg.Position == pos;
					}).Skip(idx).FirstOrDefault();

					if (item.Equals (Item.Empty)) {
						callback (new Exception ("該部位沒有裝備"));
						yield break;
					}
					var info = new object[] {
						item, Who
					};
					Common.Common.Notify ("itemPopup_unequip_item", info);
				}
				break;
			case "click_itemPopup_use":
				{
					if (IsSelectNothing) {
						callback (new Exception ("你沒有選擇任何道具"));
						yield break;
					}
					var item = itemDataProvider.Data[SelectIndex];
					Common.Common.Notify ("itemPopup_use_item", item);
				}
				break;
			case "click_itemPopup_nouse":
				{
					if (IsSelectNothing) {
						callback (new Exception ("你沒有選擇任何道具"));
						yield break;
					}
					var item = itemDataProvider.Data[SelectIndex];
					Common.Common.Notify ("itemPopup_sell_item", item);
				}
				break;
			case "click_itemPopup_normalMode":
				{
					var popup = this;
					// 重新顯示所有道具
					popup.itemDataProvider.Data = model.GetMapPlayer(Who).storage;
					popup.itemDataProvider.ShowMode = ItemDataProvider.Mode.Normal;
					popup.listView.UpdateDataView (model);
				}
				break;
			case "click_itemPopup_head":
			case "click_itemPopup_body":
			case "click_itemPopup_foot":
			case "click_itemPopup_rightHand":
			case "click_itemPopup_leftHand":
			case "click_itemPopup_a1":
			case "click_itemPopup_a2":
			case "click_itemPopup_a3":
				{
					if (Who == Place.Storage) {
						callback(new Exception ("倉庫中不能裝備"));
						yield break;
					}
					var popup = this;
					// 修改為武器顯示模式
					popup.itemDataProvider.ShowMode = ItemDataProvider.Mode.Equip;
					// 取得點擊部位
					var pos = "";
					var idx = 0;
					ParsePosition (msg, ref pos, ref idx);
					// 依部分過濾道具
					popup.itemDataProvider.Data = model.GetMapPlayer(Who).storage.Where (item => {
						var cfg = ConfigItem.Get(item.prototype);
						return cfg.Type == ConfigItemType.ID_weapon && cfg.Position == pos;
					}).ToList();
					popup.listView.Page = 0;
					// 修改列表內容
					popup.listView.UpdateDataView (model);
					// 取消列表索引
					ClearSelectIndex ();
					// 更新狀態文字
					popup.listView.CurrItemLabel (model, SelectIndex);
					// 記錄部位
					RecordLastPosition (msg);
				}
				break;
			case "click_itemPopup_item_0":
			case "click_itemPopup_item_1":
			case "click_itemPopup_item_2":
			case "click_itemPopup_item_3":
			case "click_itemPopup_item_4":
			case "click_itemPopup_item_5":
			case "click_itemPopup_item_6":
			case "click_itemPopup_item_7":
			case "click_itemPopup_item_8":
			case "click_itemPopup_item_9":
				{
					var selectIdx = listView.CurrIndex (msg);
					// 記錄最後一次點擊的索引
					RecordSelectIndex(selectIdx);
					// 取消部位
					ClearLastPositionCommand ();
					// 若選到武器，修改顯示模式
					var cfg = ConfigItem.Get (itemDataProvider.Data[selectIdx].prototype);
					itemDataProvider.ShowMode = cfg.Type == ConfigItemType.ID_weapon ? ItemDataProvider.Mode.Equip : ItemDataProvider.Mode.Normal;
					listView.UpdateDataView (model);
					yield return listView.HandleCommand (model, msg, args, callback);
				}
				break;
			default:
				{
					if (msg.Contains (listView.CommandPrefix)) {
						yield return listView.HandleCommand (model, msg, args, callback);
					}
				}
				break;
			}
			yield return null;
		}
		#endregion
		#region helper
		void ParsePosition(string cmd, ref string pos, ref int idx) {
			switch (cmd) {
			case "click_itemPopup_head":
				{
					pos = ConfigWeaponPosition.ID_head;
				}
				break;
			case "click_itemPopup_body":
				{
					pos = ConfigWeaponPosition.ID_body;
				}
				break;
			case "click_itemPopup_foot":
				{
					pos = ConfigWeaponPosition.ID_foot;
				}
				break;
			case "click_itemPopup_leftHand":
				{
					pos = ConfigWeaponPosition.ID_hand;
					idx = 0;
				}
				break;
			case "click_itemPopup_rightHand":
				{
					pos = ConfigWeaponPosition.ID_hand;
					idx = 1;
				}
				break;
			case "click_itemPopup_a1":
				{
					pos = ConfigWeaponPosition.ID_accessory;
					idx = 0;
				}
				break;
			case "click_itemPopup_a2":
				{
					pos = ConfigWeaponPosition.ID_accessory;
					idx = 1;
				}
				break;
			case "click_itemPopup_a3":
				{
					pos = ConfigWeaponPosition.ID_accessory;
					idx = 2;
				}
				break;
			default:
				throw new Exception ("xxxx");
			}
		}
		#endregion

		#region lastSelectIndex
		public int lastSelectItemIndex = -1;
		/// <summary>
		/// 取消列表索引，在點擊部位後呼叫，讓道具使用賣掉等功能失效
		/// </summary>
		public void ClearSelectIndex(){
			lastSelectItemIndex = -1;
		}

		/// <summary>
		/// 記錄最後一次點擊的索引
		/// </summary>
		/// <param name="idx">Index.</param>
		void RecordSelectIndex(int idx){
			lastSelectItemIndex = idx;
		}

		bool IsSelectNothing{
			get{
				return lastSelectItemIndex == -1;
			}
		}
		public int SelectIndex{
			get{
				return lastSelectItemIndex;
			}
		}
		#endregion

		#region lastPosition
		public string lastPositionCommand = "";
		/// <summary>
		/// 取消部位的選取
		/// 在點擊列表後呼叫，讓拆裝備失效
		/// </summary>
		public void ClearLastPositionCommand(){
			lastPositionCommand = "";
		}
		/// <summary>
		/// 記錄最後點擊的裝備部位，用來之後拆掉裝備
		/// </summary>
		/// <param name="cmd">Cmd.</param>
		public void RecordLastPosition(string cmd){
			lastPositionCommand = cmd;
		}
		public bool HasLastPosition {
			get {
				try{
					var pos = "";
					var idx = 0;
					LastPosition(ref pos, ref idx);
					return true;
				}catch(Exception){
					return false;
				}
			}
		}
		public void LastPosition(ref string pos, ref int idx) {
			ParsePosition (lastPositionCommand, ref pos, ref idx);
		}
		#endregion
	}
}

