using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UniRx;

namespace Remix
{
	public class InviteDlg : MonoBehaviour
	{
		public List<Text> texts;
		public GameObject objOK;
		public Text textNowInviteCode, textInviteCode, textName, textInviteNum, textTitle;
		public Subject<string> OnInviteCodeValueChangeEvent, OnNameValueChangeEvent;

		void Awake(){
			OnInviteCodeValueChangeEvent = new Subject<string> ();
			OnNameValueChangeEvent = new Subject<string> ();
		}

		public int InviteCount {
			set {
				textInviteNum.text = string.Format ("{0}/5", value);
			}
		}

		public string Name {
			set{
				textName.text = value;
			}
		}

		public string NowInviteCode {
			set{
				textNowInviteCode.text = value;
			}
		}

		public string InviteCode {
			get {
				return textInviteCode.text;
			}
		}

		public bool IsOkVisible{
			get{
				return objOK.activeSelf;
			}
			set{
				objOK.SetActive (value);
			}
		}

		public void SetText(LanguageText lt, int lang){
			foreach (var t in texts) {
				var key = t.name;
				t.text = lt.GetDlgMessage (lang, key);
			}
			textTitle.text = lt.GetDlgNote (lang, "InviteDlg");
		}

		IEnumerator PerformOnNextFrame(Action fn){
			yield return null;
			fn ();
		}

		public void OnInviteCodeValueChange(){
			StartCoroutine (PerformOnNextFrame (() => {
				// 直接呼叫沒辨法取到現值，所以延後呼叫
				OnInviteCodeValueChangeEvent.OnNext (textInviteCode.text);
			}));
		}

		public void OnNameValueChange(){
			StartCoroutine (PerformOnNextFrame (() => {
				// 直接呼叫沒辨法取到現值，所以延後呼叫
				OnNameValueChangeEvent.OnNext (textName.text);
			}));
		}
	}
}

