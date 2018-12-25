using UnityEngine;
namespace Remix{
	public class GachaPmS1{
		public const int ID_COUNT = 3;

		public string ID {get; set;}
public string Type {get; set;}
public string Cat01 {get; set;}
public int Cat01Weights {get; set;}
public string Cat02 {get; set;}
public int Cat02Weights {get; set;}
public string Cat03 {get; set;}
public int Cat03Weights {get; set;}

		public static GachaPmS1 Get(string key){
			switch (key) {
			case "Gacha01": return new GachaPmS1{ID="Gacha01",Type="CM",Cat01="CM1",Cat01Weights=90,Cat02="CM2",Cat02Weights=7,Cat03="CM3",Cat03Weights=2};
case "Gacha02": return new GachaPmS1{ID="Gacha02"};
case "Gacha03": return new GachaPmS1{ID="Gacha03"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static GachaPmS1 Get(int key){
			switch (key) {
			case 0: return new GachaPmS1{ID="Gacha01",Type="CM",Cat01="CM1",Cat01Weights=90,Cat02="CM2",Cat02Weights=7,Cat03="CM3",Cat03Weights=2};
case 1: return new GachaPmS1{ID="Gacha02"};
case 2: return new GachaPmS1{ID="Gacha03"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}