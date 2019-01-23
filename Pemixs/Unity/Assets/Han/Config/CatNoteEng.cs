using UnityEngine;
namespace Remix{
	public class CatNoteEng{
		public const int ID_COUNT = 28;

		public string ID {get; set;}
public string Name {get; set;}
public string SkillNote {get; set;}

		public static CatNoteEng Get(string key){
			switch (key) {
			case "C01": return new CatNoteEng{ID="C01",Name="GM",SkillNote="I'm good at running            Parkouring mode damage reduced by 10%"};
case "C02": return new CatNoteEng{ID="C02",Name="Dark face",SkillNote="I love to dance                  Dancing mode damage reduced by 10%"};
case "C03": return new CatNoteEng{ID="C03",Name="Far Sir",SkillNote="I'm good at fighting            Fighting mode damage reduced by 10%"};
case "C04": return new CatNoteEng{ID="C04",Name="Meow2",SkillNote="I'm super good at running   Parkouring mode damage reduced by 20%"};
case "C05": return new CatNoteEng{ID="C05",Name="Banana",SkillNote="I'm born to dance              Dancing mode damage reduced by 20%"};
case "C06": return new CatNoteEng{ID="C06",Name="Shadow",SkillNote="I'm super good at fighting  Fighting mode damage reduced by 20%"};
case "C07": return new CatNoteEng{ID="C07",Name="Sun flower",SkillNote="I'm good at running           Parkouring mode damage reduced by 10%"};
case "C08": return new CatNoteEng{ID="C08",Name="Oden",SkillNote="I love to dance                 Dancing mode damage reduced by 10%"};
case "C09": return new CatNoteEng{ID="C09",Name="Raccoon",SkillNote="I'm good at fighting           Fighting mode damage reduced by 10%"};
case "C10": return new CatNoteEng{ID="C10",Name="Prussianblue",SkillNote="Call me sports car            Parkouring mode damage reduced by 30%"};
case "C11": return new CatNoteEng{ID="C11",Name="One way",SkillNote="Call me dancing king        Dancing mode damage reduced by 30%"};
case "C12": return new CatNoteEng{ID="C12",Name="Orange",SkillNote="Sparta!!                           Fighting mode damage reduced by 30%"};
case "C13": return new CatNoteEng{ID="C13",Name="Nana",SkillNote="I'm good at running           Parkouring mode damage reduced by 10%"};
case "C14": return new CatNoteEng{ID="C14",Name="Kabuki",SkillNote="I love to dance                 Dancing mode damage reduced by 10%"};
case "C15": return new CatNoteEng{ID="C15",Name="Spades",SkillNote="I'm good at fighting           Fighting mode damage reduced by 10%"};
case "C16": return new CatNoteEng{ID="C16",Name="onmyoji",SkillNote="I'm a marathon runner       Parkouring mode damage reduced by 40%"};
case "C17": return new CatNoteEng{ID="C17",Name="Milk",SkillNote="Let's dance all night long   Dancing mode damage reduced by 40%"};
case "C18": return new CatNoteEng{ID="C18",Name="Telescope",SkillNote="I can fight against 10        Fighting mode damage reduced by 40%"};
case "C19": return new CatNoteEng{ID="C19",Name="Blue ribbon",SkillNote="I'm super good at running   Parkouring mode damage reduced by 20%"};
case "C20": return new CatNoteEng{ID="C20",Name="Shoyu",SkillNote="I'm born to dance              Dancing mode damage reduced by 20%"};
case "C21": return new CatNoteEng{ID="C21",Name="Over pay",SkillNote="I'm super good at fighting  Fighting mode damage reduced by 20%"};
case "C22": return new CatNoteEng{ID="C22",Name="Sharp eye",SkillNote="I'm a marathon runner       Parkouring mode damage reduced by 40%"};
case "C23": return new CatNoteEng{ID="C23",Name="Sesame",SkillNote="Let's dance all night long   Dancing mode damage reduced by 40%"};
case "C24": return new CatNoteEng{ID="C24",Name="Pharaoh",SkillNote="I can fight against 10        Fighting mode damage reduced by 40%"};
case "CM1": return new CatNoteEng{ID="CM1",Name="Lynx",SkillNote="Let's dance all night long   Dancing mode damage reduced by 50%"};
case "CM2": return new CatNoteEng{ID="CM2",Name="Lynx rufus",SkillNote="I can fight against 10        Fighting mode damage reduced by 50%"};
case "CM3": return new CatNoteEng{ID="CM3",Name="Lynx canadensis",SkillNote="I'm a marathon runner       Parkouring mode damage reduced by 50%"};
case "CM4": return new CatNoteEng{ID="CM4",Name="Lynx pardinus",SkillNote="I'm a marathon runner       Parkouring mode damage reduced by 50%"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static CatNoteEng Get(int key){
			switch (key) {
			case 0: return new CatNoteEng{ID="C01",Name="GM",SkillNote="I'm good at running            Parkouring mode damage reduced by 10%"};
case 1: return new CatNoteEng{ID="C02",Name="Dark face",SkillNote="I love to dance                  Dancing mode damage reduced by 10%"};
case 2: return new CatNoteEng{ID="C03",Name="Far Sir",SkillNote="I'm good at fighting            Fighting mode damage reduced by 10%"};
case 3: return new CatNoteEng{ID="C04",Name="Meow2",SkillNote="I'm super good at running   Parkouring mode damage reduced by 20%"};
case 4: return new CatNoteEng{ID="C05",Name="Banana",SkillNote="I'm born to dance              Dancing mode damage reduced by 20%"};
case 5: return new CatNoteEng{ID="C06",Name="Shadow",SkillNote="I'm super good at fighting  Fighting mode damage reduced by 20%"};
case 6: return new CatNoteEng{ID="C07",Name="Sun flower",SkillNote="I'm good at running           Parkouring mode damage reduced by 10%"};
case 7: return new CatNoteEng{ID="C08",Name="Oden",SkillNote="I love to dance                 Dancing mode damage reduced by 10%"};
case 8: return new CatNoteEng{ID="C09",Name="Raccoon",SkillNote="I'm good at fighting           Fighting mode damage reduced by 10%"};
case 9: return new CatNoteEng{ID="C10",Name="Prussianblue",SkillNote="Call me sports car            Parkouring mode damage reduced by 30%"};
case 10: return new CatNoteEng{ID="C11",Name="One way",SkillNote="Call me dancing king        Dancing mode damage reduced by 30%"};
case 11: return new CatNoteEng{ID="C12",Name="Orange",SkillNote="Sparta!!                           Fighting mode damage reduced by 30%"};
case 12: return new CatNoteEng{ID="C13",Name="Nana",SkillNote="I'm good at running           Parkouring mode damage reduced by 10%"};
case 13: return new CatNoteEng{ID="C14",Name="Kabuki",SkillNote="I love to dance                 Dancing mode damage reduced by 10%"};
case 14: return new CatNoteEng{ID="C15",Name="Spades",SkillNote="I'm good at fighting           Fighting mode damage reduced by 10%"};
case 15: return new CatNoteEng{ID="C16",Name="onmyoji",SkillNote="I'm a marathon runner       Parkouring mode damage reduced by 40%"};
case 16: return new CatNoteEng{ID="C17",Name="Milk",SkillNote="Let's dance all night long   Dancing mode damage reduced by 40%"};
case 17: return new CatNoteEng{ID="C18",Name="Telescope",SkillNote="I can fight against 10        Fighting mode damage reduced by 40%"};
case 18: return new CatNoteEng{ID="C19",Name="Blue ribbon",SkillNote="I'm super good at running   Parkouring mode damage reduced by 20%"};
case 19: return new CatNoteEng{ID="C20",Name="Shoyu",SkillNote="I'm born to dance              Dancing mode damage reduced by 20%"};
case 20: return new CatNoteEng{ID="C21",Name="Over pay",SkillNote="I'm super good at fighting  Fighting mode damage reduced by 20%"};
case 21: return new CatNoteEng{ID="C22",Name="Sharp eye",SkillNote="I'm a marathon runner       Parkouring mode damage reduced by 40%"};
case 22: return new CatNoteEng{ID="C23",Name="Sesame",SkillNote="Let's dance all night long   Dancing mode damage reduced by 40%"};
case 23: return new CatNoteEng{ID="C24",Name="Pharaoh",SkillNote="I can fight against 10        Fighting mode damage reduced by 40%"};
case 24: return new CatNoteEng{ID="CM1",Name="Lynx",SkillNote="Let's dance all night long   Dancing mode damage reduced by 50%"};
case 25: return new CatNoteEng{ID="CM2",Name="Lynx rufus",SkillNote="I can fight against 10        Fighting mode damage reduced by 50%"};
case 26: return new CatNoteEng{ID="CM3",Name="Lynx canadensis",SkillNote="I'm a marathon runner       Parkouring mode damage reduced by 50%"};
case 27: return new CatNoteEng{ID="CM4",Name="Lynx pardinus",SkillNote="I'm a marathon runner       Parkouring mode damage reduced by 50%"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}