using System;
using UnityEngine;

namespace Remix
{
	public class HomeUI : MonoBehaviour
	{
		public bool IsInHousePage(){
			var pg = GetComponent<PageGroup> ();
			if (pg.CurrentPageIdx != 2) {
				return false;
			} 
			return true;
		}

		public bool IsInCommunityPage(){
			var pg = GetComponent<PageGroup> ();
			if (pg.CurrentPageIdx != 1) {
				return false;
			} 
			return true;
		}

		public HousePage GetHousePage(){
			var pg = GetComponent<PageGroup> ();
			if (pg.CurrentPageIdx != 2) {
				throw new UnityException ("請先切換到房屋頁");
			}
			return pg.CurrentPage.GetComponent<HousePage> ();
		}

		public bool IsInMainPage(){
			var pg = GetComponent<PageGroup> ();
			if (pg.CurrentPageIdx != 0) {
				return false;
			} 
			return true;
		}

		public HomeUIMainPage GetMainPage(){
			var pg = GetComponent<PageGroup> ();
			if (pg.CurrentPageIdx != 0) {
				throw new UnityException ("請先切換到主頁");
			}
			var houseMap = pg.CurrentPage.GetComponentInChildren<HomeUIMainPage> ();
			if (houseMap == null) {
				throw new UnityException ("你忘了加入HomeUIMainPage");
			}
			return houseMap;
		}
	}
}

