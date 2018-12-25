using UnityEngine;
namespace Remix{
	public class TutorialNoteEng{
		public const int ID_COUNT = 54;

		public string ID {get; set;}
public string Page {get; set;}
public string Desc {get; set;}

		public static TutorialNoteEng Get(string key){
			switch (key) {
			case "TP01T01": return new TutorialNoteEng{ID="TP01T01",Page="TP01",Desc="Gameplay"};
case "TP01T02": return new TutorialNoteEng{ID="TP01T02",Page="TP01",Desc="Point"};
case "TP01T03": return new TutorialNoteEng{ID="TP01T03",Page="TP01",Desc="Up & Down"};
case "TP01T04": return new TutorialNoteEng{ID="TP01T04",Page="TP01",Desc="Continuous"};
case "TP01T05": return new TutorialNoteEng{ID="TP01T05",Page="TP01",Desc="Twist"};
case "TP01T06": return new TutorialNoteEng{ID="TP01T06",Page="TP01",Desc="Hit the paw"};
case "TP01T07": return new TutorialNoteEng{ID="TP01T07",Page="TP01",Desc="Slide upward or downward on the paw"};
case "TP01T08": return new TutorialNoteEng{ID="TP01T08",Page="TP01",Desc="Continuously hit the paw"};
case "TP01T09": return new TutorialNoteEng{ID="TP01T09",Page="TP01",Desc="Continuously twist the paw"};
case "TP02T01": return new TutorialNoteEng{ID="TP02T01",Page="TP02",Desc="Interactive Mode"};
case "TP02T02": return new TutorialNoteEng{ID="TP02T02",Page="TP02",Desc="Interactive"};
case "TP02T03": return new TutorialNoteEng{ID="TP02T03",Page="TP02",Desc="Petting"};
case "TP02T04": return new TutorialNoteEng{ID="TP02T04",Page="TP02",Desc="Angery"};
case "TP02T05": return new TutorialNoteEng{ID="TP02T05",Page="TP02",Desc="Boost"};
case "TP02T06": return new TutorialNoteEng{ID="TP02T06",Page="TP02",Desc="Open interactive window"};
case "TP02T07": return new TutorialNoteEng{ID="TP02T07",Page="TP02",Desc="Pet the cat to add EXP"};
case "TP02T08": return new TutorialNoteEng{ID="TP02T08",Page="TP02",Desc="Over petting will set him off"};
case "TP02T09": return new TutorialNoteEng{ID="TP02T09",Page="TP02",Desc="Use catnip to cool him down"};
case "TP03T01": return new TutorialNoteEng{ID="TP03T01",Page="TP03",Desc="Interactive Mode"};
case "TP03T02": return new TutorialNoteEng{ID="TP03T02",Page="TP03",Desc="Hungry"};
case "TP03T03": return new TutorialNoteEng{ID="TP03T03",Page="TP03",Desc="Sleep"};
case "TP03T04": return new TutorialNoteEng{ID="TP03T04",Page="TP03",Desc="Excited"};
case "TP03T05": return new TutorialNoteEng{ID="TP03T05",Page="TP03",Desc="Body types"};
case "TP03T06": return new TutorialNoteEng{ID="TP03T06",Page="TP03",Desc="Feed the cat to add HP"};
case "TP03T07": return new TutorialNoteEng{ID="TP03T07",Page="TP03",Desc="Pet the cat to wake him up"};
case "TP03T08": return new TutorialNoteEng{ID="TP03T08",Page="TP03",Desc="Use a toy to add EXP"};
case "TP03T09": return new TutorialNoteEng{ID="TP03T09",Page="TP03",Desc="Exclusive toys"};
case "TP04T01": return new TutorialNoteEng{ID="TP04T01",Page="TP04",Desc="Explore Mode"};
case "TP04T02": return new TutorialNoteEng{ID="TP04T02",Page="TP04",Desc="Cat tree"};
case "TP04T03": return new TutorialNoteEng{ID="TP04T03",Page="TP04",Desc="Waiting"};
case "TP04T04": return new TutorialNoteEng{ID="TP04T04",Page="TP04",Desc="Boost"};
case "TP04T05": return new TutorialNoteEng{ID="TP04T05",Page="TP04",Desc="Body types"};
case "TP04T06": return new TutorialNoteEng{ID="TP04T06",Page="TP04",Desc="Use a gacha item"};
case "TP04T07": return new TutorialNoteEng{ID="TP04T07",Page="TP04",Desc="Capture a new cat"};
case "TP04T08": return new TutorialNoteEng{ID="TP04T08",Page="TP04",Desc="Use catnip to boost"};
case "TP04T09": return new TutorialNoteEng{ID="TP04T09",Page="TP04",Desc="Exclusive gacha items"};
case "TP05T01": return new TutorialNoteEng{ID="TP05T01",Page="TP05",Desc="Explore Mode"};
case "TP05T02": return new TutorialNoteEng{ID="TP05T02",Page="TP05",Desc="Habitat"};
case "TP05T03": return new TutorialNoteEng{ID="TP05T03",Page="TP05",Desc="Houton"};
case "TP05T04": return new TutorialNoteEng{ID="TP05T04",Page="TP05",Desc="Zamami"};
case "TP05T05": return new TutorialNoteEng{ID="TP05T05",Page="TP05",Desc="Tokashiki"};
case "TP05T06": return new TutorialNoteEng{ID="TP05T06",Page="TP05",Desc="South Islands map"};
case "TP05T07": return new TutorialNoteEng{ID="TP05T07",Page="TP05"};
case "TP05T08": return new TutorialNoteEng{ID="TP05T08",Page="TP05"};
case "TP05T09": return new TutorialNoteEng{ID="TP05T09",Page="TP05"};
case "TP06T01": return new TutorialNoteEng{ID="TP06T01",Page="TP06",Desc="Explore Mode"};
case "TP06T02": return new TutorialNoteEng{ID="TP06T02",Page="TP06",Desc="Cat tree"};
case "TP06T03": return new TutorialNoteEng{ID="TP06T03",Page="TP06",Desc="Waiting"};
case "TP06T04": return new TutorialNoteEng{ID="TP06T04",Page="TP06",Desc="Boost"};
case "TP06T05": return new TutorialNoteEng{ID="TP06T05",Page="TP06",Desc="Cameras"};
case "TP06T06": return new TutorialNoteEng{ID="TP06T06",Page="TP06",Desc="Use a camera"};
case "TP06T07": return new TutorialNoteEng{ID="TP06T07",Page="TP06",Desc="Take a new photo"};
case "TP06T08": return new TutorialNoteEng{ID="TP06T08",Page="TP06",Desc="Use a tripod to boost"};
case "TP06T09": return new TutorialNoteEng{ID="TP06T09",Page="TP06",Desc="High performance cameras"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static TutorialNoteEng Get(int key){
			switch (key) {
			case 0: return new TutorialNoteEng{ID="TP01T01",Page="TP01",Desc="Gameplay"};
case 1: return new TutorialNoteEng{ID="TP01T02",Page="TP01",Desc="Point"};
case 2: return new TutorialNoteEng{ID="TP01T03",Page="TP01",Desc="Up & Down"};
case 3: return new TutorialNoteEng{ID="TP01T04",Page="TP01",Desc="Continuous"};
case 4: return new TutorialNoteEng{ID="TP01T05",Page="TP01",Desc="Twist"};
case 5: return new TutorialNoteEng{ID="TP01T06",Page="TP01",Desc="Hit the paw"};
case 6: return new TutorialNoteEng{ID="TP01T07",Page="TP01",Desc="Slide upward or downward on the paw"};
case 7: return new TutorialNoteEng{ID="TP01T08",Page="TP01",Desc="Continuously hit the paw"};
case 8: return new TutorialNoteEng{ID="TP01T09",Page="TP01",Desc="Continuously twist the paw"};
case 9: return new TutorialNoteEng{ID="TP02T01",Page="TP02",Desc="Interactive Mode"};
case 10: return new TutorialNoteEng{ID="TP02T02",Page="TP02",Desc="Interactive"};
case 11: return new TutorialNoteEng{ID="TP02T03",Page="TP02",Desc="Petting"};
case 12: return new TutorialNoteEng{ID="TP02T04",Page="TP02",Desc="Angery"};
case 13: return new TutorialNoteEng{ID="TP02T05",Page="TP02",Desc="Boost"};
case 14: return new TutorialNoteEng{ID="TP02T06",Page="TP02",Desc="Open interactive window"};
case 15: return new TutorialNoteEng{ID="TP02T07",Page="TP02",Desc="Pet the cat to add EXP"};
case 16: return new TutorialNoteEng{ID="TP02T08",Page="TP02",Desc="Over petting will set him off"};
case 17: return new TutorialNoteEng{ID="TP02T09",Page="TP02",Desc="Use catnip to cool him down"};
case 18: return new TutorialNoteEng{ID="TP03T01",Page="TP03",Desc="Interactive Mode"};
case 19: return new TutorialNoteEng{ID="TP03T02",Page="TP03",Desc="Hungry"};
case 20: return new TutorialNoteEng{ID="TP03T03",Page="TP03",Desc="Sleep"};
case 21: return new TutorialNoteEng{ID="TP03T04",Page="TP03",Desc="Excited"};
case 22: return new TutorialNoteEng{ID="TP03T05",Page="TP03",Desc="Body types"};
case 23: return new TutorialNoteEng{ID="TP03T06",Page="TP03",Desc="Feed the cat to add HP"};
case 24: return new TutorialNoteEng{ID="TP03T07",Page="TP03",Desc="Pet the cat to wake him up"};
case 25: return new TutorialNoteEng{ID="TP03T08",Page="TP03",Desc="Use a toy to add EXP"};
case 26: return new TutorialNoteEng{ID="TP03T09",Page="TP03",Desc="Exclusive toys"};
case 27: return new TutorialNoteEng{ID="TP04T01",Page="TP04",Desc="Explore Mode"};
case 28: return new TutorialNoteEng{ID="TP04T02",Page="TP04",Desc="Cat tree"};
case 29: return new TutorialNoteEng{ID="TP04T03",Page="TP04",Desc="Waiting"};
case 30: return new TutorialNoteEng{ID="TP04T04",Page="TP04",Desc="Boost"};
case 31: return new TutorialNoteEng{ID="TP04T05",Page="TP04",Desc="Body types"};
case 32: return new TutorialNoteEng{ID="TP04T06",Page="TP04",Desc="Use a gacha item"};
case 33: return new TutorialNoteEng{ID="TP04T07",Page="TP04",Desc="Capture a new cat"};
case 34: return new TutorialNoteEng{ID="TP04T08",Page="TP04",Desc="Use catnip to boost"};
case 35: return new TutorialNoteEng{ID="TP04T09",Page="TP04",Desc="Exclusive gacha items"};
case 36: return new TutorialNoteEng{ID="TP05T01",Page="TP05",Desc="Explore Mode"};
case 37: return new TutorialNoteEng{ID="TP05T02",Page="TP05",Desc="Habitat"};
case 38: return new TutorialNoteEng{ID="TP05T03",Page="TP05",Desc="Houton"};
case 39: return new TutorialNoteEng{ID="TP05T04",Page="TP05",Desc="Zamami"};
case 40: return new TutorialNoteEng{ID="TP05T05",Page="TP05",Desc="Tokashiki"};
case 41: return new TutorialNoteEng{ID="TP05T06",Page="TP05",Desc="South Islands map"};
case 42: return new TutorialNoteEng{ID="TP05T07",Page="TP05"};
case 43: return new TutorialNoteEng{ID="TP05T08",Page="TP05"};
case 44: return new TutorialNoteEng{ID="TP05T09",Page="TP05"};
case 45: return new TutorialNoteEng{ID="TP06T01",Page="TP06",Desc="Explore Mode"};
case 46: return new TutorialNoteEng{ID="TP06T02",Page="TP06",Desc="Cat tree"};
case 47: return new TutorialNoteEng{ID="TP06T03",Page="TP06",Desc="Waiting"};
case 48: return new TutorialNoteEng{ID="TP06T04",Page="TP06",Desc="Boost"};
case 49: return new TutorialNoteEng{ID="TP06T05",Page="TP06",Desc="Cameras"};
case 50: return new TutorialNoteEng{ID="TP06T06",Page="TP06",Desc="Use a camera"};
case 51: return new TutorialNoteEng{ID="TP06T07",Page="TP06",Desc="Take a new photo"};
case 52: return new TutorialNoteEng{ID="TP06T08",Page="TP06",Desc="Use a tripod to boost"};
case 53: return new TutorialNoteEng{ID="TP06T09",Page="TP06",Desc="High performance cameras"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}