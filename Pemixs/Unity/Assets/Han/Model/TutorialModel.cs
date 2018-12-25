using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace Remix
{
	public class TutorialModel : MonoBehaviour, IModel
	{
		public enum Type
		{
			Interative,
			Explore,
			ExploreForCat
		}

		public GameRecord record;
		public Type tutorialType;

		public int step;
		public List<string> cmdList;

		public void SetupStep(){
			step = 0;
			switch (tutorialType) {
			case TutorialModel.Type.Interative:
				{
					var validCmds = new List<string>(){
						"MainUIBtnCatIcon",
						"SelectCatDlgBtnTouchFromMain",
						"__CatAngry__",
						"SelectCatDlgBtnAngry",
						"StorageDlgBtnST_30",
						"SelectCatDlgBtnWannaPlay",
						"StorageDlgBtnT_18",
						"SelectCatDlgBtnHungry",
						"StorageDlgBtnF_1",
						"__CatWakeup__"
					};
					cmdList = validCmds;
				}
				break;
			case TutorialModel.Type.Explore:
				{
					var validCmds = new List<string> () {
						"MainUIBtnCapture",
						"CaptureUIPOS_node_GameCap_01_0",
						"StorageDlgBtnC_8",
						"CaptureUISpeedUp_node_GameCap_01_0",
						"StorageDlgBtnSC_17",
						"CaptureUIOK_node_GameCap_01_0",
						"NewSPhotoDlgBtnOK",
						"NewItemDlgBtnOK",
						"MainUIBtnAlbum",
						"AlbumDlgBtnBack",
					};
					cmdList = validCmds;
				}
				break;
			case TutorialModel.Type.ExploreForCat:
				{
					var validCmds = new List<string> () {
						//"MainUIBtnCapture",
						"CaptureUIPOS_node_GameCap_01_0",
						"StorageDlgBtnTC_32",
						"CaptureUISpeedUp_node_GameCap_01_0",
						"StorageDlgBtnST_30",
						"CaptureUIOK_node_GameCap_01_0",
						"NewCatDlgBtnOK",
						"MainUIBtnCatIcon",
						"SelectCatDlgBtnRight",
						"SelectCatDlgBtnSelect",
						"MainUIBtnBuy",
						"IAPDlgBtnBack",
					};
					cmdList = validCmds;
				}
				break;
			}
		}

		public int Step {
			get {
				return step;
			}
		}

		public bool NextStep(){
			if (++step >= cmdList.Count) {
				step = cmdList.Count - 1;
				return false;
			}
			return true;
		}

		public string CurrentWaitingCommand{
			get{
				return cmdList [step];
			}
		}

		Coroutine animateTutorialUI;
		public void RequestAnimateTutorialUI(View view, string cmd, Action<string> notification = null){
			if (animateTutorialUI != null) {
				StopCoroutine (animateTutorialUI);
				animateTutorialUI = null;
			}
			animateTutorialUI = StartCoroutine (AnimateTutorialUI (view, cmd, notification));
		}

		public IEnumerator AnimateTutorialUI(View view, string cmd, Action<string> notification = null){
			// 注意，只有AnimateTutorialInteractive有傳入notification
			// 若其它的有需要，就請自行修改程式
			switch (tutorialType) {
			case Type.Interative:
				yield return AnimateTutorialInteractive (view, cmd, notification);
				break;
			case Type.Explore:
				yield return AnimateTutorialExplore (view, cmd);
				break;
			case Type.ExploreForCat:
				yield return AnimateTutorialExploreForCat (view, cmd);
				break;
			default:
				break;
			}
			yield return null;
		}

		IEnumerator AnimateTutorialExploreForCat(View view, string cmd){
			var langText = Util.Instance.langText;
			var userSettings = Util.Instance.userSettings;

			var mainUI = view.GetMainMenu ().GetMainUI ();
			TrainingUI.HideAllFocus (mainUI.gameObject);

			switch (cmd) {
			/*case "MainUIBtnCapture":
				{
					var txt = langText.GetTrainingDesc (userSettings.Language, 28);
					TrainingUI.SetDesc (mainUI.gameObject, txt);
					yield return new WaitForSeconds (5f);

					txt = langText.GetTrainingDesc (userSettings.Language, 29);
					TrainingUI.SetDesc (mainUI.gameObject, txt);
					TrainingUI.SetFocusVisible (mainUI.gameObject, "TUI_Ring_R02", true);
					TrainingUI.SetFocusVisible (mainUI.gameObject, "TUI_Arrow_R02", true);
				}
				break;*/
			case "CaptureUIPOS_node_GameCap_01_0":
				{
					// wait for loading
					while (view.GetMainMenu ().GetMainUI ().IsInCaptureUI () == false) {
						yield return null;
					}

					var txt = langText.GetTrainingDesc (userSettings.Language, 34);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					var node = view.GetMainMenu ().GetMainUI ().GetCaptureUI ().GetGameCapRoot ().GetGameCap ().GetPosNode (new NodeKey () {
						Group = NodeKey.GroupGameCap,
						MapIdx = "01",
						Idx = 0
					});
					TrainingUI.SetVisible (node.gameObject, true);
					TrainingUI.SetFocusVisible (node.gameObject, "TUI_Arrow_R", true);
				}
				break;
			case "StorageDlgBtnTC_32":
				{
					var node = view.GetMainMenu ().GetMainUI ().GetCaptureUI ().GetGameCapRoot ().GetGameCap ().GetPosNode (new NodeKey () {
						Group = NodeKey.GroupGameCap,
						MapIdx = "01",
						Idx = 0
					});
					TrainingUI.HideAllFocus (node.gameObject);
					TrainingUI.HideAllFocus (mainUI.gameObject);
				}
				break;
			case "CaptureUISpeedUp_node_GameCap_01_0":
				{
					var txt = langText.GetTrainingDesc (userSettings.Language, 35);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					var node = view.GetMainMenu ().GetMainUI ().GetCaptureUI ().GetGameCapRoot ().GetGameCap ().GetPosNode (new NodeKey () {
						Group = NodeKey.GroupGameCap,
						MapIdx = "01",
						Idx = 0
					});
					TrainingUI.HideAllFocus (node.gameObject);
					TrainingUI.SetFocusVisible (node.gameObject, "TUI_Arrow_R", true);
				}
				break;
			case "StorageDlgBtnST_30":
				{
					var node = view.GetMainMenu ().GetMainUI ().GetCaptureUI ().GetGameCapRoot ().GetGameCap ().GetPosNode (new NodeKey () {
						Group = NodeKey.GroupGameCap,
						MapIdx = "01",
						Idx = 0
					});
					TrainingUI.HideAllFocus (node.gameObject);
					TrainingUI.HideAllFocus (mainUI.gameObject);
				}
				break;
			case "CaptureUIOK_node_GameCap_01_0":
				{
					var txt = langText.GetTrainingDesc (userSettings.Language, 36);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					var node = view.GetMainMenu ().GetMainUI ().GetCaptureUI ().GetGameCapRoot ().GetGameCap ().GetPosNode (new NodeKey () {
						Group = NodeKey.GroupGameCap,
						MapIdx = "01",
						Idx = 0
					});
					TrainingUI.HideAllFocus (node.gameObject);
					TrainingUI.SetFocusVisible (node.gameObject, "TUI_Arrow_R", true);
				}
				break;
			case "NewCatDlgBtnOK":
				{
					var node = view.GetMainMenu ().GetMainUI ().GetCaptureUI ().GetGameCapRoot ().GetGameCap ().GetPosNode (new NodeKey () {
						Group = NodeKey.GroupGameCap,
						MapIdx = "01",
						Idx = 0
					});
					TrainingUI.HideAllFocus (node.gameObject);
					// wait for loading
					while (view.GetMainMenu ().GetNewCatDlg() == null) {
						yield return null;
					}
					var dlg = view.GetMainMenu ().GetNewCatDlg ();
					TrainingUI.SetVisible (dlg.gameObject, true);
					TrainingUI.SetFocusVisible (dlg.gameObject, "TUI_Arrow_R", true);
				}
				break;
			case "MainUIBtnCatIcon":
				{
					var txt = langText.GetTrainingDesc (userSettings.Language, 37);
					TrainingUI.SetDesc (mainUI.gameObject, txt);
					yield return new WaitForSeconds (3f);

					txt = langText.GetTrainingDesc (userSettings.Language, 38);
					TrainingUI.SetDesc (mainUI.gameObject, txt);
					TrainingUI.HideAllFocus (mainUI.gameObject);
					TrainingUI.SetFocusVisible (mainUI.gameObject, "TUI_Ring_R01", true);
					TrainingUI.SetFocusVisible (mainUI.gameObject, "TUI_Arrow_R01", true);
				}
				break;
			case "SelectCatDlgBtnRight":
				{
					TrainingUI.HideAllFocus (mainUI.gameObject);
					while (view.GetMainMenu ().GetSelectCatDlgCtrl() == null) {
						yield return null;
					}
					var txt = langText.GetTrainingDesc (userSettings.Language, 39);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					var dlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
					TrainingUI.SetVisible (dlg.gameObject, true);
					TrainingUI.HideAllFocus (dlg.gameObject);
					TrainingUI.SetFocusVisible (dlg.gameObject, "TUI_Arrow_R02", true);
				}
				break;
			case "SelectCatDlgBtnSelect":
				{
					TrainingUI.HideAllFocus (mainUI.gameObject);

					var txt = langText.GetTrainingDesc (userSettings.Language, 40);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					var dlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
					TrainingUI.SetVisible (dlg.gameObject, true);
					TrainingUI.HideAllFocus (dlg.gameObject);
					TrainingUI.SetFocusVisible (dlg.gameObject, "TUI_Arrow_R03", true);
				}
				break;
			case "MainUIBtnBuy":
				{
					TrainingUI.HideAllFocus (mainUI.gameObject);

					var txt = langText.GetTrainingDesc (userSettings.Language, 41);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					TrainingUI.SetFocusVisible (mainUI.gameObject, "TUI_Ring_R04", true);
					TrainingUI.SetFocusVisible (mainUI.gameObject, "TUI_Arrow_R04", true);
				}
				break;
			case "IAPDlgBtnBack":
				{
					// wait for loading
					while (view.GetMainMenu ().GetIAPDlg() == null) {
						yield return null;
					}
					var dlg = view.GetMainMenu ().GetIAPDlg ();
					TrainingUI.SetVisible (dlg.gameObject, true);
					TrainingUI.SetFocusVisible (dlg.gameObject, "TUI_Arrow_R", true);
				}
				break;
			case "end":
				{
					var txt = langText.GetTrainingDesc (userSettings.Language, 42);
					TrainingUI.SetDesc (mainUI.gameObject, txt);
					yield return new WaitForSeconds (3f);

					// 所有教學結束
					UIEventFacade.OnTutorialModelEvent.OnNext("TutorialModelEnd");
				}
				break;
			}
		}

		IEnumerator AnimateTutorialExplore(View view, string cmd){
			var langText = Util.Instance.langText;
			var userSettings = Util.Instance.userSettings;

			var mainUI = view.GetMainMenu ().GetMainUI ();
			TrainingUI.HideAllFocus (mainUI.gameObject);

			switch (cmd) {
			case "MainUIBtnCapture":
				{
					var txt = langText.GetTrainingDesc (userSettings.Language, 28);
					TrainingUI.SetDesc (mainUI.gameObject, txt);
					yield return new WaitForSeconds (5f);

					txt = langText.GetTrainingDesc (userSettings.Language, 29);
					TrainingUI.SetDesc (mainUI.gameObject, txt);
					TrainingUI.SetFocusVisible (mainUI.gameObject, "TUI_Ring_R02", true);
					TrainingUI.SetFocusVisible (mainUI.gameObject, "TUI_Arrow_R02", true);
				}
				break;
			case "CaptureUIPOS_node_GameCap_01_0":
				{
					// wait for loading
					while (view.GetMainMenu ().GetMainUI ().IsInCaptureUI () == false) {
						yield return null;
					}

					var txt = langText.GetTrainingDesc (userSettings.Language, 30);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					var node = view.GetMainMenu ().GetMainUI ().GetCaptureUI ().GetGameCapRoot ().GetGameCap ().GetPosNode (new NodeKey () {
						Group = NodeKey.GroupGameCap,
						MapIdx = "01",
						Idx = 0
					});
					TrainingUI.SetVisible (node.gameObject, true);
					TrainingUI.SetFocusVisible (node.gameObject, "TUI_Arrow_R", true);
				}
				break;
			case "StorageDlgBtnC_8":
				{
					var node = view.GetMainMenu ().GetMainUI ().GetCaptureUI ().GetGameCapRoot ().GetGameCap ().GetPosNode (new NodeKey () {
						Group = NodeKey.GroupGameCap,
						MapIdx = "01",
						Idx = 0
					});
					TrainingUI.HideAllFocus (node.gameObject);
					TrainingUI.HideAllFocus (mainUI.gameObject);
				}
				break;
			case "CaptureUISpeedUp_node_GameCap_01_0":
				{
					var txt = langText.GetTrainingDesc (userSettings.Language, 31);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					var node = view.GetMainMenu ().GetMainUI ().GetCaptureUI ().GetGameCapRoot ().GetGameCap ().GetPosNode (new NodeKey () {
						Group = NodeKey.GroupGameCap,
						MapIdx = "01",
						Idx = 0
					});
					TrainingUI.HideAllFocus (node.gameObject);
					TrainingUI.SetFocusVisible (node.gameObject, "TUI_Arrow_R", true);
				}
				break;
			case "StorageDlgBtnSC_17":
				{
					var node = view.GetMainMenu ().GetMainUI ().GetCaptureUI ().GetGameCapRoot ().GetGameCap ().GetPosNode (new NodeKey () {
						Group = NodeKey.GroupGameCap,
						MapIdx = "01",
						Idx = 0
					});
					TrainingUI.HideAllFocus (node.gameObject);
					TrainingUI.HideAllFocus (mainUI.gameObject);
				}
				break;
			case "CaptureUIOK_node_GameCap_01_0":
				{
					var txt = langText.GetTrainingDesc (userSettings.Language, 32);
					TrainingUI.SetDesc (mainUI.gameObject, txt);
				}
				break;
			case "NewSPhotoDlgBtnOK":
				{
					// wait for loading
					while (view.GetMainMenu ().GetTopNewSPhotoDlg() == null) {
						yield return null;
					}

					var txt = langText.GetTrainingDesc (userSettings.Language, 32);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					var dlg = view.GetMainMenu ().GetTopNewSPhotoDlg ();
					TrainingUI.SetVisible (dlg.gameObject, true);
					TrainingUI.SetFocusVisible (dlg.gameObject, "TUI_Arrow_R", true);
				}
				break;
			case "NewItemDlgBtnOK":
				{
					// wait for loading
					while (view.GetMainMenu ().GetTopNewItemDlg() == null) {
						yield return null;
					}

					var dlg = view.GetMainMenu ().GetTopNewItemDlg ();
					TrainingUI.SetVisible (dlg.gameObject, true);
					TrainingUI.SetFocusVisible (dlg.gameObject, "TUI_Arrow_R", true);
				}
				break;
			case "MainUIBtnAlbum":
				{
					var txt = langText.GetTrainingDesc (userSettings.Language, 33);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					TrainingUI.HideAllFocus (mainUI.gameObject);
					TrainingUI.SetFocusVisible (mainUI.gameObject, "TUI_Ring_R03", true);
					TrainingUI.SetFocusVisible (mainUI.gameObject, "TUI_Arrow_R03", true);
				}
				break;
			case "AlbumDlgBtnBack":
				{
					// wait for loading
					while (view.GetMainMenu ().GetAlbumDlg() == null) {
						yield return null;
					}

					var dlg = view.GetMainMenu ().GetAlbumDlg ();
					TrainingUI.SetVisible (dlg.gameObject, true);
					TrainingUI.SetFocusVisible (dlg.gameObject, "TUI_Arrow_R", true);
				}
				break;
			case "end":
				{
					yield return new WaitForSeconds (3f);

					tutorialType = Type.ExploreForCat;
					Load ();
					RequestAnimateTutorialUI (view, CurrentWaitingCommand);
				}
				break;
			default:
				{
					TrainingUI.HideAllFocus (mainUI.gameObject);
				}
				break;
			}
		}

		IEnumerator AnimateTutorialInteractive(View view, string cmd, Action<string> notification = null){
			var langText = Util.Instance.langText;
			var userSettings = Util.Instance.userSettings;

			var mainUI = view.GetMainMenu ().GetMainUI ();
			TrainingUI.HideAllFocus (mainUI.gameObject);

			switch (cmd) {
			case "MainUIBtnCatIcon":
				{
					var txt = langText.GetTrainingDesc (userSettings.Language, 14);
					TrainingUI.SetDesc (mainUI.gameObject, txt);
					yield return new WaitForSeconds (3f);

					txt = langText.GetTrainingDesc (userSettings.Language, 15);
					TrainingUI.SetDesc (mainUI.gameObject, txt);
					TrainingUI.SetFocusVisible (mainUI.gameObject, "TUI_Ring_R01", true);
					TrainingUI.SetFocusVisible (mainUI.gameObject, "TUI_Arrow_R01", true);
				}
				break;
			case "SelectCatDlgBtnTouchFromMain":
				{
					var txt = langText.GetTrainingDesc (userSettings.Language, 16);
					TrainingUI.SetDesc (mainUI.gameObject, txt);
					while (view.GetMainMenu ().GetSelectCatDlgCtrl () == null) {
						yield return null;
					}
					var selectCatDlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
					TrainingUI.SetVisible (selectCatDlg.gameObject, true);
					TrainingUI.HideAllFocus (selectCatDlg.gameObject);
					TrainingUI.SetFocusVisible (selectCatDlg.gameObject, "TUI_Hand", true);
				}
				break;
			case "SelectCatDlgBtnAngry":
				{
					var txt = langText.GetTrainingDesc (userSettings.Language, 17);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					var selectCatDlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
					TrainingUI.SetVisible (selectCatDlg.gameObject, true);
					TrainingUI.HideAllFocus (selectCatDlg.gameObject);
					yield return new WaitForSeconds (2f);

					txt = langText.GetTrainingDesc (userSettings.Language, 18);
					TrainingUI.SetDesc (mainUI.gameObject, txt);
					TrainingUI.SetFocusVisible (selectCatDlg.gameObject, "TUI_Arrow_R01", true);
				}
				break;
			case "StorageDlgBtnST_30":
				{
					var txt = langText.GetTrainingDesc (userSettings.Language, 18);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					var selectCatDlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
					TrainingUI.SetVisible (selectCatDlg.gameObject, true);
					TrainingUI.HideAllFocus (selectCatDlg.gameObject);
				}
				break;
			case "SelectCatDlgBtnWannaPlay":
				{
					yield return new WaitForSeconds (5f);
					var txt = langText.GetTrainingDesc (userSettings.Language, 19);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					record.ForceSetWinnaPlay ();
					var selectCatDlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
					selectCatDlg.ApplyCatAnimation ();
					selectCatDlg.UpdateUI ();
					notification ("update ui");
					yield return new WaitForSeconds (1f);

					TrainingUI.SetVisible (selectCatDlg.gameObject, true);
					TrainingUI.HideAllFocus (selectCatDlg.gameObject);
					TrainingUI.SetFocusVisible (selectCatDlg.gameObject, "TUI_Arrow_R01", true);
				}
				break;
			case "StorageDlgBtnT_18":
				{
					var txt = langText.GetTrainingDesc (userSettings.Language, 20);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					var selectCatDlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
					TrainingUI.SetVisible (selectCatDlg.gameObject, true);
					TrainingUI.HideAllFocus (selectCatDlg.gameObject);
				}
				break;
			case "SelectCatDlgBtnHungry":
				{
					var txt = langText.GetTrainingDesc (userSettings.Language, 21);
					TrainingUI.SetDesc (mainUI.gameObject, txt);
					yield return new WaitForSeconds (5f);

					txt = langText.GetTrainingDesc (userSettings.Language, 22);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					record.ForceSetHungry ();
					var selectCatDlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
					selectCatDlg.ApplyCatAnimation ();
					selectCatDlg.UpdateUI ();
					notification ("update ui");

					TrainingUI.SetVisible (selectCatDlg.gameObject, true);
					TrainingUI.HideAllFocus (selectCatDlg.gameObject);
					TrainingUI.SetFocusVisible (selectCatDlg.gameObject, "TUI_Arrow_R01", true);
				}
				break;
			case "StorageDlgBtnF_1":
				{
					var txt = langText.GetTrainingDesc (userSettings.Language, 23);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					var selectCatDlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
					TrainingUI.SetVisible (selectCatDlg.gameObject, true);
					TrainingUI.HideAllFocus (selectCatDlg.gameObject);
				}
				break;
			case "__CatWakeup__":
				{
					yield return new WaitForSeconds (5f);

					var txt = langText.GetTrainingDesc (userSettings.Language, 24);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					record.ForceSetSleep ();
					var selectCatDlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
					selectCatDlg.ApplyCatAnimation ();
					selectCatDlg.UpdateUI ();
					notification ("update ui");

					txt = langText.GetTrainingDesc (userSettings.Language, 25);
					TrainingUI.SetDesc (mainUI.gameObject, txt);

					TrainingUI.SetVisible (selectCatDlg.gameObject, true);
					TrainingUI.HideAllFocus (selectCatDlg.gameObject);
					TrainingUI.SetFocusVisible (selectCatDlg.gameObject, "TUI_Hand", true);
				}
				break;
			case "end":
				{
					var selectCatDlg = view.GetMainMenu ().GetSelectCatDlgCtrl ();
					TrainingUI.SetVisible (selectCatDlg.gameObject, true);
					TrainingUI.HideAllFocus (selectCatDlg.gameObject);

					// "但可繼續遊玩關卡..."
					var txt = langText.GetTrainingDesc (userSettings.Language, 26);
					TrainingUI.SetDesc (mainUI.gameObject, txt);
					yield return new WaitForSeconds (3f);

					record.ForceSetNormal ();
					// 更新貓狀態
					selectCatDlg.ApplyCatAnimation ();
					selectCatDlg.UpdateUI ();
					notification ("update ui");

					txt = langText.GetTrainingDesc (userSettings.Language, 27);
					TrainingUI.SetDesc (mainUI.gameObject, txt);
					yield return new WaitForSeconds (4f);

					// 重讀狀態
					tutorialType = Type.Explore;
					Load ();

					view.GetMainMenu ().CloseSelectCatDlg ();
					RequestAnimateTutorialUI (view, CurrentWaitingCommand);
				}
				break;
			default:
				TrainingUI.HideAllFocus (mainUI.gameObject);
				break;
			}
		}

		public void Load(){
			Clear ();
		}
		public void Clear(){
			record.SetBasicData ();
			switch (tutorialType) {
			case Type.Interative:
				record.AddItem (ItemKey.WithItemConfigID("I10020"), 1);
				record.AddItem (ItemKey.WithItemConfigID("I30010"), 1);
				record.AddItem (ItemKey.WithItemConfigID("I31020"), 1);
				record.AddItem (ItemKey.WithItemConfigID("I32020"), 1);
				break;
			case Type.Explore:
				record.AddItem (ItemKey.WithItemConfigID("I20010"), 1);
				record.AddItem (ItemKey.WithItemConfigID("I21020"), 1);
				break;
			case Type.ExploreForCat:
				record.AddItem (ItemKey.WithItemConfigID("I31020"), 1);
				record.AddItem (ItemKey.WithItemConfigID("I32020"), 1);
				break;
			}
			SetupStep ();
		}
		public void Override (string json){
			
		}
		public void Save(){
			
		}

		public bool IsMarkRead(string key){
			return record.IsUnlockIAP (key);
		}

		public void MarkRead(string key){
			var isDirty = record.UnlockIAP (key);
			if (isDirty) {
				Save ();
			}
		}

		public float AudioOffsetTime{
			get{ return record.AudioOffsetTime; } 
			set{ 
				record.AudioOffsetTime = value;
				Save ();
			}
		}

		public int Money{
			get{ return record.Money; }
			set{ 
				record.Money = value;
				Save ();
			}
		}
		public int Gold{
			get{ return record.Gold; }
			set{ 
				record.Gold = value;
				Save ();
			}
		}

		public IEnumerable<ItemKey> OwnedCats{ get{ return record.GetOwnedCats (); } }
		public ItemKey SelectCatKey{
			get{ return record.GetSelectCat(); }
			set{ 
				record.SetSelectCat (value); 
				Save ();
			}
		}
		public Cat GetCat(ItemKey catId){
			return record.GetCat (catId);
		}
		public bool EnableCat (ItemKey catId){
			var ok = record.EnableCat (catId);
			if (ok) {
				Save ();
			}
			return ok;
		}
		public bool AddCatExp (ItemKey catId, int exp){
			var isLevelUp = record.AddCatExp (GetCat(catId), exp);
			Save ();
			return isLevelUp;
		}
		public void CatEnterGame (ItemKey catId){
			record.CatEnterGame (catId);
		}
		public void CatExitGame(ItemKey catId){
			record.CatEndGame (catId);
		}
		public bool IsCatCanEnterGame(ItemKey catId){
			return record.IsCanEnterGame (catId);
		}
		public bool IsCanTouch (ItemKey catId){
			return record.IsCanTouch(catId);
		}
		public void UpdateCatState(Action<ItemKey> OnCatWannaPlay, Action<ItemKey> OnCatSleepOver, Action<ItemKey> OnCatAngryOver){
			var dirty = false;
			dirty |= record.UpdateWannaPlayTime (OnCatWannaPlay);
			dirty |= record.UpdateSleepTime (OnCatSleepOver);
			dirty |= record.UpdateAngryTime (OnCatAngryOver);
			if (dirty) {
				Save ();
			}
		}
		public AnimationResult TouchCat(ItemKey catId){
			var anim = record.SetTouchCat (catId);
			Save ();
			return anim;
		}
		public AnimationResult UseSToyForCat (ItemKey catId, ItemKey itemId){
			var anim = record.UseSToyForCat (catId, itemId);
			Save ();
			return anim;
		}
		public AnimationResult UseFoodForCat (ItemKey catId, ItemKey itemId){
			var anim = record.UseFoodForCat (catId, itemId);
			Save ();
			return anim;
		}
		public AnimationResult UseToyForCat (ItemKey catId, ItemKey itemId){
			var anim = record.UseToyForCat (catId, itemId);
			Save ();
			return anim;
		}

		public bool IsPhotoEnable (PhotoKey key){
			return record.GetEnablePhotoKey ().Any (p => {
				return p.StringKey == key.StringKey;
			});
		}
		public void EnablePhoto (PhotoKey key){
			record.EnablePhoto (key);
			Save ();
		}

		public int ItemCount (ItemKey key){
			return record.QueryItemCount (key);
		}
		public bool ConsumeItem (ItemKey key){
			var ok = record.ConsumeItem (key);
			Save ();
			return ok;
		}

		bool HandleItemForTypeMGMM(ItemKey key, int count){
			switch (key.Type) {
			case StoreCtrl.DATA_MG:
			case StoreCtrl.DATA_MM:
				{
					var def = ItemDef.Get (key.Idx);
					Money += -(def.Money * count);
					Gold += -(def.Gold * count);
					return true;
				}
			}
			return false;
		}

		public void AddItem(ItemKey key, int count){
			// MG/MM類型的不要加到道具裡，直接使用掉
			if (HandleItemForTypeMGMM (key, count)) {
				Util.Instance.LogWarning ("MG道具，直接使用掉");
				return;
			}
			record.AddItem (key, count);
			Save ();
		}

		public Capture Capture (CaptureKey key){
			return record.GetCapture(key);
		}
		public bool CheckCaptureCompleted(){
			var dirty = record.CheckCaptureCompleted ();
			if (dirty) {
				Save ();
			}
			return dirty;
		}
		public void StartCapture (CaptureKey key, ItemKey item){
			record.StartCapture(key, item);
			Save ();
		}
		public bool SpeedUpCaptureWithItem(CaptureKey key, ItemKey item){
			var ok = record.SpeedUpCaptureWithItem (key, item);
			if (ok) {
				Save ();
			}
			return ok;
		}
		public void CompletedCapture(CaptureKey key){
			if (record.CompletedCapture (key)) {
				Save ();
			}
		}
		public void ClearCapture(CaptureKey key){
			record.ClearCapture (key);
			Save ();
		}
		public IEnumerable<string> EnableCaptures{ get{ return record.EnableCaptures; }}

		public void EnableCapture(CaptureKey key){
			record.EnableCapture (key);
			Save ();
		}
		public bool IsCaptureEnable(CaptureKey key){
			return record.IsCaptureEnable (key);
		}

		public string GetCurrentMapIdx(){
			return record.ComputeCurrentMap ();
		}

		public IEnumerable<LevelKey> GetCurrentLevel(string mapIdx) { 
			return record.ComputeCurrentLevel (mapIdx);
		}
		public void LevelClear (LevelKey level){
			record.PassLevel (level);
			Save ();
		}

		public bool IsAllLevelDifficultyClear(string mapIdx, int difficulty){
			var clearLvs = 
				from lv in from k in record.PassLevels
				select new LevelKey (k)
					where lv.MapIdx == mapIdx && lv.Difficulty == difficulty
				select lv;
			var mapId = GameConfig.MapIdx2Int (mapIdx);
			var levelCount = GameConfig.MAP_LEVEL_COUNT [mapId];
			return clearLvs.Count () == levelCount;
		}

		public void DebugCommand (string cmd){
			switch (cmd) {
			case "ForceSetPlayerWinnaPlayTime":
				record.ForceSetWinnaPlay ();
				break;
			}
		}

		public GameRecord.PassLevelInfo NewPassLevelInfo(LevelKey key){
			return record.NewPassLevelInfo (key);
		}

		public void AddPassLevelInfo(GameRecord.PassLevelInfo info){
			record.AddPassLevelInfo (info);
			Save ();
		}
		public GameRecord.PassLevelInfo GetMaxRankLevelInfo(LevelKey key){
			return record.GetMaxRankLevelInfo (key);
		}

		public IEnumerable<ItemKey> GetTodayGift(){
			var gift = record.GetTodayGiftAndRecordIt ();
			Save ();
			return gift;
		}
		public bool IsAlreadyGetGift(long time){
			return record.IsAlreadyGetGift (time);
		}
		public int GetTopGiftId(){
			return record.GetTopGiftId ();
		}

		public bool IsUnlockBannerAd{ 
			get{
				return record.IsUnlockIAP (GameConfig.UNLOCK_BANNER_IAP_SKU);
			}
			set{
				if (value == true) {
					record.UnlockIAP (GameConfig.UNLOCK_BANNER_IAP_SKU);
					Save ();
				} else {
					throw new UnityException ("無法把已解鎖的重設:IsUnlockBannerAd = false");
				}
			}
		}
	}
}

