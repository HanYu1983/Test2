using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UniRx;
using System.Collections;

namespace Remix
{
	public class TransferDlg : MonoBehaviour
	{
		public List<Text> texts;
		public GameObject objOK;
		public Text textNowId, textId, textName, textTitle;
		public Subject<string> OnIDValueChangeEvent, OnNameValueChangeEvent;

		void Awake(){
			OnIDValueChangeEvent = new Subject<string> ();
			OnNameValueChangeEvent = new Subject<string> ();
		}

		public string TransferCode {
			get{
				return textId.text;
			}
		}

		public string TransferName {
			get{
				return textName.text;
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
			textTitle.text = lt.GetDlgNote (lang, "TransferDlg");
		}

		IEnumerator PerformOnNextFrame(Action fn){
			yield return null;
			fn ();
		}

		public void OnIDValueChange(){
			StartCoroutine (PerformOnNextFrame (() => {
				// 直接呼叫沒辨法取到現值，所以延後呼叫
				OnIDValueChangeEvent.OnNext (textId.text);
			}));
		}

		public void OnNameValueChange(){
			StartCoroutine (PerformOnNextFrame (() => {
				// 直接呼叫沒辨法取到現值，所以延後呼叫
				OnNameValueChangeEvent.OnNext (textName.text);
			}));
		}

		public string NowId{
			set {
				textNowId.text = value;
			}
		}
	}
}

