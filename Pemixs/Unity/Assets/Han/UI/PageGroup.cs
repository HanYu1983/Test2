using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Remix
{

	public class PageGroup : MonoBehaviour
	{
		public Transform anchor;
		public List<GameObject> pages;
		public List<string> paths;
		public bool isPrefab;

		public GameObject currentPage;
		public int currentPageIdx;

		public Action<PageGroup, GameObject> OnPageActive = delegate {};
		public Action<PageGroup, GameObject> OnPageInactive = delegate {};

		public bool HasCurrentPage{ get { return currentPage != null; } }

		void Awake(){
			currentPageIdx = -1;
		}

		public GameObject CurrentPage{
			get{
				if (currentPage == null) {
					throw new UnityException ("你還沒有換到任何頁");
				}
				return currentPage; 
			} 
		}

		public int CurrentPageIdx{
			get{
				if (currentPageIdx == -1) {
					throw new UnityException ("你還沒有換到任何頁");
				}
				return currentPageIdx; 
			} 
		}

		public void NextPage(){
			var next = CurrentPageIdx + 1;
			if (isPrefab) {
				if (next >= paths.Count) {
					next = 0;
				}
			} else {
				if (next >= pages.Count) {
					next = 0;
				}
			}
			ChangePage (next);
		}

		public void PrevPage(){
			var next = CurrentPageIdx - 1;
			if (isPrefab) {
				if (next < 0) {
					next = paths.Count-1;
				}
			} else {
				if (next < 0) {
					next = pages.Count-1;
				}
			}
			ChangePage (next);
		}

		public void ChangePage(string path){
			Clear ();
			var page = Util.Instance.GetPrefab (path, null);
			if (anchor == null) {
				anchor = this.transform;
			}
			page.transform.SetParent (anchor, false);
			page.SetActive (true);
			currentPage = page;
			currentPageIdx = 99;
			OnPageActive (this, page);
		}

		public void ChangePage(int idx){

			if (isPrefab) {
				if (idx < 0 || idx >= paths.Count) {
					Debug.LogWarning ("這頁還沒有做好:"+idx);
					return;
				}
				if (currentPageIdx == idx) {
					Debug.LogWarning ("同一個頁面，不必切換");
					return;
				}
				Clear ();
				var path = paths [idx];
				var page = Util.Instance.GetPrefab (path, null);
				if (anchor == null) {
					anchor = this.transform;
				}
				page.transform.SetParent (anchor, false);
				page.SetActive (true);
				currentPage = page;
				currentPageIdx = idx;
				OnPageActive (this, page);

			} else {
				if (idx < 0 || idx >= pages.Count) {
					Debug.LogWarning ("這頁還沒有做好");
					return;
				}
				if (currentPage == pages [idx]) {
					Debug.LogWarning ("同一個頁面，不必切換");
					return;
				}
				Clear ();
				var page = pages[idx];
				page.SetActive (true);
				currentPage = page;
				currentPageIdx = idx;
				OnPageActive (this, page);
			}
		}

		public IEnumerator ChangePageAsync(int idx){
			
			if (isPrefab) {
				if (idx < 0 || idx >= paths.Count) {
					Debug.LogWarning ("這頁還沒有做好");
					yield break;
				}
				if (currentPageIdx == idx) {
					Debug.LogWarning ("同一個頁面，不必切換");
					yield break;
				}
				Clear ();
				var path = paths [idx];
				var page = new RemixApi.Either<GameObject> ();
				yield return Util.Instance.GetPrefabAsync (page, path, null);
				if (anchor == null) {
					anchor = this.transform;
				}
				page.Ref.transform.SetParent (anchor, false);
				page.Ref.SetActive (true);
				currentPage = page.Ref;
				currentPageIdx = idx;
				OnPageActive (this, page.Ref);

			} else {
				if (idx < 0 || idx >= pages.Count) {
					Debug.LogWarning ("這頁還沒有做好");
					yield break;
				}
				if (currentPage == pages [idx]) {
					Debug.LogWarning ("同一個頁面，不必切換");
					yield break;
				}
				Clear ();
				var page = pages[idx];
				page.SetActive (true);
				currentPage = page;
				currentPageIdx = idx;
				OnPageActive (this, page);
			}
		}

		public void Clear(){
			if (isPrefab) {
				if (currentPage != null) {
					OnPageInactive (this, currentPage);
					GameObject.Destroy (currentPage);
					currentPage = null;
					currentPageIdx = -1;
					Util.Instance.RequestUnloadUnusedAssets ();
				}
			} else {
				if (currentPage != null) {
					OnPageInactive (this, currentPage);
					currentPage.SetActive (false);
					currentPage = null;
					currentPageIdx = -1;
				}
			}
		}
	}
}

