using System;
using UnityEngine;
using UnityEngine.UI;

namespace View
{
	public class MessagePopup : MonoBehaviour
	{
		public Text text;
		public string Message{
			set{
				text.text = value;
			}
		}
	}
}

