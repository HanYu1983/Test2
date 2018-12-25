using System;
using UnityEngine;

namespace Remix
{
	public class ShopUI : MonoBehaviour
	{
		public bool IsInShopPage(){
			var pg = GetComponent<PageGroup> ();
			if (pg.CurrentPageIdx != 0) {
				return false;
			} 
			return true;
		}

		public ShopUIShopPage GetShopPage(){
			var pg = GetComponent<PageGroup> ();
			if (pg.CurrentPageIdx != 0) {
				throw new UnityException ("請先切換到主頁");
			}
			var houseMap = pg.CurrentPage.GetComponentInChildren<ShopUIShopPage> ();
			if (houseMap == null) {
				throw new UnityException ("你忘了加入ShopUIShopPage");
			}
			return houseMap;
		}

		public bool IsInIAPShopPage(){
			var pg = GetComponent<PageGroup> ();
			if (pg.CurrentPageIdx != 1) {
				return false;
			} 
			return true;
		}

		public ShopUIIAPShopPage GetIAPShopPage(){
			var pg = GetComponent<PageGroup> ();
			if (pg.CurrentPageIdx != 1) {
				throw new UnityException ("請先切換到IAPShop");
			}
			var houseMap = pg.CurrentPage.GetComponentInChildren<ShopUIIAPShopPage> ();
			if (houseMap == null) {
				throw new UnityException ("你忘了加入ShopUIIAPShopPage");
			}
			return houseMap;
		}

		public bool IsInRadioShopPage(){
			var pg = GetComponent<PageGroup> ();
			if (pg.CurrentPageIdx != 2) {
				return false;
			} 
			return true;
		}

		public ShopUIRadioShopPage GetRadioShopPage(){
			var pg = GetComponent<PageGroup> ();
			if (pg.CurrentPageIdx != 2) {
				throw new UnityException ("請先切換到RadioShopPage");
			}
			var houseMap = pg.CurrentPage.GetComponentInChildren<ShopUIRadioShopPage> ();
			if (houseMap == null) {
				throw new UnityException ("你忘了加入ShopUIRadioShopPage");
			}
			return houseMap;
		}
	}
}

