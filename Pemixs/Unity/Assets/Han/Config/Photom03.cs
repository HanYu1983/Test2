using UnityEngine;
namespace Remix{
	public class Photom03{
		public const int ID_COUNT = 27;

		public string ID {get; set;}
public string Name {get; set;}
public string Type {get; set;}

		public static Photom03 Get(string key){
			switch (key) {
			case "SP03S01": return new Photom03{ID="SP03S01",Name="M03S01",Type="S"};
case "SP03S02": return new Photom03{ID="SP03S02",Name="M03S02",Type="S"};
case "SP03S03": return new Photom03{ID="SP03S03",Name="M03S03",Type="S"};
case "SP03S04": return new Photom03{ID="SP03S04",Name="M03S04",Type="S"};
case "SP03S05": return new Photom03{ID="SP03S05",Name="M03S05",Type="S"};
case "SP03S06": return new Photom03{ID="SP03S06",Name="M03S06",Type="S"};
case "SP03S07": return new Photom03{ID="SP03S07",Name="M03S07",Type="S"};
case "SP03S08": return new Photom03{ID="SP03S08",Name="M03S08",Type="S"};
case "SP03S09": return new Photom03{ID="SP03S09",Name="M03S09",Type="S"};
case "SP03S10": return new Photom03{ID="SP03S10",Name="M03S10",Type="S"};
case "SP03S11": return new Photom03{ID="SP03S11",Name="M03S11",Type="S"};
case "SP03S12": return new Photom03{ID="SP03S12",Name="M03S12",Type="S"};
case "SP03S13": return new Photom03{ID="SP03S13",Name="M03S13",Type="S"};
case "SP03S14": return new Photom03{ID="SP03S14",Name="M03S14",Type="S"};
case "SP03S15": return new Photom03{ID="SP03S15",Name="M03S15",Type="S"};
case "SP03S16": return new Photom03{ID="SP03S16",Name="M03S16",Type="S"};
case "SP03S17": return new Photom03{ID="SP03S17",Name="M03S17",Type="S"};
case "SP03S18": return new Photom03{ID="SP03S18",Name="M03S18",Type="S"};
case "SP03S19": return new Photom03{ID="SP03S19",Name="M03S19",Type="S"};
case "SP03S20": return new Photom03{ID="SP03S20",Name="M03S20",Type="S"};
case "SP03S21": return new Photom03{ID="SP03S21",Name="M03S21",Type="S"};
case "SP03S22": return new Photom03{ID="SP03S22",Name="M03S22",Type="S"};
case "SP03S23": return new Photom03{ID="SP03S23",Name="M03S23",Type="S"};
case "SP03S24": return new Photom03{ID="SP03S24",Name="M03S24",Type="S"};
case "BP03S01": return new Photom03{ID="BP03S01",Name="M03B01",Type="B"};
case "BP03S02": return new Photom03{ID="BP03S02",Name="M03B02",Type="B"};
case "BP03S03": return new Photom03{ID="BP03S03",Name="M03B03",Type="B"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static Photom03 Get(int key){
			switch (key) {
			case 0: return new Photom03{ID="SP03S01",Name="M03S01",Type="S"};
case 1: return new Photom03{ID="SP03S02",Name="M03S02",Type="S"};
case 2: return new Photom03{ID="SP03S03",Name="M03S03",Type="S"};
case 3: return new Photom03{ID="SP03S04",Name="M03S04",Type="S"};
case 4: return new Photom03{ID="SP03S05",Name="M03S05",Type="S"};
case 5: return new Photom03{ID="SP03S06",Name="M03S06",Type="S"};
case 6: return new Photom03{ID="SP03S07",Name="M03S07",Type="S"};
case 7: return new Photom03{ID="SP03S08",Name="M03S08",Type="S"};
case 8: return new Photom03{ID="SP03S09",Name="M03S09",Type="S"};
case 9: return new Photom03{ID="SP03S10",Name="M03S10",Type="S"};
case 10: return new Photom03{ID="SP03S11",Name="M03S11",Type="S"};
case 11: return new Photom03{ID="SP03S12",Name="M03S12",Type="S"};
case 12: return new Photom03{ID="SP03S13",Name="M03S13",Type="S"};
case 13: return new Photom03{ID="SP03S14",Name="M03S14",Type="S"};
case 14: return new Photom03{ID="SP03S15",Name="M03S15",Type="S"};
case 15: return new Photom03{ID="SP03S16",Name="M03S16",Type="S"};
case 16: return new Photom03{ID="SP03S17",Name="M03S17",Type="S"};
case 17: return new Photom03{ID="SP03S18",Name="M03S18",Type="S"};
case 18: return new Photom03{ID="SP03S19",Name="M03S19",Type="S"};
case 19: return new Photom03{ID="SP03S20",Name="M03S20",Type="S"};
case 20: return new Photom03{ID="SP03S21",Name="M03S21",Type="S"};
case 21: return new Photom03{ID="SP03S22",Name="M03S22",Type="S"};
case 22: return new Photom03{ID="SP03S23",Name="M03S23",Type="S"};
case 23: return new Photom03{ID="SP03S24",Name="M03S24",Type="S"};
case 24: return new Photom03{ID="BP03S01",Name="M03B01",Type="B"};
case 25: return new Photom03{ID="BP03S02",Name="M03B02",Type="B"};
case 26: return new Photom03{ID="BP03S03",Name="M03B03",Type="B"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}