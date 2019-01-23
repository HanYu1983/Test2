using UnityEngine;
namespace Remix{
	public class TutorialNoteCht{
		public const int ID_COUNT = 54;

		public string ID {get; set;}
public string Page {get; set;}
public string Desc {get; set;}

		public static TutorialNoteCht Get(string key){
			switch (key) {
			case "TP01T01": return new TutorialNoteCht{ID="TP01T01",Page="TP01",Desc="節奏遊戲"};
case "TP01T02": return new TutorialNoteCht{ID="TP01T02",Page="TP01",Desc="題示點"};
case "TP01T03": return new TutorialNoteCht{ID="TP01T03",Page="TP01",Desc="上滑與下滑"};
case "TP01T04": return new TutorialNoteCht{ID="TP01T04",Page="TP01",Desc="連打"};
case "TP01T05": return new TutorialNoteCht{ID="TP01T05",Page="TP01",Desc="搓貓掌"};
case "TP01T06": return new TutorialNoteCht{ID="TP01T06",Page="TP01",Desc="點擊貓掌"};
case "TP01T07": return new TutorialNoteCht{ID="TP01T07",Page="TP01",Desc="上滑與下滑貓掌"};
case "TP01T08": return new TutorialNoteCht{ID="TP01T08",Page="TP01",Desc="連打貓掌"};
case "TP01T09": return new TutorialNoteCht{ID="TP01T09",Page="TP01",Desc="狂搓貓掌"};
case "TP02T01": return new TutorialNoteCht{ID="TP02T01",Page="TP02",Desc="互動模式"};
case "TP02T02": return new TutorialNoteCht{ID="TP02T02",Page="TP02",Desc="互動"};
case "TP02T03": return new TutorialNoteCht{ID="TP02T03",Page="TP02",Desc="撫摸"};
case "TP02T04": return new TutorialNoteCht{ID="TP02T04",Page="TP02",Desc="生氣"};
case "TP02T05": return new TutorialNoteCht{ID="TP02T05",Page="TP02",Desc="加速"};
case "TP02T06": return new TutorialNoteCht{ID="TP02T06",Page="TP02",Desc="打開互動模式視窗"};
case "TP02T07": return new TutorialNoteCht{ID="TP02T07",Page="TP02",Desc="撫摸貓增加EXP"};
case "TP02T08": return new TutorialNoteCht{ID="TP02T08",Page="TP02",Desc="摸太多貓會生氣"};
case "TP02T09": return new TutorialNoteCht{ID="TP02T09",Page="TP02",Desc="使用貓草讓牠冷靜"};
case "TP03T01": return new TutorialNoteCht{ID="TP03T01",Page="TP03",Desc="互動模式"};
case "TP03T02": return new TutorialNoteCht{ID="TP03T02",Page="TP03",Desc="餓"};
case "TP03T03": return new TutorialNoteCht{ID="TP03T03",Page="TP03",Desc="睡"};
case "TP03T04": return new TutorialNoteCht{ID="TP03T04",Page="TP03",Desc="興奮"};
case "TP03T05": return new TutorialNoteCht{ID="TP03T05",Page="TP03",Desc="體型"};
case "TP03T06": return new TutorialNoteCht{ID="TP03T06",Page="TP03",Desc="餵貓增加HP"};
case "TP03T07": return new TutorialNoteCht{ID="TP03T07",Page="TP03",Desc="撫摸貓叫牠起床"};
case "TP03T08": return new TutorialNoteCht{ID="TP03T08",Page="TP03",Desc="玩貓玩具增加EXP"};
case "TP03T09": return new TutorialNoteCht{ID="TP03T09",Page="TP03",Desc="專用的貓玩具"};
case "TP04T01": return new TutorialNoteCht{ID="TP04T01",Page="TP04",Desc="探索模式"};
case "TP04T02": return new TutorialNoteCht{ID="TP04T02",Page="TP04",Desc="貓咪遊樂場"};
case "TP04T03": return new TutorialNoteCht{ID="TP04T03",Page="TP04",Desc="等待"};
case "TP04T04": return new TutorialNoteCht{ID="TP04T04",Page="TP04",Desc="加速"};
case "TP04T05": return new TutorialNoteCht{ID="TP04T05",Page="TP04",Desc="體型"};
case "TP04T06": return new TutorialNoteCht{ID="TP04T06",Page="TP04",Desc="使用捕貓道具"};
case "TP04T07": return new TutorialNoteCht{ID="TP04T07",Page="TP04",Desc="得到新的貓"};
case "TP04T08": return new TutorialNoteCht{ID="TP04T08",Page="TP04",Desc="使用貓草來節省時間"};
case "TP04T09": return new TutorialNoteCht{ID="TP04T09",Page="TP04",Desc="專用的捕貓道具"};
case "TP05T01": return new TutorialNoteCht{ID="TP05T01",Page="TP05",Desc="探索模式"};
case "TP05T02": return new TutorialNoteCht{ID="TP05T02",Page="TP05",Desc="棲息地"};
case "TP05T03": return new TutorialNoteCht{ID="TP05T03",Page="TP05",Desc="猴硐"};
case "TP05T04": return new TutorialNoteCht{ID="TP05T04",Page="TP05",Desc="座間味"};
case "TP05T05": return new TutorialNoteCht{ID="TP05T05",Page="TP05",Desc="渡嘉敷"};
case "TP05T06": return new TutorialNoteCht{ID="TP05T06",Page="TP05",Desc="南島地圖"};
case "TP05T07": return new TutorialNoteCht{ID="TP05T07",Page="TP05"};
case "TP05T08": return new TutorialNoteCht{ID="TP05T08",Page="TP05"};
case "TP05T09": return new TutorialNoteCht{ID="TP05T09",Page="TP05"};
case "TP06T01": return new TutorialNoteCht{ID="TP06T01",Page="TP06",Desc="探索模式"};
case "TP06T02": return new TutorialNoteCht{ID="TP06T02",Page="TP06",Desc="貓咪遊樂場"};
case "TP06T03": return new TutorialNoteCht{ID="TP06T03",Page="TP06",Desc="等待"};
case "TP06T04": return new TutorialNoteCht{ID="TP06T04",Page="TP06",Desc="加速"};
case "TP06T05": return new TutorialNoteCht{ID="TP06T05",Page="TP06",Desc="相機"};
case "TP06T06": return new TutorialNoteCht{ID="TP06T06",Page="TP06",Desc="使用相機"};
case "TP06T07": return new TutorialNoteCht{ID="TP06T07",Page="TP06",Desc="得到新照片"};
case "TP06T08": return new TutorialNoteCht{ID="TP06T08",Page="TP06",Desc="使用三腳架來節省時間"};
case "TP06T09": return new TutorialNoteCht{ID="TP06T09",Page="TP06",Desc="高性能相機"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static TutorialNoteCht Get(int key){
			switch (key) {
			case 0: return new TutorialNoteCht{ID="TP01T01",Page="TP01",Desc="節奏遊戲"};
case 1: return new TutorialNoteCht{ID="TP01T02",Page="TP01",Desc="題示點"};
case 2: return new TutorialNoteCht{ID="TP01T03",Page="TP01",Desc="上滑與下滑"};
case 3: return new TutorialNoteCht{ID="TP01T04",Page="TP01",Desc="連打"};
case 4: return new TutorialNoteCht{ID="TP01T05",Page="TP01",Desc="搓貓掌"};
case 5: return new TutorialNoteCht{ID="TP01T06",Page="TP01",Desc="點擊貓掌"};
case 6: return new TutorialNoteCht{ID="TP01T07",Page="TP01",Desc="上滑與下滑貓掌"};
case 7: return new TutorialNoteCht{ID="TP01T08",Page="TP01",Desc="連打貓掌"};
case 8: return new TutorialNoteCht{ID="TP01T09",Page="TP01",Desc="狂搓貓掌"};
case 9: return new TutorialNoteCht{ID="TP02T01",Page="TP02",Desc="互動模式"};
case 10: return new TutorialNoteCht{ID="TP02T02",Page="TP02",Desc="互動"};
case 11: return new TutorialNoteCht{ID="TP02T03",Page="TP02",Desc="撫摸"};
case 12: return new TutorialNoteCht{ID="TP02T04",Page="TP02",Desc="生氣"};
case 13: return new TutorialNoteCht{ID="TP02T05",Page="TP02",Desc="加速"};
case 14: return new TutorialNoteCht{ID="TP02T06",Page="TP02",Desc="打開互動模式視窗"};
case 15: return new TutorialNoteCht{ID="TP02T07",Page="TP02",Desc="撫摸貓增加EXP"};
case 16: return new TutorialNoteCht{ID="TP02T08",Page="TP02",Desc="摸太多貓會生氣"};
case 17: return new TutorialNoteCht{ID="TP02T09",Page="TP02",Desc="使用貓草讓牠冷靜"};
case 18: return new TutorialNoteCht{ID="TP03T01",Page="TP03",Desc="互動模式"};
case 19: return new TutorialNoteCht{ID="TP03T02",Page="TP03",Desc="餓"};
case 20: return new TutorialNoteCht{ID="TP03T03",Page="TP03",Desc="睡"};
case 21: return new TutorialNoteCht{ID="TP03T04",Page="TP03",Desc="興奮"};
case 22: return new TutorialNoteCht{ID="TP03T05",Page="TP03",Desc="體型"};
case 23: return new TutorialNoteCht{ID="TP03T06",Page="TP03",Desc="餵貓增加HP"};
case 24: return new TutorialNoteCht{ID="TP03T07",Page="TP03",Desc="撫摸貓叫牠起床"};
case 25: return new TutorialNoteCht{ID="TP03T08",Page="TP03",Desc="玩貓玩具增加EXP"};
case 26: return new TutorialNoteCht{ID="TP03T09",Page="TP03",Desc="專用的貓玩具"};
case 27: return new TutorialNoteCht{ID="TP04T01",Page="TP04",Desc="探索模式"};
case 28: return new TutorialNoteCht{ID="TP04T02",Page="TP04",Desc="貓咪遊樂場"};
case 29: return new TutorialNoteCht{ID="TP04T03",Page="TP04",Desc="等待"};
case 30: return new TutorialNoteCht{ID="TP04T04",Page="TP04",Desc="加速"};
case 31: return new TutorialNoteCht{ID="TP04T05",Page="TP04",Desc="體型"};
case 32: return new TutorialNoteCht{ID="TP04T06",Page="TP04",Desc="使用捕貓道具"};
case 33: return new TutorialNoteCht{ID="TP04T07",Page="TP04",Desc="得到新的貓"};
case 34: return new TutorialNoteCht{ID="TP04T08",Page="TP04",Desc="使用貓草來節省時間"};
case 35: return new TutorialNoteCht{ID="TP04T09",Page="TP04",Desc="專用的捕貓道具"};
case 36: return new TutorialNoteCht{ID="TP05T01",Page="TP05",Desc="探索模式"};
case 37: return new TutorialNoteCht{ID="TP05T02",Page="TP05",Desc="棲息地"};
case 38: return new TutorialNoteCht{ID="TP05T03",Page="TP05",Desc="猴硐"};
case 39: return new TutorialNoteCht{ID="TP05T04",Page="TP05",Desc="座間味"};
case 40: return new TutorialNoteCht{ID="TP05T05",Page="TP05",Desc="渡嘉敷"};
case 41: return new TutorialNoteCht{ID="TP05T06",Page="TP05",Desc="南島地圖"};
case 42: return new TutorialNoteCht{ID="TP05T07",Page="TP05"};
case 43: return new TutorialNoteCht{ID="TP05T08",Page="TP05"};
case 44: return new TutorialNoteCht{ID="TP05T09",Page="TP05"};
case 45: return new TutorialNoteCht{ID="TP06T01",Page="TP06",Desc="探索模式"};
case 46: return new TutorialNoteCht{ID="TP06T02",Page="TP06",Desc="貓咪遊樂場"};
case 47: return new TutorialNoteCht{ID="TP06T03",Page="TP06",Desc="等待"};
case 48: return new TutorialNoteCht{ID="TP06T04",Page="TP06",Desc="加速"};
case 49: return new TutorialNoteCht{ID="TP06T05",Page="TP06",Desc="相機"};
case 50: return new TutorialNoteCht{ID="TP06T06",Page="TP06",Desc="使用相機"};
case 51: return new TutorialNoteCht{ID="TP06T07",Page="TP06",Desc="得到新照片"};
case 52: return new TutorialNoteCht{ID="TP06T08",Page="TP06",Desc="使用三腳架來節省時間"};
case 53: return new TutorialNoteCht{ID="TP06T09",Page="TP06",Desc="高性能相機"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}