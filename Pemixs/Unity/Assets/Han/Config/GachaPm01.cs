using UnityEngine;
namespace Remix{
	public class GachaPm01{
		public const int ID_COUNT = 3;

		public string ID {get; set;}
public string Type {get; set;}
public string Cat01 {get; set;}
public int Cat01Weights {get; set;}
public string Cat02 {get; set;}
public int Cat02Weights {get; set;}

		public static GachaPm01 Get(string key){
			switch (key) {
			case "Gacha01": return new GachaPm01{ID="Gacha01",Type="CA",Cat01="C01",Cat01Weights=83,Cat02="C04",Cat02Weights=17};
case "Gacha02": return new GachaPm01{ID="Gacha02",Type="CB",Cat01="C02",Cat01Weights=83,Cat02="C05",Cat02Weights=17};
case "Gacha03": return new GachaPm01{ID="Gacha03",Type="CC",Cat01="C03",Cat01Weights=83,Cat02="C06",Cat02Weights=17};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static GachaPm01 Get(int key){
			switch (key) {
			case 0: return new GachaPm01{ID="Gacha01",Type="CA",Cat01="C01",Cat01Weights=83,Cat02="C04",Cat02Weights=17};
case 1: return new GachaPm01{ID="Gacha02",Type="CB",Cat01="C02",Cat01Weights=83,Cat02="C05",Cat02Weights=17};
case 2: return new GachaPm01{ID="Gacha03",Type="CC",Cat01="C03",Cat01Weights=83,Cat02="C06",Cat02Weights=17};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}