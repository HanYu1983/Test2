using System;
using System.IO;
using LitJson;
using UnityEngine;
using System.Collections.Generic;

namespace Remix
{
	public class DeviceData : MonoBehaviour
	{
		public string deviceId, inviteCode, transferCode, username;

		public string DeviceID {
			get {
				if (string.IsNullOrEmpty (deviceId)) {
					throw new UnityException ("deviceId is empty!!");
				}
				return deviceId;
			}
			set{
				deviceId = value;
			}
		}

		public string InviteCode {
			get {
				return inviteCode;
			}
			set{
				inviteCode = value;
			}
		}

		public string TransferCode {
			get {
				return transferCode;
			}
			set{
				transferCode = value;
			}
		}

		public string Name {
			get {
				return username;
			}
			set{
				username = value;
			}
		}

		public bool IsFirstTime{
			get{
				return string.IsNullOrEmpty (deviceId);
			}
		}

		public void Clear(){
			if (Directory.Exists (GameConfig.SAVE_PATH) && File.Exists (GameConfig.SAVE_PATH + "/device.json")) {
				File.Delete(GameConfig.SAVE_PATH + "/device.json");
			}
			deviceId = inviteCode = transferCode = username = null;
		}

		public void Save(){
			if (!Directory.Exists (GameConfig.SAVE_PATH))
				Directory.CreateDirectory (GameConfig.SAVE_PATH);

			try{
				var json = JsonMapper.ToJson (new Dictionary<string,object>(){
					{"deviceId", deviceId},
					{"inviteCode", inviteCode},
					{"transferCode", transferCode},
					{"name", username},
				});
				var sr = File.CreateText(GameConfig.SAVE_PATH + "/device.json");
				sr.WriteLine(json);
				sr.Close ();
			}catch(Exception e){
				throw e;
			}
		}

		public void Load (){
			if (Directory.Exists (GameConfig.SAVE_PATH) && File.Exists (GameConfig.SAVE_PATH + "/device.json")) {
				var saveFile = File.OpenText (GameConfig.SAVE_PATH + "/device.json");
				if (saveFile != null) {
					try{
						var json = saveFile.ReadToEnd ();
						Util.Instance.Log(json);
						JsonData jo = JsonMapper.ToObject (json);
						if(jo["deviceId"] != null){
							deviceId = (string)jo["deviceId"];
						}
						if(jo["inviteCode"] != null){
							inviteCode = (string)jo["inviteCode"];
						}
						if(jo["transferCode"] != null){
							transferCode = (string)jo["transferCode"];
						}
						if(jo["name"] != null){
							username = (string)jo["name"];
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

