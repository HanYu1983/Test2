using UnityEngine;
namespace Remix{
	public class CatNoteChs{
		public const int ID_COUNT = 28;

		public string ID {get; set;}
public string Name {get; set;}
public string SkillNote {get; set;}

		public static CatNoteChs Get(string key){
			switch (key) {
			case "C01": return new CatNoteChs{ID="C01",Name="橘毛",SkillNote="技能：我很会跑        跑酷模式伤害减少10%"};
case "C02": return new CatNoteChs{ID="C02",Name="黑脸",SkillNote="技能：我很爱跳        跳舞模式伤害减少10%"};
case "C03": return new CatNoteChs{ID="C03",Name="肥三",SkillNote="技能：我很能打        对战模式伤害减少10%"};
case "C04": return new CatNoteChs{ID="C04",Name="喵喵",SkillNote="技能：我超会跑        跑酷模式伤害减少20%"};
case "C05": return new CatNoteChs{ID="C05",Name="香蕉",SkillNote="技能：我超爱跳        跳舞模式伤害减少20%"};
case "C06": return new CatNoteChs{ID="C06",Name="瞎抖",SkillNote="技能：我超能打        对战模式伤害减少20%"};
case "C07": return new CatNoteChs{ID="C07",Name="三花",SkillNote="技能：我很会跑        跑酷模式伤害减少10%"};
case "C08": return new CatNoteChs{ID="C08",Name="黑轮",SkillNote="技能：我很爱跳        跳舞模式伤害减少10%"};
case "C09": return new CatNoteChs{ID="C09",Name="狸猫",SkillNote="技能：我很能打        对战模式伤害减少10%"};
case "C10": return new CatNoteChs{ID="C10",Name="普鲁士蓝",SkillNote="技能：叫我超跑        跑酷模式伤害减少30%"};
case "C11": return new CatNoteChs{ID="C11",Name="单行道",SkillNote="技能：叫我舞王        跳舞模式伤害减少30%"};
case "C12": return new CatNoteChs{ID="C12",Name="椪柑",SkillNote="技能：斯巴达!           对战模式伤害减少30%"};
case "C13": return new CatNoteChs{ID="C13",Name="娜娜",SkillNote="技能：我很会跑        跑酷模式伤害减少10%"};
case "C14": return new CatNoteChs{ID="C14",Name="歌舞伎",SkillNote="技能：我很爱跳        跳舞模式伤害减少10%"};
case "C15": return new CatNoteChs{ID="C15",Name="黑桃",SkillNote="技能：我很能打        对战模式伤害减少10%"};
case "C16": return new CatNoteChs{ID="C16",Name="阴阳师",SkillNote="技能：我专跑马拉松  跑酷模式伤害减少40%"};
case "C17": return new CatNoteChs{ID="C17",Name="牛奶",SkillNote="技能：我可以跳通宵  跳舞模式伤害减少40%"};
case "C18": return new CatNoteChs{ID="C18",Name="望远镜",SkillNote="技能：我一次打十个  对战模式伤害减少40%"};
case "C19": return new CatNoteChs{ID="C19",Name="蓝带",SkillNote="技能：我超会跑        跑酷模式伤害减少20%"};
case "C20": return new CatNoteChs{ID="C20",Name="酱油",SkillNote="技能：我超爱跳        跳舞模式伤害减少20%"};
case "C21": return new CatNoteChs{ID="C21",Name="领太多",SkillNote="技能：我超能打        对战模式伤害减少20%"};
case "C22": return new CatNoteChs{ID="C22",Name="吊眼",SkillNote="技能：我专跑马拉松  跑酷模式伤害减少40%"};
case "C23": return new CatNoteChs{ID="C23",Name="芝麻",SkillNote="技能：我可以跳通宵  跳舞模式伤害减少40%"};
case "C24": return new CatNoteChs{ID="C24",Name="法老",SkillNote="技能：我一次打十个  对战模式伤害减少40%"};
case "CM1": return new CatNoteChs{ID="CM1",Name="猞猁",SkillNote="技能：我可以跳通宵  跳舞模式伤害减少50%"};
case "CM2": return new CatNoteChs{ID="CM2",Name="短尾猫",SkillNote="技能：我一次打十个  对战模式伤害减少50%"};
case "CM3": return new CatNoteChs{ID="CM3",Name="加拿大猞猁",SkillNote="技能：我专跑马拉松  跑酷模式伤害减少50%"};
case "CM4": return new CatNoteChs{ID="CM4",Name="伊比利亚猞猁",SkillNote="技能：我专跑马拉松  跑酷模式伤害减少50%"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static CatNoteChs Get(int key){
			switch (key) {
			case 0: return new CatNoteChs{ID="C01",Name="橘毛",SkillNote="技能：我很会跑        跑酷模式伤害减少10%"};
case 1: return new CatNoteChs{ID="C02",Name="黑脸",SkillNote="技能：我很爱跳        跳舞模式伤害减少10%"};
case 2: return new CatNoteChs{ID="C03",Name="肥三",SkillNote="技能：我很能打        对战模式伤害减少10%"};
case 3: return new CatNoteChs{ID="C04",Name="喵喵",SkillNote="技能：我超会跑        跑酷模式伤害减少20%"};
case 4: return new CatNoteChs{ID="C05",Name="香蕉",SkillNote="技能：我超爱跳        跳舞模式伤害减少20%"};
case 5: return new CatNoteChs{ID="C06",Name="瞎抖",SkillNote="技能：我超能打        对战模式伤害减少20%"};
case 6: return new CatNoteChs{ID="C07",Name="三花",SkillNote="技能：我很会跑        跑酷模式伤害减少10%"};
case 7: return new CatNoteChs{ID="C08",Name="黑轮",SkillNote="技能：我很爱跳        跳舞模式伤害减少10%"};
case 8: return new CatNoteChs{ID="C09",Name="狸猫",SkillNote="技能：我很能打        对战模式伤害减少10%"};
case 9: return new CatNoteChs{ID="C10",Name="普鲁士蓝",SkillNote="技能：叫我超跑        跑酷模式伤害减少30%"};
case 10: return new CatNoteChs{ID="C11",Name="单行道",SkillNote="技能：叫我舞王        跳舞模式伤害减少30%"};
case 11: return new CatNoteChs{ID="C12",Name="椪柑",SkillNote="技能：斯巴达!           对战模式伤害减少30%"};
case 12: return new CatNoteChs{ID="C13",Name="娜娜",SkillNote="技能：我很会跑        跑酷模式伤害减少10%"};
case 13: return new CatNoteChs{ID="C14",Name="歌舞伎",SkillNote="技能：我很爱跳        跳舞模式伤害减少10%"};
case 14: return new CatNoteChs{ID="C15",Name="黑桃",SkillNote="技能：我很能打        对战模式伤害减少10%"};
case 15: return new CatNoteChs{ID="C16",Name="阴阳师",SkillNote="技能：我专跑马拉松  跑酷模式伤害减少40%"};
case 16: return new CatNoteChs{ID="C17",Name="牛奶",SkillNote="技能：我可以跳通宵  跳舞模式伤害减少40%"};
case 17: return new CatNoteChs{ID="C18",Name="望远镜",SkillNote="技能：我一次打十个  对战模式伤害减少40%"};
case 18: return new CatNoteChs{ID="C19",Name="蓝带",SkillNote="技能：我超会跑        跑酷模式伤害减少20%"};
case 19: return new CatNoteChs{ID="C20",Name="酱油",SkillNote="技能：我超爱跳        跳舞模式伤害减少20%"};
case 20: return new CatNoteChs{ID="C21",Name="领太多",SkillNote="技能：我超能打        对战模式伤害减少20%"};
case 21: return new CatNoteChs{ID="C22",Name="吊眼",SkillNote="技能：我专跑马拉松  跑酷模式伤害减少40%"};
case 22: return new CatNoteChs{ID="C23",Name="芝麻",SkillNote="技能：我可以跳通宵  跳舞模式伤害减少40%"};
case 23: return new CatNoteChs{ID="C24",Name="法老",SkillNote="技能：我一次打十个  对战模式伤害减少40%"};
case 24: return new CatNoteChs{ID="CM1",Name="猞猁",SkillNote="技能：我可以跳通宵  跳舞模式伤害减少50%"};
case 25: return new CatNoteChs{ID="CM2",Name="短尾猫",SkillNote="技能：我一次打十个  对战模式伤害减少50%"};
case 26: return new CatNoteChs{ID="CM3",Name="加拿大猞猁",SkillNote="技能：我专跑马拉松  跑酷模式伤害减少50%"};
case 27: return new CatNoteChs{ID="CM4",Name="伊比利亚猞猁",SkillNote="技能：我专跑马拉松  跑酷模式伤害减少50%"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}