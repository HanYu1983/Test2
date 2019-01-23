using System;
using UnityEngine;
using UnityEngine.UI;

namespace Remix
{
	public class LoadingDlg : MonoBehaviour
	{
		public Text textTitle;
		public PageGroup loadPics;

		public string Title {
			set {
				textTitle.text = value;
			}
		}

		public void RandomPic(){
			if (loadPics == null) {
				Debug.LogWarning ("loadPics not set. ignore RandomPic");
				return;
			}
			loadPics.ChangePage (UnityEngine.Random.Range (0, 3));
		}
	}
}

