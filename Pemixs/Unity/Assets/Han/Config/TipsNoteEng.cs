using UnityEngine;
namespace Remix{
	public class TipsNoteEng{
		public const int ID_COUNT = 25;

		public string ID {get; set;}
public string Title {get; set;}
public string Ch {get; set;}
public string Desc {get; set;}

		public static TipsNoteEng Get(string key){
			switch (key) {
			case "Tips01": return new TipsNoteEng{ID="Tips01",Title="Tips 01",Ch="CI01",Desc="You can watch AD 8 times a day， Each time will get 1 cat money"};
case "Tips02": return new TipsNoteEng{ID="Tips02",Title="Tips 02",Ch="CI02",Desc="Clear all the easy levels of one map， will get 2 cat money"};
case "Tips03": return new TipsNoteEng{ID="Tips03",Title="Tips 03",Ch="CI02",Desc="Clear all the normal levels of one map， will get 5 cat money"};
case "Tips04": return new TipsNoteEng{ID="Tips04",Title="Tips 04",Ch="CI02",Desc="Clear all the hard levels of one map， will get 10 cat money"};
case "Tips05": return new TipsNoteEng{ID="Tips05",Title="Tips 05",Ch="CI03",Desc="The food will be used in the level when your HP reach 0"};
case "Tips06": return new TipsNoteEng{ID="Tips06",Title="Tips 06",Ch="CI03",Desc="If the food isn't used in the level， it will be returned to the Item storage"};
case "Tips07": return new TipsNoteEng{ID="Tips07",Title="Tips 07",Ch="CI04",Desc="Use one cat to play too many levels， will make him fall asleep "};
case "Tips08": return new TipsNoteEng{ID="Tips08",Title="Tips 08",Ch="CI04",Desc="Touch the cat to wake him up"};
case "Tips09": return new TipsNoteEng{ID="Tips09",Title="Tips 09",Ch="CI05",Desc="Different body type of cats is good at different game mode"};
case "Tips10": return new TipsNoteEng{ID="Tips10",Title="Tips 10",Ch="CI05",Desc="Got to cat record to check the probability of every level's reward "};
case "Tips11": return new TipsNoteEng{ID="Tips11",Title="Tips 11",Ch="CI06",Desc="Click on cat%27s profile picture%2c it will open the interactive window."};
case "Tips12": return new TipsNoteEng{ID="Tips12",Title="Tips 12",Ch="CI07",Desc="Pet the cat to add EXP"};
case "Tips13": return new TipsNoteEng{ID="Tips13",Title="Tips 13",Ch="CI07",Desc="When the cat is angry， use catnip to cool him down"};
case "Tips14": return new TipsNoteEng{ID="Tips14",Title="Tips 14",Ch="CI08",Desc="When the cat is excited， use a toy to add EXP"};
case "Tips15": return new TipsNoteEng{ID="Tips15",Title="Tips 15",Ch="CI08",Desc="Different body type of cats likes different kinds of toys"};
case "Tips16": return new TipsNoteEng{ID="Tips16",Title="Tips 16",Ch="CI09",Desc="Feed the cat to add HP"};
case "Tips17": return new TipsNoteEng{ID="Tips17",Title="Tips 17",Ch="CI10",Desc="If you capture a cat you already have， will add the cat EXP500"};
case "Tips18": return new TipsNoteEng{ID="Tips18",Title="Tips 18",Ch="CI10",Desc="If you want to capture a cat， please go to Cat shop to buy exclusive gacha items"};
case "Tips19": return new TipsNoteEng{ID="Tips19",Title="Tips 19",Ch="CI10",Desc="Got to Tutorial to check every cat's Habitat"};
case "Tips20": return new TipsNoteEng{ID="Tips20",Title="Tips 20",Ch="CI11",Desc="Use cameras at cat tree， will get photos"};
case "Tips21": return new TipsNoteEng{ID="Tips21",Title="Tips 21",Ch="CI11",Desc="When you get a normal photo，will get 1 cat money"};
case "Tips22": return new TipsNoteEng{ID="Tips22",Title="Tips 22",Ch="CI11",Desc="When you get a panorama， will get 5 cat money"};
case "Tips23": return new TipsNoteEng{ID="Tips23",Title="Tips 23",Ch="CI12",Desc="Receive a reward at the Mailbox of Main page everyday"};
case "Tips24": return new TipsNoteEng{ID="Tips24",Title="Tips 24",Ch="CI12",Desc="Gift list will rest at every 1st of the month"};
case "Tips25": return new TipsNoteEng{ID="Tips25",Title="Tips 25",Ch="CI12",Desc="Invite each frient， will get 5 cat money"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static TipsNoteEng Get(int key){
			switch (key) {
			case 0: return new TipsNoteEng{ID="Tips01",Title="Tips 01",Ch="CI01",Desc="You can watch AD 8 times a day， Each time will get 1 cat money"};
case 1: return new TipsNoteEng{ID="Tips02",Title="Tips 02",Ch="CI02",Desc="Clear all the easy levels of one map， will get 2 cat money"};
case 2: return new TipsNoteEng{ID="Tips03",Title="Tips 03",Ch="CI02",Desc="Clear all the normal levels of one map， will get 5 cat money"};
case 3: return new TipsNoteEng{ID="Tips04",Title="Tips 04",Ch="CI02",Desc="Clear all the hard levels of one map， will get 10 cat money"};
case 4: return new TipsNoteEng{ID="Tips05",Title="Tips 05",Ch="CI03",Desc="The food will be used in the level when your HP reach 0"};
case 5: return new TipsNoteEng{ID="Tips06",Title="Tips 06",Ch="CI03",Desc="If the food isn't used in the level， it will be returned to the Item storage"};
case 6: return new TipsNoteEng{ID="Tips07",Title="Tips 07",Ch="CI04",Desc="Use one cat to play too many levels， will make him fall asleep "};
case 7: return new TipsNoteEng{ID="Tips08",Title="Tips 08",Ch="CI04",Desc="Touch the cat to wake him up"};
case 8: return new TipsNoteEng{ID="Tips09",Title="Tips 09",Ch="CI05",Desc="Different body type of cats is good at different game mode"};
case 9: return new TipsNoteEng{ID="Tips10",Title="Tips 10",Ch="CI05",Desc="Got to cat record to check the probability of every level's reward "};
case 10: return new TipsNoteEng{ID="Tips11",Title="Tips 11",Ch="CI06",Desc="Click on cat%27s profile picture%2c it will open the interactive window."};
case 11: return new TipsNoteEng{ID="Tips12",Title="Tips 12",Ch="CI07",Desc="Pet the cat to add EXP"};
case 12: return new TipsNoteEng{ID="Tips13",Title="Tips 13",Ch="CI07",Desc="When the cat is angry， use catnip to cool him down"};
case 13: return new TipsNoteEng{ID="Tips14",Title="Tips 14",Ch="CI08",Desc="When the cat is excited， use a toy to add EXP"};
case 14: return new TipsNoteEng{ID="Tips15",Title="Tips 15",Ch="CI08",Desc="Different body type of cats likes different kinds of toys"};
case 15: return new TipsNoteEng{ID="Tips16",Title="Tips 16",Ch="CI09",Desc="Feed the cat to add HP"};
case 16: return new TipsNoteEng{ID="Tips17",Title="Tips 17",Ch="CI10",Desc="If you capture a cat you already have， will add the cat EXP500"};
case 17: return new TipsNoteEng{ID="Tips18",Title="Tips 18",Ch="CI10",Desc="If you want to capture a cat， please go to Cat shop to buy exclusive gacha items"};
case 18: return new TipsNoteEng{ID="Tips19",Title="Tips 19",Ch="CI10",Desc="Got to Tutorial to check every cat's Habitat"};
case 19: return new TipsNoteEng{ID="Tips20",Title="Tips 20",Ch="CI11",Desc="Use cameras at cat tree， will get photos"};
case 20: return new TipsNoteEng{ID="Tips21",Title="Tips 21",Ch="CI11",Desc="When you get a normal photo，will get 1 cat money"};
case 21: return new TipsNoteEng{ID="Tips22",Title="Tips 22",Ch="CI11",Desc="When you get a panorama， will get 5 cat money"};
case 22: return new TipsNoteEng{ID="Tips23",Title="Tips 23",Ch="CI12",Desc="Receive a reward at the Mailbox of Main page everyday"};
case 23: return new TipsNoteEng{ID="Tips24",Title="Tips 24",Ch="CI12",Desc="Gift list will rest at every 1st of the month"};
case 24: return new TipsNoteEng{ID="Tips25",Title="Tips 25",Ch="CI12",Desc="Invite each frient， will get 5 cat money"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}