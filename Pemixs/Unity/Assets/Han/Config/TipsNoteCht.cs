using UnityEngine;
namespace Remix{
	public class TipsNoteCht{
		public const int ID_COUNT = 25;

		public string ID {get; set;}
public string Title {get; set;}
public string Ch {get; set;}
public string Desc {get; set;}

		public static TipsNoteCht Get(string key){
			switch (key) {
			case "Tips01": return new TipsNoteCht{ID="Tips01",Title="秘訣 01",Ch="CI01",Desc="每日可觀看獎勵廣告8次，每次可獲得1張貓銀票。"};
case "Tips02": return new TipsNoteCht{ID="Tips02",Title="秘訣 02",Ch="CI02",Desc="通關一個地圖的全部Easy關卡，可獲得2張貓銀票。"};
case "Tips03": return new TipsNoteCht{ID="Tips03",Title="秘訣 03",Ch="CI02",Desc="通關一個地圖的全部Normal關卡，可獲得5張貓銀票。"};
case "Tips04": return new TipsNoteCht{ID="Tips04",Title="秘訣 04",Ch="CI02",Desc="通關一個地圖的全部Hard關卡，可獲得10張貓銀票。"};
case "Tips05": return new TipsNoteCht{ID="Tips05",Title="秘訣 05",Ch="CI03",Desc="攜帶到關卡中的食物道具，在HP為0時會自動使用。"};
case "Tips06": return new TipsNoteCht{ID="Tips06",Title="秘訣 06",Ch="CI03",Desc="攜帶到關卡中的食物道具，若無使用會自動回到道具倉庫。"};
case "Tips07": return new TipsNoteCht{ID="Tips07",Title="秘訣 07",Ch="CI04",Desc="使用同一隻貓咪連續玩太多關卡，貓咪會睡著。"};
case "Tips08": return new TipsNoteCht{ID="Tips08",Title="秘訣 08",Ch="CI04",Desc="貓咪睡著時，可撫摸把貓咪叫醒。"};
case "Tips09": return new TipsNoteCht{ID="Tips09",Title="秘訣 09",Ch="CI05",Desc="不同體型的貓擅長不同模式的遊戲關卡。"};
case "Tips10": return new TipsNoteCht{ID="Tips10",Title="秘訣 10",Ch="CI05",Desc="每一關卡可獲得的道具與機率，可到貓唱片行查詢。"};
case "Tips11": return new TipsNoteCht{ID="Tips11",Title="秘訣 11",Ch="CI06",Desc="點擊貓咪頭像可直接開啟互動視窗。"};
case "Tips12": return new TipsNoteCht{ID="Tips12",Title="秘訣 12",Ch="CI07",Desc="撫摸貓咪可增加EXP。"};
case "Tips13": return new TipsNoteCht{ID="Tips13",Title="秘訣 13",Ch="CI07",Desc="貓咪生氣時，可使用貓草讓貓咪冷靜下來。"};
case "Tips14": return new TipsNoteCht{ID="Tips14",Title="秘訣 14",Ch="CI08",Desc="貓咪興奮時，跟他玩玩具可增加EXP。"};
case "Tips15": return new TipsNoteCht{ID="Tips15",Title="秘訣 15",Ch="CI08",Desc="不同體型的貓咪，喜歡的玩具不一樣。"};
case "Tips16": return new TipsNoteCht{ID="Tips16",Title="秘訣 16",Ch="CI09",Desc="貓咪的HP降低時，需要吃食物回復。"};
case "Tips17": return new TipsNoteCht{ID="Tips17",Title="秘訣 17",Ch="CI10",Desc="若捕捉到重覆的貓咪會直接增加該貓咪的EXP500。"};
case "Tips18": return new TipsNoteCht{ID="Tips18",Title="秘訣 18",Ch="CI10",Desc="要捕捉到貓，必須要在猫商店購買該體型的捕捉專用道具。"};
case "Tips19": return new TipsNoteCht{ID="Tips19",Title="秘訣 19",Ch="CI10",Desc="每隻貓有不同的棲息地，可到遊戲教學查詢。"};
case "Tips20": return new TipsNoteCht{ID="Tips20",Title="秘訣 20",Ch="CI11",Desc="在貓咪遊樂場使用相機，可獲得該地圖的相片。"};
case "Tips21": return new TipsNoteCht{ID="Tips21",Title="秘訣 21",Ch="CI11",Desc="獲得一般相片時，會自動獲得1張貓銀票。"};
case "Tips22": return new TipsNoteCht{ID="Tips22",Title="秘訣 22",Ch="CI11",Desc="獲得環景相片時，會自動獲得5張貓銀票。"};
case "Tips23": return new TipsNoteCht{ID="Tips23",Title="秘訣 23",Ch="CI12",Desc="記得到首頁的郵箱領取每日的獎勵。"};
case "Tips24": return new TipsNoteCht{ID="Tips24",Title="秘訣 24",Ch="CI12",Desc="每日的獎勵會在每月1日重新計算。"};
case "Tips25": return new TipsNoteCht{ID="Tips25",Title="秘訣 25",Ch="CI12",Desc="招待每一位好友可獲得5張貓銀票。"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static TipsNoteCht Get(int key){
			switch (key) {
			case 0: return new TipsNoteCht{ID="Tips01",Title="秘訣 01",Ch="CI01",Desc="每日可觀看獎勵廣告8次，每次可獲得1張貓銀票。"};
case 1: return new TipsNoteCht{ID="Tips02",Title="秘訣 02",Ch="CI02",Desc="通關一個地圖的全部Easy關卡，可獲得2張貓銀票。"};
case 2: return new TipsNoteCht{ID="Tips03",Title="秘訣 03",Ch="CI02",Desc="通關一個地圖的全部Normal關卡，可獲得5張貓銀票。"};
case 3: return new TipsNoteCht{ID="Tips04",Title="秘訣 04",Ch="CI02",Desc="通關一個地圖的全部Hard關卡，可獲得10張貓銀票。"};
case 4: return new TipsNoteCht{ID="Tips05",Title="秘訣 05",Ch="CI03",Desc="攜帶到關卡中的食物道具，在HP為0時會自動使用。"};
case 5: return new TipsNoteCht{ID="Tips06",Title="秘訣 06",Ch="CI03",Desc="攜帶到關卡中的食物道具，若無使用會自動回到道具倉庫。"};
case 6: return new TipsNoteCht{ID="Tips07",Title="秘訣 07",Ch="CI04",Desc="使用同一隻貓咪連續玩太多關卡，貓咪會睡著。"};
case 7: return new TipsNoteCht{ID="Tips08",Title="秘訣 08",Ch="CI04",Desc="貓咪睡著時，可撫摸把貓咪叫醒。"};
case 8: return new TipsNoteCht{ID="Tips09",Title="秘訣 09",Ch="CI05",Desc="不同體型的貓擅長不同模式的遊戲關卡。"};
case 9: return new TipsNoteCht{ID="Tips10",Title="秘訣 10",Ch="CI05",Desc="每一關卡可獲得的道具與機率，可到貓唱片行查詢。"};
case 10: return new TipsNoteCht{ID="Tips11",Title="秘訣 11",Ch="CI06",Desc="點擊貓咪頭像可直接開啟互動視窗。"};
case 11: return new TipsNoteCht{ID="Tips12",Title="秘訣 12",Ch="CI07",Desc="撫摸貓咪可增加EXP。"};
case 12: return new TipsNoteCht{ID="Tips13",Title="秘訣 13",Ch="CI07",Desc="貓咪生氣時，可使用貓草讓貓咪冷靜下來。"};
case 13: return new TipsNoteCht{ID="Tips14",Title="秘訣 14",Ch="CI08",Desc="貓咪興奮時，跟他玩玩具可增加EXP。"};
case 14: return new TipsNoteCht{ID="Tips15",Title="秘訣 15",Ch="CI08",Desc="不同體型的貓咪，喜歡的玩具不一樣。"};
case 15: return new TipsNoteCht{ID="Tips16",Title="秘訣 16",Ch="CI09",Desc="貓咪的HP降低時，需要吃食物回復。"};
case 16: return new TipsNoteCht{ID="Tips17",Title="秘訣 17",Ch="CI10",Desc="若捕捉到重覆的貓咪會直接增加該貓咪的EXP500。"};
case 17: return new TipsNoteCht{ID="Tips18",Title="秘訣 18",Ch="CI10",Desc="要捕捉到貓，必須要在猫商店購買該體型的捕捉專用道具。"};
case 18: return new TipsNoteCht{ID="Tips19",Title="秘訣 19",Ch="CI10",Desc="每隻貓有不同的棲息地，可到遊戲教學查詢。"};
case 19: return new TipsNoteCht{ID="Tips20",Title="秘訣 20",Ch="CI11",Desc="在貓咪遊樂場使用相機，可獲得該地圖的相片。"};
case 20: return new TipsNoteCht{ID="Tips21",Title="秘訣 21",Ch="CI11",Desc="獲得一般相片時，會自動獲得1張貓銀票。"};
case 21: return new TipsNoteCht{ID="Tips22",Title="秘訣 22",Ch="CI11",Desc="獲得環景相片時，會自動獲得5張貓銀票。"};
case 22: return new TipsNoteCht{ID="Tips23",Title="秘訣 23",Ch="CI12",Desc="記得到首頁的郵箱領取每日的獎勵。"};
case 23: return new TipsNoteCht{ID="Tips24",Title="秘訣 24",Ch="CI12",Desc="每日的獎勵會在每月1日重新計算。"};
case 24: return new TipsNoteCht{ID="Tips25",Title="秘訣 25",Ch="CI12",Desc="招待每一位好友可獲得5張貓銀票。"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}