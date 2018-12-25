using UnityEngine;
namespace Remix{
	public class IAPDefChs{
		public const int ID_COUNT = 9;

		public string ID {get; set;}
public string Type {get; set;}
public string Prefab {get; set;}
public string Name {get; set;}
public string Desc {get; set;}
public string Cost {get; set;}
public int Gold {get; set;}
public int Money {get; set;}

		public static IAPDefChs Get(string key){
			switch (key) {
			case "iap01": return new IAPDefChs{ID="iap01",Type="IAP",Prefab="IAP01",Name="猫银票",Desc="猫银票 X 30",Cost="1 美元",Gold=0,Money=30};
case "iap02": return new IAPDefChs{ID="iap02",Type="IAP",Prefab="IAP02",Name="一些猫银票",Desc="猫银票 X 100",Cost="3 美元",Gold=0,Money=100};
case "iap03": return new IAPDefChs{ID="iap03",Type="IAP",Prefab="IAP03",Name="一堆猫银票",Desc="猫银票 X 210",Cost="6 美元",Gold=0,Money=210};
case "iap04": return new IAPDefChs{ID="iap04",Type="IAP",Prefab="IAP04",Name="一箱猫银票",Desc="猫银票 X 450",Cost="12 美元",Gold=0,Money=450};
case "iap05": return new IAPDefChs{ID="iap05",Type="IAP",Prefab="IAP05",Name="猫金币",Desc="猫金币 X 1500",Cost="1 美元",Gold=1500,Money=0};
case "iap06": return new IAPDefChs{ID="iap06",Type="IAP",Prefab="IAP06",Name="一些猫金币",Desc="猫金币 X 5000",Cost="3 美元",Gold=5000,Money=0};
case "iap07": return new IAPDefChs{ID="iap07",Type="IAP",Prefab="IAP07",Name="一堆猫金币",Desc="猫金币 X 10500",Cost="6 美元",Gold=10500,Money=0};
case "iap08": return new IAPDefChs{ID="iap08",Type="IAP",Prefab="IAP08",Name="一箱猫金币",Desc="猫金币 X 21500",Cost="12 美元",Gold=21500,Money=0};
case "iap09": return new IAPDefChs{ID="iap09",Type="IAP",Prefab="IAP09",Name="消除广告",Desc="消除底部横幅广告",Cost="2 美元",Gold=0,Money=0};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static IAPDefChs Get(int key){
			switch (key) {
			case 0: return new IAPDefChs{ID="iap01",Type="IAP",Prefab="IAP01",Name="猫银票",Desc="猫银票 X 30",Cost="1 美元",Gold=0,Money=30};
case 1: return new IAPDefChs{ID="iap02",Type="IAP",Prefab="IAP02",Name="一些猫银票",Desc="猫银票 X 100",Cost="3 美元",Gold=0,Money=100};
case 2: return new IAPDefChs{ID="iap03",Type="IAP",Prefab="IAP03",Name="一堆猫银票",Desc="猫银票 X 210",Cost="6 美元",Gold=0,Money=210};
case 3: return new IAPDefChs{ID="iap04",Type="IAP",Prefab="IAP04",Name="一箱猫银票",Desc="猫银票 X 450",Cost="12 美元",Gold=0,Money=450};
case 4: return new IAPDefChs{ID="iap05",Type="IAP",Prefab="IAP05",Name="猫金币",Desc="猫金币 X 1500",Cost="1 美元",Gold=1500,Money=0};
case 5: return new IAPDefChs{ID="iap06",Type="IAP",Prefab="IAP06",Name="一些猫金币",Desc="猫金币 X 5000",Cost="3 美元",Gold=5000,Money=0};
case 6: return new IAPDefChs{ID="iap07",Type="IAP",Prefab="IAP07",Name="一堆猫金币",Desc="猫金币 X 10500",Cost="6 美元",Gold=10500,Money=0};
case 7: return new IAPDefChs{ID="iap08",Type="IAP",Prefab="IAP08",Name="一箱猫金币",Desc="猫金币 X 21500",Cost="12 美元",Gold=21500,Money=0};
case 8: return new IAPDefChs{ID="iap09",Type="IAP",Prefab="IAP09",Name="消除广告",Desc="消除底部横幅广告",Cost="2 美元",Gold=0,Money=0};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}