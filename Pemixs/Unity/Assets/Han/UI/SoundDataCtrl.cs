using UnityEngine;
using System.Collections;
using Remix;
using System.Collections.Generic;

public class SoundDataCtrl : MonoBehaviour
{
	// 0. Speak sound
	// 1. Yellow button sound
	// 2. Pupple button sound
	// 3. Blue button sound
	// 4. Green button sound
	// 5. Red slide Up sound
	// 6. Red slide Down sound
	// 7. Attack sound
	// 8. Defense sound
	// 9. Hurt sound
	// 10. FeverStart sound
	public const int SPEAK_SOUND = 0;
	public const int ATK_SOUND = 7;
	public const int DEF_SOUND = 8;
	public const int HURT_SOUND = 9;
	public const int FEVER_SOUND = 10;

	public AudioClip[] soundArray;
	public AudioClip[] soundTypeAArray;
	public AudioClip[] soundTypeBArray;
	public AudioClip[] soundTypeCArray;
	public AudioClip[] soundTypeDArray;
	public AudioClip[] soundTypeEArray;
	public AudioClip[] soundTypeFArray;
	public AudioClip[] soundTypeGArray;
	public AudioClip[] soundTypeHArray;
	public AudioClip[] soundTypeIArray;

	public void PlaySound (int soundIdx, char soundType = '\0')
	{
		AudioClip sound;
		if (soundType == '\0') {
			sound = soundArray [soundIdx];
		} else {
			switch (soundType) {
			case 'A':
				sound = soundTypeAArray [soundIdx];
				break;
			case 'B':
				if (soundIdx >= soundTypeBArray.Length)
					sound = soundTypeAArray [soundIdx];
				else
					sound = soundTypeBArray [soundIdx];
				break;
			case 'C':
				if (soundIdx >= soundTypeCArray.Length)
					sound = soundTypeAArray [soundIdx];
				else
					sound = soundTypeCArray [soundIdx];
				break;
			case 'D':
				if (soundIdx >= soundTypeDArray.Length)
					sound = soundTypeAArray [soundIdx];
				else
					sound = soundTypeDArray [soundIdx];
				break;
			case 'E':
				if (soundIdx >= soundTypeEArray.Length)
					sound = soundTypeAArray [soundIdx];
				else
					sound = soundTypeEArray [soundIdx];
				break;
			case 'F':
				if (soundIdx >= soundTypeFArray.Length)
					sound = soundTypeAArray [soundIdx];
				else
					sound = soundTypeFArray [soundIdx];
				break;
			case 'G':
				if (soundIdx >= soundTypeGArray.Length)
					sound = soundTypeAArray [soundIdx];
				else
					sound = soundTypeGArray [soundIdx];
				break;
			case 'H':
				if (soundIdx >= soundTypeHArray.Length)
					sound = soundTypeAArray [soundIdx];
				else
					sound = soundTypeHArray [soundIdx];
				break;
			case 'I':
				if (soundIdx >= soundTypeIArray.Length)
					sound = soundTypeAArray [soundIdx];
				else
					sound = soundTypeIArray [soundIdx];
				break;
			default:
				return;
			}
		}
		UIEventFacade.OnAudioClipRequest.OnNext (new UIEventFacade.AudioClipRequest () {
			IsLoop = false,
			Clip = sound,
			Type = UIEventFacade.AudioClipRequest.TypeSound
		});
	}

	public const int NORMAL = 0;
	public const int TOUCH_NORMAL = 1;
	public const int SCARE = 2;
	public const int ANGRY = 3;
	public const int PACIFY = 4;
	public const int HAPPY = 5;
	public const int WANNA_PLAY = 6;
	public const int PLAY = 7;
	public const int SLEEP = 8;
	public const int TOUCH_SLEEP = 9;
	public const int WAKEUP = 10;
	public const int HUNGRY = 11;
	public const int EAT = 12;
	public const int FULLUP = 13;
	public const int LEVELUP = 14;

	public AudioClip[] interactiveModeSound;

	public void PlayInteractiveModeSound(int idx){
		if (idx >= interactiveModeSound.Length) {
			Debug.LogWarning ("互動音效沒有設定:"+idx);
			return;
		}
		var sound = interactiveModeSound [idx];
		if (sound == null) {
			Debug.LogWarning ("互動音效沒有設定:"+idx);
			return;
		}
		UIEventFacade.OnAudioClipRequest.OnNext (new UIEventFacade.AudioClipRequest () {
			IsLoop = false,
			Clip = sound,
			Type = UIEventFacade.AudioClipRequest.TypeNormalSound
		});
	}

	void OnDestroy(){
		var total = new List<AudioClip> ();
		total.AddRange (soundArray);
		total.AddRange (soundTypeAArray);
		total.AddRange (soundTypeBArray);
		total.AddRange (soundTypeCArray);
		total.AddRange (soundTypeDArray);
		total.AddRange (soundTypeEArray);
		total.AddRange (soundTypeFArray);
		total.AddRange (soundTypeGArray);
		total.AddRange (soundTypeHArray);
		total.AddRange (soundTypeIArray);

		foreach (var clip in total) {
			Resources.UnloadAsset (clip);
		}
	}
}
