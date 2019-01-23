using UnityEngine;
using System.Collections;
using Remix;

public class LevelSoundDataCtrl : MonoBehaviour 
{
	public AudioClip levelBGM;
	public AudioClip feverBGM;

	public void MuteAndPlayLevelBGM(){
		UIEventFacade.OnAudioClipRequest.OnNext (new UIEventFacade.AudioClipRequest () {
			Type = UIEventFacade.AudioClipRequest.TypeGameMusic,
			IsLoop = false,
			Clip = levelBGM,
			TrackID = GameConfig.GAME_BGM_TRACK_ID,
			IsMute = true
		});
	}

	public void PlayLevelBGM()
	{
		// 偷懶處理
		// 將menu音樂關閉
		UIEventFacade.OnAudioClipRequest.OnNext (new UIEventFacade.AudioClipRequest () {
			TrackID = GameConfig.MENU_BGM_TRACK_ID,
			IsPause = true
		});

		UIEventFacade.OnAudioClipRequest.OnNext (new UIEventFacade.AudioClipRequest () {
			Type = UIEventFacade.AudioClipRequest.TypeGameMusic,
			IsLoop = false,
			Clip = levelBGM,
			TrackID = GameConfig.GAME_BGM_TRACK_ID,
			IsPause = false
		});
	}

	public void PauseLevelBGM(){
		UIEventFacade.OnAudioClipRequest.OnNext (new UIEventFacade.AudioClipRequest () {
			Type = UIEventFacade.AudioClipRequest.TypeGameMusic,
			Clip = levelBGM,
			TrackID = GameConfig.GAME_BGM_TRACK_ID,
			IsPause = true
		});
	}

	public void PlayFeverBGM()
	{
		// 開啟Fever的BGM疊上本來的音樂，所以不需要下TrackID
		UIEventFacade.OnAudioClipRequest.OnNext (new UIEventFacade.AudioClipRequest () {
			Type = UIEventFacade.AudioClipRequest.TypeGameMusic,
			IsLoop = false,
			TrackID = GameConfig.FEVER_BGM_TRACK_ID,
			Clip = feverBGM
		});
	}

	public void PauseFeverBGM(){
		UIEventFacade.OnAudioClipRequest.OnNext (new UIEventFacade.AudioClipRequest () {
			Type = UIEventFacade.AudioClipRequest.TypeGameMusic,
			Clip = levelBGM,
			TrackID = GameConfig.FEVER_BGM_TRACK_ID,
			IsPause = true
		});
	}
}
