using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Remix
{
	public class View : MonoBehaviour
	{
		public GameObject preLoadingDlg;
		public PopupTracker popupTracker;
		public HandleAssetBundle handleAssetBundle;

		public PopupTracker PopupTracker{ get { return popupTracker; } }

		public Menu mainMenu;
		public Game game;
		public Game Game{ 
			get { 
				if (game == null) {
					throw new UnityException ("請先進入遊戲");
				}
				return game; 
			} 
		}

		public Menu GetMainMenu(){
			var menu = GetComponentInChildren<Menu> ();
			return menu;
		}

		public bool IsInMenu(){
			var pg = GetComponent<PageGroup> ();
			return pg.CurrentPageIdx == 1;
		}

		public void ChangeToTitle(){
			var pg = GetComponent<PageGroup> ();
			pg.ChangePage (0);
		}

		public void ChangeToTitleCN(){
			var pg = GetComponent<PageGroup> ();
			pg.ChangePage ("UI/StartMenuCN");
		}

		public IEnumerator ChangeToMenuAsync(RemixApi.Either<Menu> answer){
			var pg = GetComponent<PageGroup> ();
			yield return pg.ChangePageAsync (1);

			var menu = GetMainMenu ();
			menu.popupTracker = popupTracker;

			menu.InitComponent ();
			this.mainMenu = menu;
			answer.Ref = menu;
		}

		public Menu ChangeToMenu(){
			var pg = GetComponent<PageGroup> ();
			pg.ChangePage (1);
			var menu = GetMainMenu ();
			menu.popupTracker = popupTracker;

			menu.InitComponent ();
			this.mainMenu = menu;
			return this.mainMenu;
		}

		public bool IsInGamePlay(){
			var pg = GetComponent<PageGroup> ();
			return pg.CurrentPageIdx == 2;
		}

		public bool IsInvalidPage{
			get{
				var pg = GetComponent<PageGroup> ();
				return pg.HasCurrentPage == false;
			}
		}

		public void ChangeToGamePlay(){
			var pg = GetComponent<PageGroup> ();
			pg.ChangePage (2);
			var game = pg.CurrentPage.GetComponentInChildren<Game>();
			if (game == null) {
				throw new UnityException ("切換到的頁面必須有Game");
			}
			this.game = game;
		}

		public bool IsInDelayTool(){
			var pg = GetComponent<PageGroup> ();
			return pg.CurrentPageIdx == 3;
		}

		public void ChangeToDelayTool(){
			var pg = GetComponent<PageGroup> ();
			pg.ChangePage (3);
			var game = pg.CurrentPage.GetComponentInChildren<Game>();
			if (game == null) {
				throw new UnityException ("切換到的頁面必須有Game");
			}
			this.game = game;
		}

		public bool IsInTutorial(){
			var pg = GetComponent<PageGroup> ();
			return pg.CurrentPageIdx == 4;
		}

		public void ChangeToTutorial(){
			var pg = GetComponent<PageGroup> ();
			pg.ChangePage (4);
			var game = pg.CurrentPage.GetComponentInChildren<Game>();
			if (game == null) {
				throw new UnityException ("切換到的頁面必須有Game");
			}
			this.game = game;
		}

		public LoadingDlg loadingDlg;
		public LoadingDlg OpenLoadingDlg(){
			if (preLoadingDlg == null) {
				throw new UnityException ("沒有設定preLoadingDlg");
			}
			if (loadingDlg != null) {
				return loadingDlg;
			}
			var pop = GameObject.Instantiate (preLoadingDlg);
			var ctrl = pop.GetComponent<LoadingDlg>();
			pop.transform.SetParent (this.transform, false);
			pop.SetActive (true);
			loadingDlg = ctrl;
			PopupTracker.Track (loadingDlg);
			return loadingDlg;
		}

		public void CloseLoadingDlg(){
			if (loadingDlg != null) {
				PopupTracker.Untrack (loadingDlg, true);
				GameObject.Destroy (loadingDlg.gameObject);
				loadingDlg = null;
			}
		}


		#region LoginDlg
		public string loginDlgPath;
		public LoginDlg loginDlg;
		public LoginDlg OpenLoginDlg(){
			if (loginDlg != null) {
				return loginDlg;
			}
			var prefab = Resources.Load (loginDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+loginDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			pop.transform.SetParent (this.transform, false);
			var ctrl = pop.GetComponent<LoginDlg>();
			pop.SetActive (true);
			loginDlg = ctrl;
			return ctrl;
		}
		public void CloseLoginDlg(){
			if (loginDlg != null) {
				GameObject.Destroy (loginDlg.gameObject);
				loginDlg = null;
			}
		}
		#endregion


		#region DownloadDlg
		public string downloadDlgPath;
		public DownloadDlg downloadDlg;
		public DownloadDlg OpenDownloadDlg(){
			if (downloadDlg != null) {
				return downloadDlg;
			}
			var prefab = Resources.Load (downloadDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+downloadDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			pop.transform.SetParent (this.transform, false);
			var ctrl = pop.GetComponent<DownloadDlg>();
			pop.SetActive (true);
			downloadDlg = ctrl;
			PopupTracker.Track (downloadDlg);
			return ctrl;
		}
		public void CloseDownloadDlg(){
			if (downloadDlg != null) {
				PopupTracker.Untrack (downloadDlg, true);
				GameObject.Destroy (downloadDlg.gameObject);
				downloadDlg = null;
			}
		}
		#endregion

		#region MessageDlg
		public string messageDlgPath;
		public List<MessageDlg> messageDlgs;
		public MessageDlg OpenMessageDlg(){
			var prefab = Resources.Load (messageDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+messageDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			var ctrl = pop.GetComponent<MessageDlg> ();
			pop.transform.SetParent (this.transform, false);
			pop.SetActive (true);
			messageDlgs.Add (ctrl);
			popupTracker.Track (ctrl);
			return ctrl;
		}
		public MessageDlg GetTopMessageDlg(){
			if (messageDlgs.Count == 0) {
				throw new UnityException ("沒有開啟MessageDlg");
			}
			return messageDlgs [messageDlgs.Count - 1];
		}

		public void CloseMessageDlg(MessageDlg ctrl){
			if (messageDlgs.Contains (ctrl)) {
				messageDlgs.Remove (ctrl);
				popupTracker.Untrack (ctrl);
				GameObject.Destroy (ctrl.gameObject);
			}
		}
		#endregion

		#region LanguageDlg
		public string languageDlgPath;
		public LanguageDlg languageDlg;
		public LanguageDlg OpenLanguageDlg(){
			if (languageDlg != null) {
				return languageDlg;
			}
			var prefab = Resources.Load (languageDlgPath);
			if (prefab == null) {
				throw new UnityException ("沒有這個Prefab:"+languageDlgPath);
			}
			var pop = GameObject.Instantiate (prefab) as GameObject;
			// 純粹的替代物件
			var ctrl = pop.gameObject.GetComponent<LanguageDlg>();
			pop.transform.SetParent (this.transform, false);
			pop.SetActive (true);
			popupTracker.Track (ctrl);
			languageDlg = ctrl;
			return ctrl;
		}
		public void CloseLanguageDlg(){
			if (languageDlg != null) {
				popupTracker.Untrack (languageDlg);
				GameObject.Destroy (languageDlg.gameObject);
				languageDlg = null;
			}
		}
		#endregion
	}
}

