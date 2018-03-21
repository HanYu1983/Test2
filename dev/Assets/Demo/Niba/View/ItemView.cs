using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Common;
using System.Linq;
using System.Collections;
using HanRPGAPI;

namespace View
{
	public class ItemView : MonoBehaviour
	{
		void Awake(){
			PrepareButtonsAndEvents ();
		}

		#region view
		public GameObject itemParent;
		public int offset;
		public int limit;
		public Text txtCurrItem;
		public Button[] items;
		public string commandPrefix;
		public Button btnPageUp, btnPageDown;
		public string CommandPrefix{ get { return commandPrefix; } }

		void PrepareButtonsAndEvents(){
			if (limit > 10) {
				throw new Exception ("limit不能大於10");
			}

			items = itemParent.GetComponentsInChildren<Button> ();
			for(var i=0; i<items.Length; ++i){
				Func<int,UnityEngine.Events.UnityAction> closure = idx=>{
					return ()=>{
						Common.Common.Notify(string.Format("{0}_item_{1}",commandPrefix, idx), null);
					};
				};
				items [i].onClick.AddListener (closure (i));
			}

			btnPageUp.onClick.AddListener (() => {
				Common.Common.Notify(commandPrefix+"_pageup", null);
			});

			btnPageDown.onClick.AddListener (() => {
				Common.Common.Notify(commandPrefix+"_pagedown", null);
			});
		}
		/// <summary>
		/// 將點選列表觸發的指令還原成道具索引
		/// 用這個索引呼叫CurrItemLabel來改變狀態列
		/// </summary>
		/// <returns>The index.</returns>
		/// <param name="cmd">Cmd.</param>
		public int CurrIndex(string cmd) {
			try{
				var idx = int.Parse (cmd.Replace (commandPrefix+"_item_", ""));
				return idx + offset;
			}catch(Exception){
				throw new Exception ("指令格式錯誤:"+cmd);
			}
		}
		/// <summary>
		/// 修改狀態列文字
		/// 指定顯示第currIndex個道具
		/// </summary>
		/// <param name="model">Model.</param>
		/// <param name="currIndex">Curr index.</param>
		public void CurrItemLabel(IModelGetter model, int currIndex){
			if (data == null) {
				Debug.LogWarning ("還沒有設定data");
				return;
			}
			if (currIndex <0 || currIndex >= data.Count ()) {
				txtCurrItem.text = "你沒有選擇任何道具";
				return;
			}
			var item = data.Skip (currIndex).First ();
			var cfg = ConfigItem.Get (item.prototype);
			txtCurrItem.text = string.Format ("你選擇{0}", cfg.Name);
		}
		/// <summary>
		/// 更新列表
		/// 注意要先設定Data
		/// </summary>
		/// <param name="model">Model.</param>
		public void UpdateDataView(IModelGetter model){
			if (data == null) {
				Debug.LogWarning ("還沒有設定data");
				return;
			}
			var modelItems = data.ToList ();
			for (var i = 0; i < limit; ++i) {
				var curr = i + offset;
				var btn = items [i];

				if (curr >= modelItems.Count) {
					btn.gameObject.SetActive (false);
					continue;
				}
				var modelItem = modelItems [curr];
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
				btn.gameObject.GetComponentInChildren<Text> ().text = msg;
				btn.gameObject.SetActive (true);
			}
		}
		public int Page{
			get{
				return offset / limit;
			}
			set{
				if (value <= 0) {
					offset = 0;
					return;
				}
				offset = value * limit;
			}
		}
		#endregion

		#region model
		/// <summary>
		/// 顯示用的資料，在呼叫UpdateUI前要先設定
		/// </summary>
		IEnumerable<Item> data;
		public IEnumerable<Item> Data{
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
		#endregion

		#region controller
		public IEnumerator HandleCommand(IModelGetter model, string msg, object args, Action<Exception> callback){
			if (msg == commandPrefix + "_pageup") {
				Page -= 1;
				UpdateDataView (model);
			}
			if (msg == commandPrefix + "_pagedown") {
				Page += 1;
				UpdateDataView (model);
			}
			if (msg.Contains (commandPrefix+"_item_")) {
				// 修改狀態文字
				var selectIdx = CurrIndex (msg);
				CurrItemLabel (model, selectIdx);
				// 修改列表內容
				var cfg = ConfigItem.Get (data.ToList() [selectIdx].prototype);
				ShowMode = cfg.Type == ConfigItemType.ID_weapon ? ItemView.Mode.Equip : ItemView.Mode.Normal;
				UpdateDataView (model);
			}
			yield return null;
		}
		#endregion
	}
}

