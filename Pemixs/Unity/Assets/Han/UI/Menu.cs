using System;
using UnityEngine;
using UniRx;
using System.Collections;
using System.Collections.Generic;

namespace Remix
{
	public class Menu : MonoBehaviour
	{
		public Transform anchorMenu, anchorBackground;
		public PopupTracker popupTracker;

		public PopupTracker PopupTracker{ get { return popupTracker; } }

		public void InitComponent(){
			if (anchorMenu == null) {
				anchorMenu = this.transform;
			}
			if (anchorBackground == null) {
				anchorBackground = this.transform;
			}
			CreateMainUI ();
		}
		#region async
		public IEnumerator ChangeToHomeAsync(){
			yield return mainUI.ChangePageAsync (anchorBackground, 0);
		}
		public IEnumerator ChangeToMapAsync(){
			yield return mainUI.ChangePageAsync (anchorBackground, 2);
		}
		public IEnumerator ChangeToSub2Async(){
			yield return mainUI.ChangeSubPageAsync (2);
		}
		#endregion

		public void ChangeToHome(){
			mainUI.ChangePage (anchorBackground, 0);
		}

		public void ChangeToShop(){
			mainUI.ChangePage (anchorBackground, 1);
		}

		public void ChangeToMap(){
			mainUI.ChangePage (anchorBackground, 2);
		}

		public void ChangeToCapture(){
			mainUI.ChangePage (anchorBackground, 3);
		}

		public void ChangeToConfig(){
			mainUI.ChangePage (anchorBackground, 4);
		}

		public void ChangeToSub0(){
			mainUI.ChangeSubPage (0);
		}

		public void ChangeToSub1(){
			mainUI.ChangeSubPage (1);
		}

		public void ChangeToSub2(){
			mainUI.ChangeSubPage (2);
		}

		public UnityEngine.Object Load(string path){
			return Util.Instance.LoadAsset (path, typeof(UnityEngine.Object));
		}

		#region MainUI
		public string mainUIPath;
		public MainUI mainUI;
		public MainUI CreateMainUI(){
			if (this.mainUI != null) {
				throw new UnityException ("已經建立了MainUI");
			}
			var prefab = Load (mainUIPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+mainUIPath);
			}
			var ui = GameObject.Instantiate (prefab) as GameObject;
			ui.SetActive (true);
			ui.transform.SetParent (anchorMenu, false);
			if (ui.GetComponent<MainUI> () == null) {
				throw new UnityException ("preMainUI必須有MainUI的Component");
			}
			this.mainUI = ui.GetComponent<MainUI>();
			this.mainUI.InitComponent ();
			return this.mainUI;
		}

		public MainUI GetMainUI(){
			if (mainUI == null) {
				throw new UnityException ("請先呼叫InitComponent");
			}
			return mainUI;
		}
		#endregion

		#region IAPDlg
		public delegate ArrayList GetDataList();

		public string iapDlgPath;
		public IAPDlgCtrl iapDlg;
		public IAPDlgCtrl OpenIAPDlg(){
			if (iapDlg != null) {
				return iapDlg;
			}
			var prefab = Load (iapDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+iapDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<IAPDlgCtrl> ();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			iapDlg = ctrl;
			popupTracker.Track (ctrl);
			return iapDlg;
		}
		public IAPDlgCtrl GetIAPDlg(){
			return iapDlg;
		}
		public void CloseIAPDlg(){
			if (iapDlg != null) {
				popupTracker.Untrack (iapDlg);
				GameObject.Destroy (iapDlg.gameObject);
				iapDlg = null;
			}
		}
		#endregion

		#region AlbumDlgCtr
		public string albumDlgPath;
		public AlbumDlgCtrl albumDlg;
		public AlbumDlgCtrl GetAlbumDlg(){
			return albumDlg;
		}
		public AlbumDlgCtrl OpenAlbumDlg(){
			if (albumDlg != null) {
				return albumDlg;
			}
			var prefab = Load (albumDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+albumDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<AlbumDlgCtrl> ();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			albumDlg = ctrl;
			popupTracker.Track (ctrl);
			return albumDlg;
		}

		public void CloseAlbumDlg(){
			if (albumDlg != null) {
				popupTracker.Untrack (albumDlg);
				GameObject.Destroy (albumDlg.gameObject);
				albumDlg = null;
			}
		}
		#endregion

		#region BuyItemDlg
		public string buyItemDlgPath;
		public BuyItemDlgCtrl buyItemDlg;
		public BuyItemDlgCtrl BuyItemDlg{ get { return buyItemDlg; } }
		public BuyItemDlgCtrl OpenBuyItemDlg(){
			if (buyItemDlg != null) {
				return buyItemDlg;
			}
			var prefab = Load (buyItemDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+buyItemDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<BuyItemDlgCtrl> ();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			buyItemDlg = ctrl;
			popupTracker.Track (ctrl);
			return buyItemDlg;
		}

		public void CloseBuyItemDlg(){
			if (buyItemDlg != null) {
				popupTracker.Untrack (buyItemDlg);
				GameObject.Destroy (buyItemDlg.gameObject);
				buyItemDlg = null;
			}
		}
		#endregion

		#region StartGameDlg
		public string startGameDlgPath;
		public StartGameDlgCtrl startGameDlg;
		public StartGameDlgCtrl OpenStartGameDlg(){
			if (startGameDlg != null) {
				return startGameDlg;
			}
			var prefab = Load (startGameDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+startGameDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<StartGameDlgCtrl> ();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			startGameDlg = ctrl;
			popupTracker.Track (ctrl);
			return startGameDlg;
		}

		public void CloseStartGameDlg(){
			if (startGameDlg != null) {
				popupTracker.Untrack (startGameDlg);
				GameObject.Destroy (startGameDlg.gameObject);
				startGameDlg = null;
			}
		}
		#endregion
			
		#region ScoreDlg
		public string scoreDlgPath;
		public ScoreDlgCtrl scoreDlg;
		public ScoreDlgCtrl OpenScoreDlgCtrl(){
			if (scoreDlg != null) {
				return scoreDlg;
			}
			var prefab = Load (scoreDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+scoreDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<ScoreDlgCtrl> ();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			scoreDlg = ctrl;
			popupTracker.Track (ctrl);
			return scoreDlg;
		}

		public void CloseScoreDlg(){
			if (scoreDlg != null) {
				popupTracker.Untrack (scoreDlg);
				GameObject.Destroy (scoreDlg.gameObject);
				scoreDlg = null;
			}
		}
		#endregion

		#region StorageDlg
		public string storageDlgPath;
		public ItemStorageDlgCtrl storageDlg;
		public ItemStorageDlgCtrl OpenStorageDlg(){
			if (storageDlg != null) {
				return storageDlg;
			}
			var prefab = Load (storageDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+storageDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<ItemStorageDlgCtrl> ();
			pop.transform.SetParent (anchorMenu, false);
			// TODO 將面板放到最上層, 以下在探索頁沒有用
			/*
			var t = pop.transform.localPosition;
			t.z = -2;
			pop.transform.localPosition = t;
			*/
			pop.SetActive (true);
			storageDlg = ctrl;
			popupTracker.Track (ctrl);
			return storageDlg;
		}

		public void CloseStorageDlg(){
			if (storageDlg != null) {
				popupTracker.Untrack (storageDlg);
				GameObject.Destroy (storageDlg.gameObject);
				storageDlg = null;
			}
		}
		#endregion


		#region NewCatDlg
		public string newCatDlgPath;
		public NewDlgCtrl newCatDlg;
		public NewDlgCtrl OpenNewCatDlg(){
			if (newCatDlg != null) {
				return newCatDlg;
			}
			var prefab = Load (newCatDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+newCatDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<NewDlgCtrl> ();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			newCatDlg = ctrl;
			popupTracker.Track (ctrl);
			return newCatDlg;
		}
		public NewDlgCtrl GetNewCatDlg(){
			return newCatDlg;
		}
		public void CloseNewCatDlg(){
			if (newCatDlg != null) {
				popupTracker.Untrack (newCatDlg);
				GameObject.Destroy (newCatDlg.gameObject);
				newCatDlg = null;
			}
		}
		#endregion

		#region NewBPhotoDlg
		public string newBPhotoDlgPath;
		public List<NewDlgCtrl> newBPhotoDlgs;
		public NewDlgCtrl OpenNewBPhotoDlg(){
			var prefab = Load (newBPhotoDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+newBPhotoDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<NewDlgCtrl> ();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			newBPhotoDlgs.Add (ctrl);
			popupTracker.Track (ctrl);
			return ctrl;
		}
		public NewDlgCtrl GetTopNewBPhotoDlg(){
			if (newBPhotoDlgs.Count == 0) {
				throw new UnityException ("沒有開啟NewBPhotoDlg");
			}
			return newBPhotoDlgs [newBPhotoDlgs.Count - 1];
		}

		public void CloseNewBPhotoDlg(NewDlgCtrl ctrl){
			if (newBPhotoDlgs.Contains (ctrl)) {
				newBPhotoDlgs.Remove (ctrl);
				popupTracker.Untrack (ctrl);
				GameObject.Destroy (ctrl.gameObject);
			}
		}
		#endregion

		#region NewSPhotoDlg
		public string newSPhotoDlgPath;
		public List<NewDlgCtrl> newSPhotoDlgs;
		public NewDlgCtrl OpenNewSPhotoDlg(){
			var prefab = Load (newSPhotoDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+newSPhotoDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<NewDlgCtrl> ();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			newSPhotoDlgs.Add (ctrl);
			popupTracker.Track (ctrl);
			return ctrl;
		}
		public NewDlgCtrl GetTopNewSPhotoDlg(){
			if (newSPhotoDlgs.Count == 0) {
				return null;
			}
			return newSPhotoDlgs [newSPhotoDlgs.Count - 1];
		}
		public void CloseNewSPhotoDlg(NewDlgCtrl ctrl){
			if (newSPhotoDlgs.Contains (ctrl)) {
				newSPhotoDlgs.Remove (ctrl);
				popupTracker.Untrack (ctrl);
				GameObject.Destroy (ctrl.gameObject);
			}
		}
		#endregion


		#region SelectCatDlgCtrl
		public string selectCatDlgPath;
		public SelectCatDlgCtrl selectCatDlg;
		public SelectCatDlgCtrl GetSelectCatDlgCtrl(){
			return selectCatDlg;
		}
		public SelectCatDlgCtrl OpenSelectCatDlg(){
			if (selectCatDlg != null) {
				return selectCatDlg;
			}
			var prefab = Load (selectCatDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+selectCatDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<SelectCatDlgCtrl> ();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			selectCatDlg = ctrl;
			popupTracker.Track (ctrl);
			return selectCatDlg;
		}
		public IEnumerator OpenSelectCatDlgAsync(RemixApi.Either<SelectCatDlgCtrl> answer){
			if (selectCatDlg != null) {
				answer.Ref = selectCatDlg;
				yield return null;
			}
			var prefab = new RemixApi.Either<UnityEngine.Object> ();
			yield return Util.Instance.LoadAssetAsync (prefab, selectCatDlgPath, typeof(UnityEngine.Object));

			if (prefab.Ref == null) {
				throw new UnityException ("沒有這個Prefab:"+selectCatDlgPath);
			}
			var pop = GameObject.Instantiate (prefab.Ref) as GameObject;
			var ctrl = pop.GetComponent<SelectCatDlgCtrl> ();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			selectCatDlg = ctrl;
			popupTracker.Track (ctrl);
			answer.Ref = selectCatDlg;
		}
		public void CloseSelectCatDlg(){
			if (selectCatDlg != null) {
				popupTracker.Untrack (selectCatDlg);
				GameObject.Destroy (selectCatDlg.gameObject);
				selectCatDlg = null;
			}
		}
		#endregion


		#region PhotoDlg
		public string photoDlgPath;
		public PhotoDlgCtrl photoDlg;
		public PhotoDlgCtrl OpenPhotoDlg(){
			if (photoDlg != null) {
				return photoDlg;
			}
			var prefab = Load (photoDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+photoDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<PhotoDlgCtrl> ();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			photoDlg = ctrl;
			popupTracker.Track (ctrl);
			return photoDlg;
		}

		public void ClosePhotoDlg(){
			if (photoDlg != null) {
				popupTracker.Untrack (photoDlg);
				GameObject.Destroy (photoDlg.gameObject);
				photoDlg = null;
			}
		}
		#endregion


		#region BuyGachaDlg
		public string buyGachaDlgPath;
		public BuyGachaDlg buyGachaDlg;
		public BuyGachaDlg OpenBuyGachaDlg(){
			if (buyGachaDlg != null) {
				return buyGachaDlg;
			}
			var prefab = Load (buyGachaDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+buyGachaDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<BuyGachaDlg> ();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			buyGachaDlg = ctrl;
			popupTracker.Track (ctrl);
			return buyGachaDlg;
		}

		public void CloseBuyGachaDlg(){
			if (buyGachaDlg != null) {
				popupTracker.Untrack (buyGachaDlg);
				GameObject.Destroy (buyGachaDlg.gameObject);
				buyGachaDlg = null;
			}
		}
		#endregion


		#region MailGiftDlg
		public string mailGiftDlgPath;
		public MailGiftDlg mailGiftDlg;
		public MailGiftDlg OpenMailGiftDlg(){
			if (mailGiftDlg != null) {
				return mailGiftDlg;
			}
			var prefab = Load (mailGiftDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+mailGiftDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<MailGiftDlg> ();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			mailGiftDlg = ctrl;
			popupTracker.Track (ctrl);
			return mailGiftDlg;
		}

		public void CloseMailGiftDlg(){
			if (mailGiftDlg != null) {
				popupTracker.Untrack (mailGiftDlg);
				GameObject.Destroy (mailGiftDlg.gameObject);
				mailGiftDlg = null;
			}
		}
		#endregion

		#region EventDlg
		public string eventDlgPath;
		public List<EventDlg> eventDlgs;
		public EventDlg OpenEventDlg(){
			var prefab = Load (eventDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+eventDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<EventDlg> ();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			popupTracker.Track (ctrl);
			eventDlgs.Add (ctrl);
			return ctrl;
		}
		public EventDlg GetTopEventDlg(){
			if (eventDlgs.Count == 0) {
				throw new UnityException ("沒有開啟NewSPhotoDlg");
			}
			return eventDlgs [eventDlgs.Count - 1];
		}
		public void CloseEventDlg(EventDlg ctrl){
			if (eventDlgs.Contains (ctrl)) {
				eventDlgs.Remove (ctrl);
				popupTracker.Untrack (ctrl);
				GameObject.Destroy (ctrl.gameObject);
			}
		}
		#endregion


		#region NewItemDlg
		public string newItemPath;
		public List<NewDlgCtrl> newItemDlgs;
		public NewDlgCtrl OpenNewItemDlg(){
			var prefab = Load (newItemPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+newItemPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<NewDlgCtrl> ();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			newItemDlgs.Add (ctrl);
			popupTracker.Track (ctrl);
			return ctrl;
		}
		public NewDlgCtrl GetTopNewItemDlg(){
			if (newItemDlgs.Count == 0) {
				return null;
			}
			return newItemDlgs [newItemDlgs.Count - 1];
		}

		public void CloseNewItemDlg(NewDlgCtrl ctrl){
			if (newItemDlgs.Contains (ctrl)) {
				newItemDlgs.Remove (ctrl);
				popupTracker.Untrack (ctrl);
				GameObject.Destroy (ctrl.gameObject);
			}
		}
		#endregion

		#region StaffDlg
		public string staffDlgPath;
		public StaffDlg staffDlg;
		public StaffDlg OpenStaffDlg(){
			if (staffDlg != null) {
				return staffDlg;
			}
			var prefab = Load (staffDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+staffDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			// 純粹的替代物件
			var ctrl = pop.gameObject.GetComponent<StaffDlg> ();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			popupTracker.Track (ctrl);
			staffDlg = ctrl;
			return ctrl;
		}
		public void CloseStaffDlg(){
			if (staffDlg != null) {
				popupTracker.Untrack (staffDlg);
				GameObject.Destroy (staffDlg.gameObject);
				staffDlg = null;
			}
		}
		#endregion

		#region TutorialDlg
		public string tutorialDlgPath;
		public TutorialDlg tutorialDlg;
		public TutorialDlg OpenTutorialDlg(){
			if (tutorialDlg != null) {
				return tutorialDlg;
			}
			var prefab = Load (tutorialDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+tutorialDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<TutorialDlg>();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			popupTracker.Track (ctrl);
			tutorialDlg = ctrl;
			return ctrl;
		}
		public void CloseTutorialDlg(){
			if (tutorialDlg != null) {
				popupTracker.Untrack (tutorialDlg);
				GameObject.Destroy (tutorialDlg.gameObject);
				tutorialDlg = null;
			}
		}
		#endregion


		#region TransferDlg
		public string transferDlgPath;
		public TransferDlg transferDlg;
		public TransferDlg OpenTransferDlg(){
			if (transferDlg != null) {
				return transferDlg;
			}
			var prefab = Load (transferDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+transferDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<TransferDlg>();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			popupTracker.Track (ctrl);
			transferDlg = ctrl;
			return ctrl;
		}
		public void CloseTransferDlg(){
			if (transferDlg != null) {
				popupTracker.Untrack (transferDlg);
				GameObject.Destroy (transferDlg.gameObject);
				transferDlg = null;
			}
		}
		#endregion


		#region InviteDlg
		public string inviteDlgPath;
		public InviteDlg inviteDlg;
		public InviteDlg OpenInviteDlg(){
			if (inviteDlg != null) {
				return inviteDlg;
			}
			var prefab = Load (inviteDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+inviteDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<InviteDlg>();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			popupTracker.Track (ctrl);
			inviteDlg = ctrl;
			return ctrl;
		}
		public InviteDlg GetInviteDlg(){
			return inviteDlg;
		}
		public void CloseInviteDlg(){
			if (inviteDlg != null) {
				popupTracker.Untrack (inviteDlg);
				GameObject.Destroy (inviteDlg.gameObject);
				inviteDlg = null;
			}
		}
		#endregion


		#region MusicDlg
		public string musicDlgPath;
		public MusicDlg musicDlg;
		public MusicDlg OpenMusicDlg(){
			if (musicDlg != null) {
				return musicDlg;
			}
			var prefab = Load (musicDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+musicDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<MusicDlg>();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			popupTracker.Track (ctrl);
			musicDlg = ctrl;
			return ctrl;
		}
		public MusicDlg GetMusicDlg(){
			return musicDlg;
		}
		public void CloseMusicDlg(){
			if (musicDlg != null) {
				popupTracker.Untrack (musicDlg);
				GameObject.Destroy (musicDlg.gameObject);
				musicDlg = null;
			}
		}
		#endregion


		#region StageItemDlg
		public string stageItemDlgPath;
		public StageItemDlg stageItemDlg;
		public StageItemDlg OpenStageItemDlg(){
			if (stageItemDlg != null) {
				return stageItemDlg;
			}
			var prefab = Load (stageItemDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+stageItemDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<StageItemDlg>();
			pop.transform.SetParent (anchorMenu, false);
			pop.SetActive (true);
			popupTracker.Track (ctrl);
			stageItemDlg = ctrl;
			return ctrl;
		}
		public void CloseStageItemDlg(){
			if (stageItemDlg != null) {
				popupTracker.Untrack (stageItemDlg);
				GameObject.Destroy (stageItemDlg.gameObject);
				stageItemDlg = null;
			}
		}
		#endregion
	}
}

