using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Remix
{
	public class GamePlayTutorialModeControl : MonoBehaviour, IGamePlayModeControl
	{
		public float syncTimer;
		public float sinceTime;
		public GamePlayView view;
		public GamePlayModel model;
		public GamePlayModelControlHelper helper;
		public LanguageText langText;
		public UserSettings userSettings;

		public bool isGameEnd;
		public bool IsGameEnd{ get{ return isGameEnd; } }

		public int currentLevel = 1;
		public int CurrentLevel{ get { return currentLevel; } }

		public bool IsFeverMode{ get { return false; } }

		public GamePlayView GamePlayView{ get{ return view; }}
		public GamePlayModel GamePlayModel{ get{ return model; } }

		public void InitComponent(){
			view = GetComponent<GamePlayView> ();
			model = GetComponent<GamePlayModel> ();
			helper = GetComponent<GamePlayModelControlHelper> ();
			langText = Util.Instance.langText;
			userSettings = Util.Instance.userSettings;
		}

		public void InitMode(){
			model.ClearForNewGame ();
			view.UpdateScore (model.Score);
			view.StageView.ResetStage ();
			view.StageView.ResetCat ();
			// 將鏡頭移到左貓
			view.StageView.StepMoveStage ();
			view.StageView.StepMoveStage ();
			view.StageView.RightCat.SetActive (false);

			InitTutorialLogic ();
		}

		// 單獨用來判定音樂Loop
		public float lastAudioTime;

		public void Step(float audioTime, float audioOffset){
			if (IsGameEnd) {
				return;
			}
			// === 單獨用來判定音樂Loop ===
			if (audioTime < lastAudioTime) {
				SetNextSinceTime (0);
				sinceTime = 0;
				ResetBad ();
			}
			lastAudioTime = audioTime;
			// === 單獨用來判定音樂Loop ===

			var syncTime = audioTime + audioOffset;
			StepTutorialLogic (syncTime);
			// 同步打擊點
			view.SyncTimer (syncTime - sinceTime + OffsetTime);
			// 同步貓的位置
			view.UpdateCatAnimation ();
			this.syncTimer = syncTime;
			// 長按
			model.StepLongClickVer2 (syncTime - sinceTime + OffsetTime);
			// 產生節拍事件OnBeat
			helper.AppendTimeForComputeBeat (0, syncTime - sinceTime + OffsetTime, OnBeat);
			// 產生點擊節拍事件OnTouchBeat
			helper.AppendTimeForComputeBeat (1, syncTime - sinceTime + OffsetTime + RhythmCtrl.HALF_BEAT_TIME/2, OnTouchBeat);
			helper.AppendTimeForComputeBeat (2, audioTime + audioOffset, OnShiningBeat);
			// 自動打擊
			if (helper.IsAutoPlay) {
				helper.AutoClickHintVer2 (syncTime - sinceTime + OffsetTime, OnGameButtonClick);
			}
			if (helper.IsUseAutoSoundEffect) {
				helper.OnAutoSoundEffectStep (syncTime - sinceTime + OffsetTime, OnAutoSoundEffect);
			}
		}

		void OnAutoSoundEffect(int beat){
			var timer = this.syncTimer - sinceTime + OffsetTime;
			helper.OnAutoSoundEffect (timer);
		}

		void OnShiningBeat(int beat){
			if (beat % 2 == 0) {
				StartCoroutine (view.ShiningHint ());
			}
		}

		void OnClickSuccess(int clickIdx, Game.ClickType clickType, int turn, float count, float factor){
			var clickResult = "";
			switch (clickType) {
			case Game.ClickType.Single:
				// 單擊需要計算
				clickResult = Game.ComputeDistToResult (factor);
				break;
			case Game.ClickType.Long:
				// 連擊就一律Perfect
				clickResult = "Perfect";
				break;
			}
			var isPerfect = clickResult == "Perfect";
			var fixCount = (int)count;
			// 打擊點互動
			var hintIdx = Game.TurnCount2Idx (16, turn, fixCount);
			view.HintPlayGood (hintIdx, clickIdx, isPerfect, Game.IsFeverModeSection(currentLevel), clickType);
			// 記錄這個打擊點已處理過，這樣就不會發生同一個打擊點打了1次以上
			model.CompleteProcess (turn, fixCount, GamePlayModel.ProcessClickOK);

			var animIdx = StageView.P1_BTN_01_IDX + (clickIdx - 1);
			view.PlayCatAnim (GamePlayView.LeftPos, animIdx, StageView.P1_DEFAULT_IDX, false, OnPlayCatAnimAfter);
		}

		void OnClickFail(int clickIdx, int turn, float count){
			if (count < 0 || turn >= RhythmCtrl.MAX_TURN) {
				return;
			}
			var result = Game.HandleOnClickFail (this, clickIdx, turn, count, null);
			OnClickHintResult (result);
			if (helper.IsUseAutoSoundEffect == false) {
				helper.PlayCatSound (clickIdx);
			}
			var animIdx = StageView.P1_BTN_01_IDX + (clickIdx - 1);
			view.PlayCatAnim (GamePlayView.LeftPos, animIdx, StageView.P1_DEFAULT_IDX, false, OnPlayCatAnimAfter);
		}

		void OnClickSuccessRepeating(int clickIdx, Game.ClickType clickType, int turn, float count, float factor){

		}

		void OnClickHintResult(string result){
			if (result == "Bad") {
				Bad ();
			}
		}

		void OnBeat(int beat){
			OnTutorialLogicBeat (beat);
		}

		void OnTouchBeat(int beat){
			var timer = this.syncTimer - sinceTime + OffsetTime;
			var turn = 0;
			var countf = 0f;
			BeatTool.Time2TurnAndCount (16, RhythmCtrl.HALF_BEAT_TIME, timer + RhythmCtrl.HALF_BEAT_TIME / 2, ref turn, ref countf);
			if (turn <0 || turn >= RhythmCtrl.MAX_TURN) {
				return;
			}
			Game.ComputeTouchBeatEndVer2 (timer, OnTouchBeatEnd);
		}

		void OnTouchBeatEnd(int turn, int count){
			var result = Game.HandleOnTouchBeatEnd (view, model, currentLevel, turn, count, null);
			if(result != null){
				OnClickHintResult (result);
			}
		}


		void OnLevelClear(bool isWin, int currentLevel){
			isGameEnd = true;
			//UIEventFacade.OnGameEvent.OnNext ("GameTrainingGameEnd");
			StartCoroutine(StopTutorialAndBackToMap());
		}

		IEnumerator StopTutorialAndBackToMap(){
			// 同時播動畫和音樂
			yield return view.InvokeLevelClear (0, true, false, false);
			// 等待成功音樂播的差不多時
			yield return new WaitForSeconds (8);
			UIEventFacade.OnGameEvent.OnNext ("GameTrainingGameEnd");
		}

		public void OnGameButtonClick(string command){
			if (model == null) {
				throw new UnityException ("處理按鈕事件前請先呼叫InitMode!");
			}
			Game.HandleOnGameButtonClick (view, model, syncTimer - sinceTime + OffsetTime, currentLevel, command, OnClickSuccess, OnClickFail, OnClickSuccessRepeating);
		}
		public void OnGameButtonSlide(string command){

		}

		void OnPlayCatAnimAfter(int pos, int animIdx, bool triggerWithSuccess){
			if (helper.IsUseAutoSoundEffect == true) {
				return;
			}

			var timer = this.syncTimer - sinceTime;
			if (pos == GamePlayView.LeftPos) {
				var turn = 0;
				var countf = 0f;
				// 要多加HALF_BEAT_TIME/2的原因是節奏點的計算要提前節奏點間隔的一半
				BeatTool.Time2TurnAndCount (16, RhythmCtrl.HALF_BEAT_TIME, timer + RhythmCtrl.HALF_BEAT_TIME/2, ref turn, ref countf);
				var count = (int)countf;
				if (turn >= 0 && turn < RhythmCtrl.MAX_TURN) {
					try{
						var playIdx = 0;
						var clickType = Game.ClickType.Pending;
						Game.GetRhythmIndex(RhythmCtrl.RHYTHM_IDX_ARRAY, RhythmCtrl.RHYTHM_BTNMASH_ARRAY, turn, count, ref playIdx, ref clickType);
						if (triggerWithSuccess) {
							// 若打擊成功，記住它並使用它
							helper.AppendRhythmType(turn, count);
						}
						helper.PlayCatSound(playIdx);
					} catch (IndexOutOfRangeException e){
						Debug.LogError ("回合拍數不合法，請檢查"+turn+"/"+count);
						throw e;
					}
				}
			}
		}

		float OffsetTime{
			get{
				return -RhythmCtrl.HALF_BEAT_TIME * 4 * 8;
			}
		}

		float ComputeSinceTime(string type, float range, float now){
			if (type == "before") {
				var tmp = 0f;
				while (true) {
					if (tmp + range > now) {
						break;
					} else {
						tmp += range;
					}
				}
				return tmp;
			}
			if (type == "after") {
				var tmp = 0f;
				while (tmp <= now) {
					tmp += range;
				}
				return tmp;
			}
			throw new Exception ("XXXX");
		}
		#region state
		public int currentLevelStep;
		void ResetStep(){
			currentLevelStep = 0;
		}
		void NextStep(){
			++currentLevelStep;
		}
		#endregion

		#region game play desc
		void InitDesc(){
			view.TextExplan = langText.GetTrainingDesc (userSettings.Language, 0);
		}
		void UpdateDesc(){
			var explan = "";
			switch (currentLevel) {
			case 1:
				{
					explan = langText.GetTrainingDesc (userSettings.Language, 1 + currentLevelStep);
				}
				break;
			case 2:
				{
					explan = langText.GetTrainingDesc (userSettings.Language, 3 + currentLevelStep);
				}
				break;
			case 3:
				{
					explan = langText.GetTrainingDesc (userSettings.Language, 5 + currentLevelStep);
				}
				break;
			case 4:
				{
					explan = langText.GetTrainingDesc (userSettings.Language, 7 + currentLevelStep);
				}
				break;
			case 5:
				{
					explan = langText.GetTrainingDesc (userSettings.Language, 9 + currentLevelStep);
				}
				break;
			case 6:
				{
					explan = langText.GetTrainingDesc (userSettings.Language, 11 + currentLevelStep);
				}
				break;
			case 7:
				{
					explan = langText.GetTrainingDesc (userSettings.Language, 13);
				}
				break;
			}
			view.TextExplan = explan;
		}
		#endregion

		#region bad process
		public int badCount;

		void Bad(){
			++badCount;
		}
		void ResetBad(){
			badCount = 0;
		}
		bool IsPassTutorial{
			get{
				return badCount == 0;
			}
		}
		#endregion

		#region nextSinceTime
		public float nextSinceTime;

		void InitNextSinceTime(){
			var range = RhythmCtrl.HALF_BEAT_TIME * 4 * 8;
			nextSinceTime = ComputeSinceTime ("after", range, 0);
		}
		void SetNextSinceTime(float now){
			var range = RhythmCtrl.HALF_BEAT_TIME * 4 * 8;
			nextSinceTime = ComputeSinceTime ("after", range, now);
		}
		float NextSinceTime{
			get{
				return nextSinceTime;
			}
		}
		#endregion

		#region focus obj
		void HideFocus(){
			TrainingUI.HideAllFocus (view.gameObject);
		}

		void UpdateFocus(int currentLevel){
			TrainingUI.HideAllFocus (view.gameObject);
			switch (currentLevel) {
			case 1:
				{
					switch (currentLevelStep) {
					case 1:
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Ring_R", true);
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Arrow_R", true);
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Ring_B01", true);
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Finger", true);
						TrainingUI.AnimateFinger (view.gameObject, 0);
						break;
					}
				}
				break;
			case 2:
				{
					switch (currentLevelStep) {
					case 1:
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Ring_R", true);
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Arrow_R", true);
						break;
					}
				}
				break;
			case 3:
				{
					switch (currentLevelStep) {
					case 1:
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Ring_R", true);
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Arrow_R", true);
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Ring_B01", true);
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Finger", true);
						TrainingUI.AnimateFinger (view.gameObject, 1);
						break;
					}
				}
				break;
			case 4:
				{
					switch (currentLevelStep) {
					case 1:
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Ring_R", true);
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Arrow_R", true);
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Ring_B02", true);
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Finger", true);
						TrainingUI.AnimateFinger (view.gameObject, 2);
						break;
					}
				}
				break;
			case 5:
				{
					switch (currentLevelStep) {
					case 1:
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Ring_R", true);
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Arrow_R", true);
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Ring_B02", true);
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Finger", true);
						TrainingUI.AnimateFinger (view.gameObject, 3);
						break;
					}
				}
				break;
			case 6:
				{
					switch (currentLevelStep) {
					case 1:
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Ring_R", true);
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Arrow_R", true);
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Ring_B03", true);
						TrainingUI.SetFocusVisible (view.gameObject, "TUI_Finger", true);
						TrainingUI.AnimateFinger (view.gameObject, 4);
						break;
					}
				}
				break;
			}
		}
		#endregion


		#region logic
		void InitTutorialLogic(){
			ResetStep ();
			ResetBad ();
			InitNextSinceTime ();
			HideFocus ();
			InitDesc ();
		}
		void ResetTutorialLogic(){
			ResetStep ();
			ResetBad ();
		}
		void StepTutorialLogic(float syncTime){
			if (syncTime + OffsetTime > NextSinceTime) {
				//Debug.LogError ("Here!!!:"+currentLevel);
				sinceTime = NextSinceTime;
				SetNextSinceTime (NextSinceTime);
				if (IsPassTutorial) {
					ResetTutorialLogic ();
					currentLevel = Game.HandleLevelEnd (view, model, currentLevel, OnLevelClear, null);
				} else {
					ResetTutorialLogic ();	
					Game.LoadLevel (view, model, currentLevel);
				}
				UpdateFocus (currentLevel);
			}
		}

		void OnTutorialLogicBeat(int beat){
			var timer = this.syncTimer - sinceTime + OffsetTime;
			var turn = 0;
			var countf = 0f;
			BeatTool.Time2TurnAndCount (16, RhythmCtrl.HALF_BEAT_TIME, timer + RhythmCtrl.HALF_BEAT_TIME / 2, ref turn, ref countf);
			if (turn <0 || turn >= RhythmCtrl.MAX_TURN) {
				return;
			}
			// Debug.LogError ("Here!!!:"+turn+"/"+countf);
			if (turn == 0 && (int)countf == 0) {
				UpdateFocus (currentLevel);
				UpdateDesc ();
			}
			if (turn == 0 && (int)countf == 8) {
				NextStep ();
				UpdateDesc ();
				UpdateFocus (currentLevel);
			}
		}
		#endregion

	}
}

