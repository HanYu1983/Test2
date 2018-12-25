using UnityEngine;
namespace Remix{
	public class GiftDef{
		public const int ID_COUNT = 25;

		public string ID {get; set;}
public string Item {get; set;}
public int Quantity {get; set;}

		public static GiftDef Get(string key){
			switch (key) {
			case "Mg01": return new GiftDef{ID="Mg01",Item="I10010",Quantity=1};
case "Mg02": return new GiftDef{ID="Mg02",Item="I20010",Quantity=1};
case "Mg03": return new GiftDef{ID="Mg03",Item="I40010",Quantity=1};
case "Mg04": return new GiftDef{ID="Mg04",Item="I30010",Quantity=1};
case "Mg05": return new GiftDef{ID="Mg05",Item="I41030",Quantity=1};
case "Mg06": return new GiftDef{ID="Mg06",Item="I10020",Quantity=1};
case "Mg07": return new GiftDef{ID="Mg07",Item="I21010",Quantity=1};
case "Mg08": return new GiftDef{ID="Mg08",Item="I40010",Quantity=1};
case "Mg09": return new GiftDef{ID="Mg09",Item="I31010",Quantity=1};
case "Mg10": return new GiftDef{ID="Mg10",Item="I32020",Quantity=1};
case "Mg11": return new GiftDef{ID="Mg11",Item="I10030",Quantity=1};
case "Mg12": return new GiftDef{ID="Mg12",Item="I20020",Quantity=1};
case "Mg13": return new GiftDef{ID="Mg13",Item="I40010",Quantity=1};
case "Mg14": return new GiftDef{ID="Mg14",Item="I30040",Quantity=1};
case "Mg15": return new GiftDef{ID="Mg15",Item="I41010",Quantity=1};
case "Mg16": return new GiftDef{ID="Mg16",Item="I10040",Quantity=1};
case "Mg17": return new GiftDef{ID="Mg17",Item="I21020",Quantity=1};
case "Mg18": return new GiftDef{ID="Mg18",Item="I40020",Quantity=1};
case "Mg19": return new GiftDef{ID="Mg19",Item="I31020",Quantity=1};
case "Mg20": return new GiftDef{ID="Mg20",Item="I32030",Quantity=1};
case "Mg21": return new GiftDef{ID="Mg21",Item="I10050",Quantity=1};
case "Mg22": return new GiftDef{ID="Mg22",Item="I20030",Quantity=1};
case "Mg23": return new GiftDef{ID="Mg23",Item="I40020",Quantity=1};
case "Mg24": return new GiftDef{ID="Mg24",Item="I30070",Quantity=1};
case "Mg25": return new GiftDef{ID="Mg25",Item="I41020",Quantity=1};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static GiftDef Get(int key){
			switch (key) {
			case 0: return new GiftDef{ID="Mg01",Item="I10010",Quantity=1};
case 1: return new GiftDef{ID="Mg02",Item="I20010",Quantity=1};
case 2: return new GiftDef{ID="Mg03",Item="I40010",Quantity=1};
case 3: return new GiftDef{ID="Mg04",Item="I30010",Quantity=1};
case 4: return new GiftDef{ID="Mg05",Item="I41030",Quantity=1};
case 5: return new GiftDef{ID="Mg06",Item="I10020",Quantity=1};
case 6: return new GiftDef{ID="Mg07",Item="I21010",Quantity=1};
case 7: return new GiftDef{ID="Mg08",Item="I40010",Quantity=1};
case 8: return new GiftDef{ID="Mg09",Item="I31010",Quantity=1};
case 9: return new GiftDef{ID="Mg10",Item="I32020",Quantity=1};
case 10: return new GiftDef{ID="Mg11",Item="I10030",Quantity=1};
case 11: return new GiftDef{ID="Mg12",Item="I20020",Quantity=1};
case 12: return new GiftDef{ID="Mg13",Item="I40010",Quantity=1};
case 13: return new GiftDef{ID="Mg14",Item="I30040",Quantity=1};
case 14: return new GiftDef{ID="Mg15",Item="I41010",Quantity=1};
case 15: return new GiftDef{ID="Mg16",Item="I10040",Quantity=1};
case 16: return new GiftDef{ID="Mg17",Item="I21020",Quantity=1};
case 17: return new GiftDef{ID="Mg18",Item="I40020",Quantity=1};
case 18: return new GiftDef{ID="Mg19",Item="I31020",Quantity=1};
case 19: return new GiftDef{ID="Mg20",Item="I32030",Quantity=1};
case 20: return new GiftDef{ID="Mg21",Item="I10050",Quantity=1};
case 21: return new GiftDef{ID="Mg22",Item="I20030",Quantity=1};
case 22: return new GiftDef{ID="Mg23",Item="I40020",Quantity=1};
case 23: return new GiftDef{ID="Mg24",Item="I30070",Quantity=1};
case 24: return new GiftDef{ID="Mg25",Item="I41020",Quantity=1};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}