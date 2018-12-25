using System;
using UnityEngine;
using UnityEngine.UI;

namespace Remix
{
	public class StaffDlg : MonoBehaviour
	{
		public Text textTitle;

		public string Title {
			set {
				textTitle.text = value;
			}
		}
	}
}

