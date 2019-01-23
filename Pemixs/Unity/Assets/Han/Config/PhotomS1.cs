using UnityEngine;
namespace Remix{
	public class PhotomS1{
		public const int ID_COUNT = 42;

		public string ID {get; set;}
public string Name {get; set;}
public string Type {get; set;}

		public static PhotomS1 Get(string key){
			switch (key) {
			case "SPS1S01": return new PhotomS1{ID="SPS1S01",Name="MS1S01",Type="S"};
case "SPS1S02": return new PhotomS1{ID="SPS1S02",Name="MS1S02",Type="S"};
case "SPS1S03": return new PhotomS1{ID="SPS1S03",Name="MS1S03",Type="S"};
case "SPS1S04": return new PhotomS1{ID="SPS1S04",Name="MS1S04",Type="S"};
case "SPS1S05": return new PhotomS1{ID="SPS1S05",Name="MS1S05",Type="S"};
case "SPS1S06": return new PhotomS1{ID="SPS1S06",Name="MS1S06",Type="S"};
case "SPS1S07": return new PhotomS1{ID="SPS1S07",Name="MS1S07",Type="S"};
case "SPS1S08": return new PhotomS1{ID="SPS1S08",Name="MS1S08",Type="S"};
case "SPS1S09": return new PhotomS1{ID="SPS1S09",Name="MS1S09",Type="S"};
case "SPS1S10": return new PhotomS1{ID="SPS1S10",Name="MS1S10",Type="S"};
case "SPS1S11": return new PhotomS1{ID="SPS1S11",Name="MS1S11",Type="S"};
case "SPS1S12": return new PhotomS1{ID="SPS1S12",Name="MS1S12",Type="S"};
case "SPS1S13": return new PhotomS1{ID="SPS1S13",Name="MS1S13",Type="S"};
case "SPS1S14": return new PhotomS1{ID="SPS1S14",Name="MS1S14",Type="S"};
case "SPS1S15": return new PhotomS1{ID="SPS1S15",Name="MS1S15",Type="S"};
case "SPS1S16": return new PhotomS1{ID="SPS1S16",Name="MS1S16",Type="S"};
case "SPS1S17": return new PhotomS1{ID="SPS1S17",Name="MS1S17",Type="S"};
case "SPS1S18": return new PhotomS1{ID="SPS1S18",Name="MS1S18",Type="S"};
case "SPS1S19": return new PhotomS1{ID="SPS1S19",Name="MS1S19",Type="S"};
case "SPS1S20": return new PhotomS1{ID="SPS1S20",Name="MS1S20",Type="S"};
case "SPS1S21": return new PhotomS1{ID="SPS1S21",Name="MS1S21",Type="S"};
case "SPS1S22": return new PhotomS1{ID="SPS1S22",Name="MS1S22",Type="S"};
case "SPS1S23": return new PhotomS1{ID="SPS1S23",Name="MS1S23",Type="S"};
case "SPS1S24": return new PhotomS1{ID="SPS1S24",Name="MS1S24",Type="S"};
case "SPS1S25": return new PhotomS1{ID="SPS1S25",Name="MS1S25",Type="S"};
case "SPS1S26": return new PhotomS1{ID="SPS1S26",Name="MS1S26",Type="S"};
case "SPS1S27": return new PhotomS1{ID="SPS1S27",Name="MS1S27",Type="S"};
case "SPS1S28": return new PhotomS1{ID="SPS1S28",Name="MS1S28",Type="S"};
case "SPS1S29": return new PhotomS1{ID="SPS1S29",Name="MS1S29",Type="S"};
case "SPS1S30": return new PhotomS1{ID="SPS1S30",Name="MS1S30",Type="S"};
case "SPS1S31": return new PhotomS1{ID="SPS1S31",Name="MS1S31",Type="S"};
case "SPS1S32": return new PhotomS1{ID="SPS1S32",Name="MS1S32",Type="S"};
case "SPS1S33": return new PhotomS1{ID="SPS1S33",Name="MS1S33",Type="S"};
case "SPS1S34": return new PhotomS1{ID="SPS1S34",Name="MS1S34",Type="S"};
case "SPS1S35": return new PhotomS1{ID="SPS1S35",Name="MS1S35",Type="S"};
case "SPS1S36": return new PhotomS1{ID="SPS1S36",Name="MS1S36",Type="S"};
case "BPS1S01": return new PhotomS1{ID="BPS1S01",Name="MS1B01",Type="B"};
case "BPS1S02": return new PhotomS1{ID="BPS1S02",Name="MS1B02",Type="B"};
case "BPS1S03": return new PhotomS1{ID="BPS1S03",Name="MS1B03",Type="B"};
case "BPS1S04": return new PhotomS1{ID="BPS1S04",Name="MS1B04",Type="B"};
case "BPS1S05": return new PhotomS1{ID="BPS1S05",Name="MS1B05",Type="B"};
case "BPS1S06": return new PhotomS1{ID="BPS1S06",Name="MS1B06",Type="B"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static PhotomS1 Get(int key){
			switch (key) {
			case 0: return new PhotomS1{ID="SPS1S01",Name="MS1S01",Type="S"};
case 1: return new PhotomS1{ID="SPS1S02",Name="MS1S02",Type="S"};
case 2: return new PhotomS1{ID="SPS1S03",Name="MS1S03",Type="S"};
case 3: return new PhotomS1{ID="SPS1S04",Name="MS1S04",Type="S"};
case 4: return new PhotomS1{ID="SPS1S05",Name="MS1S05",Type="S"};
case 5: return new PhotomS1{ID="SPS1S06",Name="MS1S06",Type="S"};
case 6: return new PhotomS1{ID="SPS1S07",Name="MS1S07",Type="S"};
case 7: return new PhotomS1{ID="SPS1S08",Name="MS1S08",Type="S"};
case 8: return new PhotomS1{ID="SPS1S09",Name="MS1S09",Type="S"};
case 9: return new PhotomS1{ID="SPS1S10",Name="MS1S10",Type="S"};
case 10: return new PhotomS1{ID="SPS1S11",Name="MS1S11",Type="S"};
case 11: return new PhotomS1{ID="SPS1S12",Name="MS1S12",Type="S"};
case 12: return new PhotomS1{ID="SPS1S13",Name="MS1S13",Type="S"};
case 13: return new PhotomS1{ID="SPS1S14",Name="MS1S14",Type="S"};
case 14: return new PhotomS1{ID="SPS1S15",Name="MS1S15",Type="S"};
case 15: return new PhotomS1{ID="SPS1S16",Name="MS1S16",Type="S"};
case 16: return new PhotomS1{ID="SPS1S17",Name="MS1S17",Type="S"};
case 17: return new PhotomS1{ID="SPS1S18",Name="MS1S18",Type="S"};
case 18: return new PhotomS1{ID="SPS1S19",Name="MS1S19",Type="S"};
case 19: return new PhotomS1{ID="SPS1S20",Name="MS1S20",Type="S"};
case 20: return new PhotomS1{ID="SPS1S21",Name="MS1S21",Type="S"};
case 21: return new PhotomS1{ID="SPS1S22",Name="MS1S22",Type="S"};
case 22: return new PhotomS1{ID="SPS1S23",Name="MS1S23",Type="S"};
case 23: return new PhotomS1{ID="SPS1S24",Name="MS1S24",Type="S"};
case 24: return new PhotomS1{ID="SPS1S25",Name="MS1S25",Type="S"};
case 25: return new PhotomS1{ID="SPS1S26",Name="MS1S26",Type="S"};
case 26: return new PhotomS1{ID="SPS1S27",Name="MS1S27",Type="S"};
case 27: return new PhotomS1{ID="SPS1S28",Name="MS1S28",Type="S"};
case 28: return new PhotomS1{ID="SPS1S29",Name="MS1S29",Type="S"};
case 29: return new PhotomS1{ID="SPS1S30",Name="MS1S30",Type="S"};
case 30: return new PhotomS1{ID="SPS1S31",Name="MS1S31",Type="S"};
case 31: return new PhotomS1{ID="SPS1S32",Name="MS1S32",Type="S"};
case 32: return new PhotomS1{ID="SPS1S33",Name="MS1S33",Type="S"};
case 33: return new PhotomS1{ID="SPS1S34",Name="MS1S34",Type="S"};
case 34: return new PhotomS1{ID="SPS1S35",Name="MS1S35",Type="S"};
case 35: return new PhotomS1{ID="SPS1S36",Name="MS1S36",Type="S"};
case 36: return new PhotomS1{ID="BPS1S01",Name="MS1B01",Type="B"};
case 37: return new PhotomS1{ID="BPS1S02",Name="MS1B02",Type="B"};
case 38: return new PhotomS1{ID="BPS1S03",Name="MS1B03",Type="B"};
case 39: return new PhotomS1{ID="BPS1S04",Name="MS1B04",Type="B"};
case 40: return new PhotomS1{ID="BPS1S05",Name="MS1B05",Type="B"};
case 41: return new PhotomS1{ID="BPS1S06",Name="MS1B06",Type="B"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}