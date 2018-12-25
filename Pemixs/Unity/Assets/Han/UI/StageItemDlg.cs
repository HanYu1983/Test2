using System;
using UnityEngine;
using UnityEngine.UI;

namespace Remix
{
	public class StageItemDlg : TutorialDlg
	{
		public new void UpdateUI(LanguageText lt, int lang){
			textTitle.text = lt.GetDlgNote (lang, "StageitemDlg");
			textPageNum.text = string.Format ("{0}/{1}", pageIdx + 1, picPaths.Count);
			var prefab = picPaths [pageIdx];
			var obj = Util.Instance.GetPrefab (prefab, null);
			imgPic.sprite = obj.GetComponent<Image> ().sprite;
			Destroy (obj);
		}
	}
}

