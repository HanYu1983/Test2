using UnityEngine;
namespace Remix{
	public class MapDef{
		public const int ID_COUNT = 8;

		public string ID {get; set;}
public int Event {get; set;}
public string Desc {get; set;}
public string Prefab {get; set;}

		public static MapDef Get(string key){
			switch (key) {
			case "Map01": return new MapDef{ID="Map01",Event=0,Desc="無"};
case "Map02": return new MapDef{ID="Map02",Event=0,Desc="無"};
case "Map03": return new MapDef{ID="Map03",Event=0,Desc="無"};
case "Map04": return new MapDef{ID="Map04",Event=0,Desc="無"};
case "Map05": return new MapDef{ID="Map05",Event=0,Desc="無"};
case "Map06": return new MapDef{ID="Map06",Event=0,Desc="無"};
case "MapS1": return new MapDef{ID="MapS1",Event=1,Desc="隨時開啟",Prefab="Event1"};
case "MapS2": return new MapDef{ID="MapS2",Event=2,Desc="隨時開啟",Prefab="Event2"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static MapDef Get(int key){
			switch (key) {
			case 0: return new MapDef{ID="Map01",Event=0,Desc="無"};
case 1: return new MapDef{ID="Map02",Event=0,Desc="無"};
case 2: return new MapDef{ID="Map03",Event=0,Desc="無"};
case 3: return new MapDef{ID="Map04",Event=0,Desc="無"};
case 4: return new MapDef{ID="Map05",Event=0,Desc="無"};
case 5: return new MapDef{ID="Map06",Event=0,Desc="無"};
case 6: return new MapDef{ID="MapS1",Event=1,Desc="隨時開啟",Prefab="Event1"};
case 7: return new MapDef{ID="MapS2",Event=2,Desc="隨時開啟",Prefab="Event2"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}