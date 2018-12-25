using System;
using UnityEngine;
using System.Collections.Generic;

namespace Remix
{
	public class GamePlayDancingModeControlVer2 : MonoBehaviour, IGamePlayModeControl
	{
		public Game.DancingModeFlowState state = Game.DancingModeFlowState.Pending;
		public float sinceTime;
		public float syncTimer;
		public bool isFeverMode;
		public int requestFeverLevel;

		public GamePlayView view;
		public GamePlayModel model;
		public GamePlayModelControlHelper helper;

		public bool IsGameEnd{ get{ return state == Game.DancingModeFlowState.End; } }

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
			sinceTime = RhythmCtrl.HALF_BEAT_TIME * 4 * 4;
		}

		public void Step(float audioTime, float audioOffset){
			if (IsGameEnd) {
				return;
			}
			var syncTime = audioTime + audioOffset;
			// 處理被延後的Fever Start
			if (isFeverMode == false) {
				if (requestFeverLevel > 0) {
					// 計算Fever Start的開始音樂時間(4拍倒數+Fever前Level數*24拍)
					var feverStartTime = 
						RhythmCtrl.HALF_BEAT_TIME * 4 * 4 +
						(RhythmCtrl.MAX_LEVEL - RhythmCtrl.FEVER_LENGTH) * RhythmCtrl.HALF_BEAT_TIME * 4 * 24;
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
			Game.ComputeDancingModeState(RhythmCtrl.HALF_BEAT_TIME*4, RhythmCtrl.MAX_LEVEL-RhythmCtrl.FEVER_LENGTH, RhythmCtrl.FEVER_LENGTH, isFeverMode, syncTime, ref state, ref currentSinceTime);
			if (lastState != state) {
				// 以下2個時機點要讀取關卡
				var shouldLoadNext = 
					(lastState == Game.DancingModeFlowState.Rest && state == Game.DancingModeFlowState.EnemyPerformance) ||
					(lastState == Game.DancingModeFlowState.FeverRest && state == Game.DancingModeFlowState.FeverEnemyPerformance);
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
			// 這個state其實不該用來當音樂結束的判斷，
			// 因為若使用者將AudioOffset設為正N秒，則遊戲會比音樂提前N秒結束
			// 這樣做的原因為：
			// 1. 程式不能讓玩家設為正秒
			// 2. 當成防呆處理
			// 音樂結束的判斷在Main.IsGameEnd
			if (state == Game.DancingModeFlowState.End) {
				// 判斷勝負
				var fullHp = 100;
				float finalHpScale = view.GetHP() * 100.0f / fullHp;
				bool isWin = (finalHpScale >= RhythmCtrl.LEVEL_THRESHOLD);
				OnLevelClear (isWin);
				return;
			}
			// 同步打擊點
			// 注意：時間都必須多位移8拍的時間，因為都是8拍前預讀的
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
			switch (state) {
			case Game.DancingModeFlowState.EnemyPerformance:
			case Game.DancingModeFlowState.FeverEnemyPerformance:
				{
					var enemyTime = timer + RhythmCtrl.HALF_BEAT_TIME * 4 * 8;
					helper.OnAutoSoundEffect (enemyTime, GamePlayView.RightPos);
				}
				break;
			default:
				helper.OnAutoSoundEffect (timer);
				break;
			}
		}

		void OnShiningBeat(int beat){
			if (beat % 2 == 0) {
				StartCoroutine (view.ShiningHint ());
			}
		}

		void OnClickSuccess(int clickIdx, Game.ClickType clickType, int turn, float count, float factor){
			var clickResult = Game.HandleOnClickSuccess (this, currentLevel, clickIdx, clickType, turn, count, factor, OnHpChange);
			OnClickHintResult (clickResult);
			var animIdx = StageView.P1_BTN_01_IDX + (clickIdx - 1);
			// 若符合輪播條件，取代原動畫
			if (IsInLoopAnimCase (clickType, clickIdx)) {
				animIdx = CurrentAnimIdx ();
				AppendLastAnimIdx (animIdx);
			}
			view.PlayCatAnim (GamePlayView.LeftPos, animIdx, StageView.P1_DEFAULT_IDX, true, OnPlayCatAnimAfter);
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
			if (helper.IsUseAutoSoundEffect == false) {
				helper.PlayCatSound (clickIdx);
			}
			var animIdx = StageView.P1_BTN_01_IDX + (clickIdx - 1);
			view.PlayCatAnim (GamePlayView.LeftPos, animIdx, StageView.P1_DEFAULT_IDX, false, OnPlayCatAnimAfter);
		}

		void OnClickSuccessRepeating(int clickIdx, Game.ClickType clickType, int turn, float count, float factor){
			Game.HandleOnClickSuccessRepeating (this , clickIdx, clickType, turn, count, factor);
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
			var timer = this.syncTimer - sinceTime + OffsetTime;
			Game.HandleOnBeat (view, model, timer, beat, OnClickSuccess, OnClickFail);

			// 處理移動Stage
			// 每個level都在同樣的時間移動就行了
			if (Game.IsFeverModeSection (currentLevel) == false) {
				if (beat == - 4 * 8) {
					view.StepMoveStage ();
				}
				if (beat == - 4 * 0) {
					view.StepMoveStage ();
				}
				if (beat == 4 * 8) {
					view.StepMoveStage ();
				}
			}
			// 處理對手貓動畫
			// 把負數的beat區段 + 對手貓演時間的區段 = 對手貓表演的開始時間(從0開始)
			var enemyTime = timer + RhythmCtrl.HALF_BEAT_TIME * 4 * 8;
			if (enemyTime >= 0) {
				var turn = 0;
				var count = 0.0f;
				BeatTool.Time2TurnAndCount (16, RhythmCtrl.HALF_BEAT_TIME, enemyTime, ref turn, ref count);
				if (turn / RhythmCtrl.MAX_TURN == 0) {
					var playIdx = 0;
					var clickType = Game.ClickType.Pending;
					Game.GetRhythmIndex (RhythmCtrl.RHYTHM_IDX_ARRAY, RhythmCtrl.RHYTHM_BTNMASH_ARRAY, turn, (int)count, ref playIdx, ref clickType);
					if (playIdx > 0) {
						var animIdx = StageView.P1_BTN_01_IDX + (playIdx - 1);
						// 若符合輪播條件，取代原動畫
						// 左貓和右貓共用輪播資訊
						if (IsInLoopAnimCase (clickType, playIdx)) {
							animIdx = CurrentAnimIdx ();
							AppendLastAnimIdx (animIdx);
						}
						view.PlayCatAnim (GamePlayView.RightPos, animIdx, StageView.P1_DEFAULT_IDX, true, OnPlayCatAnimAfter);
					}
				}
			}
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

		// 當呼叫PlayCatAnim後，都會觸發OnPlayCatAnimAfter
		// triggerWithSuccess參數會一并被傳過來，代表是不是因為成功打擊而觸發的
		void OnPlayCatAnimAfter(int pos, int animIdx, bool triggerWithSuccess){
			if (helper.IsUseAutoSoundEffect == true) {
				return;
			}

			var timer = this.syncTimer - sinceTime + OffsetTime;
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
						// 連擊的Loop動畫部分，音樂也跟著輪播
						switch(animIdx){
						case StageView.P1_BTN_LOOP_1_IDX:
							playIdx = 5;
							break;
						case StageView.P1_BTN_LOOP_2_IDX:
							playIdx = 6;
							break;
						}
						helper.PlayCatSound(playIdx);
					} catch (IndexOutOfRangeException e){
						Debug.LogError ("回合拍數不合法，請檢查"+turn+"/"+count);
						throw e;
					}
				}
			} else {
				// 注意：右邊貓的拍數計算要另外計
				// 把負數的beat區段 + 對手貓演時間的區段 = 對手貓表演的開始時間(從0開始)
				var enemyTime = timer + RhythmCtrl.HALF_BEAT_TIME * 4 * 8;
				var turn = 0;
				var count = 0.0f;
				BeatTool.Time2TurnAndCount (16, RhythmCtrl.HALF_BEAT_TIME, enemyTime, ref turn, ref count);
				if (turn >= 0 && turn < RhythmCtrl.MAX_TURN) {
					try {
						var playIdx = 0;
						var clickType = Game.ClickType.Pending;
						Game.GetRhythmIndex(RhythmCtrl.RHYTHM_IDX_ARRAY, RhythmCtrl.RHYTHM_BTNMASH_ARRAY, turn, (int)count, ref playIdx, ref clickType);
						var type = RhythmCtrl.RHYTHM_TYPE_ARRAY [turn] [(int)count];
						// 連擊的Loop動畫部分，音樂也跟著輪播
						switch(animIdx){
						case StageView.P1_BTN_LOOP_1_IDX:
							playIdx = 5;
							break;
						case StageView.P1_BTN_LOOP_2_IDX:
							playIdx = 6;
							break;
						}
						// 右邊的貓不處理打擊錯誤(triggerWithSuccess)
						view.PlayCatSound (pos, playIdx, type);
					} catch (IndexOutOfRangeException e) {
						Debug.LogError ("回合拍數不合法，請檢查" + turn + "/" + count);
						throw e;
					}
				}
			}
		}

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

