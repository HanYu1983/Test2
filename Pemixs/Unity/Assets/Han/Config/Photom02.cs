using UnityEngine;
namespace Remix{
	public class Photom02{
		public const int ID_COUNT = 27;

		public string ID {get; set;}
public string Name {get; set;}
public string Type {get; set;}

		public static Photom02 Get(string key){
			switch (key) {
			case "SP02S01": return new Photom02{ID="SP02S01",Name="M02S01",Type="S"};
case "SP02S02": return new Photom02{ID="SP02S02",Name="M02S02",Type="S"};
case "SP02S03": return new Photom02{ID="SP02S03",Name="M02S03",Type="S"};
case "SP02S04": return new Photom02{ID="SP02S04",Name="M02S04",Type="S"};
case "SP02S05": return new Photom02{ID="SP02S05",Name="M02S05",Type="S"};
case "SP02S06": return new Photom02{ID="SP02S06",Name="M02S06",Type="S"};
case "SP02S07": return new Photom02{ID="SP02S07",Name="M02S07",Type="S"};
case "SP02S08": return new Photom02{ID="SP02S08",Name="M02S08",Type="S"};
case "SP02S09": return new Photom02{ID="SP02S09",Name="M02S09",Type="S"};
case "SP02S10": return new Photom02{ID="SP02S10",Name="M02S10",Type="S"};
case "SP02S11": return new Photom02{ID="SP02S11",Name="M02S11",Type="S"};
case "SP02S12": return new Photom02{ID="SP02S12",Name="M02S12",Type="S"};
case "SP02S13": return new Photom02{ID="SP02S13",Name="M02S13",Type="S"};
case "SP02S14": return new Photom02{ID="SP02S14",Name="M02S14",Type="S"};
case "SP02S15": return new Photom02{ID="SP02S15",Name="M02S15",Type="S"};
case "SP02S16": return new Photom02{ID="SP02S16",Name="M02S16",Type="S"};
case "SP02S17": return new Photom02{ID="SP02S17",Name="M02S17",Type="S"};
case "SP02S18": return new Photom02{ID="SP02S18",Name="M02S18",Type="S"};
case "SP02S19": return new Photom02{ID="SP02S19",Name="M02S19",Type="S"};
case "SP02S20": return new Photom02{ID="SP02S20",Name="M02S20",Type="S"};
case "SP02S21": return new Photom02{ID="SP02S21",Name="M02S21",Type="S"};
case "SP02S22": return new Photom02{ID="SP02S22",Name="M02S22",Type="S"};
case "SP02S23": return new Photom02{ID="SP02S23",Name="M02S23",Type="S"};
case "SP02S24": return new Photom02{ID="SP02S24",Name="M02S24",Type="S"};
case "BP02S01": return new Photom02{ID="BP02S01",Name="M02B01",Type="B"};
case "BP02S02": return new Photom02{ID="BP02S02",Name="M02B02",Type="B"};
case "BP02S03": return new Photom02{ID="BP02S03",Name="M02B03",Type="B"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static Photom02 Get(int key){
			switch (key) {
			case 0: return new Photom02{ID="SP02S01",Name="M02S01",Type="S"};
case 1: return new Photom02{ID="SP02S02",Name="M02S02",Type="S"};
case 2: return new Photom02{ID="SP02S03",Name="M02S03",Type="S"};
case 3: return new Photom02{ID="SP02S04",Name="M02S04",Type="S"};
case 4: return new Photom02{ID="SP02S05",Name="M02S05",Type="S"};
case 5: return new Photom02{ID="SP02S06",Name="M02S06",Type="S"};
case 6: return new Photom02{ID="SP02S07",Name="M02S07",Type="S"};
case 7: return new Photom02{ID="SP02S08",Name="M02S08",Type="S"};
case 8: return new Photom02{ID="SP02S09",Name="M02S09",Type="S"};
case 9: return new Photom02{ID="SP02S10",Name="M02S10",Type="S"};
case 10: return new Photom02{ID="SP02S11",Name="M02S11",Type="S"};
case 11: return new Photom02{ID="SP02S12",Name="M02S12",Type="S"};
case 12: return new Photom02{ID="SP02S13",Name="M02S13",Type="S"};
case 13: return new Photom02{ID="SP02S14",Name="M02S14",Type="S"};
case 14: return new Photom02{ID="SP02S15",Name="M02S15",Type="S"};
case 15: return new Photom02{ID="SP02S16",Name="M02S16",Type="S"};
case 16: return new Photom02{ID="SP02S17",Name="M02S17",Type="S"};
case 17: return new Photom02{ID="SP02S18",Name="M02S18",Type="S"};
case 18: return new Photom02{ID="SP02S19",Name="M02S19",Type="S"};
case 19: return new Photom02{ID="SP02S20",Name="M02S20",Type="S"};
case 20: return new Photom02{ID="SP02S21",Name="M02S21",Type="S"};
case 21: return new Photom02{ID="SP02S22",Name="M02S22",Type="S"};
case 22: return new Photom02{ID="SP02S23",Name="M02S23",Type="S"};
case 23: return new Photom02{ID="SP02S24",Name="M02S24",Type="S"};
case 24: return new Photom02{ID="BP02S01",Name="M02B01",Type="B"};
case 25: return new Photom02{ID="BP02S02",Name="M02B02",Type="B"};
case 26: return new Photom02{ID="BP02S03",Name="M02B03",Type="B"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}