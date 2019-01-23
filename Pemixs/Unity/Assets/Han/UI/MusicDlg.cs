using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Remix
{
	public class MusicDlg : MonoBehaviour
	{
		public Text txtTitle, txtSong, txtArtist, txtNum;
		public Image picMusic;
		public Text txtDownload, txtDownloadNum;
		public Image picDownloadBar;

		public int selectIdx;
		public string[] musicIds;

		public string CurrentMusicId{
			get{
				if (musicIds.Length == 0) {
					throw new UnityException ("not set musicIds yet");
				}
				return musicIds [selectIdx];
			}
		}

		public void SetMusicIds(List<string> musicIds){
			this.musicIds = musicIds.ToArray ();
		}

		public void Prev(){
			var next = selectIdx - 1;
			if (next < 0) {
				next = musicIds.Length - 1;
			}
			selectIdx = next;
		}

		public void Next(){
			var next = selectIdx + 1;
			if (next >= musicIds.Length) {
				next = 0;
			}
			selectIdx = next;
		}

		string lastUpdateId;

		public void UpdateUI(LanguageText txt, int lang, HandleMp3Player player){
			if (musicIds.Length == 0) {
				Util.Instance.LogWarning ("not set musicIds yet");
				return;
			}

			var id = musicIds [selectIdx];
			var title = txt.GetDlgNote (lang, "MusicDlg");
			var artist = txt.GetMusicArtist (lang, id);
			var song = txt.GetMusicSong (lang, id);

			txtTitle.text = title;
			txtArtist.text = artist;
			txtSong.text = song;
			txtNum.text = string.Format("{0}/{1}", selectIdx+1, musicIds.Length);

			// handle cache
			if (lastUpdateId != id) {
				var prename = string.Format("musicref;LP{0}", id);
				var tmp = Util.Instance.GetPrefab (prename, null);
				picMusic.sprite = tmp.GetComponent<Image> ().sprite;
				GameObject.Destroy (tmp);
				lastUpdateId = id;
			}
			var downloadTxt = txt.GetDlgNote(lang, "LoadDlg");
			var state = player.CheckFileState (id);
			switch (state) {
			case HandleMp3Player.FileState.DownloadSuccess:
				{
					txtDownload.text = downloadTxt;
					txtDownloadNum.text = "100%";
					var scale = picDownloadBar.rectTransform.localScale;
					scale.x = 1;
					picDownloadBar.rectTransform.localScale = scale;
				}
				break;
			case HandleMp3Player.FileState.DownloadFail:
				{
					txtDownload.text = "DownloadFail";
					txtDownloadNum.text = "0%";
					var scale = picDownloadBar.rectTransform.localScale;
					scale.x = 0;
					picDownloadBar.rectTransform.localScale = scale;
				}
				break;
			case HandleMp3Player.FileState.Downloading:
				{
					var progress = player.CheckDownloadProgress (id);

					txtDownload.text = downloadTxt;
					txtDownloadNum.text = (int)(progress * 100) + "%";
					var scale = picDownloadBar.rectTransform.localScale;
					scale.x = progress;
					picDownloadBar.rectTransform.localScale = scale;
				}
				break;
			case HandleMp3Player.FileState.NotExist:
			default:
				{
					var progress = 0;
					txtDownload.text = downloadTxt;
					txtDownloadNum.text = "0%";
					var scale = picDownloadBar.rectTransform.localScale;
					scale.x = progress;
					picDownloadBar.rectTransform.localScale = scale;
				}
				break;
			}
		}
	}
}

