using System;
using UnityEngine;
using System.Collections.Generic;

namespace Remix
{
	public class GamePlayDelayToolModeControlVer2 : MonoBehaviour, IGamePlayModeControl
	{
		public float syncTimer;
		public float sinceTime;
		public GamePlayView view;
		public GamePlayModel model;
		public GamePlayModelControlHelper helper;

		public bool IsGameEnd{ get{ return false; } }

		public int currentLevel = 1;
		public int CurrentLevel{ get { return currentLevel; } }

		public bool IsFeverMode{ get { return false; } }

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
			// 將鏡頭移到左貓
			view.StageView.StepMoveStage ();
			view.StageView.StepMoveStage ();
			view.StageView.RightCat.SetActive (false);
		}
			
		public void Step(float audioTime, float audioOffset){
			var syncTime = audioTime + audioOffset;
			// DelayTool音樂會loop
			// 剛loop時，要重設sinceTime和LoadLevel
			var isLoop = syncTime < this.syncTimer;
			if(isLoop){
				sinceTime = syncTime;
				Game.LoadLevel(view, model, 1);
			}
			// 同步打擊點
			view.SyncTimer (syncTime - sinceTime);
			// 同步貓的位置
			view.UpdateCatAnimation ();
			this.syncTimer = syncTime;
			// 產生節拍事件OnBeat
			AppendTimeForComputeBeat (0, syncTime - sinceTime, OnBeat);
			AppendTimeForComputeBeat (2, audioTime + audioOffset, OnShiningBeat);
			// 自動打擊
			if (helper.IsAutoPlay) {
				helper.AutoClickHintVer2 (syncTime - sinceTime, OnGameButtonClick);
			}
			if (helper.IsUseAutoSoundEffect) {
				helper.OnAutoSoundEffectStep (syncTime - sinceTime, OnAutoSoundEffect);
			}
		}

		void OnAutoSoundEffect(int beat){
			var timer = this.syncTimer - sinceTime ;
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
			model.CompleteProcess (turn, (int)count, GamePlayModel.ProcessClickFail);
		}

		void OnClickSuccessRepeating(int clickIdx, Game.ClickType clickType, int turn, float count, float factor){
			
		}

		void OnBeat(int beat){
			var isOver = (syncTimer - sinceTime) / (RhythmCtrl.HALF_BEAT_TIME * 4 * 8);
			if (isOver >= 1) {
				sinceTime = syncTimer;
				// 永遠讀第1個就行了
				Game.LoadLevel(view, model, 1);
			}
		}

		public void OnGameButtonClick(string command){
			if (model == null) {
				throw new UnityException ("處理按鈕事件前請先呼叫InitMode!");
			}
			Game.HandleOnGameButtonClick (view, model, syncTimer - sinceTime, currentLevel, command, OnClickSuccess, OnClickFail, OnClickSuccessRepeating);
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

		#region beat
		List<int> beatStore = new List<int>(){-1,-1,-1};
		void AppendTimeForComputeBeat(int slot, float timer, Action<int> OnBeatFn){
			var currBeat = (int)Mathf.Floor (timer / RhythmCtrl.HALF_BEAT_TIME);
			if (beatStore[slot] != currBeat) {
				beatStore[slot] = currBeat;
				if (OnBeatFn != null) {
					OnBeatFn (beatStore[slot]);
				}
			}
		}
		#endregion
	}
}

