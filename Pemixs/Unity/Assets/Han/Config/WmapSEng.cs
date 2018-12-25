using UnityEngine;
namespace Remix{
	public class WmapSEng{
		public const int ID_COUNT = 9;

		public string ID {get; set;}
public string Name {get; set;}
public string Desc {get; set;}

		public static WmapSEng Get(string key){
			switch (key) {
			case "Map-01": return new WmapSEng{ID="Map-01",Name="Text_MAPName-01",Desc="Houtong"};
case "Map-02": return new WmapSEng{ID="Map-02",Name="Text_MAPName-02",Desc="Zamami"};
case "Map-03": return new WmapSEng{ID="Map-03",Name="Text_MAPName-03",Desc="Tokashiki"};
case "Map-04": return new WmapSEng{ID="Map-04",Name="Text_MAPName-04",Desc="Dorm of CPDC"};
case "Map-05": return new WmapSEng{ID="Map-05",Name="Text_MAPName-05",Desc="Orchid"};
case "Map-06": return new WmapSEng{ID="Map-06",Name="Text_MAPName-06",Desc="Taketomi"};
case "Map-S1": return new WmapSEng{ID="Map-S1",Name="Text_MAPName-S1",Desc="Penghu"};
case "Map-S2": return new WmapSEng{ID="Map-S2",Name="Text_MAPName-S2",Desc="Naha"};
case "Map-NS": return new WmapSEng{ID="Map-NS",Name="Text_2.0Name",Desc="North Island"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static WmapSEng Get(int key){
			switch (key) {
			case 0: return new WmapSEng{ID="Map-01",Name="Text_MAPName-01",Desc="Houtong"};
case 1: return new WmapSEng{ID="Map-02",Name="Text_MAPName-02",Desc="Zamami"};
case 2: return new WmapSEng{ID="Map-03",Name="Text_MAPName-03",Desc="Tokashiki"};
case 3: return new WmapSEng{ID="Map-04",Name="Text_MAPName-04",Desc="Dorm of CPDC"};
case 4: return new WmapSEng{ID="Map-05",Name="Text_MAPName-05",Desc="Orchid"};
case 5: return new WmapSEng{ID="Map-06",Name="Text_MAPName-06",Desc="Taketomi"};
case 6: return new WmapSEng{ID="Map-S1",Name="Text_MAPName-S1",Desc="Penghu"};
case 7: return new WmapSEng{ID="Map-S2",Name="Text_MAPName-S2",Desc="Naha"};
case 8: return new WmapSEng{ID="Map-NS",Name="Text_2.0Name",Desc="North Island"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}