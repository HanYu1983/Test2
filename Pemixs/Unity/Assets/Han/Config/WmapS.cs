using UnityEngine;
namespace Remix{
	public class WmapS{
		public const int ID_COUNT = 24;

		public string ID {get; set;}
public string Item {get; set;}
public int Quantity {get; set;}

		public static WmapS Get(string key){
			switch (key) {
			case "Map-01E": return new WmapS{ID="Map-01E",Item="I41050",Quantity=1};
case "Map-01N": return new WmapS{ID="Map-01N",Item="I41040",Quantity=1};
case "Map-01H": return new WmapS{ID="Map-01H",Item="I41030",Quantity=1};
case "Map-02E": return new WmapS{ID="Map-02E",Item="I41050",Quantity=1};
case "Map-02N": return new WmapS{ID="Map-02N",Item="I41040",Quantity=1};
case "Map-02H": return new WmapS{ID="Map-02H",Item="I41030",Quantity=1};
case "Map-03E": return new WmapS{ID="Map-03E",Item="I41050",Quantity=1};
case "Map-03N": return new WmapS{ID="Map-03N",Item="I41040",Quantity=1};
case "Map-03H": return new WmapS{ID="Map-03H",Item="I41030",Quantity=1};
case "Map-04E": return new WmapS{ID="Map-04E",Item="I41050",Quantity=1};
case "Map-04N": return new WmapS{ID="Map-04N",Item="I41040",Quantity=1};
case "Map-04H": return new WmapS{ID="Map-04H",Item="I41030",Quantity=1};
case "Map-05E": return new WmapS{ID="Map-05E",Item="I41050",Quantity=1};
case "Map-05N": return new WmapS{ID="Map-05N",Item="I41040",Quantity=1};
case "Map-05H": return new WmapS{ID="Map-05H",Item="I41030",Quantity=1};
case "Map-06E": return new WmapS{ID="Map-06E",Item="I41050",Quantity=1};
case "Map-06N": return new WmapS{ID="Map-06N",Item="I41040",Quantity=1};
case "Map-06H": return new WmapS{ID="Map-06H",Item="I41030",Quantity=1};
case "Map-S1E": return new WmapS{ID="Map-S1E",Item="I60020",Quantity=1};
case "Map-S1N": return new WmapS{ID="Map-S1N",Item="I50030",Quantity=1};
case "Map-S1H": return new WmapS{ID="Map-S1H",Item="I62010",Quantity=1};
case "Map-S2E": return new WmapS{ID="Map-S2E",Item="I41040",Quantity=1};
case "Map-S2N": return new WmapS{ID="Map-S2N",Item="I41030",Quantity=1};
case "Map-S2H": return new WmapS{ID="Map-S2H",Item="I41010",Quantity=1};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static WmapS Get(int key){
			switch (key) {
			case 0: return new WmapS{ID="Map-01E",Item="I41050",Quantity=1};
case 1: return new WmapS{ID="Map-01N",Item="I41040",Quantity=1};
case 2: return new WmapS{ID="Map-01H",Item="I41030",Quantity=1};
case 3: return new WmapS{ID="Map-02E",Item="I41050",Quantity=1};
case 4: return new WmapS{ID="Map-02N",Item="I41040",Quantity=1};
case 5: return new WmapS{ID="Map-02H",Item="I41030",Quantity=1};
case 6: return new WmapS{ID="Map-03E",Item="I41050",Quantity=1};
case 7: return new WmapS{ID="Map-03N",Item="I41040",Quantity=1};
case 8: return new WmapS{ID="Map-03H",Item="I41030",Quantity=1};
case 9: return new WmapS{ID="Map-04E",Item="I41050",Quantity=1};
case 10: return new WmapS{ID="Map-04N",Item="I41040",Quantity=1};
case 11: return new WmapS{ID="Map-04H",Item="I41030",Quantity=1};
case 12: return new WmapS{ID="Map-05E",Item="I41050",Quantity=1};
case 13: return new WmapS{ID="Map-05N",Item="I41040",Quantity=1};
case 14: return new WmapS{ID="Map-05H",Item="I41030",Quantity=1};
case 15: return new WmapS{ID="Map-06E",Item="I41050",Quantity=1};
case 16: return new WmapS{ID="Map-06N",Item="I41040",Quantity=1};
case 17: return new WmapS{ID="Map-06H",Item="I41030",Quantity=1};
case 18: return new WmapS{ID="Map-S1E",Item="I60020",Quantity=1};
case 19: return new WmapS{ID="Map-S1N",Item="I50030",Quantity=1};
case 20: return new WmapS{ID="Map-S1H",Item="I62010",Quantity=1};
case 21: return new WmapS{ID="Map-S2E",Item="I41040",Quantity=1};
case 22: return new WmapS{ID="Map-S2N",Item="I41030",Quantity=1};
case 23: return new WmapS{ID="Map-S2H",Item="I41010",Quantity=1};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}