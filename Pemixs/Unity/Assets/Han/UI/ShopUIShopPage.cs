using System;
using UnityEngine;
using UnityEngine.UI;

namespace Remix
{
	public class ShopUIShopPage : MonoBehaviour
	{
		public GameObject catIconAnchor;

		public void SetCatImage(Sprite sprite){
			catIconAnchor.GetComponent<Image> ().sprite = sprite;
		}
	}
}

