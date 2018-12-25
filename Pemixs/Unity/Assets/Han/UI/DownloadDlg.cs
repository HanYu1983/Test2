using System;
using UnityEngine;
using UnityEngine.UI;

namespace Remix
{
	public class DownloadDlg : MonoBehaviour
	{
		public TutorialDlg tutorialDlg;
		public Text textLoadNum, textDownload, textTitle;
		public Image imgBar;

		public void SetDownloadPercentage(float v){
			v = Mathf.Max (0, Mathf.Min (1, v));
			textLoadNum.text = string.Format ("{0}", (int)(v * 100));
			var scale = imgBar.rectTransform.localScale;
			scale.x = v;
			imgBar.rectTransform.localScale = scale;
		}

		public void SetDownloadText(string v){
			textDownload.text = v;
		}

		public void SetTitle(string v){
			textTitle.text = v;
		}

		public void Left(){
			tutorialDlg.Left ();
		}

		public void Right(){
			tutorialDlg.Right ();
		}

		public void UpdateUI(LanguageText lt, int lang){
			tutorialDlg.UpdateUI (lt, lang);
			SetTitle (lt.GetDlgNote (lang, "DownloadDlg"));
		}
	}
}

