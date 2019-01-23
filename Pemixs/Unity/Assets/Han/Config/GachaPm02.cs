using UnityEngine;
namespace Remix{
	public class GachaPm02{
		public const int ID_COUNT = 3;

		public string ID {get; set;}
public string Type {get; set;}
public string Cat01 {get; set;}
public int Cat01Weights {get; set;}
public string Cat02 {get; set;}
public int Cat02Weights {get; set;}

		public static GachaPm02 Get(string key){
			switch (key) {
			case "Gacha01": return new GachaPm02{ID="Gacha01",Type="CA",Cat01="C07",Cat01Weights=87,Cat02="C10",Cat02Weights=13};
case "Gacha02": return new GachaPm02{ID="Gacha02",Type="CB",Cat01="C08",Cat01Weights=87,Cat02="C11",Cat02Weights=13};
case "Gacha03": return new GachaPm02{ID="Gacha03",Type="CC",Cat01="C09",Cat01Weights=87,Cat02="C12",Cat02Weights=13};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static GachaPm02 Get(int key){
			switch (key) {
			case 0: return new GachaPm02{ID="Gacha01",Type="CA",Cat01="C07",Cat01Weights=87,Cat02="C10",Cat02Weights=13};
case 1: return new GachaPm02{ID="Gacha02",Type="CB",Cat01="C08",Cat01Weights=87,Cat02="C11",Cat02Weights=13};
case 2: return new GachaPm02{ID="Gacha03",Type="CC",Cat01="C09",Cat01Weights=87,Cat02="C12",Cat02Weights=13};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}