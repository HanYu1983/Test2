using UnityEngine;
namespace Remix{
	public class GachaPm04{
		public const int ID_COUNT = 3;

		public string ID {get; set;}
public string Type {get; set;}
public string Cat01 {get; set;}
public int Cat01Weights {get; set;}
public string Cat02 {get; set;}
public int Cat02Weights {get; set;}

		public static GachaPm04 Get(string key){
			switch (key) {
			case "Gacha01": return new GachaPm04{ID="Gacha01",Type="CA",Cat01="C19",Cat01Weights=92,Cat02="C22",Cat02Weights=8};
case "Gacha02": return new GachaPm04{ID="Gacha02",Type="CB",Cat01="C20",Cat01Weights=92,Cat02="C23",Cat02Weights=8};
case "Gacha03": return new GachaPm04{ID="Gacha03",Type="CC",Cat01="C21",Cat01Weights=92,Cat02="C24",Cat02Weights=8};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static GachaPm04 Get(int key){
			switch (key) {
			case 0: return new GachaPm04{ID="Gacha01",Type="CA",Cat01="C19",Cat01Weights=92,Cat02="C22",Cat02Weights=8};
case 1: return new GachaPm04{ID="Gacha02",Type="CB",Cat01="C20",Cat01Weights=92,Cat02="C23",Cat02Weights=8};
case 2: return new GachaPm04{ID="Gacha03",Type="CC",Cat01="C21",Cat01Weights=92,Cat02="C24",Cat02Weights=8};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}