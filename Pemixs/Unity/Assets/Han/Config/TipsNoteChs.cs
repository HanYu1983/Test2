using UnityEngine;
namespace Remix{
	public class TipsNoteChs{
		public const int ID_COUNT = 25;

		public string ID {get; set;}
public string Title {get; set;}
public string Ch {get; set;}
public string Desc {get; set;}

		public static TipsNoteChs Get(string key){
			switch (key) {
			case "Tips01": return new TipsNoteChs{ID="Tips01",Title="秘诀 01",Ch="CI01",Desc="每日可观看奖励广告8次，每次可获得1张猫银票。"};
case "Tips02": return new TipsNoteChs{ID="Tips02",Title="秘诀 02",Ch="CI02",Desc="通关一个地图的全部Easy关卡，可获得2张猫银票。"};
case "Tips03": return new TipsNoteChs{ID="Tips03",Title="秘诀 03",Ch="CI02",Desc="通关一个地图的全部Normal关卡，可获得5张猫银票。"};
case "Tips04": return new TipsNoteChs{ID="Tips04",Title="秘诀 04",Ch="CI02",Desc="通关一个地图的全部Hard关卡，可获得10张猫银票。"};
case "Tips05": return new TipsNoteChs{ID="Tips05",Title="秘诀 05",Ch="CI03",Desc="携带到关卡中的食物道具，在HP为0时会自动使用。"};
case "Tips06": return new TipsNoteChs{ID="Tips06",Title="秘诀 06",Ch="CI03",Desc="携带到关卡中的食物道具，若无使用会自动回到道具仓库。"};
case "Tips07": return new TipsNoteChs{ID="Tips07",Title="秘诀 07",Ch="CI04",Desc="使用同一只猫咪连续玩太多关卡，猫咪会睡着。"};
case "Tips08": return new TipsNoteChs{ID="Tips08",Title="秘诀 08",Ch="CI04",Desc="猫咪睡着时，可抚摸把猫咪叫醒。"};
case "Tips09": return new TipsNoteChs{ID="Tips09",Title="秘诀 09",Ch="CI05",Desc="不同体型的猫擅长不同模式的游戏关卡。"};
case "Tips10": return new TipsNoteChs{ID="Tips10",Title="秘诀 10",Ch="CI05",Desc="每一关卡可获得的道具与机率，可到猫唱片行查询。"};
case "Tips11": return new TipsNoteChs{ID="Tips11",Title="秘诀 11",Ch="CI06",Desc="点击猫咪头像可直接开启互动视窗。"};
case "Tips12": return new TipsNoteChs{ID="Tips12",Title="秘诀 12",Ch="CI07",Desc="抚摸猫咪可增加EXP。"};
case "Tips13": return new TipsNoteChs{ID="Tips13",Title="秘诀 13",Ch="CI07",Desc="猫咪生气时，可使用猫草让猫咪冷静下来。"};
case "Tips14": return new TipsNoteChs{ID="Tips14",Title="秘诀 14",Ch="CI08",Desc="猫咪兴奋时，跟他玩玩具可增加EXP。"};
case "Tips15": return new TipsNoteChs{ID="Tips15",Title="秘诀 15",Ch="CI08",Desc="不同体型的猫咪，喜欢的玩具不一样。"};
case "Tips16": return new TipsNoteChs{ID="Tips16",Title="秘诀 16",Ch="CI09",Desc="猫咪的HP降低时，需要吃食物回复。"};
case "Tips17": return new TipsNoteChs{ID="Tips17",Title="秘诀 17",Ch="CI10",Desc="若捕捉到重覆的猫咪会直接增加该猫咪的EXP500。"};
case "Tips18": return new TipsNoteChs{ID="Tips18",Title="秘诀 18",Ch="CI10",Desc="要捕捉到猫，必须要在猫商店购买该体型的捕捉专用道具。"};
case "Tips19": return new TipsNoteChs{ID="Tips19",Title="秘诀 19",Ch="CI10",Desc="每只猫有不同的栖息地，可到游戏教学查询。"};
case "Tips20": return new TipsNoteChs{ID="Tips20",Title="秘诀 20",Ch="CI11",Desc="在猫咪游乐场使用相机，可获得该地图的相片。"};
case "Tips21": return new TipsNoteChs{ID="Tips21",Title="秘诀 21",Ch="CI11",Desc="获得一般相片时，会自动获得1张猫银票。"};
case "Tips22": return new TipsNoteChs{ID="Tips22",Title="秘诀 22",Ch="CI11",Desc="获得环景相片时，会自动获得5张猫银票。"};
case "Tips23": return new TipsNoteChs{ID="Tips23",Title="秘诀 23",Ch="CI12",Desc="记得到首页的邮箱领取每日的奖励。"};
case "Tips24": return new TipsNoteChs{ID="Tips24",Title="秘诀 24",Ch="CI12",Desc="每日的奖励会在每月1日重新计算。"};
case "Tips25": return new TipsNoteChs{ID="Tips25",Title="秘诀 25",Ch="CI12",Desc="招待每一位好友可获得5张猫银票。"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static TipsNoteChs Get(int key){
			switch (key) {
			case 0: return new TipsNoteChs{ID="Tips01",Title="秘诀 01",Ch="CI01",Desc="每日可观看奖励广告8次，每次可获得1张猫银票。"};
case 1: return new TipsNoteChs{ID="Tips02",Title="秘诀 02",Ch="CI02",Desc="通关一个地图的全部Easy关卡，可获得2张猫银票。"};
case 2: return new TipsNoteChs{ID="Tips03",Title="秘诀 03",Ch="CI02",Desc="通关一个地图的全部Normal关卡，可获得5张猫银票。"};
case 3: return new TipsNoteChs{ID="Tips04",Title="秘诀 04",Ch="CI02",Desc="通关一个地图的全部Hard关卡，可获得10张猫银票。"};
case 4: return new TipsNoteChs{ID="Tips05",Title="秘诀 05",Ch="CI03",Desc="携带到关卡中的食物道具，在HP为0时会自动使用。"};
case 5: return new TipsNoteChs{ID="Tips06",Title="秘诀 06",Ch="CI03",Desc="携带到关卡中的食物道具，若无使用会自动回到道具仓库。"};
case 6: return new TipsNoteChs{ID="Tips07",Title="秘诀 07",Ch="CI04",Desc="使用同一只猫咪连续玩太多关卡，猫咪会睡着。"};
case 7: return new TipsNoteChs{ID="Tips08",Title="秘诀 08",Ch="CI04",Desc="猫咪睡着时，可抚摸把猫咪叫醒。"};
case 8: return new TipsNoteChs{ID="Tips09",Title="秘诀 09",Ch="CI05",Desc="不同体型的猫擅长不同模式的游戏关卡。"};
case 9: return new TipsNoteChs{ID="Tips10",Title="秘诀 10",Ch="CI05",Desc="每一关卡可获得的道具与机率，可到猫唱片行查询。"};
case 10: return new TipsNoteChs{ID="Tips11",Title="秘诀 11",Ch="CI06",Desc="点击猫咪头像可直接开启互动视窗。"};
case 11: return new TipsNoteChs{ID="Tips12",Title="秘诀 12",Ch="CI07",Desc="抚摸猫咪可增加EXP。"};
case 12: return new TipsNoteChs{ID="Tips13",Title="秘诀 13",Ch="CI07",Desc="猫咪生气时，可使用猫草让猫咪冷静下来。"};
case 13: return new TipsNoteChs{ID="Tips14",Title="秘诀 14",Ch="CI08",Desc="猫咪兴奋时，跟他玩玩具可增加EXP。"};
case 14: return new TipsNoteChs{ID="Tips15",Title="秘诀 15",Ch="CI08",Desc="不同体型的猫咪，喜欢的玩具不一样。"};
case 15: return new TipsNoteChs{ID="Tips16",Title="秘诀 16",Ch="CI09",Desc="猫咪的HP降低时，需要吃食物回复。"};
case 16: return new TipsNoteChs{ID="Tips17",Title="秘诀 17",Ch="CI10",Desc="若捕捉到重覆的猫咪会直接增加该猫咪的EXP500。"};
case 17: return new TipsNoteChs{ID="Tips18",Title="秘诀 18",Ch="CI10",Desc="要捕捉到猫，必须要在猫商店购买该体型的捕捉专用道具。"};
case 18: return new TipsNoteChs{ID="Tips19",Title="秘诀 19",Ch="CI10",Desc="每只猫有不同的栖息地，可到游戏教学查询。"};
case 19: return new TipsNoteChs{ID="Tips20",Title="秘诀 20",Ch="CI11",Desc="在猫咪游乐场使用相机，可获得该地图的相片。"};
case 20: return new TipsNoteChs{ID="Tips21",Title="秘诀 21",Ch="CI11",Desc="获得一般相片时，会自动获得1张猫银票。"};
case 21: return new TipsNoteChs{ID="Tips22",Title="秘诀 22",Ch="CI11",Desc="获得环景相片时，会自动获得5张猫银票。"};
case 22: return new TipsNoteChs{ID="Tips23",Title="秘诀 23",Ch="CI12",Desc="记得到首页的邮箱领取每日的奖励。"};
case 23: return new TipsNoteChs{ID="Tips24",Title="秘诀 24",Ch="CI12",Desc="每日的奖励会在每月1日重新计算。"};
case 24: return new TipsNoteChs{ID="Tips25",Title="秘诀 25",Ch="CI12",Desc="招待每一位好友可获得5张猫银票。"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}