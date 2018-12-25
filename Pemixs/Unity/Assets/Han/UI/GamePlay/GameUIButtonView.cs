using System;
using UnityEngine;
using UnityEngine.UI;

namespace Remix
{
	public class GameUIButtonView : MonoBehaviour
	{
		public Image shiningImage;
		public Image buttonImage;
		public Image feverImage;

		public void SetVisible(bool visible)
		{
			buttonImage.enabled = visible;
			shiningImage.enabled = visible;
			feverImage.enabled = visible;
		}

		public void Flash(bool isFever){
			Animator animator = shiningImage.gameObject.GetComponent<Animator>();
			animator.SetTrigger("flash");
			if (isFever == true) {
				animator = feverImage.gameObject.GetComponent<Animator> ();
				animator.SetTrigger ("flash");
			}
		}
	}
}

