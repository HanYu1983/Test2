using UnityEngine;
namespace Remix{
	public class GachaDef{
		public const int ID_COUNT = 24;

		public string ID {get; set;}
public string Map {get; set;}
public string CatFile {get; set;}
public string PohtoFile {get; set;}
public int Unlock {get; set;}
public int CatTime {get; set;}
public int PhotoTime {get; set;}

		public static GachaDef Get(string key){
			switch (key) {
			case "M01GC01": return new GachaDef{ID="M01GC01",Map="Map_01",CatFile="gachapm01",PohtoFile="photom01",Unlock=0,CatTime=20,PhotoTime=10};
case "M01GC02": return new GachaDef{ID="M01GC02",Map="Map_01",CatFile="gachapm01",PohtoFile="photom01",Unlock=50,CatTime=20,PhotoTime=10};
case "M01GC03": return new GachaDef{ID="M01GC03",Map="Map_01",CatFile="gachapm01",PohtoFile="photom01",Unlock=100,CatTime=20,PhotoTime=10};
case "M02GC01": return new GachaDef{ID="M02GC01",Map="Map_02",CatFile="gachapm02",PohtoFile="photom02",Unlock=0,CatTime=20,PhotoTime=10};
case "M02GC02": return new GachaDef{ID="M02GC02",Map="Map_02",CatFile="gachapm02",PohtoFile="photom02",Unlock=50,CatTime=20,PhotoTime=10};
case "M02GC03": return new GachaDef{ID="M02GC03",Map="Map_02",CatFile="gachapm02",PohtoFile="photom02",Unlock=100,CatTime=20,PhotoTime=10};
case "M03GC01": return new GachaDef{ID="M03GC01",Map="Map_03",CatFile="gachapm03",PohtoFile="photom03",Unlock=0,CatTime=20,PhotoTime=10};
case "M03GC02": return new GachaDef{ID="M03GC02",Map="Map_03",CatFile="gachapm03",PohtoFile="photom03",Unlock=50,CatTime=20,PhotoTime=10};
case "M03GC03": return new GachaDef{ID="M03GC03",Map="Map_03",CatFile="gachapm03",PohtoFile="photom03",Unlock=100,CatTime=20,PhotoTime=10};
case "M04GC01": return new GachaDef{ID="M04GC01",Map="Map_04",CatFile="gachapm04",PohtoFile="photom04",Unlock=0,CatTime=20,PhotoTime=10};
case "M04GC02": return new GachaDef{ID="M04GC02",Map="Map_04",CatFile="gachapm04",PohtoFile="photom04",Unlock=50,CatTime=20,PhotoTime=10};
case "M04GC03": return new GachaDef{ID="M04GC03",Map="Map_04",CatFile="gachapm04",PohtoFile="photom04",Unlock=100,CatTime=20,PhotoTime=10};
case "M05GC01": return new GachaDef{ID="M05GC01",Map="Map_05",CatFile="gachapm05",PohtoFile="photom05",Unlock=0,CatTime=20,PhotoTime=10};
case "M05GC02": return new GachaDef{ID="M05GC02",Map="Map_05",CatFile="gachapm05",PohtoFile="photom05",Unlock=50,CatTime=20,PhotoTime=10};
case "M05GC03": return new GachaDef{ID="M05GC03",Map="Map_05",CatFile="gachapm05",PohtoFile="photom05",Unlock=100,CatTime=20,PhotoTime=10};
case "M06GC01": return new GachaDef{ID="M06GC01",Map="Map_06",CatFile="gachapm06",PohtoFile="photom06",Unlock=0,CatTime=20,PhotoTime=10};
case "M06GC02": return new GachaDef{ID="M06GC02",Map="Map_06",CatFile="gachapm06",PohtoFile="photom06",Unlock=50,CatTime=20,PhotoTime=10};
case "M06GC03": return new GachaDef{ID="M06GC03",Map="Map_06",CatFile="gachapm06",PohtoFile="photom06",Unlock=100,CatTime=20,PhotoTime=10};
case "MS1GC01": return new GachaDef{ID="MS1GC01",Map="Map_S1",CatFile="gachapmS1",PohtoFile="photomS1",Unlock=0,CatTime=60,PhotoTime=10};
case "MS1GC02": return new GachaDef{ID="MS1GC02",Map="Map_S1",CatFile="gachapmS1",PohtoFile="photomS1",Unlock=75,CatTime=60,PhotoTime=10};
case "MS1GC03": return new GachaDef{ID="MS1GC03",Map="Map_S1",CatFile="gachapmS1",PohtoFile="photomS1",Unlock=150,CatTime=60,PhotoTime=10};
case "MS2GC01": return new GachaDef{ID="MS2GC01",Map="Map_S2",CatFile="gachapmS2",PohtoFile="photomS2",Unlock=0,CatTime=60,PhotoTime=10};
case "MS2GC02": return new GachaDef{ID="MS2GC02",Map="Map_S2",CatFile="gachapmS2",PohtoFile="photomS2",Unlock=75,CatTime=60,PhotoTime=10};
case "MS2GC03": return new GachaDef{ID="MS2GC03",Map="Map_S2",CatFile="gachapmS2",PohtoFile="photomS2",Unlock=150,CatTime=60,PhotoTime=10};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static GachaDef Get(int key){
			switch (key) {
			case 0: return new GachaDef{ID="M01GC01",Map="Map_01",CatFile="gachapm01",PohtoFile="photom01",Unlock=0,CatTime=20,PhotoTime=10};
case 1: return new GachaDef{ID="M01GC02",Map="Map_01",CatFile="gachapm01",PohtoFile="photom01",Unlock=50,CatTime=20,PhotoTime=10};
case 2: return new GachaDef{ID="M01GC03",Map="Map_01",CatFile="gachapm01",PohtoFile="photom01",Unlock=100,CatTime=20,PhotoTime=10};
case 3: return new GachaDef{ID="M02GC01",Map="Map_02",CatFile="gachapm02",PohtoFile="photom02",Unlock=0,CatTime=20,PhotoTime=10};
case 4: return new GachaDef{ID="M02GC02",Map="Map_02",CatFile="gachapm02",PohtoFile="photom02",Unlock=50,CatTime=20,PhotoTime=10};
case 5: return new GachaDef{ID="M02GC03",Map="Map_02",CatFile="gachapm02",PohtoFile="photom02",Unlock=100,CatTime=20,PhotoTime=10};
case 6: return new GachaDef{ID="M03GC01",Map="Map_03",CatFile="gachapm03",PohtoFile="photom03",Unlock=0,CatTime=20,PhotoTime=10};
case 7: return new GachaDef{ID="M03GC02",Map="Map_03",CatFile="gachapm03",PohtoFile="photom03",Unlock=50,CatTime=20,PhotoTime=10};
case 8: return new GachaDef{ID="M03GC03",Map="Map_03",CatFile="gachapm03",PohtoFile="photom03",Unlock=100,CatTime=20,PhotoTime=10};
case 9: return new GachaDef{ID="M04GC01",Map="Map_04",CatFile="gachapm04",PohtoFile="photom04",Unlock=0,CatTime=20,PhotoTime=10};
case 10: return new GachaDef{ID="M04GC02",Map="Map_04",CatFile="gachapm04",PohtoFile="photom04",Unlock=50,CatTime=20,PhotoTime=10};
case 11: return new GachaDef{ID="M04GC03",Map="Map_04",CatFile="gachapm04",PohtoFile="photom04",Unlock=100,CatTime=20,PhotoTime=10};
case 12: return new GachaDef{ID="M05GC01",Map="Map_05",CatFile="gachapm05",PohtoFile="photom05",Unlock=0,CatTime=20,PhotoTime=10};
case 13: return new GachaDef{ID="M05GC02",Map="Map_05",CatFile="gachapm05",PohtoFile="photom05",Unlock=50,CatTime=20,PhotoTime=10};
case 14: return new GachaDef{ID="M05GC03",Map="Map_05",CatFile="gachapm05",PohtoFile="photom05",Unlock=100,CatTime=20,PhotoTime=10};
case 15: return new GachaDef{ID="M06GC01",Map="Map_06",CatFile="gachapm06",PohtoFile="photom06",Unlock=0,CatTime=20,PhotoTime=10};
case 16: return new GachaDef{ID="M06GC02",Map="Map_06",CatFile="gachapm06",PohtoFile="photom06",Unlock=50,CatTime=20,PhotoTime=10};
case 17: return new GachaDef{ID="M06GC03",Map="Map_06",CatFile="gachapm06",PohtoFile="photom06",Unlock=100,CatTime=20,PhotoTime=10};
case 18: return new GachaDef{ID="MS1GC01",Map="Map_S1",CatFile="gachapmS1",PohtoFile="photomS1",Unlock=0,CatTime=60,PhotoTime=10};
case 19: return new GachaDef{ID="MS1GC02",Map="Map_S1",CatFile="gachapmS1",PohtoFile="photomS1",Unlock=75,CatTime=60,PhotoTime=10};
case 20: return new GachaDef{ID="MS1GC03",Map="Map_S1",CatFile="gachapmS1",PohtoFile="photomS1",Unlock=150,CatTime=60,PhotoTime=10};
case 21: return new GachaDef{ID="MS2GC01",Map="Map_S2",CatFile="gachapmS2",PohtoFile="photomS2",Unlock=0,CatTime=60,PhotoTime=10};
case 22: return new GachaDef{ID="MS2GC02",Map="Map_S2",CatFile="gachapmS2",PohtoFile="photomS2",Unlock=75,CatTime=60,PhotoTime=10};
case 23: return new GachaDef{ID="MS2GC03",Map="Map_S2",CatFile="gachapmS2",PohtoFile="photomS2",Unlock=150,CatTime=60,PhotoTime=10};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}