using UnityEngine;
namespace Remix{
	public class WmapSChs{
		public const int ID_COUNT = 9;

		public string ID {get; set;}
public string Name {get; set;}
public string Desc {get; set;}

		public static WmapSChs Get(string key){
			switch (key) {
			case "Map-01": return new WmapSChs{ID="Map-01",Name="Text_MAPName-01",Desc="猴硐"};
case "Map-02": return new WmapSChs{ID="Map-02",Name="Text_MAPName-02",Desc="座间味"};
case "Map-03": return new WmapSChs{ID="Map-03",Name="Text_MAPName-03",Desc="渡嘉敷"};
case "Map-04": return new WmapSChs{ID="Map-04",Name="Text_MAPName-04",Desc="台碱宿舍"};
case "Map-05": return new WmapSChs{ID="Map-05",Name="Text_MAPName-05",Desc="兰屿"};
case "Map-06": return new WmapSChs{ID="Map-06",Name="Text_MAPName-06",Desc="竹富"};
case "Map-S1": return new WmapSChs{ID="Map-S1",Name="Text_MAPName-S1",Desc="澎湖"};
case "Map-S2": return new WmapSChs{ID="Map-S2",Name="Text_MAPName-S2",Desc="那霸"};
case "Map-NS": return new WmapSChs{ID="Map-NS",Name="Text_2.0Name",Desc="北之岛"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static WmapSChs Get(int key){
			switch (key) {
			case 0: return new WmapSChs{ID="Map-01",Name="Text_MAPName-01",Desc="猴硐"};
case 1: return new WmapSChs{ID="Map-02",Name="Text_MAPName-02",Desc="座间味"};
case 2: return new WmapSChs{ID="Map-03",Name="Text_MAPName-03",Desc="渡嘉敷"};
case 3: return new WmapSChs{ID="Map-04",Name="Text_MAPName-04",Desc="台碱宿舍"};
case 4: return new WmapSChs{ID="Map-05",Name="Text_MAPName-05",Desc="兰屿"};
case 5: return new WmapSChs{ID="Map-06",Name="Text_MAPName-06",Desc="竹富"};
case 6: return new WmapSChs{ID="Map-S1",Name="Text_MAPName-S1",Desc="澎湖"};
case 7: return new WmapSChs{ID="Map-S2",Name="Text_MAPName-S2",Desc="那霸"};
case 8: return new WmapSChs{ID="Map-NS",Name="Text_2.0Name",Desc="北之岛"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}