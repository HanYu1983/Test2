using System;
using UnityEngine;

namespace Remix
{
	// 沒有用到這個類
	public class BGMRequest : MonoBehaviour
	{
		public string TrackID = GameConfig.MENU_BGM_TRACK_ID;
		public AudioClip bgm;
		public void PlayBGM(){
			if (bgm == null) {
				Debug.LogWarning ("沒有設定bgm");
				return;
			}
			UIEventFacade.OnAudioClipRequest.OnNext (new UIEventFacade.AudioClipRequest () {
				Clip = bgm,
				IsLoop = true,
				TrackID = TrackID
			});
		}
	}
}

