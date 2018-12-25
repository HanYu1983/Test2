using System;
using System.Collections.Generic;
using UnityEngine;

namespace Remix
{
	public class HandleAudioSyncTimerSmooth : MonoBehaviour
	{
		public int bufferSize;
		public List<float> musicTimerSeq = new List<float>();

		public void AppendTime(float time, bool forceAndReset = false){
			if (time < GetLastTime ()) {
				// forceAndReset是為了DelayTool的音樂會loop的關係，所以一定有time<GetLastTime ()的狀況
				if (forceAndReset) {
					musicTimerSeq.Clear ();
				} else {
					throw new UnityException ("time:" + time + ":last:" + GetLastTime ());
				}
			}
			musicTimerSeq.Add (time);
			if (musicTimerSeq.Count > bufferSize) {
				musicTimerSeq.RemoveAt (0);
			}
		}
		public float GetSyncTime(){
			if (musicTimerSeq.Count == 0) {
				return 0;
			}
			var total = 0f;
			foreach (var t in musicTimerSeq) {
				total += t;
			}
			return total / musicTimerSeq.Count;
		}
		public float GetLastTime(){
			if (musicTimerSeq.Count == 0) {
				return 0;
			}
			return musicTimerSeq [musicTimerSeq.Count - 1];
		}
		public void ClearTimerSeq(){
			musicTimerSeq.Clear ();
		}

		public void Clear(){
			ClearTimerSeq ();
			ClearMusicEndFlag ();
		}

		#region no name
		// 偷懶處理。這個region應該要移到新的類
		public bool isMusicEnd;
		public void ClearMusicEndFlag(){
			isMusicEnd = false;
		}
		public bool IsMusicEnd{
			get{
				return isMusicEnd;
			}
			set{
				isMusicEnd = value;
			}
		}
		#endregion
	}
}

