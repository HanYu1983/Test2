using UnityEngine;
namespace Remix{
	public class MainuiNoteEng{
		public const string MUI1A01 = "MUI1A01";
public const string MUI1B01 = "MUI1B01";
public const string MUI1C01 = "MUI1C01";
public const string MUI1C02 = "MUI1C02";
public const string MUI1C03 = "MUI1C03";
public const string MUI1C04 = "MUI1C04";
public const string MUI1C05 = "MUI1C05";
public const string MUI1C06 = "MUI1C06";
public const string MUI1CS1 = "MUI1CS1";
public const string MUI1CS2 = "MUI1CS2";
public const string MUI2A01 = "MUI2A01";
public const string MUI2B01 = "MUI2B01";
public const string MUI2C01 = "MUI2C01";
public const string MUI3A01 = "MUI3A01";
public const string MUI3A02 = "MUI3A02";
public const string MUI3A03 = "MUI3A03";
public const string MUI3A04 = "MUI3A04";
public const string MUI3A05 = "MUI3A05";
public const string MUI3A06 = "MUI3A06";
public const string MUI3AS1 = "MUI3AS1";
public const string MUI3AS2 = "MUI3AS2";
public const string MUI3B01 = "MUI3B01";
public const string MUI3C01 = "MUI3C01";
public const string MUI4A01 = "MUI4A01";
public const string MUI4A02 = "MUI4A02";
public const string MUI4A03 = "MUI4A03";
public const string MUI4A04 = "MUI4A04";
public const string MUI4A05 = "MUI4A05";
public const string MUI4A06 = "MUI4A06";
public const string MUI4AS1 = "MUI4AS1";
public const string MUI4AS2 = "MUI4AS2";
public const string MUI4B01 = "MUI4B01";
public const string MUI4C01 = "MUI4C01";
public const string MUI4C02 = "MUI4C02";
public const string MUI4C03 = "MUI4C03";
public const string MUI4C04 = "MUI4C04";
public const string MUI4C05 = "MUI4C05";
public const string MUI4C06 = "MUI4C06";
public const string MUI4CS1 = "MUI4CS1";
public const string MUI4CS2 = "MUI4CS2";
public const string MUI5A01 = "MUI5A01";
public const string MUI5B01 = "MUI5B01";
public const string MUI5B02 = "MUI5B02";
public const string MUI5B03 = "MUI5B03";
public const string MUI5B04 = "MUI5B04";
public const string MUI5B05 = "MUI5B05";
public const int ID_COUNT = 46;

		public string ID {get; set;}
public string Desc {get; set;}

		public static MainuiNoteEng Get(string key){
			switch (key) {
			case "MUI1A01": return new MainuiNoteEng{ID="MUI1A01",Desc="Main Page"};
case "MUI1B01": return new MainuiNoteEng{ID="MUI1B01",Desc="Community  Page"};
case "MUI1C01": return new MainuiNoteEng{ID="MUI1C01",Desc="Cat Home - 01"};
case "MUI1C02": return new MainuiNoteEng{ID="MUI1C02",Desc="Cat Home - 02"};
case "MUI1C03": return new MainuiNoteEng{ID="MUI1C03",Desc="Cat Home - 03"};
case "MUI1C04": return new MainuiNoteEng{ID="MUI1C04",Desc="Cat Home - 04"};
case "MUI1C05": return new MainuiNoteEng{ID="MUI1C05",Desc="Cat Home - 05"};
case "MUI1C06": return new MainuiNoteEng{ID="MUI1C06",Desc="Cat Home - 06"};
case "MUI1CS1": return new MainuiNoteEng{ID="MUI1CS1",Desc="Cat Home - S1"};
case "MUI1CS2": return new MainuiNoteEng{ID="MUI1CS2",Desc="Cat Home - S2"};
case "MUI2A01": return new MainuiNoteEng{ID="MUI2A01",Desc="Cat Shop"};
case "MUI2B01": return new MainuiNoteEng{ID="MUI2B01",Desc="Cat Bank"};
case "MUI2C01": return new MainuiNoteEng{ID="MUI2C01",Desc="Cat Record"};
case "MUI3A01": return new MainuiNoteEng{ID="MUI3A01",Desc="Map - Houton"};
case "MUI3A02": return new MainuiNoteEng{ID="MUI3A02",Desc="Map - Zamami"};
case "MUI3A03": return new MainuiNoteEng{ID="MUI3A03",Desc="Map - Tokashiki"};
case "MUI3A04": return new MainuiNoteEng{ID="MUI3A04",Desc="Map - Dorm of CPDC"};
case "MUI3A05": return new MainuiNoteEng{ID="MUI3A05",Desc="Map - Orchid"};
case "MUI3A06": return new MainuiNoteEng{ID="MUI3A06",Desc="Map - Taketomi"};
case "MUI3AS1": return new MainuiNoteEng{ID="MUI3AS1",Desc="Map - Penghu"};
case "MUI3AS2": return new MainuiNoteEng{ID="MUI3AS2",Desc="Map - Naha"};
case "MUI3B01": return new MainuiNoteEng{ID="MUI3B01",Desc="Map - South Island"};
case "MUI3C01": return new MainuiNoteEng{ID="MUI3C01",Desc="Map - None"};
case "MUI4A01": return new MainuiNoteEng{ID="MUI4A01",Desc="Explore - Houton"};
case "MUI4A02": return new MainuiNoteEng{ID="MUI4A02",Desc="Explore - Zamami"};
case "MUI4A03": return new MainuiNoteEng{ID="MUI4A03",Desc="Explore - Tokashiki"};
case "MUI4A04": return new MainuiNoteEng{ID="MUI4A04",Desc="Explore - Dorm of CPDC"};
case "MUI4A05": return new MainuiNoteEng{ID="MUI4A05",Desc="Explore - Orchid"};
case "MUI4A06": return new MainuiNoteEng{ID="MUI4A06",Desc="Explore - Taketomi"};
case "MUI4AS1": return new MainuiNoteEng{ID="MUI4AS1",Desc="Explore - Penghu"};
case "MUI4AS2": return new MainuiNoteEng{ID="MUI4AS2",Desc="Explore - Naha"};
case "MUI4B01": return new MainuiNoteEng{ID="MUI4B01",Desc="Explore - South Island"};
case "MUI4C01": return new MainuiNoteEng{ID="MUI4C01",Desc="Photo - Houton"};
case "MUI4C02": return new MainuiNoteEng{ID="MUI4C02",Desc="Photo - Zamami"};
case "MUI4C03": return new MainuiNoteEng{ID="MUI4C03",Desc="Photo - Tokashiki"};
case "MUI4C04": return new MainuiNoteEng{ID="MUI4C04",Desc="Photo - Dorm of CPDC"};
case "MUI4C05": return new MainuiNoteEng{ID="MUI4C05",Desc="Photo - Orchid"};
case "MUI4C06": return new MainuiNoteEng{ID="MUI4C06",Desc="Photo - Taketomi"};
case "MUI4CS1": return new MainuiNoteEng{ID="MUI4CS1",Desc="Photo - Penghu"};
case "MUI4CS2": return new MainuiNoteEng{ID="MUI4CS2",Desc="Photo - Naha"};
case "MUI5A01": return new MainuiNoteEng{ID="MUI5A01",Desc="Setting"};
case "MUI5B01": return new MainuiNoteEng{ID="MUI5B01",Desc="Tutorial"};
case "MUI5B02": return new MainuiNoteEng{ID="MUI5B02",Desc="Music SYNC Tool"};
case "MUI5B03": return new MainuiNoteEng{ID="MUI5B03",Desc="Data Transfer"};
case "MUI5B04": return new MainuiNoteEng{ID="MUI5B04",Desc="Language"};
case "MUI5B05": return new MainuiNoteEng{ID="MUI5B05",Desc="Staff List"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static MainuiNoteEng Get(int key){
			switch (key) {
			case 0: return new MainuiNoteEng{ID="MUI1A01",Desc="Main Page"};
case 1: return new MainuiNoteEng{ID="MUI1B01",Desc="Community  Page"};
case 2: return new MainuiNoteEng{ID="MUI1C01",Desc="Cat Home - 01"};
case 3: return new MainuiNoteEng{ID="MUI1C02",Desc="Cat Home - 02"};
case 4: return new MainuiNoteEng{ID="MUI1C03",Desc="Cat Home - 03"};
case 5: return new MainuiNoteEng{ID="MUI1C04",Desc="Cat Home - 04"};
case 6: return new MainuiNoteEng{ID="MUI1C05",Desc="Cat Home - 05"};
case 7: return new MainuiNoteEng{ID="MUI1C06",Desc="Cat Home - 06"};
case 8: return new MainuiNoteEng{ID="MUI1CS1",Desc="Cat Home - S1"};
case 9: return new MainuiNoteEng{ID="MUI1CS2",Desc="Cat Home - S2"};
case 10: return new MainuiNoteEng{ID="MUI2A01",Desc="Cat Shop"};
case 11: return new MainuiNoteEng{ID="MUI2B01",Desc="Cat Bank"};
case 12: return new MainuiNoteEng{ID="MUI2C01",Desc="Cat Record"};
case 13: return new MainuiNoteEng{ID="MUI3A01",Desc="Map - Houton"};
case 14: return new MainuiNoteEng{ID="MUI3A02",Desc="Map - Zamami"};
case 15: return new MainuiNoteEng{ID="MUI3A03",Desc="Map - Tokashiki"};
case 16: return new MainuiNoteEng{ID="MUI3A04",Desc="Map - Dorm of CPDC"};
case 17: return new MainuiNoteEng{ID="MUI3A05",Desc="Map - Orchid"};
case 18: return new MainuiNoteEng{ID="MUI3A06",Desc="Map - Taketomi"};
case 19: return new MainuiNoteEng{ID="MUI3AS1",Desc="Map - Penghu"};
case 20: return new MainuiNoteEng{ID="MUI3AS2",Desc="Map - Naha"};
case 21: return new MainuiNoteEng{ID="MUI3B01",Desc="Map - South Island"};
case 22: return new MainuiNoteEng{ID="MUI3C01",Desc="Map - None"};
case 23: return new MainuiNoteEng{ID="MUI4A01",Desc="Explore - Houton"};
case 24: return new MainuiNoteEng{ID="MUI4A02",Desc="Explore - Zamami"};
case 25: return new MainuiNoteEng{ID="MUI4A03",Desc="Explore - Tokashiki"};
case 26: return new MainuiNoteEng{ID="MUI4A04",Desc="Explore - Dorm of CPDC"};
case 27: return new MainuiNoteEng{ID="MUI4A05",Desc="Explore - Orchid"};
case 28: return new MainuiNoteEng{ID="MUI4A06",Desc="Explore - Taketomi"};
case 29: return new MainuiNoteEng{ID="MUI4AS1",Desc="Explore - Penghu"};
case 30: return new MainuiNoteEng{ID="MUI4AS2",Desc="Explore - Naha"};
case 31: return new MainuiNoteEng{ID="MUI4B01",Desc="Explore - South Island"};
case 32: return new MainuiNoteEng{ID="MUI4C01",Desc="Photo - Houton"};
case 33: return new MainuiNoteEng{ID="MUI4C02",Desc="Photo - Zamami"};
case 34: return new MainuiNoteEng{ID="MUI4C03",Desc="Photo - Tokashiki"};
case 35: return new MainuiNoteEng{ID="MUI4C04",Desc="Photo - Dorm of CPDC"};
case 36: return new MainuiNoteEng{ID="MUI4C05",Desc="Photo - Orchid"};
case 37: return new MainuiNoteEng{ID="MUI4C06",Desc="Photo - Taketomi"};
case 38: return new MainuiNoteEng{ID="MUI4CS1",Desc="Photo - Penghu"};
case 39: return new MainuiNoteEng{ID="MUI4CS2",Desc="Photo - Naha"};
case 40: return new MainuiNoteEng{ID="MUI5A01",Desc="Setting"};
case 41: return new MainuiNoteEng{ID="MUI5B01",Desc="Tutorial"};
case 42: return new MainuiNoteEng{ID="MUI5B02",Desc="Music SYNC Tool"};
case 43: return new MainuiNoteEng{ID="MUI5B03",Desc="Data Transfer"};
case 44: return new MainuiNoteEng{ID="MUI5B04",Desc="Language"};
case 45: return new MainuiNoteEng{ID="MUI5B05",Desc="Staff List"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}