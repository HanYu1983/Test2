using UnityEngine;
namespace Remix{
	public class IAPDef{
		public const string ID_IAP01 = "iap01";
public const string ID_IAP02 = "iap02";
public const string ID_IAP03 = "iap03";
public const string ID_IAP04 = "iap04";
public const string ID_IAP05 = "iap05";
public const string ID_IAP06 = "iap06";
public const string ID_IAP07 = "iap07";
public const string ID_IAP08 = "iap08";

		public string ID {get; set;}
public string Type {get; set;}
public string Prefab {get; set;}
public string Name {get; set;}
public string Desc {get; set;}
public int Cost {get; set;}
public int Gold {get; set;}
public int Money {get; set;}

		public static IAPDef Get(string key){
			switch (key) {
			case "iap01": return new IAPDef{ID="iap01",Type="IAP",Prefab="IAP01",Name="貓銀票",Desc="貓銀票   X   30",Cost=30,Gold=0,Money=30};
case "iap02": return new IAPDef{ID="iap02",Type="IAP",Prefab="IAP02",Name="一些貓銀票",Desc="貓銀票   X   100",Cost=90,Gold=0,Money=100};
case "iap03": return new IAPDef{ID="iap03",Type="IAP",Prefab="IAP03",Name="一堆貓銀票",Desc="貓銀票   X   210",Cost=180,Gold=0,Money=210};
case "iap04": return new IAPDef{ID="iap04",Type="IAP",Prefab="IAP04",Name="一箱貓銀票",Desc="貓銀票   X   450",Cost=360,Gold=0,Money=450};
case "iap05": return new IAPDef{ID="iap05",Type="IAP",Prefab="IAP05",Name="貓金幣",Desc="貓金幣   X   1500",Cost=30,Gold=1500,Money=0};
case "iap06": return new IAPDef{ID="iap06",Type="IAP",Prefab="IAP06",Name="一些貓金幣",Desc="貓金幣   X   5000",Cost=90,Gold=5000,Money=0};
case "iap07": return new IAPDef{ID="iap07",Type="IAP",Prefab="IAP07",Name="一堆貓金幣",Desc="貓金幣   X   10500",Cost=180,Gold=10500,Money=0};
case "iap08": return new IAPDef{ID="iap08",Type="IAP",Prefab="IAP08",Name="一箱貓金幣",Desc="貓金幣   X   21500",Cost=360,Gold=21500,Money=0};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}