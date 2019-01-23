using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Spine.Unity;
using System.Collections.Generic;
using System;

namespace Remix{
	public class SelectCatDlgCtrl : MonoBehaviour
	{
		public Text levelText, hpText, expText, nameText, skillDesText, pageNumText;
		public GameObject countDownObject;
		public Image hpBarImage, expBarImage;
		public SpriteRenderer touchImage;
		public Vector2 touchRectMin, touchRectMax;
		public InteractiveModeCatView catView;
		public int currentCatIndex = -1;
		LanguageText lt;
		int lang;
		List<Cat> datas;

		public InteractiveModeCatView CatView{ get { return catView; } }
		public int CurrentCatIndexInOrder{ get { return currentCatIndex; } }

		void Awake(){
			IsHandVisible = false;
		}

		public bool IsHandVisible{
			get{
				return touchImage.gameObject.activeSelf;
			}
			set{
				touchImage.gameObject.SetActive(value);
			}
		}

		public void SetHandPosition(Vector3 worldPos){
			var pos = touchImage.transform.position;
			pos.x = worldPos.x;
			pos.y = worldPos.y;
			// 先投射到區域座標
			touchImage.transform.position = pos;
			// 修正區域座標
			var localPos = touchImage.transform.localPosition;
			var xmin = touchRectMin.x;
			var xmax = touchRectMax.x;
			var ymin = touchRectMin.y;
			var ymax = touchRectMax.y;
			if (localPos.x < xmin) {
				localPos.x = xmin;
			}
			if (localPos.x > xmax) {
				localPos.x = xmax;
			}
			if (localPos.y < ymin) {
				localPos.y = ymin;
			}
			if (localPos.y > ymax) {
				localPos.y = ymax;
			}
			// 再投射到世界座標
			touchImage.transform.localPosition = localPos;
		}

		public void SetLanguage(LanguageText lt, int lang){
			this.lt = lt;
			this.lang = lang;
		}

		public void SetOwnCats(List<Cat> datas){
			this.datas = datas;
		}

		public void ClearCat(){
			catView.Clear ();
		}

		public void SetCatIndexInOrder(int catIdx){
			currentCatIndex = catIdx;
			var data = datas [currentCatIndex];
			catView.catId = new ItemKey(data.Key).Idx;
			catView.CreateCat();
			catView.CreateCatSound ();

			UpdateUI ();
			ApplyCatAnimation ();

			// 改良UX。解決剛建出來的貓的正常動畫突然切換的問題
			StartCoroutine (HideOnCreate ());
		}

		IEnumerator HideOnCreate(){
			catView.IsCatVisible = false;
			yield return new WaitForEndOfFrame ();
			catView.IsCatVisible = true;
		}

		public void UpdateUI(){
			var data = datas [currentCatIndex];
			var catKey = new ItemKey (data.Key);
			hpText.text = data.Hp+"";
			expText.text = data.Exp+"";
			if (lt == null) {
				Debug.LogWarning ("沒有呼叫SetLanguage，忽略國際化文字的設定");
			} else {
				nameText.text = lt.GetCatName(lang, catKey);
				skillDesText.text = lt.GetCatSkillNote(lang, catKey);
			}
			levelText.text = string.Format ("LV-{0}", ((data.Lv+1)+"").PadLeft(2,'0'));
			pageNumText.text = string.Format ("{0}/{1}", (currentCatIndex+1), datas.Count);
			// HP Bar
			var scale = hpBarImage.transform.localScale;
			scale.x = data.Hp / (float)data.MaxHp;
			hpBarImage.transform.localScale = scale;
			// Exp Bar
			int catLv = data.Lv;
			if (GameConfig.IsMaxLv (catLv)) {
				var scale2 = expBarImage.transform.localScale;
				scale2.x = 1;
				expBarImage.transform.localScale = scale2;
			} else {
				var maxExp = 0;
				var ignoreHp = 0;
				var ignoreMaxLv = 0;
				var lastMaxExp = 0;
				GameRecord.GetCatLvInfo (catKey, catLv, ref maxExp, ref ignoreHp, ref ignoreMaxLv);
				GameRecord.GetCatLvInfo (catKey, catLv - 1, ref lastMaxExp, ref ignoreHp, ref ignoreMaxLv);

				float expScale = (data.Exp - lastMaxExp) * 1.0f / (maxExp - lastMaxExp);
				var scale2 = expBarImage.transform.localScale;
				scale2.x = expScale;
				expBarImage.transform.localScale = scale2;
			}

			UpdateCountDown ();
		}

		public ItemKey PeekLeftCatKey(){
			var nextIndex = currentCatIndex - 1;
			if (nextIndex < 0) {
				nextIndex = datas.Count - 1;
			}
			return new ItemKey (datas [nextIndex].Key);
		}

		public ItemKey PeekRightCatKey(){
			var nextIndex = currentCatIndex + 1;
			if (nextIndex >= datas.Count) {
				nextIndex = 0;
			}
			return new ItemKey (datas [nextIndex].Key);
		}

		public void Left(){
			var nextIndex = currentCatIndex - 1;
			if (nextIndex < 0) {
				nextIndex = datas.Count - 1;
			}
			// 同一隻貓不必換頁
			/*if (nextIndex == currentCatIndex) {
				return;
			}*/
			SetCatIndexInOrder (nextIndex);
		}

		public void Right(){
			var nextIndex = currentCatIndex + 1;
			if (nextIndex >= datas.Count) {
				nextIndex = 0;
			}
			// 同一隻貓不必換頁
			/*if (nextIndex == currentCatIndex) {
				return;
			}*/
			SetCatIndexInOrder (nextIndex);
		}

		public void UpdateCountDown(){
			if (datas == null) {
				return;
			}
			var countDownText = countDownObject.GetComponentInChildren<Text> ();
			var catData = datas [currentCatIndex];
			long offsetTime = catData.TimeOfEnd - System.DateTime.Now.Ticks;
			if (offsetTime <= 0) {
				countDownObject.SetActive (false);
				return;
			}
			// 防呆處理
			switch (catData.Status) {
			// 只有生氣和睡覺要顯示倒數
			case GameConfig.CAT_STATE_ID.STATE_ANGRY:
			case GameConfig.CAT_STATE_ID.STATE_SLEEP:
				break;
			default:
				countDownObject.SetActive (false);
				#if UNITY_EDITOR
				Debug.LogError("奇怪的狀態中顯示倒數:"+catData.Status);
				#endif
				return;
			}
			System.DateTime offsetDateTime = new System.DateTime(offsetTime);
			countDownText.text = offsetDateTime.Hour + ":" + offsetDateTime.Minute + ":" + offsetDateTime.Second;
			countDownObject.SetActive(true);
		}

		public void ApplyCatAnimation(){
			var data = datas [currentCatIndex];
			var stateId = (GameConfig.CAT_STATE_ID)data.Status;
			switch (stateId) {
			case GameConfig.CAT_STATE_ID.STATE_ANGRY:
				{
					StartCoroutine (catView.Angry ());
				}
				break;
			case GameConfig.CAT_STATE_ID.STATE_HUNGRY:
				{
					StartCoroutine (catView.Hungry ());
				}
				break;
			case GameConfig.CAT_STATE_ID.STATE_SLEEP:
				{
					StartCoroutine (catView.Sleep ());
				}
				break;
			case GameConfig.CAT_STATE_ID.STATE_WANNA_PLAY:
				{
					StartCoroutine (catView.WannaPlay());
				}
				break;
			default:
				{
					catView.Normal ();
				}
				break;
			}
		}

		public void HideCatEffectView(){
			catView.Clear ();
		}

		public void ShowCat(){
			catView.IsCatVisible = true;
		}

		public void HideCat(){
			catView.IsCatVisible = false;
		}

		public IEnumerator InvokeAnimationResult(AnimationResult result, bool applyStateAfterPlay = false, Action after = null ){
			if (result.isLevelUp) {
				catView.CreateLevelUpEffect ();
			}
			if (result.addHp > 0) {
				catView.AddValue (result.addHp);
			}
			if (result.addExp > 0) {
				catView.AddValue (result.addExp);
			}
			/*if (result.reduceTime > 0) {
				catView.AddPer (-50);
			}*/
			if (result.isAngry) {
				yield return catView.Scare();
			}
			if (result.isAwake) {
				yield return catView.CatAwake(0, true);
			}
			if (result.feedItemIdx >= 0) {
				yield return catView.Feed (Remix.InteractiveModeCatView.ANIM_FEED + result.feedItemIdx, result.isFullHp, 0, true);
			}
			if (result.wannaPlayItemIdx >= 0) {
				yield return catView.PlayToy (Remix.InteractiveModeCatView.ANIM_TOY + result.wannaPlayItemIdx, true, 0, true);
			}
			if(applyStateAfterPlay){
				// 更新貓狀態動畫
				ApplyCatAnimation ();
			}
			if (after != null) {
				after ();
			}
		}
	}
}
