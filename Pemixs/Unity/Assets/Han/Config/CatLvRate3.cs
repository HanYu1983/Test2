using UnityEngine;
namespace Remix{
	public class CatLvRate3{
		public const int ID_COUNT = 50;

		public string Lv {get; set;}
public string Type {get; set;}
public int Exp {get; set;}
public int Hp {get; set;}

		public static CatLvRate3 Get(string key){
			switch (key) {
			case "1": return new CatLvRate3{Lv="1",Type="CLV",Exp=5,Hp=160};
case "2": return new CatLvRate3{Lv="2",Type="CLV",Exp=16,Hp=165};
case "3": return new CatLvRate3{Lv="3",Type="CLV",Exp=30,Hp=170};
case "4": return new CatLvRate3{Lv="4",Type="CLV",Exp=48,Hp=175};
case "5": return new CatLvRate3{Lv="5",Type="CLV",Exp=69,Hp=180};
case "6": return new CatLvRate3{Lv="6",Type="CLV",Exp=93,Hp=185};
case "7": return new CatLvRate3{Lv="7",Type="CLV",Exp=120,Hp=190};
case "8": return new CatLvRate3{Lv="8",Type="CLV",Exp=150,Hp=195};
case "9": return new CatLvRate3{Lv="9",Type="CLV",Exp=184,Hp=200};
case "10": return new CatLvRate3{Lv="10",Type="CLV",Exp=221,Hp=205};
case "11": return new CatLvRate3{Lv="11",Type="CLV",Exp=262,Hp=210};
case "12": return new CatLvRate3{Lv="12",Type="CLV",Exp=310,Hp=215};
case "13": return new CatLvRate3{Lv="13",Type="CLV",Exp=365,Hp=220};
case "14": return new CatLvRate3{Lv="14",Type="CLV",Exp=426,Hp=225};
case "15": return new CatLvRate3{Lv="15",Type="CLV",Exp=493,Hp=230};
case "16": return new CatLvRate3{Lv="16",Type="CLV",Exp=568,Hp=235};
case "17": return new CatLvRate3{Lv="17",Type="CLV",Exp=653,Hp=240};
case "18": return new CatLvRate3{Lv="18",Type="CLV",Exp=747,Hp=245};
case "19": return new CatLvRate3{Lv="19",Type="CLV",Exp=851,Hp=250};
case "20": return new CatLvRate3{Lv="20",Type="CLV",Exp=965,Hp=255};
case "21": return new CatLvRate3{Lv="21",Type="CLV",Exp=1090,Hp=260};
case "22": return new CatLvRate3{Lv="22",Type="CLV",Exp=1227,Hp=265};
case "23": return new CatLvRate3{Lv="23",Type="CLV",Exp=1378,Hp=270};
case "24": return new CatLvRate3{Lv="24",Type="CLV",Exp=1541,Hp=275};
case "25": return new CatLvRate3{Lv="25",Type="CLV",Exp=1717,Hp=280};
case "26": return new CatLvRate3{Lv="26",Type="CLV",Exp=1907,Hp=285};
case "27": return new CatLvRate3{Lv="27",Type="CLV",Exp=2114,Hp=290};
case "28": return new CatLvRate3{Lv="28",Type="CLV",Exp=2336,Hp=295};
case "29": return new CatLvRate3{Lv="29",Type="CLV",Exp=2574,Hp=300};
case "30": return new CatLvRate3{Lv="30",Type="CLV",Exp=2829,Hp=305};
case "31": return new CatLvRate3{Lv="31",Type="CLV",Exp=3101,Hp=310};
case "32": return new CatLvRate3{Lv="32",Type="CLV",Exp=3392,Hp=315};
case "33": return new CatLvRate3{Lv="33",Type="CLV",Exp=3702,Hp=320};
case "34": return new CatLvRate3{Lv="34",Type="CLV",Exp=4032,Hp=325};
case "35": return new CatLvRate3{Lv="35",Type="CLV",Exp=4381,Hp=330};
case "36": return new CatLvRate3{Lv="36",Type="CLV",Exp=4752,Hp=335};
case "37": return new CatLvRate3{Lv="37",Type="CLV",Exp=5149,Hp=340};
case "38": return new CatLvRate3{Lv="38",Type="CLV",Exp=5571,Hp=345};
case "39": return new CatLvRate3{Lv="39",Type="CLV",Exp=6019,Hp=350};
case "40": return new CatLvRate3{Lv="40",Type="CLV",Exp=6493,Hp=355};
case "41": return new CatLvRate3{Lv="41",Type="CLV",Exp=6995,Hp=360};
case "42": return new CatLvRate3{Lv="42",Type="CLV",Exp=7530,Hp=365};
case "43": return new CatLvRate3{Lv="43",Type="CLV",Exp=8096,Hp=370};
case "44": return new CatLvRate3{Lv="44",Type="CLV",Exp=8694,Hp=375};
case "45": return new CatLvRate3{Lv="45",Type="CLV",Exp=9325,Hp=380};
case "46": return new CatLvRate3{Lv="46",Type="CLV",Exp=9995,Hp=385};
case "47": return new CatLvRate3{Lv="47",Type="CLV",Exp=10714,Hp=390};
case "48": return new CatLvRate3{Lv="48",Type="CLV",Exp=11480,Hp=395};
case "49": return new CatLvRate3{Lv="49",Type="CLV",Exp=12294,Hp=400};
case "50": return new CatLvRate3{Lv="50",Type="CLV",Exp=13157,Hp=405};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static CatLvRate3 Get(int key){
			switch (key) {
			case 0: return new CatLvRate3{Lv="1",Type="CLV",Exp=5,Hp=160};
case 1: return new CatLvRate3{Lv="2",Type="CLV",Exp=16,Hp=165};
case 2: return new CatLvRate3{Lv="3",Type="CLV",Exp=30,Hp=170};
case 3: return new CatLvRate3{Lv="4",Type="CLV",Exp=48,Hp=175};
case 4: return new CatLvRate3{Lv="5",Type="CLV",Exp=69,Hp=180};
case 5: return new CatLvRate3{Lv="6",Type="CLV",Exp=93,Hp=185};
case 6: return new CatLvRate3{Lv="7",Type="CLV",Exp=120,Hp=190};
case 7: return new CatLvRate3{Lv="8",Type="CLV",Exp=150,Hp=195};
case 8: return new CatLvRate3{Lv="9",Type="CLV",Exp=184,Hp=200};
case 9: return new CatLvRate3{Lv="10",Type="CLV",Exp=221,Hp=205};
case 10: return new CatLvRate3{Lv="11",Type="CLV",Exp=262,Hp=210};
case 11: return new CatLvRate3{Lv="12",Type="CLV",Exp=310,Hp=215};
case 12: return new CatLvRate3{Lv="13",Type="CLV",Exp=365,Hp=220};
case 13: return new CatLvRate3{Lv="14",Type="CLV",Exp=426,Hp=225};
case 14: return new CatLvRate3{Lv="15",Type="CLV",Exp=493,Hp=230};
case 15: return new CatLvRate3{Lv="16",Type="CLV",Exp=568,Hp=235};
case 16: return new CatLvRate3{Lv="17",Type="CLV",Exp=653,Hp=240};
case 17: return new CatLvRate3{Lv="18",Type="CLV",Exp=747,Hp=245};
case 18: return new CatLvRate3{Lv="19",Type="CLV",Exp=851,Hp=250};
case 19: return new CatLvRate3{Lv="20",Type="CLV",Exp=965,Hp=255};
case 20: return new CatLvRate3{Lv="21",Type="CLV",Exp=1090,Hp=260};
case 21: return new CatLvRate3{Lv="22",Type="CLV",Exp=1227,Hp=265};
case 22: return new CatLvRate3{Lv="23",Type="CLV",Exp=1378,Hp=270};
case 23: return new CatLvRate3{Lv="24",Type="CLV",Exp=1541,Hp=275};
case 24: return new CatLvRate3{Lv="25",Type="CLV",Exp=1717,Hp=280};
case 25: return new CatLvRate3{Lv="26",Type="CLV",Exp=1907,Hp=285};
case 26: return new CatLvRate3{Lv="27",Type="CLV",Exp=2114,Hp=290};
case 27: return new CatLvRate3{Lv="28",Type="CLV",Exp=2336,Hp=295};
case 28: return new CatLvRate3{Lv="29",Type="CLV",Exp=2574,Hp=300};
case 29: return new CatLvRate3{Lv="30",Type="CLV",Exp=2829,Hp=305};
case 30: return new CatLvRate3{Lv="31",Type="CLV",Exp=3101,Hp=310};
case 31: return new CatLvRate3{Lv="32",Type="CLV",Exp=3392,Hp=315};
case 32: return new CatLvRate3{Lv="33",Type="CLV",Exp=3702,Hp=320};
case 33: return new CatLvRate3{Lv="34",Type="CLV",Exp=4032,Hp=325};
case 34: return new CatLvRate3{Lv="35",Type="CLV",Exp=4381,Hp=330};
case 35: return new CatLvRate3{Lv="36",Type="CLV",Exp=4752,Hp=335};
case 36: return new CatLvRate3{Lv="37",Type="CLV",Exp=5149,Hp=340};
case 37: return new CatLvRate3{Lv="38",Type="CLV",Exp=5571,Hp=345};
case 38: return new CatLvRate3{Lv="39",Type="CLV",Exp=6019,Hp=350};
case 39: return new CatLvRate3{Lv="40",Type="CLV",Exp=6493,Hp=355};
case 40: return new CatLvRate3{Lv="41",Type="CLV",Exp=6995,Hp=360};
case 41: return new CatLvRate3{Lv="42",Type="CLV",Exp=7530,Hp=365};
case 42: return new CatLvRate3{Lv="43",Type="CLV",Exp=8096,Hp=370};
case 43: return new CatLvRate3{Lv="44",Type="CLV",Exp=8694,Hp=375};
case 44: return new CatLvRate3{Lv="45",Type="CLV",Exp=9325,Hp=380};
case 45: return new CatLvRate3{Lv="46",Type="CLV",Exp=9995,Hp=385};
case 46: return new CatLvRate3{Lv="47",Type="CLV",Exp=10714,Hp=390};
case 47: return new CatLvRate3{Lv="48",Type="CLV",Exp=11480,Hp=395};
case 48: return new CatLvRate3{Lv="49",Type="CLV",Exp=12294,Hp=400};
case 49: return new CatLvRate3{Lv="50",Type="CLV",Exp=13157,Hp=405};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}