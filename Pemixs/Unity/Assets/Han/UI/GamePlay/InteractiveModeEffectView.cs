using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Remix{
	// 注意，這個元作在編輯面板打勾只代表開啟測試模式。這個元作不需要打勾
	public class InteractiveModeEffectView : MonoBehaviour {
		public List<GameObject> effectsContainer;
		public GameObject anchor;
		public string comment;

		void Awake(){
			InitEffectMap ();
			HideAllEffect ();
		}
			
		string txt_name = "";

		void OnGUI(){
			GUILayout.BeginArea (new Rect (0, 0, 500, 500));
			txt_name = GUILayout.TextField (txt_name);
			if (GUILayout.Button ("Animate")) {
				StartCoroutine (ShowEffectOnce (txt_name, 3f));
			}
			if (GUILayout.Button ("Show")) {
				SetEffectVisible (txt_name, true);
			}
			if (GUILayout.Button ("Hide")) {
				SetEffectVisible (txt_name, false);
			}
			GUILayout.EndArea ();
		}

		GameObject FindEffect(string name){
			foreach (var go in effectsContainer) {
				var effect = go.transform.Find (name);
				if (effect != null) {
					return effect.gameObject;
				}
			}
			throw new UnityException ("沒有這個特效:"+name);
		}

		public void SetEffectVisible(string name, bool visible){
			var effectname = MapToEffectName (name);
			var effect = FindEffect (effectname);
			effect.SetActive (visible);
			// 特殊處理，為了向下相容
			// EffectStatus的父層是背景框
			var isStatusEffect = effect.transform.parent.gameObject.name == "EffectStatus";
			if (isStatusEffect) {
				effect.transform.parent.gameObject.SetActive (visible);
			}
		}

		public IEnumerator ShowEffectOnce(string name, float duration){
			// 在Scenes/Kas_IM_Temp 有UI並帶動作,其中要注意的是愛心的動作有兩個,_01是正常撫摸用, _02是睡眠撫摸用
			var effectname = MapToEffectName (name);
			var effect = FindEffect (effectname);
			var copy = GameObject.Instantiate(effect) as GameObject;
			var anchor = this.anchor == null ? effect.transform.parent : this.transform;
			copy.transform.SetParent (anchor, false);
			copy.SetActive (true);

			yield return new WaitForSeconds (duration);
			copy.SetActive (false);
			DestroyObject (copy);
		}

		#region effect name mapping
		Dictionary<string, string> effectMap;
		public void InitEffectMap(){
			effectMap = new Dictionary<string, string> () {
				//{"右中的對話框","UI171310"},
				//{"右下的對話框","UI171320"},
				{"小愛心","UI171130"},
				{"中愛心","UI171120"},
				{"大愛心","UI171110"},
				{"...","UI171140"},
				{"爆青筋","UI171150"},
				{"zzz","UI171160"},
				{"貓掌","UI171170"},
				{"升級","UI171180"},
				{"食物套組","UI171410"},
				{"肉","UI171420"},
				{"+5","UI171510"},
				{"+10","UI171520"},
				{"-5s","UI171530"},
				{"-10s","UI171540"},
				{"+50","UI171550"},
				{"+100","UI171560"},
				{"+150","UI171570"},
				{"+200","UI171580"},
				{"+250","UI171590"},
				{"+300","UI171600"},
				{"-50%","UI171610"},
				{"+500","UI171620"},
				{"+1000","UI171630"},
				{"-100%","UI171640"},
			};
		}

		string MapToEffectName(string name){
			if (effectMap.ContainsKey (name) == false) {
				throw new UnityException ("對映表中沒有這個名稱:"+name);
			}
			var effectname = effectMap [name];
			return effectname;
		}

		public void HideAllEffect(){
			foreach (var effectname in effectMap.Values) {
				var go = FindEffect (effectname);
				go.SetActive (false);
			}
		}
		#endregion
	}
}