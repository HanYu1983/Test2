using System;
using UnityEngine;
using System.Collections;

namespace Remix
{
	// 這個類別沒有用到
	public class HandleGameBGM : MonoBehaviour
	{
		public RhythmTool gameBGM, feverBGM;
		public RhythmEventProvider eventProvider;
		public float volume = 1;
		public bool gameBGMEnded;

		int loadedCount;

		void Awake(){
			eventProvider.onSongLoaded.AddListener(OnSongLoaded);
			eventProvider.onBeat.AddListener(OnBeat);

			gameBGM.SongEnded += GameBGM_SongEnded;
		}

		public void PlayGameBGM(bool loop = false){
			gameBGM.Play ();
			// 直接設定為loop
			// RhythmTool的程式碼被修改過了
			// RhythmTool.cs line 699
			// 設為loop代表不觸發gameBGMEnd
			gameBGM.GetComponent<AudioSource> ().loop = loop;
		}

		public void PlayFeverBGM(){
			gameBGM.volume = 0;
			feverBGM.Play ();
		}

		public bool IsGameOrFeverBGMPlaying{
			get {
				return gameBGM.isPlaying || feverBGM.isPlaying;
			}
		}

		public bool IsGameBGMEnded{
			get{
				// 設為loop代表不觸發gameBGMEnd
				// 這裡需要這樣處理是為了在DelayTool中音樂是無限循環的，
				// 如果觸發gameBGMEnd代表在循環中音樂時間的誤差會越來越大
				var shouldNotTriggerEnd = gameBGM.GetComponent<AudioSource> ().loop == true;
				if (shouldNotTriggerEnd) {
					return false;
				}
				return gameBGMEnded;
			}
		}

		public float GameBGMAudioTime{
			get{
				return gameBGM.TimeSeconds ();
			}
		}

		public void Pause(){
			if (gameBGM.isPlaying) {
				gameBGM.Pause ();
			}
			if (feverBGM.isPlaying) {
				feverBGM.Pause ();
			}
		}

		public void UnPause(bool isFever){
			if (gameBGM.isPlaying==false) {
				gameBGM.UnPause ();
			}
			if (isFever) {
				if (feverBGM.isPlaying == false) {
					feverBGM.UnPause ();
				}
			}
		}

		public void StopAndUnload(){
			gameBGM.Stop ();
			feverBGM.Stop ();

			gameBGM.GetComponent<AudioSource> ().Stop ();
			feverBGM.GetComponent<AudioSource> ().Stop ();
			// free memory
			var clip = gameBGM.GetComponent<AudioSource> ().clip;
			var clip2 = feverBGM.GetComponent<AudioSource> ().clip;
			gameBGM.GetComponent<AudioSource> ().clip = null;
			feverBGM.GetComponent<AudioSource> ().clip = null;
			Resources.UnloadAsset (clip);
			Resources.UnloadAsset (clip2);
		}

		public void SetAudioClip(AudioClip game, AudioClip fever){
			loadedCount = 0;

			gameBGM.NewSong (game);
			if (fever != null) {
				feverBGM.NewSong (fever);
			} else {
				// pretend loaded
				loadedCount += 1;
			}

			gameBGM.volume = volume;
			feverBGM.volume = volume;

			gameBGMEnded = false;
		}

		void GameBGM_SongEnded (){
			gameBGMEnded = true;
		}

		public IEnumerator WaitingForSondLoaded(){
			yield return new WaitUntil (()=>{
				return loadedCount == 2;
			});
		}

		void OnSongLoaded(string name, int totalFrames){
			if (++loadedCount == 2) {

			}
		}

		void OnBeat(Beat beat){
			
		}
	}
}

