using System;
using UnityEngine;
using System.Collections.Generic;

namespace Remix
{
	public class RhythmToolBuffer : MonoBehaviour, ISoundBuffer
	{
		public RhythmTool[] rhythmTools;
		public RhythmEventProvider eventProvider;

		public bool songLoaded = false;
		public bool songEnded = false;

		void Awake(){
			eventProvider.onSongLoaded.AddListener (OnSongLoaded);
		}

		void OnSongLoaded(string name, int totalFrames){
			for (var i = 0; i < rhythmTools.Length; ++i) {
				var t = rhythmTools [i];
				// 用totalFrames來分別是哪一個load完
				if (t.totalFrames != totalFrames) {
					continue;
				}
				if (i == GetChannelByLockID (GameConfig.GAME_BGM_TRACK_ID)) {
					songLoaded = true;
				}
				t.Play ();
			}
		}

		void OnSongEnded(){
			// 因為只有指定的channel會呼叫這個方法
			songEnded = true;
		}

		public float GetAudioTime(int channel){
			if (channel != GetChannelByLockID (GameConfig.GAME_BGM_TRACK_ID)) {
				throw new UnityException ("只能用GAME_BGM_TRACK_ID來取GetAudioTime");
			}
			if (songLoaded == false) {
				return 0;
			}
			var t = rhythmTools [channel].TimeSeconds ();
			return t;
		}

		public bool IsAudioPlayEnd(int channel){
			if (channel != GetChannelByLockID (GameConfig.GAME_BGM_TRACK_ID)) {
				throw new UnityException ("只能用GAME_BGM_TRACK_ID來取IsAudioPlayEnd");
			}
			return songEnded;
		}

		public void PlayClip(int channel, AudioClip clip, bool loop, float delay){
			if (channel < 0 || channel >= rhythmTools.Length) {
				Debug.LogWarning ("沒有這個channel:"+channel);
				return;
			}
			var source = rhythmTools [channel];

			// 判斷是不是
			var totalSamples = clip.samples;
			totalSamples -= totalSamples % RhythmTool.frameSpacing;
			var totalFrames = totalSamples / RhythmTool.frameSpacing;

			if (source.totalFrames != totalFrames) {
				if (channel == GetChannelByLockID (GameConfig.GAME_BGM_TRACK_ID)) {
					source.SongEnded -= OnSongEnded;
					source.SongEnded += OnSongEnded;
					songEnded = false;
					songLoaded = false;
				}
				source.NewSong (clip);
			} else {
				source.Play ();
			}
			var audioSource = source.gameObject.GetComponent<AudioSource> ();
			if (audioSource != null) {
				audioSource.loop = loop;
			} else {
				Util.Instance.LogWarning ("沒有AudioSource, 無法設定loop:"+channel);
			}
			source.volume = volume;
		}

		public void SetPause(int channel, bool isPause){
			if (channel < 0 || channel >= rhythmTools.Length) {
				Debug.LogWarning ("沒有這個channel:"+channel);
				return;
			}
			var source = rhythmTools [channel];
			if (isPause == true) {
				source.Pause ();
			} else {
				source.volume = volume;
				source.UnPause ();
			}
		}

		public void SetMute(int channel, bool isMute){
			if (channel < 0 || channel >= rhythmTools.Length) {
				Debug.LogWarning ("沒有這個channel:"+channel);
				return;
			}
			var source = rhythmTools [channel];
			source.volume = isMute ? 0 : volume;
		}

		public void StopClip(int channel){
			if (channel < 0 || channel >= rhythmTools.Length) {
				Debug.LogWarning ("沒有這個channel:"+channel);
				return;
			}
			var source = rhythmTools [channel];
			source.Stop ();
		}

		public int PlayClipInIdleChannel(AudioClip clip, bool loop, float delay){
			for(int i=0; i<rhythmTools.Length; ++i){
				var source = rhythmTools [i];
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

		#region volumn
		float volume = 1.0f;

		public float Volume {
			get {
				return volume;
			}
			set {
				volume = value;
				for (int i = 0; i < rhythmTools.Length; ++i) {
					var source = rhythmTools [i];
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
	}
}

