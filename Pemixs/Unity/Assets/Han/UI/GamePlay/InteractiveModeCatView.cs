using System;
using UnityEngine;
using Spine.Unity;
using System.Collections;

namespace Remix
{
	// 注意，這個元作在編輯面板打勾只代表開啟測試模式。這個元作不需要打勾
	public class InteractiveModeCatView : MonoBehaviour
	{
		public int catId;
		public GameObject iconPivot;
		public InteractiveModeEffectView effectView;
		// 這單純是給編輯器中寫註解用的
		public string comment;

		public const int ANIM_NORMAL = 0;
		public const int ANIM_NORMAL2 = 1;
		public const int ANIM_ANGRY = 2;
		public const int ANIM_NORMAL2_IN = 3;
		public const int ANIM_NORMAL2_OUT = 4;
		public const int ANIM_ANGRY2 = 5;
		public const int ANIM_HAPPY = 6;
		public const int ANIM_PACIFY = 7;
		public const int ANIM_PACIFY2 = 8;
		public const int ANIM_WANNA_PLAY = 9;
		public const int ANIM_TOY = 10;
		public const int ANIM_TOY2 = 11;
		public const int ANIM_SLEEP = 12;
		public const int ANIM_WAKE_UP = 13;
		public const int ANIM_AWAKE = 14;
		public const int ANIM_WAKE_UP_IN = 15;
		public const int ANIM_WAKE_UP_OUT = 16;
		public const int ANIM_HUNGRY = 17;
		public const int ANIM_AFTER_FEED = 18;
		public const int ANIM_FEED = 19;
		public const int ANIM_FEED2 = 20;
		public const int ANIM_FEED3 = 21;
		public const int ANIM_FEED4 = 22;
		public const int ANIM_FEED5 = 23;
		public const int ANIM_FEED6 = 24;
		public const int ANIM_FEED7 = 25;
		public const int ANIM_FEED8 = 26;

		public GameObject cat;
		// 這個變數會影響到動畫呼叫，同置貓時請一並清除這個值
		int lastAnimIdx = -1;
		// 這個變數會影響到動畫呼叫，同置貓時請一並清除這個值
		public string state = "";

		void Start(){
			CreateCat ();
		}

		void OnGUI(){
			GUILayout.BeginArea (new Rect (0, 0, 500, 500));
			if (GUILayout.Button ("start touch")) {
				StartCoroutine(BeginTouch ());
			}
			if (GUILayout.Button ("end touch")) {
				StartCoroutine(EndTouch ());
			}
			if (GUILayout.Button ("touch")) {
				StartCoroutine(Touch(+50));
			}
			if (GUILayout.Button ("heard")) {
				AddValue (5);
			}
			if (GUILayout.Button ("score")) {
				StartCoroutine (Scare ());
			}
			if (GUILayout.Button ("play toy")) {
				StartCoroutine (PlayToy (ANIM_TOY, false, 0, false));
			}
			if (GUILayout.Button ("play toy 2 and comlete")) {
				StartCoroutine (PlayToy (ANIM_TOY2, true, 0, false));
			}
			if (GUILayout.Button ("wanna play")) {
				StartCoroutine (WannaPlay ());
			}
			if (GUILayout.Button ("sleep")) {
				StartCoroutine (Sleep ());
			}
			if (GUILayout.Button ("awake")) {
				StartCoroutine (CatAwake (0, false));
			}
			if (GUILayout.Button ("hungry")) {
				StartCoroutine (Hungry ());
			}
			if (GUILayout.Button ("feed")) {
				StartCoroutine (Feed (ANIM_FEED2, false, 0, false));
			}
			if (GUILayout.Button ("FeedComplete")) {
				StartCoroutine (FeedComplete (0, false));
			}
			if (GUILayout.Button ("feed and complete")) {
				StartCoroutine (Feed (ANIM_FEED2, true, 0, false));
			}
			GUILayout.EndArea ();
		}
		#region cat sound
		SoundDataCtrl catSound;
		public void CreateCatSound(){
			if (catSound != null) {
				GameObject.Destroy (catSound.gameObject);
				catSound = null;
			}
			var soundObj = Util.Instance.GetPrefab(new ItemKey(StoreCtrl.DATA_CAT, catId).CatSoundPrefab, this.gameObject);
			catSound = soundObj.GetComponent<SoundDataCtrl>();
		}
		public void PlayCatSound(int idx){
			if (catSound == null) {
				Debug.LogWarning ("請先呼叫CreateCatSound");
				return;
			}
			catSound.PlayInteractiveModeSound (idx);
		}
		#endregion

		public void CreateCat(){
			Clear ();
			var imageObj = Util.Instance.GetPrefab (new ItemKey(StoreCtrl.DATA_CAT, catId).CatSpinePrefab("I"), null);
			imageObj.transform.SetParent (iconPivot.transform);
			imageObj.transform.localPosition = new Vector3(0f,0f,-1);
			imageObj.transform.localScale = new Vector3 (0.23f, 0.23f, 1.0f);
			imageObj.GetComponent<MeshRenderer> ().sortingOrder = 60;
			cat = imageObj;
		}

		public Spine.TrackEntry Animate(int animIdx, bool loop, bool restart = false){
			var _spineAnim = cat.GetComponentInChildren<SkeletonAnimation>();
			if (restart == false && animIdx == lastAnimIdx) {
				Debug.LogWarning ("你呼叫了同一個動畫二次，請檢查這是不是預期行為");
				return _spineAnim.state.GetCurrent (0);
			}
			Spine.Animation anim = _spineAnim.skeleton.Data.Animations.Items[animIdx];
			var track = _spineAnim.state.SetAnimation(0, anim, loop);
			lastAnimIdx = animIdx;
			return track;
		}

		public void AddValue(int value){
			// PlayCatSound (SoundDataCtrl.TOUCH_NORMAL);
			StartCoroutine (effectView.ShowEffectOnce ("小愛心", 1f));
			StartCoroutine (effectView.ShowEffectOnce ("+"+value, 0.3f));
		}

		public void AddPer(int value){
			StartCoroutine (effectView.ShowEffectOnce ("小愛心", 1f));
			if (value > 0) {
				StartCoroutine (effectView.ShowEffectOnce ("+" + value + "%", 0.3f));
			} else if (value < 0) {
				StartCoroutine (effectView.ShowEffectOnce (value + "%", 0.3f));
			}
		}

		public void ClearEffect(){
			//effectView.SetEffectVisible ("右中的對話框", false);
			effectView.SetEffectVisible ("爆青筋", false);
			effectView.SetEffectVisible ("貓掌", false);
			effectView.SetEffectVisible ("食物套組", false);
			effectView.SetEffectVisible ("zzz", false);
		}

		public void ClearCat(){
			if (cat != null) {
				state = "";
				lastAnimIdx = -1;
				DestroyObject (cat);
				cat = null;
				Util.Instance.RequestUnloadUnusedAssets ();
			}
		}

		public void Clear(){
			ClearEffect ();
			ClearCat ();
		}

		public bool IsCatVisible{
			set {
				if (cat != null) {
					cat.SetActive (value);
				}
			}
			get {
				return cat.gameObject.activeSelf;
			}
		}

		public IEnumerator Scare(){
			PlayCatSound (SoundDataCtrl.SCARE);
			state = "angry";
			yield return new WaitForSpineAnimationComplete (Animate (ANIM_ANGRY, false));
			yield return Angry ();
		}

		public IEnumerator Angry(){
			PlayCatSound (SoundDataCtrl.ANGRY);
			state = "angry";
			ClearEffect ();
			// effectView.SetEffectVisible ("右中的對話框", true);
			effectView.SetEffectVisible ("爆青筋", true);
			Animate (ANIM_ANGRY2, true);
			yield return null;
		}

		public IEnumerator Pacify(int pacifyIdx, bool complete, int nextAnim, bool force, Action after = null){
			if (pacifyIdx != ANIM_PACIFY && pacifyIdx != ANIM_PACIFY2){
				throw new UnityException ("沒有這個玩具:"+pacifyIdx);
			}
			if (force == false && (state != "angry")) {
				throw new UnityException ("不是生氣狀態不能安撫");
			}
			PlayCatSound (SoundDataCtrl.PACIFY);
			ClearEffect ();
			yield return new WaitForSpineAnimationComplete (Animate (pacifyIdx, false));
			if (complete) {
				PlayCatSound (SoundDataCtrl.FULLUP);
				StartCoroutine (effectView.ShowEffectOnce ("大愛心", 1f));
				yield return new WaitForSpineAnimationComplete (Animate (ANIM_HAPPY, false));
				Animate (nextAnim, true);
			} else {
				yield return Angry ();
			}
			if (after != null) {
				after ();
			}
		}

		public IEnumerator PlayToy(int toyIdx, bool complete, int nextAnim, bool force){
			if (toyIdx != ANIM_TOY && toyIdx != ANIM_TOY2){
				throw new UnityException ("沒有這個玩具");
			}
			if (force == false && (state != "wanna play")) {
				throw new UnityException ("不是想玩狀態不能給玩");
			}
			PlayCatSound (SoundDataCtrl.PLAY);
			ClearEffect ();
			yield return new WaitForSpineAnimationComplete (Animate (toyIdx, false));
			if (complete) {
				PlayCatSound (SoundDataCtrl.FULLUP);
				StartCoroutine (effectView.ShowEffectOnce ("大愛心", 1f));
				yield return new WaitForSpineAnimationComplete (Animate (ANIM_HAPPY, false));
				Animate (nextAnim, true);
			} else {
				yield return WannaPlay ();
			}
		}

		public IEnumerator WannaPlay(){
			PlayCatSound (SoundDataCtrl.WANNA_PLAY);
			state = "wanna play";
			ClearEffect ();
			Animate (ANIM_WANNA_PLAY, true);
			effectView.SetEffectVisible ("貓掌", true);
			yield return null;
		}

		public IEnumerator Sleep(){
			PlayCatSound (SoundDataCtrl.SLEEP);
			state = "sleep";
			ClearEffect ();
			Animate (ANIM_SLEEP, true);
			//effectView.SetEffectVisible ("右中的對話框", true);
			effectView.SetEffectVisible ("zzz", true);
			yield return null;
		}

		public IEnumerator CatAwake(int nextAnimIdx, bool force){
			if (force == false && state != "sleep") {
				throw new UnityException ("不在睡眠狀態不能起來");
			}
			PlayCatSound (SoundDataCtrl.WAKEUP);
			ClearEffect ();
			state = "";
			yield return new WaitForSpineAnimationComplete(Animate(ANIM_AWAKE, false));
			Animate (nextAnimIdx, true);
			yield return null;
		}

		public void Normal(){
			PlayCatSound (SoundDataCtrl.NORMAL);
			ClearEffect ();
			Animate(0, true, false);
		}

		public IEnumerator Hungry(){
			PlayCatSound (SoundDataCtrl.HUNGRY);
			ClearEffect ();
			state = "hungry";
			Animate(ANIM_HUNGRY, true);
			//effectView.SetEffectVisible ("右中的對話框", true);
			effectView.SetEffectVisible ("食物套組", true);
			yield return null;
		}

		public IEnumerator Feed(int foodIdx, bool complete, int nextAnimIdx, bool force){
			if (foodIdx != ANIM_FEED && 
				foodIdx != ANIM_FEED2 &&
				foodIdx != ANIM_FEED3 &&
				foodIdx != ANIM_FEED4 &&
				foodIdx != ANIM_FEED5 &&
				foodIdx != ANIM_FEED6 &&
				foodIdx != ANIM_FEED7 &&
				foodIdx != ANIM_FEED8 )
			{
				throw new UnityException ("沒有這個食物動畫:"+foodIdx);
			}
			if (force == false && state != "hungry") {
				throw new UnityException ("不在飢餓狀態不能餵食:"+state);
			}
			PlayCatSound (SoundDataCtrl.EAT);
			ClearEffect ();
			yield return new WaitForSpineAnimationComplete(Animate(foodIdx, false));
			if (complete == false) {
				yield return Hungry ();
			} else {
				yield return FeedComplete (nextAnimIdx, force);
			}
		}

		public IEnumerator FeedComplete(int nextAnimIdx, bool force){
			if (force == false && state != "hungry") {
				throw new UnityException ("不在飢餓狀態不能飽食");
			}
			PlayCatSound (SoundDataCtrl.FULLUP);
			ClearEffect ();
			StartCoroutine (effectView.ShowEffectOnce ("大愛心", 1f));
			yield return new WaitForSpineAnimationComplete(Animate(ANIM_AFTER_FEED, false));
			Animate (nextAnimIdx, true);
		}

		public void ForceState(string state){
			this.state = state;
		}

		public void CreateLevelUpEffect(){
			PlayCatSound (SoundDataCtrl.LEVELUP);
			StartCoroutine (effectView.ShowEffectOnce ("升級", 1f));
		}

		public IEnumerator LevelUp(int nextAnimIdx){
			PlayCatSound (SoundDataCtrl.LEVELUP);
			ClearEffect ();
			StartCoroutine (effectView.ShowEffectOnce ("升級", 1f));
			yield return new WaitForSpineAnimationComplete(Animate(ANIM_HAPPY, false));
			Animate (nextAnimIdx, true);
		}

		public IEnumerator TouchAndAwake(int nextAnimIdx, int value, bool force){
			if (force == false && state != "sleep") {
				throw new UnityException ("只有睡眠狀態才能呼叫TouchAndAwake");
			}
			yield return Touch (value);
			yield return CatAwake (nextAnimIdx, false);
		}

		public IEnumerator Touch(int value){
			ClearEffect ();
			switch (state) {
			case "sleep":
				{
					AddValue (value);
					yield return new WaitForSpineAnimationComplete(Animate(ANIM_WAKE_UP_IN, false));
					yield return new WaitForSpineAnimationComplete(Animate(ANIM_WAKE_UP_OUT, false));
					yield return Sleep ();
				}
				break;
			default:
				{
					AddValue (value);
					yield return new WaitForSpineAnimationComplete(Animate(ANIM_NORMAL2_IN, false));
					yield return new WaitForSpineAnimationComplete(Animate(ANIM_NORMAL2_OUT, false));
					Animate (ANIM_NORMAL, true);
				}
				break;
			}
		}

		public IEnumerator BeginTouch(){
			ClearEffect ();
			switch (state) {
			case "sleep":
				{
					PlayCatSound (SoundDataCtrl.TOUCH_SLEEP);
					Animate (ANIM_WAKE_UP, true);
				}
				break;
			default:
				{
					PlayCatSound (SoundDataCtrl.TOUCH_NORMAL);
					Animate (ANIM_NORMAL2, true);
				}
				break;
			}
			yield return null;
		}

		public IEnumerator EndTouch(){
			ClearEffect ();
			switch (state) {
			case "sleep":
				{
					Animate (ANIM_SLEEP, true);
					effectView.SetEffectVisible ("zzz", true);
				}
				break;
			default:
				{
					Normal ();
					// Animate (ANIM_NORMAL, true);
				}
				break;
			}
			yield return null;
		}
	}
}

