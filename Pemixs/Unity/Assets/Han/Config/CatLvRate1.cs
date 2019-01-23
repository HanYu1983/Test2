using UnityEngine;
namespace Remix{
	public class CatLvRate1{
		public const int ID_COUNT = 50;

		public string Lv {get; set;}
public string Type {get; set;}
public int Exp {get; set;}
public int Hp {get; set;}

		public static CatLvRate1 Get(string key){
			switch (key) {
			case "1": return new CatLvRate1{Lv="1",Type="CLV",Exp=3,Hp=100};
case "2": return new CatLvRate1{Lv="2",Type="CLV",Exp=10,Hp=103};
case "3": return new CatLvRate1{Lv="3",Type="CLV",Exp=19,Hp=106};
case "4": return new CatLvRate1{Lv="4",Type="CLV",Exp=30,Hp=109};
case "5": return new CatLvRate1{Lv="5",Type="CLV",Exp=43,Hp=112};
case "6": return new CatLvRate1{Lv="6",Type="CLV",Exp=58,Hp=115};
case "7": return new CatLvRate1{Lv="7",Type="CLV",Exp=75,Hp=118};
case "8": return new CatLvRate1{Lv="8",Type="CLV",Exp=94,Hp=121};
case "9": return new CatLvRate1{Lv="9",Type="CLV",Exp=115,Hp=124};
case "10": return new CatLvRate1{Lv="10",Type="CLV",Exp=138,Hp=127};
case "11": return new CatLvRate1{Lv="11",Type="CLV",Exp=164,Hp=130};
case "12": return new CatLvRate1{Lv="12",Type="CLV",Exp=194,Hp=133};
case "13": return new CatLvRate1{Lv="13",Type="CLV",Exp=228,Hp=136};
case "14": return new CatLvRate1{Lv="14",Type="CLV",Exp=266,Hp=139};
case "15": return new CatLvRate1{Lv="15",Type="CLV",Exp=308,Hp=142};
case "16": return new CatLvRate1{Lv="16",Type="CLV",Exp=355,Hp=145};
case "17": return new CatLvRate1{Lv="17",Type="CLV",Exp=408,Hp=148};
case "18": return new CatLvRate1{Lv="18",Type="CLV",Exp=467,Hp=151};
case "19": return new CatLvRate1{Lv="19",Type="CLV",Exp=532,Hp=154};
case "20": return new CatLvRate1{Lv="20",Type="CLV",Exp=603,Hp=157};
case "21": return new CatLvRate1{Lv="21",Type="CLV",Exp=681,Hp=160};
case "22": return new CatLvRate1{Lv="22",Type="CLV",Exp=767,Hp=163};
case "23": return new CatLvRate1{Lv="23",Type="CLV",Exp=861,Hp=166};
case "24": return new CatLvRate1{Lv="24",Type="CLV",Exp=963,Hp=169};
case "25": return new CatLvRate1{Lv="25",Type="CLV",Exp=1073,Hp=172};
case "26": return new CatLvRate1{Lv="26",Type="CLV",Exp=1192,Hp=175};
case "27": return new CatLvRate1{Lv="27",Type="CLV",Exp=1321,Hp=178};
case "28": return new CatLvRate1{Lv="28",Type="CLV",Exp=1460,Hp=181};
case "29": return new CatLvRate1{Lv="29",Type="CLV",Exp=1609,Hp=184};
case "30": return new CatLvRate1{Lv="30",Type="CLV",Exp=1768,Hp=187};
case "31": return new CatLvRate1{Lv="31",Type="CLV",Exp=1938,Hp=190};
case "32": return new CatLvRate1{Lv="32",Type="CLV",Exp=2120,Hp=193};
case "33": return new CatLvRate1{Lv="33",Type="CLV",Exp=2314,Hp=196};
case "34": return new CatLvRate1{Lv="34",Type="CLV",Exp=2520,Hp=199};
case "35": return new CatLvRate1{Lv="35",Type="CLV",Exp=2738,Hp=202};
case "36": return new CatLvRate1{Lv="36",Type="CLV",Exp=2970,Hp=205};
case "37": return new CatLvRate1{Lv="37",Type="CLV",Exp=3218,Hp=208};
case "38": return new CatLvRate1{Lv="38",Type="CLV",Exp=3482,Hp=211};
case "39": return new CatLvRate1{Lv="39",Type="CLV",Exp=3762,Hp=214};
case "40": return new CatLvRate1{Lv="40",Type="CLV",Exp=4058,Hp=217};
case "41": return new CatLvRate1{Lv="41",Type="CLV",Exp=4372,Hp=220};
case "42": return new CatLvRate1{Lv="42",Type="CLV",Exp=4706,Hp=223};
case "43": return new CatLvRate1{Lv="43",Type="CLV",Exp=5060,Hp=226};
case "44": return new CatLvRate1{Lv="44",Type="CLV",Exp=5434,Hp=229};
case "45": return new CatLvRate1{Lv="45",Type="CLV",Exp=5828,Hp=232};
case "46": return new CatLvRate1{Lv="46",Type="CLV",Exp=6247,Hp=235};
case "47": return new CatLvRate1{Lv="47",Type="CLV",Exp=6696,Hp=238};
case "48": return new CatLvRate1{Lv="48",Type="CLV",Exp=7175,Hp=241};
case "49": return new CatLvRate1{Lv="49",Type="CLV",Exp=7684,Hp=244};
case "50": return new CatLvRate1{Lv="50",Type="CLV",Exp=8223,Hp=247};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static CatLvRate1 Get(int key){
			switch (key) {
			case 0: return new CatLvRate1{Lv="1",Type="CLV",Exp=3,Hp=100};
case 1: return new CatLvRate1{Lv="2",Type="CLV",Exp=10,Hp=103};
case 2: return new CatLvRate1{Lv="3",Type="CLV",Exp=19,Hp=106};
case 3: return new CatLvRate1{Lv="4",Type="CLV",Exp=30,Hp=109};
case 4: return new CatLvRate1{Lv="5",Type="CLV",Exp=43,Hp=112};
case 5: return new CatLvRate1{Lv="6",Type="CLV",Exp=58,Hp=115};
case 6: return new CatLvRate1{Lv="7",Type="CLV",Exp=75,Hp=118};
case 7: return new CatLvRate1{Lv="8",Type="CLV",Exp=94,Hp=121};
case 8: return new CatLvRate1{Lv="9",Type="CLV",Exp=115,Hp=124};
case 9: return new CatLvRate1{Lv="10",Type="CLV",Exp=138,Hp=127};
case 10: return new CatLvRate1{Lv="11",Type="CLV",Exp=164,Hp=130};
case 11: return new CatLvRate1{Lv="12",Type="CLV",Exp=194,Hp=133};
case 12: return new CatLvRate1{Lv="13",Type="CLV",Exp=228,Hp=136};
case 13: return new CatLvRate1{Lv="14",Type="CLV",Exp=266,Hp=139};
case 14: return new CatLvRate1{Lv="15",Type="CLV",Exp=308,Hp=142};
case 15: return new CatLvRate1{Lv="16",Type="CLV",Exp=355,Hp=145};
case 16: return new CatLvRate1{Lv="17",Type="CLV",Exp=408,Hp=148};
case 17: return new CatLvRate1{Lv="18",Type="CLV",Exp=467,Hp=151};
case 18: return new CatLvRate1{Lv="19",Type="CLV",Exp=532,Hp=154};
case 19: return new CatLvRate1{Lv="20",Type="CLV",Exp=603,Hp=157};
case 20: return new CatLvRate1{Lv="21",Type="CLV",Exp=681,Hp=160};
case 21: return new CatLvRate1{Lv="22",Type="CLV",Exp=767,Hp=163};
case 22: return new CatLvRate1{Lv="23",Type="CLV",Exp=861,Hp=166};
case 23: return new CatLvRate1{Lv="24",Type="CLV",Exp=963,Hp=169};
case 24: return new CatLvRate1{Lv="25",Type="CLV",Exp=1073,Hp=172};
case 25: return new CatLvRate1{Lv="26",Type="CLV",Exp=1192,Hp=175};
case 26: return new CatLvRate1{Lv="27",Type="CLV",Exp=1321,Hp=178};
case 27: return new CatLvRate1{Lv="28",Type="CLV",Exp=1460,Hp=181};
case 28: return new CatLvRate1{Lv="29",Type="CLV",Exp=1609,Hp=184};
case 29: return new CatLvRate1{Lv="30",Type="CLV",Exp=1768,Hp=187};
case 30: return new CatLvRate1{Lv="31",Type="CLV",Exp=1938,Hp=190};
case 31: return new CatLvRate1{Lv="32",Type="CLV",Exp=2120,Hp=193};
case 32: return new CatLvRate1{Lv="33",Type="CLV",Exp=2314,Hp=196};
case 33: return new CatLvRate1{Lv="34",Type="CLV",Exp=2520,Hp=199};
case 34: return new CatLvRate1{Lv="35",Type="CLV",Exp=2738,Hp=202};
case 35: return new CatLvRate1{Lv="36",Type="CLV",Exp=2970,Hp=205};
case 36: return new CatLvRate1{Lv="37",Type="CLV",Exp=3218,Hp=208};
case 37: return new CatLvRate1{Lv="38",Type="CLV",Exp=3482,Hp=211};
case 38: return new CatLvRate1{Lv="39",Type="CLV",Exp=3762,Hp=214};
case 39: return new CatLvRate1{Lv="40",Type="CLV",Exp=4058,Hp=217};
case 40: return new CatLvRate1{Lv="41",Type="CLV",Exp=4372,Hp=220};
case 41: return new CatLvRate1{Lv="42",Type="CLV",Exp=4706,Hp=223};
case 42: return new CatLvRate1{Lv="43",Type="CLV",Exp=5060,Hp=226};
case 43: return new CatLvRate1{Lv="44",Type="CLV",Exp=5434,Hp=229};
case 44: return new CatLvRate1{Lv="45",Type="CLV",Exp=5828,Hp=232};
case 45: return new CatLvRate1{Lv="46",Type="CLV",Exp=6247,Hp=235};
case 46: return new CatLvRate1{Lv="47",Type="CLV",Exp=6696,Hp=238};
case 47: return new CatLvRate1{Lv="48",Type="CLV",Exp=7175,Hp=241};
case 48: return new CatLvRate1{Lv="49",Type="CLV",Exp=7684,Hp=244};
case 49: return new CatLvRate1{Lv="50",Type="CLV",Exp=8223,Hp=247};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}