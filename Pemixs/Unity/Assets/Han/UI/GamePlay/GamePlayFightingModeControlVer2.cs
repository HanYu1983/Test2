using System;
using UnityEngine;
using System.Collections.Generic;

namespace Remix
{
	public class GamePlayFightingModeControlVer2 : MonoBehaviour, IGamePlayModeControl
	{
		public Game.FightingModeFlowState state = Game.FightingModeFlowState.Pending;
		public float sinceTime;
		public float syncTimer;
		public bool isFeverMode;
		public int requestFeverLevel;

		public GamePlayView view;
		public GamePlayModel model;
		public GamePlayModelControlHelper helper;

		public bool IsGameEnd{ get{ return state == Game.FightingModeFlowState.End; } }

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
			InitAttackControl ();
		}
			
		public void Step(float audioTime, float audioOffset){
			if (IsGameEnd) {
				return;
			}
			var syncTime = audioTime + audioOffset;
			// 處理被延後的Fever Start
			if (isFeverMode == false) {
				if (requestFeverLevel > 0) {
					// 計算Fever Start的開始音樂時間(8拍倒數+Fever前Level數*16拍)
					var feverStartTime = 
						RhythmCtrl.HALF_BEAT_TIME * 4 * 8 + 
						(RhythmCtrl.MAX_LEVEL - RhythmCtrl.FEVER_LENGTH) * RhythmCtrl.HALF_BEAT_TIME * 4 * 8 * 2;
					if (syncTime > feverStartTime) {
						// 注意：sinceTime是用來計算正確的打擊代碼，這個類的sinceTime都是這個意義
						sinceTime = feverStartTime;
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
			Game.ComputeFightingModeState(RhythmCtrl.HALF_BEAT_TIME*4, RhythmCtrl.MAX_LEVEL-RhythmCtrl.FEVER_LENGTH, RhythmCtrl.FEVER_LENGTH, isFeverMode, syncTime, ref state, ref currentSinceTime);
			if (lastState != state) {
				// 以下3個時機點要讀取關卡
				var shouldLoadNext = 
					(lastState == Game.FightingModeFlowState.PlayerInput && state == Game.FightingModeFlowState.EnemyAttack) ||
					(lastState == Game.FightingModeFlowState.PlayerInput && state == Game.FightingModeFlowState.PlayerAttack) ||
					(lastState == Game.FightingModeFlowState.FeverPlayerInput && state == Game.FightingModeFlowState.FeverRest);
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
				// 以下2個時機要播攻擊動畫
				var shouldPlayAttackAnim = 
					state == Game.FightingModeFlowState.EnemyAttack ||
					state == Game.FightingModeFlowState.PlayerAttack;
				if(shouldPlayAttackAnim){
					if (Game.IsFeverModeSection (currentLevel) == false) {
						bool isWinLevel = IsWinLevel ();
						NextGenAndAttackAnimation (GetAttackPos(), isWinLevel, currentLevel, false);
						ChangeAttackPos ();
					}
				}
			}
			// 時間到就遊戲結束
			if (state == Game.FightingModeFlowState.End) {
				// 判斷勝負
				var fullHp = 100;
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
			if (Game.IsFeverModeSection (currentLevel)) {
				NextGenAndAttackAnimation (GamePlayView.LeftPos, true, currentLevel, true);
			}

			// 注意：這裡的animIdx只是用來計算!!!
			var animIdx = StageView.P1_BTN_01_IDX + (clickIdx - 1);
			// 若符合輪播條件，取代原動畫
			if (IsInLoopAnimCase (clickType, clickIdx)) {
				animIdx = CurrentAnimIdx ();
				AppendLastAnimIdx (animIdx);
			}
			// animIdx只拿來計算音效!!!
			var playIdx = clickIdx;
			// 連擊的Loop動畫部分，音樂也跟著輪播
			switch(animIdx){
			case StageView.P1_BTN_LOOP_1_IDX:
				playIdx = 5;
				break;
			case StageView.P1_BTN_LOOP_2_IDX:
				playIdx = 6;
				break;
			}
			if (helper.IsUseAutoSoundEffect == false) {
				var fixCount = (int)count;
				// 若打擊成功，記住它並使用它
				helper.AppendRhythmType (turn, fixCount);
				helper.PlayCatSound (playIdx);
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
			// 以上流程3個模式通用
			// 以下流程為對戰模式處理
			helper.PlayCatSound (clickIdx);
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
			isFeverMode = true;
		}
		void OnLevelClear(bool isWin){
			StartCoroutine(Game.HandleLevelClear(view, model, isWin, isFeverMode));
		}
		void OnBeat(int beat){
			Game.HandleOnBeat (view, model, this.syncTimer - sinceTime + OffsetTime, beat, OnClickSuccess, OnClickFail);
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
				return -RhythmCtrl.HALF_BEAT_TIME * 4 * 8;
			}
		}

		#region attack control
		float lastHp;
		int attackPos = GamePlayView.RightPos;
		int feverAnimIdx = 0;
		int GetAttackPos(){
			return attackPos;
		}
		void InitAttackControl(){
			lastHp = view.GetHP ();
		}
		void ChangeAttackPos(){
			attackPos = 
				attackPos == GamePlayView.LeftPos ?
				GamePlayView.RightPos : 
				GamePlayView.LeftPos;
		}
		bool IsWinLevel(){
			var hp = view.GetHP ();
			var isWin = (hp >= lastHp && hp > 0);
			return isWin;
		}
		void NextGenAndAttackAnimation(int attackPos, bool win, int currentLevel, bool isFeverMode){
			if (win) {
				lastHp = view.GetHP ();
			}
			var atkIdx = 
				currentLevel % 4 > 1 ? 
				StageView.P2_UPPER_ATK_IDX : 
				StageView.P2_LOWER_ATK_IDX;

			var defIdx = 0;
			if (attackPos == GamePlayView.LeftPos) {
				defIdx = 
					currentLevel % 4 > 1 ? 
					(win ? StageView.P2_HURT_IDX : StageView.P2_UPPER_DEF_IDX) : 
					(win ? StageView.P2_HURT_IDX : StageView.P2_LOWER_DEF_IDX);
			} else {
				defIdx = 
					currentLevel % 4 > 1 ? 
					(win ? StageView.P2_UPPER_DEF_IDX : StageView.P2_HURT_IDX) : 
					(win ? StageView.P2_LOWER_DEF_IDX : StageView.P2_HURT_IDX);
			}

			var delayCount = defIdx == StageView.P2_HURT_IDX ? 8f : 4f;

			if (isFeverMode) {
				atkIdx = 
					feverAnimIdx % 2 == 0 ? 
					StageView.P2_FEVER_UPPER_ATK_IDX : 
					StageView.P2_FEVER_LOWER_ATK_IDX;
				defIdx = StageView.P2_FEVER_HURT_IDX;
				delayCount = 2f;
				feverAnimIdx += 1;
			}
			SoundDataCtrl leftSound = view.LeftSound;
			SoundDataCtrl rightSound = view.RightSound;
			// Fever Mode中取消播放音效
			if (isFeverMode) {
				leftSound = rightSound = null;
			}
			if (attackPos == GamePlayView.LeftPos) {
				// 改變貓的圖層順序
				view.StageView.ChangeCatInTopLayer (view.StageView.LeftCat);
				// 播放攻擊動畫
				StartCoroutine (view.StageView.InvokeCatAttack (view.StageView.LeftCat, atkIdx, defIdx, delayCount* RhythmCtrl.HALF_BEAT_TIME, leftSound, rightSound));
				// Fever模式起動時就會先移動場地到被攻擊方
				// 所以Fever中不必移動場地
				// 參照Game.InvokeStartFeverMode
				if (isFeverMode == false) {
					StartCoroutine (view.StageView.MoveStage (StageView.RightAndBack));
				}
			} else {
				// 改變貓的圖層順序
				view.StageView.ChangeCatInTopLayer (view.StageView.RightCat);
				// 播放攻擊動畫
				StartCoroutine (view.StageView.InvokeCatAttack (view.StageView.RightCat, atkIdx, defIdx, delayCount* RhythmCtrl.HALF_BEAT_TIME, leftSound, rightSound));
				// Fever模式起動時就會先移動場地到被攻擊方
				// 所以Fever中不必移動場地
				// 參照Game.InvokeStartFeverMode
				if (isFeverMode == false) {
					StartCoroutine (view.StageView.MoveStage (StageView.LeftAndBack));
				}
			}
		}
		#endregion

		// 在P2模式中，這裡指定的P1動畫只是用來計算，不會真的用到
		#region handleLoopAnim
		int lastAnimIdx = StageView.P1_BTN_LOOP_1_IDX;
		// 輪播條件
		bool IsInLoopAnimCase(Game.ClickType clickType, int clickIdx){
			// 若是連擊並且是轉圈
			return clickType == Game.ClickType.Long && (clickIdx == 5 || clickIdx == 6);
		}
		void AppendLastAnimIdx(int animIdx){
			lastAnimIdx = animIdx;
		}
		int CurrentAnimIdx(){
			return lastAnimIdx == StageView.P1_BTN_LOOP_1_IDX ? StageView.P1_BTN_LOOP_2_IDX : StageView.P1_BTN_LOOP_1_IDX;
		}
		#endregion
	}
}

