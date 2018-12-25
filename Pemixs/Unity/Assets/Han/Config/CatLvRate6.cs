using UnityEngine;
namespace Remix{
	public class CatLvRate6{
		public const int ID_COUNT = 50;

		public string Lv {get; set;}
public string Type {get; set;}
public int Exp {get; set;}
public int Hp {get; set;}

		public static CatLvRate6 Get(string key){
			switch (key) {
			case "1": return new CatLvRate6{Lv="1",Type="CLV",Exp=9,Hp=300};
case "2": return new CatLvRate6{Lv="2",Type="CLV",Exp=30,Hp=312};
case "3": return new CatLvRate6{Lv="3",Type="CLV",Exp=57,Hp=324};
case "4": return new CatLvRate6{Lv="4",Type="CLV",Exp=90,Hp=336};
case "5": return new CatLvRate6{Lv="5",Type="CLV",Exp=129,Hp=348};
case "6": return new CatLvRate6{Lv="6",Type="CLV",Exp=174,Hp=360};
case "7": return new CatLvRate6{Lv="7",Type="CLV",Exp=225,Hp=372};
case "8": return new CatLvRate6{Lv="8",Type="CLV",Exp=282,Hp=384};
case "9": return new CatLvRate6{Lv="9",Type="CLV",Exp=345,Hp=396};
case "10": return new CatLvRate6{Lv="10",Type="CLV",Exp=414,Hp=408};
case "11": return new CatLvRate6{Lv="11",Type="CLV",Exp=492,Hp=420};
case "12": return new CatLvRate6{Lv="12",Type="CLV",Exp=582,Hp=432};
case "13": return new CatLvRate6{Lv="13",Type="CLV",Exp=684,Hp=444};
case "14": return new CatLvRate6{Lv="14",Type="CLV",Exp=798,Hp=456};
case "15": return new CatLvRate6{Lv="15",Type="CLV",Exp=924,Hp=468};
case "16": return new CatLvRate6{Lv="16",Type="CLV",Exp=1065,Hp=480};
case "17": return new CatLvRate6{Lv="17",Type="CLV",Exp=1224,Hp=492};
case "18": return new CatLvRate6{Lv="18",Type="CLV",Exp=1401,Hp=504};
case "19": return new CatLvRate6{Lv="19",Type="CLV",Exp=1596,Hp=516};
case "20": return new CatLvRate6{Lv="20",Type="CLV",Exp=1809,Hp=528};
case "21": return new CatLvRate6{Lv="21",Type="CLV",Exp=2043,Hp=540};
case "22": return new CatLvRate6{Lv="22",Type="CLV",Exp=2301,Hp=552};
case "23": return new CatLvRate6{Lv="23",Type="CLV",Exp=2583,Hp=564};
case "24": return new CatLvRate6{Lv="24",Type="CLV",Exp=2889,Hp=576};
case "25": return new CatLvRate6{Lv="25",Type="CLV",Exp=3219,Hp=588};
case "26": return new CatLvRate6{Lv="26",Type="CLV",Exp=3576,Hp=600};
case "27": return new CatLvRate6{Lv="27",Type="CLV",Exp=3963,Hp=612};
case "28": return new CatLvRate6{Lv="28",Type="CLV",Exp=4380,Hp=624};
case "29": return new CatLvRate6{Lv="29",Type="CLV",Exp=4827,Hp=636};
case "30": return new CatLvRate6{Lv="30",Type="CLV",Exp=5304,Hp=648};
case "31": return new CatLvRate6{Lv="31",Type="CLV",Exp=5814,Hp=660};
case "32": return new CatLvRate6{Lv="32",Type="CLV",Exp=6360,Hp=672};
case "33": return new CatLvRate6{Lv="33",Type="CLV",Exp=6942,Hp=684};
case "34": return new CatLvRate6{Lv="34",Type="CLV",Exp=7560,Hp=696};
case "35": return new CatLvRate6{Lv="35",Type="CLV",Exp=8214,Hp=708};
case "36": return new CatLvRate6{Lv="36",Type="CLV",Exp=8910,Hp=720};
case "37": return new CatLvRate6{Lv="37",Type="CLV",Exp=9654,Hp=732};
case "38": return new CatLvRate6{Lv="38",Type="CLV",Exp=10446,Hp=744};
case "39": return new CatLvRate6{Lv="39",Type="CLV",Exp=11286,Hp=756};
case "40": return new CatLvRate6{Lv="40",Type="CLV",Exp=12174,Hp=768};
case "41": return new CatLvRate6{Lv="41",Type="CLV",Exp=13116,Hp=780};
case "42": return new CatLvRate6{Lv="42",Type="CLV",Exp=14118,Hp=792};
case "43": return new CatLvRate6{Lv="43",Type="CLV",Exp=15180,Hp=804};
case "44": return new CatLvRate6{Lv="44",Type="CLV",Exp=16302,Hp=816};
case "45": return new CatLvRate6{Lv="45",Type="CLV",Exp=17484,Hp=828};
case "46": return new CatLvRate6{Lv="46",Type="CLV",Exp=18741,Hp=840};
case "47": return new CatLvRate6{Lv="47",Type="CLV",Exp=20088,Hp=852};
case "48": return new CatLvRate6{Lv="48",Type="CLV",Exp=21525,Hp=864};
case "49": return new CatLvRate6{Lv="49",Type="CLV",Exp=23052,Hp=876};
case "50": return new CatLvRate6{Lv="50",Type="CLV",Exp=24669,Hp=888};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static CatLvRate6 Get(int key){
			switch (key) {
			case 0: return new CatLvRate6{Lv="1",Type="CLV",Exp=9,Hp=300};
case 1: return new CatLvRate6{Lv="2",Type="CLV",Exp=30,Hp=312};
case 2: return new CatLvRate6{Lv="3",Type="CLV",Exp=57,Hp=324};
case 3: return new CatLvRate6{Lv="4",Type="CLV",Exp=90,Hp=336};
case 4: return new CatLvRate6{Lv="5",Type="CLV",Exp=129,Hp=348};
case 5: return new CatLvRate6{Lv="6",Type="CLV",Exp=174,Hp=360};
case 6: return new CatLvRate6{Lv="7",Type="CLV",Exp=225,Hp=372};
case 7: return new CatLvRate6{Lv="8",Type="CLV",Exp=282,Hp=384};
case 8: return new CatLvRate6{Lv="9",Type="CLV",Exp=345,Hp=396};
case 9: return new CatLvRate6{Lv="10",Type="CLV",Exp=414,Hp=408};
case 10: return new CatLvRate6{Lv="11",Type="CLV",Exp=492,Hp=420};
case 11: return new CatLvRate6{Lv="12",Type="CLV",Exp=582,Hp=432};
case 12: return new CatLvRate6{Lv="13",Type="CLV",Exp=684,Hp=444};
case 13: return new CatLvRate6{Lv="14",Type="CLV",Exp=798,Hp=456};
case 14: return new CatLvRate6{Lv="15",Type="CLV",Exp=924,Hp=468};
case 15: return new CatLvRate6{Lv="16",Type="CLV",Exp=1065,Hp=480};
case 16: return new CatLvRate6{Lv="17",Type="CLV",Exp=1224,Hp=492};
case 17: return new CatLvRate6{Lv="18",Type="CLV",Exp=1401,Hp=504};
case 18: return new CatLvRate6{Lv="19",Type="CLV",Exp=1596,Hp=516};
case 19: return new CatLvRate6{Lv="20",Type="CLV",Exp=1809,Hp=528};
case 20: return new CatLvRate6{Lv="21",Type="CLV",Exp=2043,Hp=540};
case 21: return new CatLvRate6{Lv="22",Type="CLV",Exp=2301,Hp=552};
case 22: return new CatLvRate6{Lv="23",Type="CLV",Exp=2583,Hp=564};
case 23: return new CatLvRate6{Lv="24",Type="CLV",Exp=2889,Hp=576};
case 24: return new CatLvRate6{Lv="25",Type="CLV",Exp=3219,Hp=588};
case 25: return new CatLvRate6{Lv="26",Type="CLV",Exp=3576,Hp=600};
case 26: return new CatLvRate6{Lv="27",Type="CLV",Exp=3963,Hp=612};
case 27: return new CatLvRate6{Lv="28",Type="CLV",Exp=4380,Hp=624};
case 28: return new CatLvRate6{Lv="29",Type="CLV",Exp=4827,Hp=636};
case 29: return new CatLvRate6{Lv="30",Type="CLV",Exp=5304,Hp=648};
case 30: return new CatLvRate6{Lv="31",Type="CLV",Exp=5814,Hp=660};
case 31: return new CatLvRate6{Lv="32",Type="CLV",Exp=6360,Hp=672};
case 32: return new CatLvRate6{Lv="33",Type="CLV",Exp=6942,Hp=684};
case 33: return new CatLvRate6{Lv="34",Type="CLV",Exp=7560,Hp=696};
case 34: return new CatLvRate6{Lv="35",Type="CLV",Exp=8214,Hp=708};
case 35: return new CatLvRate6{Lv="36",Type="CLV",Exp=8910,Hp=720};
case 36: return new CatLvRate6{Lv="37",Type="CLV",Exp=9654,Hp=732};
case 37: return new CatLvRate6{Lv="38",Type="CLV",Exp=10446,Hp=744};
case 38: return new CatLvRate6{Lv="39",Type="CLV",Exp=11286,Hp=756};
case 39: return new CatLvRate6{Lv="40",Type="CLV",Exp=12174,Hp=768};
case 40: return new CatLvRate6{Lv="41",Type="CLV",Exp=13116,Hp=780};
case 41: return new CatLvRate6{Lv="42",Type="CLV",Exp=14118,Hp=792};
case 42: return new CatLvRate6{Lv="43",Type="CLV",Exp=15180,Hp=804};
case 43: return new CatLvRate6{Lv="44",Type="CLV",Exp=16302,Hp=816};
case 44: return new CatLvRate6{Lv="45",Type="CLV",Exp=17484,Hp=828};
case 45: return new CatLvRate6{Lv="46",Type="CLV",Exp=18741,Hp=840};
case 46: return new CatLvRate6{Lv="47",Type="CLV",Exp=20088,Hp=852};
case 47: return new CatLvRate6{Lv="48",Type="CLV",Exp=21525,Hp=864};
case 48: return new CatLvRate6{Lv="49",Type="CLV",Exp=23052,Hp=876};
case 49: return new CatLvRate6{Lv="50",Type="CLV",Exp=24669,Hp=888};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}