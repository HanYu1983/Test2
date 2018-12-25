using UnityEngine;
namespace Remix{
	public class IAPDefEng{
		public const int ID_COUNT = 9;

		public string ID {get; set;}
public string Type {get; set;}
public string Prefab {get; set;}
public string Name {get; set;}
public string Desc {get; set;}
public string Cost {get; set;}
public int Gold {get; set;}
public int Money {get; set;}

		public static IAPDefEng Get(string key){
			switch (key) {
			case "iap01": return new IAPDefEng{ID="iap01",Type="IAP",Prefab="IAP01",Name="Cat Money",Desc="Cat Money X 30",Cost="0.99 USD",Gold=0,Money=30};
case "iap02": return new IAPDefEng{ID="iap02",Type="IAP",Prefab="IAP02",Name="Cat Money",Desc="Cat Money X 100",Cost="2.99 USD",Gold=0,Money=100};
case "iap03": return new IAPDefEng{ID="iap03",Type="IAP",Prefab="IAP03",Name="Cat Money",Desc="Cat Money X 210",Cost="5.99 USD",Gold=0,Money=210};
case "iap04": return new IAPDefEng{ID="iap04",Type="IAP",Prefab="IAP04",Name="Cat Money",Desc="Cat Money X 450",Cost="11.99 USD",Gold=0,Money=450};
case "iap05": return new IAPDefEng{ID="iap05",Type="IAP",Prefab="IAP05",Name="Cat Gold",Desc="Cat Gold X 1500",Cost="0.99 USD",Gold=1500,Money=0};
case "iap06": return new IAPDefEng{ID="iap06",Type="IAP",Prefab="IAP06",Name="Cat Gold",Desc="Cat Gold X 5000",Cost="2.99 USD",Gold=5000,Money=0};
case "iap07": return new IAPDefEng{ID="iap07",Type="IAP",Prefab="IAP07",Name="Cat Gold",Desc="Cat Gold X 10500",Cost="5.99 USD",Gold=10500,Money=0};
case "iap08": return new IAPDefEng{ID="iap08",Type="IAP",Prefab="IAP08",Name="Cat Gold",Desc="Cat Gold X 21500",Cost="11.99 USD",Gold=21500,Money=0};
case "iap09": return new IAPDefEng{ID="iap09",Type="IAP",Prefab="IAP09",Name="Remove ADs",Desc="Remove ADs",Cost="1.99 USD",Gold=0,Money=0};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static IAPDefEng Get(int key){
			switch (key) {
			case 0: return new IAPDefEng{ID="iap01",Type="IAP",Prefab="IAP01",Name="Cat Money",Desc="Cat Money X 30",Cost="0.99 USD",Gold=0,Money=30};
case 1: return new IAPDefEng{ID="iap02",Type="IAP",Prefab="IAP02",Name="Cat Money",Desc="Cat Money X 100",Cost="2.99 USD",Gold=0,Money=100};
case 2: return new IAPDefEng{ID="iap03",Type="IAP",Prefab="IAP03",Name="Cat Money",Desc="Cat Money X 210",Cost="5.99 USD",Gold=0,Money=210};
case 3: return new IAPDefEng{ID="iap04",Type="IAP",Prefab="IAP04",Name="Cat Money",Desc="Cat Money X 450",Cost="11.99 USD",Gold=0,Money=450};
case 4: return new IAPDefEng{ID="iap05",Type="IAP",Prefab="IAP05",Name="Cat Gold",Desc="Cat Gold X 1500",Cost="0.99 USD",Gold=1500,Money=0};
case 5: return new IAPDefEng{ID="iap06",Type="IAP",Prefab="IAP06",Name="Cat Gold",Desc="Cat Gold X 5000",Cost="2.99 USD",Gold=5000,Money=0};
case 6: return new IAPDefEng{ID="iap07",Type="IAP",Prefab="IAP07",Name="Cat Gold",Desc="Cat Gold X 10500",Cost="5.99 USD",Gold=10500,Money=0};
case 7: return new IAPDefEng{ID="iap08",Type="IAP",Prefab="IAP08",Name="Cat Gold",Desc="Cat Gold X 21500",Cost="11.99 USD",Gold=21500,Money=0};
case 8: return new IAPDefEng{ID="iap09",Type="IAP",Prefab="IAP09",Name="Remove ADs",Desc="Remove ADs",Cost="1.99 USD",Gold=0,Money=0};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}