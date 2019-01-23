using UnityEngine;
using System.Collections;
using System;
using Remix;

// 韻律控制
public class RhythmCtrl : MonoBehaviour
{
	public static int MINUS_SCORE = -4;
	public static int ADD_HP = 4;
	public static int MAX_TURN = 2;
	public static float MAX_SCORE = 9999999.0f;
	public static int LONGCLICK_BEFORE_SCORE = 500;

	public static string[][] RHYTHM_DATA_ARRAY;
	public static int[][] RHYTHM_IDX_ARRAY;
	public static char[][] RHYTHM_TYPE_ARRAY;
	public static int[][] RHYTHM_BTNMASH_ARRAY;
	// 背景音用的資料
	public static int[][] RHYTHM_SOUND_IDX_ARRAY;
	public static char[][] RHYTHM_SOUND_TYPE_ARRAY;
	public static int[][] RHYTHM_SOUND_BTNMASH_ARRAY;

	public static string LEVEL_NAME = "";
	public static int MAX_LEVEL;
	public static int MAX_BEAT;
	public static float TIME_SCALE;
	public static float BEAT_TIME;
	public static float COUNT_DOWN_TIME;
	public static float HALF_COUNT_DOWN_TIME;
	public static float HINT_WIDTH;
	public static float READY_TIME;
	public static float GO_TIME;
	public static float HALF_BEAT_TIME;
	public static float BEAT_OFFSET;
	public static float BEAT_PRE_OFFSET;
	public static float BEAT_POST_OFFSET;
	public static float SHOW_RESULT_TIME;
	public static float HINT_MOVE_SPEED;
	public static int PERFECT_SCORE;
	public static int FEVER_SCORE;
	public static int WARM_ZONE_SCORE;

	public static int LEVEL_TYPE { get { return Convert.ToInt32 (RHYTHM_DATA_ARRAY [0] [0]); } private set { } }

	public static int LEVEL_BPM { get { return Convert.ToInt32 (RHYTHM_DATA_ARRAY [0] [1]); } private set { } }

	public static int LEVEL_THRESHOLD { get { return Convert.ToInt32 (RHYTHM_DATA_ARRAY [0] [2]); } private set { } }

	public static string LEVEL_BGM { get { return RHYTHM_DATA_ARRAY [0] [3]; } private set { } }

	public static string LEVEL_SUCC_BGM { get { return RHYTHM_DATA_ARRAY [0] [4]; } private set { } }

	public static string LEVEL_FAIL_BGM { get { return RHYTHM_DATA_ARRAY [0] [5]; } private set { } }

	public static string ENEMY_CAT { get { return RHYTHM_DATA_ARRAY [0] [6]; } private set { } }

	public static int RHYTHM_COUNT;

	public static ArrayList CLEAR_REWARD;

	public static int FEVER_THRESHOLD { 
		get { 
			try{
				return Convert.ToInt32 (RHYTHM_DATA_ARRAY [0] [8]); 
			}catch(Exception){
				Debug.LogWarning ("沒有輸入Fever啟動條件，當成0");
				return 0;
			}
		} 
		private set { } 
	}

	public static int FEVER_LENGTH { 
		get { 
			try{
				return Convert.ToInt32 (RHYTHM_DATA_ARRAY [0] [9]); 
			}catch(Exception){
				Debug.LogWarning ("沒有輸入Fever長度，當成0");
				return 0;
			}
		} 
		private set { } 
	}

	public static int FEVER_START_LEVEL;

	public const int DELAY_TOOL_MODE = 0;
	public const int DANCING_MODE = 1;
	public const int FIGHTING_MODE = 2;
	public const int PARKOURING_MODE = 3;
	public const int TUTORIAL_MODE = 4;

	public static int[] SHOW_HINT_MODE;
	public static string[][] TEXT_DATA_ARRAY;

	public static void LoadData (string filePath)
	{
		RHYTHM_DATA_ARRAY = Remix.Util.Instance.ParseCSV (filePath);
	}

	public static IEnumerator LoadDataAsync (string filePath)
	{
		var data = new RemixApi.Either<string[][]> ();
		yield return Remix.Util.Instance.ParseCSVAsync (data, filePath);
		RHYTHM_DATA_ARRAY = data.Ref;
	}

	public static void InitRhythm (CatDef catDefData, LevelDef levelDef){
		MAX_LEVEL = RHYTHM_DATA_ARRAY.Length / 3 - FEVER_LENGTH;
		BEAT_TIME = 60.0f / (LEVEL_BPM * 2.0f);
		READY_TIME = BEAT_TIME * 8.0f;
		GO_TIME = BEAT_TIME * 4.0f;
		HALF_BEAT_TIME = BEAT_TIME * 0.5f;
		BEAT_OFFSET = HALF_BEAT_TIME / 2;
		BEAT_PRE_OFFSET = BEAT_OFFSET;
		BEAT_POST_OFFSET = BEAT_OFFSET;

		if (levelDef != null) {
			MINUS_SCORE = -levelDef.Damage;
			ADD_HP = levelDef.Reply;
		}

		switch (LEVEL_TYPE) {
		case DANCING_MODE:
			MAX_BEAT = 8;
			COUNT_DOWN_TIME = BEAT_TIME * (MAX_BEAT * MAX_TURN) * 0.5f;
			SHOW_RESULT_TIME = BEAT_TIME * (MAX_BEAT * MAX_TURN) * 0.5f + 8.0f;
			MINUS_SCORE = (int)(MINUS_SCORE * catDefData.DDmg);
			break;
		case FIGHTING_MODE:
			MAX_BEAT = 8;
			COUNT_DOWN_TIME = BEAT_TIME * (MAX_BEAT * MAX_TURN);
			SHOW_RESULT_TIME = BEAT_TIME * (MAX_BEAT * MAX_TURN) * 0.5f + 8.0f;
			MINUS_SCORE = (int)(MINUS_SCORE * catDefData.BDmp);
			break;
		case PARKOURING_MODE:
			MAX_BEAT = 8;
			COUNT_DOWN_TIME = BEAT_TIME * (MAX_BEAT * MAX_TURN);
			SHOW_RESULT_TIME = BEAT_TIME * (MAX_BEAT * MAX_TURN) * 0.5f + 8.0f;
			MINUS_SCORE = (int)(MINUS_SCORE * catDefData.PDmg);
			break;
		case DELAY_TOOL_MODE:
			MAX_BEAT = 4;
			COUNT_DOWN_TIME = BEAT_TIME * (MAX_BEAT * MAX_TURN) * 2.0f;
			SHOW_RESULT_TIME = BEAT_TIME * (MAX_BEAT * MAX_TURN) + 8.0f;
			// ParseTutorialText ();
			break;
		}
		TIME_SCALE = LEVEL_BPM / 60.0f;
		HALF_COUNT_DOWN_TIME = COUNT_DOWN_TIME * 0.5f;
		HINT_WIDTH = 120.0f;

		string levelReward = RHYTHM_DATA_ARRAY [0] [7];
		string[] levelRewardArray = levelReward.Split ('&');
		CLEAR_REWARD = new ArrayList ();
		if (levelReward.Length > 0) {
			for (int i = 0; i < levelRewardArray.Length; ++i) {
				string rewardStr = levelRewardArray [i];
				CLEAR_REWARD.Add (rewardStr);
			}
		}

		FEVER_START_LEVEL = (FEVER_LENGTH > 0) ? (MAX_LEVEL - FEVER_LENGTH) : -1;

		int rhythmCount = 0;
		int warmRhythmCount = 0;
		int feverRhythmCount = 0;
		for (int i = 0; i < MAX_LEVEL + FEVER_LENGTH; ++i) {
			int index = i * 3 + 1;
			string[] mainStrArray = RHYTHM_DATA_ARRAY [index];
			string[] subStrArray = RHYTHM_DATA_ARRAY [index + 1];
			int length = mainStrArray.Length;
			
			for (int j = 0; j < length; ++j) {
				var flag = mainStrArray [j].Trim();
				var isHide = false;
				var rhythmType = '\0';
				var rhythm = 0;
				foreach(var f in flag){
					if (f == '-') {
						isHide = true;
					} else if (f >= 'A' && f <= 'Z') {
						rhythmType = f;
					} else if (f >= '0' && f <= '9') {
						rhythm = int.Parse (f.ToString ());
					}
				}
				if (isHide == false) {
					// 1~8都算節奏點
					// 不知為何本來只算1~6
					if (rhythm > 0 && rhythm < 9) {
						if (i < MAX_LEVEL)
							rhythmCount++;
						else
							feverRhythmCount++;
						if (i < FEVER_START_LEVEL)
							warmRhythmCount++;
					}
				}

				flag = subStrArray [j].Trim();
				isHide = false;
				rhythmType = '\0';
				rhythm = 0;
				foreach(var f in flag){
					if (f == '-') {
						isHide = true;
					} else if (f >= 'A' && f <= 'Z') {
						rhythmType = f;
					} else if (f >= '0' && f <= '9') {
						rhythm = int.Parse (f.ToString ());
					}
				}
				if (isHide == false) {
					// 1~8都算節奏點
					// 不知為何本來只算1~6
					if (rhythm > 0 && rhythm < 9) {
						if (i < MAX_LEVEL)
							rhythmCount++;
						else
							feverRhythmCount++;
						if (i < FEVER_START_LEVEL)
							warmRhythmCount++;
					}
				}

				/*
				char[] rhythmArray = mainStrArray [j].ToCharArray ();
				int rhythm = int.Parse (rhythmArray [rhythmArray.Length - 1].ToString ());
				// 1~8都算節奏點
				// 不知為何本來只算1~6
				if (rhythm > 0 && rhythm < 9) {
					if (i < MAX_LEVEL)
						rhythmCount++;
					else
						feverRhythmCount++;
					if (i < FEVER_START_LEVEL)
						warmRhythmCount++;
				}

				rhythmArray = subStrArray [j].ToCharArray ();
				rhythm = int.Parse (rhythmArray [rhythmArray.Length - 1].ToString ());
				// 1~8都算節奏點
				// 不知為何本來只算1~6
				if (rhythm > 0 && rhythm < 9) {
					if (i < MAX_LEVEL)
						rhythmCount++;
					else
						feverRhythmCount++;
					if (i < FEVER_START_LEVEL)
						warmRhythmCount++;
				}*/
			}
		}

		float dist = 2.0f * RhythmCtrl.HINT_WIDTH * (RhythmCtrl.MAX_BEAT * RhythmCtrl.MAX_TURN + 1.0f);
		float moveTime = RhythmCtrl.BEAT_TIME * (RhythmCtrl.MAX_BEAT * RhythmCtrl.MAX_TURN * 2.0f);
		HINT_MOVE_SPEED = dist / moveTime;

		PERFECT_SCORE = Mathf.CeilToInt (MAX_SCORE / rhythmCount);

		WARM_ZONE_SCORE = warmRhythmCount * PERFECT_SCORE;

		FEVER_SCORE = Mathf.CeilToInt ((MAX_SCORE - warmRhythmCount * PERFECT_SCORE) / feverRhythmCount);

		RHYTHM_COUNT = rhythmCount;
	}

	private static void ParseTutorialText ()
	{
		string filePath = "Level/tutorial_text";
		string[][] tempDataArray = Remix.Util.Instance.ParseCSV (filePath);
		int length = tempDataArray.Length / 4;
		TEXT_DATA_ARRAY = new string[length][];
		SHOW_HINT_MODE = new int[length];
		for (int i = 0; i < length; ++i) {
			int index = i * 4;
			int textSize = int.Parse (tempDataArray [index] [0]);
			SHOW_HINT_MODE [i] = int.Parse (tempDataArray [index] [1]);
			TEXT_DATA_ARRAY [i] = new string[textSize];
			for (int j = 0; j < textSize; ++j) {
				// TODO
				//int lineIndex = index + 1 + GameRecordCtrl.GetLanguage ();
				//TEXT_DATA_ARRAY [i] [j] = tempDataArray [lineIndex] [j + 1];
			}
		}
	}

	//public static bool use32 = false;

	public static void InitLevelRhythm (int currentLevel)
	{
		if (currentLevel < 1) {
			throw new UnityException ("請輸入1以上的值");
		}
		int index = (currentLevel - 1) * 3 + 1;
		string[] mainStrArray = RHYTHM_DATA_ARRAY [index];
		string[] subStrArray = RHYTHM_DATA_ARRAY [index + 1];
		int length = mainStrArray.Length;
		
		RHYTHM_IDX_ARRAY = new int[2][];
		RHYTHM_IDX_ARRAY [0] = new int[length];
		RHYTHM_IDX_ARRAY [1] = new int[length];
		RHYTHM_TYPE_ARRAY = new char[2][];
		RHYTHM_TYPE_ARRAY [0] = new char[length];
		RHYTHM_TYPE_ARRAY [1] = new char[length];
		RHYTHM_BTNMASH_ARRAY = new int[2][];
		RHYTHM_BTNMASH_ARRAY [0] = new int[length];
		RHYTHM_BTNMASH_ARRAY [1] = new int[length];

		RHYTHM_SOUND_IDX_ARRAY = new int[2][];
		RHYTHM_SOUND_IDX_ARRAY [0] = new int[length];
		RHYTHM_SOUND_IDX_ARRAY [1] = new int[length];
		RHYTHM_SOUND_TYPE_ARRAY = new char[2][];
		RHYTHM_SOUND_TYPE_ARRAY [0] = new char[length];
		RHYTHM_SOUND_TYPE_ARRAY [1] = new char[length];
		RHYTHM_SOUND_BTNMASH_ARRAY = new int[2][];
		RHYTHM_SOUND_BTNMASH_ARRAY [0] = new int[length];
		RHYTHM_SOUND_BTNMASH_ARRAY [1] = new int[length];

		// 先計算一次
		// 要略過要當背景音的音符
		for (int t = 0; t < MAX_TURN; ++t) {
			var dataAry = RHYTHM_DATA_ARRAY [index + t];
			for (int i = 0; i < length; ++i) {
				var flag = dataAry [i].Trim();
				var isHide = false;
				var rhythmType = '\0';
				var rhythm = 0;
				foreach(var f in flag){
					if (f == '-') {
						isHide = true;
					} else if (f >= 'A' && f <= 'Z') {
						rhythmType = f;
					} else if (f >= '0' && f <= '9') {
						rhythm = int.Parse (f.ToString ());
					}
				}
				if (isHide == false) {
					RHYTHM_IDX_ARRAY [t] [i] = rhythm;
					RHYTHM_TYPE_ARRAY [t] [i] = rhythmType;
					// 這應該是處理長按的功能
					if (rhythm == 7) {
						if (i > 0 && RHYTHM_BTNMASH_ARRAY [t] [i - 1] == 0) {
							RHYTHM_BTNMASH_ARRAY [t] [i - 1] = RHYTHM_IDX_ARRAY [t] [i - 1];
						}
						RHYTHM_BTNMASH_ARRAY [t] [i] = RHYTHM_BTNMASH_ARRAY [t] [i - 1];
					} else if (rhythm == 8) {
						RHYTHM_BTNMASH_ARRAY [t] [i] = RHYTHM_BTNMASH_ARRAY [t] [i - 1];
					} else {
						RHYTHM_BTNMASH_ARRAY [t] [i] = 0;
					}
				}
			}
		}
		// 再計算第二次
		// 這次將所有音符都當成背景音
		for (int t = 0; t < MAX_TURN; ++t) {
			var dataAry = RHYTHM_DATA_ARRAY [index + t];
			for (int i = 0; i < length; ++i) {
				var flag = dataAry [i].Trim();
				var rhythmType = '\0';
				var rhythm = 0;
				foreach(var f in flag){
					if (f >= 'A' && f <= 'Z') {
						rhythmType = f;
					} else if (f >= '0' && f <= '9') {
						rhythm = int.Parse (f.ToString ());
					}
				}
				RHYTHM_SOUND_IDX_ARRAY [t] [i] = rhythm;
				RHYTHM_SOUND_TYPE_ARRAY [t] [i] = rhythmType;

				// 這應該是處理長按的功能
				if (rhythm == 7) {
					if (i > 0 && RHYTHM_SOUND_BTNMASH_ARRAY [t] [i - 1] == 0) {
						RHYTHM_SOUND_BTNMASH_ARRAY [t] [i - 1] = RHYTHM_SOUND_IDX_ARRAY [t] [i - 1];
					}
					RHYTHM_SOUND_BTNMASH_ARRAY [t] [i] = RHYTHM_SOUND_BTNMASH_ARRAY [t] [i - 1];
				} else if (rhythm == 8) {
					RHYTHM_SOUND_BTNMASH_ARRAY [t] [i] = RHYTHM_SOUND_BTNMASH_ARRAY [t] [i - 1];
				} else {
					RHYTHM_SOUND_BTNMASH_ARRAY [t] [i] = 0;
				}
			}
		}
		/*if (use32) {
			for (int t = 0; t < MAX_TURN; ++t) {
				var dataAry = RHYTHM_DATA_ARRAY [index + t];
				for (int i = 0; i < length; ++i) {
					char[] rhythmArray = dataAry [i].ToCharArray ();
					// rhythmArray的最後1個一定是數字
					// 不然就是格式錯誤
					int rhythm = int.Parse (rhythmArray [rhythmArray.Length - 1].ToString ());
					RHYTHM_IDX_ARRAY [t] [i] = rhythm;
					// 略過包含換行字元的(ascii code = 10)
					// 通常是每行的第一個字元
					if (Convert.ToInt32 (rhythmArray [0]) == 10) {
						// 有換行字元的長度為3
						// 所以剛好取rhythmArray[1]為Type
						RHYTHM_TYPE_ARRAY [t] [i] = (rhythmArray.Length == 2) ? '\0' : rhythmArray [1];
					} else {
						// 沒有換行字元的長度為2
						// 所以剛好取rhythmArray[0]為Type
						RHYTHM_TYPE_ARRAY [t] [i] = (rhythmArray.Length == 1) ? '\0' : rhythmArray [0];
					}
					// 這應該是處理長按的功能
					if (rhythm == 7) {
						if (i > 0 && RHYTHM_BTNMASH_ARRAY [t] [i - 1] == 0) {
							RHYTHM_BTNMASH_ARRAY [t] [i - 1] = RHYTHM_IDX_ARRAY [t] [i - 1];
						}
						RHYTHM_BTNMASH_ARRAY [t] [i] = RHYTHM_BTNMASH_ARRAY [t] [i - 1];
					} else if (rhythm == 8) {
						RHYTHM_BTNMASH_ARRAY [t] [i] = RHYTHM_BTNMASH_ARRAY [t] [i - 1];
					} else {
						RHYTHM_BTNMASH_ARRAY [t] [i] = 0;
					}
				}
			}
		} else {
			for (int i = 0; i < length; ++i) {
				char[] rhythmArray = mainStrArray [i].ToCharArray ();
				int rhythm = int.Parse (rhythmArray [rhythmArray.Length - 1].ToString ());
				RHYTHM_IDX_ARRAY [0] [i] = rhythm;
				// 略過包含換行字元的(ascii code = 10)
				// 通常是每行的第一個字元
				if (Convert.ToInt32 (rhythmArray [0]) == 10) {
					// 有換行字元的長度為3
					// 所以剛好取rhythmArray[1]為Type
					RHYTHM_TYPE_ARRAY [0] [i] = (rhythmArray.Length == 2) ? '\0' : rhythmArray [1];
				} else {
					// 沒有換行字元的長度為2
					// 所以剛好取rhythmArray[0]為Type
					RHYTHM_TYPE_ARRAY [0] [i] = (rhythmArray.Length == 1) ? '\0' : rhythmArray [0];
				}
				// 這應該是處理長按的功能
				if (rhythm == 7) {
					if (i > 1 && RHYTHM_BTNMASH_ARRAY [0] [i - 2] == 0) {
						RHYTHM_BTNMASH_ARRAY [0] [i - 2] = RHYTHM_IDX_ARRAY [0] [i - 2];
					}
					RHYTHM_BTNMASH_ARRAY [0] [i] = RHYTHM_BTNMASH_ARRAY [0] [i - 2];
				} else if (rhythm == 8) {
					RHYTHM_BTNMASH_ARRAY [0] [i] = RHYTHM_BTNMASH_ARRAY [0] [i - 2];
				} else {
					RHYTHM_BTNMASH_ARRAY [0] [i] = 0;
				}

				rhythmArray = subStrArray [i].ToCharArray ();
				rhythm = int.Parse (rhythmArray [rhythmArray.Length - 1].ToString ());
				RHYTHM_IDX_ARRAY [1] [i] = rhythm;
				if (Convert.ToInt32 (rhythmArray [0]) == 10) {
					RHYTHM_TYPE_ARRAY [1] [i] = (rhythmArray.Length == 2) ? '\0' : rhythmArray [1];
				} else {
					RHYTHM_TYPE_ARRAY [1] [i] = (rhythmArray.Length == 1) ? '\0' : rhythmArray [0];
				}

				if (rhythm == 7) {
					if (i > 1 && RHYTHM_BTNMASH_ARRAY [1] [i - 2] == 0) {
						RHYTHM_BTNMASH_ARRAY [1] [i - 2] = RHYTHM_IDX_ARRAY [1] [i - 2];
					}
					RHYTHM_BTNMASH_ARRAY [1] [i] = RHYTHM_BTNMASH_ARRAY [1] [i - 2];
				} else if (rhythm == 8) {
					RHYTHM_BTNMASH_ARRAY [1] [i] = RHYTHM_BTNMASH_ARRAY [1] [i - 2];
				} else {
					RHYTHM_BTNMASH_ARRAY [1] [i] = 0;
				}
			}
			Debug.Log ("InitLevelRhythm " + currentLevel + " end");
		}*/
	}
	
	// Use this for initialization
	void Start ()
	{
		Debug.Log ("RhythmCtrl Start");
	}
}
