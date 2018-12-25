//
// 請使用DEMO1來建置DEMO版本
// 
using System;
using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using TouchScript.Gestures;
using System.Linq;
using UnityEngine.Networking;
using System.Text;
using LitJson;
using System.IO;
using System.Text.RegularExpressions;

namespace Remix
{
	public partial class Main : MonoBehaviour
	{
		public Canvas canvas;
		public View view;
		public SoundBuffer musicBuffer;
		[Tooltip("遊戲中音效的buffer。當中設定了FadeOut")]
		public SoundBuffer soundBuffer;
		[Tooltip("一般音效用的buffer。沒有設定FadeOut")]
		public SoundBuffer normalSoundBuffer;
		public List<AudioClip> clips;
		public Sessions sessions;
		public HandleTouchCatEvent handleTouchCat;
		public HandleScreenSize handleScreenSize;
		public HandleAudioSyncTimerSmooth handleAudioSyncTimerSmooth;
		public HandleMailEvent handleMailEvent;
		public HandleAnalytics handleAna;
		public HandleIAP handleIAP;
		public HandleGash handleGash;
		public HandleDebug handleDebug;
		public HandleAssetBundle handleAssetBundle;
		public HandlePhoto handlePhoto;
		public HandleBGM handleBGM;
		public HandleGameBGM handleGameBGM;
		public HandleMp3Player handleMp3Player;
		public UserSettings userSettings;
		public DeviceData deviceData;
		public Camera useCamera;
		public LanguageText langText;

		IModel model;

		void Start(){
			handleDebug.Log ("Start");
			SetupApp ();
		}

		void Update(){
			Step ();
		}

		void CreateNeedDir(){
			if (Directory.Exists (GameConfig.LOCAL_ASSET_BUNDELS_PATH) == false) {
				Directory.CreateDirectory (GameConfig.LOCAL_ASSET_BUNDELS_PATH);
			}
			if (Directory.Exists (GameConfig.LOCAL_PHOTOS_PATH) == false) {
				Directory.CreateDirectory (GameConfig.LOCAL_PHOTOS_PATH);
			}
			if (Directory.Exists (GameConfig.LOCAL_MUSIC_PATH) == false) {
				Directory.CreateDirectory (GameConfig.LOCAL_MUSIC_PATH);
			}
			if (Directory.Exists (GameConfig.LOCAL_TEMP_PATH) == false) {
				Directory.CreateDirectory (GameConfig.LOCAL_TEMP_PATH);
			}
			if (Directory.Exists (GameConfig.SAVE_PATH) == false) {
				Directory.CreateDirectory (GameConfig.SAVE_PATH);
			}
		}

		void SetupApp(){
			
			CreateNeedDir ();

			Application.logMessageReceived += Application_logMessageReceived;
			// 監聽事件
			BindEvent ();
			// 調整視窗大小
			handleScreenSize.Resize (view.transform);
			// 讀取裝置設定
			deviceData.Load ();
			// 讀取使用者資訊
			userSettings.Load ();
			// 讀取記憶
			model = GetModel ();
			if (model == null) {
				throw new UnityException ("沒有設定Model");
			}
			InitIAP ();
			InitAd ();
			handleMp3Player.Setup ();
			// 切到開頭畫面
			#if TAPTAP1
			view.ChangeToTitleCN ();
			#else
			view.ChangeToTitle ();
			#endif
			// 更新音樂起始狀態(可能為關閉或打開)
			UpdateMusicSound ();
			//
			SetupFirstEnterAppStep (deviceData.IsFirstTime);

			handleAna.EnterApplication ();
        }

		void InitIAP(){
			// 注意：不能在這裡觸發任何會影響資料的回呼函式，因為這個時候資料還沒有讀取
			#if UNITY_ANDROID
				#if GASH1
			handleGash.Init ();
			handleGash.OnInvokeSDK += HandleGash_OnInvokeSDK;
				#else
			handleIAP.Init ();
			handleIAP.OnAndroidServiceConnected += HandleIAP_OnAndroidServiceConnected;
			handleIAP.OnAndroidActivityResult += HandleIAP_OnAndroidActivityResult;
			handleIAP.OnAndroidConsumePurchaseOK += HandleIAP_OnAndroidConsumePurchaseOK;
			handleIAP.OnAndroidException += HandleIAP_OnAndroidException;
			handleIAP.OnAndroidGetSkuDetailsResult += HandleIAP_OnAndroidGetSkuDetailsResult;
			handleIAP.OnAndroidGetPurchasesResult += HandleIAP_OnAndroidGetPurchasesResult;
			// android 要先連接service
			// callback is HandleIAP_OnAndroidServiceConnected
			handleIAP.AndroidBindService ();
				#endif
			#else
			handleIAP.Init ();
			handleIAP.OnPurchaseOK += HandleIAP_OnPurchaseOK;
			handleIAP.OnPurchaseFail += HandleIAP_OnPurchaseFail;
			handleIAP.OnReceviceResponse += HandleIAP_OnReceviceResponse;
			handleIAP.OnException += HandleIAP_OnException;
			// ios 直接取得資料
			// callback is HandleIAP_OnReceviceResponse
			handleIAP.IOSRequestProductData ();
			#endif
		}

		void PurchaseIAP(ItemKey itemKey){
			var itemInfo = IAPDefCht.Get (itemKey.Idx);
			#if UNITY_EDITOR || FULL_OPEN_VERSION1 || DEMO1
			// 假回傳
			HandleIAP_OnPurchaseOK(itemInfo.ID, "");
			#elif UNITY_IPHONE
			handleIAP.IOSPurchase (itemInfo.ID);
			#elif UNITY_ANDROID
				#if GASH1
			// 注意，這裡是IAPDefChs
			var cost = IAPDefChs.Get (itemKey.Idx).Cost;
			view.OpenLoadingDlg();
			// amount的單位為美元
			var amount = 0.0f;
			var currency = "";
			ParseCost(cost, ref currency, ref amount);
			if(currency != "USD"){
				throw new Exception("Gash的支付寶只支援美元");
			}
			StartCoroutine (handleGash.NewOrderAndInvokeSDK (amount, itemInfo.ID, e => {
				view.CloseLoadingDlg();
				if(e!=null){
					OnException(new ShowMessageException(e.Message));
					return;
				}
			}));
				#else
			handleIAP.AndroidGetBuyIntent (itemInfo.ID);
				#endif
			#endif
		}

		static void ParseCost(string cost, ref string currency, ref float amount){
			var ntdPattern = @"NT.(?<amount>\d+)";
			var usdPattern = @"(?<amount>\d+.?\d*) ?USD";
			var usd2Pattern = @"(?<amount>\d+) ?美元";
			var patterns = new string[] {
				ntdPattern, usdPattern, usd2Pattern
			};
			var currencyStr = new string[] {
				"NTD","USD","USD"
			};
			for(var i=0; i<patterns.Length; ++i){
				var ptn = patterns [i];
				Regex regex = new Regex (ptn, RegexOptions.IgnoreCase);
				MatchCollection matches = regex.Matches (cost);
				foreach (Match match in matches) {
					GroupCollection groups = match.Groups;
					var amt = groups ["amount"].Value;
					try{
						var parseAmt = float.Parse (amt);
						currency = currencyStr[i];
						amount = parseAmt;
						return;
					}catch(Exception e){
						throw new Exception ("資料錯誤:"+cost, e);
					}
				}
			}
			throw new Exception ("解析失敗:"+cost);
		}

		void RestoreIAP(){
			#if UNITY_IPHONE
			handleIAP.IOSRestoreCompletedTransactions ();
			#elif UNITY_ANDROID
			handleIAP.AndroidGetPhuchases (null);
			#endif
		}

		IEnumerator GetGashItem(){
			yield return handleGash.QueryAndGetItem (
				(e2, order) => {
					if(e2!=null){
						OnException(new ShowMessageException(e2.Message));
						return;
					}
					HandleIAP_OnPurchaseOK(order.item, null);
				},
				(e2) => {
					if(e2!=null){
						OnException(e2);
					}
					PerformActionAndRemove();
				}
			);
		}

		void HandleGash_OnInvokeSDK(Exception e, InvokeSDKResult result){
			if (e != null) {
				OnException (new ShowMessageException(e.Message));
			} else {
				if (result.IsSuccess) {
					StartCoroutine (GetGashItem ());
				} else {
					OnException (new ShowMessageException("rcode:"+result.RCODE));
				}
			}
		}

		void HandleIAP_OnAndroidGetPurchasesResult (HandleIAP.AndroidPurchasesResult obj){
			/*if (obj.INAPP_CONTINUATION_TOKEN != null) {
				handleIAP.AndroidGetPhuchases (obj.INAPP_CONTINUATION_TOKEN);
			}*/
			foreach (var product in obj.INAPP_PURCHASE_DATA_LIST) {
				var sku = product.productId;
				var token = product.purchaseToken;
				if (sku == "iap09") {
					model.IsUnlockBannerAd = true;
					IsBannerVisible = false;
				} else {
					handleIAP.AndroidConsumePurchase (sku, token);
				}
			}
		}

		void HandleIAP_OnAndroidGetSkuDetailsResult (IEnumerable<LitJson.JsonData> obj){
			handleDebug.Log("HandleIAP_OnAndroidGetSkuDetailsResult:"+obj.Count());
		}

		void HandleIAP_OnAndroidException (Exception obj)
		{
			view.CloseLoadingDlg ();
			OnException (obj);
		}

		void HandleIAP_OnAndroidConsumePurchaseOK (string sku)
		{
			view.CloseLoadingDlg ();
			HandleIAP_OnPurchaseOK (sku, null);
		}

		void HandleIAP_OnAndroidActivityResult (string resultCode, HandleIAP.ActivityResult result)
		{
			handleDebug.Log ("HandleIAP_OnAndroidActivityResult:responseCode:"+result.RESPONSE_CODE);
			view.CloseLoadingDlg ();
			handleIAP.AndroidGetPhuchases (null);
			var response = result.RESPONSE_CODE;
			if (response != 0) {
				return;
			}
			var sku = result.INAPP_PURCHASE_DATA.productId;
			var token = result.INAPP_PURCHASE_DATA.purchaseToken;
			handleIAP.AndroidConsumePurchase (sku, token);
		}

		void HandleIAP_OnAndroidServiceConnected ()
		{
			handleDebug.Log("HandleIAP_OnAndroidServiceConnected");
			handleIAP.AndroidGetSkuDetails ();
		}

		void HandleIAP_OnException (Exception e){
			view.CloseLoadingDlg ();
			OnException (new ShowMessageException (e.Message, e));
		}

		void HandleIAP_OnReceviceResponse (JsonData obj){
			handleDebug.Log (obj.ToJson ());
			view.CloseLoadingDlg ();
		}

		void HandleIAP_OnPurchaseFail (string sku, string errcode){
			view.CloseLoadingDlg ();
			OnException (new ShowMessageException ("errcode:"+errcode));
		}

		void HandleIAP_OnPurchaseOK (string sku, string recipt){
			handleDebug.Log ("HandleIAP_OnPurchaseOK:"+sku);
			handleAna.LogItem (null, null, sku, "IAP", 0, 1);
			view.CloseLoadingDlg ();
			var def = IAPDefCht.Get (sku);
			switch (def.ID) {
			case "iap09":
				{
					model.IsUnlockBannerAd = true;
					IsBannerVisible = false;
					Alert ("消除廣告");
				}
				break;
			default:
				model.Gold += def.Gold;
				model.Money += def.Money;
				Alert (langText.GetDlgMessage(userSettings.Language, "MesgText_M02"));
				break;
			}
			// 消除廣告Item
			UpdateUI ();
		}

		void Alert(string msg){
			var dlg = view.OpenMessageDlg ();
			dlg.Title = langText.GetDlgNote (userSettings.Language, "MessegnDlg");
			dlg.Message = msg;
		}



		void AfterWatchAd(){
			if (userSettings.IsAdRewardEnable) {
				userSettings.AppendAdRewardTime (DateTime.Now.Ticks);
				userSettings.Save ();

				var reward = ItemKey.WithItemConfigID (GameConfig.WATCH_ADS_REWARD_ITEM_CONFIGID);
				model.AddItem (reward, 1);

				var newItemDlg = view.GetMainMenu ().OpenNewItemDlg ();
				OnInitNewItemDlg (newItemDlg, reward);

				handleAna.TrackGetAdReward ();
			} else {
				Debug.LogWarning ("今天已獲得"+GameConfig.WATCH_ADS_REWARD_COUNT+"次，無法獲得獎利");
			}
		}

		void OnApplicationFocus( bool hasFocus ){
			handleAna.EnterApplication ();
		}

		void OnApplicationPause( bool pauseStatus ){
			handleAna.ExitApplication ("unknown");
		}

		void OnApplicationQuit() {
			handleAna.ExitApplication ("unknown");
		}

		void HandleExitApplication(){
			if (Input.GetKeyDown(KeyCode.Escape)) {
				Util.Instance.Log ("Exit");
				Application.Quit();
			}
		}

		static bool IsGameEnd(IGamePlayModeControl ctrl, float audioTime){
			if (ctrl is GamePlayDancingModeControlVer2) {
				var state = Game.DancingModeFlowState.Pending;
				var ignore = 0f;
				Game.ComputeDancingModeState (RhythmCtrl.HALF_BEAT_TIME * 4, RhythmCtrl.MAX_LEVEL - RhythmCtrl.FEVER_LENGTH, RhythmCtrl.FEVER_LENGTH, ctrl.IsFeverMode, audioTime, ref state, ref ignore);
				return state == Game.DancingModeFlowState.End;
			}
			if (ctrl is GamePlayFightingModeControlVer2) {
				var state = Game.FightingModeFlowState.Pending;
				var ignore = 0f;
				Game.ComputeFightingModeState (RhythmCtrl.HALF_BEAT_TIME * 4, RhythmCtrl.MAX_LEVEL - RhythmCtrl.FEVER_LENGTH, RhythmCtrl.FEVER_LENGTH, ctrl.IsFeverMode, audioTime, ref state, ref ignore);
				return state == Game.FightingModeFlowState.End;
			}
			if (ctrl is GamePlayParkouringModeControlVer2) {
				var state = Game.ParkouringModeFlowState.Pending;
				var ignore = 0f;
				Game.ComputeParkouringModeState (RhythmCtrl.HALF_BEAT_TIME * 4, RhythmCtrl.MAX_LEVEL - RhythmCtrl.FEVER_LENGTH, RhythmCtrl.FEVER_LENGTH, ctrl.IsFeverMode, audioTime, ref state, ref ignore);
				return state == Game.ParkouringModeFlowState.End;
			}
			return false;
		}

		float oneSTimer = 0;
		void Step(){
			HandleExitApplication ();
			if (IsLoading) {
				return;
			}
			if (view.IsInvalidPage) {
				return;
			}
			if (view.IsInGamePlay () || view.IsInDelayTool() || view.IsInTutorial()) {
				if (handleGameBGM.IsGameOrFeverBGMPlaying) {
					var audioTime = handleGameBGM.GameBGMAudioTime;
					// 取得音樂的時間同步遊戲
					var offsetTime = model.AudioOffsetTime;
					// 跑酷模式的Fever音樂多出一個休息時間，導致主音樂結束時，Fever音樂還沒結束
					// 所以來不及到結束時機點，主音樂就時間就停止並被設為0，無法到達ParkouringModeFlowState.End
					// 所以統一在主音樂結束後，就直接用假時間
					if (handleGameBGM.IsGameBGMEnded) {
						// 主音樂結束，這時在跑酷模式下Fever音樂還沒結束
						// 這個Flag必須在每次遊戲開始時清除
						handleAudioSyncTimerSmooth.IsMusicEnd = true;
					}
					// 要用IsMusicEnd來判斷的原因是要確保主音樂結束後一定要使用假時間
					// 有些狀況是即使主音樂結束卻可能因為按暫停的音樂的Play,Stop處理讓音樂重播了而使音樂時間從0開始計算
					// 注意：DelayTool音樂是loop，不會觸發IsMusicEnd
					if (handleAudioSyncTimerSmooth.IsMusicEnd) {
						// 假時間必須判斷沒有暫停才能更新
						if (view.Game.IsPause == false) {
							// DelayTool不會跑到這，所以不須要強制將時間Append
							handleAudioSyncTimerSmooth.AppendTime (handleAudioSyncTimerSmooth.GetLastTime () + Time.deltaTime, false);
						}
					} else {
						// 音樂時間會隨音樂暫停而暫停，所以不必判斷遊戲暫停
						// 這麼做的目的是維持要計算平滑時間值的歷史狀態
						if (view.Game.IsPause == false) {
							// DelayTool會loop，所以要強制將時間Append
							var forceAppendTime = view.IsInDelayTool () || view.IsInTutorial();
							handleAudioSyncTimerSmooth.AppendTime (audioTime, forceAppendTime);
						}
					}
					// 計算遊戲結束要用非平滑的時間
					var noSmoothAudioTime = handleAudioSyncTimerSmooth.GetLastTime ();
					var isGameEnd = IsGameEnd (view.Game.GamePlayModeControl, noSmoothAudioTime);
					if (isGameEnd) {
						view.Game.Step (9999999, 0);
					} else {
						var smoothAudioTime = handleAudioSyncTimerSmooth.GetSyncTime ();
						view.Game.Step (smoothAudioTime, offsetTime);
					}
				}
			}

			handleTouchCat.Step (Time.deltaTime, OnDoingTouchCat);
			if (view.GetMainMenu () == null) {
				return;
			}
			if (view.GetMainMenu ().GetMainUI () == null) {
				return;
			}
			// 處理貓的地圖移動
			if (view.GetMainMenu ().GetMainUI ().IsInMapUI ()) {
				var map = view.GetMainMenu ().GetMainUI ().GetMapUI ();
				if (map.IsInGameMapRoot ()) {
					var gm = map.GetGameMapRoot().GetGameMap ();
					gm.PerformStepMoveToWaypoint (Time.deltaTime);
				}
			}
			// 移動摸貓的手
			if (handleTouchCat.IsTouch) {
				var selectCat = view.GetMainMenu ().GetSelectCatDlgCtrl ();
				if (selectCat != null) {
					var worldPos = useCamera.ScreenToWorldPoint (Input.mousePosition);
					selectCat.SetHandPosition (worldPos);
				}
			}
			if (oneSTimer > 1) {
				oneSTimer -= 1;
				if (view.GetMainMenu ().GetSelectCatDlgCtrl () != null) {
					view.GetMainMenu ().GetSelectCatDlgCtrl ().UpdateCountDown ();
				}
				model.UpdateCatState(OnCatWannaPlay, OnCatSleepTimeOver, OnCatAngryTimeOver);
				model.CheckCaptureCompleted ();
				if (view.GetMainMenu ().GetMainUI ().IsInCaptureUI ()) {
					var map = view.GetMainMenu ().GetMainUI ().GetCaptureUI ();
					if (map.IsInGameCapRoot ()) {
						var gm = map.GetGameCapRoot().GetGameCap ();
						gm.UpdateUI ();
					}
				}
				if (view.GetMainMenu ().GetMusicDlg () != null) {
					handleMp3Player.Step ();
					var dlg = view.GetMainMenu ().GetMusicDlg ();
					dlg.UpdateUI (langText, userSettings.Language, handleMp3Player);
				}
			}
			oneSTimer += Time.deltaTime;
		}

		void Application_logMessageReceived (string condition, string stackTrace, LogType type){
			var isFatel = type == LogType.Error;
			handleAna.LogException(condition, isFatel);
		}

		IModel GetModel(){
			return GetComponent<GameRecordModel>();
		}

		void OnCatWannaPlay(ItemKey catId){
			UpdateUI ();
		}

		void OnCatSleepTimeOver(ItemKey catId){
			UpdateUI ();
		}

		void OnCatAngryTimeOver(ItemKey catId){
			UpdateUI ();
		}

		void OnDoingTouchCat(HandleTouchCatEvent sender){
			PerformCommand ("SelectCatDlgBtnTouchFromMain");
		}

		delegate bool CommandHandler(string cmd);

		public bool PerformCommand(string cmd){
			handleDebug.Log ("handle:" + cmd);
			if (IsTutorial) {
				if (IsValidCommandInTutorial (cmd) == false) {
					Debug.LogWarning ("現在在教學模式");
					return false;
				}
			}

			var handlers = new List<CommandHandler> () {
				HandleStartMenu,
				HandleMainUI, 
				HandleHomeUI,
				HandleShopUI,
				HandleMapUI,
				HandleCaptureUI,
				HandleConfigUI,
				HandleGamePlayUI,
				HandleDelayToolUI,
				HandleBuyItemDlg,
				HandleStartGameDlg,
				HandleStorageDlg,
				HandleSelectCatDlg,
				HandleAlbumDlg,
				HandlePhotoDlg,
				HandleIAPDlg,
				HandleBuyGachaDlg,
				HandleEventDlg,
				HandleTutorialDlg,
				HandleDownloadDlg,
				HandleMusicDlg,
				HandleStageItemDlg,
				HandleOthers
			};
			foreach (var handler in handlers) {
				if (handler (cmd)) {
					if (IsTutorial) {
						HandleTutorialCommand (cmd);
					}
					return true;
				}
			}
			Util.Instance.LogWarning ("沒有這個指令:" + cmd);
			return false;
		}

		void BindEvent(){
			var btnCommands = UniRx.Observable.Merge (
				UIEventFacade.OnPointerClick, 
				UIEventFacade.OnPointerSlide,
				UIEventFacade.OnTransformGesture,
				UIEventFacade.OnTapGesture,
				UIEventFacade.OnGameEvent
			);
			btnCommands.Subscribe (
				command => {
					var handled = PerformCommand(command);
					var result = new UIEventFacade.ButtonDownAfterHandleResult(){
						IsSucceed=handled, 
						Command=command
					};
					UIEventFacade.OnButtonDownAfterHandle.OnNext(result);
				},
				ex => {
					OnException(ex);
				}
			);

			var systemCommands = UIEventFacade.OnTutorialModelEvent;
			systemCommands.Subscribe (
				command => {
					PerformCommand(command);
				},
				ex => {
					OnException(ex);
				}
			);

			// 只留下UI的事件
			var shouldEvent = UniRx.Observable.Merge (
				                  UIEventFacade.OnPointerClick, 
				                  UIEventFacade.OnPointerSlide
			                  ).Where (cmd => {
				return cmd.Contains("GamePlayUI") == false;
			});

			shouldEvent.Subscribe (
				cmd => {
					handleAna.LogEvent ("Main", "PerformCommand", cmd, 0);
				},
				ex => {
					
				}
			);

			UIEventFacade.OnButtonDownAfterHandle.Subscribe (
				info => {
					// 遊戲中的按鈕不播音效
					if(info.Command.Contains("GamePlayUI")){
						return;
					}
					// 摸貓也不播音效
					if(info.Command.Contains("SelectCatDlgBtnTouch")){
						return;
					}
					if(info.Command.Contains("GameMapRootTransform")){
						return;
					}
					if(info.Command.Contains("GameCapRootTransform")){
						return;
					}
					if(info.Command.Contains("WorldMapRootTransform")){
						return;
					}
					if(info.Command.Contains("WorldCapRootTransform")){
						return;
					}
					if(info.IsSucceed){
						normalSoundBuffer.PlayClipInIdleChannel (clips [0], false, 0);
					} else {
						normalSoundBuffer.PlayClipInIdleChannel (clips [1], false, 0);
					}
				},
				ex => {
				}
			);

			UIEventFacade.OnAudioClipRequest.Subscribe (
				request => {
					switch(request.Type){
					case UIEventFacade.AudioClipRequest.TypeNormalSound:
						HandleAudioClipRequest(normalSoundBuffer, request);
						break;
					case UIEventFacade.AudioClipRequest.TypeSound:
						HandleAudioClipRequest(soundBuffer, request);
						break;
					case UIEventFacade.AudioClipRequest.TypeGameMusic:
						HandleAudioClipRequest(request);
						break;
					default:
						// 注意：default是Music，不要忘了處理
						HandleAudioClipRequest(musicBuffer, request);
						break;
					}
				},
				ex => {
				}
			);
		}

		void HandleAudioClipRequest(UIEventFacade.AudioClipRequest request){
			switch (request.TrackID) {
			case GameConfig.GAME_BGM_TRACK_ID:
				if (request.IsPause) {
					handleGameBGM.Pause ();
				} else {
					handleGameBGM.UnPause (view.Game.GamePlayModeControl.IsFeverMode);
				}
				break;
			case GameConfig.FEVER_BGM_TRACK_ID:
				if (request.IsPause) {
					handleGameBGM.Pause ();
				} else {
					handleGameBGM.PlayFeverBGM ();
				}
				break;
			}
		}

		static void HandleAudioClipRequest(ISoundBuffer soundBuffer, UIEventFacade.AudioClipRequest request){
			if(request.TrackID != null ){
				var channel = soundBuffer.GetChannelByLockID(request.TrackID);
				if (channel == -1) {
					if (request.IsPause) {
						
					} else {
						// 剛追蹤的話就鎖定channel
						channel = soundBuffer.PlayClipInIdleChannel (request.Clip, request.IsLoop, 0);
						soundBuffer.LockChannel (request.TrackID, channel);
						soundBuffer.SetMute(channel, request.IsMute);
					}
				} else {
					if (request.IsPause) {
						soundBuffer.SetPause (channel, request.IsPause);
					} else {
						soundBuffer.PlayClip(channel, request.Clip, request.IsLoop, 0);
					}
					soundBuffer.SetMute(channel, request.IsMute);
				}
			} else {
				soundBuffer.PlayClipInIdleChannel (request.Clip, request.IsLoop, 0);
			}
		}

		#region Update UI
		void UpdateUI(){
			UpdateMusicSound ();
			UpdateMainUI ();
			UpdateHomeUI ();
			UpdateShopUI ();
			UpdateMapUI ();
			UpdateCaptureUI ();
			UpdateConfigUI ();
			UpdateGameUI ();
			UpdatePopup ();
		}

		void UpdatePopup(){
			foreach (var dlg in view.PopupTracker.Popups) {
				if (dlg is LanguageDlg) {
					var t = dlg as LanguageDlg;
					t.Title = langText.GetDlgNote (userSettings.Language, "LanguageDlg");
				}

				if (dlg is StaffDlg) {
					var t = dlg as StaffDlg;
					t.Title = langText.GetDlgNote (userSettings.Language, "StaffDlg");
				}

				if (dlg is InviteDlg) {
					var t = dlg as InviteDlg;
					t.SetText (langText, userSettings.Language);
				}

				if (dlg is TransferDlg) {
					var t = dlg as TransferDlg;
					t.SetText (langText, userSettings.Language);
				}
				// 買完去除廣告後，iap09要消去
				if (dlg is IAPDlgCtrl) {
					var t = dlg as IAPDlgCtrl;
					OnInitIAPDlg (t);
				}

				if (dlg is MusicDlg) {
					var t = dlg as MusicDlg;
					t.UpdateUI (langText, userSettings.Language, handleMp3Player);
				}
			}
		}

		void UpdateMusicSound(){
			musicBuffer.Volume = userSettings.isMusicOn ? 1 : 0;
			soundBuffer.Volume = userSettings.isSoundOn ? 1 : 0;
			normalSoundBuffer.Volume = userSettings.isSoundOn ? 1 : 0;
			handleGameBGM.volume = userSettings.isMusicOn ? 1 : 0;
		}

		void UpdateGameUI(){
			if (view.IsInDelayTool ()) {
				view.Game.GamePlayView.SetOffsetTime (model.AudioOffsetTime);
			}
		}

		void UpdateCaptureUI(){
			if (view.GetMainMenu () == null) {
				return;
			}
			if (view.GetMainMenu ().GetMainUI () == null) {
				return;
			}
			if (view.GetMainMenu ().GetMainUI ().IsInCaptureUI ()) {
				var map = view.GetMainMenu ().GetMainUI ().GetCaptureUI ();
				if (map.IsInGameCapRoot ()) {
					var gm = map.GetGameCapRoot().GetGameCap ();
					// 更新轉蛋台解鎖
					for (var i = 0; i < 3; ++i) {
						var key = new NodeKey () {
							Group = NodeKey.GroupGameCap,
							MapIdx = gm.MapIdx,
							Idx = i
						};
						var unlock = model.IsCaptureEnable (key.CaptureKey);
						gm.SetPlatformEnable (key, unlock);
					}
					gm.UpdateUI ();
				}

				if (map.IsInWorldCapRoot ()) {
					var wc = map.GetWorldCapRoot ().GetWorldCap();
					// 國際化文字
					for (var i = 0; i < WmapSCht.ID_COUNT; ++i) {
						var key = WmapSCht.Get (i).Name;
						var value = langText.GetWorldMapDesc (userSettings.Language, key);
						wc.SetText (key, value);
					}

					wc.HideAllClear ();
					for (var i = 0; i < GameConfig.MAP_IDXS.Count; ++i) {
						var mapIdx = GameConfig.MAP_IDXS [i];
						var isUnlockMap = IsUnlockMap (mapIdx);
						if (isUnlockMap == false) {
							continue;
						}
						// 解鎖地區
						wc.Unlock (mapIdx);

						// 更新轉蛋台狀態圖
						for (var j = 0; j < 3; ++j) {
							var key = new NodeKey () {
								Group = NodeKey.GroupGameCap,
								MapIdx = mapIdx,
								Idx = j
							}; 
							var isUnlock = model.IsCaptureEnable (key.CaptureKey);
							if (isUnlock == false) {
								continue;
							}
							var state = GameConfig.CAPTURE_STATE.PENDING;
							var isCat = false;
							var cap = model.Capture (key.CaptureKey);

							state = cap.State;
							if (state == GameConfig.CAPTURE_STATE.COMPLETED) {
								isCat = cap.IsShouldGetCat;
							}

							switch (state) {
							case GameConfig.CAPTURE_STATE.COMPLETED:
								{
									// 不是貓就是照片
									if (isCat) {
										wc.EnableWaitGetCat (key);
									} else {
										wc.EnableWaitGetPhoto (key);
									}
								}
								break;
							case GameConfig.CAPTURE_STATE.CAPTURE:
								wc.EnableWaitCapture (key);
								break;
							case GameConfig.CAPTURE_STATE.NORMAL:
								wc.EnableCan (key);
								break;
							case GameConfig.CAPTURE_STATE.PENDING:
								throw new UnityException ("不可能是Pending狀態，請檢查程式");
							}
						}
					}
				}
			}
		}

		void UpdateMapUI(){
			if (view.GetMainMenu () == null) {
				return;
			}
			if (view.GetMainMenu ().GetMainUI () == null) {
				return;
			}
			if (view.GetMainMenu ().GetMainUI ().IsInMapUI ()) {
				var map = view.GetMainMenu ().GetMainUI ().GetMapUI ();
				if (map.IsInGameMapRoot ()) {
					var gm = map.GetGameMapRoot().GetGameMap ();
					gm.IsAdIconVisible = userSettings.IsAdRewardEnable;

					var catIdx = model.SelectCatKey;
					var catData = model.GetCat (catIdx);
					var prefabName = model.SelectCatKey.CatIconPrefabName ("CM", catData.Status);
					var imageObj = Util.Instance.GetPrefab (prefabName, null);
					gm.SetCatImage (imageObj.GetComponent<Image> ().sprite);
					GameObject.Destroy (imageObj);
				}
				if (map.IsInWorldMapRoot ()) {
					var worldMapRoot = map.GetWorldMapRoot ();
					var worldMap = worldMapRoot.GetWorldMap ();

					// 國際化文字
					for (var i = 0; i < WmapSCht.ID_COUNT; ++i) {
						var key = WmapSCht.Get (i).Name;
						var value = langText.GetWorldMapDesc (userSettings.Language, key);
						worldMap.SetText (key, value);
					}

					worldMap.HideAllClear ();
					for (var i = 0; i < GameConfig.MAP_IDXS.Count; ++i) {
						var mapIdx = GameConfig.MAP_IDXS[i];
						var isAllEasyClear = model.IsAllLevelDifficultyClear (mapIdx, GameRecord.Easy);
						var isAllNormalClear = model.IsAllLevelDifficultyClear (mapIdx, GameRecord.Normal);
						var isAllHardClear = model.IsAllLevelDifficultyClear (mapIdx, GameRecord.Hard);
						// 更新關卡狀態圖
						if (isAllEasyClear) {
							worldMap.ClearEasy (mapIdx);
						}
						if (isAllNormalClear) {
							worldMap.ClearNormal (mapIdx);
						}
						if (isAllHardClear) {
							worldMap.ClearHard (mapIdx);
						}
						// 解鎖地區
						var isUnlock = IsUnlockMap (mapIdx);
						if (isUnlock) {
							worldMap.Unlock (mapIdx);
						}
					}
				}
			}
		}

		bool IsUnlockMap(string mapIdx){
			// S1無條件開啟
			if (mapIdx == "S1") {
				return true;
			}
			// S2是用別的方法解鎖
			if (mapIdx == "S2") {
				foreach (var evt in handleMailEvent.Events) {
					switch (evt.ConditionID) {
					case 1:
					case 2:
						if (HandleMailEvent.IsInSatSunDay (DateTime.Now)) {
							return true;
						}
						break;
					}
				}
				return false;
			}
			var i = GameConfig.MAP_IDXS.IndexOf (mapIdx);
			if (i == -1) {
				throw new UnityException ("沒有這張地圖:"+mapIdx);
			}
			if (i == 0) {
				if (mapIdx != "01") {
					throw new UnityException ("第一張地圖必須是01");
				}
				// 第一張地圖預設解鎖
				return true;
			} else {
				// 判斷上一個地圖是不是全部過關了
				var prevMap = GameConfig.MAP_IDXS[i - 1];
				var nextLevel = model.GetCurrentLevel (prevMap);
				if (nextLevel.First ().Idx >= 10) { 
					return true;
				}
			}
			return false;
		}

		void UpdateMainUI(){
			var menu = view.GetMainMenu ();
			if (menu == null) {
				return;
			}
			var mainUI = menu.GetMainUI ();
			mainUI.SetMoney (model.Money);
			mainUI.SetGold (model.Gold);
			var catIdx = model.SelectCatKey;
			var catData = model.GetCat (catIdx);
			mainUI.SetName (deviceData.Name);
			mainUI.SetHp (catData.Hp, catData.MaxHp);
			var prefabName = catIdx.CatIconPrefabName ("CI", catData.Status);
			var imageObj = Util.Instance.GetPrefab (prefabName, null);
			mainUI.SetCatImage (imageObj.GetComponent<Image> ().sprite);
			GameObject.Destroy (imageObj);
			mainUI.Tips.UpdateUI (langText, userSettings.Language, model);

			var lang = userSettings.Language;
			var title = "";
			if (mainUI.IsInHomeUI ()) {
				title = langText.GetMainuiNote (lang, MainuiNoteEng.MUI1A01);
				if (mainUI.GetHomeUI ().IsInHousePage ()) {
					var house = mainUI.GetHomeUI ().GetHousePage ();
					foreach (var mapIdx in GameConfig.MAP_IDXS) {
						if (house.IsInHousePageMap (mapIdx) == false) {
							continue;
						}
						var key = string.Format ("MUI1C{0}", mapIdx);
						title = langText.GetMainuiNote (lang, key);
					}
					var selectCat = menu.GetSelectCatDlgCtrl ();
					if (selectCat != null) {
						var order = selectCat.CurrentCatIndexInOrder;
						var catKey = model.OwnedCats.Skip (order).First ();
						switch (catKey.Idx) {
						case 0:
							title = langText.GetMainuiNote (lang, MainuiNoteEng.MUI1C01);
							break;
						case 1:
							title = langText.GetMainuiNote (lang, MainuiNoteEng.MUI1C02);
							break;
						case 2:
							title = langText.GetMainuiNote (lang, MainuiNoteEng.MUI1C03);
							break;
						case 3:
							title = langText.GetMainuiNote (lang, MainuiNoteEng.MUI1C04);
							break;
						case 4:
							title = langText.GetMainuiNote (lang, MainuiNoteEng.MUI1C05);
							break;
						case 5:
							title = langText.GetMainuiNote (lang, MainuiNoteEng.MUI1C06);
							break;
						}
					}
				} else if (mainUI.GetHomeUI ().IsInCommunityPage ()) {
					title = langText.GetMainuiNote (lang, MainuiNoteEng.MUI1B01);
				}
			} else if (mainUI.IsInShopUI ()) {
				title = langText.GetMainuiNote (lang, MainuiNoteEng.MUI2A01);
				if (mainUI.GetShopUI ().IsInIAPShopPage ()) {
					title = langText.GetMainuiNote (lang, MainuiNoteEng.MUI2B01);
				} else if (mainUI.GetShopUI ().IsInRadioShopPage ()) {
					title = langText.GetMainuiNote (lang, MainuiNoteEng.MUI2C01);
				}
			} else if (mainUI.IsInMapUI ()) {
				if (mainUI.GetMapUI ().IsInGameMapRoot ()) {
					var map = mainUI.GetMapUI ().GetGameMapRoot ();
					foreach (var mapIdx in GameConfig.MAP_IDXS) {
						if (map.IsInMap (mapIdx) == false) {
							continue;
						}
						var key = string.Format ("MUI3A{0}", mapIdx);
						title = langText.GetMainuiNote (lang, key);
					}
				} else {
					title = langText.GetMainuiNote (lang, "MUI3B01");
				}
			} else if (mainUI.IsInCaptureUI ()) {
				if (mainUI.GetCaptureUI ().IsInGameCapRoot ()) {
					var map = mainUI.GetCaptureUI ().GetGameCapRoot ();
					foreach (var mapIdx in GameConfig.MAP_IDXS) {
						if (map.IsInMap (mapIdx) == false) {
							continue;
						}
						var key = string.Format ("MUI4A{0}", mapIdx);
						title = langText.GetMainuiNote (lang, key);
					}
				} else {
					title = langText.GetMainuiNote (lang, "MUI4B01");
				}
				var album = menu.GetAlbumDlg ();
				if (album != null) {
					title = langText.GetMainuiNote (lang, MainuiNoteEng.MUI4C01);
				}
			} else if (mainUI.IsInConfigUI ()) {
				title = langText.GetMainuiNote (lang, MainuiNoteEng.MUI5A01);
			}
			mainUI.SetPageName (title);

			if (mainUI.IsInConfigUI ()) {
				mainUI.IsSoundOn = userSettings.isSoundOn;
				mainUI.IsMusicOn = userSettings.isMusicOn;
			}
		}

		void UpdateConfigUI(){
			var menu = view.GetMainMenu ();
			if (menu == null) {
				return;
			}
			var mainUI = menu.GetMainUI ();
			if (mainUI.IsInConfigUI () == false) {
				return;
			}
			var ui = mainUI.GetConfigUI ();
			ui.UpdateUI (langText, userSettings.Language);
		}

		void UpdateHomeUI(){
			var menu = view.GetMainMenu ();
			if (menu == null) {
				return;
			}
			var mainUI = menu.GetMainUI ();
			if (mainUI.IsInHomeUI () == false) {
				return;
			}
			var homeUI = mainUI.GetHomeUI ();
			if (homeUI.IsInHousePage ()) {
				var housePage = homeUI.GetHousePage();
				var catKeys = StoreCtrl.GetItemKeys (StoreCtrl.DATA_CAT);
				foreach(var catId in catKeys){
					var isInShowingPage = housePage.IsInHousePageMap (catId.HousePageMapIdx);
					if (isInShowingPage == false) {
						continue;
					}
					var filtered = 
						from c in model.OwnedCats
							where c.StringKey == catId.StringKey
						select c;
					var enable = filtered.ToList ().Count != 0;
					housePage.GetHousePageMap().SetCatEnable (catId.CatSeqIdInHousePageMap, enable);
				}
			}
			if (homeUI.IsInMainPage ()) {
				var page = homeUI.GetMainPage ();
				var catId = model.SelectCatKey;
				var catData = model.GetCat (catId);
				var prefabName = catId.CatIconPrefabName ("CM", catData.Status);
				var imageObj = Util.Instance.GetPrefab (prefabName, null);
				page.SetCatImage (imageObj.GetComponent<Image> ().sprite);
				GameObject.Destroy (imageObj);
				try{
					page.SetUnreadMailCount (handleMailEvent.UnreadMails.Count());
					page.SetUnreadEventCount (handleMailEvent.UnreadEvents.Count());
				}catch(Exception e){
					Debug.LogWarning ("沒有取得未讀郵件或事件，忽略數字設定:"+e.Message);
				}
			}
			if (view.GetMainMenu ().GetSelectCatDlgCtrl () != null) {
				view.GetMainMenu ().GetSelectCatDlgCtrl ().UpdateUI ();
				view.GetMainMenu ().GetSelectCatDlgCtrl ().ApplyCatAnimation ();
			}
		}

		void UpdateShopUI(){
			var menu = view.GetMainMenu ();
			if (menu == null) {
				return;
			}
			var mainUI = menu.GetMainUI ();
			if (mainUI.IsInShopUI () == false) {
				return;
			}
			var shopUI = mainUI.GetShopUI ();
			if (shopUI.IsInShopPage ()) {
				var page = shopUI.GetShopPage ();
				var cat = model.GetCat (model.SelectCatKey);
				var catState = cat.Status;
				var prefabName = model.SelectCatKey.CatIconPrefabName ("CM", catState);
				var imageObj = Util.Instance.GetPrefab (prefabName, null);
				page.SetCatImage (imageObj.GetComponent<Image> ().sprite);
				GameObject.Destroy (imageObj);
			}
			if (shopUI.IsInIAPShopPage ()) {
				var page = shopUI.GetIAPShopPage ();
				page.IsAdIconVisible = userSettings.IsAdRewardEnable;
				var cat = model.GetCat (model.SelectCatKey);
				var catState = cat.Status;
				var prefabName = model.SelectCatKey.CatIconPrefabName ("CM", catState);
				var imageObj = Util.Instance.GetPrefab (prefabName, null);
				page.SetCatImage (imageObj.GetComponent<Image> ().sprite);
				GameObject.Destroy (imageObj);
			}
			if (shopUI.IsInRadioShopPage ()) {
				var page = shopUI.GetRadioShopPage ();
				var cat = model.GetCat (model.SelectCatKey);
				var catState = cat.Status;
				var prefabName = model.SelectCatKey.CatIconPrefabName ("CM", catState);
				var imageObj = Util.Instance.GetPrefab (prefabName, null);
				page.SetCatImage (imageObj.GetComponent<Image> ().sprite);
				GameObject.Destroy (imageObj);
			}
		}

		void OnInitLoadingDlg(LoadingDlg dlg){
			dlg.Title = langText.GetDlgNote (userSettings.Language, "LoadDlg");
			dlg.RandomPic ();
		}
		#endregion

		#region Query
		bool QueryPhotoEnable(PhotoKey key){
			return model.IsPhotoEnable (key);
		}

		string QueryLevelFileName(LevelKey level){
			return level.LevelFileName;
		}

		int QueryItemCount(ItemKey key){
			return model.ItemCount (key);
		}
		#endregion

		void OnInitDownloadDlg(DownloadDlg dlg){
			dlg.UpdateUI (langText, userSettings.Language);
			dlg.SetDownloadPercentage (0);
		}

		#region Helper
		IEnumerator PerformDownloadAssetBundleIfNeeded(IEnumerable<string> needBundles){
			if (needBundles == null) {
				yield break;
			}
			var shouldDownload = needBundles.Where (bundleName => {
				return AssetBundleUtil.IsLocalAssetBundleExist(bundleName) == false;
			});
			if (shouldDownload.Count() == 0) {
				yield break;
			}
			var dlg = view.OpenDownloadDlg ();
			OnInitDownloadDlg (dlg);
			foreach (var bundleName in shouldDownload) {
				yield return handleAssetBundle.DownloadAssetBundle (bundleName, progress=>{
					dlg.SetDownloadText(bundleName);
					dlg.SetDownloadPercentage(progress);
				}, null);
			}
			view.CloseDownloadDlg ();
		}

		bool isLoading = false;

		bool IsLoading{
			get{ return isLoading; }
		}

		IEnumerator ExecWithLoadingDlg(Action fn, IEnumerator steps = null){
			isLoading = true;
			var dlg = view.OpenLoadingDlg ();
			OnInitLoadingDlg (dlg);
			yield return null;
			if (fn != null) {
				try {
					fn ();
				} catch (Exception e) {
					OnException (e);
				}
			}
			if (steps != null) {
				/* 自己觸發Corutine, 只支援yield return null
				while(steps.MoveNext()){
					yield return new WaitForSeconds(0.1f);
				}*/
				yield return steps;
			}
			view.CloseLoadingDlg ();
			isLoading = false;
		}

		IEnumerator ExecWithDownloadAssetBundleIfNeeded(Action fn, IEnumerable<string> needBundles = null, IEnumerator steps = null){
			yield return PerformDownloadAssetBundleIfNeeded (needBundles);
			yield return ExecWithLoadingDlg (fn, steps);
		}

		static bool IsPositionInRect(Camera camera, Vector3 mouse, RectPosition rect){
			var min = rect.Min (camera);
			var max = rect.Max (camera);
			if (min.x > mouse.x || mouse.x > max.x) {
				return false;
			}
			if (min.y > mouse.y || mouse.y > max.y) {
				return false;
			}
			return true;
		}

		static bool ShouldBoundGameMapRoot(Camera camera, Transform rootTransform, RectPosition rootRect, RectPosition windowRect, ref float scale, ref Vector3 offset){
			var sholdTrans = false;
			var mapWidth = rootRect.Width (camera);
			var mapHeight = rootRect.Height (camera);
			if (mapWidth < mapHeight) {
				if (mapWidth < windowRect.Width (camera)) {
					scale = 1.1f;
					sholdTrans = true;
				}
			} else {
				if (mapHeight < windowRect.Height (camera)) {
					scale = 1.1f;
					sholdTrans = true;
				}
			}
			if (rootTransform.localScale.x < GameConfig.MIN_SCALE) {
				scale = 1.1f;
				sholdTrans = true;
			}
			var leftBottomOffset = windowRect.Min (camera) - rootRect.Min(camera);
			if (leftBottomOffset.x < 0) {
				var worldOffset = windowRect.Min(null) - rootRect.Min(null);
				var localOffset = rootTransform.worldToLocalMatrix * worldOffset;
				offset.x = localOffset.x;
				sholdTrans = true;
			}
			if (leftBottomOffset.y < 0) {
				var worldOffset = windowRect.Min(null) - rootRect.Min(null);
				var localOffset = rootTransform.worldToLocalMatrix * worldOffset;
				offset.y = localOffset.y;
				sholdTrans = true;
			}
			var rightTopOffset = windowRect.Max (camera) - rootRect.Max(camera);
			if (rightTopOffset.x > 0) {
				var worldOffset = windowRect.Max(null) - rootRect.Max(null);
				var localOffset = rootTransform.worldToLocalMatrix * worldOffset;
				offset.x = localOffset.x;
				sholdTrans = true;
			}
			if (rightTopOffset.y > 0) {
				var worldOffset = windowRect.Max(null) - rootRect.Max(null);
				var localOffset = rootTransform.worldToLocalMatrix * worldOffset;
				offset.y = localOffset.y;
				sholdTrans = true;
			}
			return sholdTrans;
		}

		static int GetRightCatIdx(int defCatIdx){
			if (RhythmCtrl.RHYTHM_DATA_ARRAY == null) {
				throw new UnityException ("必須先呼叫RhythmCtrl.InitRhythm");
			}
			string enemyCat = RhythmCtrl.ENEMY_CAT;
			if (enemyCat == "") {
				Debug.LogWarning ("沒有設定敵人貓");
				return defCatIdx;
			}
			try{
				char[] token = {'_'};
				string[] subStr = enemyCat.Split(token);
				int enemyCatIndex = System.Convert.ToInt32(subStr[1])-1;
				return enemyCatIndex;
			}catch(Exception){
				Debug.LogWarning ("敵人貓格式不錯，回傳預設值");
				return defCatIdx;
			}
		}

		void CheckLevelConfig(AudioClip bgm){
			switch (RhythmCtrl.LEVEL_TYPE) {
			case RhythmCtrl.PARKOURING_MODE:
				{
					var state = Game.ParkouringModeFlowState.Pending;
					var currentSinceTime = 0f;
					var musicLength = bgm.length;

					var isFeverMode = true;
					var timeForComputeState = isFeverMode ? musicLength + RhythmCtrl.HALF_BEAT_TIME * 4 * 8 : musicLength;
					Game.ComputeParkouringModeState(RhythmCtrl.HALF_BEAT_TIME*4, RhythmCtrl.MAX_LEVEL-RhythmCtrl.FEVER_LENGTH, RhythmCtrl.FEVER_LENGTH, isFeverMode, timeForComputeState, ref state, ref currentSinceTime);

					if (state != Game.ParkouringModeFlowState.End) {
						throw new ShowMessageException ("偵測到關卡無法結束，請聯絡我們");
					}

					isFeverMode = false;
					timeForComputeState = isFeverMode ? musicLength + RhythmCtrl.HALF_BEAT_TIME * 4 * 8 : musicLength;
					Game.ComputeParkouringModeState(RhythmCtrl.HALF_BEAT_TIME*4, RhythmCtrl.MAX_LEVEL-RhythmCtrl.FEVER_LENGTH, RhythmCtrl.FEVER_LENGTH, isFeverMode, timeForComputeState, ref state, ref currentSinceTime);
					if (state != Game.ParkouringModeFlowState.End) {
						throw new ShowMessageException ("偵測到關卡無法結束，請聯絡我們");
					}
				}
				break;
			}
		}

		IEnumerator StartGamePlayTutorialVer2(View view){
			return StartPlayGameVer2 (view, LevelKey.TutorialLevelKey);
		}

		IEnumerator StartDelayToolVer2(View view){
			return StartPlayGameVer2 (view, LevelKey.DelayToolLevelKey);
		}

		IEnumerator StartPlayGameVer2(View view, LevelKey levelKey){
			isLoading = true;
			Util.Instance.UnloadAsset ();
			// 打開Loading頁
			var dlg = view.OpenLoadingDlg ();
			OnInitLoadingDlg (dlg);
			yield return null;
			var levelName = QueryLevelFileName (levelKey);
			// 讀關卡
			//RhythmCtrl.use32 = true;
			RhythmCtrl.LoadData (levelName);
			// 取得參加貓
			var leftCatIdx = model.SelectCatKey;
			var rightCatIdx = GetRightCatIdx (leftCatIdx.Idx);
			// 計算關卡資料
			var catDefData = CatDef.Get(leftCatIdx.Idx);
			if (levelKey.IsNormalLevel) {
				// 一般關卡要計算貓的特性
				var levelDef = LevelDef.Get (levelKey.ConfigID);
				RhythmCtrl.InitRhythm (catDefData, levelDef);
			} else {
				RhythmCtrl.InitRhythm (catDefData, null);
			}
			// 切換到遊戲頁
			if (levelKey == LevelKey.DelayToolLevelKey) {
				view.ChangeToDelayTool ();
			} else if (levelKey == LevelKey.TutorialLevelKey) {
				view.ChangeToTutorial ();
			} else {
				view.ChangeToGamePlay ();
			}
			// 設定參加貓
			view.Game.leftCatIdx = leftCatIdx.Idx;
			view.Game.rightCatIdx = rightCatIdx;
			view.Game.levelKey = levelKey;
			// 初始化遊戲
			view.Game.InitComponent ();
			view.Game.SetData (RhythmCtrl.LEVEL_TYPE, RhythmCtrl.TIME_SCALE);
			// 一般關卡要設定血量和顯示使用道具
			if (levelKey.IsNormalLevel) {
				var hp = model.GetCat (model.SelectCatKey).Hp;
				var fullHp = model.GetCat (model.SelectCatKey).MaxHp;
				view.Game.SetHp (hp, fullHp);
				if (sessions.UsedItem != null) {
					var levelItemObj = Util.Instance.GetPrefab (new ItemKey (sessions.UsedItem).BuyItemPrefabName, null);
					view.Game.GamePlayView.LevelItem = levelItemObj.GetComponent<Image> ().sprite;
					GameObject.Destroy (levelItemObj);
				}
			}
			if (levelKey == LevelKey.DelayToolLevelKey) {
				view.Game.GamePlayView.TextExplan = langText.GetDlgMessage (userSettings.Language, "MesgText_GD1");
			}
			// 準備開始遊戲
			handleAudioSyncTimerSmooth.Clear ();
			handleGameBGM.SetAudioClip (view.Game.GamePlayView.LevelSoundData.levelBGM, view.Game.GamePlayView.LevelSoundData.feverBGM);
			yield return handleGameBGM.WaitingForSondLoaded();
			if (levelKey.IsNormalLevel) {
				// 確認關卡資料的正確性。不正確會導致遊戲無法結束
				CheckLevelConfig (view.Game.GamePlayView.LevelSoundData.levelBGM);
				model.CatEnterGame (model.SelectCatKey);
			}
			IsBannerVisible = false;
			if (view.IsInDelayTool ()) {
				UpdateUI ();
			}
			view.CloseLoadingDlg ();
			// 開始遊戲
			view.Game.StartPlay ();
			var shouldLoop = levelKey == LevelKey.TutorialLevelKey || levelKey == LevelKey.DelayToolLevelKey;
			handleGameBGM.PlayGameBGM (shouldLoop);

			isLoading = false;
		}

		static GameObject GetCatIcon(string prefix, int catIdx, GameConfig.CAT_STATE_ID catStateId, GameObject anchor){
			string postfix = null;
			switch (catStateId)
			{
			case GameConfig.CAT_STATE_ID.STATE_IDLE:
				postfix = "_01";
				break;
			case GameConfig.CAT_STATE_ID.STATE_ANGRY:
				postfix = "_02";
				break;
			case GameConfig.CAT_STATE_ID.STATE_WANNA_PLAY:
				postfix = "_03";
				break;
			case GameConfig.CAT_STATE_ID.STATE_SLEEP:
				postfix = "_04";
				break;
			case GameConfig.CAT_STATE_ID.STATE_HUNGRY:
				postfix = "_05";
				break;
			default:
				postfix = "";
				break;
			}

			var path = string.Format ("Output/CatIcon/{0}{1:00}{2}", prefix, catIdx, postfix);
			var imageObj = Util.Instance.GetPrefab (path, anchor);
			return imageObj;
		}
		#endregion

		public bool pressOnceFlag;
		bool HandleStartMenu(string cmd){
			switch (cmd) {
			case "StartMenuBtnEng":
			case "StartMenuBtnCht":
				{
					if (pressOnceFlag == true) {
						return false;
					}
					if (NextFirstEnterAppStep ()) {
						// 有任何副作用才將pressOnceFlag設為真
						pressOnceFlag = true;
					}
				}
				break;
			case "LoginDlgBtnOK":
				{
					var dlg = view.OpenLoginDlg ();
					// 名字全部用大寫
					var name = dlg.NameText.ToUpper().Trim();
					if(name.Length == 0){
						Debug.LogWarning("必須輸入名字");
						return false;
					}
					if (name.Length > 20) {
						var msg = langText.GetDlgMessage (userSettings.Language, "MesgText_M05");
						Alert (msg);
						return false;
					}
					var action = "";
					if( dlg.MetaData.ContainsKey("action")){
						action = (string)dlg.MetaData ["action"];
					}
					switch (action) {
					case "changeName":
						{
							StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
								try{
									RemixApi.UpdateName (deviceData.DeviceID, name);
									deviceData.Name = name;
									deviceData.Save();
									var inviteDlg = view.GetMainMenu().GetInviteDlg();
									if(inviteDlg != null){
										inviteDlg.Name = name;
									}
									view.CloseLoginDlg();
									UpdateUI();
								}catch(Exception e){
									OnException(new ShowMessageException(langText.GetDlgMessage(userSettings.Language, "MesgText_M04"), e));
								}
							}));
						}
						break;
					default:
						{
							try{
								// 登入並取得ID
								var device = RemixApi.GetDevice (name);
								// 將ID記在本機
								deviceData.DeviceID = device.ID;
								deviceData.InviteCode = device.InviteCode;
								deviceData.TransferCode = device.TransferCode;
								deviceData.Name = device.Name;
								deviceData.Save ();
								// 關閉LoginDlg
								view.CloseLoginDlg();

								NextFirstEnterAppStep();
							}catch(Exception e){
								var msg = langText.GetDlgMessage (userSettings.Language, "MesgText_L01");
								OnException(new ShowMessageException(msg, e));
							}
						}
						break;
					}
				}
				break;
			default:
				return false;
			}
			return true;
		}

		#region handle seq execution
		// 處理依順序觸發的執行
		// 在可能會有依順序觸發的按鈕中都要呼叫PerformActionAndRemove
		// 比如：NewBPhotoDlgBtnOK, NewItemDlgBtnOK等，多視窗重疊出現時
		// 注意：推進來的Action只能修改視覺(view)，不能有影響model的行為，因為玩家可能在觸發中跳離遊戲
		List<Action> seqs = new List<Action> ();
		void PushAction(Action action){
			seqs.Add (action);
		}
		void PerformActionAndRemove(){
			if (seqs.Count == 0) {
				return;
			}
			try{
				seqs [0] ();
				seqs.RemoveAt (0);
			}catch(Exception e){
				OnException (e);
			}
		}
		#endregion

		#region handle first enter app
		List<Func<bool>> steps = new List<Func<bool>> ();
		// 回傳true代表有副作用
		bool NextFirstEnterAppStep(){
			var hasSideEffect = false;
			// steps被消化光後，就不再有任何副作用在這個函式裡
			// 注意呼叫NextFirstEnterAppStep的時機
			while (steps.Count() > 0) {
				hasSideEffect = true;
				var fn = steps.First ();
				try{
					var wait = fn.Invoke () == false;
					steps.RemoveAt (0);
					if (wait) {
						break;
					}
				}catch(Exception e){
					OnException (e);
					break;
				}
			}
			return hasSideEffect;
		}
		IEnumerator RestartApp(){
			yield return null;
			steps.Clear ();
			steps.Add (() => {
				Alert ("你已轉機，請重新登入");
				return true;
			});
			steps.Add (() => {
				deviceData.Clear();
				userSettings.Clear();
				model.Clear ();
				return false;
			});
			SetupFirstEnterAppStep (true);
			NextFirstEnterAppStep ();
		}
		// 這個方法必須在DeviceData.Load之後才能呼叫
		void SetupFirstEnterAppStep(bool isFirstTime){
			// 第一次遊戲
			if (isFirstTime) {
				steps.Add (() => {
					// 讓玩家輸入使用者名稱，輸入完後會呼叫"LoginDlgBtnOK"的command
					var dlg = view.OpenLoginDlg ();
					OnInitLoginDlg (dlg);
					return false;
				});
				#if TAPTAP1
				// ignore
				#else
				// 非TapTap的版本才需要打開語言
				// 打開語言
				steps.Add (() => {
					view.OpenLanguageDlg ();
					return false;
				});
				#endif
			}
			#if TAPTAP1
			// 做修改但不儲存，因為UserSettings只會在遊戲啟動時讀一次
			userSettings.language = LanguageText.Chs;
			#endif
			// 本機測試不必檢查帳號，讓遊戲可以在沒有Server的情況下測試
			var shouldCheckUserAccount = handleAssetBundle.IsSimulation == false;
			if (shouldCheckUserAccount) {
				steps.Add (() => {
					// 查詢帳號有沒有被禁
					string msg = "";
					try {
						if (RemixApi.IsDeviceEnable (deviceData.DeviceID, ref msg) == false) {
							var isTransfer = msg.Contains("transfer to");
							if( isTransfer ){
								// 注意：必須用協程呼叫，因為方法中會修改到steps
								StartCoroutine(RestartApp());
								return false;
							} else {
								var errorMsg = langText.GetDlgMessage(userSettings.Language, "MesgText_M06");
								OnException (new ShowMessageException (errorMsg + ":" + msg));
								return false;
							}
						}
					} catch (Exception e) {
						OnException (new ShowMessageException (langText.GetDlgMessage(userSettings.Language, "MesgText_M04"), e));
						return false;
					}
					return true;
				});
			}

			Action PassDownloadStep = () => {
				view.CloseDownloadDlg ();

				#if UNITY_ANDROID
				if( isFirstTime ){
					model.AudioOffsetTime = GameConfig.INIT_ANDROID_AUDIO_OFFSET;
					// 安卓第一次登入時要進入DelayTool
					StartCoroutine(StartDelayToolVer2(view));
				} 
				else 
				#endif
				{
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var menu = view.ChangeToMenu ();
						OnInitMenu(menu);

						view.GetMainMenu ().ChangeToMap ();
						var map = view.GetMainMenu ().GetMainUI ().GetMapUI ();
						var mapIdx = GameConfig.MAP_IDXS[0];
						// 取得最後一次遊玩的地圖
						if (userSettings.HasLastPlayLevel) {
							mapIdx = userSettings.LastEnterLevel.MapIdx;
						}
						OnInitMap (map, mapIdx);
						// 每次進入遊戲就判斷有沒有付了錢卻沒拿到的Item
						StartCoroutine (GetGashItem ());
					}));
				}
			};

			steps.Add (() => {
				// 取得存檔
				model.Load();
				// 每次都必須執行DownloadAllAssetBundlesAtFirstTime檢查檔案完整性
				var dlg = view.OpenDownloadDlg ();
				OnInitDownloadDlg(dlg);
				// 遠端AssetBundle改變，全部重新下載
				var isNeedDownload = handleAssetBundle.IsNeedDownloadAssetBundleAndSetDownloadFlag ();
				// 如果AssetBundle有變動，就先把Flag設為False
				// 記得在下載完成時將Flag設為True
				if(isNeedDownload){
					userSettings.IsDownloadPackageOKFlag = false;
					userSettings.Save();
				}
				// 如果完全下載成功(沒有任一包下載不完全)，就直接進入遊戲
				// 避免無意義的檢查
				if(userSettings.IsDownloadPackageOKFlag == true){
					PassDownloadStep();
					return true;
				}
				// 如果isNeedDownload為真，代表不用檢查完整性。反正全部都要重新下載
				// 反之，就需要檢查完整性
				var isNeedCheckFile = isNeedDownload == false;
				var isNeedCheckUpdate = isNeedDownload;
				StartCoroutine (handleAssetBundle.DownloadAllAssetBundlesAtFirstTime(
					langText,
					userSettings.Language,
					isNeedCheckFile,
					isNeedCheckUpdate,
					()=>{
						// 下載完成，將Flag設為True
						// 避免無意義的檢查
						userSettings.IsDownloadPackageOKFlag = true;
						userSettings.Save();
					},
					(progress, curr, count) => {
						var percentage = (float)curr / count;
						dlg.SetDownloadPercentage (percentage);
						dlg.SetDownloadText (progress);
					},
					(e) => {
						if (e != null) {
							OnException (new ShowMessageException (e.Message, e));
						} else {
							PassDownloadStep();
						}
					}
				));
				return true;
			});
		}
		#endregion

		void OnInitMenu(Menu menu){
			menu.GetMainUI ().Tips.RandomTip ();

			// 音樂會在GamePlay中LevelSoundDataCtrl.PlayLevelBGM時關閉
			handleBGM.RequestPlay (HandleBGM.MainUI);
			IsBannerVisible = model.IsUnlockBannerAd == false;
			// CheckEventAndOpenEventDlg ();
			OnOpenScreen(menu);
		}

		void OnOpenScreen(object obj){
			if (obj is Menu) {
				// 不需要Log菜單，因為一進入菜單就是map頁
				// handleAna.LogScreen ("menu");
			} else if (obj is HousePage) {
				handleAna.LogScreen ("house");
			} else if (obj is MapUI) {
				handleAna.LogScreen ("map");
			} else if (obj is CaptureUI) {
				handleAna.LogScreen ("capture");
			} else if (obj is HomeUI) {
				handleAna.LogScreen ("home");
			} else if (obj is ShopUI) {
				handleAna.LogScreen ("shop");
			}
		}

		void CheckEventAndOpenEventDlg(){

			Func<MapDef, RemixApi.Event, Action> GenAction = (MapDef md, RemixApi.Event evt)=>{
				return ()=>{
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var dlg = view.GetMainMenu ().OpenEventDlg ();
						OnInitEventDlg (dlg, md, evt);
					}));
				};
			};

			foreach (var evt in handleMailEvent.Events) {
				for (var i = 0; i < MapDef.ID_COUNT; ++i) {
					var md = MapDef.Get (i);
					if (md.Event == evt.ConditionID) {
						PushAction (GenAction (md, evt));
						break;
					}
				}
			}

			PerformActionAndRemove ();
		}

		void OnInitNewItemDlg(NewDlgCtrl dlg, object key){
			// prevent out of memory
			Util.Instance.UnloadAsset ();

			if (key is ItemKey) {
				var item = key as ItemKey;
				switch (item.Type) {
				case StoreCtrl.DATA_CAT:
					dlg.SetTitle (langText.GetDlgNote(userSettings.Language, "NewCatDlg"));
					dlg.SetName (langText.GetCatName(userSettings.Language, item));
					break;
				default:
					dlg.SetTitle (langText.GetDlgNote(userSettings.Language, "NewItemDlg"));
					dlg.SetName (langText.GetItemName (userSettings.Language, item));
					break;
				}
				dlg.LoadData (item);
			} else if (key is PhotoKey) {
				var photo = key as PhotoKey;
				switch (photo.Type) {
				case PhotoKey.TypeBigPhoto:
					dlg.SetTitle (langText.GetDlgNote(userSettings.Language, "NewBPhotoDlg"));
					break;
				default:
					dlg.SetTitle (langText.GetDlgNote(userSettings.Language, "NewSPhotoDlg"));
					break;
				}
				dlg.SetName (photo.PhotoNameForS);
				dlg.LoadData (photo);
			} else {
				OnException (new UnityException ("沒有這個類型才對:"+key.GetType()));
			}
		}

		void OnInitEventDlg(EventDlg dlg, MapDef md, RemixApi.Event evt){
			dlg.SetTitle (md.Desc);
			dlg.MetaData = new Dictionary<string, object> () {
				{"id", md.ID},
				{"evtId", evt.ID},
			};
			try{
				var obj = Util.Instance.GetPrefab (GameRecord.EventPicPrefabName(md), null);
				dlg.SetPic (obj.GetComponent<Image> ().sprite);
				Destroy (obj);
			}catch(Exception e){
				Debug.LogWarning ("無法設定圖:"+e.Message);
			}
		}

		void OnInitMailGiftDlg(MailGiftDlg dlg, int gotCnt){
			dlg.SetTitle (langText.GetDlgNote (userSettings.Language, "MailGiftDlg"));

			var items = new List<ItemKey> ();
			for (var i = 0; i < GiftDef.ID_COUNT; ++i) {
				var giftKey = string.Format ("Mg{0:00}", i+1);
				var gd = GiftDef.Get (giftKey);
				var item = ItemKey.WithItemConfigID (gd.Item);
				items.Add (item);
			}
			dlg.SetItem (items);

			var now = DateTime.Now;
			for (var i = 0; i < 25; ++i) {
				var getted = gotCnt > i;
				dlg.SetGetDay (i, getted);
			}
		}

		void OnInitMap(MapUI map, string mapIdx){
			// 防呆處理
			if (GameConfig.IsMapDeveloped (mapIdx) == false) {
				mapIdx = GameConfig.MAP_IDXS [0];
			}
			// 教學模式只會跳到第一關
			if (IsTutorial == true) {
				mapIdx = GameConfig.MAP_IDXS [0];

			} else {
				// 記錄最後一次進入的地圖，用來返回
				userSettings.LastEnterLevel = new LevelKey () {
					MapIdx = mapIdx,
					Idx = 0
				};
				userSettings.Save ();
			}

			// 取得目前關卡
			var currLevel = model.GetCurrentLevel(mapIdx).First();
			// 讀取地圖
			map.GetGameMapRoot().LoadMap (currLevel.MapIdx);
			map.GetGameMapRoot ().GetGameMap ().PrepareAdIconButton ();
			map.GetGameMapRoot().GetGameMap ().PrepareMapNodeCtrl (currLevel.MapIdx, currLevel.Idx, true);
			// 先將貓跳到上一關的節點
			var prevLevel = currLevel.Idx - 1;
			var p = map.GetGameMapRoot().GetGameMap ().JumpCatToWaypoint ("level" + (prevLevel+1));
			// 準備移到目前關卡節點的waypoint移動
			// 由update中呼叫PerformStepMoveToWaypoint來移動貓
			map.GetGameMapRoot().GetGameMap ().PrepareStepMoveToWaypoint (p, "level"+(currLevel.Idx+1));
			// 將"鏡頭"移到貓目的地
			var p2 = map.GetGameMapRoot ().GetGameMap ().WaypointManager.GetWaypoint ("level"+(currLevel.Idx+1));
			map.GetGameMapRoot().transform.localPosition = -(p2.transform.localPosition * map.GetGameMapRoot().transform.localScale.x);
			// 矯正邊界位置
			StartCoroutine (FixMapTransform (useCamera, map.GetGameMapRoot().transform, map.GetGameMapRoot().GetGameMap().RectPosition, view.GetMainMenu().GetMainUI().WindowRect));
			UpdateUI ();
			OnOpenScreen (map);
			// 每進入地圖頁就判斷需不需要進入教學
			// TriggerTutorialIfNeed ();
		}

		void TriggerTutorialIfNeed(){
			StartCoroutine (TriggerTutorialIfNeedImpl ());
		}

		const string GamePlayTutorialFlag = "GamePlayTutorialModel";
		const string CommonTutorialFlag = "CommonTutorialModel";

		// 參照"GameTrainingGameEnd"和"TutorialModelEnd"
		IEnumerator TriggerTutorialIfNeedImpl(){
			yield return null;
			// 只會觸發一次
			if (model.IsMarkRead (GamePlayTutorialFlag) == false) {
				StartCoroutine (StartGamePlayTutorialVer2 (view));
				yield break;
			}
			if (model.IsMarkRead (CommonTutorialFlag) == false) {
				// 第二次呼叫會跑到這
				// 這時會切換成假model
				StartTutorial (TutorialModel.Type.Interative);
				yield break;
			}
		}

		bool HandleMainUI(string cmd){
			if (view.IsInMenu () == false) {
				return false;
			}
			if (view.GetMainMenu ().PopupTracker.IsPopupActive (null) == false) {
				Util.Instance.LogWarning ("被PopupTracker阻擋，請關掉上層面板");
				return false;
			}
			switch (cmd) {
			case "MainUIBtnCatIcon":
				{
					Util.Instance.UnloadAsset();
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						view.GetMainMenu ().ChangeToHome ();
						var ui = view.GetMainMenu ().GetMainUI ().GetHomeUI ();
						OnInitHome (ui);
						view.GetMainMenu ().ChangeToSub2 ();
						var house = ui.GetHousePage ();
						OnInitHousePage (house);
						var dlg = view.GetMainMenu ().OpenSelectCatDlg ();
						OnInitSelectCatDlg (dlg, model.SelectCatKey);
						UpdateUI ();
					}));
				}
				break;
			case "MainUIBtnHome":
				{
					Util.Instance.UnloadAsset ();
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						view.GetMainMenu ().ChangeToHome ();
						var ui = view.GetMainMenu ().GetMainUI ().GetHomeUI ();
						OnInitHome (ui);
						UpdateUI ();
					}));
				}
				break;
			case "MainUIBtnShop":
				{
					Util.Instance.UnloadAsset ();
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (()=>{
						view.GetMainMenu ().ChangeToShop ();
						var ui = view.GetMainMenu ().GetMainUI ().GetShopUI ();
						OnInitShop (ui);
						UpdateUI ();
					}));
				}
				break;
			case "MainUIBtnMap":
				{
					Util.Instance.UnloadAsset ();
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (()=>{
						view.GetMainMenu ().ChangeToMap ();
						var map = view.GetMainMenu ().GetMainUI ().GetMapUI ();
						var mapIdx = GameConfig.MAP_IDXS[0];
						// 取得最後一次遊玩的地圖
						if (userSettings.HasLastPlayLevel) {
							mapIdx = userSettings.LastEnterLevel.MapIdx;
						}
						OnInitMap (map, mapIdx);
					}));
				}
				break;
			case "MainUIBtnCapture":
				{
					Util.Instance.UnloadAsset ();
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (()=>{
						view.GetMainMenu ().ChangeToCapture ();
						var mapIdx = GameConfig.MAP_IDXS[0];
						// 取得最後一次遊玩的地圖
						if (userSettings.HasLastPlayCapture) {
							mapIdx = userSettings.LastEnterCapture.MapIdx;
						}
						var ui = view.GetMainMenu ().GetMainUI ().GetCaptureUI ();
						OnInitCapture (ui, mapIdx);
					}));
				}
				break;
			case "MainUIBtnConfig":
				Util.Instance.UnloadAsset ();
				StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
					view.GetMainMenu ().ChangeToConfig ();
					UpdateUI ();
				}));
				break;
			case "MainUIBtnSub1":
				Util.Instance.UnloadAsset ();
				StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
					view.GetMainMenu ().ChangeToSub1 ();
					if (view.GetMainMenu ().GetMainUI ().IsInMapUI ()) {
						var map = view.GetMainMenu ().GetMainUI ().GetMapUI ();
						if (map.IsInWorldMapRoot ()) {
							var worldMapRoot = map.GetWorldMapRoot ();
							OnInitWorldMapRoot (worldMapRoot, "");
						}
					}
					if (view.GetMainMenu ().GetMainUI ().IsInCaptureUI ()) {
						var map = view.GetMainMenu ().GetMainUI ().GetCaptureUI ();
						if (map.IsInWorldCapRoot ()) {
							var worldMapRoot = map.GetWorldCapRoot ();
							worldMapRoot.LoadMap (0);
							var worldMap = worldMapRoot.GetWorldCap ();
							StartCoroutine (FixMapTransform (useCamera, worldMapRoot.transform, worldMap.RectPosition, view.GetMainMenu ().GetMainUI ().WindowRect));
							// TODO
						}
					}
					UpdateUI ();
				}));
				break;
			case "MainUIBtnSub2":
				Util.Instance.UnloadAsset ();
				if (view.GetMainMenu ().GetMainUI ().IsInMapUI ()) {
					Alert("Coming Soon");
					return false;
				}
				StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
					view.GetMainMenu ().ChangeToSub2 ();
					if (view.GetMainMenu ().GetMainUI ().IsInHomeUI ()) {
						var map = view.GetMainMenu ().GetMainUI ().GetHomeUI ();
						if (map.IsInHousePage ()) {
							OnInitHousePage (map.GetHousePage ());
						}
					}
					UpdateUI ();
				}));
				break;
			case "MainUIBtnAlbum":
				{
					Util.Instance.UnloadAsset ();
					var captureUI = view.GetMainMenu ().GetMainUI ().GetCaptureUI ();
					if (captureUI.IsInWorldCapRoot ()) {
						Debug.LogWarning ("世界地圖中無法看照片");
						return false;
					}
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var dlg = view.GetMainMenu ().OpenAlbumDlg ();
						var mapIdx = captureUI.GetGameCapRoot ().CurrentMapIdx;
						OnInitAlbumDlg(dlg, mapIdx);
					}));
				}
				break;
			case "MainUIBtnSound":
				{
					userSettings.isSoundOn = !userSettings.isSoundOn;
					userSettings.Save ();
					UpdateUI ();
				}
				break;
			case "MainUIBtnMusic":
				{
					userSettings.isMusicOn = !userSettings.isMusicOn;
					userSettings.Save ();
					UpdateUI ();
				}
				break;
			case "MainUIBtnBuy":
				{
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var dlg = view.GetMainMenu ().OpenIAPDlg ();
						OnInitIAPDlg (dlg);
					}));
				}
				break;
			case "MainUIBtnTips":
				{
					view.GetMainMenu ().GetMainUI ().Tips.NextTip ();
					UpdateUI ();
				}
				break;
			default:
				return false;
			}
			return true;
		}

		void OnInitAlbumDlg(AlbumDlgCtrl dlg, string mapIdx){
			var configId = string.Format ("AlbumDlg{0}", mapIdx);
			dlg.SetTitle (langText.GetDlgNote (userSettings.Language, configId));
			dlg.LoadData (mapIdx, QueryPhotoEnable);
		}

		void OnInitIAPDlg(IAPDlgCtrl dlg){
			var keys = StoreCtrl.GetItemKeys (StoreCtrl.DATA_IAP);
			// 取消bannerAd, 所以也沒有解鎖功能
			if (true /*model.IsUnlockBannerAd == true*/) {
				keys = keys.Where (key => {
					var def = IAPDefCht.Get (key.Idx);
					return def.ID != GameConfig.UNLOCK_BANNER_IAP_SKU;
				}).ToList();
			}
			dlg.InitData (langText, userSettings.Language, keys);
			dlg.UpdateUI (langText, userSettings.Language);
		}

		void OnInitHome(HomeUI ui){
			// 讀取郵件
			StartCoroutine (handleMailEvent.LoadMail (model, e=>{
				if( e != null){
					Debug.LogWarning(e.Message);
				}
				UpdateUI();
			}));
			// 讀取事件
			StartCoroutine (handleMailEvent.LoadEvent (e=>{
				if( e != null){
					Debug.LogWarning(e.Message);
				}
				UpdateUI();
			}));
			OnOpenScreen (ui);
		}

		void OnInitShop(ShopUI ui){
			OnOpenScreen (ui);
		}

		void OnInitCapture(CaptureUI ui, string mapIdx){
			// 防呆處理
			if (GameConfig.IsMapDeveloped (mapIdx) == false) {
				mapIdx = GameConfig.MAP_IDXS [0];
			}
			// 教學模式只會跳到第一關
			if (IsTutorial) {
				mapIdx = GameConfig.MAP_IDXS [0];
			} else {
				userSettings.LastEnterCapture = new CaptureKey () {
					MapIdx = mapIdx,
					Idx = 0
				};
				userSettings.Save ();
			}

			var gameCapRoot = ui.GetGameCapRoot ();
			gameCapRoot.LoadMap (mapIdx);
			var gameCap = gameCapRoot.GetGameCap ();
			OnInitGameCap (gameCap, mapIdx, gameCapRoot);

			OnOpenScreen (ui);
		}

		void OnInitGameCap(GameCap gameCap, string mapIdx, GameCapRoot gameCapRoot){
			gameCap.PrepareMapNodeCtrl (mapIdx, nodeKey=>{
				// 還沒解鎖，給查詢一個空物件
				if(model.IsCaptureEnable(nodeKey.CaptureKey) == false){
					var empty = new GameObject().AddComponent<Capture>();
					GameObject.Destroy(empty.gameObject);
					return empty;
				}
				return model.Capture(nodeKey.CaptureKey);
			});

			{
				// 移到第一個點
				var nodeKey = new CaptureKey () {
					MapIdx = mapIdx,
					Idx = 0
				}.NodeKey;
				var node = gameCap.GetPosNode (nodeKey);
				gameCapRoot.transform.localPosition = -(node.PositionInRoot * gameCapRoot.transform.localScale.x);
			}

			StartCoroutine (FixMapTransform (useCamera, gameCapRoot.transform, gameCap.RectPosition, view.GetMainMenu().GetMainUI().WindowRect));
			UpdateUI ();
		}

		void OnInitHousePage(HousePage page){
			page.SelectHousePageMap (0);
			OnOpenScreen (page);
		}

		IEnumerator OnInitHousePageAsync(HousePage page){
			yield return page.SelectHousePageMapAsync (0);
			OnOpenScreen (page);
		}

		void OnInitWorldMapRoot(WorldMapRoot worldMapRoot, string bigMapIdx){
			worldMapRoot.LoadMap (0);
			var worldMap = worldMapRoot.GetWorldMap ();
			StartCoroutine (FixMapTransform (useCamera, worldMapRoot.transform, worldMap.RectPosition, view.GetMainMenu().GetMainUI().WindowRect));
			UpdateUI();
		}

		bool HandleHomeUI(string cmd){
			switch (cmd) {
			case "HomeUIBtnRecord":
			case "HomeUIBtnOmlet":
			case "HomeUIBtnChat":
			case "HomeUIBtnBrowsing":
				{
					Alert("Coming Soon");
				}
				break;
			case "HomeUIBtnEvent":
				{
					Alert("Coming Soon");
					// CheckEventAndOpenEventDlg ();
				}
				break;
			case "HomeUIBtnMail":
				{
#if DEMO1
                        if(true){
                            Alert("DEMO版不支援郵件");
                            return false;
                        }
#endif
                    // 領取今日禮物
                    var hasMail = handleMailEvent.UnreadMails.Count() > 0;
					if (hasMail == false) {
						Debug.LogWarning ("你沒有郵件");
						return false;
					}
					Debug.LogWarning ("直接設定第一封郵件已讀，之後應該會有Mail視窗才對");
					var mail = handleMailEvent.UnreadMails.First();

					// 設定已讀
					StartCoroutine (handleMailEvent.MarkMailRead (model, mail.ID, (err)=>{
						if(err != null){
							OnException(new ShowMessageException(err.Message, err));
							return;
						}
						try{
							var dailyGift = ItemKey.WithItemConfigID(mail.Gift);
							var dailyGiftCount = mail.GiftCount;
							// 獲得禮物
							model.AddItem (dailyGift, dailyGiftCount);
							// 先打開新禮物面板
							PushAction(()=>{
								// 打開新禮物面板
								StartCoroutine(ExecWithDownloadAssetBundleIfNeeded(()=>{
									for (var i=0; i<dailyGiftCount; ++i) {
										var newItemDlg = view.GetMainMenu().OpenNewItemDlg ();
										OnInitNewItemDlg (newItemDlg, dailyGift);
									}
								}));
							});
							if(mail.IsDailyGift){
								// 再打開每日禮物面板
								PushAction(()=>{
									// 取得領取禮物數量
									StartCoroutine(handleMailEvent.GetUserGotGiftCountInMonth(model, (e, cnt)=>{
										// 打開每日禮物面板
										StartCoroutine(ExecWithDownloadAssetBundleIfNeeded(()=>{
											var dlg = view.GetMainMenu().OpenMailGiftDlg ();
											OnInitMailGiftDlg (dlg, cnt);
										}));
									}));
								});
							}
							// 更新郵件未讀數量
							UpdateUI();
							//
							PerformActionAndRemove();
						}catch(Exception e){
							handleDebug.LogWarning("沒有這天的禮物:"+e.Message);
						}

					}));
				}
				break;
			case "HomeUIBtnLeft":
				{
					var house = view.GetMainMenu ().GetMainUI ().GetHomeUI ().GetHousePage ();
					house.PrevHousePageMap ();
					UpdateUI ();
				}
				break;
			case "HomeUIBtnRight":
				{
					var house = view.GetMainMenu ().GetMainUI ().GetHomeUI ().GetHousePage ();
					house.NextHousePageMap ();
					UpdateUI ();
				}
				break;
			case "HomeUICat_CA1":
			case "HomeUICat_CA2":
			case "HomeUICat_CA3":
			case "HomeUICat_CA4":
			case "HomeUICat_CA5":
			case "HomeUICat_CA6":
			case "HomeUICat_CA7":
			case "HomeUICat_CA8":
			case "HomeUICat_CB1":
			case "HomeUICat_CB2":
			case "HomeUICat_CB3":
			case "HomeUICat_CB4":
			case "HomeUICat_CB5":
			case "HomeUICat_CB6":
			case "HomeUICat_CB7":
			case "HomeUICat_CB8":
			case "HomeUICat_CC1":
			case "HomeUICat_CC2":
			case "HomeUICat_CC3":
			case "HomeUICat_CC4":
			case "HomeUICat_CC5":
			case "HomeUICat_CC6":
			case "HomeUICat_CC7":
			case "HomeUICat_CC8":
			case "HomeUICat_CM1":
			case "HomeUICat_CM2":
			case "HomeUICat_CM3":
			case "HomeUICat_CM4":
				{
					var catPreName = cmd.Substring ("HomeUICat_C".Length);
					var catIdx = new ItemKey(StoreCtrl.DATA_CAT, GameConfig.CatEditorID2Idx(catPreName));
					var datas = model.OwnedCats;
					var filter =
						from o in datas
						where o.StringKey == catIdx.StringKey
						select o;
					var enabled = filter.ToList().Count != 0;
					if (enabled == false) {
						return false;
					}
					Util.Instance.UnloadAsset();

					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var dlg = view.GetMainMenu ().OpenSelectCatDlg ();
						OnInitSelectCatDlg (dlg, catIdx);
					}));
				}
				break;
			case "HomeUIBtnItem":
				{
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var dlg = view.GetMainMenu ().OpenStorageDlg ();
						OnInitStorageDlg (dlg, QueryItemCount, (key) => {
							return true;
						});
					}));
				}
				break;
			default:
				return false;
			}
			return true;
		}

		void OnInitStorageDlg(ItemStorageDlgCtrl dlg, Func<ItemKey, int> queryItemCount, Func<ItemKey, bool> queryItemEnable){
			dlg.UpdateUI (langText, userSettings.Language);
			dlg.InitialDialog (queryItemCount, queryItemEnable);
		}

		void OnInitSelectCatDlg(SelectCatDlgCtrl dlg, ItemKey selectCat){
			var datas = model.OwnedCats;
			var cats =
				from data in datas
				select model.GetCat (data);
			var catsList = cats.ToList ();
			dlg.SetLanguage (langText, userSettings.Language);
			dlg.SetOwnCats (catsList);
			var indexInOrder = 0;
			for (var i = 0; i < catsList.Count; ++i) {
				var cat = catsList [i];
				if (cat.Key == selectCat.StringKey) {
					indexInOrder = i;
					break;
				}
			}
			dlg.SetCatIndexInOrder (indexInOrder);
		}

		bool HandleBuyGachaDlg(string cmd){
			switch (cmd) {
			case "BuyGachaDlgBtnBuy":
				{
					var gcDlg = view.GetMainMenu ().OpenBuyGachaDlg ();
					var cost = (int)gcDlg.MetaData ["cost"];
					var strkey = gcDlg.CurrentLockKey;
					if (model.Money < cost) {
						handleDebug.LogWarning ("你的銀票不夠:"+model.Money);
						return false;
					}
					var key = new CaptureKey (strkey);
					model.Money -= cost;
					model.EnableCapture (key);
					UpdateUI ();
					PerformCommand ("BuyGachaDlgBtnBack");

					handleAna.TrackUnlockCaptureCount (model);
				}
				break;
			case "BuyGachaDlgBtnLeft":
				{
					var dlg = view.GetMainMenu ().OpenBuyGachaDlg ();
					dlg.Left ();
					dlg.UpdateUI (langText, userSettings.Language);

					// 將"鏡頭"移到指定轉蛋台
					var strkey = dlg.CurrentLockKey;
					var capKey = new CaptureKey (strkey);
					var nodeKey = capKey.NodeKey;
					var capRoot = view.GetMainMenu ().GetMainUI ().GetCaptureUI ().GetGameCapRoot ();
					var cap = capRoot.GetGameCap ();
					var node = cap.GetPosNode (nodeKey);
					capRoot.transform.localPosition = -(node.PositionInRoot * capRoot.transform.localScale.x);
				}
				break;
			case "BuyGachaDlgBtnRight":
				{
					var dlg = view.GetMainMenu ().OpenBuyGachaDlg ();
					dlg.Right ();
					dlg.UpdateUI (langText, userSettings.Language);

					// 將"鏡頭"移到指定轉蛋台
					var strkey = dlg.CurrentLockKey;
					var capKey = new CaptureKey (strkey);
					var nodeKey = capKey.NodeKey;
					var capRoot = view.GetMainMenu ().GetMainUI ().GetCaptureUI ().GetGameCapRoot ();
					var cap = capRoot.GetGameCap ();
					var node = cap.GetPosNode (nodeKey);
					capRoot.transform.localPosition = -(node.PositionInRoot * capRoot.transform.localScale.x);
				}
				break;
			case "BuyGachaDlgBtnBack":
				{
					view.GetMainMenu ().CloseBuyGachaDlg ();
				}
				break;
			default:
				return false;
			}
			return true;
		}


		Coroutine lastCoroutine;

		bool HandleSelectCatDlg(string cmd){
			switch (cmd) {
			case "SelectCatDlgBtnBack":
				{
					if (view.PopupTracker.IsPopupActive (view.GetMainMenu ().GetSelectCatDlgCtrl ()) == false) {
						return false;
					}
					view.GetMainMenu ().CloseSelectCatDlg ();
				}
				break;
			case "SelectCatDlgBtnLeft":
				{
					if (view.PopupTracker.IsPopupActive (view.GetMainMenu ().GetSelectCatDlgCtrl ()) == false) {
						return false;
					}
					Util.Instance.UnloadAsset ();

					var dlg = view.GetMainMenu ().OpenSelectCatDlg ();
					dlg.ClearCat ();

					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						dlg.Left ();
						UpdateUI ();
					}));
				}
				break;
			case "SelectCatDlgBtnRight":
				{
					if (view.PopupTracker.IsPopupActive (view.GetMainMenu ().GetSelectCatDlgCtrl ()) == false) {
						return false;
					}
					Util.Instance.UnloadAsset ();

					var dlg = view.GetMainMenu ().OpenSelectCatDlg ();
					dlg.ClearCat ();

					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						dlg.Right ();
						UpdateUI ();
					}));
				}
				break;
			case "SelectCatDlgBtnSelect":
				{
					if (view.PopupTracker.IsPopupActive (view.GetMainMenu ().GetSelectCatDlgCtrl ()) == false) {
						return false;
					}
					var dlg = view.GetMainMenu ().OpenSelectCatDlg ();
					var selectIndex = dlg.CurrentCatIndexInOrder;
					var catId = model.OwnedCats.Skip (selectIndex).First ();
					model.SelectCatKey = catId;
					view.GetMainMenu ().CloseSelectCatDlg ();
					UpdateUI ();
				}
				break;
			case "SelectCatDlgBtnTouchStart":
				{
					if (view.PopupTracker.IsPopupActive (view.GetMainMenu ().GetSelectCatDlgCtrl ()) == false) {
						return false;
					}
					var dlg = view.GetMainMenu ().OpenSelectCatDlg ();
					var selectIndex = dlg.CurrentCatIndexInOrder;
					var catId = model.OwnedCats.Skip (selectIndex).First ();
					if (model.IsCanTouch (catId) == false) {
						handleDebug.LogWarning ("現在不能摸貓");
						return false;
					}
					handleTouchCat.StartTouch ();
					dlg.IsHandVisible = true;
					StartCoroutine (dlg.CatView.BeginTouch ());
				}
				break;
			case "SelectCatDlgBtnTouchEnd":
				{
					if (view.PopupTracker.IsPopupActive (view.GetMainMenu ().GetSelectCatDlgCtrl ()) == false) {
						return false;
					}
					if (handleTouchCat.IsTouch == false) {
						return true;
					}
					handleTouchCat.EndTouch ();
					var dlg = view.GetMainMenu ().OpenSelectCatDlg ();
					dlg.IsHandVisible = false;
					StartCoroutine (dlg.CatView.EndTouch ());
					var duration = handleTouchCat.DurationTick;
				}
				break;
			case "SelectCatDlgBtnTouchFromMain":
				{
					if (view.PopupTracker.IsPopupActive (view.GetMainMenu ().GetSelectCatDlgCtrl ()) == false) {
						return false;
					}
					var dlg = view.GetMainMenu ().OpenSelectCatDlg ();
					var selectIndex = dlg.CurrentCatIndexInOrder;
					var catId = model.OwnedCats.Skip (selectIndex).First ();
					var cat = model.GetCat (catId);
					GameConfig.CAT_STATE_ID stateId = cat.Status;
					switch (stateId) {
					case GameConfig.CAT_STATE_ID.STATE_IDLE:
						{
							// 摸貓，這會改變貓的狀態
							var result = model.TouchCat (catId);
							// 如果一直摸到生氣狀態，就直接當成摸完
							// 這樣SelectCatDlgBtnTouchEnd中的方法就不會執行也不需要執行
							// 不然動畫會切換錯
							if (result.isAngry) {
								handleTouchCat.EndTouch ();
								// 將摸貓手關掉
								dlg.IsHandVisible = false;
								UpdateUI ();
							}
							// 因為摸貓是可以連續觸發的，所以先把上一次的cancel掉
							if (lastCoroutine != null) {
								StopCoroutine (lastCoroutine);
								lastCoroutine = null;
							}
							// cancel掉後強制動畫狀態變成無，不然在視覺的睡眠中連按無法起床
							dlg.CatView.ForceState ("");
							// 再開新的動畫
							lastCoroutine = StartCoroutine (dlg.InvokeAnimationResult (result, false, ()=>{
								// do nothing
								// 為了維持摸貓的動畫
							}));
							dlg.UpdateUI ();
						}
						break;
					case GameConfig.CAT_STATE_ID.STATE_SLEEP:
						{
							var result = model.TouchCat (catId);
							StartCoroutine (dlg.InvokeAnimationResult (result, true, ()=>{
								UpdateUI ();
							}));
							// 睡眠中一次的摸貓只能觸發一次效果
							handleTouchCat.EndTouch ();
							// 將摸貓手關掉
							dlg.IsHandVisible = false;
						}
						break;
					default:
						handleDebug.LogWarning ("現在是"+stateId+"狀態，不能摸貓");
						break;
					}
				}
				break;
			case "SelectCatDlgBtnAngry":
				{
					if (view.PopupTracker.IsPopupActive (view.GetMainMenu ().GetSelectCatDlgCtrl ()) == false) {
						return false;
					}
					// 上層面板無法蓋住貓，所以將貓Hide掉
					var catDlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
					catDlg.HideCat ();
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var storage = view.GetMainMenu ().OpenStorageDlg ();
						OnInitStorageDlg (storage, QueryItemCount, FilterItemIsType (GameConfig.ITEM_TYPE.ITEM_CATCH));
						storage.MetaData = new Dictionary<string, object> () {
							{ "action", "selectAngryItem" },
						};
					}));
				}
				break;
			case "SelectCatDlgBtnWannaPlay":
				{
					if (view.PopupTracker.IsPopupActive (view.GetMainMenu ().GetSelectCatDlgCtrl ()) == false) {
						return false;
					}
					// 上層面板無法蓋住貓，所以將貓Hide掉
					var catDlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
					catDlg.HideCat ();

					var dlg = view.GetMainMenu ().OpenSelectCatDlg ();
					var selectIndex = dlg.CurrentCatIndexInOrder;
					var catId = model.OwnedCats.Skip (selectIndex).First ();
#if RESTRICT_WANNA_PLAY_ITEM1
					var cat = model.GetCat (catId);
					var wannaPlayItem = new ItemKey(cat.WannaPlayToyItemKey);
#endif

					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var storage = view.GetMainMenu ().OpenStorageDlg ();
						OnInitStorageDlg(storage, QueryItemCount, itemKey => {
#if RESTRICT_WANNA_PLAY_ITEM1
							return itemKey.StringKey == wannaPlayItem.StringKey;
#else
							if(itemKey.Type != StoreCtrl.DATA_TOY){
								return false;
							}
							var catDef = CatDef.Get(catId.Idx);
							var isMatchType = FilterItemIsCatType(catDef.Type)(itemKey);
							return isMatchType;
							/*
							var catDef = CatDef.Get(catId.Idx);
							var itemDef = ItemDef.Get(itemKey.Idx);
							switch(catDef.Type){
							case "CA":
								if(itemDef.CatTypeCA==0){
									return false;
								}
								break;
							case "CB":
								if(itemDef.CatTypeCB==0){
									return false;
								}
								break;
							case "CC":
								if(itemDef.CatTypeCC==0){
									return false;
								}
								break;
							case "CM":
								if(itemDef.CatTypeCM==0){
									return false;
								}
								break;
							}
							return true;
							*/
#endif
						});
						storage.MetaData = new Dictionary<string, object> () {
							{ "action", "selectWannaPlayItem" }
						};
					}));
				}
				break;
			case "SelectCatDlgBtnHungry":
				{
					if (view.PopupTracker.IsPopupActive (view.GetMainMenu ().GetSelectCatDlgCtrl ()) == false) {
						return false;
					}
					// 上層面板無法蓋住貓，所以將貓Hide掉
					var catDlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
					catDlg.HideCat ();

					var selectIndex = catDlg.CurrentCatIndexInOrder;
					var catId = model.OwnedCats.Skip (selectIndex).First ();
					var catDef = CatDef.Get (catId.Idx);
					var filterFn = 
						FilterItemAnd (
							FilterItemIsType (GameConfig.ITEM_TYPE.ITEM_FOOD),
							FilterItemIsCatType(catDef.Type)
						);
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var storage = view.GetMainMenu ().OpenStorageDlg ();
						OnInitStorageDlg(storage, QueryItemCount, filterFn);
						storage.MetaData = new Dictionary<string, object> () {
							{ "action", "selectFoodItem" }
						};
					}));
				}
				break;
			default:
				return false;
			}
			return true;
		}

		static Func<ItemKey, bool> FilterItemIsCatType(string catType){
			return (ItemKey itemKey) => {
				var itemDef = ItemDef.Get(itemKey.Idx);
				switch(catType){
				case "CA":
					if(itemDef.CatTypeCA==1){
						return true;
					}
					break;
				case "CB":
					if(itemDef.CatTypeCB==1){
						return true;
					}
					break;
				case "CC":
					if(itemDef.CatTypeCC==1){
						return true;
					}
					break;
				case "CM":
					if(itemDef.CatTypeCM==1){
						return true;
					}
					break;
				}
				return false;
			};
		}

		static Func<ItemKey, bool> FilterItemOr(Func<ItemKey, bool> fn1, Func<ItemKey, bool> fn2){
			return (ItemKey itemKey) => {
				return fn1(itemKey) | fn2(itemKey);
			};
		}

		static Func<ItemKey, bool> FilterItemAnd(Func<ItemKey, bool> fn1, Func<ItemKey, bool> fn2){
			return (ItemKey itemKey) => {
				return fn1(itemKey) & fn2(itemKey);
			};
		}

		static Func<ItemKey, bool> FilterItemNot(Func<ItemKey, bool> fn){
			return (ItemKey itemKey) => {
				return !fn(itemKey);
			};
		}

		static Func<ItemKey, bool> FilterItemIn(List<string> ids){
			return (ItemKey itemKey) => {
				var def = ItemDef.Get(itemKey.Idx);
				return ids.Contains(def.ID);
			};
		}

		static Func<ItemKey, bool> FilterItemIsType(GameConfig.ITEM_TYPE type){
			return (ItemKey itemKey) => {
				var itemEnable = true;
				switch (type)
				{
				case GameConfig.ITEM_TYPE.ITEM_BASE:
					// ITEM_BASE 等於是 ITEM_BASE_CAMERA + ITEM_BASE_TOY
					itemEnable &= (itemKey.Type == StoreCtrl.DATA_CAMERA || itemKey.Type == StoreCtrl.DATA_TOY_CAPTURE);
					break;
				case GameConfig.ITEM_TYPE.ITEM_EXPLORE:
					itemEnable &= (itemKey.Type == StoreCtrl.DATA_S_CAMERA);
					break;
				case GameConfig.ITEM_TYPE.ITEM_CATCH:
					itemEnable &= (itemKey.Type == StoreCtrl.DATA_S_TOY);
					break;
				case GameConfig.ITEM_TYPE.ITEM_FOOD:
					itemEnable &= (itemKey.Type == StoreCtrl.DATA_FOOD);
					break;
				case GameConfig.ITEM_TYPE.ITEM_BASE_CAMERA:
					itemEnable &= (itemKey.Type == StoreCtrl.DATA_CAMERA);
					break;
				case GameConfig.ITEM_TYPE.ITEM_BASE_TOY:
					itemEnable &= (itemKey.Type == StoreCtrl.DATA_TOY_CAPTURE);
					break;
				}
				return itemEnable;
			};
		}

		bool HandleStorageDlg(string cmd){
			switch (cmd) {
			case "StorageDlgBtnLeft":
				{
					var dlg = view.GetMainMenu ().OpenStorageDlg ();
					dlg.Left ();
				}
				break;
			case "StorageDlgBtnRight":
				{
					var dlg = view.GetMainMenu ().OpenStorageDlg ();
					dlg.Right ();
				}
				break;
			case "StorageDlgBtnBack":
				{
					var dlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
					if (dlg != null) {
						// 將貓Show回來
						dlg.ShowCat ();
					}
					view.GetMainMenu ().CloseStorageDlg ();
				}
				break;
			default:
				// 有包含StorageDlgBtn的並且已確定字串內容的cmd必須先處理才能進入這裡
				// 所以以下這段不能移到別處
				if (cmd.Contains ("StorageDlgBtn")) {
					var itemStrKey = cmd.Substring ("StorageDlgBtn".Length);
					var itemKey = new ItemKey (itemStrKey);
					var itemType = itemKey.Type;
					var itemIdx = itemKey.Idx;
					var storageDlg = view.GetMainMenu ().OpenStorageDlg ();
					var action = storageDlg.MetaData == null ? "" : (string)storageDlg.MetaData ["action"];
					switch (action) {
					case "selectCaptureItem":
						{
							var posIdx = (string)storageDlg.MetaData ["posIdx"];
							var nodeKey = new NodeKey (posIdx);
							var captureKey = nodeKey.CaptureKey;
							switch (itemType) {
							case StoreCtrl.DATA_CAMERA:
								{
									// 判斷道具能力
									var itemDef = ItemDef.Get (itemKey.Idx);
									var photos = new List<PhotoKey> ();
									// 一定可抽一般照片
									photos.AddRange (PhotoKey.Keys (captureKey.MapIdx, PhotoKey.TypeSmallPhoto));
									// 如果可抽環景片，就加入環景照
									var hasGetBigPhotoAiblity = itemDef.BPhoto > 0;
									if (hasGetBigPhotoAiblity) {
										photos.AddRange (PhotoKey.Keys (captureKey.MapIdx, PhotoKey.TypeBigPhoto));
									}
									// 判斷還有沒有照片可以抽
									var validPhotos = 
										from photo in photos
											where model.IsPhotoEnable(photo) == false
										select photo;
									// 沒有可抽的就回傳
									if (validPhotos.Count () == 0) {
										Util.Instance.LogWarning ("你已擁有這張地圖的所有照片");
										return false;
									}
									// 處理探索模式的道具使用
									if (model.ConsumeItem (itemKey) == false) {
										return false;
									}
									// 探索
									model.StartCapture (captureKey, itemKey);
									UpdateUI ();

									handleAna.TrackPlayCapture (model);
								}
								break;
							case StoreCtrl.DATA_TOY_CAPTURE:
								{
									// 處理探索模式的道具使用
									if (model.ConsumeItem (itemKey) == false) {
										return false;
									}
									// 探索
									model.StartCapture (captureKey, itemKey);
									UpdateUI ();

									handleAna.TrackPlayCapture (model);
								}
								break;
							case StoreCtrl.DATA_S_CAMERA:
								{
									// 處理探索模式的道具使用
									if (model.ConsumeItem (itemKey) == false) {
										return false;
									}
									// 加速
									model.SpeedUpCaptureWithItem(captureKey, itemKey);
									model.CheckCaptureCompleted ();
									UpdateUI ();
								}
								break;
							case StoreCtrl.DATA_S_TOY:
								{
									// 處理探索模式的道具使用
									if (model.ConsumeItem (itemKey) == false) {
										return false;
									}
									// 加速
									model.SpeedUpCaptureWithItem(captureKey, itemKey);
									model.CheckCaptureCompleted ();
									UpdateUI ();
								}
								break;
							default:
								{
									handleDebug.LogWarning ("不能使用這類的道具，請檢查程式:" + itemType);
									return false;
								}
							}
							view.GetMainMenu ().CloseStorageDlg ();
							return true;
						}
					case "selectAngryItem":
						{
							// 處理互動模式的道具使用, 貓生氣時
							view.GetMainMenu ().CloseStorageDlg ();
							var selectCatDlg = view.GetMainMenu ().OpenSelectCatDlg ();
							// 將貓Show回來
							selectCatDlg.ShowCat ();
							// 取得選取資料
							var selectCatIndex = selectCatDlg.CurrentCatIndexInOrder;
							var catId = model.OwnedCats.Skip (selectCatIndex).First();
							var catData = model.GetCat (catId);
							GameConfig.CAT_STATE_ID stateId = catData.Status;
							switch (stateId) {
							case GameConfig.CAT_STATE_ID.STATE_ANGRY:
								{
									if (itemType != StoreCtrl.DATA_S_TOY) {
										handleDebug.LogWarning ("不需用這個類別的道具");
										return false;
									}
									if (model.ConsumeItem (itemKey) == false) {
										handleDebug.LogWarning ("沒有道具了");
										return false;
									}
									var result = model.UseSToyForCat (catId, itemKey);
									/*if (result.reduceTime > 0) {
										selectCatDlg.CatView.AddPer (-50);
									}*/
									var isEndOfTime = result.isEndOfTime;
									var itemDef = ItemDef.Get (itemIdx);
									var animIdx = 0;
									if (itemDef.ID == "I31010") {
										// 貓草噴劑
										// 減少等待時間50%
										animIdx = Remix.InteractiveModeCatView.ANIM_PACIFY;
									} else if (itemDef.ID == "I31020") {
										// 罐裝乾貓草
										// 減少等待時間100%
										animIdx = Remix.InteractiveModeCatView.ANIM_PACIFY + 1;
									}
									StartCoroutine (selectCatDlg.CatView.Pacify (animIdx, isEndOfTime, 0, false, ()=>{
										UpdateUI();
									}));
									selectCatDlg.UpdateUI ();
								}
								break;
							default:
								return false;
							}
							return true;
						}
					case "selectFoodItem":
						{
							// 處理互動模式的道具使用，貓肚餓時
							view.GetMainMenu ().CloseStorageDlg ();
							var selectCatDlg = view.GetMainMenu ().OpenSelectCatDlg ();
							// 將貓Show回來
							selectCatDlg.ShowCat ();
							// 取得選取資料
							var selectCatIndex = selectCatDlg.CurrentCatIndexInOrder;
							var catId = model.OwnedCats.Skip (selectCatIndex).First();
							var catData = model.GetCat (catId);
							GameConfig.CAT_STATE_ID stateId = catData.Status;
							switch (stateId) {
							case GameConfig.CAT_STATE_ID.STATE_HUNGRY:
								{
									if (itemType != StoreCtrl.DATA_FOOD) {
										handleDebug.LogWarning ("不需用這個類別的道具");
										return false;
									}
									if (model.ConsumeItem (itemKey) == false) {
										handleDebug.LogWarning ("沒有道具了");
										return false;
									}
									var result = model.UseFoodForCat (catId, itemKey);
									StartCoroutine (selectCatDlg.InvokeAnimationResult (result, true, ()=>{
										UpdateUI ();
									}));
									selectCatDlg.UpdateUI ();
								}
								break;
							default:
								return false;
							}
							return true;
						}
					case "selectWannaPlayItem":
						{
							// 處理互動模式的道具使用，貓想玩時
							view.GetMainMenu ().CloseStorageDlg ();
							var selectCatDlg = view.GetMainMenu ().OpenSelectCatDlg ();
							// 將貓Show回來
							selectCatDlg.ShowCat ();
							// 取得選取資料
							var selectCatIndex = selectCatDlg.CurrentCatIndexInOrder;
							var catId = model.OwnedCats.Skip (selectCatIndex).First();
							var catData = model.GetCat (catId);
							GameConfig.CAT_STATE_ID stateId = catData.Status;
							switch (stateId) {
							case GameConfig.CAT_STATE_ID.STATE_WANNA_PLAY:
								{
									if (itemType != StoreCtrl.DATA_TOY) {
										handleDebug.LogWarning ("不需用這個類別的道具");
										return false;
									}
									if (model.ConsumeItem (itemKey) == false) {
										handleDebug.LogWarning ("沒有道具了");
										return false;
									}
									var result = model.UseToyForCat (catId, itemKey);
									StartCoroutine (selectCatDlg.InvokeAnimationResult (result, true, ()=>{
										UpdateUI ();
									}));
									selectCatDlg.UpdateUI ();
								}
								break;
							default:
								return false;
							}
							return true;
						}
					case "selectItemForStartGameDlg":
						{
							var startGameDlg = view.GetMainMenu ().OpenStartGameDlg ();
							startGameDlg.SetUseItem (itemKey);
							view.GetMainMenu ().CloseStorageDlg ();
							return true;
						}
					default:
						{
							switch (itemKey.Type) {
							case StoreCtrl.DATA_MG:
							case StoreCtrl.DATA_MM:
								{
									var def = ItemDef.Get (itemKey.Idx);
									// 欄位定義中負值代表增加
									model.Gold -= def.Gold;
									model.Money -= def.Money;
									model.ConsumeItem (itemKey);
									// 更新storage中的道具顯示
									storageDlg.InitialDialog (QueryItemCount, (key)=>{return true;});
									UpdateUI ();
									return true;
								}
							}
						}
						return false;
					}
				}
				return false;
			}
			return true;
		}

		bool HandleCaptureUI(string cmd){
			switch (cmd) {
			case "WorldCapRootTap":
			case "GameCapRootTap":
				{
					if (IsPositionInRect (useCamera, Input.mousePosition, view.GetMainMenu ().GetMainUI ().WindowRect) == false) {
						handleDebug.LogWarning ("請在視窗內點擊:"+cmd);
						return false;
					}
					if (view.GetMainMenu ().PopupTracker.IsPopupActive (null) == false) {
						return false;
					}
					if (IsDragMap ()) {
						Util.Instance.LogWarning ("正在拖曳地圖");
						return false;
					}
					var mouse = Input.mousePosition;
					// 取得3D空間中的世界坐標
					var world = useCamera.ScreenToWorldPoint (mouse);
					// 取得在canvas的世界座標
					// convertWolrdToCanvas才能包含到canvas被移到(0,1,90)的偏移
					var canvas = Util.Instance.convertWolrdToCanvas (world);

					var map = view.GetMainMenu ().GetMainUI ().GetCaptureUI ();
					MonoBehaviour root = null;
					RectPosition rootRect = null;
					if (cmd == "GameCapRootTap") {
						root = map.GetGameCapRoot ();
						rootRect = map.GetGameCapRoot ().GetGameCap ().RectPosition;
					} else {
						root = map.GetWorldCapRoot ();
						rootRect = map.GetWorldCapRoot ().GetWorldCap ().RectPosition;
					}
					// 直接計算點擊到的區域座標
					// 因為所有父層都是單位矩陣，所以可以這樣做
					var canvasLocal = canvas - root.transform.localPosition;
					canvasLocal.x *= 1/root.transform.localScale.x;
					canvasLocal.y *= 1/root.transform.localScale.y;

					var maxScale = GameConfig.MAX_SCALE;
					var minScale = GameConfig.MIN_SCALE;
					var shouldZoomOut = root.transform.localScale.x <= minScale;
					var scaleRate = shouldZoomOut ? maxScale : minScale;
					if (scaleRate > maxScale) {
						scaleRate = maxScale;
					}
					if (scaleRate < minScale) {
						scaleRate = minScale;
					}
					// 先將縮放點移到攝影機原點再偏移點擊的位置
					var moveTo = (-canvasLocal * scaleRate) + canvas;
					root.transform.localPosition = moveTo;

					var tmpScale = root.transform.localScale;
					tmpScale.x = scaleRate;
					tmpScale.y = scaleRate;
					root.transform.localScale = tmpScale;

					StartCoroutine (FixMapTransform (useCamera, root.transform, rootRect, view.GetMainMenu().GetMainUI().WindowRect));
				}
				break;
			default:
				{
					if (cmd.Contains ("WorldCapBtnMap_")) {
						if (IsPositionInRect (useCamera, Input.mousePosition, view.GetMainMenu ().GetMainUI ().WindowRect) == false) {
							handleDebug.LogWarning ("請在視窗內點擊:"+cmd);
							return false;
						}
						if (view.GetMainMenu ().PopupTracker.IsPopupActive (null) == false) {
							return false;
						}
						if (IsDragMap ()) {
							Util.Instance.LogWarning ("正在拖曳地圖");
							return false;
						}
						var mapIdx = cmd.Substring ("WorldCapBtnMap_".Length);
						if (GameConfig.IsMapDeveloped (mapIdx) == false) {
							Alert ("Coming Soon");
							return false;
						}
						var isUnlock = IsUnlockMap (mapIdx);
						// 阻檔地區
						if (isUnlock == false) {
							handleDebug.LogWarning ("地區還沒解鎖:"+mapIdx);
							return false;
						}
						// 自動解鎖第一個轉蛋台(雖然在大關過了會自動解鎖，這裡只是防呆)
						var autoUnlockKey = new CaptureKey () {
							MapIdx = mapIdx,
							Idx = 0
						};
						if (model.IsCaptureEnable (autoUnlockKey) == false) {
							model.EnableCapture (autoUnlockKey);
						}
						StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
							view.GetMainMenu ().ChangeToSub0 ();
							var map = view.GetMainMenu ().GetMainUI ().GetCaptureUI ();
							OnInitCapture(map, mapIdx);
						}));
						return true;
					}
					if (cmd.Contains ("CaptureUIPOS_unlockNode_")) {
						if (IsPositionInRect (useCamera, Input.mousePosition, view.GetMainMenu ().GetMainUI ().WindowRect) == false) {
							handleDebug.LogWarning ("請在視窗內點擊:"+cmd);
							return false;
						}
						if (view.GetMainMenu ().PopupTracker.IsPopupActive (null) == false) {
							return false;
						}
						if (IsDragMap ()) {
							Util.Instance.LogWarning ("正在拖曳地圖");
							return false;
						}
						var posId = cmd.Substring ("CaptureUIPOS_unlockNode_".Length);
						var nodeKey = new NodeKey (posId);
						var isUnlock = model.IsCaptureEnable (nodeKey.CaptureKey);
						if (isUnlock) {
							throw new UnityException ("程式跑到這應該還沒解鎖，請檢查");
						}
						StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
							var dlg = view.GetMainMenu ().OpenBuyGachaDlg ();
							var gcDef = GachaDef.Get (nodeKey.CaptureKey.GachaConfigID);
							dlg.MetaData = new Dictionary<string, object> () {
								{ "cost", gcDef.Unlock }
							};
							var lockCaptureKeys = new List<string> ();
							for (var i = 0; i < 3; ++i) {
								var captureKey = new CaptureKey () {
									MapIdx = nodeKey.MapIdx,
									Idx = i
								};
								var shouldIgnore = model.IsCaptureEnable (captureKey);
								if (shouldIgnore) {
									continue;
								}
								lockCaptureKeys.Add (captureKey.StringKey);
							}
							dlg.SetLockCaptureKeys (lockCaptureKeys);
							// 設定到所點擊的轉蛋台
							dlg.CurrentLockKey = new CaptureKey () {
								MapIdx = nodeKey.MapIdx,
								Idx = nodeKey.Idx
							}.StringKey;
							dlg.UpdateUI (langText, userSettings.Language);
						}));
						return true;
					}
					// 點擊探索
					if (cmd.Contains ("CaptureUIPOS_node_")) {
						if (IsPositionInRect (useCamera, Input.mousePosition, view.GetMainMenu ().GetMainUI ().WindowRect) == false) {
							handleDebug.LogWarning ("請在視窗內點擊:"+cmd);
							return false;
						}
						if (view.GetMainMenu ().PopupTracker.IsPopupActive (null) == false) {
							return false;
						}
						if (IsDragMap ()) {
							Util.Instance.LogWarning ("正在拖曳地圖");
							return false;
						}
						// 假裝在拖曳，避免連續觸發
						DoingDragMap ();

						var posId = cmd.Substring ("CaptureUIPOS_node_".Length);
						var nodeKey = new NodeKey (posId);
						// 如果是S1(Boss)關卡，則只能使用Boss的抓貓道具
						var filterBossItem = 
							nodeKey.MapIdx == "S1" ?
							FilterItemIn (new List<string> (){ "I62010" }) :
							FilterItemNot (FilterItemIn (new List<string> (){ "I62010" }));
						var filterFn = 
							FilterItemOr(
								// 所有的照像道具
								FilterItemIsType (GameConfig.ITEM_TYPE.ITEM_BASE_CAMERA),
								// 和包含或不包含Boss的補抓道具
								FilterItemAnd (
									FilterItemIsType (GameConfig.ITEM_TYPE.ITEM_BASE_TOY),
									filterBossItem
								)
							);
						StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
							var dlg = view.GetMainMenu ().OpenStorageDlg ();
							OnInitStorageDlg (dlg, QueryItemCount, filterFn);
							dlg.MetaData = new Dictionary<string, object> () {
								{ "action", "selectCaptureItem" },
								{ "posIdx", posId }
							};
						}));
						return true;
					}
					// 點擊加速
					if (cmd.Contains ("CaptureUISpeedUp_node_")) {
						if (IsPositionInRect (useCamera, Input.mousePosition, view.GetMainMenu ().GetMainUI ().WindowRect) == false) {
							handleDebug.LogWarning ("請在視窗內點擊:"+cmd);
							return false;
						}
						if (view.GetMainMenu ().PopupTracker.IsPopupActive (null) == false) {
							return false;
						}
						if (IsDragMap ()) {
							Util.Instance.LogWarning ("正在拖曳地圖");
							return false;
						}
						// 假裝在拖曳，避免連續觸發
						DoingDragMap ();
						var posId = cmd.Substring ("CaptureUISpeedUp_node_".Length);
						var nodeKey = new NodeKey (posId);

						StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
							var dlg = view.GetMainMenu ().OpenStorageDlg ();
							var capture = model.Capture (nodeKey.CaptureKey);
							var captureItem = new ItemKey (capture.ItemKey);
							dlg.InitialDialog (QueryItemCount, item => {
								switch (captureItem.Type) {
								case StoreCtrl.DATA_TOY_CAPTURE:
									return item.Type == StoreCtrl.DATA_S_TOY;
								case StoreCtrl.DATA_CAMERA:
									return item.Type == StoreCtrl.DATA_S_CAMERA;
								}
								handleDebug.LogWarning ("不該能使用這個道具來探索，程式有誤");
								return false;
							});
							dlg.MetaData = new Dictionary<string, object> () {
								{ "action", "selectCaptureItem" },
								{ "posIdx", posId }
							};
						}));
						return true;
					}
					// 點擊採收
					if (cmd.Contains ("CaptureUIOK_node_")) {
						if (IsPositionInRect (useCamera, Input.mousePosition, view.GetMainMenu ().GetMainUI ().WindowRect) == false) {
							handleDebug.LogWarning ("請在視窗內點擊:"+cmd);
							return false;
						}
						if (view.GetMainMenu ().PopupTracker.IsPopupActive (null) == false) {
							return false;
						}
						if (IsDragMap ()) {
							Util.Instance.LogWarning ("正在拖曳地圖");
							return false;
						}
						// 假裝在拖曳，避免連續觸發
						DoingDragMap ();

						var posId = cmd.Substring ("CaptureUIOK_node_".Length);
						var nodeKey = new NodeKey (posId);
						var posNode = view.GetMainMenu ().GetMainUI ().GetCaptureUI ().GetGameCapRoot().GetGameCap().GetPosNode (nodeKey);
						var captureKey = nodeKey.CaptureKey;
						var capture = model.Capture (captureKey);
						if (capture.IsShouldGetCat) {
							var catKey = capture.GetGettedCat ();
							var isNew = model.EnableCat (catKey);
							// 開啟新貓
							if (isNew == false) {
								// 開啟到相同的貓
								model.AddCatExp (catKey, GameConfig.DUPLICATE_CAT_ADD_EXP);
							}
							model.ClearCapture (captureKey);

							StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
								var dlg = view.GetMainMenu ().OpenNewCatDlg ();
								OnInitNewItemDlg(dlg, catKey);
								// 顯示new或是+500
								dlg.IsNew = isNew;
							}));
							posNode.Clear ();

						} else if (capture.IsShouldGetPhoto) {
							// 這裡的photos長度可能為0
							var photos = capture.GetGettedPhoto ();
							foreach (var photo in photos) {
								var isNew = model.IsPhotoEnable (photo) == false;
								// 開啟新照片
								model.EnablePhoto (photo);
								PushAction (GenShowItemAction(photo, isNew));

								if (isNew) {
									// 獲得銀票
									ItemKey getItem = null;
									switch (photo.Type) {
									case PhotoKey.TypeBigPhoto:
										getItem = ItemKey.WithItemConfigID (GameConfig.GET_BIG_PHOTO_REWARD);
										break;
									case PhotoKey.TypeSmallPhoto:
										getItem = ItemKey.WithItemConfigID (GameConfig.GET_SMALL_PHOTO_REWARD);
										break;
									}
									if (getItem != null) {
										model.AddItem (getItem, 1);
										PushAction (GenShowItemAction (getItem));
									}
								}
							}
							model.ClearCapture (captureKey);
							posNode.Clear ();
							UpdateUI ();

							PerformActionAndRemove ();
						} else {
							handleDebug.LogWarning ("什麼都沒發生！這不可能。請檢查程式");
							return false;
						}
						return true;
					}
					// 處理手指滑動地圖
					if (cmd.Contains ("GameCapRootTransformDoing")) {
						if (IsPositionInRect (useCamera, Input.mousePosition, view.GetMainMenu ().GetMainUI ().WindowRect) == false) {
							handleDebug.LogWarning ("請在視窗內點擊:"+cmd);
							return false;
						}
						DoingDragMap ();

						var paramId = cmd.Substring ("GameCapRootTransformDoing".Length);
						// 先將參數Pop掉再說，防止記憶體爆掉
						var param = UIEventFacade.PopParams (paramId) as ITransformGesture;
						if (view.GetMainMenu ().PopupTracker.IsPopupActive (null) == false) {
							return false;
						}
						var mapRoot = view.GetMainMenu ().GetMainUI ().GetCaptureUI ().GetGameCapRoot ();
						mapRoot.ApplyTransform (param);
						return true;
					}

					if (cmd.Contains ("WorldCapRootTransformDoing")) {
						if (IsPositionInRect (useCamera, Input.mousePosition, view.GetMainMenu ().GetMainUI ().WindowRect) == false) {
							handleDebug.LogWarning ("請在視窗內點擊:"+cmd);
							return false;
						}
						DoingDragMap ();

						var paramId = cmd.Substring ("WorldCapRootTransformDoing".Length);
						// 先將參數Pop掉再說，防止記憶體爆掉
						var param = UIEventFacade.PopParams (paramId) as ITransformGesture;
						if (view.GetMainMenu ().PopupTracker.IsPopupActive (null) == false) {
							return false;
						}
						var mapRoot = view.GetMainMenu ().GetMainUI ().GetCaptureUI ().GetWorldCapRoot ();
						mapRoot.ApplyTransform (param);
						return true;
					}
				}
				return false;
			}
			return true;
		}

#region detect drag map
		public long lastDragTime;
		// 處理拖曳中不觸發點擊
		// 在拖曳的command(TransformDoing)都要呼叫這個
		void DoingDragMap(){
			lastDragTime = DateTime.Now.Ticks;
		}
		// 在要阻檔的按鈕中先呼叫這個判斷是不是在拖曳中
		bool IsDragMap(){
			var duration = DateTime.Now - new DateTime (lastDragTime);
			return duration.TotalSeconds < 0.35;
		}
#endregion

		bool HandleGamePlayUI(string cmd){
			switch (cmd) {
			case "GamePlayUIBtn1":
				view.Game.PerformButtonCommand ("Btn01");
				break;
			case "GamePlayUIBtn2":
				view.Game.PerformButtonCommand ("Btn02");
				break;
			case "GamePlayUIBtn3":
				view.Game.PerformButtonCommand ("Btn03");
				break;
			case "GamePlayUIBtn4":
				view.Game.PerformButtonCommand ("Btn04");
				break;
			case "GamePlayUIBtn5-1":
				view.Game.PerformButtonCommand ("Btn05-1");
				break;
			case "GamePlayUIBtn5-2":
				view.Game.PerformButtonCommand ("Btn05-2");
				break;
			case "GamePlayUIBtnReplay":
				{
					var cat = model.GetCat (model.SelectCatKey);
					view.Game.SetHp (cat.Hp, cat.MaxHp);
					view.Game.PerformButtonCommand ("REPLAY");
				}
				break;
			case "GamePlayUIBtnPause":
				{
					view.Game.PerformButtonCommand ("Pause");
				}
				break;
			case "GamePlayUIBtnBackGame":
				{
					view.Game.PerformButtonCommand ("Pause");
				}
				break;
			case "GamePlayUIBtnBackMenu":
				{
					Util.Instance.UnloadAsset ();
					// 
					CloseGameMusic ();
					if (view.Game.IsDelayTool || view.Game.IsTutorial) {
						Util.Instance.Log ("DelayTool跳出");
						// 跳回地圖頁
						StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
							var menu = view.ChangeToMenu ();
							OnInitMenu (menu);

							view.GetMainMenu ().ChangeToMap ();
							var map = view.GetMainMenu ().GetMainUI ().GetMapUI ();
							var mapIdx = GameConfig.MAP_IDXS[0];
							// 取得最後一次遊玩的地圖
							if (userSettings.HasLastPlayLevel) {
								mapIdx = userSettings.LastEnterLevel.MapIdx;
							}
							OnInitMap (map, mapIdx);
							UpdateUI ();
						}));
						return true;
					}
					// 記錄貓的血量
					var catId = model.SelectCatKey;
					var cat = model.GetCat (catId);
					int finalHp = (int)view.Game.GamePlayView.GetHP ();
					cat.Hp = finalHp;
					model.Save ();
					// 讓貓離開遊戲
					model.CatExitGame (catId);
					// 跳回地圖頁
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var menu = view.ChangeToMenu ();
						OnInitMenu (menu);

						view.GetMainMenu ().ChangeToMap ();
						var map = view.GetMainMenu ().GetMainUI ().GetMapUI ();
						var mapIdx = GameConfig.MAP_IDXS[0];
						// 取得最後一次遊玩的地圖
						if (userSettings.HasLastPlayLevel) {
							mapIdx = userSettings.LastEnterLevel.MapIdx;
						}
						OnInitMap (map, mapIdx);
						UpdateUI ();
					}));
				}
				break;
			case "GamePlayUIGameEnd":
				{
					Util.Instance.UnloadAsset ();
					// 
					CloseGameMusic ();
					if (view.Game.IsDelayTool || view.Game.IsTutorial) {
						Util.Instance.Log ("DelayTool跳出");
						// 跳回地圖頁
						StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
							var menu = view.ChangeToMenu ();
							OnInitMenu (menu);

							view.GetMainMenu ().ChangeToMap ();
							var map = view.GetMainMenu ().GetMainUI ().GetMapUI ();
							var mapIdx = GameConfig.MAP_IDXS[0];
							// 取得最後一次遊玩的地圖
							if (userSettings.HasLastPlayLevel) {
								mapIdx = userSettings.LastEnterLevel.MapIdx;
							}
							OnInitMap (map, mapIdx);
							UpdateUI ();
						}));
						return true;
					}

					if (sessions.HasCurrentPlayingLevel == false) {
						OnException(new ShowMessageException ("應該在會話中要有CurrentPlayLevel，請檢查程式"));
						return false;
					}

					handleAna.TrackCurrentLevel (model);
					// 記錄貓的血量
					var catId = model.SelectCatKey;
					var cat = model.GetCat (catId);
					int finalHp = (int)view.Game.GamePlayView.GetHP ();
					cat.Hp = finalHp;
					model.Save ();
					// 讓貓離開遊戲
					model.CatExitGame (catId);
					// 計算過關
					var passLevelKey = new LevelKey(sessions.CurrentPlayingLevel);
					sessions.ClearCurrentPlayingLevel ();
					// 計算勝負
					float finalHpScale = cat.Hp * 100.0f / cat.MaxHp;
					bool isWin = (finalHpScale >= RhythmCtrl.LEVEL_THRESHOLD);
					var gamePlayModel = view.Game.GamePlayModel;
					// 4 是最差
					var rank = 4;
					ItemKey reward = null;
					var winGold = 0;
					if (isWin) {
						// 記錄過關
						model.LevelClear(passLevelKey);
						// 取得獲得金錢
						var def = LevelDef.Get (passLevelKey.ConfigID);
						winGold = def.Gold;
						model.Gold += winGold;
						// 獲得過關道具
						var isGetItem = UnityEngine.Random.Range (0, 100) <= def.ItemProbability*100;
						if (isGetItem) {
							reward = ItemKey.WithItemConfigID (def.ItemID);
							model.AddItem (reward, 1);
						}

						// 只有勝利才要算rank
						var tmp = gamePlayModel.GetBadCount () / (float)RhythmCtrl.RHYTHM_COUNT;
						if (tmp < 0.1f) {
							rank = 0;
						} else if (tmp < 0.2f) {
							rank = 1;
						} else if (tmp < 0.3f) {
							rank = 2;
						} else if (tmp < 0.4f) {
							rank = 3;
						}
					}

					// 不管勝敗。記錄得分
					var info = model.NewPassLevelInfo (passLevelKey);
					info.score = gamePlayModel.Score;
					info.rank = rank;
					model.AddPassLevelInfo (info);

					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						// 跳回地圖頁
						var menu = view.ChangeToMenu ();
						OnInitMenu (menu);

						view.GetMainMenu ().ChangeToMap ();
						var map = view.GetMainMenu ().GetMainUI ().GetMapUI ();
						var mapIdx = GameConfig.MAP_IDXS[0];
						// 取得最後一次遊玩的地圖
						if (userSettings.HasLastPlayLevel) {
							mapIdx = userSettings.LastEnterLevel.MapIdx;
						}
						OnInitMap (map, mapIdx);

						UpdateUI ();

						// 加入第一個動作
						PushAction(()=>{
							var scoreDlg = view.GetMainMenu ().OpenScoreDlgCtrl ();
							OnInitScoreDlg (
								scoreDlg, 
								passLevelKey, 
								isWin, 
								reward, 
								winGold,
								gamePlayModel.Score, 
								gamePlayModel.GetMaxComboCount (),
								rank);
						});
						// 判斷大關全過獎勵並加入相關動作
						CheckPassLevelGiftAndPushAction (passLevelKey.MapIdx);
						// 執行加入的動作
						PerformActionAndRemove();
					}));
				}
				break;
			case "GamePlayUIHpChange":
				{
					var game = view.Game;
					var hp = game.GamePlayView.GetHP ();
					// 血量為0時
					if (hp <= 0) {
						if (sessions.UsedItem != null) {
							// 若有配置道具，自動吃道具
							var useItem = new ItemKey (sessions.UsedItem);
							var def = ItemDef.Get (useItem.Idx);
							game.GamePlayView.AddHP (def.Hp, null);
							game.GamePlayView.AnimateUseLevelItem ();
							sessions.UsedItem = null;
						} else {
							// 若沒有，遊戲結束
							// 立即關閉背景音樂
							CloseGameMusic ();
							game.Step (999999, 0);
						}
					}
				}
				break;
			default:
				return false;
			}
			return true;
		}

		void CloseGameMusic(){
			handleGameBGM.StopAndUnload ();
			// free sound memory when SoundDataCtrl's Destroy
		}

		void OnLeaveGame(LevelKey passLevelKey){
			sessions.ClearCurrentPlayingLevel ();

			var catId = model.SelectCatKey;
			var cat = model.GetCat (catId);
			int finalHp = (int)view.Game.GamePlayView.GetHP ();
			// 記錄貓的血量
			cat.Hp = finalHp;
			model.Save ();
		}
			
		// 回傳null代表已經領過或是還沒達到條件
		IEnumerable<ItemKey> GetClearAllDifficultyGiftAndMarkRead(string mapIdx, int difficulty){
			var id = GameRecord.GetWmapSConfigID (mapIdx, difficulty);
			var key = id;
			if (model.IsMarkRead (key) == false) {
				var isPassAll = model.IsAllLevelDifficultyClear (mapIdx, difficulty);
				if (isPassAll) {
					model.MarkRead (key);
					var def = WmapS.Get (id);
					var itemKey = ItemKey.WithItemConfigID (def.Item);
					var ret = new List<ItemKey> ();
					for (var i = 0; i < def.Quantity; ++i) {
						ret.Add (itemKey);
					}
					return ret;
				}
			}
			return null;
		}

		void CheckPassLevelGiftAndPushAction(string mapIdx){
			var level = model.GetCurrentLevel (mapIdx).First ();
			// 如果通關同一難度所有關卡
			var difs = new List<int>(){GameRecord.Easy, GameRecord.Normal, GameRecord.Hard};
			foreach(var dif in difs){
				var getItems = GetClearAllDifficultyGiftAndMarkRead (mapIdx, dif);
				var isClearAll = getItems != null;
				if (isClearAll) {
					// 取得獎勵
					foreach (var itemKey in getItems) {
						model.AddItem (itemKey, 1);
					}
					// 顯示取得道具
					foreach (var itemKey in getItems) {
						PushAction (GenShowItemAction (itemKey));
					}
				} 
			}
			var mapId = GameConfig.MapIdx2Int (level.MapIdx);
			var levelCount = GameConfig.MAP_LEVEL_COUNT [mapId];
			// 大關全過，顯示闖關訊息
			// level.Idx從0開始算，所以levelCount要減1
			if (level.Idx > levelCount-1) {
				// 判斷有沒有觸發過
				var notifyId = string.Format ("notify_{0}", mapIdx);
				if (model.IsMarkRead (notifyId) == false) {
					// 標定觸發
					model.MarkRead (notifyId);
					// 自動解鎖下一關的第一個轉蛋台
					if (GameConfig.HasNextMapIdx (mapIdx)) {
						var autoUnlockKey = new CaptureKey () {
							MapIdx = GameConfig.NextMapIdx (mapIdx),
							Idx = 0
						};
						if (model.IsCaptureEnable (autoUnlockKey) == false) {
							model.EnableCapture (autoUnlockKey);
						}
					}
					// 顯示闖關訊息
					PushAction (() => {
						var msg = langText.GetDlgMessage(userSettings.Language, "MesgText_M03");
						Alert(msg);
					});
					// 回到地圖頁
					PushAction (() => {
						// 切換到世界地圖
						// 因為現在在MapUI中的主頁，只要切換到Sub頁就行了
						PerformCommand("MainUIBtnSub1");
					});
				}
			}
		}

		Action GenShowItemAction(ItemKey itemKey){
			return () => {
				var dlg = view.GetMainMenu ().OpenNewItemDlg ();
				OnInitNewItemDlg (dlg, itemKey); 
			};
		}

		Action GenShowItemAction(PhotoKey _photo, bool isNew){
			return ()=>{
				StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
					NewDlgCtrl dlg = null;
					switch (_photo.Type) {
					case PhotoKey.TypeBigPhoto:
						dlg = view.GetMainMenu ().OpenNewBPhotoDlg ();
						break;
					case PhotoKey.TypeSmallPhoto:
						dlg = view.GetMainMenu ().OpenNewSPhotoDlg ();
						break;
					}
					OnInitNewItemDlg(dlg, _photo);
					dlg.IsNew = isNew;
				}));
			};
		}

		void OnInitScoreDlg(ScoreDlgCtrl scoreDlg, LevelKey level, bool isWin, ItemKey reward, int gold, int score, int combo, int rank){
			Exception error = null;
			scoreDlg.SetLevelReward (reward, ref error);
			if (error != null) {
				OnException (error);
			}
			var obj = Util.Instance.GetPrefab (level.LevelPicPrefabName, null);
			scoreDlg.LevelPic = obj.GetComponent<Image> ().sprite;
			GameObject.Destroy (obj);
			scoreDlg.SetLevelScore (score);
			scoreDlg.SetGold (gold);
			scoreDlg.SetCombo (combo);
			scoreDlg.SetGameModeText (langText.GetGameModeText (userSettings.Language, level));
			scoreDlg.SetTitle (langText.GetLevelName(userSettings.Language, level));
			scoreDlg.SetDifficulty (level.Difficulty);
			scoreDlg.SetIsWin (isWin);
			scoreDlg.SetRank (rank);
		}

		bool HandleAlbumDlg(string cmd){
			switch (cmd) {
			case "AlbumDlgBtnBack":
				view.GetMainMenu ().CloseAlbumDlg ();
				break;
			default:
				{
					if (cmd.Contains ("AlbumDlgBtnPhoto_")) {
						var photoKeyStr = cmd.Substring ("AlbumDlgBtnPhoto_".Length);
						var photoKey = new PhotoKey (photoKeyStr);
						if (QueryPhotoEnable (photoKey) == false) {
							handleDebug.LogWarning ("你還沒取得照片");
							return false;
						}
						StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
							var dlg = view.GetMainMenu ().OpenPhotoDlg ();
							dlg.LoadPhoto (photoKey, QueryPhotoEnable);
							IsBannerVisible = false;
						}));
						return true;
					}
				}
				return false;
			}
			return true;
		}

		bool HandlePhotoDlg(string cmd){
			switch (cmd) {
			case "PhotoDlgBtnUp":
				{
					view.GetMainMenu ().OpenPhotoDlg ().PrevAndLoad ();
					Resources.UnloadUnusedAssets ();
				}
				break;
			case "PhotoDlgBtnDown":
				{
					view.GetMainMenu ().OpenPhotoDlg ().NextAndLoad ();
					Resources.UnloadUnusedAssets ();
				}
				break;
			case "PhotoDlgBtnBack":
				view.GetMainMenu ().ClosePhotoDlg ();
				Resources.UnloadUnusedAssets ();
				IsBannerVisible = model.IsUnlockBannerAd == false;
				break;
			default:
				return false;
			}
			return true;
		}

		bool HandleDelayToolUI(string command){
			switch(command){
			case "DelayToolUIBtnAdd":
				{
					// 確保浮點數計算不累積誤差
					/*var curr = model.AudioOffsetTime * 100;
					// 使用轉型直接把小數點去掉，不要使用Math.Floor，有些情況數值會被鎖死
					var removeDot = (int)curr;
					var finalValue = (removeDot + 1) / 100f;
					if (finalValue > 0) {
						finalValue = 0;
					}
					model.AudioOffsetTime = finalValue;*/
					var finalValue = model.AudioOffsetTime + 0.01f;
					if (finalValue > 0) {
						finalValue = 0;
					} 
					model.AudioOffsetTime = finalValue;
					UpdateUI ();
				}
				break;
			case "DelayToolUIBtnLess":
				{
					// 確保浮點數計算不累積誤差
					/*var curr = model.AudioOffsetTime * 100;
					var removeDot = (int)curr;
					var finalValue = (removeDot - 1) / 100f;
					model.AudioOffsetTime = finalValue;*/
					model.AudioOffsetTime -= 0.01f;
					UpdateUI ();
				}
				break;
			case "DelayToolUIBtnExit":
				{
					// 跳回地圖頁
					PerformCommand ("GamePlayUIBtnBackMenu");
				}
				break;
			case "DelayToolUIBtnAuto":
				view.Game.Helper.IsAutoPlay = !view.Game.Helper.IsAutoPlay;
				break;
			default:
				return false;
			}
			return true;
		}

		bool HandleIAPDlg(string cmd){
			switch (cmd) {
			case "IAPDlgBtnBack":
				view.GetMainMenu ().CloseIAPDlg ();
				break;
			default:
				if(cmd.Contains("IAPDlgBtn")){
					var key = cmd.Substring ("IAPDlgBtn".Length);
					var itemKey = new ItemKey (key);
					PurchaseIAP (itemKey);
					return true;
				}
				return false;
			}
			return true;
		}

		void OnInitTutorialDlg(TutorialDlg dlg){
			dlg.UpdateUI (langText, userSettings.Language);
		}

		bool HandleConfigUI(string cmd){
			switch (cmd) {
			case "ConfigUIBtnTutorial":
				{
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var dlg = view.GetMainMenu ().OpenTutorialDlg ();
						OnInitTutorialDlg (dlg);
					}));
				}
				break;
			case "ConfigUIBtnSync":
				{
					StartCoroutine (StartDelayToolVer2 (view));
				}
				break;
			case "ConfigUIBtnStaff":
				{
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var dlg = view.GetMainMenu ().OpenStaffDlg ();
						OnInitStaffDlg (dlg);
						UpdateUI ();
					}));
				}
				break;
			case "ConfigUIBtnLanguage":
				{
#if TAPTAP1
					var msg = langText.GetDlgMessage(userSettings.language, "MesgText_M08");
					OnShowMessageException (new ShowMessageException (msg));
					return true;
#else
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						view.OpenLanguageDlg ();
						UpdateUI ();
					}));
#endif
				}
				break;
			case "ConfigUIBtnData":
				{
#if DEMO1
                        if (true)
                        {
                            Alert("DEMO版不支援資料轉移");
                            return false;
                        }
#endif
                        StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var dlg = view.GetMainMenu ().OpenTransferDlg ();
						OnInitTransferDlg (dlg);
						UpdateUI ();
					}));
				}
				break;
			case "TransferDlgBtnExit":
				view.GetMainMenu ().CloseTransferDlg ();
				break;
			case "TransferDlgBtnOK":
				{
					var dlg = view.GetMainMenu ().OpenTransferDlg ();
					var code = dlg.TransferCode;
					var name = dlg.TransferName;
					if (code.Trim () == "" || name.Trim() == "") {
						return false;
					}
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						try{
							var json = RemixApi.Transfer (deviceData.DeviceID, code, name);
							model.Override (json);
							Alert(langText.GetDlgMessage(userSettings.Language, "MesgText_T05"));
							view.GetMainMenu().CloseTransferDlg();
							UpdateUI();
						}catch(Exception e){
							var msg = langText.FormatExceptionString(userSettings.Language, e.Message);
							throw new ShowMessageException(msg, e);
						}
					}));
				}
				break;
			case "languageDlgBtnBack":
				{
					view.CloseLanguageDlg ();

					NextFirstEnterAppStep ();
				}
				break;
			case "LanguageDlgBtnEn":
				{
					userSettings.Language = LanguageText.En;
					userSettings.Save ();
					UpdateUI ();
					// 一開始選完語言後，就自動關閉面板
					if (NextFirstEnterAppStep ()) {
						view.CloseLanguageDlg ();
					}
				}
				break;
			case "LanguageDlgBtnCh":
				{
					userSettings.Language = LanguageText.Ch;
					userSettings.Save ();
					UpdateUI ();
					// 一開始選完語言後，就自動關閉面板
					if (NextFirstEnterAppStep ()) {
						view.CloseLanguageDlg ();
					}
				}
				break;
			case "LanguageDlgBtnCs":
				{
					userSettings.Language = LanguageText.Chs;
					userSettings.Save ();
					UpdateUI ();
					// 一開始選完語言後，就自動關閉面板
					if (NextFirstEnterAppStep ()) {
						view.CloseLanguageDlg ();
					}
				}
				break;
			default:
				return false;
			}
			return true;
		}

		void OnInitStaffDlg(StaffDlg dlg){
			// nothing to do
		}

		IObservable<string[]> shouldCheck = null;
		void OnInitTransferDlg(TransferDlg dlg){
			dlg.SetText (langText, userSettings.Language);
			dlg.NowId = deviceData.TransferCode;
			dlg.IsOkVisible = false;
			var idchange = dlg.OnIDValueChangeEvent.Throttle (TimeSpan.FromSeconds (1)).DistinctUntilChanged ();
			var namechange = dlg.OnNameValueChangeEvent.Throttle (TimeSpan.FromSeconds (1)).DistinctUntilChanged ();
			shouldCheck = Observable.CombineLatest (
				idchange, namechange, 
				(id, name) => {
					return new string[]{id, name};
				}
			);
			shouldCheck.Subscribe (
				info => {
					var code = info[0];
					var name = info[1];
					try{
						var isValid = RemixApi.CheckTransfer(deviceData.DeviceID, code, name);
						dlg.IsOkVisible = isValid;
					}catch(Exception e){
						OnException(e);
					}
				},
				e => {
					handleDebug.LogError(e.Message);
				}
			);
		}

		bool HandleEventDlg(string cmd){
			switch (cmd) {
			case "EventDlgBtnGo":
				{
					var dlg = view.GetMainMenu ().GetTopEventDlg ();
					var id = (string)dlg.MetaData ["id"];
					var evtId = (int)dlg.MetaData ["evtId"];
					var md = MapDef.Get (id);
					var evt = handleMailEvent.EventWithID(evtId);
					switch (evt.ConditionID) {
					case 1:
					case 2:
						if(HandleMailEvent.IsInSatSunDay(DateTime.Now) == false){
							handleDebug.LogWarning ("事件條件沒有達成");
							return false;
						}
						try{					
							// 讀取指定地圖
							view.GetMainMenu ().GetMainUI ().GetMapUI ().GetGameMapRoot ().LoadMap (md.ID);
						}catch(Exception e){
							handleDebug.LogWarning ("指定地圖沒有完成，取消動作:"+e.Message);
						}finally{
							// 關閉事件視窗
							view.GetMainMenu ().CloseEventDlg (view.GetMainMenu ().GetTopEventDlg ());
						}
						break;
					default:
						view.GetMainMenu ().CloseEventDlg (view.GetMainMenu ().GetTopEventDlg ());
						break;
					}
					handleMailEvent.MarkEventRead (evtId);
					UpdateUI ();
				}
				break;
			case "EventDlgBtnBack":
			case "EventDlgBtnExit":
				{
					if (view.GetMainMenu ().popupTracker.IsPopupActive (view.GetMainMenu ().GetTopEventDlg ()) == false) {
						return false;
					}
					var dlg = view.GetMainMenu ().GetTopEventDlg ();
					var evtId = (int)dlg.MetaData ["evtId"];

					view.GetMainMenu ().CloseEventDlg (dlg);

					handleMailEvent.MarkEventRead (evtId);
					UpdateUI ();
				}
				break;
			default:
				return false;
			}
			return true;
		}

		bool HandleDownloadDlg(string cmd){
			switch (cmd) {
			case "DownloadDlgBtnBack":
				view.CloseDownloadDlg ();
				break;
			case "DownloadDlgBtnLeft":
				{
					var dlg = view.OpenDownloadDlg ();
					dlg.Left ();
					dlg.UpdateUI (langText, userSettings.Language);
				}
				break;
			case "DownloadDlgBtnRight":
				{
					var dlg = view.OpenDownloadDlg ();
					dlg.Right ();
					dlg.UpdateUI (langText, userSettings.Language);
				}
				break;
			default:
				return false;
			}
			return true;
		}

		bool HandleTutorialDlg(string cmd){
			switch (cmd) {
			case "TutorialDlgBtnBack":
				view.GetMainMenu ().CloseTutorialDlg ();
				break;
			case "TutorialDlgBtnLeft":
				{
					var dlg = view.GetMainMenu ().OpenTutorialDlg ();
					dlg.Left ();
					dlg.UpdateUI (langText, userSettings.Language);
				}
				break;
			case "TutorialDlgBtnRight":
				{
					var dlg = view.GetMainMenu ().OpenTutorialDlg ();
					dlg.Right ();
					dlg.UpdateUI (langText, userSettings.Language);
				}
				break;
			default:
				return false;
			}
			return true;
		}

		bool HandleStageItemDlg(string cmd){
			switch (cmd) {
			case "StageItemDlgBtnBack":
				view.GetMainMenu ().CloseStageItemDlg ();
				break;
			case "StageItemDlgBtnLeft":
				{
					var dlg = view.GetMainMenu ().OpenStageItemDlg ();
					dlg.Left ();
					dlg.UpdateUI (langText, userSettings.Language);
				}
				break;
			case "StageItemDlgBtnRight":
				{
					var dlg = view.GetMainMenu ().OpenStageItemDlg ();
					dlg.Right ();
					dlg.UpdateUI (langText, userSettings.Language);
				}
				break;
			default:
				return false;
			}
			return true;
		}

		bool HandleOthers(string cmd){
			switch (cmd) {
			case "GameTrainingGameEnd":
				{
					// 這裡很重要，因為呼叫GamePlayUIGameEnd後又會回到地圖頁而叫用OnInitMap
					// 又會再次觸發TriggerTutorialIfNeed
					model.MarkRead (GamePlayTutorialFlag);
					PerformCommand ("GamePlayUIGameEnd");
				}
				break;
			case "TutorialModelEnd":
				StopTutorialModel ();
				break;
			case "MessageDlgBtnOK":
				{
					var dlg = view.GetTopMessageDlg ();
					view.CloseMessageDlg (dlg);

					PerformActionAndRemove ();
					NextFirstEnterAppStep ();
				}
				break;
			case "StaffDlgBtnBack":
				view.GetMainMenu ().CloseStaffDlg ();
				break;
			case "MailGiftDlgBtnBack":
				{
					view.GetMainMenu ().CloseMailGiftDlg ();

					PerformActionAndRemove ();
				}
				break;
			case "ScoreDlgBtnBack":
			case "ScoreDlgBtnExit":
				{
					view.GetMainMenu ().CloseScoreDlg ();

					PerformActionAndRemove ();
				}
				break;
			case "NewItemDlgBtnOK":
				{
					if (view.PopupTracker.IsPopupActive (view.GetMainMenu ().GetTopNewItemDlg ()) == false) {
						return false;
					}
					view.GetMainMenu ().CloseNewItemDlg (view.GetMainMenu ().GetTopNewItemDlg ());

					PerformActionAndRemove ();
				}
				break;
			case "NewBPhotoDlgBtnOK":
				{
					if (view.PopupTracker.IsPopupActive (view.GetMainMenu ().GetTopNewBPhotoDlg ()) == false) {
						return false;
					}
					view.GetMainMenu ().CloseNewBPhotoDlg (view.GetMainMenu ().GetTopNewBPhotoDlg ());

					PerformActionAndRemove ();
				}
				break;
			case "NewSPhotoDlgBtnOK":
				{
					if (view.PopupTracker.IsPopupActive (view.GetMainMenu ().GetTopNewSPhotoDlg ()) == false) {
						return false;
					}
					view.GetMainMenu ().CloseNewSPhotoDlg (view.GetMainMenu ().GetTopNewSPhotoDlg ());

					PerformActionAndRemove ();
				}
				break;
			case "NewCatDlgBtnOK":
				{
					view.GetMainMenu ().CloseNewCatDlg ();

					PerformActionAndRemove ();
				}
				break;
			case "GameMapRootTransformEnd":
				{
					if (view.GetMainMenu () == null) {
						break;
					}
					if (view.GetMainMenu ().GetMainUI () == null) {
						break;
					}
					if (view.GetMainMenu ().GetMainUI ().IsInMapUI () == false) {
						break;
					}
					var map = view.GetMainMenu ().GetMainUI ().GetMapUI ();
					StartCoroutine (FixMapTransform (useCamera, map.GetGameMapRoot().transform, map.GetGameMapRoot().GetGameMap().RectPosition, view.GetMainMenu().GetMainUI().WindowRect));
				}
				break;
			case "WorldMapRootTransformEnd":
				{
					if (view.GetMainMenu () == null) {
						break;
					}
					if (view.GetMainMenu ().GetMainUI () == null) {
						break;
					}
					if (view.GetMainMenu ().GetMainUI ().IsInMapUI () == false) {
						break;
					}
					var map = view.GetMainMenu ().GetMainUI ().GetMapUI ();
					StartCoroutine (FixMapTransform (useCamera, map.GetWorldMapRoot().transform, map.GetWorldMapRoot().GetWorldMap().RectPosition, view.GetMainMenu().GetMainUI().WindowRect));
				}
				break;
			case "GameCapRootTransformEnd":
				{
					if (view.GetMainMenu () == null) {
						break;
					}
					if (view.GetMainMenu ().GetMainUI () == null) {
						break;
					}
					if (view.GetMainMenu ().GetMainUI ().IsInCaptureUI () == false) {
						break;
					}
					var map = view.GetMainMenu ().GetMainUI ().GetCaptureUI ();
					StartCoroutine (FixMapTransform (useCamera, map.GetGameCapRoot().transform, map.GetGameCapRoot().GetGameCap().RectPosition, view.GetMainMenu().GetMainUI().WindowRect));
				}
				break;
			case "WorldCapRootTransformEnd":
				{
					if (view.GetMainMenu () == null) {
						break;
					}
					if (view.GetMainMenu ().GetMainUI () == null) {
						break;
					}
					if (view.GetMainMenu ().GetMainUI ().IsInCaptureUI () == false) {
						break;
					}
					var map = view.GetMainMenu ().GetMainUI ().GetCaptureUI ();
					StartCoroutine (FixMapTransform (useCamera, map.GetWorldCapRoot().transform, map.GetWorldCapRoot().GetWorldCap().RectPosition, view.GetMainMenu().GetMainUI().WindowRect));
				}
				break;
			default:
				return false;
			}
			return true;
		}

		IEnumerator FixMapTransform(Camera useCamera, Transform rootTransform, RectPosition rootRect, RectPosition windowRect){
			while (true) {
				try{
					var scale = 1f;
					var offset = Vector3.zero;
					ShouldBoundGameMapRoot (useCamera, rootTransform, rootRect, windowRect, ref scale, ref offset);
					if (scale > 1) {
						rootTransform.localScale *= scale;
					} else {
						break;
					}
				}catch(MissingReferenceException){
					handleDebug.LogWarning ("修正地圖邊界時被換頁了，這個錯誤可以不必理它");
					break;
				}
				yield return null;
			}
			while (true) {
				try{
					var scale = 1f;
					var offset = Vector3.zero;
					ShouldBoundGameMapRoot (useCamera, rootTransform, rootRect, windowRect, ref scale, ref offset);
					if (offset.magnitude > 0) {
						rootTransform.localPosition += offset * 0.3f;
					} else {
						break;
					}
				}catch(MissingReferenceException){
					handleDebug.LogWarning ("修正地圖邊界時被換頁了，這個錯誤可以不必理它");
					break;
				}
				yield return null;
			}
		}

		void OnInitLoginDlg(LoginDlg dlg){
			dlg.Title = langText.GetDlgNote(userSettings.Language, "LoginDlg");
		}

		bool HandleMusicDlg(string cmd){
			switch (cmd) {
			case "MusicDlgBtnPlay":
				{
					var dlg = view.GetMainMenu ().GetMusicDlg ();
					var musicId = dlg.CurrentMusicId;
					if (handleMp3Player.ReadyToPlay (musicId) == false) {
						Func<string, Action<Exception>> genOnDone = (closureMusicId)=>{
							return (e)=>{
								if(e!=null){
									OnException(new ShowMessageException(e.Message));
									return;
								}
#if UNITY_EDITOR
								Alert("不支援本地播放。請用實機測試");
#endif
								handleBGM.Stop ();
								handleMp3Player.Play (closureMusicId);
							};
						};
						handleMp3Player.StartBackgroundDownloadFile (musicId, genOnDone(musicId));
						UpdateUI ();
						return true;
					}
#if UNITY_EDITOR
					Alert("不支援本地播放。請用實機測試");
#endif
					handleBGM.Stop ();
					handleMp3Player.Play (musicId);
				}
				break;
			case "MusicDlgBtnBack":
				{
					handleMp3Player.Pause ();
					view.GetMainMenu ().CloseMusicDlg ();
					Util.Instance.UnloadAsset ();

					handleBGM.RequestPlay (HandleBGM.MainUI);
				}
				break;
			case "MusicDlgBtnForward":
				{
					var dlg = view.GetMainMenu ().GetMusicDlg ();
					dlg.Next ();
					UpdateUI ();
				}
				break;
			case "MusicDlgBtnBackward":
				{
					var dlg = view.GetMainMenu ().GetMusicDlg ();
					dlg.Prev ();
					UpdateUI ();
				}
				break;
			default:
				return false;
			}
			return true;
		}

		void OnInitMusicDlg(MusicDlg dlg){
			var ids = new List<string> ();
			for (var i = 0; i < MusicNoteCht.ID_COUNT; ++i) {
				ids.Add (MusicNoteCht.Get (i).ID);
			}
			dlg.SetMusicIds (ids);
			UpdateUI ();
		}

		void OnInitStageItemDlg(StageItemDlg dlg){
			dlg.UpdateUI (langText, userSettings.Language);
		}

		bool HandleShopUI(string cmd){
			switch (cmd) {
			case "InviteDlgBtnName":
				{
					var dlg = view.OpenLoginDlg ();
					OnInitLoginDlg (dlg);
					dlg.NameText = deviceData.Name;
					dlg.MetaData ["action"] = "changeName";
				}
				break;
			case "InviteDlgBtnExit":
				view.GetMainMenu ().CloseInviteDlg ();
				break;
			case "InviteDlgBtnOK":
				{
					var dlg = view.GetMainMenu ().OpenInviteDlg ();
					var inviteCode = dlg.InviteCode;
					if (string.IsNullOrEmpty (inviteCode)) {
						handleDebug.LogWarning ("請輸入inviteCode");
						return false;
					}
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						try{
							RemixApi.Invite (deviceData.DeviceID, inviteCode);
							view.GetMainMenu().CloseInviteDlg();
							Alert(langText.GetDlgMessage(userSettings.Language, "MesgText_M01"));
						}catch(Exception e){
							var msg = langText.FormatExceptionString(userSettings.Language, e.Message);
							Alert(msg);
						}
					}));
				}
				break;
			case "ShopUIBtnFSMusic":
				{
#if DEMO1
                        if(true){
                            Alert("DEMO版本不支援音樂");
                            return false;
                        }
#endif
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var dlg = view.GetMainMenu ().OpenMusicDlg ();
						OnInitMusicDlg (dlg);
					}));
				}
				break;
			case "ShopUIBtnFSStage":
				{
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var dlg = view.GetMainMenu ().OpenStageItemDlg();
						OnInitStageItemDlg (dlg);
					}));
				}
				break;
			case "ShopUIBtnFSPlay":
			case "ShopUIBtnFSVideo":
				{
					Alert("Coming Soon");
					return false;
				}
			case "ShopUIBtnRestore":
				{
					if (view.PopupTracker.IsPopupActive (null) == false) {
						return false;
					}
					RestoreIAP ();
				}
				break;
			case "ShopUIBtnInviteFriends":
				{
#if DEMO1
                        if (true)
                        {
                            Alert("DEMO版不支援邀請");
                            return false;
                        }
#endif
                        StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var dlg = view.GetMainMenu ().OpenInviteDlg ();
						OnInitInviteDlg (dlg);
						UpdateUI ();
					}));
				}
				break;
			case "ShopUIBtnADBouns":
				{
					if (view.PopupTracker.IsPopupActive (null) == false) {
						return false;
					}
					var dlg = view.OpenLoadingDlg ();
					OnInitLoadingDlg (dlg);
					LoadRewardAd ();
				}
				break;
			case "ShopUIBtnPayItems":
				{
					if (view.PopupTracker.IsPopupActive (null) == false) {
						return false;
					}
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var dlg = view.GetMainMenu ().OpenIAPDlg ();
						OnInitIAPDlg (dlg);
					}));
				}
				break;
			case "ShopUIBtnBuyCamera":
				{
					if (view.PopupTracker.IsPopupActive (null) == false) {
						return false;
					}
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var dlg = view.GetMainMenu ().OpenBuyItemDlg ();
						dlg.SetLanguage (langText, userSettings.Language);
						dlg.InitDlgType (BuyItemDlgCtrl.TypeCamera, QueryItemCount);
					}));
				}
				break;
			case "ShopUIBtnBuyToys":
				{
					if (view.PopupTracker.IsPopupActive (null) == false) {
						return false;
					}
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var dlg = view.GetMainMenu ().OpenBuyItemDlg ();
						dlg.SetLanguage (langText, userSettings.Language);
						dlg.InitDlgType (BuyItemDlgCtrl.TypeToy, QueryItemCount);
					}));
				}
				break;
			case "ShopUIBtnBuyCattree":
				{
					if (view.PopupTracker.IsPopupActive (null) == false) {
						return false;
					}
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var dlg = view.GetMainMenu ().OpenBuyItemDlg ();
						dlg.SetLanguage (langText, userSettings.Language);
						dlg.InitDlgType (BuyItemDlgCtrl.TypeCToy, QueryItemCount);
					}));
				}
				break;
			case "ShopUIBtnBuyFood":
				{
					if (view.PopupTracker.IsPopupActive (null) == false) {
						return false;
					}
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var dlg = view.GetMainMenu ().OpenBuyItemDlg ();
						dlg.SetLanguage (langText, userSettings.Language);
						dlg.InitDlgType (BuyItemDlgCtrl.TypeFood, QueryItemCount);
					}));
				}
				break;
			default:
				return false;
			}
			return true;
		}

		IDisposable inviteCodeSub;

		void OnInitInviteDlg(InviteDlg dlg){
			dlg.SetText (langText, userSettings.Language);
			dlg.IsOkVisible = false;
			dlg.NowInviteCode = deviceData.InviteCode;
			dlg.Name = deviceData.Name;
			try{
				dlg.InviteCount = RemixApi.InviteCount (deviceData.DeviceID);
			}catch(Exception e){
				OnException (new ShowMessageException(e.Message, e));
			}
			// 記得把之前的監聽刪除
			if (inviteCodeSub != null) {
				inviteCodeSub.Dispose ();
			}
			var shouldCheckInviteCode = dlg.OnInviteCodeValueChangeEvent.Throttle (TimeSpan.FromSeconds (1)).DistinctUntilChanged();
			inviteCodeSub = shouldCheckInviteCode.Subscribe (
				inviteCode => {
					try{
						var isValid = RemixApi.CheckInvite(deviceData.DeviceID, inviteCode);
						// 判斷面版是否被關閉了，沒被關閉才要更新文字
						if(view.IsInMenu()){
							var inviteDlg = view.GetMainMenu().GetInviteDlg();
							if(inviteDlg!=null){
								inviteDlg.IsOkVisible = isValid;
							}
						}
					}catch(Exception e){
						OnException(new ShowMessageException(e.Message, e));
					}
				},
				e => {
					OnException(new ShowMessageException(e.Message, e));	
				}
			);
		}

		void OnException(Exception e){
			view.CloseLoadingDlg ();
			handleDebug.LogError (e.Message);
			if (e is ShowMessageException) {
				OnShowMessageException (e as ShowMessageException);
			} else {
				Alert (e.Message);
				/* 本來打算只顯示"程式錯誤"
				var msg = langText.GetDlgMessage (userSettings.Language, "MesgText_M07");
				OnShowMessageException (new ShowMessageException(msg, e));
				*/
			}
#if UNITY_EDITOR
			throw e;
#endif
		}

		bool HandleBuyItemDlg(string cmd){
			if (view.IsInMenu () == false) {
				return false;
			}
			// 這個Popup在最上層時才執行
			// 這行只是示範，可以不加
			// 因為遊戲中在這個Popup之上沒有Popup了
			if (view.GetMainMenu ().PopupTracker.IsPopupActive (view.GetMainMenu ().BuyItemDlg) == false) {
				return false;
			}
			switch (cmd) {
			case "BuyItemDlgBtnBack":
				view.GetMainMenu ().CloseBuyItemDlg ();
				break;
			case "BuyItemDlgBtnLeft":
				view.GetMainMenu ().BuyItemDlg.Left (QueryItemCount);
				break;
			case "BuyItemDlgBtnRight":
				view.GetMainMenu ().BuyItemDlg.Right (QueryItemCount);
				break;
			case "BuyItemDlgBtnBuy":
				{
					ItemKey itemKey = view.GetMainMenu ().BuyItemDlg.CurrentItemData ();
					var itemDef = ItemDef.Get (itemKey.Idx);
					int costGold = itemDef.Gold;
					int costMoney = itemDef.Money;
					if (model.Gold < costGold) {
						handleDebug.LogWarning ("沒有金幣了");
						return false;
					}
					if (model.Money < costMoney) {
						handleDebug.LogWarning ("沒有銀票了");
						return false;
					}
					model.AddItem (itemKey, 1);
					model.Gold -= costGold;
					model.Money -= costMoney;
					view.GetMainMenu ().BuyItemDlg.SetItemData (view.GetMainMenu ().BuyItemDlg.CurrentItemIdx (), QueryItemCount);
					UpdateUI ();
				}
				break;
			default:
				return false;
			}
			return true;
		}

		bool HandleMapUI(string cmd){
			switch (cmd) {
			case "MapUIBtnAdicon":
				{
					if (IsDragMap ()) {
						Util.Instance.LogWarning ("正在拖曳地圖");
						return false;
					}
					var dlg = view.OpenLoadingDlg ();
					OnInitLoadingDlg (dlg);
					LoadRewardAd ();
				}
				break;
			case "WorldMapRootTap":
			case "GameMapRootTap":
				{
					if (IsPositionInRect (useCamera, Input.mousePosition, view.GetMainMenu ().GetMainUI ().WindowRect) == false) {
						handleDebug.LogWarning ("請在視窗內點擊:"+cmd);
						return false;
					}
					if (view.GetMainMenu ().PopupTracker.IsPopupActive (null) == false) {
						return false;
					}
					if (IsDragMap ()) {
						Util.Instance.LogWarning ("正在拖曳地圖");
						return false;
					}
					var mouse = Input.mousePosition;
					// 取得3D空間中的世界坐標
					var world = useCamera.ScreenToWorldPoint (mouse);
					// 取得在canvas的世界座標
					// convertWolrdToCanvas才能包含到canvas被移到(0,1,90)的偏移
					var canvas = Util.Instance.convertWolrdToCanvas (world);

					var map = view.GetMainMenu ().GetMainUI ().GetMapUI ();
					MonoBehaviour root = null;
					RectPosition rootRect = null;
					if (cmd == "GameMapRootTap") {
						root = map.GetGameMapRoot ();
						rootRect = map.GetGameMapRoot ().GetGameMap ().RectPosition;
					} else {
						root = map.GetWorldMapRoot ();
						rootRect = map.GetWorldMapRoot ().GetWorldMap ().RectPosition;
					}
					// 直接計算點擊到的區域座標
					// 因為所有父層都是單位矩陣，所以可以這樣做
					var canvasLocal = canvas - root.transform.localPosition;
					canvasLocal.x *= 1/root.transform.localScale.x;
					canvasLocal.y *= 1/root.transform.localScale.y;

					var maxScale = GameConfig.MAX_SCALE;
					var minScale = GameConfig.MIN_SCALE;
					var shouldZoomOut = root.transform.localScale.x <= minScale;
					var scaleRate = shouldZoomOut ? maxScale : minScale;
					if (scaleRate > maxScale) {
						scaleRate = maxScale;
					}
					if (scaleRate < minScale) {
						scaleRate = minScale;
					}
					// 先將縮放點移到攝影機原點再偏移點擊的位置
					var moveTo = (-canvasLocal * scaleRate) + canvas;
					root.transform.localPosition = moveTo;

					var tmpScale = root.transform.localScale;
					tmpScale.x = scaleRate;
					tmpScale.y = scaleRate;
					root.transform.localScale = tmpScale;

					StartCoroutine (FixMapTransform (useCamera, root.transform, rootRect, view.GetMainMenu().GetMainUI().WindowRect));
				}
				break;
			default:
				{
					// 點擊世界地圖點
					if (cmd.Contains ("WorldMapBtnMap_")) {
						if (IsPositionInRect (useCamera, Input.mousePosition, view.GetMainMenu ().GetMainUI ().WindowRect) == false) {
							handleDebug.LogWarning ("請在視窗內點擊:"+cmd);
							return false;
						}
						if (view.GetMainMenu ().PopupTracker.IsPopupActive (null) == false) {
							return false;
						}
						if (IsDragMap ()) {
							Util.Instance.LogWarning ("正在拖曳地圖");
							return false;
						}
						var mapIdx = cmd.Substring ("WorldMapBtnMap_".Length);
						if (GameConfig.IsMapDeveloped (mapIdx) == false) {
							Alert ("Coming Soon");
							return false;
						}
						// 阻檔地區
						var isUnlock = IsUnlockMap(mapIdx);
						if (isUnlock == false) {
							handleDebug.LogWarning ("地區還沒解鎖:"+mapIdx);
							return false;
						}
						StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
							view.GetMainMenu ().ChangeToSub0 ();
							var map = view.GetMainMenu ().GetMainUI ().GetMapUI ();
							OnInitMap(map, mapIdx);
						}));
						return true;
					}

					// 點擊區域地圖點
					if (cmd.Contains ("MapUIGameMapBtn_")) {
						if (IsPositionInRect (useCamera, Input.mousePosition, view.GetMainMenu ().GetMainUI ().WindowRect) == false) {
							handleDebug.LogWarning ("請在視窗內點擊:"+cmd);
							return false;
						}
						if (view.GetMainMenu ().PopupTracker.IsPopupActive (null) == false) {
							return false;
						}
						if (IsDragMap ()) {
							Util.Instance.LogWarning ("正在拖曳地圖");
							return false;
						}
						// 讓貓進入遊戲
						var isCanEnterGame = model.IsCatCanEnterGame (model.SelectCatKey);
						// 如果不能進入遊戲，就跳到互動模式
						if (isCanEnterGame == false) {
							PerformCommand ("MainUIBtnCatIcon");
							return false;
						}
						var posId = cmd.Substring ("MapUIGameMapBtn_".Length);
						var nodeKey = new NodeKey (posId);
						var levelKey = nodeKey.LevelKeys.First ();
						// 阻檔關卡
						var currLevel = model.GetCurrentLevel(nodeKey.MapIdx).First();
						if (levelKey.IsSurpass (currLevel)) {
							Util.Instance.LogWarning ("關卡還沒解鎖:"+levelKey.Idx);
							return false;
						}
						// 下載AssetBundle
						var bundles = new List<string> ();
						// 注意：下載的AssetBundle都要小寫
						// 但即使弄錯成大寫而導致讀取失敗
						// 也會在OnShowMessageException中將IsDownloadPackageOKFlag設為False
						// 重開遊戲時強制重新下載AssetBundle，而那個流程會剛好避免大小寫問題
						var levelBundle = string.Format ("level/m{0}l{1:00}", levelKey.MapIdx.ToLower(), levelKey.Idx+1);
						bundles.Add (levelBundle);
						// 開啟開始遊戲面板
						StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
							var dlg = view.GetMainMenu ().OpenStartGameDlg ();
							OnInitStartGameDlg (dlg, levelKey);
						}, bundles));
						return true;
					}
					// 處理手指滑動地圖
					if (cmd.Contains ("GameMapRootTransformDoing")) {
						if (IsPositionInRect (useCamera, Input.mousePosition, view.GetMainMenu ().GetMainUI ().WindowRect) == false) {
							handleDebug.LogWarning ("請在視窗內點擊:"+cmd);
							return false;
						}
						DoingDragMap ();

						var paramId = cmd.Substring ("GameMapRootTransformDoing".Length);
						// 先將參數Pop掉再說，防止記憶體爆掉
						var param = UIEventFacade.PopParams (paramId) as ITransformGesture;
						if (view.GetMainMenu ().PopupTracker.IsPopupActive (null) == false) {
							return false;
						}
						var mapRoot = view.GetMainMenu ().GetMainUI ().GetMapUI ().GetGameMapRoot ();
						mapRoot.ApplyTransform (param);
						return true;
					}

					if (cmd.Contains ("WorldMapRootTransformDoing")) {
						if (IsPositionInRect (useCamera, Input.mousePosition, view.GetMainMenu ().GetMainUI ().WindowRect) == false) {
							handleDebug.LogWarning ("請在視窗內點擊:"+cmd);
							return false;
						}
						DoingDragMap ();

						var paramId = cmd.Substring ("WorldMapRootTransformDoing".Length);
						// 先將參數Pop掉再說，防止記憶體爆掉
						var param = UIEventFacade.PopParams (paramId) as ITransformGesture;
						if (view.GetMainMenu ().PopupTracker.IsPopupActive (null) == false) {
							return false;
						}
						var mapRoot = view.GetMainMenu ().GetMainUI ().GetMapUI ().GetWorldMapRoot ();
						mapRoot.ApplyTransform (param);
						return true;
					}
				}
				return false;
			}
			return true;
		}
		// test function, no use
		IEnumerator OnInitStartGameDlgAsync(StartGameDlgCtrl dlg, LevelKey levelKey){
			view.OpenLoadingDlg ();

			var lang = userSettings.Language;
			dlg.SetTitle (levelKey.Idx + "");

			var obj = new RemixApi.Either<GameObject> ();
			yield return Util.Instance.GetPrefabAsync (obj, levelKey.LevelPicPrefabName, null);
			if (obj.Exception != null) {
				view.CloseLoadingDlg ();
				OnException (new ShowMessageException (obj.Exception.Message, obj.Exception));
				yield break;
			}
			dlg.SetLevelImage (obj.Ref.GetComponent<Image> ().sprite);
			GameObject.Destroy (obj.Ref);

			dlg.SetGameModeText (langText.GetGameModeText (lang, levelKey));
			dlg.SetTitle (langText.GetLevelName (lang, levelKey));
			dlg.SetArtistTitle (langText.GetArtistTitle (lang, levelKey));
			dlg.SetSongTitle (langText.GetSongTitle (lang, levelKey));
			dlg.SetUseItem (null);

			var easyRankInfo = model.GetMaxRankLevelInfo(levelKey.ChangeDifficulty(GameRecord.Easy));
			var normalRankInfo = model.GetMaxRankLevelInfo(levelKey.ChangeDifficulty(GameRecord.Normal));
			var hardRankInfo = model.GetMaxRankLevelInfo(levelKey.ChangeDifficulty(GameRecord.Hard));

			dlg.SetHistoryEasyRank (easyRankInfo == null ? -1 : easyRankInfo.rank);
			dlg.SetHistoryNormalRank (normalRankInfo == null ? -1 : normalRankInfo.rank);
			dlg.SetHistoryHardRank (hardRankInfo == null ? -1 : hardRankInfo.rank);

			dlg.MetaData = new Dictionary<string,object> () {
				{ "levelKey", levelKey.StringKey }
			};

			view.CloseLoadingDlg ();
		}

		void OnInitStartGameDlg(StartGameDlgCtrl dlg, LevelKey levelKey){
			try{
				var lang = userSettings.Language;
				dlg.SetTitle (levelKey.Idx + "");
				var obj = Util.Instance.GetPrefab (levelKey.LevelPicPrefabName, null);
				dlg.SetLevelImage (obj.GetComponent<Image> ().sprite);
				GameObject.Destroy (obj);
				dlg.SetGameModeText (langText.GetGameModeText (lang, levelKey));
				dlg.SetTitle (langText.GetLevelName (lang, levelKey));
				dlg.SetArtistTitle (langText.GetArtistTitle (lang, levelKey));
				dlg.SetSongTitle (langText.GetSongTitle (lang, levelKey));
				dlg.SetUseItem (null);

				var easyRankInfo = model.GetMaxRankLevelInfo(levelKey.ChangeDifficulty(GameRecord.Easy));
				var normalRankInfo = model.GetMaxRankLevelInfo(levelKey.ChangeDifficulty(GameRecord.Normal));
				var hardRankInfo = model.GetMaxRankLevelInfo(levelKey.ChangeDifficulty(GameRecord.Hard));

				dlg.SetHistoryEasyRank (easyRankInfo == null ? -1 : easyRankInfo.rank);
				dlg.SetHistoryNormalRank (normalRankInfo == null ? -1 : normalRankInfo.rank);
				dlg.SetHistoryHardRank (hardRankInfo == null ? -1 : hardRankInfo.rank);

				dlg.MetaData = new Dictionary<string,object> () {
					{ "levelKey", levelKey.StringKey }
				};
			} catch (ShowMessageException e){
				OnException (e);
			}
		}

		static void ClearAssetBundleFiles(){
			if (Directory.Exists (GameConfig.LOCAL_ASSET_BUNDELS_PATH)) {
				foreach (string file in System.IO.Directory.GetFiles(GameConfig.LOCAL_ASSET_BUNDELS_PATH)){
					Debug.Log ("delete "+file);
					File.Delete(file);
				}
				Debug.Log ("delete:"+GameConfig.LOCAL_ASSET_BUNDELS_PATH);
			}
		}

		void OnShowMessageException(ShowMessageException e){
			if (e.InnerException is NullReferenceException) {
				userSettings.IsDownloadPackageOKFlag = false;
				userSettings.Save ();
				// 有些情況上面兩行讓機子重新檢查package還是沒有起到作用
				// 保險起見，就直接全刪了，強制重來
				ClearAssetBundleFiles ();
				// 使用AssetBundle時該bundle沒下載完時會跑到這裡
				Alert (langText.GetDlgMessage (userSettings.Language, "MesgText_D01"));
			} else {
				var msg = e.Message;
				if (e.InnerException != null) {
					msg += ":" + e.InnerException.Message;
				}
				Alert (msg);
			}
		}

		bool HandleStartGameDlg(string cmd){
			switch (cmd) {
			case "StartGameDlgBtnBack":
				view.GetMainMenu ().CloseStartGameDlg ();
				break;
			case "StartGameDlgBtnEasy":
			case "StartGameDlgBtnNormal":
			case "StartGameDlgBtnHard":
				{
					var dlg = view.GetMainMenu ().OpenStartGameDlg ();
					var useItem = dlg.GetUseItem ();
					var metaData = dlg.MetaData;
					if (metaData == null) {
						throw new UnityException ("初始化StartGameDlg有問題，請檢查");
					}
					var levelKeyStr = (string)metaData ["levelKey"];
					var levelKey = new LevelKey (levelKeyStr);
					if (cmd == "StartGameDlgBtnEasy") {
						levelKey = levelKey.ChangeDifficulty (GameRecord.Easy);
					} else if (cmd == "StartGameDlgBtnNormal") {
						levelKey = levelKey.ChangeDifficulty (GameRecord.Normal);
					} else {
						levelKey = levelKey.ChangeDifficulty (GameRecord.Hard);
					}
					sessions.CurrentPlayingLevel = levelKey.StringKey;
					if (useItem != null) {
						sessions.UsedItem = useItem.StringKey;
					} else {
						sessions.UsedItem = null;
					}
					// 開始遊戲
					StartCoroutine (StartPlayGameVer2 (view, levelKey));
				}
				break;
			case "StartGameDlgBtnSelectItem":
				{
					var catId = model.SelectCatKey;
					var catDef = CatDef.Get (catId.Idx);
					var filterFn = 
						FilterItemAnd (
							FilterItemIsType (GameConfig.ITEM_TYPE.ITEM_FOOD),
							FilterItemIsCatType(catDef.Type)
						);
					StartCoroutine (ExecWithDownloadAssetBundleIfNeeded (() => {
						var dlg = view.GetMainMenu ().OpenStorageDlg ();
						OnInitStorageDlg (dlg, QueryItemCount, filterFn);
						dlg.MetaData = new Dictionary<string, object> () {
							{ "action", "selectItemForStartGameDlg" },
						};
					}));
				}
				break;
			default:
				return false;
			}
			return true;
		}
#region tutorial
		void StartTutorial(TutorialModel.Type type){
			if (view.IsInMenu () == false) {
				Debug.LogError ("必須在菜單頁才能開始教學");
				return;
			}
			// 取得教學模型
			var model = GetComponent<TutorialModel> ();
			// 設定教學類型，通常是傳入Type.Interactive
			model.tutorialType = type;
			model.Load ();
			// 取代
			this.model = model;
			UpdateUI ();

			var mainUI = view.GetMainMenu ().GetMainUI ();
			mainUI.Tips.Visible = false;
			TrainingUI.SetVisible (mainUI.gameObject, true);
			// 啟動教學動畫
			model.RequestAnimateTutorialUI (view, model.CurrentWaitingCommand);
		}
		void StopTutorialModel(){
			this.model = GetModel ();

			var mainUI = view.GetMainMenu ().GetMainUI ();
			mainUI.Tips.Visible = true;
			TrainingUI.SetVisible (mainUI.gameObject, false);
			UpdateUI ();

			var getItems = GameConfig.TUTORIAL_REWARDS;
			foreach (var itemConfigId in getItems) {
				model.AddItem (ItemKey.WithItemConfigID(itemConfigId), 1);
			}
			foreach (var itemConfigId in getItems) {
				PushAction (GenShowItemAction (ItemKey.WithItemConfigID(itemConfigId)));
			}
			PerformActionAndRemove ();

			model.MarkRead (CommonTutorialFlag);
		}

		void HandleTutorialCommand(string cmd){
			// 更新菜單的UI，因為傳入TutorialModel的只View
			// 這個解法不是很漂亮，但沒辨法
			// 這個程式沒有把視覺操做全部包裝在View中是個失誤
			Action<string> notification = str => {
				if(str == "update ui"){
					UpdateUI();
				}
			};
			var model = this.model as TutorialModel;
			Action handleModelStep = () => {
				if (model.NextStep ()) {
					model.RequestAnimateTutorialUI (view, model.CurrentWaitingCommand, notification);
				} else {
					model.RequestAnimateTutorialUI (view, "end", notification);
				}
			};
			// 特殊處理的指令__CatAngry__ | __CatWakeup__
			// __CatAngry__在玩家觸發貓生氣之後，直接在這裡將指令處理掉
			if (model.CurrentWaitingCommand == "__CatAngry__") {
				var cat = model.GetCat (model.SelectCatKey);
				if (cat.Status == GameConfig.CAT_STATE_ID.STATE_ANGRY) {
					handleModelStep ();
					return;
				}
			}
			// __CatWakeup__在玩家觸發叫醒貓之後，直接在這裡將指令處理掉
			if (model.CurrentWaitingCommand == "__CatWakeup__") {
				var cat = model.GetCat (model.SelectCatKey);
				if (cat.Status == GameConfig.CAT_STATE_ID.STATE_ANGRY) {
					handleModelStep ();
					return;
				}
			}
			if (cmd != model.CurrentWaitingCommand) {
				return;
			}
			handleModelStep ();
		}

		bool IsTutorial{
			get{
				return this.model is TutorialModel;
			}
		}

		bool IsValidCommandInTutorial(string cmd){
			if (IsTutorial == false) {
				Debug.LogError ("不正確的流程，這時必須是在教學模式");
				return true;
			}
			// 白名單
			if (cmd == "TutorialModelEnd") {
				return true;
			}
			var tm = this.model as TutorialModel;
			var isShouldHandleTouch = 
				tm.CurrentWaitingCommand == "SelectCatDlgBtnTouchFromMain" ||
				tm.CurrentWaitingCommand == "__CatAngry__" ||
				tm.CurrentWaitingCommand == "__CatWakeup__";
			// 特殊處理
			if (isShouldHandleTouch) {
				if (cmd.Contains ("SelectCatDlgBtnTouch")) {
					return true;
				}
			}
			if (cmd == tm.CurrentWaitingCommand) {
				return true;
			}
			return false;
		}
#endregion

#region Ad
		void InitAd(){
#if GCM1
			AdmobInit();
#else
			YomobInit();
#endif
		}
		void ShowRewardAd(){
#if GCM1
			AdmobShowRewardAd ();
#else
			YomobShowRewardAd();
#endif
		}
		bool IsBannerVisible{
			set{
#if GCM1
				AdmobIsBannerVisible = value;
#else
				// ignore
#endif
			}
		}
		void LoadRewardAd(){
#if GCM1
			AdmobLoadRewardAd();
#else
			view.CloseLoadingDlg ();
			ShowRewardAd();
#endif
		}
#endregion

#region Yomob
		public HandleYoMob handleYomob;
		void YomobInit(){
			handleYomob.Init();
			handleYomob.OnException += OnException;
			handleYomob.OnADAwardFailed += OnException;
			handleYomob.OnShowFailed += OnException;
			handleYomob.OnADAwardSuccess += HandleYomob_OnADAwardSuccess;
			handleYomob.OnADClose += HandleYomob_OnADClose;
		}

		void HandleYomob_OnADClose ()
		{
			view.CloseLoadingDlg ();
			// 統一在這裡獲得獎利
			AfterWatchAd ();
			UpdateUI ();
		}

		void HandleYomob_OnADAwardSuccess ()
		{
			view.CloseLoadingDlg ();
			// 不在這裡獲得獎利，換在HandleYomob_OnADClose
		}
		void YomobShowRewardAd(){
			handleYomob.ShowRewardAd ();
		}
#endregion

#region Admob
		public HandleAdmob handleAdmob;

		void AdmobInit(){
			handleAdmob.Init();
			handleAdmob.OnInterstitialDidDismissScreen += HandleAdmob_OnInterstitialDidDismissScreen;
			handleAdmob.OnInterstitialDidReceiveAd += HandleAdmob_OnInterstitialDidReceiveAd;
			handleAdmob.OnInterstitialDidFailToReceiveAdWithError += HandleAdmob_OnInterstitialDidFailToReceiveAdWithError;
			handleAdmob.OnAdViewDidFailToReceiveAdWithError += HandleAdmob_OnAdViewDidFailToReceiveAdWithError;
			if (model.IsUnlockBannerAd == false) {
				handleAdmob.LoadBanner ();
			}
		}

		void AdmobShowRewardAd(){
			handleAdmob.ShowRewardAd ();
		}

		bool AdmobIsBannerVisible{
			set{
				handleAdmob.IsBannerVisible = value;
			}
		}

		void AdmobLoadRewardAd(){
			handleAdmob.LoadRewardAd ();
		}

		void HandleAdmob_OnInterstitialDidDismissScreen (){
			AfterWatchAd ();
			UpdateUI ();
		}

		void HandleAdmob_OnAdViewDidFailToReceiveAdWithError (Exception obj){
			OnException (obj);
		}

		void HandleAdmob_OnInterstitialDidFailToReceiveAdWithError (Exception obj){
			view.CloseLoadingDlg ();
			OnException (obj);
		}

		void HandleAdmob_OnInterstitialDidReceiveAd (){
			view.CloseLoadingDlg ();
			ShowRewardAd ();
		}
#endregion
	}

	public partial class Main {
		public bool debug;

		static byte[] TestBytes(){
			int size = 100000000;
			IntPtr iptr = System.Runtime.InteropServices.Marshal.AllocHGlobal(size);
			var bytes = new byte[size];
			System.Runtime.InteropServices.Marshal.Copy(bytes, 0, iptr, size);
			System.Runtime.InteropServices.Marshal.FreeHGlobal(iptr);
			return bytes;
		}

		void OnGUI(){
			if (debug == false) {
				return;
			}
			GUILayout.BeginArea (new Rect (0, 0, 400, 400));
			if (GUILayout.Button ("IOS run this function will cause memory leak")) {
				TestBytes ();
			}
			if (GUILayout.Button ("開貓")) {
				var keys = StoreCtrl.GetItemKeys (StoreCtrl.DATA_CAT);
				foreach (var key in keys) {
					model.EnableCat (key);
				}
				UpdateUI ();
			}
			if (GUILayout.Button ("gash query")) {
				StartCoroutine (GetGashItem ());
			}
			if (GUILayout.Button ("pass all level for mapS1")) {
				for (var i = 0; i < 15; ++i) {
					var levelKey = new LevelKey () {
						MapIdx = "S1",
						Idx = i,
						Difficulty = GameRecord.Normal
					};
					model.LevelClear (levelKey);
				}
				CheckPassLevelGiftAndPushAction ("S1");
				UpdateUI ();

				PerformActionAndRemove ();
			}
			if (GUILayout.Button ("TrainingMode")) {
				StartCoroutine (StartGamePlayTutorialVer2 (view));
			}
			if (GUILayout.Button ("TutorialMode-Interactive")) {
				StartTutorial (TutorialModel.Type.Interative);
			}
			if (GUILayout.Button ("TutorialMode-Explore")) {
				StartTutorial (TutorialModel.Type.Explore);
			}
			if (GUILayout.Button ("TutorialMode-ExploreForCat")) {
				StartTutorial (TutorialModel.Type.ExploreForCat);
			}
			if (GUILayout.Button ("enable all photos")) {
				// 照片全開
				foreach (var mapIdx in GameConfig.MAP_IDXS) {
					try{
						var sc = GameRecord.GetPhotosCount (mapIdx, PhotoKey.TypeSmallPhoto);
						var bc = GameRecord.GetPhotosCount (mapIdx, PhotoKey.TypeBigPhoto);
						var total = sc + bc;
						var firstPhoto = new PhotoKey () {
							MapIdx = mapIdx,
							Type = PhotoKey.TypeSmallPhoto,
							Idx = 0
						};
						var currPhoto = firstPhoto;
						for (var i = 0; i < total; ++i) {
							model.EnablePhoto (currPhoto);
							currPhoto = currPhoto.NextKey;
						}
					}catch(Exception){
						// ignore
					}
				}
				UpdateUI ();
			}
			if (GUILayout.Button ("DownloadDlg")) {
				var dlg = view.OpenDownloadDlg ();
				dlg.UpdateUI (langText, userSettings.Language);
			}
			if (GUILayout.Button ("過關")) {
				view.Game.SetHp (100, 100);
				view.Game.Step (99999999, 0);
			}
			if (GUILayout.Button ("失敗")) {
				view.Game.SetHp (1, 100);
				view.Game.Step (99999999, 0);
			}
			// > 玩5場變成睡眠
			if (GUILayout.Button ("玩一場")) {
				var dlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
				var catId = model.OwnedCats.Skip (dlg.CurrentCatIndexInOrder).First();
				// 玩一場
				model.CatEnterGame(catId);
				model.CatExitGame (catId);
				UpdateUI ();
			}
			// > 假設貓玩完後剩一血，再按P就會變成飢餓
			if (GUILayout.Button ("剩一滴")) {
				var dlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
				var catId = model.OwnedCats.Skip (dlg.CurrentCatIndexInOrder).First();
				var cat = model.GetCat (catId);
				cat.Hp = 1;
				UpdateUI ();
			}
			// > 強制時間經過，變成想玩
			if (GUILayout.Button ("變想玩")) {
				model.DebugCommand ("ForceSetPlayerWinnaPlayTime");
				UpdateUI ();
			}
			if (GUILayout.Button ("MailGiftDlg")) {
				var dlg = view.GetMainMenu ().OpenMailGiftDlg ();
				dlg.SetTitle ("aaccc");
				var items = new List<ItemKey> ();
				for (var i = 0; i < GiftDef.ID_COUNT; ++i) {
					var giftKey = string.Format ("Mg{0:00}", i+1);
					var gd = GiftDef.Get (giftKey);
					var item = ItemKey.WithItemConfigID (gd.Item);
					items.Add (item);
				}
				dlg.SetItem (items);
			}
			if (GUILayout.Button ("AfterWatchAd")) {
				AfterWatchAd ();
			}
			if (GUILayout.Button ("pass all level for map01")) {
				for (var i = 0; i < 10; ++i) {
					var levelKey = new LevelKey () {
						MapIdx = "01",
						Idx = i,
						Difficulty = GameRecord.Normal
					};
					model.LevelClear (levelKey);
				}
				CheckPassLevelGiftAndPushAction ("01");
				UpdateUI ();

				PerformActionAndRemove ();
			}
			if (GUILayout.Button ("pass all level for map02")) {
				for (var i = 0; i < 10; ++i) {
					var levelKey = new LevelKey () {
						MapIdx = "02",
						Idx = i,
						Difficulty = GameRecord.Easy
					};
					model.LevelClear (levelKey);
				}
				CheckPassLevelGiftAndPushAction ("02");
				UpdateUI ();

				PerformActionAndRemove ();
			}
			if (GUILayout.Button ("pass all level for map03")) {
				for (var i = 0; i < 10; ++i) {
					var levelKey = new LevelKey () {
						MapIdx = "03",
						Idx = i,
						Difficulty = GameRecord.Easy
					};
					model.LevelClear (levelKey);
				}
				CheckPassLevelGiftAndPushAction ("03");
				UpdateUI ();

				PerformActionAndRemove ();
			}
			if (GUILayout.Button ("pass all level for map03")) {
				for (var i = 0; i < 10; ++i) {
					var levelKey = new LevelKey () {
						MapIdx = "04",
						Idx = i,
						Difficulty = GameRecord.Easy
					};
					model.LevelClear (levelKey);
				}
				CheckPassLevelGiftAndPushAction ("04");
				UpdateUI ();

				PerformActionAndRemove ();
			}
			if (GUILayout.Button ("add exp 1000")) {
				var dlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
				var catId = model.OwnedCats.Skip (dlg.CurrentCatIndexInOrder).First();
				model.AddCatExp (catId, 1000);
				UpdateUI ();
			}
			GUILayout.EndArea ();
		}
	}
}

