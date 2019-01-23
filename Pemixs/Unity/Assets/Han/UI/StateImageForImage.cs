using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Remix
{
	public class StateImageForImage : MonoBehaviour
	{
		public List<Sprite> sprites;
		public Image forImage;
		public int currentIdx;

		public int CurrentIdx{ 
			get{ return currentIdx; }
			set{ ChangeTo (value); }
		}

		public void ChangeTo(int idx){
			if (idx < 0 || idx >= sprites.Count) {
				Debug.LogWarning ("沒有這個idx："+idx);
				return;
			}
			if (sprites [idx] == null) {
				Debug.LogWarning ("這個idx沒有設定："+idx);
				return;
			}
			forImage.sprite = sprites [idx];
			currentIdx = idx;
		}
	}
}

