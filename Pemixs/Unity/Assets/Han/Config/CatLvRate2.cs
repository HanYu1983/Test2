using UnityEngine;
namespace Remix{
	public class CatLvRate2{
		public const int ID_COUNT = 50;

		public string Lv {get; set;}
public string Type {get; set;}
public int Exp {get; set;}
public int Hp {get; set;}

		public static CatLvRate2 Get(string key){
			switch (key) {
			case "1": return new CatLvRate2{Lv="1",Type="CLV",Exp=4,Hp=130};
case "2": return new CatLvRate2{Lv="2",Type="CLV",Exp=13,Hp=134};
case "3": return new CatLvRate2{Lv="3",Type="CLV",Exp=25,Hp=138};
case "4": return new CatLvRate2{Lv="4",Type="CLV",Exp=39,Hp=142};
case "5": return new CatLvRate2{Lv="5",Type="CLV",Exp=56,Hp=146};
case "6": return new CatLvRate2{Lv="6",Type="CLV",Exp=75,Hp=150};
case "7": return new CatLvRate2{Lv="7",Type="CLV",Exp=98,Hp=154};
case "8": return new CatLvRate2{Lv="8",Type="CLV",Exp=122,Hp=158};
case "9": return new CatLvRate2{Lv="9",Type="CLV",Exp=150,Hp=162};
case "10": return new CatLvRate2{Lv="10",Type="CLV",Exp=179,Hp=166};
case "11": return new CatLvRate2{Lv="11",Type="CLV",Exp=213,Hp=170};
case "12": return new CatLvRate2{Lv="12",Type="CLV",Exp=252,Hp=174};
case "13": return new CatLvRate2{Lv="13",Type="CLV",Exp=296,Hp=178};
case "14": return new CatLvRate2{Lv="14",Type="CLV",Exp=346,Hp=182};
case "15": return new CatLvRate2{Lv="15",Type="CLV",Exp=400,Hp=186};
case "16": return new CatLvRate2{Lv="16",Type="CLV",Exp=462,Hp=190};
case "17": return new CatLvRate2{Lv="17",Type="CLV",Exp=530,Hp=194};
case "18": return new CatLvRate2{Lv="18",Type="CLV",Exp=607,Hp=198};
case "19": return new CatLvRate2{Lv="19",Type="CLV",Exp=692,Hp=202};
case "20": return new CatLvRate2{Lv="20",Type="CLV",Exp=784,Hp=206};
case "21": return new CatLvRate2{Lv="21",Type="CLV",Exp=885,Hp=210};
case "22": return new CatLvRate2{Lv="22",Type="CLV",Exp=997,Hp=214};
case "23": return new CatLvRate2{Lv="23",Type="CLV",Exp=1119,Hp=218};
case "24": return new CatLvRate2{Lv="24",Type="CLV",Exp=1252,Hp=222};
case "25": return new CatLvRate2{Lv="25",Type="CLV",Exp=1395,Hp=226};
case "26": return new CatLvRate2{Lv="26",Type="CLV",Exp=1550,Hp=230};
case "27": return new CatLvRate2{Lv="27",Type="CLV",Exp=1717,Hp=234};
case "28": return new CatLvRate2{Lv="28",Type="CLV",Exp=1898,Hp=238};
case "29": return new CatLvRate2{Lv="29",Type="CLV",Exp=2092,Hp=242};
case "30": return new CatLvRate2{Lv="30",Type="CLV",Exp=2298,Hp=246};
case "31": return new CatLvRate2{Lv="31",Type="CLV",Exp=2519,Hp=250};
case "32": return new CatLvRate2{Lv="32",Type="CLV",Exp=2756,Hp=254};
case "33": return new CatLvRate2{Lv="33",Type="CLV",Exp=3008,Hp=258};
case "34": return new CatLvRate2{Lv="34",Type="CLV",Exp=3276,Hp=262};
case "35": return new CatLvRate2{Lv="35",Type="CLV",Exp=3559,Hp=266};
case "36": return new CatLvRate2{Lv="36",Type="CLV",Exp=3861,Hp=270};
case "37": return new CatLvRate2{Lv="37",Type="CLV",Exp=4183,Hp=274};
case "38": return new CatLvRate2{Lv="38",Type="CLV",Exp=4527,Hp=278};
case "39": return new CatLvRate2{Lv="39",Type="CLV",Exp=4891,Hp=282};
case "40": return new CatLvRate2{Lv="40",Type="CLV",Exp=5275,Hp=286};
case "41": return new CatLvRate2{Lv="41",Type="CLV",Exp=5684,Hp=290};
case "42": return new CatLvRate2{Lv="42",Type="CLV",Exp=6118,Hp=294};
case "43": return new CatLvRate2{Lv="43",Type="CLV",Exp=6578,Hp=298};
case "44": return new CatLvRate2{Lv="44",Type="CLV",Exp=7064,Hp=302};
case "45": return new CatLvRate2{Lv="45",Type="CLV",Exp=7576,Hp=306};
case "46": return new CatLvRate2{Lv="46",Type="CLV",Exp=8121,Hp=310};
case "47": return new CatLvRate2{Lv="47",Type="CLV",Exp=8705,Hp=314};
case "48": return new CatLvRate2{Lv="48",Type="CLV",Exp=9328,Hp=318};
case "49": return new CatLvRate2{Lv="49",Type="CLV",Exp=9989,Hp=322};
case "50": return new CatLvRate2{Lv="50",Type="CLV",Exp=10690,Hp=326};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static CatLvRate2 Get(int key){
			switch (key) {
			case 0: return new CatLvRate2{Lv="1",Type="CLV",Exp=4,Hp=130};
case 1: return new CatLvRate2{Lv="2",Type="CLV",Exp=13,Hp=134};
case 2: return new CatLvRate2{Lv="3",Type="CLV",Exp=25,Hp=138};
case 3: return new CatLvRate2{Lv="4",Type="CLV",Exp=39,Hp=142};
case 4: return new CatLvRate2{Lv="5",Type="CLV",Exp=56,Hp=146};
case 5: return new CatLvRate2{Lv="6",Type="CLV",Exp=75,Hp=150};
case 6: return new CatLvRate2{Lv="7",Type="CLV",Exp=98,Hp=154};
case 7: return new CatLvRate2{Lv="8",Type="CLV",Exp=122,Hp=158};
case 8: return new CatLvRate2{Lv="9",Type="CLV",Exp=150,Hp=162};
case 9: return new CatLvRate2{Lv="10",Type="CLV",Exp=179,Hp=166};
case 10: return new CatLvRate2{Lv="11",Type="CLV",Exp=213,Hp=170};
case 11: return new CatLvRate2{Lv="12",Type="CLV",Exp=252,Hp=174};
case 12: return new CatLvRate2{Lv="13",Type="CLV",Exp=296,Hp=178};
case 13: return new CatLvRate2{Lv="14",Type="CLV",Exp=346,Hp=182};
case 14: return new CatLvRate2{Lv="15",Type="CLV",Exp=400,Hp=186};
case 15: return new CatLvRate2{Lv="16",Type="CLV",Exp=462,Hp=190};
case 16: return new CatLvRate2{Lv="17",Type="CLV",Exp=530,Hp=194};
case 17: return new CatLvRate2{Lv="18",Type="CLV",Exp=607,Hp=198};
case 18: return new CatLvRate2{Lv="19",Type="CLV",Exp=692,Hp=202};
case 19: return new CatLvRate2{Lv="20",Type="CLV",Exp=784,Hp=206};
case 20: return new CatLvRate2{Lv="21",Type="CLV",Exp=885,Hp=210};
case 21: return new CatLvRate2{Lv="22",Type="CLV",Exp=997,Hp=214};
case 22: return new CatLvRate2{Lv="23",Type="CLV",Exp=1119,Hp=218};
case 23: return new CatLvRate2{Lv="24",Type="CLV",Exp=1252,Hp=222};
case 24: return new CatLvRate2{Lv="25",Type="CLV",Exp=1395,Hp=226};
case 25: return new CatLvRate2{Lv="26",Type="CLV",Exp=1550,Hp=230};
case 26: return new CatLvRate2{Lv="27",Type="CLV",Exp=1717,Hp=234};
case 27: return new CatLvRate2{Lv="28",Type="CLV",Exp=1898,Hp=238};
case 28: return new CatLvRate2{Lv="29",Type="CLV",Exp=2092,Hp=242};
case 29: return new CatLvRate2{Lv="30",Type="CLV",Exp=2298,Hp=246};
case 30: return new CatLvRate2{Lv="31",Type="CLV",Exp=2519,Hp=250};
case 31: return new CatLvRate2{Lv="32",Type="CLV",Exp=2756,Hp=254};
case 32: return new CatLvRate2{Lv="33",Type="CLV",Exp=3008,Hp=258};
case 33: return new CatLvRate2{Lv="34",Type="CLV",Exp=3276,Hp=262};
case 34: return new CatLvRate2{Lv="35",Type="CLV",Exp=3559,Hp=266};
case 35: return new CatLvRate2{Lv="36",Type="CLV",Exp=3861,Hp=270};
case 36: return new CatLvRate2{Lv="37",Type="CLV",Exp=4183,Hp=274};
case 37: return new CatLvRate2{Lv="38",Type="CLV",Exp=4527,Hp=278};
case 38: return new CatLvRate2{Lv="39",Type="CLV",Exp=4891,Hp=282};
case 39: return new CatLvRate2{Lv="40",Type="CLV",Exp=5275,Hp=286};
case 40: return new CatLvRate2{Lv="41",Type="CLV",Exp=5684,Hp=290};
case 41: return new CatLvRate2{Lv="42",Type="CLV",Exp=6118,Hp=294};
case 42: return new CatLvRate2{Lv="43",Type="CLV",Exp=6578,Hp=298};
case 43: return new CatLvRate2{Lv="44",Type="CLV",Exp=7064,Hp=302};
case 44: return new CatLvRate2{Lv="45",Type="CLV",Exp=7576,Hp=306};
case 45: return new CatLvRate2{Lv="46",Type="CLV",Exp=8121,Hp=310};
case 46: return new CatLvRate2{Lv="47",Type="CLV",Exp=8705,Hp=314};
case 47: return new CatLvRate2{Lv="48",Type="CLV",Exp=9328,Hp=318};
case 48: return new CatLvRate2{Lv="49",Type="CLV",Exp=9989,Hp=322};
case 49: return new CatLvRate2{Lv="50",Type="CLV",Exp=10690,Hp=326};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}