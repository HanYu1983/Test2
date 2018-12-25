using UnityEngine;
namespace Remix{
	public class IAPDefCht{
		public const int ID_COUNT = 9;

		public string ID {get; set;}
public string Type {get; set;}
public string Prefab {get; set;}
public string Name {get; set;}
public string Desc {get; set;}
public string Cost {get; set;}
public int Gold {get; set;}
public int Money {get; set;}

		public static IAPDefCht Get(string key){
			switch (key) {
			case "iap01": return new IAPDefCht{ID="iap01",Type="IAP",Prefab="IAP01",Name="貓銀票",Desc="貓銀票  X     30",Cost="NT.30",Gold=0,Money=30};
case "iap02": return new IAPDefCht{ID="iap02",Type="IAP",Prefab="IAP02",Name="一些貓銀票",Desc="貓銀票  X    100",Cost="NT.90",Gold=0,Money=100};
case "iap03": return new IAPDefCht{ID="iap03",Type="IAP",Prefab="IAP03",Name="一堆貓銀票",Desc="貓銀票  X    210",Cost="NT.180",Gold=0,Money=210};
case "iap04": return new IAPDefCht{ID="iap04",Type="IAP",Prefab="IAP04",Name="一箱貓銀票",Desc="貓銀票  X    450",Cost="NT.360",Gold=0,Money=450};
case "iap05": return new IAPDefCht{ID="iap05",Type="IAP",Prefab="IAP05",Name="貓金幣",Desc="貓金幣  X   1500",Cost="NT.30",Gold=1500,Money=0};
case "iap06": return new IAPDefCht{ID="iap06",Type="IAP",Prefab="IAP06",Name="一些貓金幣",Desc="貓金幣  X   5000",Cost="NT.90",Gold=5000,Money=0};
case "iap07": return new IAPDefCht{ID="iap07",Type="IAP",Prefab="IAP07",Name="一堆貓金幣",Desc="貓金幣  X  10500",Cost="NT.180",Gold=10500,Money=0};
case "iap08": return new IAPDefCht{ID="iap08",Type="IAP",Prefab="IAP08",Name="一箱貓金幣",Desc="貓金幣  X  21500",Cost="NT.360",Gold=21500,Money=0};
case "iap09": return new IAPDefCht{ID="iap09",Type="IAP",Prefab="IAP09",Name="消除廣告",Desc="消除底部橫幅廣告",Cost="NT.60",Gold=0,Money=0};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static IAPDefCht Get(int key){
			switch (key) {
			case 0: return new IAPDefCht{ID="iap01",Type="IAP",Prefab="IAP01",Name="貓銀票",Desc="貓銀票  X     30",Cost="NT.30",Gold=0,Money=30};
case 1: return new IAPDefCht{ID="iap02",Type="IAP",Prefab="IAP02",Name="一些貓銀票",Desc="貓銀票  X    100",Cost="NT.90",Gold=0,Money=100};
case 2: return new IAPDefCht{ID="iap03",Type="IAP",Prefab="IAP03",Name="一堆貓銀票",Desc="貓銀票  X    210",Cost="NT.180",Gold=0,Money=210};
case 3: return new IAPDefCht{ID="iap04",Type="IAP",Prefab="IAP04",Name="一箱貓銀票",Desc="貓銀票  X    450",Cost="NT.360",Gold=0,Money=450};
case 4: return new IAPDefCht{ID="iap05",Type="IAP",Prefab="IAP05",Name="貓金幣",Desc="貓金幣  X   1500",Cost="NT.30",Gold=1500,Money=0};
case 5: return new IAPDefCht{ID="iap06",Type="IAP",Prefab="IAP06",Name="一些貓金幣",Desc="貓金幣  X   5000",Cost="NT.90",Gold=5000,Money=0};
case 6: return new IAPDefCht{ID="iap07",Type="IAP",Prefab="IAP07",Name="一堆貓金幣",Desc="貓金幣  X  10500",Cost="NT.180",Gold=10500,Money=0};
case 7: return new IAPDefCht{ID="iap08",Type="IAP",Prefab="IAP08",Name="一箱貓金幣",Desc="貓金幣  X  21500",Cost="NT.360",Gold=21500,Money=0};
case 8: return new IAPDefCht{ID="iap09",Type="IAP",Prefab="IAP09",Name="消除廣告",Desc="消除底部橫幅廣告",Cost="NT.60",Gold=0,Money=0};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}