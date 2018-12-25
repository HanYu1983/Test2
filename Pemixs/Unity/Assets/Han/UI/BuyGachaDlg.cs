using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Remix
{
	public class BuyGachaDlg : MonoBehaviour
	{
		public Text textTitle, textCost, textDesc, textPage;
		public int selectIdx;
		public List<string> lockCaptureKeys;

		public Dictionary<string,object> MetaData{get; set;}

		public void SetLockCaptureKeys(List<string> captureKeys){
			this.lockCaptureKeys = captureKeys;
		}

		public string CurrentLockKey {
			get {
				if (lockCaptureKeys == null) {
					throw new UnityException ("你必須先呼叫SetLockCaptureKeys");
				}
				return lockCaptureKeys [selectIdx];
			}

			set {
				if (lockCaptureKeys == null) {
					throw new UnityException ("你必須先呼叫SetLockCaptureKeys");
				}
				for (var i = 0; i < lockCaptureKeys.Count; ++i) {
					if (lockCaptureKeys [i] == value) {
						selectIdx = i;
						break;
					}
				}
			}
		}

		public void Left(){
			if (lockCaptureKeys == null) {
				throw new UnityException ("你必須先呼叫SetLockCaptureKeys");
			}
			selectIdx -= 1;
			if (selectIdx < 0) {
				selectIdx = lockCaptureKeys.Count - 1;
			}
		}

		public void Right(){
			if (lockCaptureKeys == null) {
				throw new UnityException ("你必須先呼叫SetLockCaptureKeys");
			}
			selectIdx += 1;
			if (selectIdx >= lockCaptureKeys.Count) {
				selectIdx = 0;
			}
		}

		public void UpdateUI(LanguageText lt, int lang){
			var strkey = lockCaptureKeys [selectIdx];
			var captureKey = new CaptureKey (strkey);
			var gcDef = GachaDef.Get (captureKey.GachaConfigID);
			SetCost (gcDef.Unlock);
			SetTitle (lt.GetDlgNote(lang, "BuyGachaDlg"));
			SetDesc (lt.GetDlgMessage(lang, "MesgText_G01"));
			textPage.text = string.Format ("{0}/{1}", selectIdx+1, lockCaptureKeys.Count);
		}

		public void SetCost(int cost){
			textCost.text = cost + "";
		}

		public void SetTitle(string title){
			textTitle.text = title;
		}

		public void SetDesc(string v){
			textDesc.text = v;
		}
	}
}

