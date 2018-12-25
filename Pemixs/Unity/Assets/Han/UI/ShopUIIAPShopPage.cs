using System;
using UnityEngine;
using UnityEngine.UI;

namespace Remix
{
	public class ShopUIIAPShopPage : MonoBehaviour
	{
		public GameObject catIconAnchor;
		public GameObject adIcon;

		public void SetCatImage(Sprite sprite){
			catIconAnchor.GetComponent<Image> ().sprite = sprite;
		}

		public bool IsAdIconVisible{
			set {
				adIcon.SetActive (value);
			}
		}
	}
}

