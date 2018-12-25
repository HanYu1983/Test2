using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Remix
{
	public class SoundBuffer : MonoBehaviour, ISoundBuffer{
		public int lastPlayChannel = -1;
		public bool isOpenFadeOutFeature = false;

		#region volumn
		float volume = 1.0f;

		public float Volume {
			get {
				return volume;
			}
			set {
				volume = value;
				var sourceList = GetComponents<AudioSource> ();
				for (int i = 0; i < sourceList.Length; ++i) {
					var source = sourceList [i];
					source.volume = value;
				}
			}
		}
		#endregion

		#region lock
		Dictionary<string, int> trackMap = new Dictionary<string, int>();

		public int GetChannelByLockID(string id){
			if (trackMap.ContainsKey (id) == false) {
				return -1;
			}
			return trackMap [id];
		}

		public bool IsChannelLock(int channel){
			foreach (var c in trackMap.Values) {
				if (c == channel) {
					return true;
				}
			}
			return false;
		}

		public void LockChannel(string id, int channel){
			if (trackMap.ContainsKey (id)) {
				throw new UnityException ("這個channel已經被lock了");
			}
			trackMap [id] = channel;
		}

		public void UnlockChannel(string id){
			trackMap.Remove (id);
		}

		#endregion

		public AudioSource GetAudioSource(int channel){
			var sourceList = GetComponents<AudioSource> ();
			return sourceList [channel];
		}

		public float GetAudioTime(int channel){
			var source = GetAudioSource (channel);
			return source.time;
		}

		public bool IsAudioPlayEnd(int channel){
			var source = GetAudioSource (channel);
			return source.time == 0 || source.time == source.clip.length;
		}

		public void PlayClip(int channel, AudioClip clip, bool loop, float delay){
			var sourceList = GetComponents<AudioSource> ();
			if (channel < 0 || channel >= sourceList.Length) {
				Debug.LogWarning ("沒有這個channel:"+channel);
				return;
			}
			var source = sourceList [channel];
			source.clip = clip;
			source.loop = loop;
			source.volume = volume;
			// 這個方法佷像無法信任
			//source.PlayScheduled (AudioSettings.dspTime + delay);
			if (delay == 0) {
				source.Play ();
			} else {
				source.PlayDelayed (delay);
			}

			if (isOpenFadeOutFeature) {
				if (lastPlayChannel != channel) {
					FadeOutChannel (lastPlayChannel);
				}
				ResetFadeOutChannel (channel);
				lastPlayChannel = channel;
			}
		}

		public void SetPause(int channel, bool isPause){
			var sourceList = GetComponents<AudioSource> ();
			if (channel < 0 || channel >= sourceList.Length) {
				Debug.LogWarning ("沒有這個channel:"+channel);
				return;
			}
			var source = sourceList [channel];
			if (isPause == true)
				source.Pause();
			else
				source.Play();
		}

		public void SetMute(int channel, bool isMute){
			var sourceList = GetComponents<AudioSource> ();
			if (channel < 0 || channel >= sourceList.Length) {
				Debug.LogWarning ("沒有這個channel:"+channel);
				return;
			}
			var source = sourceList [channel];
			source.mute = isMute;
		}

		public void StopClip(int channel){
			var sourceList = GetComponents<AudioSource> ();
			if (channel < 0 || channel >= sourceList.Length) {
				Debug.LogWarning ("沒有這個channel:"+channel);
				return;
			}
			var source = sourceList [channel];
			source.Stop ();
		}

		public int PlayClipInIdleChannel(AudioClip clip, bool loop, float delay){
			if (clip == null) {
				Util.Instance.LogWarning ("沒有clip");
				return -1;
			}
			var sourceList = GetComponents<AudioSource> ();
			for(int i=0; i<sourceList.Length; ++i){
				var source = sourceList [i];
				if (source.isPlaying) {
					continue;
				}
				if (IsChannelLock (i)) {
					continue;
				}
				PlayClip (i, clip, loop, delay);
				return i;
			}
			Util.Instance.LogWarning ("沒有channel可以用了，請增加");
			return -1;
		}

		#region fade out process
		Dictionary<int, Coroutine> fadeOutProcess = new Dictionary<int, Coroutine>();
		public float fadeOutFactor = 0.5f;

		void FadeOutChannel(int channel){
			if (channel < 0) {
				return;
			}
			var source = GetAudioSource (channel);
			// 沒在播放就不必fadeOut
			if (source.isPlaying == false) {
				return;
			}
			// 安全處理。把上一次同一個channel的fadeOut處理取消掉
			if (fadeOutProcess.ContainsKey (channel)) {
				ResetFadeOutChannel (channel);
			}
			var process = StartCoroutine (StartFadeOutChannelProcess (channel));
			fadeOutProcess [channel] = process;
		}

		IEnumerator StartFadeOutChannelProcess(int channel){
			var source = GetAudioSource (channel);
			while (source.isPlaying) {
				source.volume *= fadeOutFactor;
				yield return null;
			}
		}

		void ResetFadeOutChannel(int channel){
			if (fadeOutProcess.ContainsKey (channel) == false) {
				return;
			}
			var process = fadeOutProcess [channel];
			StopCoroutine (process);
			fadeOutProcess.Remove (channel);
		}

		#endregion
	}
}

