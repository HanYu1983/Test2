using System;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Remix
{
	public class GamePlayView : MonoBehaviour
	{
		public HintZone hintZoneOrigin;
		public ObstacleView obstacleView;
		public ButtonZone buttonZone;
		public WordZoneCtrl wordZone;
		public HpBarUICtrl hpBarUI;
		public GameObject catIconAnchor;
		public PauseZoneCtrl pauseZone;
		public DataZoneCtrl dataZone;
		public DialogCtrl replayDlg;
		public AudioClip windSound, failSound, useItemSound;
		public TrainingUI trainingUI;

		public StageView stageView;
		public IHintZone hintZone;
		public int modeId;
		public int leftCatIdx;
		public float animationTimeScale = 1;
		// for delay tool
		public Text textExplan;

		public string TextExplan{
			set{
				if (textExplan == null) {
					return;
				}
				textExplan.text = value;
			}
		}

		LevelSoundDataCtrl levelSoundData;
		SoundDataCtrl leftSound, rightSound;

		public SoundDataCtrl LeftSound{ get { return leftSound; } }
		public SoundDataCtrl RightSound{ get { return rightSound; } }
		public LevelSoundDataCtrl LevelSoundData{ get { return levelSoundData; } }

		public StageView StageView{ get{ return stageView; } }
		public int LeftCatIdx{ get { return leftCatIdx; } }

		public delegate void OnPlayCatAnimAfter(int pos, int animIdx, bool triggerWithSuccess);

		public void InitView(LevelKey levelKey, int leftCatIdx, int rightCatIdx, int modeId, int level){
			this.leftCatIdx = leftCatIdx;
			this.modeId = modeId;
			var composeHintZone = GetComponent<ComposeHintZone> ();
			// 初始化一般打擊點
			hintZoneOrigin.TimePerBeat = RhythmCtrl.HALF_BEAT_TIME;
			hintZoneOrigin.BeatCntPerTurn = 16;
			hintZoneOrigin.TurnCntPerLevel = 2;
			hintZoneOrigin.HintWidth = RhythmCtrl.HINT_WIDTH;
			composeHintZone.hintZones.Add (hintZoneOrigin);
			// 如果是跑酷模式要另外加入場景上的障礙物
			if (modeId == RhythmCtrl.PARKOURING_MODE) {
				obstacleView.mapIdx = levelKey.MapIdx;
				obstacleView.catIdx = leftCatIdx;
				obstacleView.TimePerBeat = RhythmCtrl.HALF_BEAT_TIME;
				obstacleView.BeatCntPerTurn = 16;
				obstacleView.TurnCntPerLevel = 2;
				// 本來的Sprite是做在Canvas中用的(2D座標用/單位像素)
				// 現在變成直接在3D空間顯示，所以寬度要做調整
				obstacleView.HintWidth = RhythmCtrl.HINT_WIDTH * 2;
				composeHintZone.hintZones.Add (obstacleView);
			}
			// 統一使用composeHintZone來操做所有和類打擊點的物件
			hintZone = composeHintZone;
			hintZone.InitHintZone ();
				
			stageView = GetComponent<StageView> ();
			stageView.leftCatIdx = leftCatIdx;
			stageView.rightCatIdx = rightCatIdx;
			stageView.mode = Game.GameMode2ResourceStr (modeId);;
			stageView.InitComponents ();
			stageView.LoadBackground (levelKey.MapIdx);
			stageView.TimeScale = animationTimeScale;
			stageView.ResetStage ();
			stageView.ChangeAnimation (stageView.LeftCat, StageView.P1_DEFAULT_IDX, true);
			stageView.ChangeAnimation (stageView.RightCat, StageView.P1_DEFAULT_IDX, true);

			var leftCatKey = new ItemKey (StoreCtrl.DATA_CAT, leftCatIdx);
			var rightCatKey = new ItemKey (StoreCtrl.DATA_CAT, rightCatIdx);
			if (catIconAnchor != null) {
				var prefab = leftCatKey.CatIconPrefabName("CI", GameConfig.CAT_STATE_ID.STATE_IDLE);
				Util.Instance.GetPrefab (prefab, catIconAnchor);
			} else {
				Debug.LogWarning ("你沒有設定貓Icon的錨點。無法顯示貓Icon");
			}

			var soundObj = Util.Instance.GetPrefab(leftCatKey.CatSoundPrefab, this.gameObject);
			leftSound = soundObj.GetComponent<SoundDataCtrl>();

			soundObj = Util.Instance.GetPrefab(rightCatKey.CatSoundPrefab, this.gameObject);
			rightSound = soundObj.GetComponent<SoundDataCtrl>();

			GameObject levelSoundObj = Util.Instance.GetPrefab(levelKey.LevelSoundPrefabName(RhythmCtrl.LEVEL_BGM), null);
			levelSoundData = levelSoundObj.GetComponent<LevelSoundDataCtrl>();

			// 全部加在同一個樹裡，隨著根被刪除而一並刪除
			leftSound.transform.SetParent (this.transform);
			rightSound.transform.SetParent (this.transform);
			levelSoundObj.transform.SetParent (this.transform);
		}

		// 先包裝起來，還沒有用到
		public bool IsWin{
			get{
				float finalHpScale = GetHP() / (float)GetFullHP();
				bool isWin = (finalHpScale >= RhythmCtrl.LEVEL_THRESHOLD);
				return isWin;
			}
		}

		public void ClickHint(int clickIdx, bool isFever){
			switch (clickIdx) {
			case 1:
			case 2:
			case 3:
			case 4:
				{
					var btn = buttonZone.GetButtonView (clickIdx - 1);
					btn.Flash (isFever);
				}
				break;
			case 5:
			case 6:
				{
					var btn = buttonZone.GetButtonView (4);
					btn.Flash (isFever);
				}
				break;
			}
		}

		public void SetViewPause(bool pause, bool isFeverModeSection){
			if (pause == false){
				wordZone.HideWord("Pause");
			}else{
				wordZone.ShowWord("Pause");
			}
			pauseZone.SetPause (pause);
			stageView.TimeScale = pause ? 0 : animationTimeScale;
			if (pause) {
				// 注意：不管是不是在FeverMode，原音樂都要暫停或播放
				// 因為遊戲時間是用主音樂的播放時間同步的
				if (isFeverModeSection) {
					levelSoundData.PauseFeverBGM ();
				}
				levelSoundData.PauseLevelBGM ();
			} else {
				// 注意：不管是不是在FeverMode，原音樂都要暫停或播放
				// 因為遊戲時間是用主音樂的播放時間同步的
				if (isFeverModeSection) {
					levelSoundData.PlayFeverBGM ();
					// Fever中主音樂要Mute
					levelSoundData.MuteAndPlayLevelBGM ();
				} else {
					levelSoundData.PlayLevelBGM ();
				}
			}
			buttonZone.ClickButtonVisible = pause == false;
		}

		public void UpdateScore(int score){
			if (wordZone == null) {
				Debug.LogWarning ("沒有wordZone，無法Show文字");
				return;
			}
			wordZone.UpdateScore(score);
		}

		public void UpdateCatAnimation(){
			stageView.SyncCatTransform ();
		}

		public void ResetHintZoneForNextLevel(int level){
			// 跑酷模式要顯示假的下一關的打擊點
			if (modeId == RhythmCtrl.PARKOURING_MODE || modeId == RhythmCtrl.DELAY_TOOL_MODE) {
				// 如果還有下一關
				if (Game.HasMoreLevel (level)) {
					int[][] finalIdxAry = new int[3][];
					int[][] finalMashAry = new int[3][];
					// 解析下一關的打擊資訊
					int[][] idxAry = null;
					char[][] typeAry = null;
					int[][] mashAry = null;
					Game.ParseLevelRhythm (RhythmCtrl.RHYTHM_DATA_ARRAY, level + 1, ref idxAry, ref typeAry, ref mashAry);
					// 合併關卡資料
					finalIdxAry [0] = RhythmCtrl.RHYTHM_IDX_ARRAY [0];
					finalIdxAry [1] = RhythmCtrl.RHYTHM_IDX_ARRAY [1];
					finalIdxAry [2] = idxAry [0];

					finalMashAry [0] = RhythmCtrl.RHYTHM_BTNMASH_ARRAY [0];
					finalMashAry [1] = RhythmCtrl.RHYTHM_BTNMASH_ARRAY [1];
					finalMashAry [2] = mashAry [0];
					// 設定
					hintZone.InitHintSprite (finalIdxAry, finalMashAry);
				} else {
					// 沒有下一關，做通常設定就行了
					hintZone.InitHintSprite (RhythmCtrl.RHYTHM_IDX_ARRAY, RhythmCtrl.RHYTHM_BTNMASH_ARRAY);
				}
			} else {
				// 通常設定
				hintZone.InitHintSprite (RhythmCtrl.RHYTHM_IDX_ARRAY, RhythmCtrl.RHYTHM_BTNMASH_ARRAY);
			}
			hintZone.ArrangePos (0);
		}

		public const int LeftPos = 0;
		public const int RightPos = 1;

		public void PlayCatAnim(int pos, int animIdx, int afterAnimIdx, bool triggerWithSuccess, OnPlayCatAnimAfter OnPlayCatAnimAfter){
			switch (pos) {
			case RightPos:
				foreach (var cat in stageView.RightCats) {
					StartCoroutine (stageView.InvokeCatAnim (cat, animIdx, afterAnimIdx));
				}
				break;
			case LeftPos:
				foreach (var cat in stageView.LeftCats) {
					StartCoroutine(stageView.InvokeCatAnim (cat, animIdx, afterAnimIdx));
				}
				break;
			}
			if (OnPlayCatAnimAfter != null) {
				OnPlayCatAnimAfter (pos, animIdx, triggerWithSuccess);
			}
		}

		public void PlayCatSound(int pos, int soundIdx, char soundType = '\0'){
			if (pos == LeftPos) {
				leftSound.PlaySound (soundIdx, soundType);
			} else {
				rightSound.PlaySound (soundIdx, soundType);
			}
		}

		public void ShowLevelClear(bool isWin, bool isFeverMode){
			if (isFeverMode) {
				stageView.StepFever ();
			} else {
				stageView.StopStage ();
			}
			if (wordZone != null) {
				var showWord = isWin ? "Clear" : "Fail";
				wordZone.FlashWord (showWord);
			}
			if (modeId == RhythmCtrl.DANCING_MODE) {
				// dancing mode屬於同樂模式
				if (isWin) {
					foreach (var cat in stageView.LeftCats) {
						stageView.ChangeAnimation (cat, StageView.P1_GOOD_IDX, true);
					}
					foreach (var cat in stageView.RightCats) {
						stageView.ChangeAnimation (cat, StageView.P1_GOOD_IDX, true);
					}
				} else {
					foreach (var cat in stageView.LeftCats) {
						stageView.ChangeAnimation (cat, StageView.P1_BAD_IDX, true);
					}
					foreach (var cat in stageView.RightCats) {
						stageView.ChangeAnimation (cat, StageView.P1_BAD_IDX, true);
					}
				}
			} else if (modeId == RhythmCtrl.PARKOURING_MODE) {
				// 跑酷只有左貓
				if (isWin) {
					stageView.ChangeAnimation (stageView.LeftCat, StageView.P3_GOOD_IDX, true);
				} else {
					// 跑酷的輸掉動畫不輪播，比如胖貓
					stageView.ChangeAnimation (stageView.LeftCat, StageView.P3_BAD_IDX, false);
				}
			} else {
				if (isWin) {
					stageView.ChangeAnimation (stageView.LeftCat, StageView.P2_GOOD_IDX, true);
					stageView.ChangeAnimation (stageView.RightCat, StageView.P2_BAD_IDX, true);
				} else {
					stageView.ChangeAnimation (stageView.LeftCat, StageView.P2_BAD_IDX, true);
					stageView.ChangeAnimation (stageView.RightCat, StageView.P2_GOOD_IDX, true);
				}
			}
		}

		public void ShowRetryDlg(){
			replayDlg.gameObject.SetActive(true);
			replayDlg.InitialDialog(this.gameObject, "Try again");
		}

		public void HideRetryDlg(){
			replayDlg.gameObject.SetActive(false);
		}

		public void FlashWord(string showWord){
			if (wordZone == null) {
				Debug.LogWarning ("沒有設定wordZone。無法顯示文字");
				return;
			}
			wordZone.FlashWord(showWord);
		}

		public void ResetHp(int lastHp, int fullHp){
			// InitialHp要先叫用，AddHp才會有作用
			hpBarUI.InitialHp(lastHp, fullHp);
		}

		public IEnumerator InvokeLevelClear(float delay, bool isWin, bool isFeverMode, bool showRetryDlg){
			yield return new WaitForSeconds (delay);
			ShowLevelClear (isWin, isFeverMode);
			if (isWin) {
				if (windSound == null) {
					Debug.LogWarning ("沒有設定勝利音樂");
				} else {
					UIEventFacade.OnAudioClipRequest.OnNext (new UIEventFacade.AudioClipRequest () {
						Clip = windSound
					});
				}
			} else {
				if (failSound == null) {
					Debug.LogWarning ("沒有設定失敗音樂");
				} else {
					UIEventFacade.OnAudioClipRequest.OnNext (new UIEventFacade.AudioClipRequest () {
						Clip = failSound
					});
				}
				if (showRetryDlg) {
					ShowRetryDlg ();
				}
			}
		}

		public void MuteAndPlayLevelBGM(){
			levelSoundData.MuteAndPlayLevelBGM ();
		}

		public void PlayBGM(){
			levelSoundData.PlayLevelBGM();
		}

		public void PauseBGM(){
			levelSoundData.PauseLevelBGM ();
		}

		public delegate void OnHpChange(int oldHp, int newHp);

		public void AddHP(int addHp, OnHpChange OnHpChange){
			if (hpBarUI == null) {
				return;
			}
			var oldHp = hpBarUI.GetHP ();
			hpBarUI.AddHP (addHp);
			var newHp = hpBarUI.GetHP ();
			if (OnHpChange != null) {
				OnHpChange (oldHp, newHp);
			}
		}

		public IEnumerator ShiningHint(){
			return hintZoneOrigin.ShiningHint ();
		}

		public void StepMoveStage(){
			stageView.StepMoveStage ();
		}

		public IEnumerator InvokeFeverStart(){
			return stageView.InvokeFeverStart ();
		}

		public void PlayFeverSound(){
			leftSound.PlaySound (SoundDataCtrl.FEVER_SOUND);
		}

		public void PlayFeverBGM(){
			levelSoundData.PlayFeverBGM();
		}

		public float GetHP(){
			if (hpBarUI == null) {
				Debug.LogWarning ("沒有設定hpBarUI，等於沒有Hp。一律回傳100");
				return 100;
			}
			return hpBarUI.GetHP ();
		}

		public float GetFullHP(){
			return hpBarUI.GetFullHP ();
		}

		public void HintPlayGood(int hintIdx, int clickIdx, bool isPerfect, bool isFever, Game.ClickType clickType){
			hintZone.HintPlayGood (hintIdx, clickIdx, isPerfect, isFever, clickType);
		}

		public void HintPlayMiss(int hintIdx, int clickIdx, bool isFever){
			hintZone.HintPlayMiss (hintIdx, clickIdx, isFever);
		}

		public void ShowCombo(int cnt){
			wordZone.ShowCombo (cnt);
		}

		public void SyncTimer(float timer){
			hintZone.SyncTimer (timer);
		}

		#region load state
		public string loadState;
		public string LoadState{ get { return loadState; } }
		// 等待音樂讀取完成
		// TODO 錯誤處理
		public IEnumerator WaitForLoaded(){
			yield return new WaitUntil (()=>{
				var fail = levelSoundData.levelBGM.loadState == AudioDataLoadState.Failed;
				if(levelSoundData.feverBGM != null){
					fail |= levelSoundData.feverBGM.loadState == AudioDataLoadState.Failed;
				}
				if(fail){
					loadState = "Failed";
					return true;
				}
				var ok = levelSoundData.levelBGM.loadState == AudioDataLoadState.Loaded;
				if(levelSoundData.feverBGM !=null){
					ok &= levelSoundData.feverBGM.loadState == AudioDataLoadState.Loaded;
				}
				if(ok){
					loadState = "Loaded";
					return true;
				}
				return false;
			});
		}
		#endregion

		#region for delay tool ui
		public Text offsetTimeText;
		public void SetOffsetTime(float t){
			if (offsetTimeText == null) {
				Debug.LogWarning ("沒有設定offsetTimeText");
				return;
			}
			offsetTimeText.text = string.Format("{0:0.00}", t);
		}
		#endregion


		#region level item
		public GameObject levelItem;
		public float durationSeconds = 1f;
		public Sprite LevelItem{
			set{
				var img = levelItem.GetComponent<Image> ();
				if (img == null) {
					Util.Instance.LogWarning ("沒有設定LevelItem.Image");
					return;
				}
				img.sprite = value;
				levelItem.SetActive (true);
			}
		}
		public void AnimateUseLevelItem(){
			var animator = levelItem.GetComponent<Animator>();
			if (animator == null) {
				Util.Instance.LogWarning ("沒有設定LevelItem.Animator");
				return;
			}
			animator.SetTrigger("use");
			// 防止動畫一直loop，在動畫結束前將item消失
			// 不知為何為1秒
			StartCoroutine (HideItem (durationSeconds));
			PlayUseItemSound ();
		}
			
		IEnumerator HideItem(float delay){
			yield return new WaitForSeconds (delay);
			levelItem.SetActive (false);
		}

		public void PlayUseItemSound(){
			if (useItemSound == null) {
				Util.Instance.LogWarning ("沒有設定useItemSound");
				return;
			}
			UIEventFacade.OnAudioClipRequest.OnNext (new UIEventFacade.AudioClipRequest () {
				Clip = useItemSound,
				Type = UIEventFacade.AudioClipRequest.TypeNormalSound
			});
		}
		#endregion
	}
}

