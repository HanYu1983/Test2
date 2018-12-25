using System;
using UnityEngine;

namespace Remix
{
	public class CaptureUI : MonoBehaviour
	{

		public bool IsInGameCapRoot(){
			var page = GetComponent<PageGroup> ();
			if (page.CurrentPageIdx != 0) {
				return false;
			}
			return true;
		}

		public GameCapRoot GetGameCapRoot(){
			var page = GetComponent<PageGroup> ();
			if (page.CurrentPageIdx != 0) {
				throw new UnityException ("請切換到遊戲地圖頁");
			}
			return page.CurrentPage.GetComponent<GameCapRoot> ();
		}

		public bool IsInWorldCapRoot(){
			var page = GetComponent<PageGroup> ();
			if (page.CurrentPageIdx != 1) {
				return false;
			}
			return true;
		}

		public WorldCapRoot GetWorldCapRoot(){
			var page = GetComponent<PageGroup> ();
			if (page.CurrentPageIdx != 1) {
				throw new UnityException ("請切換到遊戲地圖頁");
			}
			return page.CurrentPage.GetComponent<WorldCapRoot> ();
		}
	}
}

