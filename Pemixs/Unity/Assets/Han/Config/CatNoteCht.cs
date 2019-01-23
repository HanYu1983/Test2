using UnityEngine;
namespace Remix{
	public class CatNoteCht{
		public const int ID_COUNT = 28;

		public string ID {get; set;}
public string Name {get; set;}
public string SkillNote {get; set;}

		public static CatNoteCht Get(string key){
			switch (key) {
			case "C01": return new CatNoteCht{ID="C01",Name="橘毛",SkillNote="技能：我很會跑        跑酷模式傷害減少10%"};
case "C02": return new CatNoteCht{ID="C02",Name="黑臉",SkillNote="技能：我很愛跳        跳舞模式傷害減少10%"};
case "C03": return new CatNoteCht{ID="C03",Name="肥三",SkillNote="技能：我很能打        對戰模式傷害減少10%"};
case "C04": return new CatNoteCht{ID="C04",Name="喵喵",SkillNote="技能：我超會跑        跑酷模式傷害減少20%"};
case "C05": return new CatNoteCht{ID="C05",Name="香蕉",SkillNote="技能：我超愛跳        跳舞模式傷害減少20%"};
case "C06": return new CatNoteCht{ID="C06",Name="瞎抖",SkillNote="技能：我超能打        對戰模式傷害減少20%"};
case "C07": return new CatNoteCht{ID="C07",Name="三花",SkillNote="技能：我很會跑        跑酷模式傷害減少10%"};
case "C08": return new CatNoteCht{ID="C08",Name="黑輪",SkillNote="技能：我很愛跳        跳舞模式傷害減少10%"};
case "C09": return new CatNoteCht{ID="C09",Name="狸猫",SkillNote="技能：我很能打        對戰模式傷害減少10%"};
case "C10": return new CatNoteCht{ID="C10",Name="普魯士藍",SkillNote="技能：叫我超跑        跑酷模式傷害減少30%"};
case "C11": return new CatNoteCht{ID="C11",Name="單行道",SkillNote="技能：叫我舞王        跳舞模式傷害減少30%"};
case "C12": return new CatNoteCht{ID="C12",Name="椪柑",SkillNote="技能：斯巴達!          對戰模式傷害減少30%"};
case "C13": return new CatNoteCht{ID="C13",Name="娜娜",SkillNote="技能：我很會跑        跑酷模式傷害減少10%"};
case "C14": return new CatNoteCht{ID="C14",Name="歌舞伎",SkillNote="技能：我很愛跳        跳舞模式傷害減少10%"};
case "C15": return new CatNoteCht{ID="C15",Name="黑桃",SkillNote="技能：我很能打        對戰模式傷害減少10%"};
case "C16": return new CatNoteCht{ID="C16",Name="陰陽師",SkillNote="技能：我專跑馬拉松  跑酷模式傷害減少40%"};
case "C17": return new CatNoteCht{ID="C17",Name="牛奶",SkillNote="技能：我可以跳通宵  跳舞模式傷害減少40%"};
case "C18": return new CatNoteCht{ID="C18",Name="望遠鏡",SkillNote="技能：我一次打十個  對戰模式傷害減少40%"};
case "C19": return new CatNoteCht{ID="C19",Name="藍帶",SkillNote="技能：我超會跑        跑酷模式傷害減少20%"};
case "C20": return new CatNoteCht{ID="C20",Name="醬油",SkillNote="技能：我超愛跳        跳舞模式傷害減少20%"};
case "C21": return new CatNoteCht{ID="C21",Name="領太多",SkillNote="技能：我超能打        對戰模式傷害減少20%"};
case "C22": return new CatNoteCht{ID="C22",Name="吊眼",SkillNote="技能：我專跑馬拉松  跑酷模式傷害減少40%"};
case "C23": return new CatNoteCht{ID="C23",Name="芝麻",SkillNote="技能：我可以跳通宵  跳舞模式傷害減少40%"};
case "C24": return new CatNoteCht{ID="C24",Name="法老",SkillNote="技能：我一次打十個  對戰模式傷害減少40%"};
case "CM1": return new CatNoteCht{ID="CM1",Name="猞猁",SkillNote="技能：我可以跳通宵  跳舞模式傷害減少50%"};
case "CM2": return new CatNoteCht{ID="CM2",Name="短尾貓",SkillNote="技能：我一次打十個  對戰模式傷害減少50%"};
case "CM3": return new CatNoteCht{ID="CM3",Name="加拿大猞猁",SkillNote="技能：我專跑馬拉松  跑酷模式傷害減少50%"};
case "CM4": return new CatNoteCht{ID="CM4",Name="伊比利亞猞猁",SkillNote="技能：我專跑馬拉松  跑酷模式傷害減少50%"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static CatNoteCht Get(int key){
			switch (key) {
			case 0: return new CatNoteCht{ID="C01",Name="橘毛",SkillNote="技能：我很會跑        跑酷模式傷害減少10%"};
case 1: return new CatNoteCht{ID="C02",Name="黑臉",SkillNote="技能：我很愛跳        跳舞模式傷害減少10%"};
case 2: return new CatNoteCht{ID="C03",Name="肥三",SkillNote="技能：我很能打        對戰模式傷害減少10%"};
case 3: return new CatNoteCht{ID="C04",Name="喵喵",SkillNote="技能：我超會跑        跑酷模式傷害減少20%"};
case 4: return new CatNoteCht{ID="C05",Name="香蕉",SkillNote="技能：我超愛跳        跳舞模式傷害減少20%"};
case 5: return new CatNoteCht{ID="C06",Name="瞎抖",SkillNote="技能：我超能打        對戰模式傷害減少20%"};
case 6: return new CatNoteCht{ID="C07",Name="三花",SkillNote="技能：我很會跑        跑酷模式傷害減少10%"};
case 7: return new CatNoteCht{ID="C08",Name="黑輪",SkillNote="技能：我很愛跳        跳舞模式傷害減少10%"};
case 8: return new CatNoteCht{ID="C09",Name="狸猫",SkillNote="技能：我很能打        對戰模式傷害減少10%"};
case 9: return new CatNoteCht{ID="C10",Name="普魯士藍",SkillNote="技能：叫我超跑        跑酷模式傷害減少30%"};
case 10: return new CatNoteCht{ID="C11",Name="單行道",SkillNote="技能：叫我舞王        跳舞模式傷害減少30%"};
case 11: return new CatNoteCht{ID="C12",Name="椪柑",SkillNote="技能：斯巴達!          對戰模式傷害減少30%"};
case 12: return new CatNoteCht{ID="C13",Name="娜娜",SkillNote="技能：我很會跑        跑酷模式傷害減少10%"};
case 13: return new CatNoteCht{ID="C14",Name="歌舞伎",SkillNote="技能：我很愛跳        跳舞模式傷害減少10%"};
case 14: return new CatNoteCht{ID="C15",Name="黑桃",SkillNote="技能：我很能打        對戰模式傷害減少10%"};
case 15: return new CatNoteCht{ID="C16",Name="陰陽師",SkillNote="技能：我專跑馬拉松  跑酷模式傷害減少40%"};
case 16: return new CatNoteCht{ID="C17",Name="牛奶",SkillNote="技能：我可以跳通宵  跳舞模式傷害減少40%"};
case 17: return new CatNoteCht{ID="C18",Name="望遠鏡",SkillNote="技能：我一次打十個  對戰模式傷害減少40%"};
case 18: return new CatNoteCht{ID="C19",Name="藍帶",SkillNote="技能：我超會跑        跑酷模式傷害減少20%"};
case 19: return new CatNoteCht{ID="C20",Name="醬油",SkillNote="技能：我超愛跳        跳舞模式傷害減少20%"};
case 20: return new CatNoteCht{ID="C21",Name="領太多",SkillNote="技能：我超能打        對戰模式傷害減少20%"};
case 21: return new CatNoteCht{ID="C22",Name="吊眼",SkillNote="技能：我專跑馬拉松  跑酷模式傷害減少40%"};
case 22: return new CatNoteCht{ID="C23",Name="芝麻",SkillNote="技能：我可以跳通宵  跳舞模式傷害減少40%"};
case 23: return new CatNoteCht{ID="C24",Name="法老",SkillNote="技能：我一次打十個  對戰模式傷害減少40%"};
case 24: return new CatNoteCht{ID="CM1",Name="猞猁",SkillNote="技能：我可以跳通宵  跳舞模式傷害減少50%"};
case 25: return new CatNoteCht{ID="CM2",Name="短尾貓",SkillNote="技能：我一次打十個  對戰模式傷害減少50%"};
case 26: return new CatNoteCht{ID="CM3",Name="加拿大猞猁",SkillNote="技能：我專跑馬拉松  跑酷模式傷害減少50%"};
case 27: return new CatNoteCht{ID="CM4",Name="伊比利亞猞猁",SkillNote="技能：我專跑馬拉松  跑酷模式傷害減少50%"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}