using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Collections;
using UnityEngine.Networking;

namespace Remix
{
	public class HandleMp3Player : MonoBehaviour
	{
		public string mp3Path;
		public Native native;

		void Awake(){
			mp3Path = RemixApi.API_HOST + "/Music/";
		}

		public void Setup(){
			native.Command ("?cmd=BackgroundAudio.setup");
		}

		public void SetPlayerList(List<string> musicIds, string auth){
			var urlstrs = musicIds.Select ((id) => {
				var url = LocalFilePath (id);
				return "&url=" + WWW.EscapeURL (url);
			}).Aggregate ("", (total, url) => {
				return total + url;
			});
			var cmd = string.Format (
				          "?cmd=BackgroundAudio.setPlayerList&auth={0}{1}",
						  WWW.EscapeURL(auth),
				          urlstrs
			);
			native.Command (cmd);
		}

		public void Play(string musicId){
			var url = LocalFilePath (musicId);
			native.Command ("?cmd=BackgroundAudio.play&url="+WWW.EscapeURL(url));
		}

		public void Pause(){
			native.Command ("?cmd=BackgroundAudio.pause");
		}

		public bool ReadyToPlay(string musicId){
			return CheckFileState (musicId) == FileState.DownloadSuccess;
		}

		Dictionary<string, UnityWebRequest> requests = new Dictionary<string, UnityWebRequest> ();
		Dictionary<string, Action<Exception>> onDonePool = new Dictionary<string, Action<Exception>> ();

		public void StartBackgroundDownloadFile(string musicId, Action<Exception> onDone = null){
			if (requests.ContainsKey (musicId)) {
				var tmp = requests [musicId];
				var handler = tmp.downloadHandler as ToFileDownloadHandler;
				handler.Cancel ();

				tmp.Dispose ();
				requests.Remove (musicId);
			}
			var url = RemoteFilePath (musicId);
			var downloadPath = LocalFilePath (musicId);
			var request = RemixApi.CreateURLRequest (url, RemixApi.GetAuthorizationString (""), null);
			request.downloadHandler = new ToFileDownloadHandler (new byte[64 * 1024], downloadPath);
			request.Send ();
			requests.Add (musicId, request);

			if (onDone != null) {
				onDonePool [musicId] = onDone;
			}
		}

		public enum FileState
		{
			NotExist,
			Downloading,
			DownloadSuccess,
			DownloadFail
		}

		public FileState CheckFileState(string musicId){
			// 先判斷是不是這次啟動遊戲中按下Download
			if (requests.ContainsKey (musicId)) {
				var request = requests [musicId];
				if (request.isDone) {
					if (request.isNetworkError == false) {
						return FileState.DownloadSuccess;
					} else {
						return FileState.DownloadFail;
					}
				} else {
					return FileState.Downloading;
				}
			}
			// 如果是之前已經下載好會跑到這裡
			if (File.Exists (LocalFilePath (musicId))) {
				return FileState.DownloadSuccess;
			}
			return FileState.NotExist;
		}

		public void Step(){
			var shouldRemoved = new List<string> ();

			foreach (var key in requests.Keys) {
				var request = requests [key];
				if (request.isDone) {
					var onDone = onDonePool [key];
					if (onDone != null) {
						if (request.isNetworkError) {
							onDone (new Exception(request.error));
						} else {
							onDone (null);
						}
						onDonePool.Remove (key);
					}

					if (request.isNetworkError) {
						var handler = request.downloadHandler as ToFileDownloadHandler;
						handler.Cancel ();
						// 先不必加入shouldRemoved
					} else {
						request.Dispose ();
						shouldRemoved.Add (key);
					}
				}
			}

			foreach (var key in shouldRemoved) {
				requests.Remove (key);
			}
		}

		public float CheckDownloadProgress(string musicId){
			if (requests.ContainsKey (musicId) == false) {
				return 0;
			}
			var request = requests [musicId];
			return request.downloadProgress;
		}

		string RemoteFilePath(string musicId){
			var url = string.Format("{0}{1}.mp3", mp3Path, musicId);
			return url;
		}

		static string LocalFilePath(string musicId){
			return string.Format ("{0}{1}.mp3", GameConfig.LOCAL_MUSIC_PATH, musicId);
		}
	}
}

