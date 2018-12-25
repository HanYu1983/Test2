using UnityEngine;
namespace Remix{
	public class CatLvRate5{
		public const int ID_COUNT = 50;

		public string Lv {get; set;}
public string Type {get; set;}
public int Exp {get; set;}
public int Hp {get; set;}

		public static CatLvRate5 Get(string key){
			switch (key) {
			case "1": return new CatLvRate5{Lv="1",Type="CLV",Exp=8,Hp=250};
case "2": return new CatLvRate5{Lv="2",Type="CLV",Exp=25,Hp=259};
case "3": return new CatLvRate5{Lv="3",Type="CLV",Exp=48,Hp=268};
case "4": return new CatLvRate5{Lv="4",Type="CLV",Exp=75,Hp=277};
case "5": return new CatLvRate5{Lv="5",Type="CLV",Exp=108,Hp=286};
case "6": return new CatLvRate5{Lv="6",Type="CLV",Exp=145,Hp=295};
case "7": return new CatLvRate5{Lv="7",Type="CLV",Exp=188,Hp=304};
case "8": return new CatLvRate5{Lv="8",Type="CLV",Exp=235,Hp=313};
case "9": return new CatLvRate5{Lv="9",Type="CLV",Exp=288,Hp=322};
case "10": return new CatLvRate5{Lv="10",Type="CLV",Exp=345,Hp=331};
case "11": return new CatLvRate5{Lv="11",Type="CLV",Exp=410,Hp=340};
case "12": return new CatLvRate5{Lv="12",Type="CLV",Exp=485,Hp=349};
case "13": return new CatLvRate5{Lv="13",Type="CLV",Exp=570,Hp=358};
case "14": return new CatLvRate5{Lv="14",Type="CLV",Exp=665,Hp=367};
case "15": return new CatLvRate5{Lv="15",Type="CLV",Exp=770,Hp=376};
case "16": return new CatLvRate5{Lv="16",Type="CLV",Exp=888,Hp=385};
case "17": return new CatLvRate5{Lv="17",Type="CLV",Exp=1020,Hp=394};
case "18": return new CatLvRate5{Lv="18",Type="CLV",Exp=1168,Hp=403};
case "19": return new CatLvRate5{Lv="19",Type="CLV",Exp=1330,Hp=412};
case "20": return new CatLvRate5{Lv="20",Type="CLV",Exp=1508,Hp=421};
case "21": return new CatLvRate5{Lv="21",Type="CLV",Exp=1703,Hp=430};
case "22": return new CatLvRate5{Lv="22",Type="CLV",Exp=1918,Hp=439};
case "23": return new CatLvRate5{Lv="23",Type="CLV",Exp=2153,Hp=448};
case "24": return new CatLvRate5{Lv="24",Type="CLV",Exp=2408,Hp=457};
case "25": return new CatLvRate5{Lv="25",Type="CLV",Exp=2683,Hp=466};
case "26": return new CatLvRate5{Lv="26",Type="CLV",Exp=2980,Hp=475};
case "27": return new CatLvRate5{Lv="27",Type="CLV",Exp=3303,Hp=484};
case "28": return new CatLvRate5{Lv="28",Type="CLV",Exp=3650,Hp=493};
case "29": return new CatLvRate5{Lv="29",Type="CLV",Exp=4023,Hp=502};
case "30": return new CatLvRate5{Lv="30",Type="CLV",Exp=4420,Hp=511};
case "31": return new CatLvRate5{Lv="31",Type="CLV",Exp=4845,Hp=520};
case "32": return new CatLvRate5{Lv="32",Type="CLV",Exp=5300,Hp=529};
case "33": return new CatLvRate5{Lv="33",Type="CLV",Exp=5785,Hp=538};
case "34": return new CatLvRate5{Lv="34",Type="CLV",Exp=6300,Hp=547};
case "35": return new CatLvRate5{Lv="35",Type="CLV",Exp=6845,Hp=556};
case "36": return new CatLvRate5{Lv="36",Type="CLV",Exp=7425,Hp=565};
case "37": return new CatLvRate5{Lv="37",Type="CLV",Exp=8045,Hp=574};
case "38": return new CatLvRate5{Lv="38",Type="CLV",Exp=8705,Hp=583};
case "39": return new CatLvRate5{Lv="39",Type="CLV",Exp=9405,Hp=592};
case "40": return new CatLvRate5{Lv="40",Type="CLV",Exp=10145,Hp=601};
case "41": return new CatLvRate5{Lv="41",Type="CLV",Exp=10930,Hp=610};
case "42": return new CatLvRate5{Lv="42",Type="CLV",Exp=11765,Hp=619};
case "43": return new CatLvRate5{Lv="43",Type="CLV",Exp=12650,Hp=628};
case "44": return new CatLvRate5{Lv="44",Type="CLV",Exp=13585,Hp=637};
case "45": return new CatLvRate5{Lv="45",Type="CLV",Exp=14570,Hp=646};
case "46": return new CatLvRate5{Lv="46",Type="CLV",Exp=15618,Hp=655};
case "47": return new CatLvRate5{Lv="47",Type="CLV",Exp=16740,Hp=664};
case "48": return new CatLvRate5{Lv="48",Type="CLV",Exp=17938,Hp=673};
case "49": return new CatLvRate5{Lv="49",Type="CLV",Exp=19210,Hp=682};
case "50": return new CatLvRate5{Lv="50",Type="CLV",Exp=20558,Hp=691};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static CatLvRate5 Get(int key){
			switch (key) {
			case 0: return new CatLvRate5{Lv="1",Type="CLV",Exp=8,Hp=250};
case 1: return new CatLvRate5{Lv="2",Type="CLV",Exp=25,Hp=259};
case 2: return new CatLvRate5{Lv="3",Type="CLV",Exp=48,Hp=268};
case 3: return new CatLvRate5{Lv="4",Type="CLV",Exp=75,Hp=277};
case 4: return new CatLvRate5{Lv="5",Type="CLV",Exp=108,Hp=286};
case 5: return new CatLvRate5{Lv="6",Type="CLV",Exp=145,Hp=295};
case 6: return new CatLvRate5{Lv="7",Type="CLV",Exp=188,Hp=304};
case 7: return new CatLvRate5{Lv="8",Type="CLV",Exp=235,Hp=313};
case 8: return new CatLvRate5{Lv="9",Type="CLV",Exp=288,Hp=322};
case 9: return new CatLvRate5{Lv="10",Type="CLV",Exp=345,Hp=331};
case 10: return new CatLvRate5{Lv="11",Type="CLV",Exp=410,Hp=340};
case 11: return new CatLvRate5{Lv="12",Type="CLV",Exp=485,Hp=349};
case 12: return new CatLvRate5{Lv="13",Type="CLV",Exp=570,Hp=358};
case 13: return new CatLvRate5{Lv="14",Type="CLV",Exp=665,Hp=367};
case 14: return new CatLvRate5{Lv="15",Type="CLV",Exp=770,Hp=376};
case 15: return new CatLvRate5{Lv="16",Type="CLV",Exp=888,Hp=385};
case 16: return new CatLvRate5{Lv="17",Type="CLV",Exp=1020,Hp=394};
case 17: return new CatLvRate5{Lv="18",Type="CLV",Exp=1168,Hp=403};
case 18: return new CatLvRate5{Lv="19",Type="CLV",Exp=1330,Hp=412};
case 19: return new CatLvRate5{Lv="20",Type="CLV",Exp=1508,Hp=421};
case 20: return new CatLvRate5{Lv="21",Type="CLV",Exp=1703,Hp=430};
case 21: return new CatLvRate5{Lv="22",Type="CLV",Exp=1918,Hp=439};
case 22: return new CatLvRate5{Lv="23",Type="CLV",Exp=2153,Hp=448};
case 23: return new CatLvRate5{Lv="24",Type="CLV",Exp=2408,Hp=457};
case 24: return new CatLvRate5{Lv="25",Type="CLV",Exp=2683,Hp=466};
case 25: return new CatLvRate5{Lv="26",Type="CLV",Exp=2980,Hp=475};
case 26: return new CatLvRate5{Lv="27",Type="CLV",Exp=3303,Hp=484};
case 27: return new CatLvRate5{Lv="28",Type="CLV",Exp=3650,Hp=493};
case 28: return new CatLvRate5{Lv="29",Type="CLV",Exp=4023,Hp=502};
case 29: return new CatLvRate5{Lv="30",Type="CLV",Exp=4420,Hp=511};
case 30: return new CatLvRate5{Lv="31",Type="CLV",Exp=4845,Hp=520};
case 31: return new CatLvRate5{Lv="32",Type="CLV",Exp=5300,Hp=529};
case 32: return new CatLvRate5{Lv="33",Type="CLV",Exp=5785,Hp=538};
case 33: return new CatLvRate5{Lv="34",Type="CLV",Exp=6300,Hp=547};
case 34: return new CatLvRate5{Lv="35",Type="CLV",Exp=6845,Hp=556};
case 35: return new CatLvRate5{Lv="36",Type="CLV",Exp=7425,Hp=565};
case 36: return new CatLvRate5{Lv="37",Type="CLV",Exp=8045,Hp=574};
case 37: return new CatLvRate5{Lv="38",Type="CLV",Exp=8705,Hp=583};
case 38: return new CatLvRate5{Lv="39",Type="CLV",Exp=9405,Hp=592};
case 39: return new CatLvRate5{Lv="40",Type="CLV",Exp=10145,Hp=601};
case 40: return new CatLvRate5{Lv="41",Type="CLV",Exp=10930,Hp=610};
case 41: return new CatLvRate5{Lv="42",Type="CLV",Exp=11765,Hp=619};
case 42: return new CatLvRate5{Lv="43",Type="CLV",Exp=12650,Hp=628};
case 43: return new CatLvRate5{Lv="44",Type="CLV",Exp=13585,Hp=637};
case 44: return new CatLvRate5{Lv="45",Type="CLV",Exp=14570,Hp=646};
case 45: return new CatLvRate5{Lv="46",Type="CLV",Exp=15618,Hp=655};
case 46: return new CatLvRate5{Lv="47",Type="CLV",Exp=16740,Hp=664};
case 47: return new CatLvRate5{Lv="48",Type="CLV",Exp=17938,Hp=673};
case 48: return new CatLvRate5{Lv="49",Type="CLV",Exp=19210,Hp=682};
case 49: return new CatLvRate5{Lv="50",Type="CLV",Exp=20558,Hp=691};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}