using UnityEngine;
namespace Remix{
	public class CatLvRate4{
		public const int ID_COUNT = 50;

		public string Lv {get; set;}
public string Type {get; set;}
public int Exp {get; set;}
public int Hp {get; set;}

		public static CatLvRate4 Get(string key){
			switch (key) {
			case "1": return new CatLvRate4{Lv="1",Type="CLV",Exp=6,Hp=200};
case "2": return new CatLvRate4{Lv="2",Type="CLV",Exp=20,Hp=207};
case "3": return new CatLvRate4{Lv="3",Type="CLV",Exp=38,Hp=214};
case "4": return new CatLvRate4{Lv="4",Type="CLV",Exp=60,Hp=221};
case "5": return new CatLvRate4{Lv="5",Type="CLV",Exp=86,Hp=228};
case "6": return new CatLvRate4{Lv="6",Type="CLV",Exp=116,Hp=235};
case "7": return new CatLvRate4{Lv="7",Type="CLV",Exp=150,Hp=242};
case "8": return new CatLvRate4{Lv="8",Type="CLV",Exp=188,Hp=249};
case "9": return new CatLvRate4{Lv="9",Type="CLV",Exp=230,Hp=256};
case "10": return new CatLvRate4{Lv="10",Type="CLV",Exp=276,Hp=263};
case "11": return new CatLvRate4{Lv="11",Type="CLV",Exp=328,Hp=270};
case "12": return new CatLvRate4{Lv="12",Type="CLV",Exp=388,Hp=277};
case "13": return new CatLvRate4{Lv="13",Type="CLV",Exp=456,Hp=284};
case "14": return new CatLvRate4{Lv="14",Type="CLV",Exp=532,Hp=291};
case "15": return new CatLvRate4{Lv="15",Type="CLV",Exp=616,Hp=298};
case "16": return new CatLvRate4{Lv="16",Type="CLV",Exp=710,Hp=305};
case "17": return new CatLvRate4{Lv="17",Type="CLV",Exp=816,Hp=312};
case "18": return new CatLvRate4{Lv="18",Type="CLV",Exp=934,Hp=319};
case "19": return new CatLvRate4{Lv="19",Type="CLV",Exp=1064,Hp=326};
case "20": return new CatLvRate4{Lv="20",Type="CLV",Exp=1206,Hp=333};
case "21": return new CatLvRate4{Lv="21",Type="CLV",Exp=1362,Hp=340};
case "22": return new CatLvRate4{Lv="22",Type="CLV",Exp=1534,Hp=347};
case "23": return new CatLvRate4{Lv="23",Type="CLV",Exp=1722,Hp=354};
case "24": return new CatLvRate4{Lv="24",Type="CLV",Exp=1926,Hp=361};
case "25": return new CatLvRate4{Lv="25",Type="CLV",Exp=2146,Hp=368};
case "26": return new CatLvRate4{Lv="26",Type="CLV",Exp=2384,Hp=375};
case "27": return new CatLvRate4{Lv="27",Type="CLV",Exp=2642,Hp=382};
case "28": return new CatLvRate4{Lv="28",Type="CLV",Exp=2920,Hp=389};
case "29": return new CatLvRate4{Lv="29",Type="CLV",Exp=3218,Hp=396};
case "30": return new CatLvRate4{Lv="30",Type="CLV",Exp=3536,Hp=403};
case "31": return new CatLvRate4{Lv="31",Type="CLV",Exp=3876,Hp=410};
case "32": return new CatLvRate4{Lv="32",Type="CLV",Exp=4240,Hp=417};
case "33": return new CatLvRate4{Lv="33",Type="CLV",Exp=4628,Hp=424};
case "34": return new CatLvRate4{Lv="34",Type="CLV",Exp=5040,Hp=431};
case "35": return new CatLvRate4{Lv="35",Type="CLV",Exp=5476,Hp=438};
case "36": return new CatLvRate4{Lv="36",Type="CLV",Exp=5940,Hp=445};
case "37": return new CatLvRate4{Lv="37",Type="CLV",Exp=6436,Hp=452};
case "38": return new CatLvRate4{Lv="38",Type="CLV",Exp=6964,Hp=459};
case "39": return new CatLvRate4{Lv="39",Type="CLV",Exp=7524,Hp=466};
case "40": return new CatLvRate4{Lv="40",Type="CLV",Exp=8116,Hp=473};
case "41": return new CatLvRate4{Lv="41",Type="CLV",Exp=8744,Hp=480};
case "42": return new CatLvRate4{Lv="42",Type="CLV",Exp=9412,Hp=487};
case "43": return new CatLvRate4{Lv="43",Type="CLV",Exp=10120,Hp=494};
case "44": return new CatLvRate4{Lv="44",Type="CLV",Exp=10868,Hp=501};
case "45": return new CatLvRate4{Lv="45",Type="CLV",Exp=11656,Hp=508};
case "46": return new CatLvRate4{Lv="46",Type="CLV",Exp=12494,Hp=515};
case "47": return new CatLvRate4{Lv="47",Type="CLV",Exp=13392,Hp=522};
case "48": return new CatLvRate4{Lv="48",Type="CLV",Exp=14350,Hp=529};
case "49": return new CatLvRate4{Lv="49",Type="CLV",Exp=15368,Hp=536};
case "50": return new CatLvRate4{Lv="50",Type="CLV",Exp=16446,Hp=543};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static CatLvRate4 Get(int key){
			switch (key) {
			case 0: return new CatLvRate4{Lv="1",Type="CLV",Exp=6,Hp=200};
case 1: return new CatLvRate4{Lv="2",Type="CLV",Exp=20,Hp=207};
case 2: return new CatLvRate4{Lv="3",Type="CLV",Exp=38,Hp=214};
case 3: return new CatLvRate4{Lv="4",Type="CLV",Exp=60,Hp=221};
case 4: return new CatLvRate4{Lv="5",Type="CLV",Exp=86,Hp=228};
case 5: return new CatLvRate4{Lv="6",Type="CLV",Exp=116,Hp=235};
case 6: return new CatLvRate4{Lv="7",Type="CLV",Exp=150,Hp=242};
case 7: return new CatLvRate4{Lv="8",Type="CLV",Exp=188,Hp=249};
case 8: return new CatLvRate4{Lv="9",Type="CLV",Exp=230,Hp=256};
case 9: return new CatLvRate4{Lv="10",Type="CLV",Exp=276,Hp=263};
case 10: return new CatLvRate4{Lv="11",Type="CLV",Exp=328,Hp=270};
case 11: return new CatLvRate4{Lv="12",Type="CLV",Exp=388,Hp=277};
case 12: return new CatLvRate4{Lv="13",Type="CLV",Exp=456,Hp=284};
case 13: return new CatLvRate4{Lv="14",Type="CLV",Exp=532,Hp=291};
case 14: return new CatLvRate4{Lv="15",Type="CLV",Exp=616,Hp=298};
case 15: return new CatLvRate4{Lv="16",Type="CLV",Exp=710,Hp=305};
case 16: return new CatLvRate4{Lv="17",Type="CLV",Exp=816,Hp=312};
case 17: return new CatLvRate4{Lv="18",Type="CLV",Exp=934,Hp=319};
case 18: return new CatLvRate4{Lv="19",Type="CLV",Exp=1064,Hp=326};
case 19: return new CatLvRate4{Lv="20",Type="CLV",Exp=1206,Hp=333};
case 20: return new CatLvRate4{Lv="21",Type="CLV",Exp=1362,Hp=340};
case 21: return new CatLvRate4{Lv="22",Type="CLV",Exp=1534,Hp=347};
case 22: return new CatLvRate4{Lv="23",Type="CLV",Exp=1722,Hp=354};
case 23: return new CatLvRate4{Lv="24",Type="CLV",Exp=1926,Hp=361};
case 24: return new CatLvRate4{Lv="25",Type="CLV",Exp=2146,Hp=368};
case 25: return new CatLvRate4{Lv="26",Type="CLV",Exp=2384,Hp=375};
case 26: return new CatLvRate4{Lv="27",Type="CLV",Exp=2642,Hp=382};
case 27: return new CatLvRate4{Lv="28",Type="CLV",Exp=2920,Hp=389};
case 28: return new CatLvRate4{Lv="29",Type="CLV",Exp=3218,Hp=396};
case 29: return new CatLvRate4{Lv="30",Type="CLV",Exp=3536,Hp=403};
case 30: return new CatLvRate4{Lv="31",Type="CLV",Exp=3876,Hp=410};
case 31: return new CatLvRate4{Lv="32",Type="CLV",Exp=4240,Hp=417};
case 32: return new CatLvRate4{Lv="33",Type="CLV",Exp=4628,Hp=424};
case 33: return new CatLvRate4{Lv="34",Type="CLV",Exp=5040,Hp=431};
case 34: return new CatLvRate4{Lv="35",Type="CLV",Exp=5476,Hp=438};
case 35: return new CatLvRate4{Lv="36",Type="CLV",Exp=5940,Hp=445};
case 36: return new CatLvRate4{Lv="37",Type="CLV",Exp=6436,Hp=452};
case 37: return new CatLvRate4{Lv="38",Type="CLV",Exp=6964,Hp=459};
case 38: return new CatLvRate4{Lv="39",Type="CLV",Exp=7524,Hp=466};
case 39: return new CatLvRate4{Lv="40",Type="CLV",Exp=8116,Hp=473};
case 40: return new CatLvRate4{Lv="41",Type="CLV",Exp=8744,Hp=480};
case 41: return new CatLvRate4{Lv="42",Type="CLV",Exp=9412,Hp=487};
case 42: return new CatLvRate4{Lv="43",Type="CLV",Exp=10120,Hp=494};
case 43: return new CatLvRate4{Lv="44",Type="CLV",Exp=10868,Hp=501};
case 44: return new CatLvRate4{Lv="45",Type="CLV",Exp=11656,Hp=508};
case 45: return new CatLvRate4{Lv="46",Type="CLV",Exp=12494,Hp=515};
case 46: return new CatLvRate4{Lv="47",Type="CLV",Exp=13392,Hp=522};
case 47: return new CatLvRate4{Lv="48",Type="CLV",Exp=14350,Hp=529};
case 48: return new CatLvRate4{Lv="49",Type="CLV",Exp=15368,Hp=536};
case 49: return new CatLvRate4{Lv="50",Type="CLV",Exp=16446,Hp=543};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}