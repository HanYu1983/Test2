using System;
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Remix
{
	public class MeowRemixEditor
	{
		[MenuItem ("MeowRemix/BuildAssetBundles")]
		static void BuildAllAssetBundles (){
			AssetBundles.BuildScript.BuildAssetBundles ();
		}

		[MenuItem ("MeowRemix/ShowLocalSavePath")]
		static void ShowLocalSavePath(){
			Debug.Log (GameConfig.SAVE_PATH);
		}

		[MenuItem ("MeowRemix/ClearPhoto")]
		static void ClearPhoto(){
			if (Directory.Exists (GameConfig.LOCAL_PHOTOS_PATH)) {
				foreach (string file in System.IO.Directory.GetFiles(GameConfig.LOCAL_PHOTOS_PATH)){
					Debug.Log ("delete "+file);
					File.Delete(file);
				}
				Debug.Log ("delete:"+GameConfig.LOCAL_PHOTOS_PATH);
				Directory.Delete (GameConfig.LOCAL_PHOTOS_PATH);
			}
		}

		[MenuItem ("MeowRemix/ClearAssetBundle")]
		static void ClearAssetBundle(){
			if (Directory.Exists (GameConfig.LOCAL_ASSET_BUNDELS_PATH)) {
				foreach (string file in System.IO.Directory.GetFiles(GameConfig.LOCAL_ASSET_BUNDELS_PATH)){
					Debug.Log ("delete "+file);
					File.Delete(file);
				}
				Debug.Log ("delete:"+GameConfig.LOCAL_ASSET_BUNDELS_PATH);
				Directory.Delete (GameConfig.LOCAL_ASSET_BUNDELS_PATH);
			}
		}

		[MenuItem ("MeowRemix/ClearAllSavedData")]
		static void ClearData(){
			
			if (Directory.Exists (GameConfig.SAVE_PATH)) {
				foreach (string file in System.IO.Directory.GetFiles(GameConfig.SAVE_PATH)){
					Debug.Log ("delete "+file);
					File.Delete(file);
				}
				Debug.Log ("delete:"+GameConfig.SAVE_PATH);
				Directory.Delete (GameConfig.SAVE_PATH);
			}

			ClearAssetBundle ();
			ClearPhoto ();

			if (Directory.Exists (GameConfig.LOCAL_MUSIC_PATH)) {
				foreach (string file in System.IO.Directory.GetFiles(GameConfig.LOCAL_MUSIC_PATH)){
					Debug.Log ("delete "+file);
					File.Delete(file);
				}
				Debug.Log ("delete:"+GameConfig.LOCAL_MUSIC_PATH);
				Directory.Delete (GameConfig.LOCAL_MUSIC_PATH);
			}

		}

		[MenuItem ("MeowRemix/GenerateConfig")]
		static void GenCatDef (){
			AssetDatabase.DeleteAsset ("/Han/Config");
			AssetDatabase.CreateFolder("/Han", "Config");

			WriteFile ("mainui_note_eng", "MainuiNoteEng", true, new List<object[]> () {
				new object[]{ "ID", "string" },
				new object[]{ "Desc", "string" }
			});
			WriteFile ("mainui_note_cht", "MainuiNoteCht", false, new List<object[]> () {
				new object[]{ "ID", "string" },
				new object[]{ "Desc", "string" }
			});
			WriteFile ("mainui_note_chs", "MainuiNoteChs", false, new List<object[]> () {
				new object[]{ "ID", "string" },
				new object[]{ "Desc", "string" }
			});
				
			WriteFile ("level_def", "LevelDef", false, new List<object[]> () {
				new object[]{"StageId", "string"},
				new object[]{"Map", "string"},
				new object[]{"Name", "string"},
				new object[]{"LevelPicID", "string"},
				new object[]{"Difficulty", "int"},
				new object[]{"Filename", "string"},
				new object[]{"Event", "string"},
				new object[]{"Damage", "int"},
				new object[]{"Reply", "int"},
				new object[]{"Gold", "int"},
				new object[]{"ItemID", "string"},
				new object[]{"ItemProbability", "float"},
			});

			WriteFile ("level_note_eng", "LevelNoteEng", false, new List<object[]> () {
				new object[]{"StageId", "string"},
				new object[]{"Name", "string"},
				new object[]{"TextGameMode", "string"},
				new object[]{"TextArtistTitle", "string"},
				new object[]{"TextSongTitle", "string"}
			});
			WriteFile ("level_note_cht", "LevelNoteCht", false, new List<object[]> () {
				new object[]{"StageId", "string"},
				new object[]{"Name", "string"},
				new object[]{"TextGameMode", "string"},
				new object[]{"TextArtistTitle", "string"},
				new object[]{"TextSongTitle", "string"}
			});
			WriteFile ("level_note_chs", "LevelNoteChs", false, new List<object[]> () {
				new object[]{"StageId", "string"},
				new object[]{"Name", "string"},
				new object[]{"TextGameMode", "string"},
				new object[]{"TextArtistTitle", "string"},
				new object[]{"TextSongTitle", "string"}
			});


			WriteFile ("cat_def", "CatDef", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Type", "string"},
				new object[]{"Name", "string"},
				new object[]{"Rare", "int"},
				new object[]{"HpRate", "int"},
				new object[]{"DHp", "float"},
				new object[]{"BHp", "float"},
				new object[]{"PHp", "float"},
				new object[]{"DDmg", "float"},
				new object[]{"BDmp", "float"},
				new object[]{"PDmg", "float"},
			});
			WriteFile ("cat_note_eng", "CatNoteEng", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Name", "string"},
				new object[]{"SkillNote", "string"}
			});
			WriteFile ("cat_note_cht", "CatNoteCht", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Name", "string"},
				new object[]{"SkillNote", "string"}
			});
			WriteFile ("cat_note_chs", "CatNoteChs", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Name", "string"},
				new object[]{"SkillNote", "string"}
			});
				
			WriteFile ("cat_lv_Rate1", "CatLvRate1", false, new List<object[]> () {
				new object[]{"Lv", "string"},
				new object[]{"Type", "string"},
				new object[]{"Exp", "int"},
				new object[]{"Hp", "int"},
			});
			WriteFile ("cat_lv_Rate2", "CatLvRate2", false, new List<object[]> () {
				new object[]{"Lv", "string"},
				new object[]{"Type", "string"},
				new object[]{"Exp", "int"},
				new object[]{"Hp", "int"},
			});
			WriteFile ("cat_lv_Rate3", "CatLvRate3", false, new List<object[]> () {
				new object[]{"Lv", "string"},
				new object[]{"Type", "string"},
				new object[]{"Exp", "int"},
				new object[]{"Hp", "int"},
			});
			WriteFile ("cat_lv_Rate4", "CatLvRate4", false, new List<object[]> () {
				new object[]{"Lv", "string"},
				new object[]{"Type", "string"},
				new object[]{"Exp", "int"},
				new object[]{"Hp", "int"},
			});
			WriteFile ("cat_lv_Rate5", "CatLvRate5", false, new List<object[]> () {
				new object[]{"Lv", "string"},
				new object[]{"Type", "string"},
				new object[]{"Exp", "int"},
				new object[]{"Hp", "int"},
			});
			WriteFile ("cat_lv_Rate6", "CatLvRate6", false, new List<object[]> () {
				new object[]{"Lv", "string"},
				new object[]{"Type", "string"},
				new object[]{"Exp", "int"},
				new object[]{"Hp", "int"},
			});
				
			WriteFile ("item_def", "ItemDef", true, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Type", "string"},
				new object[]{"Prefab", "string"},
				new object[]{"Name", "string"},
				new object[]{"Desc", "string"},
				new object[]{"Gold", "int"},
				new object[]{"Money", "int"},
				new object[]{"Enable", "int"},
				new object[]{"Hp", "int"},
				new object[]{"Exp", "int"},
				new object[]{"Time", "float"},
				new object[]{"ToPhoto", "float"},
				new object[]{"BPhoto", "float"},
				new object[]{"CatTypeCA", "int"},
				new object[]{"CatTypeCB", "int"},
				new object[]{"CatTypeCC", "int"},
				new object[]{"CatTypeCD", "int"},
				new object[]{"CatTypeCE", "int"},
				new object[]{"CatTypeCF", "int"},
				new object[]{"CatTypeCM", "int"},
				new object[]{"CatTypeCN", "int"},
			});
			WriteFile ("item_note_eng", "ItemNoteEng", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Name", "string"},
				new object[]{"Desc", "string"}
			});
			WriteFile ("item_note_cht", "ItemNoteCht", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Name", "string"},
				new object[]{"Desc", "string"}
			});
			WriteFile ("item_note_chs", "ItemNoteChs", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Name", "string"},
				new object[]{"Desc", "string"}
			});

			WriteFile ("iap_def_eng", "IAPDefEng", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Type", "string"},
				new object[]{"Prefab", "string"},
				new object[]{"Name", "string"},
				new object[]{"Desc", "string"},
				new object[]{"Cost", "string"},
				new object[]{"Gold", "int"},
				new object[]{"Money", "int"}
			});
			WriteFile ("iap_def_cht", "IAPDefCht", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Type", "string"},
				new object[]{"Prefab", "string"},
				new object[]{"Name", "string"},
				new object[]{"Desc", "string"},
				new object[]{"Cost", "string"},
				new object[]{"Gold", "int"},
				new object[]{"Money", "int"}
			});
			WriteFile ("iap_def_chs", "IAPDefChs", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Type", "string"},
				new object[]{"Prefab", "string"},
				new object[]{"Name", "string"},
				new object[]{"Desc", "string"},
				new object[]{"Cost", "string"},
				new object[]{"Gold", "int"},
				new object[]{"Money", "int"}
			});


			WriteFile ("gacha_def", "GachaDef", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Map", "string"},
				new object[]{"CatFile", "string"},
				new object[]{"PohtoFile", "string"},
				new object[]{"Unlock", "int"},
				new object[]{"CatTime", "int"},
				new object[]{"PhotoTime", "int"}
			});
				
			WriteFile ("gachapm01", "GachaPm01", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Type", "string"},
				new object[]{"Cat01", "string"},
				new object[]{"Cat01Weights", "int"},
				new object[]{"Cat02", "string"},
				new object[]{"Cat02Weights", "int"},
			});
			WriteFile ("gachapm02", "GachaPm02", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Type", "string"},
				new object[]{"Cat01", "string"},
				new object[]{"Cat01Weights", "int"},
				new object[]{"Cat02", "string"},
				new object[]{"Cat02Weights", "int"},
			});
			WriteFile ("gachapm03", "GachaPm03", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Type", "string"},
				new object[]{"Cat01", "string"},
				new object[]{"Cat01Weights", "int"},
				new object[]{"Cat02", "string"},
				new object[]{"Cat02Weights", "int"},
			});
			WriteFile ("gachapm04", "GachaPm04", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Type", "string"},
				new object[]{"Cat01", "string"},
				new object[]{"Cat01Weights", "int"},
				new object[]{"Cat02", "string"},
				new object[]{"Cat02Weights", "int"},
			});
			WriteFile ("gachapmS1", "GachaPmS1", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Type", "string"},
				new object[]{"Cat01", "string"},
				new object[]{"Cat01Weights", "int"},
				new object[]{"Cat02", "string"},
				new object[]{"Cat02Weights", "int"},
				new object[]{"Cat03", "string"},
				new object[]{"Cat03Weights", "int"},
			});

			WriteFile ("gift_def", "GiftDef", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Item", "string"},
				new object[]{"Quantity", "int"},
			});
				
			WriteFile ("map_def", "MapDef", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Event", "int"},
				new object[]{"Desc", "string"},
				new object[]{"Prefab", "string"}
			});

			WriteFile ("wmap_s", "WmapS", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Item", "string"},
				new object[]{"Quantity", "int"}
			});

			WriteFile ("wmap_s_cht", "WmapSCht", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Name", "string"},
				new object[]{"Desc", "string"}
			});
			WriteFile ("wmap_s_chs", "WmapSChs", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Name", "string"},
				new object[]{"Desc", "string"}
			});
			WriteFile ("wmap_s_eng", "WmapSEng", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Name", "string"},
				new object[]{"Desc", "string"}
			});

			WriteFile ("dlg_note_cht", "DlgNoteCht", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Desc", "string"}
			});
			WriteFile ("dlg_note_chs", "DlgNoteChs", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Desc", "string"}
			});
			WriteFile ("dlg_note_eng", "DlgNoteEng", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Desc", "string"}
			});

			WriteFile ("photom01", "Photom01", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Name", "string"},
				new object[]{"Type", "string"}
			});
			WriteFile ("photom02", "Photom02", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Name", "string"},
				new object[]{"Type", "string"}
			});
			WriteFile ("photom03", "Photom03", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Name", "string"},
				new object[]{"Type", "string"}
			});
			WriteFile ("photom04", "Photom04", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Name", "string"},
				new object[]{"Type", "string"}
			});
			WriteFile ("photomS1", "PhotomS1", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Name", "string"},
				new object[]{"Type", "string"}
			});

			WriteFile ("dlg_messeng_cht", "DlgMessageCht", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Dlg", "string"},
				new object[]{"Desc", "string"}
			});
			WriteFile ("dlg_messeng_chs", "DlgMessageChs", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Dlg", "string"},
				new object[]{"Desc", "string"}
			});
			WriteFile ("dlg_messeng_eng", "DlgMessageEng", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Dlg", "string"},
				new object[]{"Desc", "string"}
			});
				
			WriteFile ("music_note_cht", "MusicNoteCht", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"ArtistTitle", "string"},
				new object[]{"SongTitle", "string"}
			});
			WriteFile ("music_note_chs", "MusicNoteChs", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"ArtistTitle", "string"},
				new object[]{"SongTitle", "string"}
			});
			WriteFile ("music_note_eng", "MusicNoteEng", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"ArtistTitle", "string"},
				new object[]{"SongTitle", "string"}
			});

			WriteFile ("tutorial_note_cht", "TutorialNoteCht", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Page", "string"},
				new object[]{"Desc", "string"}
			});
			WriteFile ("tutorial_note_chs", "TutorialNoteChs", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Page", "string"},
				new object[]{"Desc", "string"}
			});
			WriteFile ("tutorial_note_eng", "TutorialNoteEng", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Page", "string"},
				new object[]{"Desc", "string"}
			});

			WriteFile ("tips_note_cht", "TipsNoteCht", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Title", "string"},
				new object[]{"Ch", "string"},
				new object[]{"Desc", "string"}
			});
			WriteFile ("tips_note_chs", "TipsNoteChs", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Title", "string"},
				new object[]{"Ch", "string"},
				new object[]{"Desc", "string"}
			});
			WriteFile ("tips_note_eng", "TipsNoteEng", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Title", "string"},
				new object[]{"Ch", "string"},
				new object[]{"Desc", "string"}
			});


			WriteFile ("training_note_cht", "TrainingNoteCht", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Page", "string"},
				new object[]{"Desc", "string"}
			}, true);
			WriteFile ("training_note_chs", "TrainingNoteChs", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Page", "string"},
				new object[]{"Desc", "string"}
			}, true);
			WriteFile ("training_note_eng", "TrainingNoteEng", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Page", "string"},
				new object[]{"Desc", "string"}
			}, true);


			WriteFile ("tutorial_note_cht", "TutorialNoteCht", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Page", "string"},
				new object[]{"Desc", "string"}
			});
			WriteFile ("tutorial_note_chs", "TutorialNoteChs", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Page", "string"},
				new object[]{"Desc", "string"}
			});
			WriteFile ("tutorial_note_eng", "TutorialNoteEng", false, new List<object[]> () {
				new object[]{"ID", "string"},
				new object[]{"Page", "string"},
				new object[]{"Desc", "string"}
			});

			AssetDatabase.Refresh();
		}

		[MenuItem ("MeowRemix/Test")]
		static void Test(){
			Debug.Log (WWW.UnEscapeURL ("技能：我很能打        對戰模式傷害減少10%"));
			Debug.Log (WWW.UnEscapeURL ("技能：狸  猫%"));
		}

		static string HttpRequest(string url, string body){
			WebRequest request = WebRequest.Create (url);
			request.Timeout = 10000;
			if (body != null) {
				request.Method = "POST";
				byte[] byteArray = Encoding.UTF8.GetBytes (body);
				request.ContentType = "application/x-www-form-urlencoded";
				request.ContentLength = byteArray.Length;
				var writeString = request.GetRequestStream ();
				writeString.Write (byteArray, 0, byteArray.Length);
				writeString.Close ();
			}
			WebResponse response = request.GetResponse ();
			var dataStream = response.GetResponseStream ();
			StreamReader reader = new StreamReader (dataStream);
			string responseFromServer = reader.ReadToEnd ();
			reader.Close ();
			dataStream.Close ();
			response.Close ();
			return responseFromServer;
		}


		static void WriteFile(string fileName, string clzName, bool genID, List<object[]> info, bool onlyIntID = false){
			var txt = GenCode (info, clzName, genID, Application.dataPath+"/Kuo/Data/"+fileName+".csv", onlyIntID);
			File.WriteAllText(Application.dataPath + "/Han/Config/"+clzName+".cs", txt);
		}

		public static string GenCode(List<object[]> info, string clzName, bool genID, string fileName, bool onlyIntID = false){
			var lineArray = File.ReadAllLines (fileName);
			string[][] strArray = new string [lineArray.Length][];  
			for (int i=0;i<lineArray.Length;i++){
				var line = lineArray [i];

				// ==== 處理""內的文字 ====
				var curr = 0;
				while (true) {
					var id1 = line.IndexOf ("\"", curr);
					if (id1 == -1) {
						break;
					}
					var id2 = line.IndexOf ("\"", id1+1);
					if (id2 == -1) {
						throw new UnityException ("格式錯誤，少一個\"");
					}
					var contentBetweenId12 = line.Substring (id1+1, (id2 - id1)-1);
					line = line.Replace ("\""+contentBetweenId12+"\"", WWW.EscapeURL (contentBetweenId12));

					curr = id2 + 1;
				}
				var tempAry = line.Split (','); 
				for (var j = 0; j < tempAry.Length; ++j) {
					var tempStr = tempAry [j];
					// 先將%符號換掉，不然在UnEscapeURL的時候會解析成????
					tempStr = tempStr.Replace ("%", "aatempaa");
					tempStr = WWW.UnEscapeURL (tempStr);
					// 再置換回來
					tempStr = tempStr.Replace ("aatempaa", "%");
					tempAry [j] = tempStr;
				}
				// ========

				strArray[i] = tempAry;
			}
			var data = strArray;

			var fields = "";
			for (var i = 0; i < info.Count; ++i) {
				var typeInfo = info [i];
				var key = (string)typeInfo [0];
				var type = (string)typeInfo [1];
				fields += string.Format ("public {0} {1} {{get; set;}}\n", type, key);
			}

			var consts = "";
			if (genID) {
				for (var i = 1; i < data.Length; ++i) {
					var v = data [i] [0];
					consts += string.Format ("public const string {0} = \"{1}\";\n", v.ToUpper (), v);
				}
			}
			consts += string.Format ("public const int ID_COUNT = {0};\n", data.Length-1); 

			var cases = "";
			var intCases = "";
			for (var i = 1; i < data.Length; ++i) {
				var fieldsAry = new List<string>();
				for(var j=0; j<data[0].Length; ++j){
					if (j >= info.Count) {
						continue;
					}
					var value = data [i] [j];
					if (value.Length == 0) {
						continue;
					}
					var typeInfo = info [j];
					var key = (string)typeInfo [0];
					var type = (string)typeInfo [1];
					switch (type) {
					case "float":
						try{
							float.Parse(value);
						}catch(Exception){
							throw new Exception ("欄位格式錯誤:"+key+":"+value);
						}
						fieldsAry.Add (string.Format ("{0}={1}f", key, value));
						break;
					case "int":
						try{
							int.Parse(value);
						}catch(Exception){
							throw new Exception ("欄位格式錯誤:"+key+":"+value);
						}
						fieldsAry.Add(string.Format ("{0}={1}", key, value));
						break;
					case "string":
						fieldsAry.Add(string.Format ("{0}=\"{1}\"", key, value));
						break;
					}
				}
				var fieldsStr = string.Join (",", fieldsAry.ToArray ());
				cases += string.Format ("case \"{0}\": return new {1}{{{2}}};\n", data[i][0], clzName, fieldsStr);
				intCases += string.Format ("case {0}: return new {1}{{{2}}};\n", i - 1, clzName, fieldsStr);
			}

			var template = @"using UnityEngine;
namespace Remix{{
	public class {0}{{
		{1}
		{2}
		public static {0} Get(string key){{
			switch (key) {{
			{3}
			default:
				throw new UnityException (""沒有這個定義:""+key);
			}}
		}}

		public static {0} Get(int key){{
			switch (key) {{
			{4}
			default:
				throw new UnityException (""沒有這個定義:""+key);
			}}
		}}
	}}
}}";
			if (onlyIntID) {
				cases = "";
			}
			var txt = string.Format (template, clzName, consts, fields, cases, intCases);
			return txt;
		}
	}
}

