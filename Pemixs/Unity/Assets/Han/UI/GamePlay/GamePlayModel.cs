using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Remix
{
	public class GamePlayModel : MonoBehaviour
	{
		
		public void LoadNextLevel(int level){
			RhythmCtrl.InitLevelRhythm (level);
		}

		public void ClearForNewGame(){
			ClearScore ();
			ClearStatistic ();
		}

		#region handle click

		public enum ClickResult {
			HasDone,
			Success,
			Fail,
			InvalidTurnCount
		}

		public delegate void OnClickSuccess(int clickIdx, Game.ClickType type, int turn, float count, float factor);
		public delegate void OnClickFail(int clickIdx, int turn, float count);

		public ClickResult IsBtnClickSuccTimeBase(int beatCntPerTurn, float timePerBeat, int clickIdx, float timer, ref float factor){
			// 用時間算出TouchTurn/TouchCount，記得時間基準要多加TimePerBeat/2
			var turn = 0;
			var count = 0f;
			BeatTool.Time2TurnAndCount (beatCntPerTurn, timePerBeat, timer + timePerBeat/2, ref turn, ref count);
			if (count < 0 || turn >= 2) {
				return ClickResult.InvalidTurnCount;
			}
			if (HasProcessed (turn, (int)Mathf.Floor(count))) {
				return ClickResult.HasDone;
			}
			var playIdx = 0;
			var clickType = Game.ClickType.Pending;
			Game.GetRhythmIndex (RhythmCtrl.RHYTHM_IDX_ARRAY, RhythmCtrl.RHYTHM_BTNMASH_ARRAY, turn, (int)Mathf.Floor(count), ref playIdx, ref clickType);
			// 連續時5和6都一樣
			var ignoreCheck = 
				clickType == Game.ClickType.Long &&
				(clickIdx == 5 || clickIdx == 6) &&
				(playIdx == 5 || playIdx == 6);
			if (ignoreCheck || playIdx == clickIdx) {
				var checkTime = 0f;
				var distToCenter = 0.0f;
				if(Game.CheckTimeDist(RhythmCtrl.RHYTHM_IDX_ARRAY, RhythmCtrl.RHYTHM_BTNMASH_ARRAY, beatCntPerTurn, timePerBeat, timer, ref checkTime, ref distToCenter)){
					factor = Mathf.Abs(distToCenter)/(timePerBeat/2);
					return ClickResult.Success;
				}
			}
			return ClickResult.Fail;
		}

		public void HandleLongClickIfLongClickIsTrueVer2(float sinceTimer, OnClickSuccess OnClickSuccess, OnClickFail OnClickFail){
			if (IsLongClick == false) {
				return;
			}
			// 用時間算出TouchTurn/TouchCount，記得時間基準要多加TimePerBeat/2
			var turn = 0;
			var count = 0f;
			BeatTool.Time2TurnAndCount (16, RhythmCtrl.HALF_BEAT_TIME, sinceTimer + RhythmCtrl.HALF_BEAT_TIME/2, ref turn, ref count);
			if (count <0 || turn >= 2) {
				return;
			}
			var playIdx = 0;
			var clickType = Game.ClickType.Pending;
			Game.GetRhythmIndex (RhythmCtrl.RHYTHM_IDX_ARRAY, RhythmCtrl.RHYTHM_BTNMASH_ARRAY, turn, (int)count, ref playIdx, ref clickType);
			// 取得連擊的按鈕
			var clickIdx = LastClickIdx;
			// 只處理連擊
			if (clickType == Game.ClickType.Long) {
				var factor = 0f;
				var startCnt = 0;
				var endCnt = 0;
				// 連打的非第一擊處理
				Game.ComputeRhythmCountInLongClickSection (RhythmCtrl.RHYTHM_IDX_ARRAY, turn, (int)Mathf.Floor(count), ref startCnt, ref endCnt);
				var isInStartOfLongClickSection = (int)count == startCnt;
				if (isInStartOfLongClickSection == false) {
					var result = IsBtnClickSuccTimeBase (16, RhythmCtrl.HALF_BEAT_TIME, clickIdx, sinceTimer, ref factor);
					switch (result) {
					case GamePlayModel.ClickResult.Success:
						{
							OnClickSuccess (clickIdx, clickType, turn, (int)Mathf.Floor(count), factor);
							// 計算完連擊的最後一拍後，取消連擊，讓加500分失效
							var isInEndOfLongClickSection = (int)count == endCnt;
							if (isInEndOfLongClickSection) {
								CancelLongClick ();
							}
						}
						break;
					case GamePlayModel.ClickResult.HasDone:
						// 忽略掉完成的打擊點
						break;
					case GamePlayModel.ClickResult.InvalidTurnCount:
						break;
					default:
						OnClickFail (clickIdx, turn, count);
						break;
					}
				}
			}
		}


		public void ClickHintVer2(int clickIdx, float sinceTimer, OnClickSuccess OnClickSuccess, OnClickFail OnClickFail, OnClickSuccess OnClickSuccessRepeating){
			// 用時間算出TouchTurn/TouchCount，記得時間基準要多加TimePerBeat/2
			var turn = 0;
			var count = 0f;
			BeatTool.Time2TurnAndCount (16, RhythmCtrl.HALF_BEAT_TIME, sinceTimer + RhythmCtrl.HALF_BEAT_TIME/2, ref turn, ref count);
			if (count <0 || turn >= 2) {
				return;
			}
			var factor = 0.0f;
			var playIdx = 0;
			var clickType = Game.ClickType.Pending;
			// 取得按鍵類型(單擊 or 連擊)
			Game.GetRhythmIndex (RhythmCtrl.RHYTHM_IDX_ARRAY, RhythmCtrl.RHYTHM_BTNMASH_ARRAY, turn, (int)count, ref playIdx, ref clickType);
			switch (clickType) {
			case Game.ClickType.Single:
				{
					// 處理單擊
					var result = IsBtnClickSuccTimeBase (16, RhythmCtrl.HALF_BEAT_TIME, clickIdx, sinceTimer, ref factor);
					switch (result) {
					case GamePlayModel.ClickResult.Success:
						OnClickSuccess (clickIdx, clickType, turn, count, factor);
						break;
					case GamePlayModel.ClickResult.HasDone:
						// 忽略掉完成的打擊點
						break;
					case GamePlayModel.ClickResult.InvalidTurnCount:
						break;
					default:
						OnClickFail (clickIdx, turn, count);
						break;
					}
				}
				break;
			case Game.ClickType.Long:
				{
					var result = IsBtnClickSuccTimeBase (16, RhythmCtrl.HALF_BEAT_TIME, clickIdx, sinceTimer, ref factor);
					switch (result) {
					case GamePlayModel.ClickResult.Success:
						{
							// 連打的第一擊在這裡判斷，其它的在Update中處理(HandleLongClickIfLongClickIsTrue)
							var startCnt = 0;
							var endCnt = 0;
							Game.ComputeRhythmCountInLongClickSection (RhythmCtrl.RHYTHM_IDX_ARRAY, turn, (int)count, ref startCnt, ref endCnt);
							// 若是連打的第一個節奏點
							var isInStartOfLongClickSection = (int)count == startCnt;
							if (isInStartOfLongClickSection) {
								// 開啟連擊判定
								StartLongClickConditionVer2 (sinceTimer, clickIdx);
								// 算分
								OnClickSuccess (clickIdx, clickType, turn, count, factor);
								// 直接append第一次的連擊(這時次數為1次)，這樣在計算連擊的結尾分數時才會算對
								// append一次連擊代表要觸發一次OnClickSuccessRepeating
								// 注意:在呼叫StartLongClickConditionVer2後AppendLongClickVer2才會有效
								AppendLongClickVer2 (sinceTimer, clickIdx);
								// 第一擊成功順便觸發連打事件
								// 加500分
								OnClickSuccessRepeating (clickIdx, clickType, turn, count, 0);
							} else {
								// ignore
								// 非第一擊在HandleLongClickIfLongClickIsTrue
							}
						}
						break;
					case GamePlayModel.ClickResult.HasDone:
						{
							// 加入連擊次數
							// 注意：AppendLongClickVer2通常要判隨著呼叫OnClickSuccessRepeating
							// 但這裡是因為若IsLongClick為false，整個後面的OnClickSuccessRepeating也一并不會觸發到
							// 所以可以這樣寫
							AppendLongClickVer2(sinceTimer, clickIdx);
							// 並判斷IsLongClick
							// 連打中即使HasDone也要判斷是否觸發連打事件
							// IsLongClick中已經判斷了連打按鈕的正確性，所以在這裡不用判斷
							if (IsLongClick) {
								OnClickSuccessRepeating (clickIdx, clickType, turn, count, 0);
							} else {
								// 忽略掉完成的打擊點
							}
						}
						break;
					case GamePlayModel.ClickResult.InvalidTurnCount:
						break;
					default:
						OnClickFail (clickIdx, turn, count);
						break;
					}
				}
				break;
			}
		}

		// ver3是將連擊像太鼓達人一樣處理
		public void ClickHintVer3(int clickIdx, float sinceTimer, OnClickSuccess OnClickSuccess, OnClickFail OnClickFail, OnClickSuccess OnClickSuccessRepeating){
			// 用時間算出TouchTurn/TouchCount，記得時間基準要多加TimePerBeat/2
			var turn = 0;
			var count = 0f;
			BeatTool.Time2TurnAndCount (16, RhythmCtrl.HALF_BEAT_TIME, sinceTimer + RhythmCtrl.HALF_BEAT_TIME/2, ref turn, ref count);
			if (count <0 || turn >= 2) {
				return;
			}
			var factor = 0.0f;
			var playIdx = 0;
			var clickType = Game.ClickType.Pending;
			// 取得按鍵類型(單擊 or 連擊)
			Game.GetRhythmIndex (RhythmCtrl.RHYTHM_IDX_ARRAY, RhythmCtrl.RHYTHM_BTNMASH_ARRAY, turn, (int)count, ref playIdx, ref clickType);
			switch (clickType) {
			case Game.ClickType.Single:
				{
					// 處理單擊
					var result = IsBtnClickSuccTimeBase (16, RhythmCtrl.HALF_BEAT_TIME, clickIdx, sinceTimer, ref factor);
					switch (result) {
					case GamePlayModel.ClickResult.Success:
						OnClickSuccess (clickIdx, clickType, turn, count, factor);
						break;
					case GamePlayModel.ClickResult.HasDone:
						// 忽略掉完成的打擊點
						break;
					case GamePlayModel.ClickResult.InvalidTurnCount:
						break;
					default:
						OnClickFail (clickIdx, turn, count);
						break;
					}
				}
				break;
			case Game.ClickType.Long:
				{
					var result = IsBtnClickSuccTimeBase (16, RhythmCtrl.HALF_BEAT_TIME, clickIdx, sinceTimer, ref factor);
					switch (result) {
					case GamePlayModel.ClickResult.Success:
						{
							var startCnt = 0;
							var endCnt = 0;
							Game.ComputeRhythmCountInLongClickSection (RhythmCtrl.RHYTHM_IDX_ARRAY, turn, (int)count, ref startCnt, ref endCnt);
							// 連擊第一擊成功，記錄下是哪一個段落
							// 這很重要，用來計算一個段落的總得分
							var isStart = startCnt == (int)count;
							if (isStart) {
								LongClickSectionStartCnt = startCnt;
								// 清除次數
								ClearLongClickVer3 ();
							}
							// 算分，這時會加0分
							OnClickSuccess (clickIdx, clickType, turn, count, factor);
							// 第一擊成功順便觸發連打事件
							// 加一次連擊次數
							AppendLongClickVer3 ();
							// 加500分
							OnClickSuccessRepeating (clickIdx, clickType, turn, count, 0);
						}
						break;
					case GamePlayModel.ClickResult.HasDone:
						{
							var startCnt = 0;
							var endCnt = 0;
							Game.ComputeRhythmCountInLongClickSection (RhythmCtrl.RHYTHM_IDX_ARRAY, turn, (int)count, ref startCnt, ref endCnt);
							var isEnd = endCnt == (int)count;
							// 為了分數計算正確
							// 最後一個打擊點不觸發OnClickSuccessRepeating
							if (isEnd == false) {
								// 加一次連擊次數
								AppendLongClickVer3 ();
								// 加500分
								OnClickSuccessRepeating (clickIdx, clickType, turn, count, 0);
							}
						}
						break;
					case GamePlayModel.ClickResult.InvalidTurnCount:
						break;
					default:
						OnClickFail (clickIdx, turn, count);
						break;
					}
				}
				break;
			}
		}
		#endregion

		#region score
		public int score;
		public void ClearScore(){
			score = 0;
		}
		public void AddScore(int v){
			score += v;
			if (score >= RhythmCtrl.MAX_SCORE) {
				score = (int)RhythmCtrl.MAX_SCORE;
			}
		}
		public int Score{ get { return score; } }
		#endregion

		#region statistic
		public int combo;
		public int maxCombo;
		List<string> results = new List<string>();
		public void ClearStatistic(){
			combo = maxCombo = 0;
			results.Clear ();
		}
		public bool AppendResultAndComputeCombo(string result){
			// 先檢查上一個
			// 如果上一個沒有，lastResult一開始為非Bad
			// 第一個Append進來的不是Bad就要判斷為Combo
			var lastResult = "Perfect";
			if (results.Count > 0) {
				lastResult = results[results.Count-1];
			}
			// 再加入
			results.Add (result);
			if (lastResult != "Bad" && result != "Bad") {
				combo += 1;
				if (maxCombo < combo) {
					maxCombo = combo;
				}
				lastResult = result;
				return true;
			} else {
				combo = 0;
				lastResult = result;
				return false;
			}
		}
		public int GetComboCount(){
			return combo;
		}
		public int GetMaxComboCount(){
			return maxCombo;
		}
		public int GetPerfectCount(){
			return results.Count ((ret) => {
				return ret == "Perfect";
			});
		}
		public int GetGreatCount(){
			return results.Count ((ret) => {
				return ret == "Great";
			});
		}
		public int GetGoodCount(){
			return results.Count ((ret) => {
				return ret == "Good";
			});
		}
		public int GetBadCount(){
			return results.Count ((ret) => {
				return ret == "Bad";
			});
		}
		#endregion

		#region procceed
		// 處理不重復打擊問題
		int[,] procceed;
		const int ProcessReadyForClick = 0;
		public const int ProcessClickOK = 1;
		public const int ProcessClickFail = 2;
		// 要先呼叫IGamePlayModeControl.InitMode()才能呼叫這個方法
		// BeatManager會在IGamePlayModeControl中被設定
		public void InitProcessedData(){
			procceed = new int[RhythmCtrl.MAX_TURN, 16];
		}
		// 判斷這個點是否打擊過
		public bool HasProcessed(int turn, int count){
			var playIdx = 0;
			var clickType = Game.ClickType.Pending;
			// 取得按鍵類型(單擊 or 連擊)
			Game.GetRhythmIndex (RhythmCtrl.RHYTHM_IDX_ARRAY, RhythmCtrl.RHYTHM_BTNMASH_ARRAY, turn, count, ref playIdx, ref clickType);
			switch (clickType) {
			case Game.ClickType.Long:
				// 以下判斷無必要並且會失敗
				// 連打中若失敗整段連打都失敗的判斷在long click程式碼區段中就已經處理了
				/*
				int startCnt = 0;
				int endCnt = 0;
				int currCnt = count;
				// 取得連打段落
				Game.ComputeRhythmCountInLongClickSection (RhythmCtrl.RHYTHM_IDX_ARRAY, turn, count, ref startCnt, ref endCnt);
				// 這個連打段落前有其中一個失敗就全部失敗
				for (var i = currCnt; i >= startCnt; --i) {
					if (procceed [turn, i] == ProcessClickFail) {
						return true;
					}
				}
				return false;
				*/
				// 判斷方式和單打一樣就行了
				if (playIdx == 0) {
					return true;
				} else {
					return procceed [turn, count] != ProcessReadyForClick;
				}
			case Game.ClickType.Single:
				if (playIdx == 0) {
					return true;
				} else {
					return procceed [turn, count] != ProcessReadyForClick;
				}
			default:
				throw new UnityException ("clickType為Pending不可能存在，請檢查程式");
			}
		}
		// 標記打擊過
		// 以下3個時機必須要呼叫
		// 1. 打擊成功
		// 2. 打擊失敗
		// 3. 錯過打擊
		public void CompleteProcess(int turn, int count, int state){
			if (procceed [turn, count] != ProcessReadyForClick) {
				throw new UnityException ("同一個節奏點不能重覆呼叫CompleteProcess，請檢查程式");
			}
			procceed [turn, count] = state;
		}
		public bool IsAllProcessInState(int turn, int s, int e, int state){
			for (var i = s; i <= e; ++i) {
				if (procceed [turn, i] != state) {
					return false;
				}
			}
			return true;
		}
		// 清除標記
		void ClearProcessed_XXX(){
			for (var i = 0; i < procceed.GetLength (0); ++i) {
				for (var j = 0; j < procceed.GetLength (1); ++j) {
					procceed [i, j] = ProcessReadyForClick;
				}
			}
		}
		#endregion

		#region long click
		public bool isLongClick;
		public float lastTimer;
		public int lastBtnIdx;
		public int clickCount;
		public int LongClickCount{ get { return clickCount; } }
		// 是否連擊中
		public bool IsLongClick{ get { return isLongClick; } }
		// 啟動連擊的按鈕
		public int LastClickIdx{ get{ return lastBtnIdx; } }
		// 取消連擊判斷，當連擊的最後一點分數計算完畢時呼叫，不然最後的1/4拍還有一半還沒走完可能會判斷按下而加500
		public void CancelLongClick(){
			lastTimer = 0;
		}

		// 開始連擊判定
		// 這個方法在每次成功打擊時呼叫
		// 因為連擊的第1擊是當成單擊處理
		// 第1擊以外的連擊都在Update中觸發
		// 所以這樣判定是有效的
		public void StartLongClickConditionVer2(float timer, int clickIdx){
			lastBtnIdx = clickIdx;
			lastTimer = timer;
			clickCount = 0;
		}

		// 每次按下打擊鈕觸發
		// 注意和StartLongClickCondition呼叫的順序
		public void AppendLongClickVer2(float timer, int clickIdx){
			// 5和6判斷連擊是同等的
			var ignoreCheck = (clickIdx == 5 || clickIdx == 6) && (lastBtnIdx == 5 || lastBtnIdx == 6);
			if (ignoreCheck == false && clickIdx != lastBtnIdx) {
				isLongClick = false;
				return;
			}
			var currTimer = timer;
			var timerOffset = currTimer - lastTimer;
			if (timerOffset < RhythmCtrl.HALF_BEAT_TIME) {
				clickCount += 1;
				isLongClick = true;
				lastTimer = currTimer;
			} else {
				isLongClick = false;
			}
		}

		// 每次Update中都要呼叫這個方法
		public void StepLongClickVer2(float timer){
			var currTimer = timer;
			var timerOffset = currTimer - lastTimer;
			if (timerOffset >= RhythmCtrl.HALF_BEAT_TIME) {
				isLongClick = false;
			}
		}
		#endregion

		#region ver3
		public int longClickSectionStartCnt;
		public int LongClickSectionStartCnt {
			set {
				longClickSectionStartCnt = value;
			}
			get {
				return longClickSectionStartCnt;
			}
		}
		public int clickCountVer3;
		public int LongClickCountVer3{ get { return clickCountVer3; } }
		public void AppendLongClickVer3(){
			++clickCountVer3;
		}
		public void ClearLongClickVer3(){
			clickCountVer3 = 0;
		}
		#endregion
	}
}

