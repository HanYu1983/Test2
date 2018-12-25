using UnityEngine;
namespace Remix{
	public class WmapSCht{
		public const int ID_COUNT = 9;

		public string ID {get; set;}
public string Name {get; set;}
public string Desc {get; set;}

		public static WmapSCht Get(string key){
			switch (key) {
			case "Map-01": return new WmapSCht{ID="Map-01",Name="Text_MAPName-01",Desc="猴硐"};
case "Map-02": return new WmapSCht{ID="Map-02",Name="Text_MAPName-02",Desc="座間味"};
case "Map-03": return new WmapSCht{ID="Map-03",Name="Text_MAPName-03",Desc="渡嘉敷"};
case "Map-04": return new WmapSCht{ID="Map-04",Name="Text_MAPName-04",Desc="台鹼宿舍"};
case "Map-05": return new WmapSCht{ID="Map-05",Name="Text_MAPName-05",Desc="蘭嶼"};
case "Map-06": return new WmapSCht{ID="Map-06",Name="Text_MAPName-06",Desc="竹富"};
case "Map-S1": return new WmapSCht{ID="Map-S1",Name="Text_MAPName-S1",Desc="澎湖"};
case "Map-S2": return new WmapSCht{ID="Map-S2",Name="Text_MAPName-S2",Desc="那霸"};
case "Map-NS": return new WmapSCht{ID="Map-NS",Name="Text_2.0Name",Desc="北之島"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static WmapSCht Get(int key){
			switch (key) {
			case 0: return new WmapSCht{ID="Map-01",Name="Text_MAPName-01",Desc="猴硐"};
case 1: return new WmapSCht{ID="Map-02",Name="Text_MAPName-02",Desc="座間味"};
case 2: return new WmapSCht{ID="Map-03",Name="Text_MAPName-03",Desc="渡嘉敷"};
case 3: return new WmapSCht{ID="Map-04",Name="Text_MAPName-04",Desc="台鹼宿舍"};
case 4: return new WmapSCht{ID="Map-05",Name="Text_MAPName-05",Desc="蘭嶼"};
case 5: return new WmapSCht{ID="Map-06",Name="Text_MAPName-06",Desc="竹富"};
case 6: return new WmapSCht{ID="Map-S1",Name="Text_MAPName-S1",Desc="澎湖"};
case 7: return new WmapSCht{ID="Map-S2",Name="Text_MAPName-S2",Desc="那霸"};
case 8: return new WmapSCht{ID="Map-NS",Name="Text_2.0Name",Desc="北之島"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}