using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using LitJson;
using Remix;
using System.Net;

namespace Remix{
	public class GameConfig
	{
		// 安卓的音樂延遲初始設定
		public const float INIT_ANDROID_AUDIO_OFFSET = -0.08f;
		// 看廣告領獎勵的每日上限次數
		public const int WATCH_ADS_REWARD_COUNT = 8;
		// 看廣告領獎勵的獎勵道具ID
		public const string WATCH_ADS_REWARD_ITEM_CONFIGID = ItemDef.I41060;
		// 地圖雙擊縮放最大係數
		public const float MAX_SCALE = 1.75f;
		// 地圖雙擊縮放最小係數
		public const float MIN_SCALE = 0.75f;
		//
		public const string GET_BIG_PHOTO_REWARD = ItemDef.I41040;
		public const string GET_SMALL_PHOTO_REWARD = ItemDef.I41060;

		public const string UNLOCK_BANNER_IAP_SKU = "iap09";
		public static List<string> MAP_IDXS = new List<string>(){"01","02","03","04","05","06","S1","S2"};
		public static List<int> MAP_LEVEL_COUNT = new List<int> (){10, 10, 10, 10, 10, 10, 15, 15};
		public static string LAST_MAP_ID = "04";
		public static List<string> CAT_IDXS = new List<string>(){"A1","B1","C1","A2","B2","C2","A3","B3","C3","A4","B4","C4","A5","B5","C5","A6","B6","C6","A7","B7","C7","A8","B8","C8","M1","M2","M3","M4"};

		public static List<string> TUTORIAL_REWARDS = new List<string> (){ItemDef.I41010, ItemDef.I30010, ItemDef.I32020, ItemDef.I32030, ItemDef.I31020, ItemDef.I20010, ItemDef.I21020};
		// 關卡是否實做完成了
		public static bool IsMapDeveloped(string mapIdx){
			if (mapIdx == "S1") {
				return true;
			}
			var i = MapIdx2Int (mapIdx);
			var lastMap = MapIdx2Int (LAST_MAP_ID);
			if (i == -1 || i <= lastMap) {
				return true;
			}
			return false;
		}

		public static int CatEditorID2Idx(string editorId){
			if (CAT_IDXS.Contains (editorId) == false) {
				throw new UnityException ("這隻貓還沒在程式碼中實做:"+editorId);
			}
			return CAT_IDXS.IndexOf (editorId);
		}

		public static int MapIdx2Int(string mapIdx){
			return MAP_IDXS.IndexOf (mapIdx);
		}

		public static bool HasNextMapIdx(string mapIdx){
			var i = MapIdx2Int (mapIdx);
			if (i == -1) {
				return false;
			}
			if (i == MAP_IDXS.Count-1) {
				return false;
			}
			// 特殊地圖不啟動下一關
			if (mapIdx == "S1" || mapIdx == "S2") {
				return false;
			}
			// 最後一關以前都應該要有下一關
			var lastMap = MapIdx2Int (LAST_MAP_ID);
			if (i < lastMap) {
				return true;
			}
			return true;
		}

		public static string NextMapIdx(string mapIdx){
			if (HasNextMapIdx (mapIdx) == false) {
				throw new UnityException ("錯誤的mapIdx:"+mapIdx);
			}
			var i = MapIdx2Int (mapIdx);
			return MAP_IDXS [i + 1];
		}

		public const string MENU_BGM_TRACK_ID = "MenuBGM";
		public const string GAME_BGM_TRACK_ID = "GameBGM";
		public const string FEVER_BGM_TRACK_ID = "FeverBGM";

		public const int ITEM_FOOD_COUNT = 8;
		public const int ITEM_CAMERA_COUNT = 8;
		public const int ITEM_S_CAMERA_COUNT = 2;
		public const int ITEM_TOY_COUNT = 11;
		public const int ITEM_S_TOY_COUNT = 2;

		public enum ITEM_TYPE
		{
			ITEM_NONE = 0,
			ITEM_BASE,
			ITEM_CATCH,
			ITEM_EXPLORE,
			ITEM_FOOD,
			ITEM_BASE_CAMERA,
			ITEM_BASE_TOY,
			ITEM_ALL,
		};

		public enum CAT_STATE_ID
		{
			STATE_NONE = 0,
			STATE_IDLE,
			STATE_ANGRY,
			STATE_WANNA_PLAY,
			STATE_SLEEP,
			STATE_HUNGRY
		}

		public enum CAPTURE_STATE
		{
			PENDING,
			NORMAL,
			CAPTURE,
			COMPLETED
		}

		public const int MAX_CAT_NUM = 6;
		public const int MAX_MAP_NODE_NUM = 3;
		public const int MAX_MAP_COUNT = 6;

		public const int MAX_TOUCH_CAT_COUNT = 10;
		public const int DUPLICATE_CAT_ADD_EXP = 500;
		public const int CAT_ANGRY_TIME = 60;
		// minutes
		public const int WANNA_PLAY_COUNT_TIME = 360;
		// minutes
		public const int BATTLE_COUNT_TO_SLEEP = 5;
		public const int CAT_SLEEP_TIME = 60;
		// minutes
		public const int REDUCE_CAT_SLEEP_TIME = 15;
		// minutes
		public const int HUNGRY_THRESHOLD = 80;
		// percentage

		public static string ROOT_PATH = Application.persistentDataPath + "/";
		public static string LOCAL_ASSET_BUNDELS_PATH = ROOT_PATH + "assetBundles/";
		public static string LOCAL_PHOTOS_PATH = ROOT_PATH + "photos/";
		public static string LOCAL_MUSIC_PATH = ROOT_PATH + "music/";
		public static string LOCAL_TEMP_PATH = ROOT_PATH + "temp/";
		public static string SAVE_PATH = ROOT_PATH + "Saves";

		public static int GetTouchCatAddExp (Cat data){
			if (data.Lv <= 20) {
				return 5;
			} else if (data.Lv <= 50) {
				return 10;
			} else if (data.Lv <= 100) {
				return 20;
			}
			return 0;
		}

		public static DateTime ConvertFromUnixTimestamp(double timestamp){
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			return origin.AddSeconds(timestamp);
		}

		public static long ConvertToUnixTimestamp(DateTime date){
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			TimeSpan diff = date.ToUniversalTime() - origin;
			return Convert.ToInt64(Math.Floor(diff.TotalSeconds));
		}

		public static byte[] GetContent(Stream responseStream, long contentLength) {
			var data = new byte[contentLength];
			int currentIndex = 0;
			int bytesReceived = 0;
			var buffer = new byte[256];
			do {
				bytesReceived = responseStream.Read(buffer, 0, 256);
				Array.Copy(buffer, 0, data, currentIndex, bytesReceived);
				currentIndex += bytesReceived;
			} while (currentIndex < contentLength);
			return data;
		}

		public static bool IsMaxLv(int lv){
			return lv >= CatLvRate1.ID_COUNT - 1;
		}
	}
}