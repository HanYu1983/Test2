using UnityEngine;
namespace Remix{
	public class Photom01{
		public const int ID_COUNT = 39;

		public string ID {get; set;}
public string Name {get; set;}
public string Type {get; set;}

		public static Photom01 Get(string key){
			switch (key) {
			case "SP01S01": return new Photom01{ID="SP01S01",Name="M01S01",Type="S"};
case "SP01S02": return new Photom01{ID="SP01S02",Name="M01S02",Type="S"};
case "SP01S03": return new Photom01{ID="SP01S03",Name="M01S03",Type="S"};
case "SP01S04": return new Photom01{ID="SP01S04",Name="M01S04",Type="S"};
case "SP01S05": return new Photom01{ID="SP01S05",Name="M01S05",Type="S"};
case "SP01S06": return new Photom01{ID="SP01S06",Name="M01S06",Type="S"};
case "SP01S07": return new Photom01{ID="SP01S07",Name="M01S07",Type="S"};
case "SP01S08": return new Photom01{ID="SP01S08",Name="M01S08",Type="S"};
case "SP01S09": return new Photom01{ID="SP01S09",Name="M01S09",Type="S"};
case "SP01S10": return new Photom01{ID="SP01S10",Name="M01S10",Type="S"};
case "SP01S11": return new Photom01{ID="SP01S11",Name="M01S11",Type="S"};
case "SP01S12": return new Photom01{ID="SP01S12",Name="M01S12",Type="S"};
case "SP01S13": return new Photom01{ID="SP01S13",Name="M01S13",Type="S"};
case "SP01S14": return new Photom01{ID="SP01S14",Name="M01S14",Type="S"};
case "SP01S15": return new Photom01{ID="SP01S15",Name="M01S15",Type="S"};
case "SP01S16": return new Photom01{ID="SP01S16",Name="M01S16",Type="S"};
case "SP01S17": return new Photom01{ID="SP01S17",Name="M01S17",Type="S"};
case "SP01S18": return new Photom01{ID="SP01S18",Name="M01S18",Type="S"};
case "SP01S19": return new Photom01{ID="SP01S19",Name="M01S19",Type="S"};
case "SP01S20": return new Photom01{ID="SP01S20",Name="M01S20",Type="S"};
case "SP01S21": return new Photom01{ID="SP01S21",Name="M01S21",Type="S"};
case "SP01S22": return new Photom01{ID="SP01S22",Name="M01S22",Type="S"};
case "SP01S23": return new Photom01{ID="SP01S23",Name="M01S23",Type="S"};
case "SP01S24": return new Photom01{ID="SP01S24",Name="M01S24",Type="S"};
case "SP01S25": return new Photom01{ID="SP01S25",Name="M01S25",Type="S"};
case "SP01S26": return new Photom01{ID="SP01S26",Name="M01S26",Type="S"};
case "SP01S27": return new Photom01{ID="SP01S27",Name="M01S27",Type="S"};
case "SP01S28": return new Photom01{ID="SP01S28",Name="M01S28",Type="S"};
case "SP01S29": return new Photom01{ID="SP01S29",Name="M01S29",Type="S"};
case "SP01S30": return new Photom01{ID="SP01S30",Name="M01S30",Type="S"};
case "SP01S31": return new Photom01{ID="SP01S31",Name="M01S31",Type="S"};
case "SP01S32": return new Photom01{ID="SP01S32",Name="M01S32",Type="S"};
case "SP01S33": return new Photom01{ID="SP01S33",Name="M01S33",Type="S"};
case "SP01S34": return new Photom01{ID="SP01S34",Name="M01S34",Type="S"};
case "SP01S35": return new Photom01{ID="SP01S35",Name="M01S35",Type="S"};
case "SP01S36": return new Photom01{ID="SP01S36",Name="M01S36",Type="S"};
case "BP01S01": return new Photom01{ID="BP01S01",Name="M01B01",Type="B"};
case "BP01S02": return new Photom01{ID="BP01S02",Name="M01B02",Type="B"};
case "BP01S03": return new Photom01{ID="BP01S03",Name="M01B03",Type="B"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static Photom01 Get(int key){
			switch (key) {
			case 0: return new Photom01{ID="SP01S01",Name="M01S01",Type="S"};
case 1: return new Photom01{ID="SP01S02",Name="M01S02",Type="S"};
case 2: return new Photom01{ID="SP01S03",Name="M01S03",Type="S"};
case 3: return new Photom01{ID="SP01S04",Name="M01S04",Type="S"};
case 4: return new Photom01{ID="SP01S05",Name="M01S05",Type="S"};
case 5: return new Photom01{ID="SP01S06",Name="M01S06",Type="S"};
case 6: return new Photom01{ID="SP01S07",Name="M01S07",Type="S"};
case 7: return new Photom01{ID="SP01S08",Name="M01S08",Type="S"};
case 8: return new Photom01{ID="SP01S09",Name="M01S09",Type="S"};
case 9: return new Photom01{ID="SP01S10",Name="M01S10",Type="S"};
case 10: return new Photom01{ID="SP01S11",Name="M01S11",Type="S"};
case 11: return new Photom01{ID="SP01S12",Name="M01S12",Type="S"};
case 12: return new Photom01{ID="SP01S13",Name="M01S13",Type="S"};
case 13: return new Photom01{ID="SP01S14",Name="M01S14",Type="S"};
case 14: return new Photom01{ID="SP01S15",Name="M01S15",Type="S"};
case 15: return new Photom01{ID="SP01S16",Name="M01S16",Type="S"};
case 16: return new Photom01{ID="SP01S17",Name="M01S17",Type="S"};
case 17: return new Photom01{ID="SP01S18",Name="M01S18",Type="S"};
case 18: return new Photom01{ID="SP01S19",Name="M01S19",Type="S"};
case 19: return new Photom01{ID="SP01S20",Name="M01S20",Type="S"};
case 20: return new Photom01{ID="SP01S21",Name="M01S21",Type="S"};
case 21: return new Photom01{ID="SP01S22",Name="M01S22",Type="S"};
case 22: return new Photom01{ID="SP01S23",Name="M01S23",Type="S"};
case 23: return new Photom01{ID="SP01S24",Name="M01S24",Type="S"};
case 24: return new Photom01{ID="SP01S25",Name="M01S25",Type="S"};
case 25: return new Photom01{ID="SP01S26",Name="M01S26",Type="S"};
case 26: return new Photom01{ID="SP01S27",Name="M01S27",Type="S"};
case 27: return new Photom01{ID="SP01S28",Name="M01S28",Type="S"};
case 28: return new Photom01{ID="SP01S29",Name="M01S29",Type="S"};
case 29: return new Photom01{ID="SP01S30",Name="M01S30",Type="S"};
case 30: return new Photom01{ID="SP01S31",Name="M01S31",Type="S"};
case 31: return new Photom01{ID="SP01S32",Name="M01S32",Type="S"};
case 32: return new Photom01{ID="SP01S33",Name="M01S33",Type="S"};
case 33: return new Photom01{ID="SP01S34",Name="M01S34",Type="S"};
case 34: return new Photom01{ID="SP01S35",Name="M01S35",Type="S"};
case 35: return new Photom01{ID="SP01S36",Name="M01S36",Type="S"};
case 36: return new Photom01{ID="BP01S01",Name="M01B01",Type="B"};
case 37: return new Photom01{ID="BP01S02",Name="M01B02",Type="B"};
case 38: return new Photom01{ID="BP01S03",Name="M01B03",Type="B"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}