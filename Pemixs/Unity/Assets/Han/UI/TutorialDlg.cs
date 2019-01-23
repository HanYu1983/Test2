using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Remix
{
	public class TutorialDlg : MonoBehaviour
	{
		public List<string> picPaths;
		public Text textTitle;
		public Image imgPic;
		public Text textPageNum;
		public Text[] textInPics;
		public int pageIdx;

		public void Left(){
			pageIdx -= 1;
			if (pageIdx < 0) {
				pageIdx = picPaths.Count - 1;
			}
		}

		public void Right(){
			pageIdx += 1;
			if (pageIdx >= picPaths.Count) {
				pageIdx = 0;
			}
		}

		public void UpdateUI(LanguageText lt, int lang){
			textTitle.text = lt.GetMainuiNote (lang, "MUI5B01");
			textPageNum.text = string.Format ("{0}/{1}", pageIdx + 1, picPaths.Count);
			var prefab = picPaths [pageIdx];
			var obj = Util.Instance.GetPrefab (prefab, null);
			imgPic.sprite = obj.GetComponent<Image> ().sprite;
			Destroy (obj);
			for (var i = 0; i < textInPics.Length; ++i) {
				var key = string.Format ("TP{0:00}T{1:00}", pageIdx+1, i+1);
				textInPics [i].text = lt.GetTutorialNoteDesc (lang, key);
			}
		}
	}
}

