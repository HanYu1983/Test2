using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Remix
{
	// 為了向下相容，不使用這個類
	public class HandleBGM : MonoBehaviour
	{
		public const int MainUI = 0;
		[Tooltip("0:MainUI")]
		public List<string> audioClipPaths;

		Dictionary<int, SoundPointer> soundPool = new Dictionary<int, SoundPointer>();

		public void RequestPlay(int idx){
			if (soundPool.ContainsKey (idx)) {
				Play (soundPool [idx].audioClip);
				return;
			}
			if (idx >= audioClipPaths.Count || audioClipPaths [idx] == null) {
				Debug.LogWarning ("沒有設定AudioPath:"+idx);
				return;
			}

			var obj = Util.Instance.GetPrefab (audioClipPaths [idx], this.gameObject);
			var snd = obj.GetComponent<SoundPointer> ();
			GameObject.Destroy (obj);

			soundPool.Add (idx, snd);
			Play (snd.audioClip);
		}

		public void Play(AudioClip clip){
			// 延遲播放可以解決因為釋放資源而關閉並無法再播放的音樂問題
			StartCoroutine(ExeWithDelay (0.2f, () => {
				UIEventFacade.OnAudioClipRequest.OnNext (new UIEventFacade.AudioClipRequest () {
					Clip = clip,
					IsLoop = true,
					TrackID = GameConfig.MENU_BGM_TRACK_ID
				});
			}));
		}

		IEnumerator ExeWithDelay(float s, Action fn){
			yield return new WaitForSeconds (s);
			fn ();
		}

		public void Stop(){
			UIEventFacade.OnAudioClipRequest.OnNext (new UIEventFacade.AudioClipRequest () {
				IsMute = true,
				IsLoop = true,
				TrackID = GameConfig.MENU_BGM_TRACK_ID
			});
		}
	}
}

