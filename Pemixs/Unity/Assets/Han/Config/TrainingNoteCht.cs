using UnityEngine;
namespace Remix{
	public class TrainingNoteCht{
		public const int ID_COUNT = 43;

		public string ID {get; set;}
public string Page {get; set;}
public string Desc {get; set;}

		public static TrainingNoteCht Get(string key){
			switch (key) {
			
			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static TrainingNoteCht Get(int key){
			switch (key) {
			case 0: return new TrainingNoteCht{Desc="歡迎來到MeowRemix，現在開始教學關卡。"};
case 1: return new TrainingNoteCht{Desc="隨著節奏按下對應的顏色貓掌，貓就會做出可愛的動作喔。"};
case 2: return new TrainingNoteCht{Desc="當提示點向左邊到達判定框時，就是按下貓掌的時機。"};
case 3: return new TrainingNoteCht{Desc="再試一次，這一次要變得比較難喔。"};
case 4: return new TrainingNoteCht{Desc="當提示點向左邊到達判定框時，就是按下貓掌的時機。"};
case 5: return new TrainingNoteCht{Desc="接下來要連打貓掌喔。"};
case 6: return new TrainingNoteCht{Desc="當提示點向左邊到達判定框時，就是連打貓掌的時機。"};
case 7: return new TrainingNoteCht{Desc="再來是紅色的上滑動滑動提示點。"};
case 8: return new TrainingNoteCht{Desc="當提示點向左邊到達判定框時，就是向上滑動貓掌的時機。"};
case 9: return new TrainingNoteCht{Desc="再來是紅色的下滑動滑動提示點。"};
case 10: return new TrainingNoteCht{Desc="當提示點向左邊到達判定框時，就是向下滑動貓掌的時機。"};
case 11: return new TrainingNoteCht{Desc="接下來要持續的搓貓掌喔。"};
case 12: return new TrainingNoteCht{Desc="當提示點向左邊到達判定框時，就是搓貓掌的時機。"};
case 13: return new TrainingNoteCht{Desc="太厲害了，你根本不用我教嘛。"};
case 14: return new TrainingNoteCht{Desc="現在開始互動模式教學，貓咪會有各種不同的心情喔。"};
case 15: return new TrainingNoteCht{Desc="點擊貓咪的頭像可以就跟貓咪互動。"};
case 16: return new TrainingNoteCht{Desc="在貓咪開心時，持續撫摸貓咪可以增加EXP。"};
case 17: return new TrainingNoteCht{Desc="摸太多貓咪會生氣喔。"};
case 18: return new TrainingNoteCht{Desc="使用貓草可以讓貓咪快速冷靜下來。"};
case 19: return new TrainingNoteCht{Desc="貓咪有時會興奮，想要玩玩具。"};
case 20: return new TrainingNoteCht{Desc="跟貓咪玩玩具可以增加EXP。"};
case 21: return new TrainingNoteCht{Desc="不同體型的貓咪喜歡的玩具不一樣。"};
case 22: return new TrainingNoteCht{Desc="遊玩關卡後，貓咪的HP減少時會肚子餓。"};
case 23: return new TrainingNoteCht{Desc="給貓咪吃飯可以回復HP。"};
case 24: return new TrainingNoteCht{Desc="遊玩關卡太多時，貓咪會累到睡著。"};
case 25: return new TrainingNoteCht{Desc="貓咪睡著時可以把貓咪摸醒。"};
case 26: return new TrainingNoteCht{Desc="貓咪被吵醒雖然會生氣，但可以繼續遊玩關卡。"};
case 27: return new TrainingNoteCht{Desc="記得每天都來關心一下你的貓咪喔。"};
case 28: return new TrainingNoteCht{Desc="現在開始探索模式教學，每個地圖都可以遇見不同的貓咪喔。"};
case 29: return new TrainingNoteCht{Desc="點擊探索模式按鈕進入探索地圖。"};
case 30: return new TrainingNoteCht{Desc="請在貓咪遊樂場使用相機。"};
case 31: return new TrainingNoteCht{Desc="使用腳架可加速等待時間。"};
case 32: return new TrainingNoteCht{Desc="恭喜你獲得新照片，獲得照片時也會同時獲得貓銀票。"};
case 33: return new TrainingNoteCht{Desc="每個不同的地圖都有照片可以收集喔。"};
case 34: return new TrainingNoteCht{Desc="請在貓咪遊樂場使用補貓道具。"};
case 35: return new TrainingNoteCht{Desc="使用貓草可加速等待時間。"};
case 36: return new TrainingNoteCht{Desc="恭喜你獲得新貓咪，不同的補貓道具可以獲得不同體型的貓咪。"};
case 37: return new TrainingNoteCht{Desc="來試試更換貓咪吧。"};
case 38: return new TrainingNoteCht{Desc="點擊貓咪的頭像進入戶動模式。"};
case 39: return new TrainingNoteCht{Desc="點擊下一頁可以更換使用貓咪。"};
case 40: return new TrainingNoteCht{Desc="點擊選擇貓咪按鈕就可以使用新貓咪來遊玩關卡喔。"};
case 41: return new TrainingNoteCht{Desc="貓鈔票不足時記得來這裡補給喔。"};
case 42: return new TrainingNoteCht{Desc="恭喜你完成所有教學，來領取禮物吧。"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}