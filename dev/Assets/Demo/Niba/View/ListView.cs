using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Common;
using System.Linq;
using System.Collections;

namespace View
{
	public class ListView : MonoBehaviour
	{
		void Awake(){
			PrepareButtonsAndEvents ();
		}

		#region view
		public GameObject itemParent;
		public int offset;
		public int limit;
		public GameObject objDetail;
		public string commandPrefix;
		public Button btnPageUp, btnPageDown;
		public string CommandPrefix{ get { return commandPrefix; } }

		public Button[] items;
		void PrepareButtonsAndEvents(){
			items = itemParent.GetComponentsInChildren<Button> ();
			for(var i=0; i<items.Length; ++i){
				Func<int,UnityEngine.Events.UnityAction> closure = idx=>{
					return ()=>{
						Common.Common.Notify(string.Format("{0}_item_{1}",commandPrefix, idx), null);
					};
				};
				items [i].onClick.AddListener (closure (i));
			}
			if (btnPageUp != null) {
				btnPageUp.onClick.AddListener (() => {
					Common.Common.Notify (commandPrefix + "_pageup", null);
				});
			}

			if (btnPageDown != null) {
				btnPageDown.onClick.AddListener (() => {
					Common.Common.Notify (commandPrefix + "_pagedown", null);
				});
			}
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

		public interface IDataProvider{
			int DataCount{ get; }
			void ShowData(IModelGetter model, GameObject ui, int idx);
			void ShowSelect (IModelGetter model, GameObject ui, int idx);
		}

		public IDataProvider DataProvider{ get; set; }
		/// <summary>
		/// 修改狀態列文字
		/// 指定顯示第currIndex個道具
		/// </summary>
		/// <param name="model">Model.</param>
		/// <param name="currIndex">Curr index.</param>
		public void CurrItemLabel(IModelGetter model, int currIndex){
			if (DataProvider == null) {
				Debug.LogWarning ("你還沒設定DataProvider");
				return;
			}
			DataProvider.ShowSelect (model, objDetail, currIndex);
		}
		/// <summary>
		/// 更新列表
		/// 注意要先設定Data
		/// </summary>
		/// <param name="model">Model.</param>
		public void UpdateDataView(IModelGetter model){
			if (DataProvider == null) {
				Debug.LogWarning ("你還沒設定DataProvider");
				return;
			}
			for (var i = 0; i < limit; ++i) {
				var curr = i + offset;
				var btn = items [i];
				if (curr >= DataProvider.DataCount) {
					btn.gameObject.SetActive (false);
					continue;
				}
				DataProvider.ShowData (model, btn.gameObject, curr);
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
		public int lastSelectIdx = -1;
		public int LastSelectIndex{
			get{
				return lastSelectIdx;
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
				lastSelectIdx = selectIdx;
			}
			yield return null;
		}
		#endregion
	}
}

