using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace Remix{
	public class IAPLayoutCtrl : MonoBehaviour 
	{
		public GameObject anchorObj;
		public ButtonCtrl buyButton;
		public Text textCost;
		public Text textDesc;

		void ScrollCellIndex (int idx){
			var iapDlg = IAPDlgCtrl.Instance;
			var keys = iapDlg.enableKeys;
			if (idx >= keys.Count) {
				this.gameObject.SetActive (false);
				return;
			} else {
				this.gameObject.SetActive (true);
			}
			var key = keys [idx];
			textDesc.text = iapDlg.langText.GetIAPDesc (iapDlg.lang, key.Idx);
			textCost.text = iapDlg.langText.GetIAPCost (iapDlg.lang, key.Idx);

			ButtonCtrl buyBtn = buyButton;
			buyBtn.command = "IAPDlgBtn" + key.StringKey;
			buyBtn.SetEnable(true);

			try{
				Util.Instance.GetPrefab(key.IAPPrefabName, anchorObj);
			}catch(Exception e){
				Debug.LogWarning (e.Message);
			}
		}
	}
}