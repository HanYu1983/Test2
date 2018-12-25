using System;
using UnityEngine;
using UnityEngine.UI;

namespace Remix
{
	public class TrainingUI : MonoBehaviour
	{
		public GameObject focusRoot;
		public Text txtDesc;

		public string Desc {
			set {
				if (txtDesc == null) {
					return;
				}
				txtDesc.text = value;
			}
		}
		public bool Visible{
			set{
				if (focusRoot == null) {
					Debug.LogWarning ("focusRoot is null.");
					return;
				}
				focusRoot.SetActive (value);
			}
		}
		public void HideAllFocus(){
			if (focusRoot == null) {
				return;
			}
			for (var i = 0; i < focusRoot.transform.childCount; ++i) {
				var go = focusRoot.transform.GetChild (i);
				if (go.name.Contains ("TUI") == false) {
					continue;
				}
				go.gameObject.SetActive (false);
			}
		}
		public void SetFocusVisible(string key, bool visible){
			if (focusRoot == null) {
				return;
			}
			var go = focusRoot.transform.Find (key);
			if (go == null) {
				Debug.LogWarning ("can not SetFocusVisible because key is not found:"+key);
				return;
			}
			go.gameObject.SetActive (visible);
		}
		// id = 0~4
		public void AnimateFinger(int id){
			var go = focusRoot.transform.Find ("TUI_Finger");
			var animator = go.GetComponent<Animator>();
			if (animator == null) {
				return;
			}
			animator.SetTrigger (string.Format("A{0:00}", id+1));
		}


		public static void SetVisible(GameObject go, bool value){
			var trainingUI = go.GetComponent<TrainingUI> ();
			if (trainingUI == null) {
				Debug.LogWarning ("trainingUI is null!");
				return;
			}
			trainingUI.Visible = value;
		}
		public static void SetDesc(GameObject go, string value){
			var trainingUI = go.GetComponent<TrainingUI> ();
			if (trainingUI == null) {
				Debug.LogWarning ("trainingUI is null!");
				return;
			}
			trainingUI.Desc = value;
		}
		public static void HideAllFocus(GameObject go){
			var trainingUI = go.GetComponent<TrainingUI> ();
			if (trainingUI == null) {
				Debug.LogWarning ("trainingUI is null!");
				return;
			}
			trainingUI.HideAllFocus ();
		}
		public static void SetFocusVisible(GameObject go, string key, bool visible){
			var trainingUI = go.GetComponent<TrainingUI> ();
			if (trainingUI == null) {
				Debug.LogWarning ("trainingUI is null! key is "+key);
				return;
			}
			trainingUI.SetFocusVisible (key, visible);
		}
		// id = 0~4
		public static void AnimateFinger(GameObject go, int id){
			var trainingUI = go.GetComponent<TrainingUI> ();
			if (trainingUI == null) {
				Debug.LogWarning ("trainingUI is null! key is "+id);
				return;
			}
			trainingUI.AnimateFinger (id);
		}
	}
}

