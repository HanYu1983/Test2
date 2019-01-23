using UnityEngine;
namespace Remix{
	public class TutorialNoteChs{
		public const int ID_COUNT = 54;

		public string ID {get; set;}
public string Page {get; set;}
public string Desc {get; set;}

		public static TutorialNoteChs Get(string key){
			switch (key) {
			case "TP01T01": return new TutorialNoteChs{ID="TP01T01",Page="TP01",Desc="节奏游戏"};
case "TP01T02": return new TutorialNoteChs{ID="TP01T02",Page="TP01",Desc="题示点"};
case "TP01T03": return new TutorialNoteChs{ID="TP01T03",Page="TP01",Desc="上滑与下滑"};
case "TP01T04": return new TutorialNoteChs{ID="TP01T04",Page="TP01",Desc="连打"};
case "TP01T05": return new TutorialNoteChs{ID="TP01T05",Page="TP01",Desc="搓猫掌"};
case "TP01T06": return new TutorialNoteChs{ID="TP01T06",Page="TP01",Desc="点击猫掌"};
case "TP01T07": return new TutorialNoteChs{ID="TP01T07",Page="TP01",Desc="上滑与下滑猫掌"};
case "TP01T08": return new TutorialNoteChs{ID="TP01T08",Page="TP01",Desc="连打猫掌"};
case "TP01T09": return new TutorialNoteChs{ID="TP01T09",Page="TP01",Desc="狂搓猫掌"};
case "TP02T01": return new TutorialNoteChs{ID="TP02T01",Page="TP02",Desc="互动模式"};
case "TP02T02": return new TutorialNoteChs{ID="TP02T02",Page="TP02",Desc="互动"};
case "TP02T03": return new TutorialNoteChs{ID="TP02T03",Page="TP02",Desc="抚摸"};
case "TP02T04": return new TutorialNoteChs{ID="TP02T04",Page="TP02",Desc="生气"};
case "TP02T05": return new TutorialNoteChs{ID="TP02T05",Page="TP02",Desc="加速"};
case "TP02T06": return new TutorialNoteChs{ID="TP02T06",Page="TP02",Desc="打开互动模式视窗"};
case "TP02T07": return new TutorialNoteChs{ID="TP02T07",Page="TP02",Desc="抚摸猫增加EXP"};
case "TP02T08": return new TutorialNoteChs{ID="TP02T08",Page="TP02",Desc="摸太多猫会生气"};
case "TP02T09": return new TutorialNoteChs{ID="TP02T09",Page="TP02",Desc="使用猫草让它冷静"};
case "TP03T01": return new TutorialNoteChs{ID="TP03T01",Page="TP03",Desc="互动模式"};
case "TP03T02": return new TutorialNoteChs{ID="TP03T02",Page="TP03",Desc="饿"};
case "TP03T03": return new TutorialNoteChs{ID="TP03T03",Page="TP03",Desc="睡"};
case "TP03T04": return new TutorialNoteChs{ID="TP03T04",Page="TP03",Desc="兴奋"};
case "TP03T05": return new TutorialNoteChs{ID="TP03T05",Page="TP03",Desc="体型"};
case "TP03T06": return new TutorialNoteChs{ID="TP03T06",Page="TP03",Desc="喂猫增加HP"};
case "TP03T07": return new TutorialNoteChs{ID="TP03T07",Page="TP03",Desc="抚摸猫叫它起床"};
case "TP03T08": return new TutorialNoteChs{ID="TP03T08",Page="TP03",Desc="玩猫玩具增加EXP"};
case "TP03T09": return new TutorialNoteChs{ID="TP03T09",Page="TP03",Desc="专用的猫玩具"};
case "TP04T01": return new TutorialNoteChs{ID="TP04T01",Page="TP04",Desc="探索模式"};
case "TP04T02": return new TutorialNoteChs{ID="TP04T02",Page="TP04",Desc="猫咪游乐场"};
case "TP04T03": return new TutorialNoteChs{ID="TP04T03",Page="TP04",Desc="等待"};
case "TP04T04": return new TutorialNoteChs{ID="TP04T04",Page="TP04",Desc="加速"};
case "TP04T05": return new TutorialNoteChs{ID="TP04T05",Page="TP04",Desc="体型"};
case "TP04T06": return new TutorialNoteChs{ID="TP04T06",Page="TP04",Desc="使用捕猫道具"};
case "TP04T07": return new TutorialNoteChs{ID="TP04T07",Page="TP04",Desc="得到新的猫"};
case "TP04T08": return new TutorialNoteChs{ID="TP04T08",Page="TP04",Desc="使用猫草来节省时间"};
case "TP04T09": return new TutorialNoteChs{ID="TP04T09",Page="TP04",Desc="专用的捕猫道具"};
case "TP05T01": return new TutorialNoteChs{ID="TP05T01",Page="TP05",Desc="探索模式"};
case "TP05T02": return new TutorialNoteChs{ID="TP05T02",Page="TP05",Desc="栖息地"};
case "TP05T03": return new TutorialNoteChs{ID="TP05T03",Page="TP05",Desc="猴硐"};
case "TP05T04": return new TutorialNoteChs{ID="TP05T04",Page="TP05",Desc="座间味"};
case "TP05T05": return new TutorialNoteChs{ID="TP05T05",Page="TP05",Desc="渡嘉敷"};
case "TP05T06": return new TutorialNoteChs{ID="TP05T06",Page="TP05",Desc="南岛地图"};
case "TP05T07": return new TutorialNoteChs{ID="TP05T07",Page="TP05"};
case "TP05T08": return new TutorialNoteChs{ID="TP05T08",Page="TP05"};
case "TP05T09": return new TutorialNoteChs{ID="TP05T09",Page="TP05"};
case "TP06T01": return new TutorialNoteChs{ID="TP06T01",Page="TP06",Desc="探索模式"};
case "TP06T02": return new TutorialNoteChs{ID="TP06T02",Page="TP06",Desc="猫咪游乐场"};
case "TP06T03": return new TutorialNoteChs{ID="TP06T03",Page="TP06",Desc="等待"};
case "TP06T04": return new TutorialNoteChs{ID="TP06T04",Page="TP06",Desc="加速"};
case "TP06T05": return new TutorialNoteChs{ID="TP06T05",Page="TP06",Desc="相机"};
case "TP06T06": return new TutorialNoteChs{ID="TP06T06",Page="TP06",Desc="使用相机"};
case "TP06T07": return new TutorialNoteChs{ID="TP06T07",Page="TP06",Desc="得到新照片"};
case "TP06T08": return new TutorialNoteChs{ID="TP06T08",Page="TP06",Desc="使用三脚架来节省时间"};
case "TP06T09": return new TutorialNoteChs{ID="TP06T09",Page="TP06",Desc="高性能相机"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static TutorialNoteChs Get(int key){
			switch (key) {
			case 0: return new TutorialNoteChs{ID="TP01T01",Page="TP01",Desc="节奏游戏"};
case 1: return new TutorialNoteChs{ID="TP01T02",Page="TP01",Desc="题示点"};
case 2: return new TutorialNoteChs{ID="TP01T03",Page="TP01",Desc="上滑与下滑"};
case 3: return new TutorialNoteChs{ID="TP01T04",Page="TP01",Desc="连打"};
case 4: return new TutorialNoteChs{ID="TP01T05",Page="TP01",Desc="搓猫掌"};
case 5: return new TutorialNoteChs{ID="TP01T06",Page="TP01",Desc="点击猫掌"};
case 6: return new TutorialNoteChs{ID="TP01T07",Page="TP01",Desc="上滑与下滑猫掌"};
case 7: return new TutorialNoteChs{ID="TP01T08",Page="TP01",Desc="连打猫掌"};
case 8: return new TutorialNoteChs{ID="TP01T09",Page="TP01",Desc="狂搓猫掌"};
case 9: return new TutorialNoteChs{ID="TP02T01",Page="TP02",Desc="互动模式"};
case 10: return new TutorialNoteChs{ID="TP02T02",Page="TP02",Desc="互动"};
case 11: return new TutorialNoteChs{ID="TP02T03",Page="TP02",Desc="抚摸"};
case 12: return new TutorialNoteChs{ID="TP02T04",Page="TP02",Desc="生气"};
case 13: return new TutorialNoteChs{ID="TP02T05",Page="TP02",Desc="加速"};
case 14: return new TutorialNoteChs{ID="TP02T06",Page="TP02",Desc="打开互动模式视窗"};
case 15: return new TutorialNoteChs{ID="TP02T07",Page="TP02",Desc="抚摸猫增加EXP"};
case 16: return new TutorialNoteChs{ID="TP02T08",Page="TP02",Desc="摸太多猫会生气"};
case 17: return new TutorialNoteChs{ID="TP02T09",Page="TP02",Desc="使用猫草让它冷静"};
case 18: return new TutorialNoteChs{ID="TP03T01",Page="TP03",Desc="互动模式"};
case 19: return new TutorialNoteChs{ID="TP03T02",Page="TP03",Desc="饿"};
case 20: return new TutorialNoteChs{ID="TP03T03",Page="TP03",Desc="睡"};
case 21: return new TutorialNoteChs{ID="TP03T04",Page="TP03",Desc="兴奋"};
case 22: return new TutorialNoteChs{ID="TP03T05",Page="TP03",Desc="体型"};
case 23: return new TutorialNoteChs{ID="TP03T06",Page="TP03",Desc="喂猫增加HP"};
case 24: return new TutorialNoteChs{ID="TP03T07",Page="TP03",Desc="抚摸猫叫它起床"};
case 25: return new TutorialNoteChs{ID="TP03T08",Page="TP03",Desc="玩猫玩具增加EXP"};
case 26: return new TutorialNoteChs{ID="TP03T09",Page="TP03",Desc="专用的猫玩具"};
case 27: return new TutorialNoteChs{ID="TP04T01",Page="TP04",Desc="探索模式"};
case 28: return new TutorialNoteChs{ID="TP04T02",Page="TP04",Desc="猫咪游乐场"};
case 29: return new TutorialNoteChs{ID="TP04T03",Page="TP04",Desc="等待"};
case 30: return new TutorialNoteChs{ID="TP04T04",Page="TP04",Desc="加速"};
case 31: return new TutorialNoteChs{ID="TP04T05",Page="TP04",Desc="体型"};
case 32: return new TutorialNoteChs{ID="TP04T06",Page="TP04",Desc="使用捕猫道具"};
case 33: return new TutorialNoteChs{ID="TP04T07",Page="TP04",Desc="得到新的猫"};
case 34: return new TutorialNoteChs{ID="TP04T08",Page="TP04",Desc="使用猫草来节省时间"};
case 35: return new TutorialNoteChs{ID="TP04T09",Page="TP04",Desc="专用的捕猫道具"};
case 36: return new TutorialNoteChs{ID="TP05T01",Page="TP05",Desc="探索模式"};
case 37: return new TutorialNoteChs{ID="TP05T02",Page="TP05",Desc="栖息地"};
case 38: return new TutorialNoteChs{ID="TP05T03",Page="TP05",Desc="猴硐"};
case 39: return new TutorialNoteChs{ID="TP05T04",Page="TP05",Desc="座间味"};
case 40: return new TutorialNoteChs{ID="TP05T05",Page="TP05",Desc="渡嘉敷"};
case 41: return new TutorialNoteChs{ID="TP05T06",Page="TP05",Desc="南岛地图"};
case 42: return new TutorialNoteChs{ID="TP05T07",Page="TP05"};
case 43: return new TutorialNoteChs{ID="TP05T08",Page="TP05"};
case 44: return new TutorialNoteChs{ID="TP05T09",Page="TP05"};
case 45: return new TutorialNoteChs{ID="TP06T01",Page="TP06",Desc="探索模式"};
case 46: return new TutorialNoteChs{ID="TP06T02",Page="TP06",Desc="猫咪游乐场"};
case 47: return new TutorialNoteChs{ID="TP06T03",Page="TP06",Desc="等待"};
case 48: return new TutorialNoteChs{ID="TP06T04",Page="TP06",Desc="加速"};
case 49: return new TutorialNoteChs{ID="TP06T05",Page="TP06",Desc="相机"};
case 50: return new TutorialNoteChs{ID="TP06T06",Page="TP06",Desc="使用相机"};
case 51: return new TutorialNoteChs{ID="TP06T07",Page="TP06",Desc="得到新照片"};
case 52: return new TutorialNoteChs{ID="TP06T08",Page="TP06",Desc="使用三脚架来节省时间"};
case 53: return new TutorialNoteChs{ID="TP06T09",Page="TP06",Desc="高性能相机"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}