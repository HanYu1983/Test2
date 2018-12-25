using System;
using UnityEngine;

namespace Remix
{
	public interface ISoundBuffer
	{
		float GetAudioTime(int channel);
		bool IsAudioPlayEnd(int channel);
		void PlayClip(int channel, AudioClip clip, bool loop, float delay);
		void SetPause(int channel, bool isPause);
		void SetMute(int channel, bool isMute);
		void StopClip(int channel);
		int PlayClipInIdleChannel(AudioClip clip, bool loop, float delay);

		int GetChannelByLockID(string id);
		bool IsChannelLock(int channel);
		void LockChannel(string id, int channel);
		void UnlockChannel(string id);
	}
}

