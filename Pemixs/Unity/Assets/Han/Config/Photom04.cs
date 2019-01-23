using UnityEngine;
namespace Remix{
	public class Photom04{
		public const int ID_COUNT = 33;

		public string ID {get; set;}
public string Name {get; set;}
public string Type {get; set;}

		public static Photom04 Get(string key){
			switch (key) {
			case "SP04S01": return new Photom04{ID="SP04S01",Name="M04S01",Type="S"};
case "SP04S02": return new Photom04{ID="SP04S02",Name="M04S02",Type="S"};
case "SP04S03": return new Photom04{ID="SP04S03",Name="M04S03",Type="S"};
case "SP04S04": return new Photom04{ID="SP04S04",Name="M04S04",Type="S"};
case "SP04S05": return new Photom04{ID="SP04S05",Name="M04S05",Type="S"};
case "SP04S06": return new Photom04{ID="SP04S06",Name="M04S06",Type="S"};
case "SP04S07": return new Photom04{ID="SP04S07",Name="M04S07",Type="S"};
case "SP04S08": return new Photom04{ID="SP04S08",Name="M04S08",Type="S"};
case "SP04S09": return new Photom04{ID="SP04S09",Name="M04S09",Type="S"};
case "SP04S10": return new Photom04{ID="SP04S10",Name="M04S10",Type="S"};
case "SP04S11": return new Photom04{ID="SP04S11",Name="M04S11",Type="S"};
case "SP04S12": return new Photom04{ID="SP04S12",Name="M04S12",Type="S"};
case "SP04S13": return new Photom04{ID="SP04S13",Name="M04S13",Type="S"};
case "SP04S14": return new Photom04{ID="SP04S14",Name="M04S14",Type="S"};
case "SP04S15": return new Photom04{ID="SP04S15",Name="M04S15",Type="S"};
case "SP04S16": return new Photom04{ID="SP04S16",Name="M04S16",Type="S"};
case "SP04S17": return new Photom04{ID="SP04S17",Name="M04S17",Type="S"};
case "SP04S18": return new Photom04{ID="SP04S18",Name="M04S18",Type="S"};
case "SP04S19": return new Photom04{ID="SP04S19",Name="M04S19",Type="S"};
case "SP04S20": return new Photom04{ID="SP04S20",Name="M04S20",Type="S"};
case "SP04S21": return new Photom04{ID="SP04S21",Name="M04S21",Type="S"};
case "SP04S22": return new Photom04{ID="SP04S22",Name="M04S22",Type="S"};
case "SP04S23": return new Photom04{ID="SP04S23",Name="M04S23",Type="S"};
case "SP04S24": return new Photom04{ID="SP04S24",Name="M04S24",Type="S"};
case "SP04S25": return new Photom04{ID="SP04S25",Name="M04S25",Type="S"};
case "SP04S26": return new Photom04{ID="SP04S26",Name="M04S26",Type="S"};
case "SP04S27": return new Photom04{ID="SP04S27",Name="M04S27",Type="S"};
case "SP04S28": return new Photom04{ID="SP04S28",Name="M04S28",Type="S"};
case "SP04S29": return new Photom04{ID="SP04S29",Name="M04S29",Type="S"};
case "SP04S30": return new Photom04{ID="SP04S30",Name="M04S30",Type="S"};
case "BP04S01": return new Photom04{ID="BP04S01",Name="M04B01",Type="B"};
case "BP04S02": return new Photom04{ID="BP04S02",Name="M04B02",Type="B"};
case "BP04S03": return new Photom04{ID="BP04S03",Name="M04B03",Type="B"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static Photom04 Get(int key){
			switch (key) {
			case 0: return new Photom04{ID="SP04S01",Name="M04S01",Type="S"};
case 1: return new Photom04{ID="SP04S02",Name="M04S02",Type="S"};
case 2: return new Photom04{ID="SP04S03",Name="M04S03",Type="S"};
case 3: return new Photom04{ID="SP04S04",Name="M04S04",Type="S"};
case 4: return new Photom04{ID="SP04S05",Name="M04S05",Type="S"};
case 5: return new Photom04{ID="SP04S06",Name="M04S06",Type="S"};
case 6: return new Photom04{ID="SP04S07",Name="M04S07",Type="S"};
case 7: return new Photom04{ID="SP04S08",Name="M04S08",Type="S"};
case 8: return new Photom04{ID="SP04S09",Name="M04S09",Type="S"};
case 9: return new Photom04{ID="SP04S10",Name="M04S10",Type="S"};
case 10: return new Photom04{ID="SP04S11",Name="M04S11",Type="S"};
case 11: return new Photom04{ID="SP04S12",Name="M04S12",Type="S"};
case 12: return new Photom04{ID="SP04S13",Name="M04S13",Type="S"};
case 13: return new Photom04{ID="SP04S14",Name="M04S14",Type="S"};
case 14: return new Photom04{ID="SP04S15",Name="M04S15",Type="S"};
case 15: return new Photom04{ID="SP04S16",Name="M04S16",Type="S"};
case 16: return new Photom04{ID="SP04S17",Name="M04S17",Type="S"};
case 17: return new Photom04{ID="SP04S18",Name="M04S18",Type="S"};
case 18: return new Photom04{ID="SP04S19",Name="M04S19",Type="S"};
case 19: return new Photom04{ID="SP04S20",Name="M04S20",Type="S"};
case 20: return new Photom04{ID="SP04S21",Name="M04S21",Type="S"};
case 21: return new Photom04{ID="SP04S22",Name="M04S22",Type="S"};
case 22: return new Photom04{ID="SP04S23",Name="M04S23",Type="S"};
case 23: return new Photom04{ID="SP04S24",Name="M04S24",Type="S"};
case 24: return new Photom04{ID="SP04S25",Name="M04S25",Type="S"};
case 25: return new Photom04{ID="SP04S26",Name="M04S26",Type="S"};
case 26: return new Photom04{ID="SP04S27",Name="M04S27",Type="S"};
case 27: return new Photom04{ID="SP04S28",Name="M04S28",Type="S"};
case 28: return new Photom04{ID="SP04S29",Name="M04S29",Type="S"};
case 29: return new Photom04{ID="SP04S30",Name="M04S30",Type="S"};
case 30: return new Photom04{ID="BP04S01",Name="M04B01",Type="B"};
case 31: return new Photom04{ID="BP04S02",Name="M04B02",Type="B"};
case 32: return new Photom04{ID="BP04S03",Name="M04B03",Type="B"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}