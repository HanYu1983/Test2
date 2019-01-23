using UnityEngine;
namespace Remix{
	public class ItemNoteChs{
		public const int ID_COUNT = 58;

		public string ID {get; set;}
public string Name {get; set;}
public string Desc {get; set;}

		public static ItemNoteChs Get(string key){
			switch (key) {
			case "I10010": return new ItemNoteChs{ID="I10010",Name="猫罐",Desc="在互动模式与游玩关卡食用，可恢复HP50。"};
case "I10020": return new ItemNoteChs{ID="I10020",Name="牛奶",Desc="在互动模式与游玩关卡食用，可恢复HP150。"};
case "I10030": return new ItemNoteChs{ID="I10030",Name="干干",Desc="在互动模式与游玩关卡食用，可恢复HP300。"};
case "I10040": return new ItemNoteChs{ID="I10040",Name="猫草",Desc="在互动模式与游玩关卡食用，可恢复HP500。"};
case "I10050": return new ItemNoteChs{ID="I10050",Name="小鱼干",Desc="在互动模式与游玩关卡食用，可恢复HP250。"};
case "I10060": return new ItemNoteChs{ID="I10060",Name="柴鱼",Desc="在互动模式与游玩关卡食用，可恢复HP500。"};
case "I10070": return new ItemNoteChs{ID="I10070",Name="鲜鱼",Desc="在互动模式与游玩关卡食用，可恢复HP500并增加EXP500。"};
case "I10080": return new ItemNoteChs{ID="I10080",Name="高级餐包",Desc="在互动模式与游玩关卡食用，可恢复HP1000并增加EXP1000。"};
case "I20010": return new ItemNoteChs{ID="I20010",Name="数位相机",Desc="在探索模式使用，可获得一张一般照片。"};
case "I20020": return new ItemNoteChs{ID="I20020",Name="拍立得相机",Desc="在探索模式使用，可获得一张一般照片，并减少等待时间50%。"};
case "I20030": return new ItemNoteChs{ID="I20030",Name="运动相机",Desc="在探索模式使用，可获得一张一般照片，并减少等待时间100%。"};
case "I20040": return new ItemNoteChs{ID="I20040",Name="单眼相机",Desc="在探索模式使用，可获得一张一般照片，并有20%机率获得环景照片。"};
case "I20050": return new ItemNoteChs{ID="I20050",Name="数位单眼相机",Desc="在探索模式使用，可获得一张一般照片，并有40%机率获得环景照片。"};
case "I20060": return new ItemNoteChs{ID="I20060",Name="蛇腹相机",Desc="在探索模式使用，可获得一张一般照片，并有50%机率获得第二张照片。"};
case "I20070": return new ItemNoteChs{ID="I20070",Name="双眼相机",Desc="在探索模式使用，可获得二张一般照片。"};
case "I20080": return new ItemNoteChs{ID="I20080",Name="120相机",Desc="在探索模式使用，可获得二张一般照片，并有40%机率获得环景照片。"};
case "I21010": return new ItemNoteChs{ID="I21010",Name="自拍棒",Desc="在探索模式使用，可减少拍摄等待时间50%"};
case "I21020": return new ItemNoteChs{ID="I21020",Name="相机脚架",Desc="在探索模式使用，可减少拍摄等待时间100%。"};
case "I30010": return new ItemNoteChs{ID="I30010",Name="雷射光笔",Desc="成猫体型专用玩具，在互动模式使用，可增加经验值50。"};
case "I30020": return new ItemNoteChs{ID="I30020",Name="玩具老鼠",Desc="成猫体型专用玩具，在互动模式使用，可增加经验值200。"};
case "I30030": return new ItemNoteChs{ID="I30030",Name="取消中"};
case "I30040": return new ItemNoteChs{ID="I30040",Name="逗猫棒",Desc="幼猫体型专用玩具，在互动模式使用，可增加经验值50。"};
case "I30050": return new ItemNoteChs{ID="I30050",Name="毛线球",Desc="幼猫体型专用玩具，在互动模式使用，可增加经验值200。"};
case "I30060": return new ItemNoteChs{ID="I30060",Name="取消中"};
case "I30070": return new ItemNoteChs{ID="I30070",Name="猫抓板",Desc="胖猫体型专用玩具，在互动模式使用，可增加经验值50。"};
case "I30080": return new ItemNoteChs{ID="I30080",Name="暖炉",Desc="胖猫体型专用玩具，在互动模式使用，可增加经验值200。"};
case "I30090": return new ItemNoteChs{ID="I30090",Name="取消中"};
case "I30100": return new ItemNoteChs{ID="I30100",Name="取消中"};
case "I30110": return new ItemNoteChs{ID="I30110",Name="取消中"};
case "I31010": return new ItemNoteChs{ID="I31010",Name="猫草喷剂",Desc="在探索模式与互动模式使用，可减少等待时间50%。"};
case "I31020": return new ItemNoteChs{ID="I31020",Name="罐装干猫草",Desc="在探索模式与互动模式使用，可减少等待时间100%。"};
case "I32010": return new ItemNoteChs{ID="I32010",Name="宅配纸箱",Desc="成猫体型专用道具，在探索模式使用，可捕捉到成猫。"};
case "I32020": return new ItemNoteChs{ID="I32020",Name="扫地机器人",Desc="幼猫体型专用道具，在探索模式使用，可捕捉到幼猫。"};
case "I32030": return new ItemNoteChs{ID="I32030",Name="沙发床",Desc="胖猫体型专用道具，在探索模式使用，可捕捉到胖猫。"};
case "I40010": return new ItemNoteChs{ID="I40010",Name="猫金币 250",Desc="获得猫金币 250"};
case "I40020": return new ItemNoteChs{ID="I40020",Name="猫金币 500",Desc="获得猫金币 500"};
case "I40030": return new ItemNoteChs{ID="I40030",Name="猫金币 750",Desc="获得猫金币 750"};
case "I40040": return new ItemNoteChs{ID="I40040",Name="猫金币 1000",Desc="获得猫金币 1000"};
case "I40050": return new ItemNoteChs{ID="I40050",Name="猫金币 1500",Desc="获得猫金币 1500"};
case "I41010": return new ItemNoteChs{ID="I41010",Name="猫银票 20",Desc="获得猫银票 20"};
case "I41020": return new ItemNoteChs{ID="I41020",Name="猫银票 40",Desc="获得猫银票 40"};
case "I41030": return new ItemNoteChs{ID="I41030",Name="猫银票 10",Desc="获得猫银票 10"};
case "I41040": return new ItemNoteChs{ID="I41040",Name="猫银票 5",Desc="获得猫银票 5"};
case "I41050": return new ItemNoteChs{ID="I41050",Name="猫银票 2",Desc="获得猫银票 2"};
case "I41060": return new ItemNoteChs{ID="I41060",Name="猫银票 1",Desc="获得猫银票 1"};
case "I41070": return new ItemNoteChs{ID="I41070",Name="預留空位"};
case "I41080": return new ItemNoteChs{ID="I41080",Name="預留空位"};
case "I41090": return new ItemNoteChs{ID="I41090",Name="預留空位"};
case "I41100": return new ItemNoteChs{ID="I41100",Name="預留空位"};
case "I41110": return new ItemNoteChs{ID="I41110",Name="預留空位"};
case "I41120": return new ItemNoteChs{ID="I41120",Name="預留空位"};
case "I50010": return new ItemNoteChs{ID="I50010",Name="鲑鱼",Desc="大型猫专用食物，在互动模式与游玩关卡食用，可恢复HP150。"};
case "I50020": return new ItemNoteChs{ID="I50020",Name="带骨肉",Desc="大型猫专用食物，在互动模式与游玩关卡食用，可恢复HP300。"};
case "I50030": return new ItemNoteChs{ID="I50030",Name="烤乳猪",Desc="大型猫专用食物，在互动模式与游玩关卡食用，可恢复HP1000并增加EXP500。"};
case "I60010": return new ItemNoteChs{ID="I60010",Name="南瓜",Desc="猞猁体型专用玩具，在互动模式使用，可增加经验值100。"};
case "I60020": return new ItemNoteChs{ID="I60020",Name="木人桩",Desc="猞猁体型专用玩具，在互动模式使用，可增加经验值300。"};
case "I60030": return new ItemNoteChs{ID="I60030",Name="取消中"};
case "I62010": return new ItemNoteChs{ID="I62010",Name="雪橇",Desc="猞猁体型专用道具，在探索模式使用，可捕捉到猞猁。"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static ItemNoteChs Get(int key){
			switch (key) {
			case 0: return new ItemNoteChs{ID="I10010",Name="猫罐",Desc="在互动模式与游玩关卡食用，可恢复HP50。"};
case 1: return new ItemNoteChs{ID="I10020",Name="牛奶",Desc="在互动模式与游玩关卡食用，可恢复HP150。"};
case 2: return new ItemNoteChs{ID="I10030",Name="干干",Desc="在互动模式与游玩关卡食用，可恢复HP300。"};
case 3: return new ItemNoteChs{ID="I10040",Name="猫草",Desc="在互动模式与游玩关卡食用，可恢复HP500。"};
case 4: return new ItemNoteChs{ID="I10050",Name="小鱼干",Desc="在互动模式与游玩关卡食用，可恢复HP250。"};
case 5: return new ItemNoteChs{ID="I10060",Name="柴鱼",Desc="在互动模式与游玩关卡食用，可恢复HP500。"};
case 6: return new ItemNoteChs{ID="I10070",Name="鲜鱼",Desc="在互动模式与游玩关卡食用，可恢复HP500并增加EXP500。"};
case 7: return new ItemNoteChs{ID="I10080",Name="高级餐包",Desc="在互动模式与游玩关卡食用，可恢复HP1000并增加EXP1000。"};
case 8: return new ItemNoteChs{ID="I20010",Name="数位相机",Desc="在探索模式使用，可获得一张一般照片。"};
case 9: return new ItemNoteChs{ID="I20020",Name="拍立得相机",Desc="在探索模式使用，可获得一张一般照片，并减少等待时间50%。"};
case 10: return new ItemNoteChs{ID="I20030",Name="运动相机",Desc="在探索模式使用，可获得一张一般照片，并减少等待时间100%。"};
case 11: return new ItemNoteChs{ID="I20040",Name="单眼相机",Desc="在探索模式使用，可获得一张一般照片，并有20%机率获得环景照片。"};
case 12: return new ItemNoteChs{ID="I20050",Name="数位单眼相机",Desc="在探索模式使用，可获得一张一般照片，并有40%机率获得环景照片。"};
case 13: return new ItemNoteChs{ID="I20060",Name="蛇腹相机",Desc="在探索模式使用，可获得一张一般照片，并有50%机率获得第二张照片。"};
case 14: return new ItemNoteChs{ID="I20070",Name="双眼相机",Desc="在探索模式使用，可获得二张一般照片。"};
case 15: return new ItemNoteChs{ID="I20080",Name="120相机",Desc="在探索模式使用，可获得二张一般照片，并有40%机率获得环景照片。"};
case 16: return new ItemNoteChs{ID="I21010",Name="自拍棒",Desc="在探索模式使用，可减少拍摄等待时间50%"};
case 17: return new ItemNoteChs{ID="I21020",Name="相机脚架",Desc="在探索模式使用，可减少拍摄等待时间100%。"};
case 18: return new ItemNoteChs{ID="I30010",Name="雷射光笔",Desc="成猫体型专用玩具，在互动模式使用，可增加经验值50。"};
case 19: return new ItemNoteChs{ID="I30020",Name="玩具老鼠",Desc="成猫体型专用玩具，在互动模式使用，可增加经验值200。"};
case 20: return new ItemNoteChs{ID="I30030",Name="取消中"};
case 21: return new ItemNoteChs{ID="I30040",Name="逗猫棒",Desc="幼猫体型专用玩具，在互动模式使用，可增加经验值50。"};
case 22: return new ItemNoteChs{ID="I30050",Name="毛线球",Desc="幼猫体型专用玩具，在互动模式使用，可增加经验值200。"};
case 23: return new ItemNoteChs{ID="I30060",Name="取消中"};
case 24: return new ItemNoteChs{ID="I30070",Name="猫抓板",Desc="胖猫体型专用玩具，在互动模式使用，可增加经验值50。"};
case 25: return new ItemNoteChs{ID="I30080",Name="暖炉",Desc="胖猫体型专用玩具，在互动模式使用，可增加经验值200。"};
case 26: return new ItemNoteChs{ID="I30090",Name="取消中"};
case 27: return new ItemNoteChs{ID="I30100",Name="取消中"};
case 28: return new ItemNoteChs{ID="I30110",Name="取消中"};
case 29: return new ItemNoteChs{ID="I31010",Name="猫草喷剂",Desc="在探索模式与互动模式使用，可减少等待时间50%。"};
case 30: return new ItemNoteChs{ID="I31020",Name="罐装干猫草",Desc="在探索模式与互动模式使用，可减少等待时间100%。"};
case 31: return new ItemNoteChs{ID="I32010",Name="宅配纸箱",Desc="成猫体型专用道具，在探索模式使用，可捕捉到成猫。"};
case 32: return new ItemNoteChs{ID="I32020",Name="扫地机器人",Desc="幼猫体型专用道具，在探索模式使用，可捕捉到幼猫。"};
case 33: return new ItemNoteChs{ID="I32030",Name="沙发床",Desc="胖猫体型专用道具，在探索模式使用，可捕捉到胖猫。"};
case 34: return new ItemNoteChs{ID="I40010",Name="猫金币 250",Desc="获得猫金币 250"};
case 35: return new ItemNoteChs{ID="I40020",Name="猫金币 500",Desc="获得猫金币 500"};
case 36: return new ItemNoteChs{ID="I40030",Name="猫金币 750",Desc="获得猫金币 750"};
case 37: return new ItemNoteChs{ID="I40040",Name="猫金币 1000",Desc="获得猫金币 1000"};
case 38: return new ItemNoteChs{ID="I40050",Name="猫金币 1500",Desc="获得猫金币 1500"};
case 39: return new ItemNoteChs{ID="I41010",Name="猫银票 20",Desc="获得猫银票 20"};
case 40: return new ItemNoteChs{ID="I41020",Name="猫银票 40",Desc="获得猫银票 40"};
case 41: return new ItemNoteChs{ID="I41030",Name="猫银票 10",Desc="获得猫银票 10"};
case 42: return new ItemNoteChs{ID="I41040",Name="猫银票 5",Desc="获得猫银票 5"};
case 43: return new ItemNoteChs{ID="I41050",Name="猫银票 2",Desc="获得猫银票 2"};
case 44: return new ItemNoteChs{ID="I41060",Name="猫银票 1",Desc="获得猫银票 1"};
case 45: return new ItemNoteChs{ID="I41070",Name="預留空位"};
case 46: return new ItemNoteChs{ID="I41080",Name="預留空位"};
case 47: return new ItemNoteChs{ID="I41090",Name="預留空位"};
case 48: return new ItemNoteChs{ID="I41100",Name="預留空位"};
case 49: return new ItemNoteChs{ID="I41110",Name="預留空位"};
case 50: return new ItemNoteChs{ID="I41120",Name="預留空位"};
case 51: return new ItemNoteChs{ID="I50010",Name="鲑鱼",Desc="大型猫专用食物，在互动模式与游玩关卡食用，可恢复HP150。"};
case 52: return new ItemNoteChs{ID="I50020",Name="带骨肉",Desc="大型猫专用食物，在互动模式与游玩关卡食用，可恢复HP300。"};
case 53: return new ItemNoteChs{ID="I50030",Name="烤乳猪",Desc="大型猫专用食物，在互动模式与游玩关卡食用，可恢复HP1000并增加EXP500。"};
case 54: return new ItemNoteChs{ID="I60010",Name="南瓜",Desc="猞猁体型专用玩具，在互动模式使用，可增加经验值100。"};
case 55: return new ItemNoteChs{ID="I60020",Name="木人桩",Desc="猞猁体型专用玩具，在互动模式使用，可增加经验值300。"};
case 56: return new ItemNoteChs{ID="I60030",Name="取消中"};
case 57: return new ItemNoteChs{ID="I62010",Name="雪橇",Desc="猞猁体型专用道具，在探索模式使用，可捕捉到猞猁。"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}