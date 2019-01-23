using System;
using UnityEngine;
using System.IO;
using LitJson;
using System.Collections.Generic;
using System.Linq;

namespace Remix
{
	public class UserSettings : MonoBehaviour
	{
		#region music sound switch
		public bool isMusicOn = true, isSoundOn = true;
		#endregion

		#region language setting
		public int language;
		public int Language {
			get{
				return language;
			}
			set{
				language = value;
			}
		}
		#endregion

		#region last enter level
		public LevelKey LastEnterLevel{ get; set; }

		public bool HasLastPlayLevel{
			get{
				return LastEnterLevel != null;
			}
		}
		#endregion

		#region last enter capture
		public CaptureKey LastEnterCapture{ get; set; }

		public bool HasLastPlayCapture{
			get{
				return LastEnterCapture != null;
			}
		}
		#endregion


		#region mark read
		public List<string> markRead;
		public void MarkRead(string id){
			if (IsMarkRead (id)) {
				return;
			}
			markRead.Add (id);
		}

		public bool IsMarkRead(string id){
			return markRead.Contains (id);
		}
		#endregion

		#region ad reward time
		public List<long> adRewardTime;

		public void AppendAdRewardTime(long time){
			adRewardTime.Add (time);
			if (adRewardTime.Count > GameConfig.WATCH_ADS_REWARD_COUNT) {
				adRewardTime.RemoveAt (0);
			}
		}

		public int AdRewardCountToday(){
			return adRewardTime.Count (time => {
				var date = new DateTime(time);
				var now = DateTime.Now;
				return date.Year == now.Year && date.Month == now.Month && date.Day == now.Day;
			});
		}

		public bool IsAdRewardEnable{
			get {
				return AdRewardCountToday() < GameConfig.WATCH_ADS_REWARD_COUNT;
			}
		}

		#endregion

		#region check download package
		public bool isDownloadPackageOKFlag;
		public bool IsDownloadPackageOKFlag{
			get{
				return isDownloadPackageOKFlag;
			}
			set{
				isDownloadPackageOKFlag = value;
			}
		}
		#endregion

		public void Clear(){
			if (Directory.Exists (GameConfig.SAVE_PATH) && File.Exists (GameConfig.SAVE_PATH + "/user.json")) {
				File.Delete(GameConfig.SAVE_PATH + "/user.json");
			}
			ResetMemory ();
		}

		void ResetMemory(){
			isMusicOn = isSoundOn = true;
			markRead.Clear ();
			Language = LanguageText.Ch;
			// default value
			LastEnterLevel = new LevelKey(){
				MapIdx = GameConfig.MAP_IDXS[0],
				Idx = 0
			};
			LastEnterCapture = new CaptureKey(){
				MapIdx = GameConfig.MAP_IDXS[0],
				Idx = 0
			};
			adRewardTime.Clear ();
			// isDownloadPackageOKFlag = false;
		}

		public void Save(){
            if (!Directory.Exists (GameConfig.SAVE_PATH))
				Directory.CreateDirectory (GameConfig.SAVE_PATH);

			try{
				var json = JsonMapper.ToJson (new Dictionary<string,object>(){
					{"isMusicOn",isMusicOn},
					{"isSoundOn",isSoundOn},
					{"markRead", markRead},
					{"language", Language},
					{"lastPlayLevel", LastEnterLevel.StringKey},
					{"lastEnterCapture", LastEnterCapture.StringKey},
					{"adRewardTime", adRewardTime},
					{"isDownloadPackageOKFlag", isDownloadPackageOKFlag}
				});
				var sr = File.CreateText(GameConfig.SAVE_PATH + "/user.json");
				sr.WriteLine(json);
				sr.Close ();
			}catch(Exception e){
				throw e;
			}
		}

		public void Load (){
			// default value
			LastEnterLevel = new LevelKey(){
				MapIdx = GameConfig.MAP_IDXS[0],
				Idx = 0
			};
			LastEnterCapture = new CaptureKey(){
				MapIdx = GameConfig.MAP_IDXS[0],
				Idx = 0
			};
			if (Directory.Exists (GameConfig.SAVE_PATH) && File.Exists (GameConfig.SAVE_PATH + "/user.json")) {
				var saveFile = File.OpenText (GameConfig.SAVE_PATH + "/user.json");
				if (saveFile != null) {
					try{
						var json = saveFile.ReadToEnd ();
						Util.Instance.Log(json);
						JsonData jo = JsonMapper.ToObject (json);

						isMusicOn = (bool)jo["isMusicOn"];
						isSoundOn = (bool)jo["isSoundOn"];

						markRead.Clear();
						var readJo = jo["markRead"];
						for(var i=0; i<readJo.Count; ++i){
							markRead.Add((string)readJo[i]);
						}

						Language = (int)jo["language"];
						if(jo["lastPlayLevel"] != null){
							LastEnterLevel = new LevelKey((string)jo["lastPlayLevel"]);
						}
						if(jo["lastEnterCapture"] != null){
							LastEnterCapture = new CaptureKey((string)jo["lastEnterCapture"]);
						}
						var adRewardTimeJo = jo["adRewardTime"];
						for(var i=0; i<adRewardTimeJo.Count; ++i){
							var t = 0L;
							if(adRewardTimeJo[i].IsInt){
								t = (int)adRewardTimeJo[i];
							} else if(adRewardTimeJo[i].IsLong){
								t = (long)adRewardTimeJo[i];
							}
							adRewardTime.Add(t);
						}
						if(jo["isDownloadPackageOKFlag"]!=null){
							isDownloadPackageOKFlag = (bool)jo["isDownloadPackageOKFlag"];
						}
					}catch(Exception e){
						Debug.LogWarning ("沒有使用者設定，使用預設值:"+e.Message);
					}finally{
						saveFile.Close ();
					}
				}
			}
		}
	}
}

