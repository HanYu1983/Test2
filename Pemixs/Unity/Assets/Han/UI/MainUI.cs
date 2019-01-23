using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Remix
{
	public class MainUI : MonoBehaviour
	{
		
		public PageGroup subGroup;
		public RectPosition windowRect;
		public RectPosition WindowRect{ get { return windowRect; } }
		public StateImageForImage musicStateImage, soundStateImage;

		public void InitComponent(){
			
		}

		public bool IsMusicOn {
			get {
				return musicStateImage.CurrentIdx == 0;
			}
			set {
				musicStateImage.CurrentIdx = value ? 0 : 1;
			}
		}

		public bool IsSoundOn {
			get {
				return soundStateImage.CurrentIdx == 0;
			}
			set {
				soundStateImage.CurrentIdx = value ? 0 : 1;
			}
		}

		public bool IsInHomeUI(){
			var pageGroup = GetComponent<PageGroup> ();
			if (pageGroup.HasCurrentPage == false) {
				return false;
			}
			var page = pageGroup.CurrentPage;
			var ui = page.GetComponent<HomeUI> ();
			if (ui == null) {
				return false;
			}
			return true;
		}

		public HomeUI GetHomeUI(){
			var pageGroup = GetComponent<PageGroup> ();
			var page = pageGroup.CurrentPage;
			var ui = page.GetComponent<HomeUI> ();
			if (ui == null) {
				throw new UnityException ("請先切換到Home頁，或者忘了加入Component");
			}
			return ui;
		}

		public bool IsInShopUI(){
			var pageGroup = GetComponent<PageGroup> ();
			if (pageGroup.HasCurrentPage == false) {
				return false;
			}
			var page = pageGroup.CurrentPage;
			var ui = page.GetComponent<ShopUI> ();
			if (ui == null) {
				return false;
			}
			return true;
		}

		public ShopUI GetShopUI(){
			var pageGroup = GetComponent<PageGroup> ();
			var page = pageGroup.CurrentPage;
			var ui = page.GetComponent<ShopUI> ();
			if (ui == null) {
				throw new UnityException ("請先切換到Shop頁，或者忘了加入Component");
			}
			return ui;
		}

		public bool IsInMapUI(){
			var pageGroup = GetComponent<PageGroup> ();
			if (pageGroup.HasCurrentPage == false) {
				return false;
			}
			var page = pageGroup.CurrentPage;
			var ui = page.GetComponent<MapUI> ();
			if (ui == null) {
				return false;
			}
			return true;
		}

		public bool IsInConfigUI(){
			var pageGroup = GetComponent<PageGroup> ();
			if (pageGroup.HasCurrentPage == false) {
				return false;
			}
			if (pageGroup.CurrentPageIdx != 4) {
				return false;
			}
			return true;
		}

		public ConfigUI GetConfigUI(){
			var pageGroup = GetComponent<PageGroup> ();
			var page = pageGroup.CurrentPage;
			var ui = page.GetComponent<ConfigUI> ();
			if (ui == null) {
				throw new UnityException ("請先切換到ConfigUI頁，或者忘了加入Component");
			}
			return ui;
		}

		public MapUI GetMapUI(){
			var pageGroup = GetComponent<PageGroup> ();
			var page = pageGroup.CurrentPage;
			var ui = page.GetComponent<MapUI> ();
			if (ui == null) {
				throw new UnityException ("請先切換到Map頁，或者忘了加入Component");
			}
			return ui;
		}

		public bool IsInCaptureUI(){
			var pageGroup = GetComponent<PageGroup> ();
			if (pageGroup.HasCurrentPage == false) {
				return false;
			}
			var page = pageGroup.CurrentPage;
			var ui = page.GetComponent<CaptureUI> ();
			if (ui == null) {
				return false;
			}
			return true;
		}

		public CaptureUI GetCaptureUI(){
			var pageGroup = GetComponent<PageGroup> ();
			var page = pageGroup.CurrentPage;
			var ui = page.GetComponent<CaptureUI> ();
			if (ui == null) {
				throw new UnityException ("請先切換到Capture頁，或者忘了加入Component");
			}
			return ui;
		}

		public void ChangePage(Transform anchor, int idx){
			var pageGroup = GetComponent<PageGroup> ();
			pageGroup.anchor = anchor;

			var subGroupPage = subGroup.GetComponent<PageGroup> ();
			pageGroup.ChangePage (idx);
			subGroupPage.ChangePage (idx);

			ChangeSubPage (0);
		}

		public void ChangeSubPage(int idx){
			var pageGroup = GetComponent<PageGroup> ();
			var page = pageGroup.CurrentPage;
			if (page == null) {
				Debug.LogWarning ("你還沒選定一個主頁，不能切換子頁");
				return;
			}
			if (page.GetComponent<PageGroup> () != null) {
				page.GetComponent<PageGroup> ().ChangePage (idx);
			} else {
				Debug.LogWarning ("沒有子頁可切換，請設定PageGroup");
			}
		}

		#region async
		public IEnumerator ChangePageAsync(Transform anchor, int idx){
			var pageGroup = GetComponent<PageGroup> ();
			pageGroup.anchor = anchor;

			var subGroupPage = subGroup.GetComponent<PageGroup> ();
			yield return pageGroup.ChangePageAsync (idx);
			yield return subGroupPage.ChangePageAsync (idx);

			yield return ChangeSubPageAsync (0);
		}

		public IEnumerator ChangeSubPageAsync(int idx){
			var pageGroup = GetComponent<PageGroup> ();
			var page = pageGroup.CurrentPage;
			if (page == null) {
				Debug.LogWarning ("你還沒選定一個主頁，不能切換子頁");
				yield break;
			}
			if (page.GetComponent<PageGroup> () != null) {
				yield return page.GetComponent<PageGroup> ().ChangePageAsync (idx);
			} else {
				Debug.LogWarning ("沒有子頁可切換，請設定PageGroup");
			}
		}
		#endregion

		public Text nameText;
		public Text moneyText;
		public Text hpText;
		public Text goldText;
		public Image catImage;
		public Image hpBarImage;
		public Text pageNameText;
		public Tips tips;

		public Tips Tips {
			get{ return tips; }
		}

		public void SetName(string text){
			nameText.text = text;
		}

		public void SetPageName(string text){
			pageNameText.text = text;
		}

		public void SetMoney(int v){
			moneyText.text = v+"";
		}

		public void SetHp(int v, int maxHp){
			hpText.text = v+"";
			var scale = v / (float)maxHp;
			var s = hpBarImage.transform.localScale;
			s.x = scale;
			hpBarImage.transform.localScale = s;
		}

		public void SetGold(int v){
			goldText.text = v+"";
		}

		public void SetCatImage(Sprite cat){
			catImage.sprite = cat;
		}

		public GameObject GetCatImageAnchor(){
			return catImage.gameObject;
		}
	}
}

