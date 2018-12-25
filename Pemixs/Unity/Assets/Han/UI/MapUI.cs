using System;
using UnityEngine;
using TouchScript;

namespace Remix
{
	public class MapUI : MonoBehaviour
	{
		public bool IsInGameMapRoot(){
			var page = GetComponent<PageGroup> ();
			if (page.CurrentPageIdx != 0) {
				return false;
			}
			return true;
		}

		public GameMapRoot GetGameMapRoot(){
			if (IsInGameMapRoot () == false) {
				throw new UnityException ("請切換到地區頁");
			}
			var page = GetComponent<PageGroup> ();
			return page.CurrentPage.GetComponent<GameMapRoot>();
		}

		public bool IsInWorldMapRoot(){
			var page = GetComponent<PageGroup> ();
			if (page.CurrentPageIdx != 1) {
				return false;
			}
			return true;
		}

		public WorldMapRoot GetWorldMapRoot(){
			if (IsInWorldMapRoot () == false) {
				throw new UnityException ("請切換到大地圖頁");
			}
			var page = GetComponent<PageGroup> ();
			return page.CurrentPage.GetComponent<WorldMapRoot>();
		}
	}
}

