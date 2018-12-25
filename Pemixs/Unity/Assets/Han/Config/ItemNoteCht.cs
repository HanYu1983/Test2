using UnityEngine;
namespace Remix{
	public class ItemNoteCht{
		public const int ID_COUNT = 58;

		public string ID {get; set;}
public string Name {get; set;}
public string Desc {get; set;}

		public static ItemNoteCht Get(string key){
			switch (key) {
			case "I10010": return new ItemNoteCht{ID="I10010",Name="貓罐",Desc="在互動模式與遊玩關卡食用，可恢復HP50。"};
case "I10020": return new ItemNoteCht{ID="I10020",Name="牛奶",Desc="在互動模式與遊玩關卡食用，可恢復HP150。"};
case "I10030": return new ItemNoteCht{ID="I10030",Name="乾乾",Desc="在互動模式與遊玩關卡食用，可恢復HP300。"};
case "I10040": return new ItemNoteCht{ID="I10040",Name="貓草",Desc="在互動模式與遊玩關卡食用，可恢復HP500。"};
case "I10050": return new ItemNoteCht{ID="I10050",Name="小魚乾",Desc="在互動模式與遊玩關卡食用，可恢復HP250。"};
case "I10060": return new ItemNoteCht{ID="I10060",Name="柴魚",Desc="在互動模式與遊玩關卡食用，可恢復HP500。"};
case "I10070": return new ItemNoteCht{ID="I10070",Name="鮮魚",Desc="在互動模式與遊玩關卡食用，可恢復HP500並增加EXP500。"};
case "I10080": return new ItemNoteCht{ID="I10080",Name="高級餐包",Desc="在互動模式與遊玩關卡食用，可恢復HP1000並增加EXP1000。"};
case "I20010": return new ItemNoteCht{ID="I20010",Name="數位相機",Desc="在探索模式使用，可獲得一張一般照片。"};
case "I20020": return new ItemNoteCht{ID="I20020",Name="拍立得相機",Desc="在探索模式使用，可獲得一張一般照片，並減少等待時間50%。"};
case "I20030": return new ItemNoteCht{ID="I20030",Name="運動相機",Desc="在探索模式使用，可獲得一張一般照片，並減少等待時間100%。"};
case "I20040": return new ItemNoteCht{ID="I20040",Name="單眼相機",Desc="在探索模式使用，可獲得一張一般照片，並有20%機率獲得環景照片。"};
case "I20050": return new ItemNoteCht{ID="I20050",Name="數位單眼相機",Desc="在探索模式使用，可獲得一張一般照片，並有40%機率獲得環景照片。"};
case "I20060": return new ItemNoteCht{ID="I20060",Name="蛇腹相機",Desc="在探索模式使用，可獲得一張一般照片，並有50%機率獲得第二張照片。"};
case "I20070": return new ItemNoteCht{ID="I20070",Name="雙眼相機",Desc="在探索模式使用，可獲得二張一般照片。"};
case "I20080": return new ItemNoteCht{ID="I20080",Name="120相機",Desc="在探索模式使用，可獲得二張一般照片，並有40%機率獲得環景照片。"};
case "I21010": return new ItemNoteCht{ID="I21010",Name="自拍棒",Desc="在探索模式使用，可減少拍攝等待時間50%"};
case "I21020": return new ItemNoteCht{ID="I21020",Name="相機腳架",Desc="在探索模式使用，可減少拍攝等待時間100%。"};
case "I30010": return new ItemNoteCht{ID="I30010",Name="雷射光筆",Desc="成貓體型專用玩具，在互動模式使用，可增加經驗值50。"};
case "I30020": return new ItemNoteCht{ID="I30020",Name="玩具老鼠",Desc="成貓體型專用玩具，在互動模式使用，可增加經驗值200。"};
case "I30030": return new ItemNoteCht{ID="I30030",Name="取消中"};
case "I30040": return new ItemNoteCht{ID="I30040",Name="逗貓棒",Desc="幼貓體型專用玩具，在互動模式使用，增加經驗值50。"};
case "I30050": return new ItemNoteCht{ID="I30050",Name="毛線球",Desc="幼貓體型專用玩具，在互動模式使用，增加經驗值200。"};
case "I30060": return new ItemNoteCht{ID="I30060",Name="取消中"};
case "I30070": return new ItemNoteCht{ID="I30070",Name="貓抓板",Desc="胖貓體型專用玩具，在互動模式使用，增加經驗值50。"};
case "I30080": return new ItemNoteCht{ID="I30080",Name="暖爐",Desc="胖貓體型專用玩具，在互動模式使用，增加經驗值200。"};
case "I30090": return new ItemNoteCht{ID="I30090",Name="取消中"};
case "I30100": return new ItemNoteCht{ID="I30100",Name="取消中"};
case "I30110": return new ItemNoteCht{ID="I30110",Name="取消中"};
case "I31010": return new ItemNoteCht{ID="I31010",Name="貓草噴劑",Desc="在探索模式與互動模式使用，可減少等待時間50%。"};
case "I31020": return new ItemNoteCht{ID="I31020",Name="罐裝乾貓草",Desc="在探索模式與互動模式使用，可減少等待時間100%。"};
case "I32010": return new ItemNoteCht{ID="I32010",Name="宅配紙箱",Desc="成貓體型專用道具，在探索模式使用，可捕捉到成貓。"};
case "I32020": return new ItemNoteCht{ID="I32020",Name="掃地機器人",Desc="幼貓體型專用道具，在探索模式使用，可捕捉到幼貓。"};
case "I32030": return new ItemNoteCht{ID="I32030",Name="沙發床",Desc="胖貓體型專用道具，在探索模式使用，可捕捉到胖貓。"};
case "I40010": return new ItemNoteCht{ID="I40010",Name="貓金幣 250",Desc="獲得貓金幣 250"};
case "I40020": return new ItemNoteCht{ID="I40020",Name="貓金幣 500",Desc="獲得貓金幣 500"};
case "I40030": return new ItemNoteCht{ID="I40030",Name="貓金幣 750",Desc="獲得貓金幣 750"};
case "I40040": return new ItemNoteCht{ID="I40040",Name="貓金幣 1000",Desc="獲得貓金幣 1000"};
case "I40050": return new ItemNoteCht{ID="I40050",Name="貓金幣 1500",Desc="獲得貓金幣 1500"};
case "I41010": return new ItemNoteCht{ID="I41010",Name="貓銀票 20",Desc="獲得貓銀票 20"};
case "I41020": return new ItemNoteCht{ID="I41020",Name="貓銀票 40",Desc="獲得貓銀票 40"};
case "I41030": return new ItemNoteCht{ID="I41030",Name="貓銀票 10",Desc="獲得貓銀票 10"};
case "I41040": return new ItemNoteCht{ID="I41040",Name="貓銀票 5",Desc="獲得貓銀票 5"};
case "I41050": return new ItemNoteCht{ID="I41050",Name="貓銀票 2",Desc="獲得貓銀票 2"};
case "I41060": return new ItemNoteCht{ID="I41060",Name="貓銀票 1",Desc="獲得貓銀票 1"};
case "I41070": return new ItemNoteCht{ID="I41070",Name="預留空位"};
case "I41080": return new ItemNoteCht{ID="I41080",Name="預留空位"};
case "I41090": return new ItemNoteCht{ID="I41090",Name="預留空位"};
case "I41100": return new ItemNoteCht{ID="I41100",Name="預留空位"};
case "I41110": return new ItemNoteCht{ID="I41110",Name="預留空位"};
case "I41120": return new ItemNoteCht{ID="I41120",Name="預留空位"};
case "I50010": return new ItemNoteCht{ID="I50010",Name="鮭魚",Desc="大型貓專用食物，在互動模式與遊玩關卡食用，可恢復HP150。"};
case "I50020": return new ItemNoteCht{ID="I50020",Name="帶骨肉",Desc="大型貓專用食物，在互動模式與遊玩關卡食用，可恢復HP300。"};
case "I50030": return new ItemNoteCht{ID="I50030",Name="烤乳豬",Desc="大型貓專用食物，在互動模式與遊玩關卡食用，可恢復HP1000並增加EXP500。"};
case "I60010": return new ItemNoteCht{ID="I60010",Name="南瓜",Desc="猞猁體型專用玩具，在互動模式使用，可增加經驗值100。"};
case "I60020": return new ItemNoteCht{ID="I60020",Name="木人樁",Desc="猞猁體型專用玩具，在互動模式使用，可增加經驗值300。"};
case "I60030": return new ItemNoteCht{ID="I60030",Name="取消中"};
case "I62010": return new ItemNoteCht{ID="I62010",Name="雪橇",Desc="猞猁體型專用道具，在探索模式使用，可捕捉到猞猁。"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static ItemNoteCht Get(int key){
			switch (key) {
			case 0: return new ItemNoteCht{ID="I10010",Name="貓罐",Desc="在互動模式與遊玩關卡食用，可恢復HP50。"};
case 1: return new ItemNoteCht{ID="I10020",Name="牛奶",Desc="在互動模式與遊玩關卡食用，可恢復HP150。"};
case 2: return new ItemNoteCht{ID="I10030",Name="乾乾",Desc="在互動模式與遊玩關卡食用，可恢復HP300。"};
case 3: return new ItemNoteCht{ID="I10040",Name="貓草",Desc="在互動模式與遊玩關卡食用，可恢復HP500。"};
case 4: return new ItemNoteCht{ID="I10050",Name="小魚乾",Desc="在互動模式與遊玩關卡食用，可恢復HP250。"};
case 5: return new ItemNoteCht{ID="I10060",Name="柴魚",Desc="在互動模式與遊玩關卡食用，可恢復HP500。"};
case 6: return new ItemNoteCht{ID="I10070",Name="鮮魚",Desc="在互動模式與遊玩關卡食用，可恢復HP500並增加EXP500。"};
case 7: return new ItemNoteCht{ID="I10080",Name="高級餐包",Desc="在互動模式與遊玩關卡食用，可恢復HP1000並增加EXP1000。"};
case 8: return new ItemNoteCht{ID="I20010",Name="數位相機",Desc="在探索模式使用，可獲得一張一般照片。"};
case 9: return new ItemNoteCht{ID="I20020",Name="拍立得相機",Desc="在探索模式使用，可獲得一張一般照片，並減少等待時間50%。"};
case 10: return new ItemNoteCht{ID="I20030",Name="運動相機",Desc="在探索模式使用，可獲得一張一般照片，並減少等待時間100%。"};
case 11: return new ItemNoteCht{ID="I20040",Name="單眼相機",Desc="在探索模式使用，可獲得一張一般照片，並有20%機率獲得環景照片。"};
case 12: return new ItemNoteCht{ID="I20050",Name="數位單眼相機",Desc="在探索模式使用，可獲得一張一般照片，並有40%機率獲得環景照片。"};
case 13: return new ItemNoteCht{ID="I20060",Name="蛇腹相機",Desc="在探索模式使用，可獲得一張一般照片，並有50%機率獲得第二張照片。"};
case 14: return new ItemNoteCht{ID="I20070",Name="雙眼相機",Desc="在探索模式使用，可獲得二張一般照片。"};
case 15: return new ItemNoteCht{ID="I20080",Name="120相機",Desc="在探索模式使用，可獲得二張一般照片，並有40%機率獲得環景照片。"};
case 16: return new ItemNoteCht{ID="I21010",Name="自拍棒",Desc="在探索模式使用，可減少拍攝等待時間50%"};
case 17: return new ItemNoteCht{ID="I21020",Name="相機腳架",Desc="在探索模式使用，可減少拍攝等待時間100%。"};
case 18: return new ItemNoteCht{ID="I30010",Name="雷射光筆",Desc="成貓體型專用玩具，在互動模式使用，可增加經驗值50。"};
case 19: return new ItemNoteCht{ID="I30020",Name="玩具老鼠",Desc="成貓體型專用玩具，在互動模式使用，可增加經驗值200。"};
case 20: return new ItemNoteCht{ID="I30030",Name="取消中"};
case 21: return new ItemNoteCht{ID="I30040",Name="逗貓棒",Desc="幼貓體型專用玩具，在互動模式使用，增加經驗值50。"};
case 22: return new ItemNoteCht{ID="I30050",Name="毛線球",Desc="幼貓體型專用玩具，在互動模式使用，增加經驗值200。"};
case 23: return new ItemNoteCht{ID="I30060",Name="取消中"};
case 24: return new ItemNoteCht{ID="I30070",Name="貓抓板",Desc="胖貓體型專用玩具，在互動模式使用，增加經驗值50。"};
case 25: return new ItemNoteCht{ID="I30080",Name="暖爐",Desc="胖貓體型專用玩具，在互動模式使用，增加經驗值200。"};
case 26: return new ItemNoteCht{ID="I30090",Name="取消中"};
case 27: return new ItemNoteCht{ID="I30100",Name="取消中"};
case 28: return new ItemNoteCht{ID="I30110",Name="取消中"};
case 29: return new ItemNoteCht{ID="I31010",Name="貓草噴劑",Desc="在探索模式與互動模式使用，可減少等待時間50%。"};
case 30: return new ItemNoteCht{ID="I31020",Name="罐裝乾貓草",Desc="在探索模式與互動模式使用，可減少等待時間100%。"};
case 31: return new ItemNoteCht{ID="I32010",Name="宅配紙箱",Desc="成貓體型專用道具，在探索模式使用，可捕捉到成貓。"};
case 32: return new ItemNoteCht{ID="I32020",Name="掃地機器人",Desc="幼貓體型專用道具，在探索模式使用，可捕捉到幼貓。"};
case 33: return new ItemNoteCht{ID="I32030",Name="沙發床",Desc="胖貓體型專用道具，在探索模式使用，可捕捉到胖貓。"};
case 34: return new ItemNoteCht{ID="I40010",Name="貓金幣 250",Desc="獲得貓金幣 250"};
case 35: return new ItemNoteCht{ID="I40020",Name="貓金幣 500",Desc="獲得貓金幣 500"};
case 36: return new ItemNoteCht{ID="I40030",Name="貓金幣 750",Desc="獲得貓金幣 750"};
case 37: return new ItemNoteCht{ID="I40040",Name="貓金幣 1000",Desc="獲得貓金幣 1000"};
case 38: return new ItemNoteCht{ID="I40050",Name="貓金幣 1500",Desc="獲得貓金幣 1500"};
case 39: return new ItemNoteCht{ID="I41010",Name="貓銀票 20",Desc="獲得貓銀票 20"};
case 40: return new ItemNoteCht{ID="I41020",Name="貓銀票 40",Desc="獲得貓銀票 40"};
case 41: return new ItemNoteCht{ID="I41030",Name="貓銀票 10",Desc="獲得貓銀票 10"};
case 42: return new ItemNoteCht{ID="I41040",Name="貓銀票 5",Desc="獲得貓銀票 5"};
case 43: return new ItemNoteCht{ID="I41050",Name="貓銀票 2",Desc="獲得貓銀票 2"};
case 44: return new ItemNoteCht{ID="I41060",Name="貓銀票 1",Desc="獲得貓銀票 1"};
case 45: return new ItemNoteCht{ID="I41070",Name="預留空位"};
case 46: return new ItemNoteCht{ID="I41080",Name="預留空位"};
case 47: return new ItemNoteCht{ID="I41090",Name="預留空位"};
case 48: return new ItemNoteCht{ID="I41100",Name="預留空位"};
case 49: return new ItemNoteCht{ID="I41110",Name="預留空位"};
case 50: return new ItemNoteCht{ID="I41120",Name="預留空位"};
case 51: return new ItemNoteCht{ID="I50010",Name="鮭魚",Desc="大型貓專用食物，在互動模式與遊玩關卡食用，可恢復HP150。"};
case 52: return new ItemNoteCht{ID="I50020",Name="帶骨肉",Desc="大型貓專用食物，在互動模式與遊玩關卡食用，可恢復HP300。"};
case 53: return new ItemNoteCht{ID="I50030",Name="烤乳豬",Desc="大型貓專用食物，在互動模式與遊玩關卡食用，可恢復HP1000並增加EXP500。"};
case 54: return new ItemNoteCht{ID="I60010",Name="南瓜",Desc="猞猁體型專用玩具，在互動模式使用，可增加經驗值100。"};
case 55: return new ItemNoteCht{ID="I60020",Name="木人樁",Desc="猞猁體型專用玩具，在互動模式使用，可增加經驗值300。"};
case 56: return new ItemNoteCht{ID="I60030",Name="取消中"};
case 57: return new ItemNoteCht{ID="I62010",Name="雪橇",Desc="猞猁體型專用道具，在探索模式使用，可捕捉到猞猁。"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}