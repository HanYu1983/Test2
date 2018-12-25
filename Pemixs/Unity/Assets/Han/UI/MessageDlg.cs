using System;
using UnityEngine;
using UnityEngine.UI;

namespace Remix
{
	public class MessageDlg : MonoBehaviour
	{
		public Text textTitle, textMsg;

		public string Title {
			set {
				textTitle.text = value;
			}
		}

		public string Message {
			set {
				textMsg.text = value;
			}
		}
	}
}

