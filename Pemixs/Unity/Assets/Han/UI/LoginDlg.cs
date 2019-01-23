using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Remix
{
	public class LoginDlg : MonoBehaviour
	{
		public Text textTitle;
		public InputField textName;
		public Dictionary<string, object> metaData = new Dictionary<string, object>();

		public string NameText {
			get {
				return textName.text;
			}
			set{
				textName.text = value;
			}
		}

		public Dictionary<string, object> MetaData{
			get{
				return metaData;
			}
		}

		public string Title {
			set {
				textTitle.text = value;
			}
			get {
				return textTitle.text;
			}
		}
	}
}

