using UnityEngine;
namespace Remix{
	public class GachaPm03{
		public const int ID_COUNT = 3;

		public string ID {get; set;}
public string Type {get; set;}
public string Cat01 {get; set;}
public int Cat01Weights {get; set;}
public string Cat02 {get; set;}
public int Cat02Weights {get; set;}

		public static GachaPm03 Get(string key){
			switch (key) {
			case "Gacha01": return new GachaPm03{ID="Gacha01",Type="CA",Cat01="C13",Cat01Weights=90,Cat02="C16",Cat02Weights=10};
case "Gacha02": return new GachaPm03{ID="Gacha02",Type="CB",Cat01="C14",Cat01Weights=90,Cat02="C17",Cat02Weights=10};
case "Gacha03": return new GachaPm03{ID="Gacha03",Type="CC",Cat01="C15",Cat01Weights=90,Cat02="C18",Cat02Weights=10};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static GachaPm03 Get(int key){
			switch (key) {
			case 0: return new GachaPm03{ID="Gacha01",Type="CA",Cat01="C13",Cat01Weights=90,Cat02="C16",Cat02Weights=10};
case 1: return new GachaPm03{ID="Gacha02",Type="CB",Cat01="C14",Cat01Weights=90,Cat02="C17",Cat02Weights=10};
case 2: return new GachaPm03{ID="Gacha03",Type="CC",Cat01="C15",Cat01Weights=90,Cat02="C18",Cat02Weights=10};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}