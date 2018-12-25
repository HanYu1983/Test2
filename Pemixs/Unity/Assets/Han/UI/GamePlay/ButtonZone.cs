using System;
using UnityEngine;
using System.Collections.Generic;

namespace Remix
{
	public class ButtonZone : MonoBehaviour
	{
		public List<GameUIButtonView> buttonView;
		public GameUIButtonView GetButtonView(int idx){
			return buttonView [idx];
		}
		public bool ClickButtonVisible {
			set {
				foreach (var b in buttonView) {
					b.gameObject.SetActive (value);
				}
			}
			get{
				return buttonView [0].isActiveAndEnabled;
			}
		}
	}
}

