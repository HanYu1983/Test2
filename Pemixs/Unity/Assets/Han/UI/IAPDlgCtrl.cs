using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

namespace Remix{
	public class IAPDlgCtrl : MonoBehaviour 
	{
	    const float LAYOUT_HEIGHT = 110.0f;

		public Transform IAPRoot;
		public GameObject IAPLayout;
		public Text titleText;
		public List<IAPLayoutCtrl> iapDataList;

		public List<ItemKey> enableKeys;
		public LanguageText langText;
		public int lang;
		public static IAPDlgCtrl Instance;
			
		public void InitData(LanguageText lt, int lang, List<ItemKey> enableKeys)
	    {
			/*
			if (IAPLayout == null) {
				Debug.LogWarning ("沒有設定IAPLayout");
				return;
			}

			if (IAPRoot == null) {
				IAPRoot = this.transform;
			}

			IAPLayout.SetActive (false);
			*/
			foreach (var obj in iapDataList) {
				GameObject.Destroy (obj.gameObject);
			}
			iapDataList.Clear ();

			this.enableKeys = enableKeys;
			langText = lt;
			this.lang = lang;
			Instance = this;

			/*
			var keys = enableKeys;

			for (int i=0;i<keys.Count;++i)
			{
				var key = keys [i];

				GameObject layoutObj = Instantiate(IAPLayout) as GameObject;
				layoutObj.transform.SetParent(IAPRoot, false);
				layoutObj.SetActive (true);

				IAPLayoutCtrl layoutCtrl = layoutObj.GetComponent<IAPLayoutCtrl>();
				layoutCtrl.textDesc.text = lt.GetIAPDesc (lang, key.Idx);
				layoutCtrl.textCost.text = lt.GetIAPCost (lang, key.Idx);

				ButtonCtrl buyBtn = layoutCtrl.buyButton;
				buyBtn.command = "IAPDlgBtn" + key.StringKey;
				buyBtn.SetEnable(true);

				try{
					Util.Instance.GetPrefab(key.IAPPrefabName, layoutCtrl.anchorObj);
				}catch(Exception e){
					Debug.LogWarning (e.Message);
				}
				layoutObj.SetActive (true);

				iapDataList.Add (layoutCtrl);
			}
			*/
	    }

		public void UpdateUI(LanguageText lt, int lang){
			titleText.text = lt.GetDlgNote (lang, "IAPDlg");
		}
	}
}