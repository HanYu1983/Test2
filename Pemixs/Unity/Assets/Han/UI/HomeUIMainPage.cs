using System;
using UnityEngine;
using UnityEngine.UI;

namespace Remix
{
	public class HomeUIMainPage : MonoBehaviour
	{
		public GameObject catIconAnchor;
		public Text textUnreadMail, textUnreadEvent;

		public void SetCatImage(Sprite sprite){
			catIconAnchor.GetComponent<Image> ().sprite = sprite;
		}

		public void SetUnreadMailCount(int cnt){
			textUnreadMail.text = cnt + "";
		}

		public void SetUnreadEventCount(int cnt){
			textUnreadEvent.text = cnt + "";
		}
	}
}

