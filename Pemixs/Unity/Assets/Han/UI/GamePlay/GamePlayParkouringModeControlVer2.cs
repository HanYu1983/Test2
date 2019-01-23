using System;
using UnityEngine;
using System.Collections.Generic;

namespace Remix
{
	public class GamePlayParkouringModeControlVer2 : MonoBehaviour, IGamePlayModeControl
	{
		public Game.ParkouringModeFlowState state = Game.ParkouringModeFlowState.Pending;
		public float sinceTime;
		public float syncTimer;
		public bool isFeverMode;
		public int requestFeverLevel;

		public GamePlayView view;
		public GamePlayModel model;
		public GamePlayModelControlHelper helper;

		public bool IsGameEnd{ get{ return state == Game.ParkouringModeFlowState.End; } }

		public int currentLevel = 1;
		public int CurrentLevel{ get { return currentLevel; } }

		public bool IsFeverMode{ get { return isFeverMode; } }

		public GamePlayView GamePlayView{ get{ return view; }}
		public GamePlayModel GamePlayModel{ get{ return model; } }

		public void InitComponent(){
			view = GetComponent<GamePlayView> ();
			model = GetComponent<GamePlayModel> ();
			helper = GetComponent<GamePlayModelControlHelper> ();
		}

		public void InitMode(){
			model.ClearForNewGame ();
			view.UpdateScore (model.Score);
			view.StageView.ResetStage ();
			view.StageView.ResetCat ();
			sinceTime = RhythmCtrl.HALF_BEAT_TIME * 4 * 8;
		}

		public void Step(float audioTime, float audioOffset){
			if (IsGameEnd) {
				return;
			}
			var syncTime = audioTime + audioOffset;
			// 處理被延後的Fever Start
			if (isFeverMode == false) {
				if (requestFeverLevel > 0) {
					// 計算Fever Start的開始音樂時間(4拍倒數+Fever前Level數*8拍)
					var feverStartTime = 
						RhythmCtrl.HALF_BEAT_TIME * 4 * 8 +
						(RhythmCtrl.MAX_LEVEL - RhythmCtrl.FEVER_LENGTH) * RhythmCtrl.HALF_BEAT_TIME * 4 * 8;
					if (syncTime > feverStartTime) {
						// 注意：sinceTime是用來計算正確的打擊代碼，這個類的sinceTime都是這個意義
						sinceTime = feverStartTime + RhythmCtrl.HALF_BEAT_TIME * 4 * 8;
						// 讀取Fever的level
						Game.LoadLevel (view, model, requestFeverLevel);
						currentLevel = requestFeverLevel;
						// 觸發動畫等...
						OnStartFeverMode ();
					}
				}
			} else {
				// fever的延遲
				syncTime += audioOffset;
			}
			var lastState = state;
			var currentSinceTime = 0f;
			Game.ComputeParkouringModeState(RhythmCtrl.HALF_BEAT_TIME*4, RhythmCtrl.MAX_LEVEL-RhythmCtrl.FEVER_LENGTH, RhythmCtrl.FEVER_LENGTH, isFeverMode, syncTime, ref state, ref currentSinceTime);
			if (lastState != state) {
				// 以下2個時機點要讀取關卡
				var shouldLoadNext = 
					(lastState == Game.ParkouringModeFlowState.PlayerInputEven && state == Game.ParkouringModeFlowState.PlayerInputOdd) ||
					(lastState == Game.ParkouringModeFlowState.PlayerInputOdd && state == Game.ParkouringModeFlowState.PlayerInputEven) ||
					(lastState == Game.ParkouringModeFlowState.FeverPlayerInputEven && state == Game.ParkouringModeFlowState.FeverPlayerInputOdd) ||
					(lastState == Game.ParkouringModeFlowState.FeverPlayerInputOdd && state == Game.ParkouringModeFlowState.FeverPlayerInputEven);
				if (shouldLoadNext) {
					Game.HandleLevelEnd (view, model, currentLevel, null, null, (next)=>{
						// 若判斷將要讀的是Fever的關卡必須延後處理
						if(isFeverMode == false){
							if(Game.IsFeverModeSection(next)){
								requestFeverLevel = next;
								return false;
							}
						}
						// 一般關卡直接讀取, 回傳true就會自動讀取
						sinceTime = currentSinceTime;
						currentLevel = next;
						return true;
					});
				}
			}
			// 時間到就遊戲結束
			if (state == Game.ParkouringModeFlowState.End) {
				// 判斷勝負
				var fullHp = view.GetFullHP();
				float finalHpScale = view.GetHP() * 100.0f / fullHp;
				bool isWin = (finalHpScale >= RhythmCtrl.LEVEL_THRESHOLD);
				OnLevelClear (isWin);
				return;
			}
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
			// 產生節拍事件OnBeat
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
			var clickResult = Game.HandleOnClickSuccess (this, currentLevel, clickIdx, clickType, turn, count, factor, OnHpChange);
			OnClickHintResult (clickResult);

			var fixCount = (int)Mathf.Floor (count);
			helper.AppendRhythmType(turn, fixCount);

			if (Game.IsFeverModeSection (currentLevel) == false) {
				var animIdx = StageView.P3_BAD_IDX + clickIdx;
				// 若符合輪播條件，取代原動畫
				if (IsInLoopAnimCase (clickType, clickIdx)) {
					animIdx = CurrentAnimIdx ();
					AppendLastAnimIdx (animIdx);
				}
				// 處理音效
				if (helper.IsUseAutoSoundEffect == false) {
					if (IsInLoopAnimCase (clickType, clickIdx)) {
						switch (animIdx) {
						case StageView.P3_BTN_LOOP_1_IDX:
							{
								helper.PlayCatSound (5);
							}
							break;
						case StageView.P3_BTN_LOOP_2_IDX:
							{
								helper.PlayCatSound (6);
							}
							break;
						}
					} else {
						helper.PlayCatSound (clickIdx);	
					}
				}
				view.PlayCatAnim (GamePlayView.LeftPos, animIdx, StageView.P1_DEFAULT_IDX, true, null);
			} else {
				var animIdx = StageView.P3_FEVER_UP_IDX;

				// 若符合輪播條件，取代原動畫
				if (IsInLoopAnimCase (clickType, clickIdx)) {
					animIdx = CurrentAnimIdx ();
					AppendLastAnimIdx (animIdx);
					switch (animIdx) {
					case StageView.P3_BTN_LOOP_1_IDX:
						{
							// fever mode要把動畫換回fever
							animIdx = StageView.P3_FEVER_UP_IDX;
						}
						break;
					case StageView.P3_BTN_LOOP_2_IDX:
						{
							// fever mode要把動畫換回fever
							animIdx = StageView.P3_FEVER_DOWN_IDX;
						}
						break;
					}
				}

				// 處理音效
				if (helper.IsUseAutoSoundEffect == false) {
					if (IsInLoopAnimCase (clickType, clickIdx)) {
						switch (animIdx) {
						case StageView.P3_BTN_LOOP_1_IDX:
							{
								helper.PlayCatSound (5);
							}
							break;
						case StageView.P3_BTN_LOOP_2_IDX:
							{
								helper.PlayCatSound (6);
							}
							break;
						default:
							helper.PlayCatSound (clickIdx);	
							break;
						}

					} else {
						helper.PlayCatSound (clickIdx);
					}
				}

				view.PlayCatAnim (GamePlayView.LeftPos, animIdx, StageView.P3_FEVER_IDLE_IDX, true, null);
			}
		}

		void OnHpChange(int old, int curr){
			UIEventFacade.OnGameEvent.OnNext ("GamePlayUIHpChange");
		}

		void OnClickFail(int clickIdx, int turn, float count){
			if (count < 0 || turn >= RhythmCtrl.MAX_TURN) {
				return;
			}
			var result = Game.HandleOnClickFail (this, clickIdx, turn, count, OnHpChange);
			OnClickHintResult (result);

			var animIdx = StageView.P3_HURT_IDX;
			var afterAnimIdx = StageView.P1_DEFAULT_IDX;
			if (Game.IsFeverModeSection (currentLevel)) {
				animIdx = StageView.P3_FEVER_UP_IDX;
				afterAnimIdx = StageView.P3_FEVER_IDLE_IDX;
			}
			view.PlayCatAnim (GamePlayView.LeftPos, animIdx, afterAnimIdx, false, null);

			if (helper.IsUseAutoSoundEffect == false) {
				helper.PlayCatSound (SoundDataCtrl.HURT_SOUND);
			}
		}

		void OnClickSuccessRepeating(int clickIdx, Game.ClickType clickType, int turn, float count, float factor){
			Game.HandleOnClickSuccessRepeating (this, clickIdx, clickType, turn, count, factor);
		}

		void OnClickHintResult(string result){
			// 計算combo
			if (model.AppendResultAndComputeCombo (result)) {
				view.wordZone.ShowCombo (model.GetComboCount ());
			}
			// 以上流程3個模式通用
			// 以下流程為對戰模式處理
		}

		void OnStartFeverMode(){
			StartCoroutine (Game.InvokeStartFeverMode(view, model));
			view.StageView.ChangeAnimation (view.StageView.LeftCat, StageView.P3_FEVER_IDLE_IDX, true);
			isFeverMode = true;
		}
		void OnLevelClear(bool isWin){
			StartCoroutine(Game.HandleLevelClear(view, model, isWin, isFeverMode));
		}
		void OnBeat(int beat){
			var timer = this.syncTimer - sinceTime + OffsetTime;
			Game.HandleOnBeat (view, model, timer, beat, OnClickSuccess, OnClickFail);
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
			var result = Game.HandleOnTouchBeatEnd (view, model, currentLevel, turn, count, OnHpChange);
			if(result != null){
				OnClickHintResult (result);
				// 貓撞到障礙物
				var animIdx = StageView.P3_HURT_IDX;
				var afterAnimIdx = StageView.P1_DEFAULT_IDX;
				if (Game.IsFeverModeSection (currentLevel)) {
					animIdx = StageView.P3_FEVER_UP_IDX;
					afterAnimIdx = StageView.P3_FEVER_IDLE_IDX;
				}
				view.PlayCatAnim (GamePlayView.LeftPos, animIdx, afterAnimIdx, false, null);
				if (helper.IsUseAutoSoundEffect == false) {
					helper.PlayCatSound (SoundDataCtrl.HURT_SOUND);
				}
			}
		}

		public void OnGameButtonClick(string command){
			if (model == null) {
				throw new UnityException ("處理按鈕事件前請先呼叫InitMode!");
			}
			Game.HandleOnGameButtonClick (view, model, syncTimer - sinceTime + OffsetTime, currentLevel, command, OnClickSuccess, OnClickFail, OnClickSuccessRepeating);
		}
		public void OnGameButtonSlide(string command){

		}

		float OffsetTime{
			get{
				return -RhythmCtrl.HALF_BEAT_TIME * 4 * 0;
			}
		}

		#region handleLoopAnim
		int lastAnimIdx = StageView.P3_BTN_LOOP_1_IDX;
		// 輪播條件
		bool IsInLoopAnimCase(Game.ClickType clickType, int clickIdx){
			// 若是連擊並且是轉圈
			return clickType == Game.ClickType.Long && (clickIdx == 5 || clickIdx == 6);
		}
		void AppendLastAnimIdx(int animIdx){
			lastAnimIdx = animIdx;
		}
		int CurrentAnimIdx(){
			return lastAnimIdx == StageView.P3_BTN_LOOP_1_IDX ? StageView.P3_BTN_LOOP_2_IDX : StageView.P3_BTN_LOOP_1_IDX;
		}
		#endregion
	}
}

