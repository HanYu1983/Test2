using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Remix
{
	public class Game : MonoBehaviour
	{
		public LevelKey levelKey;
		public int leftCatIdx, rightCatIdx;
		public bool isPause;

		public GamePlayView view;
		public GamePlayModel model;
		public GamePlayModelControlHelper helper;

		public MonoBehaviour dancingMode;
		public IGamePlayModeControl modeControl;

		public bool IsPause{ get { return isPause; } }
		public GamePlayView GamePlayView{ get { return view; } }
		public GamePlayModel GamePlayModel{ get { return model; } }
		public GamePlayModelControlHelper Helper{ get { return helper; } }

		public void InitComponent(){
			view = GetComponent<GamePlayView> ();
			model = GetComponent<GamePlayModel> ();
			helper = GetComponent<GamePlayModelControlHelper> ();
			isPause = true;
		}

		public bool IsDelayTool{
			get{
				return modeControl is GamePlayDelayToolModeControlVer2;
			}
		}

		public bool IsTutorial{
			get{
				return modeControl is GamePlayTutorialModeControl;
			}
		}

		public IGamePlayModeControl GamePlayModeControl{
			get{
				if (modeControl == null) {
					throw new UnityException ("please call SetData first");
				}
				return modeControl;
			}
		}

		public void SetData(int mode, float timeScale){
			modeControl = GetGameModeControl (mode);
			modeControl.InitComponent ();

			dancingMode = modeControl as MonoBehaviour;
			view.animationTimeScale = timeScale;
			// 建立view
			view.InitView (levelKey, leftCatIdx, rightCatIdx, mode, modeControl.CurrentLevel);
		}

		public void SetHp(int hp, int maxHp){
			view.ResetHp (hp, maxHp);
		}

		public IEnumerator StartPlayWhenLevelSoundLoaded(){
			if (modeControl == null) {
				throw new UnityException ("請先呼叫SetData");
			}
			yield return view.WaitForLoaded ();
			if (view.LoadState == "Failed") {
				throw new UnityException ("音樂讀取錯誤，這個地方需要處理");
			}
			StartPlay ();
		}

		public void StartPlay(){
			view.PlayBGM ();
			isPause = false;
			// 啟動遊戲模式
			modeControl.InitMode ();
			// 讀取第一個關卡
			LoadLevel (view, model, modeControl.CurrentLevel);
		}
			
		public void Step(float audioTime, float audioOffset){
			if (isPause) {
				return;
			}
			modeControl.Step (audioTime, audioOffset);
		}

		public void PerformButtonCommand(string command){
			switch (command) {
			case "REPLAY":
				// 啟動遊戲模式
				view.HideRetryDlg ();
				StartPlay ();
				break;
			case "Pause":
				if (modeControl.IsGameEnd) {
					Debug.LogWarning ("遊戲結束時無法暫停");
					return;
				}
				OnPause ();
				break;
			case "BackGame":
				OnPause ();
				break;
			case "BackMenu":
				// 跳回地圖頁
				Game.BackToMap ();
				break;
			default:
				modeControl.OnGameButtonClick (command);
				break;
			}
		}

		void OnButtonSlide(string command){
			modeControl.OnGameButtonSlide (command);
		}

		public void OnPause(){
			/*
			// 先特殊處理
			if (modeControl is GamePlayFightingModeControlVer2 == false) {
				if (model.GetState () != GamePlayModel.State.Play) {
					return;
				}
			}
			*/
			isPause = !isPause;
			view.SetViewPause (isPause, modeControl.IsFeverMode);
		}

		IGamePlayModeControl GetGameModeControl(int mode){
			switch (mode) {
			case RhythmCtrl.DANCING_MODE:
				return GetComponent<GamePlayDancingModeControlVer2>();
			case RhythmCtrl.FIGHTING_MODE:
				return GetComponent<GamePlayFightingModeControlVer2>();
			case RhythmCtrl.PARKOURING_MODE:
				return GetComponent<GamePlayParkouringModeControlVer2>();
			case RhythmCtrl.DELAY_TOOL_MODE:
				return GetComponent<GamePlayDelayToolModeControlVer2>();
			case RhythmCtrl.TUTORIAL_MODE:
				return GetComponent<GamePlayTutorialModeControl>();
			}
			throw new UnityException ("還沒有實作 "+mode+" mode");
		}

		#region util
		// 傳進來的i若是打擊點的索引，記得要乘2
		// 因為打擊的點數是比節拍數少一半
		public static void Idx2TurnCount(int beatCntPerTurn, int i, ref int turn, ref int count){
			turn = i / beatCntPerTurn;
			count = i % beatCntPerTurn;
		}
		// 傳出來的值若用在打擊點(球)上，記得要除2
		// 因為打擊的點數是比節拍數少一半
		public static int TurnCount2Idx(int beatCntPerTurn, int turn, int count){
			return turn * beatCntPerTurn + count;
		}

		public enum ClickType {
			Pending,
			Single,
			Long
		}
		// 連打計分：當玩家開始連打後，每連打一下為500分，並在最後判斷整段連打成功時，
		// 將剩餘分數加回去。例如，該首歌每個節奏點為30000分，玩家連打15下，中途就會
		// +7500分，連打結束後，一口氣補上該節奏點剩餘的22500；若連打中途失敗則不將剩
		// 餘分數補上。
		public static bool ComputeRhythmCountInLongClickSection(int[][] idxAry, int turn, int count, ref int startCnt, ref int endCnt){
			// 本身就有1個
			var currIdx = idxAry [turn] [count];
			// 目前點為7或8的狀況
			if (currIdx == 7 || currIdx == 8) {
				// 往前計算
				for (var i = count - 1; i >= 0; --i) {
					if (idxAry [turn] [i] == 7) {
						// ignore
					} else {
						// 計算非7或8的第一個連打點
						// 不必多加1
						startCnt = i;
						break;
					}
				}
				// 往後計算
				var i2 = count+1;
				for (; i2 < idxAry [turn].Length; ++i2) {
					if (idxAry [turn] [i2] == 7 || idxAry [turn] [i2] == 8) {
						// ignore
					} else {
						break;
					}
				}
				endCnt = i2 - 1;
				return true;
			} else {
				// 目前點非7或8的狀況則判斷下一個點是不是7或8
				var hasMore = count < idxAry [turn].Length - 1;
				var nextIdx = hasMore ? idxAry [turn] [count+1] : -1;
				if (nextIdx == 7 || nextIdx == 8) {
					// 不必往前計算，因為是第1個
					startCnt = count;
					// 往後計算
					var i = count+1;
					for (; i < idxAry [turn].Length; ++i) {
						if (idxAry [turn] [i] == 7 || idxAry [turn] [i] == 8) {
							// ignore
						} else {
							break;
						}
					}
					endCnt = i - 1;
					return true;
				}
			}
			return false;
		}

		public static void GetRhythmIndex(int[][] idxAry, int[][] mashAry, int turn, int count, ref int idx, ref ClickType t){
			var playIdx = idxAry [turn] [count];
			if (playIdx == 7 || playIdx == 8) {
				idx = mashAry [turn] [count];
			} else {
				idx = playIdx;
			}
			var startCnt = 0;
			var endCnt = 0;
			var isInLongClickSection = Game.ComputeRhythmCountInLongClickSection (idxAry, turn, count, ref startCnt, ref endCnt);
			t = isInLongClickSection ? ClickType.Long : ClickType.Single;
		}

		//  |-|-|->
		// |o|o|o|
		//  0 1 2
		// | | | |
		// 0 1 2 
		public static bool CheckTimeDist(int[][] idxAry, int[][] mashAry, int beatCntPerTurn, float timePerBeat, float timer, ref float checkTime, ref float dist){
			var turn = 0;
			var count = 0f;
			BeatTool.Time2TurnAndCount (beatCntPerTurn, timePerBeat, timer + timePerBeat/2, ref turn, ref count);
			if (count < 0 || turn >= 2) {
				return false;
			}
			checkTime = (turn * beatCntPerTurn + Mathf.Floor(count)) * timePerBeat;
			dist = checkTime - timer;
			return true;
		}

		static bool ComputeDistToBeatCenter_XXX(int[][] idxAry, int[][] meshAry, int currTurn, float currCount, int clickIdx, ref float distToCenter){
			var fixCount = (int)currCount;
			var playIdx = 0;
			var clickType = ClickType.Pending;
			GetRhythmIndex (idxAry, meshAry, currTurn, fixCount, ref playIdx, ref clickType);
			// 連續時5和6都一樣
			var ignoreCheck = 
				clickType == ClickType.Long &&
				(clickIdx == 5 || clickIdx == 6) &&
				(playIdx == 5 || playIdx == 6);
			if (ignoreCheck || playIdx == clickIdx) {
				var floatPart = currCount - Mathf.Floor(currCount);
				distToCenter = Mathf.Abs (0.5f - floatPart);
				return true;
			} else {
				return false;
			}
		}

		public static string ComputeDistToResult(float disToCenter, bool isEasyCheck = true){
			var result = "Bad";
			if (isEasyCheck) {
				if (disToCenter < 0.75f) {
					result = "Perfect";
				} else if (disToCenter < 0.875f) {
					result = "Great";
				} else if (disToCenter < 1f) {
					result = "Good";
				}
			} else {
				if (disToCenter < 0.25f) {
					result = "Perfect";
				} else if (disToCenter < 0.5f) {
					result = "Great";
				} else if (disToCenter < 1f) {
					result = "Good";
				}
			}
			return result;
		}

		// 計算得分
		// 連擊只有在最後一擊才會在這裡算出分數
		// 其它的分數會在GamePlayModel的ClickHint的參數OnClickSuccessRepeating中計算
		// 規則是手指每按一次加500直到整段連打點按完後才把剩下的分數一次加上來
		public static int ComputeScore(IGamePlayModeControl ctrl, int turn, int count, Game.ClickType clickType, string result){
			var model = ctrl.GamePlayModel;
			var isFever = ctrl.IsFeverMode;
			var singleScore = isFever ? RhythmCtrl.FEVER_SCORE : RhythmCtrl.PERFECT_SCORE;
			switch (clickType) {
			case ClickType.Long:
				{
					#if LONGCLICK_OLD_VERSION1
					// 連擊只有在最後一擊才會在這裡算出分數
					int startCnt = 0;
					int endCnt = 0;
					// 取得連打段落
					Game.ComputeRhythmCountInLongClickSection (RhythmCtrl.RHYTHM_IDX_ARRAY, turn, count, ref startCnt, ref endCnt);
					// 0~15代表有16個
					int totalCnt = (endCnt - startCnt)+1;
					// 如果整段打完
					// 可以這樣判斷的原因是如果整段中有一個節奏點打擊失敗就根本不會進來這裡，會一早就被Model.IsLongClick過濾掉
					if (endCnt == count) {
						// 獲得剩下的分數
						var totalScore = totalCnt * singleScore;
						var beforeScore = model.LongClickCount * RhythmCtrl.LONGCLICK_BEFORE_SCORE;
						var remainScore = totalScore - beforeScore;
						return remainScore;
					} else {
						return 0;
					}
					#else
					int startCnt = 0;
					int endCnt = 0;
					// 取得連打段落
					Game.ComputeRhythmCountInLongClickSection (RhythmCtrl.RHYTHM_IDX_ARRAY, turn, count, ref startCnt, ref endCnt);
					// 不是最後一點，加500分
					var isEnd = count == endCnt;
					if(isEnd == false){
						return RhythmCtrl.LONGCLICK_BEFORE_SCORE;
					}
					// 不是開始連擊的同一線段， 加500分
					var isOriginalSection = startCnt == model.LongClickSectionStartCnt;
					if(isOriginalSection == false){
						return RhythmCtrl.LONGCLICK_BEFORE_SCORE;
					}
					// 沒有全部都打擊完成，加500分
					var isAllClickOK = model.IsAllProcessInState(turn, startCnt, endCnt, GamePlayModel.ProcessClickOK);
					if(isAllClickOK == false){
						return RhythmCtrl.LONGCLICK_BEFORE_SCORE;
					}
					// 0~15代表有16個
					int totalCnt = (endCnt - startCnt)+1;
					// 獲得剩下的分數
					var totalScore = totalCnt * singleScore;
					var beforeScore = model.LongClickCountVer3 * RhythmCtrl.LONGCLICK_BEFORE_SCORE;
					var remainScore = totalScore - beforeScore;
					return remainScore;
					#endif
				}
			case ClickType.Single:
				{
					// Perfect佔該點的100%分數、Great為66%、Good為33%。若該節奏點的
					switch (result) {
					case "Perfect":
						return singleScore;
					case "Great":
						return Mathf.CeilToInt (singleScore * 2.0f / 3.0f);
					case "Good":
						return Mathf.CeilToInt (singleScore * 1.0f / 3.0f);
					}
				}
				break;
			}
			return 0;
		}

		public static bool CanEnterFeverStage(float score){
			float scoreScale = score * 100.0f / RhythmCtrl.WARM_ZONE_SCORE;
			return (scoreScale >= RhythmCtrl.FEVER_THRESHOLD);
		}

		// 給StageView的模式
		public static string GameMode2ResourceStr(int mode){
			switch (mode) {
			case RhythmCtrl.DANCING_MODE:
			case RhythmCtrl.DELAY_TOOL_MODE:
			case RhythmCtrl.TUTORIAL_MODE:
				return "P1";
			case RhythmCtrl.FIGHTING_MODE:
				return "P2";
			case RhythmCtrl.PARKOURING_MODE:
				return "P3";
			}
			throw new UnityException ("沒有這個mode:"+mode);
		}

		public static bool HasFeverMode(){
			return RhythmCtrl.FEVER_LENGTH > 0;
		}

		// 判斷是不是在Fever Mode區段
		// 注意：level > RhythmCtrl.MAX_LEVEL 可能僅僅是過關而已
		public static bool IsFeverModeSection(int level){
			return level > RhythmCtrl.MAX_LEVEL;
		}

		public static IEnumerator InvokeStartFeverMode(GamePlayView view, GamePlayModel model){
			view.PauseBGM ();
			view.PlayFeverSound ();
			yield return view.InvokeFeverStart ();
			view.PlayFeverBGM();

			//yield return new WaitForSeconds(RhythmCtrl.HALF_BEAT_TIME*2);
			// 使用MuteAndPlay的原因是要繼續追蹤本來音樂的音樂時間
			view.MuteAndPlayLevelBGM ();

			// 對戰模式要移動場地
			if (view.modeId == RhythmCtrl.FIGHTING_MODE) {
				view.StartCoroutine(view.StageView.MoveStage (StageView.Right));
			}
		}

		public static bool HasMoreLevel(int level){
			if (Game.HasFeverMode ()) {
				if (Game.IsFeverModeSection (level)) {
					return (level+1) <= (RhythmCtrl.MAX_LEVEL + RhythmCtrl.FEVER_LENGTH);
				}
			}
			return (level+1) <= RhythmCtrl.MAX_LEVEL;
		}

		public static void LoadLevel(GamePlayView view, GamePlayModel model, int level){
			// 重設重復打擊標記
			model.InitProcessedData ();
			// 解析關卡
			model.LoadNextLevel (level);
			// 重擺打擊點位置
			view.ResetHintZoneForNextLevel (level);
		}

		public static string HandleOnClickSuccess(IGamePlayModeControl ctrl, int currentLevel, int clickIdx, Game.ClickType clickType, int turn, float count, float factor, GamePlayView.OnHpChange OnHpChange){
			var view = ctrl.GamePlayView;
			var model = ctrl.GamePlayModel;

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
			// 加血
			view.AddHP(RhythmCtrl.ADD_HP, OnHpChange);
			// 加分
			model.AddScore(Game.ComputeScore(ctrl, turn, fixCount, clickType, clickResult));
			view.UpdateScore(model.Score);
			// 打擊點互動
			var hintIdx = Game.TurnCount2Idx (16, turn, fixCount);
			view.HintPlayGood (hintIdx, clickIdx, isPerfect, ctrl.IsFeverMode, clickType);
			// 文字互動
			view.FlashWord (clickResult);
			// 記錄這個打擊點已處理過，這樣就不會發生同一個打擊點打了1次以上
			model.CompleteProcess (turn, fixCount, GamePlayModel.ProcessClickOK);
			return clickResult;
		}

		public static string HandleOnClickFail(IGamePlayModeControl ctrl, int clickIdx, int turn, float count, GamePlayView.OnHpChange OnHpChange){
			var model = ctrl.GamePlayModel;
			var view = ctrl.GamePlayView;

			model.CompleteProcess (turn, (int)count, GamePlayModel.ProcessClickFail);
			view.AddHP (RhythmCtrl.MINUS_SCORE, OnHpChange);
			// 打擊點互動
			var hintIdx = Game.TurnCount2Idx (16, turn, (int)count);
			var isFever = false;
			view.HintPlayMiss (hintIdx, clickIdx, isFever);
			// 文字互動
			var result = Game.ComputeDistToResult (1);
			view.FlashWord (result);
			return result;
		}

		public static void HandleOnClickSuccessRepeating(IGamePlayModeControl ctrl, int clickIdx, Game.ClickType clickType, int turn, float count, float factor){
			var model = ctrl.GamePlayModel;
			var view = ctrl.GamePlayView;
			// 加分
			model.AddScore(Game.ComputeScore(ctrl, turn, (int)count, clickType, "Ignore"));
			view.UpdateScore (model.Score);
			// 文字互動
			view.FlashWord ("Perfect");
		}

		public static void HandleOnBeat(GamePlayView view, GamePlayModel model, float sinceTime, int beat, GamePlayModel.OnClickSuccess OnClickSuccess, GamePlayModel.OnClickFail OnClickFail){
			// 跳出Ready Go 
			if (beat == -6) {
				view.FlashWord ("Ready");
			}
			if (beat == -2) {
				view.FlashWord ("Go");
			}
			#if LONGCLICK_OLD_VERSION1
			// 處理連擊
			// 連擊的動畫觸發時機是跟著旋律並非按下按鈕
			// 所以要在這處理
			model.HandleLongClickIfLongClickIsTrueVer2(sinceTime, OnClickSuccess, OnClickFail);
			#endif
		}

		static Dictionary<string,int> cmd2playIdx = new Dictionary<string,int> {
			{"Btn01",1},
			{"Btn02",2},
			{"Btn03",3},
			{"Btn04",4},
			{"Btn05-1",5},
			{"Btn05-2",6},
			{"Hold",7},
			{"Stop",0}
		};
		public static bool HandleOnGameButtonClick(GamePlayView view, GamePlayModel model, float sinceTime, int currentLevel, string command, GamePlayModel.OnClickSuccess OnClickSuccess, GamePlayModel.OnClickFail OnClickFail, GamePlayModel.OnClickSuccess OnClickSuccessRepeating){
			if (cmd2playIdx.ContainsKey (command)) {
				var clickIdx = cmd2playIdx [command];
				#if LONGCLICK_OLD_VERSION1
				model.ClickHintVer2 (clickIdx, sinceTime, OnClickSuccess, OnClickFail, OnClickSuccessRepeating);
				#else
				model.ClickHintVer3 (clickIdx, sinceTime, OnClickSuccess, OnClickFail, OnClickSuccessRepeating);
				#endif
				view.ClickHint (clickIdx, Game.IsFeverModeSection (currentLevel));
				return true;
			}
			return false;
		}

		// 注意：return可能為null
		public static string HandleOnTouchBeatEnd(GamePlayView view, GamePlayModel model, int currentLevel, int turn, int count, GamePlayView.OnHpChange OnHpChange){
			if (model.HasProcessed (turn, count) == false) {
				var result = Game.ComputeDistToResult (1);
				view.FlashWord (result);
				view.AddHP (RhythmCtrl.MINUS_SCORE, OnHpChange);
				// 打擊點互動
				var clickIdx = RhythmCtrl.RHYTHM_IDX_ARRAY[turn][count];
				var hintIdx = Game.TurnCount2Idx (16, turn, count);
				view.HintPlayMiss (hintIdx, clickIdx, Game.IsFeverModeSection(currentLevel));
				return result;
			}
			return null;
		}

		public delegate void OnLevelClear (bool isWin, int currentLevel);
		public delegate void OnStartFeverMode ();

		// 統一處理Level結束
		// 可以在BeatManager.OnLevelEnd時呼叫
		// 傳入的OnLevelClear呼叫時currentLevel一定剛好都被加1
		// 其它的狀況就是OnStartFeverMode，這時currentLevel會跳到FeverMode的區段
		public static int HandleLevelEnd (GamePlayView view, GamePlayModel model, int currentLevel, OnLevelClear OnLevelClear, OnStartFeverMode OnStartFeverMode, Func<int, bool> QueryAutoLoadNext = null){
			var prevLevel = currentLevel;
			var willGoLevel = currentLevel + 1;
			// 先判斷這關有沒有Fever Mode
			if (Game.HasFeverMode ()) {
				// 如果結束前的關卡處於Fever Mode
				if (Game.IsFeverModeSection (prevLevel)) {
					// 判斷是不是跑完所有關卡
					// Fever Mode的關卡判斷和Normal Mode不一樣
					if (willGoLevel > RhythmCtrl.MAX_LEVEL + RhythmCtrl.FEVER_LENGTH) {
						var fullHp = 100;
						float finalHpScale = view.GetHP() * 100.0f / fullHp;
						bool isWin = (finalHpScale >= RhythmCtrl.LEVEL_THRESHOLD);
						if (OnLevelClear != null) {
							OnLevelClear (isWin, willGoLevel);
						}
						return willGoLevel;
					}
				} else {
					var enterFeverMode = false;
					// 如果結束的關卡是判斷為Fever Mode的關卡
					if (prevLevel == RhythmCtrl.FEVER_START_LEVEL) {
						// 判斷是否進入Fever Mode
						if (Game.CanEnterFeverStage (model.Score)) {
							// 進入到Fever Mode的關卡並讓IsFeverMode成立
							willGoLevel = RhythmCtrl.MAX_LEVEL + 1;
							// 進入Fever Mode的當下不要判斷是否過關
							enterFeverMode = true;
							if (OnStartFeverMode != null) {
								OnStartFeverMode ();
							}
						}
					}
					// 沒進入Fever Mode，正常判斷過關
					if (enterFeverMode == false && willGoLevel > RhythmCtrl.MAX_LEVEL) {
						var fullHp = 100;
						float finalHpScale = view.GetHP() * 100.0f / fullHp;
						bool isWin = (finalHpScale >= RhythmCtrl.LEVEL_THRESHOLD);
						if (OnLevelClear != null) {
							OnLevelClear (isWin, willGoLevel);
						}
						return willGoLevel;
					}
				}
			} else {
				// 這關沒有Fever Mode，正常判斷過關
				if (willGoLevel > RhythmCtrl.MAX_LEVEL) {
					var fullHp = 100;
					float finalHpScale = view.GetHP() * 100.0f / fullHp;
					bool isWin = (finalHpScale >= RhythmCtrl.LEVEL_THRESHOLD);
					if (OnLevelClear != null) {
						OnLevelClear (isWin, willGoLevel);
					}
					return willGoLevel;
				}
			}
			if (QueryAutoLoadNext == null) {
				LoadLevel (view, model, willGoLevel);
			} else {
				if (QueryAutoLoadNext (willGoLevel)) {
					LoadLevel (view, model, willGoLevel);
				}
			}
			return willGoLevel;
		}

		public delegate void OnTouchBeatEnd(int turn, int count);

		public static void ComputeTouchBeatEndVer2(float sinceTime, OnTouchBeatEnd OnTouchBeatEnd){
			var turn = 0;
			var countf = 0f;
			var beatCntPerTurn = 16;
			BeatTool.Time2TurnAndCount (beatCntPerTurn, RhythmCtrl.HALF_BEAT_TIME, sinceTime + RhythmCtrl.HALF_BEAT_TIME / 2, ref turn, ref countf);

			// 計算拍數結尾
			var lastCount = (int)Mathf.Floor(countf) - 1;
			var lastTurn = turn;
			// 如果在第0拍(減了1)的位置
			// 代表是上1Turn的最後一個Count
			if (lastCount == -1) {
				lastTurn = lastTurn - 1;
				lastCount = beatCntPerTurn - 1;
			}
			// 處理失誤
			if (lastTurn >= 0) {
				OnTouchBeatEnd (lastTurn, (int)lastCount);
			}
		}

		public static void ParseLevelRhythm (string[][] dataAry_, int currentLevel, ref int[][] idxAry, ref char[][] typeAry, ref int[][] mashAry)
		{
			int index = (currentLevel - 1) * 3 + 1;
			string[] mainStrArray = dataAry_ [index];
			int length = mainStrArray.Length;

			idxAry = new int[2][];
			idxAry [0] = new int[length];
			idxAry [1] = new int[length];
			typeAry = new char[2][];
			typeAry [0] = new char[length];
			typeAry [1] = new char[length];
			mashAry = new int[2][];
			mashAry [0] = new int[length];
			mashAry [1] = new int[length];

			for (int t = 0; t < 2; ++t) {
				var dataAry = dataAry_ [index + t];
				for (int i = 0; i < length; ++i) {
					/*
					char[] rhythmArray = dataAry [i].ToCharArray ();
					// rhythmArray的最後1個一定是數字
					// 不然就是格式錯誤
					int rhythm = int.Parse (rhythmArray [rhythmArray.Length - 1].ToString ());
					idxAry [t] [i] = rhythm;
					// 略過包含換行字元的(ascii code = 10)
					// 通常是每行的第一個字元
					if (Convert.ToInt32 (rhythmArray [0]) == 10) {
						// 有換行字元的長度為3
						// 所以剛好取rhythmArray[1]為Type
						typeAry [t] [i] = (rhythmArray.Length == 2) ? '\0' : rhythmArray [1];
					} else {
						// 沒有換行字元的長度為2
						// 所以剛好取rhythmArray[0]為Type
						typeAry [t] [i] = (rhythmArray.Length == 1) ? '\0' : rhythmArray [0];
					}
					// 這應該是處理長按的功能
					if (rhythm == 7) {
						if (i > 0 && mashAry [t] [i - 1] == 0) {
							mashAry [t] [i - 1] = idxAry [t] [i - 1];
						}
						mashAry [t] [i] = mashAry [t] [i - 1];
					} else if (rhythm == 8) {
						mashAry [t] [i] = mashAry [t] [i - 1];
					} else {
						mashAry [t] [i] = 0;
					}
					*/
					var flag = dataAry [i].Trim();
					var isHide = false;
					var rhythmType = '\0';
					var rhythm = 0;
					foreach(var f in flag){
						if (f == '-') {
							isHide = true;
						} else if (f >= 'A' && f <= 'Z') {
							rhythmType = f;
						} else if (f >= '0' && f <= '9') {
							rhythm = int.Parse (f.ToString ());
						}
					}
					if (isHide == false) {
						idxAry [t] [i] = rhythm;
						typeAry [t] [i] = rhythmType;
						// 這應該是處理長按的功能
						if (rhythm == 7) {
							if (i > 0 && mashAry [t] [i - 1] == 0) {
								mashAry [t] [i - 1] = idxAry [t] [i - 1];
							}
							mashAry [t] [i] = mashAry [t] [i - 1];
						} else if (rhythm == 8) {
							mashAry [t] [i] = mashAry [t] [i - 1];
						} else {
							mashAry [t] [i] = 0;
						}
					}
				}
			}
		}

		public static IEnumerator HandleLevelClear(GamePlayView view, GamePlayModel model, bool isWin, bool isFeverMode){
			//model.ChangeState (GamePlayModel.State.End);
			yield return Game.InvokeHandleLevelClear (view, model, isWin, isFeverMode);
		}

		public static IEnumerator InvokeHandleLevelClear(GamePlayView view, GamePlayModel model, bool isWin, bool isFeverMode){
			// 讓Hint消失到看不見的地方
			view.SyncTimer (99999);
			// 血量為0的失敗不要延遲
			var shouldNotDelay = view.GetHP() <= 0;
			// 同時播動畫和音樂
			yield return view.InvokeLevelClear (shouldNotDelay ? 0 : 2, isWin, isFeverMode, false);
			// 等待成功音樂播的差不多時
			yield return new WaitForSeconds (8);
			// 跳回地圖頁
			Game.BackToMap ();
		}

		static public void BackToMap(){
			UIEventFacade.OnGameEvent.OnNext ("GamePlayUIGameEnd");
		}
		#endregion

		public enum DancingModeFlowState{
			Pending,
			CountDown,
			EnemyPerformance,
			PlayerInput,
			Rest,
			FeverRest,
			FeverEnemyPerformance,
			FeverPlayerInput,
			End
		}

		public static void ComputeDancingModeState(float timerPer4Beat, int levelCount, int feverLength, bool isFever, float timer, ref DancingModeFlowState state, ref float sinceTime){
			var offset = 0f;
			offset += timerPer4Beat * 4;
			if (offset > timer) {
				state = DancingModeFlowState.CountDown;
				sinceTime = offset - timerPer4Beat * 4;
				return;
			}
			for (var i = 0; i < levelCount; ++i) {
				offset += timerPer4Beat * 8;
				if (offset > timer) {
					state = DancingModeFlowState.EnemyPerformance;
					sinceTime = offset - timerPer4Beat * 8;
					return;
				}
				offset += timerPer4Beat * 8;
				if (offset > timer) {
					state = DancingModeFlowState.PlayerInput;
					sinceTime = offset - timerPer4Beat * 8;
					return;
				}
				// 沒有feverMode的情況，這裡就會到結束，要處理最後一個level
				if (feverLength == 0) {
					// 最後一個level沒有休息時間
					if (i == levelCount - 1) {
						break;
					}
				}
				offset += timerPer4Beat * 8;
				if (offset > timer) {
					state = DancingModeFlowState.Rest;
					sinceTime = offset - timerPer4Beat * 8;
					return;
				}
			}
			if (isFever == false) {
				for (var i = levelCount; i < levelCount + feverLength; ++i) {
					offset += timerPer4Beat * 8;
					if (offset > timer) {
						state = DancingModeFlowState.EnemyPerformance;
						sinceTime = offset - timerPer4Beat * 8;
						return;
					}
					offset += timerPer4Beat * 8;
					if (offset > timer) {
						state = DancingModeFlowState.PlayerInput;
						sinceTime = offset - timerPer4Beat * 8;
						return;
					}
					// 最後一個level沒有休息時間
					if (i == levelCount + feverLength - 1) {
						break;
					}
					offset += timerPer4Beat * 8;
					if (offset > timer) {
						state = DancingModeFlowState.Rest;
						sinceTime = offset - timerPer4Beat * 8;
						return;
					}
				}
			} else {
				for (var i = 0; i < feverLength; ++i) {
					offset += timerPer4Beat * 8;
					if (offset > timer) {
						state = DancingModeFlowState.FeverEnemyPerformance;
						sinceTime = offset - timerPer4Beat * 8;
						return;
					}
					offset += timerPer4Beat * 8;
					if (offset > timer) {
						state = DancingModeFlowState.FeverPlayerInput;
						sinceTime = offset - timerPer4Beat * 8;
						return;
					}
					// 最後一個level沒有休息時間
					if (i == feverLength - 1) {
						break;
					}
					offset += timerPer4Beat * 8;
					if (offset > timer) {
						state = DancingModeFlowState.FeverRest;
						sinceTime = offset - timerPer4Beat * 8;
						return;
					}
				}
			}
			state = DancingModeFlowState.End;
		}

		public enum FightingModeFlowState{
			Pending,
			CountDown,
			PlayerInput,
			PlayerAttack,
			EnemyAttack,
			FeverRest,
			FeverPlayerInput,
			End
		}

		public static void ComputeFightingModeState(float timerPer4Beat, int levelCount, int feverLength, bool isFever, float timer, ref FightingModeFlowState state, ref float sinceTime){
			var offset = 0f;
			offset += timerPer4Beat * 8;
			if (offset > timer) {
				state = FightingModeFlowState.CountDown;
				sinceTime = offset - timerPer4Beat * 8;
				return;
			}
			for (var i = 0; i < levelCount; ++i) {
				offset += timerPer4Beat * 8;
				if (offset > timer) {
					state = FightingModeFlowState.PlayerInput;
					sinceTime = offset - timerPer4Beat * 8;
					return;
				}
				offset += timerPer4Beat * 8;
				if (offset > timer) {
					state = i % 2 == 0 ? FightingModeFlowState.EnemyAttack : FightingModeFlowState.PlayerAttack;
					sinceTime = offset - timerPer4Beat * 8;
					return;
				}
			}
			if (isFever == false) {
				for (var i = levelCount; i < levelCount + feverLength; ++i) {
					offset += timerPer4Beat * 8;
					if (offset > timer) {
						state = FightingModeFlowState.PlayerInput;
						sinceTime = offset - timerPer4Beat * 8;
						return;
					}
					offset += timerPer4Beat * 8;
					if (offset > timer) {
						state = i % 2 == 0 ? FightingModeFlowState.EnemyAttack : FightingModeFlowState.PlayerAttack;
						sinceTime = offset - timerPer4Beat * 8;
						return;
					}
				}
			} else {
				for (var i = 0; i < feverLength; ++i) {
					offset += timerPer4Beat * 8;
					if (offset > timer) {
						state = FightingModeFlowState.FeverRest;
						sinceTime = offset - timerPer4Beat * 8;
						return;
					}
					offset += timerPer4Beat * 8;
					if (offset > timer) {
						state = FightingModeFlowState.FeverPlayerInput;
						sinceTime = offset - timerPer4Beat * 8;
						return;
					}
				}
			}
			state = FightingModeFlowState.End;
		}

		public enum ParkouringModeFlowState{
			Pending,
			CountDown,
			PlayerInputOdd,
			PlayerInputEven,
			FeverRest,
			FeverPlayerInputOdd,
			FeverPlayerInputEven,
			End
		}

		public static void ComputeParkouringModeState(float timerPer4Beat, int levelCount, int feverLength, bool isFever, float timer, ref ParkouringModeFlowState state, ref float sinceTime){
			var offset = 0f;
			offset += timerPer4Beat * 8;
			if (offset > timer) {
				state = ParkouringModeFlowState.CountDown;
				sinceTime = offset - timerPer4Beat * 8;
				return;
			}
			for (var i = 0; i < levelCount; ++i) {
				offset += timerPer4Beat * 8;
				if (offset > timer) {
					state = i % 2 == 0 ? ParkouringModeFlowState.PlayerInputEven : ParkouringModeFlowState.PlayerInputOdd;
					sinceTime = offset - timerPer4Beat * 8;
					return;
				}
			}
			if (isFever == false) {
				for (var i = levelCount; i < levelCount + feverLength; ++i) {
					offset += timerPer4Beat * 8;
					if (offset > timer) {
						state = i % 2 == 0 ? ParkouringModeFlowState.PlayerInputEven : ParkouringModeFlowState.PlayerInputOdd;
						sinceTime = offset - timerPer4Beat * 8;
						return;
					}
				}
			} else {
				offset += timerPer4Beat * 8;
				if (offset > timer) {
					state = ParkouringModeFlowState.FeverRest;
					sinceTime = offset - timerPer4Beat * 8;
					return;
				}
				for (var i = 0; i < feverLength; ++i) {
					offset += timerPer4Beat * 8;
					if (offset > timer) {
						state = i % 2 == 0 ? ParkouringModeFlowState.FeverPlayerInputEven : ParkouringModeFlowState.FeverPlayerInputOdd;
						sinceTime = offset - timerPer4Beat * 8;
						return;
					}
				}
			}
			state = ParkouringModeFlowState.End;
		}

	}
}

