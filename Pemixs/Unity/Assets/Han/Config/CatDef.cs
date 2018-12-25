using UnityEngine;
namespace Remix{
	public class CatDef{
		public const int ID_COUNT = 28;

		public string ID {get; set;}
public string Type {get; set;}
public string Name {get; set;}
public int Rare {get; set;}
public int HpRate {get; set;}
public float DHp {get; set;}
public float BHp {get; set;}
public float PHp {get; set;}
public float DDmg {get; set;}
public float BDmp {get; set;}
public float PDmg {get; set;}

		public static CatDef Get(string key){
			switch (key) {
			case "C01": return new CatDef{ID="C01",Type="CA",Name="C01",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.9f};
case "C02": return new CatDef{ID="C02",Type="CB",Name="C02",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.9f,BDmp=1f,PDmg=1f};
case "C03": return new CatDef{ID="C03",Type="CC",Name="C03",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.9f,PDmg=1f};
case "C04": return new CatDef{ID="C04",Type="CA",Name="C04",Rare=2,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.8f};
case "C05": return new CatDef{ID="C05",Type="CB",Name="C05",Rare=2,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.8f,BDmp=1f,PDmg=1f};
case "C06": return new CatDef{ID="C06",Type="CC",Name="C06",Rare=2,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.8f,PDmg=1f};
case "C07": return new CatDef{ID="C07",Type="CA",Name="CA3",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.9f};
case "C08": return new CatDef{ID="C08",Type="CB",Name="CB3",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.9f,BDmp=1f,PDmg=1f};
case "C09": return new CatDef{ID="C09",Type="CC",Name="CC3",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.9f,PDmg=1f};
case "C10": return new CatDef{ID="C10",Type="CA",Name="CA4",Rare=3,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.7f};
case "C11": return new CatDef{ID="C11",Type="CB",Name="CB4",Rare=3,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.7f,BDmp=1f,PDmg=1f};
case "C12": return new CatDef{ID="C12",Type="CC",Name="CC4",Rare=3,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.7f,PDmg=1f};
case "C13": return new CatDef{ID="C13",Type="CA",Name="CA5",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.9f};
case "C14": return new CatDef{ID="C14",Type="CB",Name="CB5",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.9f,BDmp=1f,PDmg=1f};
case "C15": return new CatDef{ID="C15",Type="CC",Name="CC5",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.9f,PDmg=1f};
case "C16": return new CatDef{ID="C16",Type="CA",Name="CA6",Rare=4,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.6f};
case "C17": return new CatDef{ID="C17",Type="CB",Name="CB6",Rare=4,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.6f,BDmp=1f,PDmg=1f};
case "C18": return new CatDef{ID="C18",Type="CC",Name="CC6",Rare=4,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.6f,PDmg=1f};
case "C19": return new CatDef{ID="C19",Type="CA",Name="CA7",Rare=2,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.8f};
case "C20": return new CatDef{ID="C20",Type="CB",Name="CB7",Rare=2,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.8f,BDmp=1f,PDmg=1f};
case "C21": return new CatDef{ID="C21",Type="CC",Name="CC7",Rare=2,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.8f,PDmg=1f};
case "C22": return new CatDef{ID="C22",Type="CA",Name="CA8",Rare=4,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.6f};
case "C23": return new CatDef{ID="C23",Type="CB",Name="CB8",Rare=4,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.6f,BDmp=1f,PDmg=1f};
case "C24": return new CatDef{ID="C24",Type="CC",Name="CC8",Rare=4,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.6f,PDmg=1f};
case "CM1": return new CatDef{ID="CM1",Type="CM",Name="CM1",Rare=5,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.5f,BDmp=1f,PDmg=1f};
case "CM2": return new CatDef{ID="CM2",Type="CM",Name="CM2",Rare=5,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.5f,PDmg=1f};
case "CM3": return new CatDef{ID="CM3",Type="CM",Name="CM3",Rare=6,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.5f};
case "CM4": return new CatDef{ID="CM4",Type="CM",Name="CM4",Rare=6,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.5f};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static CatDef Get(int key){
			switch (key) {
			case 0: return new CatDef{ID="C01",Type="CA",Name="C01",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.9f};
case 1: return new CatDef{ID="C02",Type="CB",Name="C02",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.9f,BDmp=1f,PDmg=1f};
case 2: return new CatDef{ID="C03",Type="CC",Name="C03",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.9f,PDmg=1f};
case 3: return new CatDef{ID="C04",Type="CA",Name="C04",Rare=2,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.8f};
case 4: return new CatDef{ID="C05",Type="CB",Name="C05",Rare=2,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.8f,BDmp=1f,PDmg=1f};
case 5: return new CatDef{ID="C06",Type="CC",Name="C06",Rare=2,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.8f,PDmg=1f};
case 6: return new CatDef{ID="C07",Type="CA",Name="CA3",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.9f};
case 7: return new CatDef{ID="C08",Type="CB",Name="CB3",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.9f,BDmp=1f,PDmg=1f};
case 8: return new CatDef{ID="C09",Type="CC",Name="CC3",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.9f,PDmg=1f};
case 9: return new CatDef{ID="C10",Type="CA",Name="CA4",Rare=3,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.7f};
case 10: return new CatDef{ID="C11",Type="CB",Name="CB4",Rare=3,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.7f,BDmp=1f,PDmg=1f};
case 11: return new CatDef{ID="C12",Type="CC",Name="CC4",Rare=3,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.7f,PDmg=1f};
case 12: return new CatDef{ID="C13",Type="CA",Name="CA5",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.9f};
case 13: return new CatDef{ID="C14",Type="CB",Name="CB5",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.9f,BDmp=1f,PDmg=1f};
case 14: return new CatDef{ID="C15",Type="CC",Name="CC5",Rare=1,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.9f,PDmg=1f};
case 15: return new CatDef{ID="C16",Type="CA",Name="CA6",Rare=4,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.6f};
case 16: return new CatDef{ID="C17",Type="CB",Name="CB6",Rare=4,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.6f,BDmp=1f,PDmg=1f};
case 17: return new CatDef{ID="C18",Type="CC",Name="CC6",Rare=4,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.6f,PDmg=1f};
case 18: return new CatDef{ID="C19",Type="CA",Name="CA7",Rare=2,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.8f};
case 19: return new CatDef{ID="C20",Type="CB",Name="CB7",Rare=2,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.8f,BDmp=1f,PDmg=1f};
case 20: return new CatDef{ID="C21",Type="CC",Name="CC7",Rare=2,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.8f,PDmg=1f};
case 21: return new CatDef{ID="C22",Type="CA",Name="CA8",Rare=4,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.6f};
case 22: return new CatDef{ID="C23",Type="CB",Name="CB8",Rare=4,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.6f,BDmp=1f,PDmg=1f};
case 23: return new CatDef{ID="C24",Type="CC",Name="CC8",Rare=4,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.6f,PDmg=1f};
case 24: return new CatDef{ID="CM1",Type="CM",Name="CM1",Rare=5,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=0.5f,BDmp=1f,PDmg=1f};
case 25: return new CatDef{ID="CM2",Type="CM",Name="CM2",Rare=5,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=0.5f,PDmg=1f};
case 26: return new CatDef{ID="CM3",Type="CM",Name="CM3",Rare=6,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.5f};
case 27: return new CatDef{ID="CM4",Type="CM",Name="CM4",Rare=6,HpRate=10,DHp=0f,BHp=0f,PHp=0f,DDmg=1f,BDmp=1f,PDmg=0.5f};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}