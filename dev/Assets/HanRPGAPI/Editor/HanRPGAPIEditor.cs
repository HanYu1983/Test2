using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HanRPGAPI
{
	public class HanRPGAPIEditor
	{
		public static string ASSET_PATH = "Assets/HanRPGAPI/HanRPGAPIConfig.asset";	

		[MenuItem("HanRPGAPI/HanRPGAPIConfig")]
		public static void CreateAsset()
		{
			var info = ScriptableObject.CreateInstance<HanRPGAPIConfig>();
			info.classPath = Application.dataPath + "/HanRPGAPI/Config/";
			info.csvPath = Application.dataPath + "/HanRPGAPI/CSV/";
			string path = AssetDatabase.GenerateUniqueAssetPath(ASSET_PATH);
			AssetDatabase.CreateAsset(info, path);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		[MenuItem ("HanRPGAPI/CSV2Class")]
		public static void GenConfig(){
			var config = AssetDatabase.LoadAssetAtPath<HanRPGAPIConfig> (ASSET_PATH);
			if (config == null) {
				throw new Exception ("請先執行:HanRPGAPI/CreateConfigAsset");
			}
			var path = config.classPath;
			var csvPath = config.csvPath;

			GenCode ("ConfigConst", null, new string[]{
				"ID", "string",
				"Float", "float",
				"Int", "int", 
				"String", "string", 
			}, csvPath+"/gameData - const.tsv", path);

			GenCode ("ConfigItemType", null, new string[]{
				"ID", "string",
				"Name", "string",
				"Description", "string", 
			}, csvPath+"/gameData - itemType.tsv", path);

			GenCode ("ConfigMonster", null, new string[]{
				"ID", "string",
				"Name", "string",
				"Description", "string", 
				"Item","string",
				"Terrian", "string",
				"Str", "int",
				"Vit", "int",
				"Agi", "int",
				"Dex", "int", 
				"Int","int",
				"Luc","int",
				"Characteristic","string",
			}, csvPath+"/gameData - monster.tsv", path);

			GenCode ("ConfigResource", null, new string[]{
				"ID", "string",
				"Name", "string",
				"Description", "string", 
				"Item","string",
				"UseCount", "int"
			}, csvPath+"/gameData - resource.tsv", path);

			GenCode ("ConfigTerrian", null, new string[]{
				"ID", "string",
				"Name", "string",
				"Description", "string", 
				"Require","string",
				"Class","int",
				"MoveRequire", "string"
			}, csvPath+"/gameData - terrian.tsv", path);

			GenCode ("ConfigItem", null, new string[]{
				"ID", "string",
				"Name", "string",
				"Description", "string", 
				"Type","string",
				"SkillType","string",
				"MaxCount","int",
				"FusionRequire","string",
				"SkillRequire","string",
				"Ability","string",
				"Position","string",
				"UseCount","int",
			}, csvPath+"/gameData - item.tsv", path);

			GenCode ("ConfigSkillType", null, new string[]{
				"ID", "string",
				"Name", "string",
				"Description", "string", 
			}, csvPath+"/gameData - skillType.tsv", path);

			GenCode ("ConfigSkill", null, new string[]{
				"ID", "string",
				"Name", "string",
				"Description", "string", 
				"SkillTypeRequire", "string",
				"TriggerBouns", "float",
				"Condition", "string",
				"SlotCount", "int",
				"Effect", "string",
				"Values", "string",
				"Characteristic","string",
			}, csvPath+"/gameData - skill.tsv", path);

			GenCode ("ConfigCharacteristic", null, new string[]{
				"ID", "string",
				"Name", "string",
				"Description", "string",
			}, csvPath+"/gameData - Characteristic.tsv", path);

			GenCode ("ConfigConditionType", null, new string[]{
				"ID", "string",
				"Name", "string",
				"Description", "string", 
			}, csvPath+"/gameData - conditionType.tsv", path);

			GenCode ("ConfigNpc", null, new string[]{
				"ID", "string",
				"Name", "string",
				"Description", "string", 
			}, csvPath+"/gameData - npc.tsv", path);

			GenCode ("ConfigNpcMission", null, new string[]{
				"ID", "string",
				"Npc", "string",
				"Level", "int",
				"RequireItem", "string",
				"RequireKill", "string",
				"RequireStatus", "string",
				"Reward", "string",
				"Dependency", "string",
				"Dialog", "string",
			}, csvPath+"/gameData - npcMission.tsv", path);

			GenCode ("ConfigAbility", null, new string[]{
				"ID", "string",
				"Str", "float",
				"Vit", "float",
				"Agi", "float",
				"Dex", "float", 
				"Int","float",
				"Luc","float",
			}, csvPath+"/gameData - ability.tsv", path);

			GenCode ("ConfigWeaponPosition", null, new string[]{
				"ID", "string",
				"Name", "string",
				"SlotCount", "int",
			}, csvPath+"/gameData - weaponPosition.tsv", path);

			AssetDatabase.Refresh();
		}

		public static void GenCode(string clzName, string parent, string[] typeInfo, string fileName, string savePath){
			var csv = ReadCSV (fileName, '\t');
			var code = WriteClass (
				"HanRPGAPI",
				clzName + (parent != null ? (" :" + parent) : ""), 
				clzName, 
				typeInfo,
				csv
			);
			File.WriteAllText(savePath+"/"+clzName+".cs", code);
		}

		public static string WriteClass(string ns, string clz, string retClz, string[] typeInfo, string[][] csv){
			var str = "";
			str += "using System;\n";
			str += string.Format("namespace {0}{{\n", ns);
			str += string.Format("public class {0} {{\n", clz);
			{
				// getter setter
				for (var i = 0; i < typeInfo.Length; i += 2) {
					var key = typeInfo [i];
					var type = typeInfo [i + 1];
					str += string.Format ("public {0} {1} {{ get; set; }}\n", type, key);
				}
				// count
				str += "public const int ID_COUNT = " + (csv.Length-1)+";\n";
				for (var i = 1; i < csv.Length; ++i) {
					var id = csv [i][0];
					str += string.Format ("public const string ID_{0} = \"{0}\";\n", id);
				}
				// int key
				str += string.Format ("public static {0} Get(int key){{\n", retClz);
				{
					str += "switch(key){\n";
					{
						for (var i = 1; i < csv.Length; ++i) {
							var id = i-1;
							str += string.Format ("case {0}: return new {1} {{{2}}};\n", id, clz, WriteAssignment(typeInfo, i, csv));
						}
						str += "default: throw new Exception(key+\"\");\n";
					}
					str += "}";
				}
				str += "}";
				// str key
				str += string.Format ("public static {0} Get(string key){{\n", retClz);
				{
					str += "switch(key){\n";
					{
						for (var i = 1; i < csv.Length; ++i) {
							var id = csv [i][0];
							str += string.Format ("case \"{0}\": return new {1} {{{2}}};\n", id, clz, WriteAssignment(typeInfo, i, csv));
						}
						str += "default: throw new Exception(key);\n";
					}
					str += "}";
				}
				str += "}";
			}
			str += "}";
			str += "}";
			return str;
		}

		public static string WriteAssignment(string[] info, int i, string[][] data){
			var fieldsAry = new List<string>();
			for(var j=0; j<data[0].Length; ++j){
				if (j*2 >= info.Length) {
					continue;
				}
				var value = data [i] [j];
				if (value.Length == 0) {
					continue;
				}
				var key = info [j*2];
				var type = info [j*2+1];
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
			return string.Join (",", fieldsAry.ToArray ());
		}

		public static string[][] ReadCSV(string fileName, char split = ','){
			var lineArray = File.ReadAllLines (fileName);
			string[][] strArray = new string [lineArray.Length][];  
			for (int i=0;i<lineArray.Length;i++){
				var line = lineArray [i];
				var needSpecialProcess = line.IndexOf ("\"") != -1;
				if (needSpecialProcess) {
					// ==== 處理""內的文字 ====
					var curr = 0;
					while (true) {
						var id1 = line.IndexOf ("\"", curr);
						if (id1 == -1) {
							break;
						}
						var id2 = line.IndexOf ("\"", id1 + 1);
						if (id2 == -1) {
							throw new Exception ("格式錯誤，少一個\"");
						}
						var contentBetweenId12 = line.Substring (id1 + 1, (id2 - id1) - 1);
						line = line.Replace ("\"" + contentBetweenId12 + "\"", WWW.EscapeURL (contentBetweenId12));

						curr = id2 + 1;
					}
					var tempAry = line.Split (split); 
					for (var j = 0; j < tempAry.Length; ++j) {
						var tempStr = tempAry [j];
						var replaceMark = "anystringforreplace";
						// 先將%符號換掉，不然在UnEscapeURL的時候會解析成????
						tempStr = tempStr.Replace ("%", replaceMark);
						tempStr = WWW.UnEscapeURL (tempStr);
						// 再置換回來
						tempStr = tempStr.Replace (replaceMark, "%");
						tempAry [j] = tempStr;
					}
					strArray [i] = tempAry;
				} else {
					strArray [i] = line.Split (split);
				}
			}
			return strArray;
		}
	}
}

