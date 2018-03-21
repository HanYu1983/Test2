using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Common;
using System;
using System.Linq;
using HanRPGAPI;

namespace View{
	public class FusionRequireView : MonoBehaviour {
		public GameObject requireItemParent;
		public Button btnAdd1, btnAdd10, btnMax, btnSub1, btnSub10, btnOK;
		public Text txt_fusionTarget;
		public string commandPrefix;

		public Text[] txt_requireItems;

		void Awake(){
			btnAdd1.onClick.AddListener (() => {
				Common.Common.Notify(commandPrefix+"_add1", null);
			});
			btnAdd10.onClick.AddListener (() => {
				Common.Common.Notify(commandPrefix+"_add10", null);
			});
			btnMax.onClick.AddListener (() => {
				Common.Common.Notify(commandPrefix+"_max", null);
			});
			btnSub1.onClick.AddListener (() => {
				Common.Common.Notify(commandPrefix+"_sub1", null);
			});
			btnSub10.onClick.AddListener (() => {
				Common.Common.Notify(commandPrefix+"_sub10", null);
			});
			btnOK.onClick.AddListener (() => {
				Common.Common.Notify(commandPrefix+"_ok", null);
			});

			txt_requireItems = requireItemParent.GetComponentsInChildren<Text> ();
		}

		public string CommandPrefix {
			get{ return commandPrefix; }
		}

		public Place Who{ get; set; }
		public Item FusionTarget{ get; set; }

		public void UpdateUI(IModelGetter model){
			if (FusionTarget.Equals (Item.Empty)) {
				throw new Exception ("必須先設定FusionTarget");
			}
			var targetCfg = ConfigItem.Get (FusionTarget.prototype);
			txt_fusionTarget.text = string.Format ("合成{0}{1}個", targetCfg.Name, FusionTarget.count);

			var requireItems = HanRPGAPI.Alg.ParseItem (targetCfg.FusionRequire).ToList();
			for (var i = 0; i < txt_requireItems.Length; ++i) {
				var txt = txt_requireItems [i];
				if (i >= requireItems.Count) {
					txt.gameObject.SetActive (false);
					continue;
				}
				var requireItem = requireItems [i];
				var total = model.GetMapPlayer(Who).storage.Where(j=>{
					return j.prototype == requireItem.prototype;
				}).Sum(j=>j.count);
				var cfg = ConfigItem.Get (requireItem.prototype);
				txt.text = string.Format ("需要{0}{1}個({2})", cfg.Name, FusionTarget.count* requireItem.count, total);
				txt.gameObject.SetActive (true);
			}

		}

		#region controller
		public IEnumerator HandleCommand(IModelGetter model, string msg, object args, Action<Exception> callback){
			if (msg.Contains (CommandPrefix)) {
				var isNotValidItem = FusionTarget.Equals (Item.Empty);
				if (isNotValidItem) {
					callback (new Exception ("你還沒選擇道具"));
					yield break;
				}
				if (msg == commandPrefix + "_add1") {
					var item = FusionTarget;
					item.count += 1;
					FusionTarget = item;
				}
				if (msg == commandPrefix + "_add10") {
					var item = FusionTarget;
					item.count += 10;
					FusionTarget = item;
				}
				if (msg == commandPrefix + "_sub1") {
					var item = FusionTarget;
					item.count -= 1;
					if (item.count < 0) {
						item.count = 0;
					}
					FusionTarget = item;
				}
				if (msg == commandPrefix + "_sub10") {
					var item = FusionTarget;
					item.count -= 10;
					if (item.count < 0) {
						item.count = 0;
					}
					FusionTarget = item;
				}
				if (msg == commandPrefix + "_max") {
					var maxFusionCount = model.IsCanFusion (FusionTarget.prototype, Who);
					var item = FusionTarget;
					item.count = maxFusionCount;
					if (item.count < 0) {
						item.count = 0;
					}
					FusionTarget = item;
				}
				if (msg == commandPrefix + "_ok") {
					if (FusionTarget.count == 0) {
						yield break;
					}
					var info = new object[]{
						FusionTarget, Who
					};
					Common.Common.Notify("fusionRequireView_ok", info);
				}
				UpdateUI (model);
			}
			yield return null;
		}
		#endregion
	}
}