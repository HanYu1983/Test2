using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Remix
{
	public class EventDlg : MonoBehaviour
	{
		public Text textTitle;
		public Image imgEventPic;

		public Dictionary<string,object> MetaData{ get; set; }

		public void SetTitle(string title){
			textTitle.text = title;
		}

		public void SetPic(Sprite img){
			imgEventPic.sprite = img;
		}
	}
}

