using UnityEngine;
namespace Remix{
	public class MainuiNoteCht{
		public const int ID_COUNT = 46;

		public string ID {get; set;}
public string Desc {get; set;}

		public static MainuiNoteCht Get(string key){
			switch (key) {
			case "MUI1A01": return new MainuiNoteCht{ID="MUI1A01",Desc="玩家首頁"};
case "MUI1B01": return new MainuiNoteCht{ID="MUI1B01",Desc="社群頁面"};
case "MUI1C01": return new MainuiNoteCht{ID="MUI1C01",Desc="貓窩 - 01"};
case "MUI1C02": return new MainuiNoteCht{ID="MUI1C02",Desc="貓窩 - 02"};
case "MUI1C03": return new MainuiNoteCht{ID="MUI1C03",Desc="貓窩 - 03"};
case "MUI1C04": return new MainuiNoteCht{ID="MUI1C04",Desc="貓窩 - 04"};
case "MUI1C05": return new MainuiNoteCht{ID="MUI1C05",Desc="貓窩 - 05"};
case "MUI1C06": return new MainuiNoteCht{ID="MUI1C06",Desc="貓窩 - 06"};
case "MUI1CS1": return new MainuiNoteCht{ID="MUI1CS1",Desc="貓窩 - S1"};
case "MUI1CS2": return new MainuiNoteCht{ID="MUI1CS2",Desc="貓窩 - S2"};
case "MUI2A01": return new MainuiNoteCht{ID="MUI2A01",Desc="貓商店"};
case "MUI2B01": return new MainuiNoteCht{ID="MUI2B01",Desc="貓銀行"};
case "MUI2C01": return new MainuiNoteCht{ID="MUI2C01",Desc="貓唱片行"};
case "MUI3A01": return new MainuiNoteCht{ID="MUI3A01",Desc="地圖 - 猴硐"};
case "MUI3A02": return new MainuiNoteCht{ID="MUI3A02",Desc="地圖 - 座間味"};
case "MUI3A03": return new MainuiNoteCht{ID="MUI3A03",Desc="地圖 - 渡嘉敷"};
case "MUI3A04": return new MainuiNoteCht{ID="MUI3A04",Desc="地圖 - 台鹼宿舍"};
case "MUI3A05": return new MainuiNoteCht{ID="MUI3A05",Desc="地圖 - 蘭嶼"};
case "MUI3A06": return new MainuiNoteCht{ID="MUI3A06",Desc="地圖 - 竹富"};
case "MUI3AS1": return new MainuiNoteCht{ID="MUI3AS1",Desc="地圖 - 澎湖"};
case "MUI3AS2": return new MainuiNoteCht{ID="MUI3AS2",Desc="地圖 - 那霸"};
case "MUI3B01": return new MainuiNoteCht{ID="MUI3B01",Desc="地圖 - 南之島"};
case "MUI3C01": return new MainuiNoteCht{ID="MUI3C01",Desc="地圖 - 未開放"};
case "MUI4A01": return new MainuiNoteCht{ID="MUI4A01",Desc="探索 - 猴硐"};
case "MUI4A02": return new MainuiNoteCht{ID="MUI4A02",Desc="探索 - 座間味"};
case "MUI4A03": return new MainuiNoteCht{ID="MUI4A03",Desc="探索 - 渡嘉敷"};
case "MUI4A04": return new MainuiNoteCht{ID="MUI4A04",Desc="探索 - 台鹼宿舍"};
case "MUI4A05": return new MainuiNoteCht{ID="MUI4A05",Desc="探索 - 蘭嶼"};
case "MUI4A06": return new MainuiNoteCht{ID="MUI4A06",Desc="探索 - 竹富"};
case "MUI4AS1": return new MainuiNoteCht{ID="MUI4AS1",Desc="探索 - 澎湖"};
case "MUI4AS2": return new MainuiNoteCht{ID="MUI4AS2",Desc="探索 - 那霸"};
case "MUI4B01": return new MainuiNoteCht{ID="MUI4B01",Desc="探索 - 南之島"};
case "MUI4C01": return new MainuiNoteCht{ID="MUI4C01",Desc="照片 - 猴硐"};
case "MUI4C02": return new MainuiNoteCht{ID="MUI4C02",Desc="照片 - 座間味"};
case "MUI4C03": return new MainuiNoteCht{ID="MUI4C03",Desc="照片 - 渡嘉敷"};
case "MUI4C04": return new MainuiNoteCht{ID="MUI4C04",Desc="照片 - 台鹼宿舍"};
case "MUI4C05": return new MainuiNoteCht{ID="MUI4C05",Desc="照片 - 蘭嶼"};
case "MUI4C06": return new MainuiNoteCht{ID="MUI4C06",Desc="照片 - 竹富"};
case "MUI4CS1": return new MainuiNoteCht{ID="MUI4CS1",Desc="照片 - 澎湖"};
case "MUI4CS2": return new MainuiNoteCht{ID="MUI4CS2",Desc="照片 - 那霸"};
case "MUI5A01": return new MainuiNoteCht{ID="MUI5A01",Desc="設定"};
case "MUI5B01": return new MainuiNoteCht{ID="MUI5B01",Desc="遊戲教學"};
case "MUI5B02": return new MainuiNoteCht{ID="MUI5B02",Desc="音樂同步工具"};
case "MUI5B03": return new MainuiNoteCht{ID="MUI5B03",Desc="資料轉移"};
case "MUI5B04": return new MainuiNoteCht{ID="MUI5B04",Desc="語言選擇"};
case "MUI5B05": return new MainuiNoteCht{ID="MUI5B05",Desc="製作人員名單"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static MainuiNoteCht Get(int key){
			switch (key) {
			case 0: return new MainuiNoteCht{ID="MUI1A01",Desc="玩家首頁"};
case 1: return new MainuiNoteCht{ID="MUI1B01",Desc="社群頁面"};
case 2: return new MainuiNoteCht{ID="MUI1C01",Desc="貓窩 - 01"};
case 3: return new MainuiNoteCht{ID="MUI1C02",Desc="貓窩 - 02"};
case 4: return new MainuiNoteCht{ID="MUI1C03",Desc="貓窩 - 03"};
case 5: return new MainuiNoteCht{ID="MUI1C04",Desc="貓窩 - 04"};
case 6: return new MainuiNoteCht{ID="MUI1C05",Desc="貓窩 - 05"};
case 7: return new MainuiNoteCht{ID="MUI1C06",Desc="貓窩 - 06"};
case 8: return new MainuiNoteCht{ID="MUI1CS1",Desc="貓窩 - S1"};
case 9: return new MainuiNoteCht{ID="MUI1CS2",Desc="貓窩 - S2"};
case 10: return new MainuiNoteCht{ID="MUI2A01",Desc="貓商店"};
case 11: return new MainuiNoteCht{ID="MUI2B01",Desc="貓銀行"};
case 12: return new MainuiNoteCht{ID="MUI2C01",Desc="貓唱片行"};
case 13: return new MainuiNoteCht{ID="MUI3A01",Desc="地圖 - 猴硐"};
case 14: return new MainuiNoteCht{ID="MUI3A02",Desc="地圖 - 座間味"};
case 15: return new MainuiNoteCht{ID="MUI3A03",Desc="地圖 - 渡嘉敷"};
case 16: return new MainuiNoteCht{ID="MUI3A04",Desc="地圖 - 台鹼宿舍"};
case 17: return new MainuiNoteCht{ID="MUI3A05",Desc="地圖 - 蘭嶼"};
case 18: return new MainuiNoteCht{ID="MUI3A06",Desc="地圖 - 竹富"};
case 19: return new MainuiNoteCht{ID="MUI3AS1",Desc="地圖 - 澎湖"};
case 20: return new MainuiNoteCht{ID="MUI3AS2",Desc="地圖 - 那霸"};
case 21: return new MainuiNoteCht{ID="MUI3B01",Desc="地圖 - 南之島"};
case 22: return new MainuiNoteCht{ID="MUI3C01",Desc="地圖 - 未開放"};
case 23: return new MainuiNoteCht{ID="MUI4A01",Desc="探索 - 猴硐"};
case 24: return new MainuiNoteCht{ID="MUI4A02",Desc="探索 - 座間味"};
case 25: return new MainuiNoteCht{ID="MUI4A03",Desc="探索 - 渡嘉敷"};
case 26: return new MainuiNoteCht{ID="MUI4A04",Desc="探索 - 台鹼宿舍"};
case 27: return new MainuiNoteCht{ID="MUI4A05",Desc="探索 - 蘭嶼"};
case 28: return new MainuiNoteCht{ID="MUI4A06",Desc="探索 - 竹富"};
case 29: return new MainuiNoteCht{ID="MUI4AS1",Desc="探索 - 澎湖"};
case 30: return new MainuiNoteCht{ID="MUI4AS2",Desc="探索 - 那霸"};
case 31: return new MainuiNoteCht{ID="MUI4B01",Desc="探索 - 南之島"};
case 32: return new MainuiNoteCht{ID="MUI4C01",Desc="照片 - 猴硐"};
case 33: return new MainuiNoteCht{ID="MUI4C02",Desc="照片 - 座間味"};
case 34: return new MainuiNoteCht{ID="MUI4C03",Desc="照片 - 渡嘉敷"};
case 35: return new MainuiNoteCht{ID="MUI4C04",Desc="照片 - 台鹼宿舍"};
case 36: return new MainuiNoteCht{ID="MUI4C05",Desc="照片 - 蘭嶼"};
case 37: return new MainuiNoteCht{ID="MUI4C06",Desc="照片 - 竹富"};
case 38: return new MainuiNoteCht{ID="MUI4CS1",Desc="照片 - 澎湖"};
case 39: return new MainuiNoteCht{ID="MUI4CS2",Desc="照片 - 那霸"};
case 40: return new MainuiNoteCht{ID="MUI5A01",Desc="設定"};
case 41: return new MainuiNoteCht{ID="MUI5B01",Desc="遊戲教學"};
case 42: return new MainuiNoteCht{ID="MUI5B02",Desc="音樂同步工具"};
case 43: return new MainuiNoteCht{ID="MUI5B03",Desc="資料轉移"};
case 44: return new MainuiNoteCht{ID="MUI5B04",Desc="語言選擇"};
case 45: return new MainuiNoteCht{ID="MUI5B05",Desc="製作人員名單"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}