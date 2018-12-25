using UnityEngine;
namespace Remix{
	public class ItemDef{
		public const string I10010 = "I10010";
public const string I10020 = "I10020";
public const string I10030 = "I10030";
public const string I10040 = "I10040";
public const string I10050 = "I10050";
public const string I10060 = "I10060";
public const string I10070 = "I10070";
public const string I10080 = "I10080";
public const string I20010 = "I20010";
public const string I20020 = "I20020";
public const string I20030 = "I20030";
public const string I20040 = "I20040";
public const string I20050 = "I20050";
public const string I20060 = "I20060";
public const string I20070 = "I20070";
public const string I20080 = "I20080";
public const string I21010 = "I21010";
public const string I21020 = "I21020";
public const string I30010 = "I30010";
public const string I30020 = "I30020";
public const string I30030 = "I30030";
public const string I30040 = "I30040";
public const string I30050 = "I30050";
public const string I30060 = "I30060";
public const string I30070 = "I30070";
public const string I30080 = "I30080";
public const string I30090 = "I30090";
public const string I30100 = "I30100";
public const string I30110 = "I30110";
public const string I31010 = "I31010";
public const string I31020 = "I31020";
public const string I32010 = "I32010";
public const string I32020 = "I32020";
public const string I32030 = "I32030";
public const string I40010 = "I40010";
public const string I40020 = "I40020";
public const string I40030 = "I40030";
public const string I40040 = "I40040";
public const string I40050 = "I40050";
public const string I41010 = "I41010";
public const string I41020 = "I41020";
public const string I41030 = "I41030";
public const string I41040 = "I41040";
public const string I41050 = "I41050";
public const string I41060 = "I41060";
public const string I41070 = "I41070";
public const string I41080 = "I41080";
public const string I41090 = "I41090";
public const string I41100 = "I41100";
public const string I41110 = "I41110";
public const string I41120 = "I41120";
public const string I50010 = "I50010";
public const string I50020 = "I50020";
public const string I50030 = "I50030";
public const string I60010 = "I60010";
public const string I60020 = "I60020";
public const string I60030 = "I60030";
public const string I62010 = "I62010";
public const int ID_COUNT = 58;

		public string ID {get; set;}
public string Type {get; set;}
public string Prefab {get; set;}
public string Name {get; set;}
public string Desc {get; set;}
public int Gold {get; set;}
public int Money {get; set;}
public int Enable {get; set;}
public int Hp {get; set;}
public int Exp {get; set;}
public float Time {get; set;}
public float ToPhoto {get; set;}
public float BPhoto {get; set;}
public int CatTypeCA {get; set;}
public int CatTypeCB {get; set;}
public int CatTypeCC {get; set;}
public int CatTypeCD {get; set;}
public int CatTypeCE {get; set;}
public int CatTypeCF {get; set;}
public int CatTypeCM {get; set;}
public int CatTypeCN {get; set;}

		public static ItemDef Get(string key){
			switch (key) {
			case "I10010": return new ItemDef{ID="I10010",Type="F",Prefab="FIT01",Name="罐頭",Desc="在互動模式與遊玩關卡食用，可恢復HP50。",Gold=50,Money=0,Enable=1,Hp=50,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=1,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I10020": return new ItemDef{ID="I10020",Type="F",Prefab="FIT02",Name="牛奶",Desc="在互動模式與遊玩關卡食用，可恢復HP150。",Gold=150,Money=0,Enable=1,Hp=150,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=1,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I10030": return new ItemDef{ID="I10030",Type="F",Prefab="FIT03",Name="乾飼料",Desc="在互動模式與遊玩關卡食用，可恢復HP300。",Gold=300,Money=0,Enable=1,Hp=300,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=1,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I10040": return new ItemDef{ID="I10040",Type="F",Prefab="FIT04",Name="貓草",Desc="在互動模式與遊玩關卡食用，可恢復HP500。",Gold=500,Money=0,Enable=1,Hp=500,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=1,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I10050": return new ItemDef{ID="I10050",Type="F",Prefab="FIT05",Name="小魚乾",Desc="在互動模式與遊玩關卡食用，可恢復HP250。",Gold=250,Money=0,Enable=1,Hp=250,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=1,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I10060": return new ItemDef{ID="I10060",Type="F",Prefab="FIT06",Name="柴魚",Desc="在互動模式與遊玩關卡食用，可恢復HP500。",Gold=500,Money=0,Enable=1,Hp=500,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=1,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I10070": return new ItemDef{ID="I10070",Type="F",Prefab="FIT07",Name="鮮魚",Desc="在互動模式與遊玩關卡食用，可恢復HP500並增加EXP500。",Gold=0,Money=10,Enable=1,Hp=500,Exp=500,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=1,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I10080": return new ItemDef{ID="I10080",Type="F",Prefab="FIT08",Name="高級餐包",Desc="在互動模式與遊玩關卡食用，可恢復HP1000並增加EXP1000。",Gold=0,Money=20,Enable=1,Hp=1000,Exp=1000,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=1,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I20010": return new ItemDef{ID="I20010",Type="C",Prefab="CIT01",Name="小型數位相機",Desc="在探索模式使用，可獲得一張一般照片。",Gold=200,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I20020": return new ItemDef{ID="I20020",Type="C",Prefab="CIT02",Name="拍立得相機",Desc="在探索模式使用，可獲得一張一般照片，並減少等待時間50%。",Gold=450,Money=0,Enable=1,Hp=0,Exp=0,Time=0.5f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I20030": return new ItemDef{ID="I20030",Type="C",Prefab="CIT03",Name="運動相機",Desc="在探索模式使用，可獲得一張一般照片，並減少等待時間100%。",Gold=700,Money=0,Enable=1,Hp=0,Exp=0,Time=1f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I20040": return new ItemDef{ID="I20040",Type="C",Prefab="CIT04",Name="單眼相機",Desc="在探索模式使用，可獲得一張一般照片，並有20%機率獲得環景照片。",Gold=400,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0.2f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I20050": return new ItemDef{ID="I20050",Type="C",Prefab="CIT05",Name="數位單眼相機",Desc="在探索模式使用，可獲得一張一般照片，並有40%機率獲得環景照片。",Gold=600,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0.4f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I20060": return new ItemDef{ID="I20060",Type="C",Prefab="CIT06",Name="蛇腹相機",Desc="在探索模式使用，可獲得一張一般照片，並有50%機率獲得第二張照片。",Gold=0,Money=8,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0.5f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I20070": return new ItemDef{ID="I20070",Type="C",Prefab="CIT07",Name="雙眼相機",Desc="在探索模式使用，可獲得二張一般照片。",Gold=0,Money=12,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=1f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I20080": return new ItemDef{ID="I20080",Type="C",Prefab="CIT08",Name="復古相機",Desc="在探索模式使用，可獲得二張一般照片，並有40%機率獲得環景照片。",Gold=0,Money=16,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=1f,BPhoto=0.4f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I21010": return new ItemDef{ID="I21010",Type="SC",Prefab="SCIT01",Name="自拍棒",Desc="在探索模式使用，可減少拍攝等待時間50%",Gold=0,Money=5,Enable=1,Hp=0,Exp=0,Time=0.5f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I21020": return new ItemDef{ID="I21020",Type="SC",Prefab="SCIT02",Name="相機腳架",Desc="在探索模式使用，可減少拍攝等待時間100%。",Gold=0,Money=10,Enable=1,Hp=0,Exp=0,Time=1f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I30010": return new ItemDef{ID="I30010",Type="T",Prefab="TIT01",Name="雷射光筆",Desc="成貓體型專用玩具，在互動模式使用，可增加經驗值50。",Gold=400,Money=0,Enable=1,Hp=0,Exp=50,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I30020": return new ItemDef{ID="I30020",Type="T",Prefab="TIT02",Name="玩具老鼠",Desc="成貓體型專用玩具，在互動模式使用，可增加經驗值200。",Gold=600,Money=0,Enable=1,Hp=0,Exp=200,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I30030": return new ItemDef{ID="I30030",Type="T",Prefab="TIT03",Name="取消中",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I30040": return new ItemDef{ID="I30040",Type="T",Prefab="TIT04",Name="逗貓棒",Desc="幼貓體型專用玩具，在互動模式使用，增加經驗值50。",Gold=400,Money=0,Enable=1,Hp=0,Exp=50,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=1,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I30050": return new ItemDef{ID="I30050",Type="T",Prefab="TIT05",Name="毛線球",Desc="幼貓體型專用玩具，在互動模式使用，增加經驗值200。",Gold=600,Money=0,Enable=1,Hp=0,Exp=200,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=1,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I30060": return new ItemDef{ID="I30060",Type="T",Prefab="TIT06",Name="取消中",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I30070": return new ItemDef{ID="I30070",Type="T",Prefab="TIT07",Name="貓抓板",Desc="胖貓體型專用玩具，在互動模式使用，增加經驗值50。",Gold=400,Money=0,Enable=1,Hp=0,Exp=50,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I30080": return new ItemDef{ID="I30080",Type="T",Prefab="TIT08",Name="暖爐",Desc="胖貓體型專用玩具，在互動模式使用，增加經驗值200。",Gold=600,Money=0,Enable=1,Hp=0,Exp=200,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I30090": return new ItemDef{ID="I30090",Type="T",Prefab="TIT09",Name="取消中",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I30100": return new ItemDef{ID="I30100",Type="T",Name="取消中",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I30110": return new ItemDef{ID="I30110",Type="T",Name="取消中",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I31010": return new ItemDef{ID="I31010",Type="ST",Prefab="STIT01",Name="貓草噴劑",Desc="在探索模式與互動模式使用，可減少等待時間50%。",Gold=0,Money=10,Enable=1,Hp=0,Exp=0,Time=0.5f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I31020": return new ItemDef{ID="I31020",Type="ST",Prefab="STIT02",Name="罐裝乾貓草",Desc="在探索模式與互動模式使用，可減少等待時間100%。",Gold=0,Money=20,Enable=1,Hp=0,Exp=0,Time=1f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I32010": return new ItemDef{ID="I32010",Type="TC",Prefab="TCIT01",Name="宅配紙箱",Desc="成貓體型專用道具，在探索模式使用，可捕捉到成貓。",Gold=0,Money=16,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I32020": return new ItemDef{ID="I32020",Type="TC",Prefab="TCIT02",Name="掃地機器人",Desc="幼貓體型專用道具，在探索模式使用，可捕捉到幼貓。",Gold=0,Money=16,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=1,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I32030": return new ItemDef{ID="I32030",Type="TC",Prefab="TCIT03",Name="沙發床",Desc="胖貓體型專用道具，在探索模式使用，可捕捉到胖貓。",Gold=0,Money=16,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I40010": return new ItemDef{ID="I40010",Type="MG",Prefab="MGIT01",Name="貓金幣 250",Desc="獲得貓金幣 250",Gold=-250,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I40020": return new ItemDef{ID="I40020",Type="MG",Prefab="MGIT02",Name="貓金幣 500",Desc="獲得貓金幣 500",Gold=-500,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I40030": return new ItemDef{ID="I40030",Type="MG",Prefab="MGIT03",Name="貓金幣 750",Desc="獲得貓金幣 750",Gold=-750,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I40040": return new ItemDef{ID="I40040",Type="MG",Prefab="MGIT04",Name="貓金幣 1000",Desc="獲得貓金幣 1000",Gold=-1000,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I40050": return new ItemDef{ID="I40050",Type="MG",Prefab="MGIT05",Name="貓金幣 1500",Desc="獲得貓金幣 1500",Gold=-1500,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I41010": return new ItemDef{ID="I41010",Type="MM",Prefab="MMIT01",Name="貓銀票 20",Desc="獲得貓銀票 20",Gold=0,Money=-20,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I41020": return new ItemDef{ID="I41020",Type="MM",Prefab="MMIT02",Name="貓銀票 40",Desc="獲得貓銀票 40",Gold=0,Money=-40,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I41030": return new ItemDef{ID="I41030",Type="MM",Prefab="MMIT03",Name="貓銀票 10",Desc="獲得貓銀票 10",Gold=0,Money=-10,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I41040": return new ItemDef{ID="I41040",Type="MM",Prefab="MMIT04",Name="貓銀票 5",Desc="獲得貓銀票 5",Gold=0,Money=-5,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I41050": return new ItemDef{ID="I41050",Type="MM",Prefab="MMIT05",Name="貓銀票 2",Desc="獲得貓銀票 2",Gold=0,Money=-2,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I41060": return new ItemDef{ID="I41060",Type="MM",Prefab="MMIT06",Name="貓銀票 1",Desc="獲得貓銀票 1",Gold=0,Money=-1,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I41070": return new ItemDef{ID="I41070",Name="預留空位",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I41080": return new ItemDef{ID="I41080",Name="預留空位",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I41090": return new ItemDef{ID="I41090",Name="預留空位",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I41100": return new ItemDef{ID="I41100",Name="預留空位",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I41110": return new ItemDef{ID="I41110",Name="預留空位",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I41120": return new ItemDef{ID="I41120",Name="預留空位",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I50010": return new ItemDef{ID="I50010",Type="F",Prefab="FIT10",Name="鮭魚",Desc="大型貓專用食物，在互動模式與遊玩關卡食用，可恢復HP150。",Gold=500,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=1,CatTypeCN=1};
case "I50020": return new ItemDef{ID="I50020",Type="F",Prefab="FIT11",Name="帶骨肉",Desc="大型貓專用食物，在互動模式與遊玩關卡食用，可恢復HP300。",Gold=1000,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=1,CatTypeCN=1};
case "I50030": return new ItemDef{ID="I50030",Type="F",Prefab="FIT12",Name="烤乳豬",Desc="大型貓專用食物，在互動模式與遊玩關卡食用，可恢復HP1000並增加EXP500。",Gold=0,Money=30,Enable=1,Hp=0,Exp=500,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=1,CatTypeCN=1};
case "I60010": return new ItemDef{ID="I60010",Type="T",Prefab="TIT10",Name="南瓜",Desc="猞猁體型專用玩具，在互動模式使用，可增加經驗值100。",Gold=1000,Money=0,Enable=1,Hp=0,Exp=100,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=1,CatTypeCN=0};
case "I60020": return new ItemDef{ID="I60020",Type="T",Prefab="TIT11",Name="木人樁",Desc="猞猁體型專用玩具，在互動模式使用，可增加經驗值300。",Gold=3000,Money=0,Enable=1,Hp=0,Exp=300,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=1,CatTypeCN=0};
case "I60030": return new ItemDef{ID="I60030",Type="T",Prefab="TIT12",Name="取消中",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case "I62010": return new ItemDef{ID="I62010",Type="TC",Prefab="TCIT04",Name="雪橇",Desc="猞猁體型專用道具，在探索模式使用，可捕捉到猞猁。",Gold=0,Money=24,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=1,CatTypeCN=0};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static ItemDef Get(int key){
			switch (key) {
			case 0: return new ItemDef{ID="I10010",Type="F",Prefab="FIT01",Name="罐頭",Desc="在互動模式與遊玩關卡食用，可恢復HP50。",Gold=50,Money=0,Enable=1,Hp=50,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=1,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 1: return new ItemDef{ID="I10020",Type="F",Prefab="FIT02",Name="牛奶",Desc="在互動模式與遊玩關卡食用，可恢復HP150。",Gold=150,Money=0,Enable=1,Hp=150,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=1,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 2: return new ItemDef{ID="I10030",Type="F",Prefab="FIT03",Name="乾飼料",Desc="在互動模式與遊玩關卡食用，可恢復HP300。",Gold=300,Money=0,Enable=1,Hp=300,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=1,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 3: return new ItemDef{ID="I10040",Type="F",Prefab="FIT04",Name="貓草",Desc="在互動模式與遊玩關卡食用，可恢復HP500。",Gold=500,Money=0,Enable=1,Hp=500,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=1,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 4: return new ItemDef{ID="I10050",Type="F",Prefab="FIT05",Name="小魚乾",Desc="在互動模式與遊玩關卡食用，可恢復HP250。",Gold=250,Money=0,Enable=1,Hp=250,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=1,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 5: return new ItemDef{ID="I10060",Type="F",Prefab="FIT06",Name="柴魚",Desc="在互動模式與遊玩關卡食用，可恢復HP500。",Gold=500,Money=0,Enable=1,Hp=500,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=1,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 6: return new ItemDef{ID="I10070",Type="F",Prefab="FIT07",Name="鮮魚",Desc="在互動模式與遊玩關卡食用，可恢復HP500並增加EXP500。",Gold=0,Money=10,Enable=1,Hp=500,Exp=500,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=1,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 7: return new ItemDef{ID="I10080",Type="F",Prefab="FIT08",Name="高級餐包",Desc="在互動模式與遊玩關卡食用，可恢復HP1000並增加EXP1000。",Gold=0,Money=20,Enable=1,Hp=1000,Exp=1000,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=1,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 8: return new ItemDef{ID="I20010",Type="C",Prefab="CIT01",Name="小型數位相機",Desc="在探索模式使用，可獲得一張一般照片。",Gold=200,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 9: return new ItemDef{ID="I20020",Type="C",Prefab="CIT02",Name="拍立得相機",Desc="在探索模式使用，可獲得一張一般照片，並減少等待時間50%。",Gold=450,Money=0,Enable=1,Hp=0,Exp=0,Time=0.5f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 10: return new ItemDef{ID="I20030",Type="C",Prefab="CIT03",Name="運動相機",Desc="在探索模式使用，可獲得一張一般照片，並減少等待時間100%。",Gold=700,Money=0,Enable=1,Hp=0,Exp=0,Time=1f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 11: return new ItemDef{ID="I20040",Type="C",Prefab="CIT04",Name="單眼相機",Desc="在探索模式使用，可獲得一張一般照片，並有20%機率獲得環景照片。",Gold=400,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0.2f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 12: return new ItemDef{ID="I20050",Type="C",Prefab="CIT05",Name="數位單眼相機",Desc="在探索模式使用，可獲得一張一般照片，並有40%機率獲得環景照片。",Gold=600,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0.4f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 13: return new ItemDef{ID="I20060",Type="C",Prefab="CIT06",Name="蛇腹相機",Desc="在探索模式使用，可獲得一張一般照片，並有50%機率獲得第二張照片。",Gold=0,Money=8,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0.5f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 14: return new ItemDef{ID="I20070",Type="C",Prefab="CIT07",Name="雙眼相機",Desc="在探索模式使用，可獲得二張一般照片。",Gold=0,Money=12,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=1f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 15: return new ItemDef{ID="I20080",Type="C",Prefab="CIT08",Name="復古相機",Desc="在探索模式使用，可獲得二張一般照片，並有40%機率獲得環景照片。",Gold=0,Money=16,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=1f,BPhoto=0.4f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 16: return new ItemDef{ID="I21010",Type="SC",Prefab="SCIT01",Name="自拍棒",Desc="在探索模式使用，可減少拍攝等待時間50%",Gold=0,Money=5,Enable=1,Hp=0,Exp=0,Time=0.5f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 17: return new ItemDef{ID="I21020",Type="SC",Prefab="SCIT02",Name="相機腳架",Desc="在探索模式使用，可減少拍攝等待時間100%。",Gold=0,Money=10,Enable=1,Hp=0,Exp=0,Time=1f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 18: return new ItemDef{ID="I30010",Type="T",Prefab="TIT01",Name="雷射光筆",Desc="成貓體型專用玩具，在互動模式使用，可增加經驗值50。",Gold=400,Money=0,Enable=1,Hp=0,Exp=50,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 19: return new ItemDef{ID="I30020",Type="T",Prefab="TIT02",Name="玩具老鼠",Desc="成貓體型專用玩具，在互動模式使用，可增加經驗值200。",Gold=600,Money=0,Enable=1,Hp=0,Exp=200,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 20: return new ItemDef{ID="I30030",Type="T",Prefab="TIT03",Name="取消中",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 21: return new ItemDef{ID="I30040",Type="T",Prefab="TIT04",Name="逗貓棒",Desc="幼貓體型專用玩具，在互動模式使用，增加經驗值50。",Gold=400,Money=0,Enable=1,Hp=0,Exp=50,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=1,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 22: return new ItemDef{ID="I30050",Type="T",Prefab="TIT05",Name="毛線球",Desc="幼貓體型專用玩具，在互動模式使用，增加經驗值200。",Gold=600,Money=0,Enable=1,Hp=0,Exp=200,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=1,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 23: return new ItemDef{ID="I30060",Type="T",Prefab="TIT06",Name="取消中",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 24: return new ItemDef{ID="I30070",Type="T",Prefab="TIT07",Name="貓抓板",Desc="胖貓體型專用玩具，在互動模式使用，增加經驗值50。",Gold=400,Money=0,Enable=1,Hp=0,Exp=50,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 25: return new ItemDef{ID="I30080",Type="T",Prefab="TIT08",Name="暖爐",Desc="胖貓體型專用玩具，在互動模式使用，增加經驗值200。",Gold=600,Money=0,Enable=1,Hp=0,Exp=200,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 26: return new ItemDef{ID="I30090",Type="T",Prefab="TIT09",Name="取消中",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 27: return new ItemDef{ID="I30100",Type="T",Name="取消中",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 28: return new ItemDef{ID="I30110",Type="T",Name="取消中",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 29: return new ItemDef{ID="I31010",Type="ST",Prefab="STIT01",Name="貓草噴劑",Desc="在探索模式與互動模式使用，可減少等待時間50%。",Gold=0,Money=10,Enable=1,Hp=0,Exp=0,Time=0.5f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 30: return new ItemDef{ID="I31020",Type="ST",Prefab="STIT02",Name="罐裝乾貓草",Desc="在探索模式與互動模式使用，可減少等待時間100%。",Gold=0,Money=20,Enable=1,Hp=0,Exp=0,Time=1f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 31: return new ItemDef{ID="I32010",Type="TC",Prefab="TCIT01",Name="宅配紙箱",Desc="成貓體型專用道具，在探索模式使用，可捕捉到成貓。",Gold=0,Money=16,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=1,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 32: return new ItemDef{ID="I32020",Type="TC",Prefab="TCIT02",Name="掃地機器人",Desc="幼貓體型專用道具，在探索模式使用，可捕捉到幼貓。",Gold=0,Money=16,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=1,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 33: return new ItemDef{ID="I32030",Type="TC",Prefab="TCIT03",Name="沙發床",Desc="胖貓體型專用道具，在探索模式使用，可捕捉到胖貓。",Gold=0,Money=16,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=1,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 34: return new ItemDef{ID="I40010",Type="MG",Prefab="MGIT01",Name="貓金幣 250",Desc="獲得貓金幣 250",Gold=-250,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 35: return new ItemDef{ID="I40020",Type="MG",Prefab="MGIT02",Name="貓金幣 500",Desc="獲得貓金幣 500",Gold=-500,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 36: return new ItemDef{ID="I40030",Type="MG",Prefab="MGIT03",Name="貓金幣 750",Desc="獲得貓金幣 750",Gold=-750,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 37: return new ItemDef{ID="I40040",Type="MG",Prefab="MGIT04",Name="貓金幣 1000",Desc="獲得貓金幣 1000",Gold=-1000,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 38: return new ItemDef{ID="I40050",Type="MG",Prefab="MGIT05",Name="貓金幣 1500",Desc="獲得貓金幣 1500",Gold=-1500,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 39: return new ItemDef{ID="I41010",Type="MM",Prefab="MMIT01",Name="貓銀票 20",Desc="獲得貓銀票 20",Gold=0,Money=-20,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 40: return new ItemDef{ID="I41020",Type="MM",Prefab="MMIT02",Name="貓銀票 40",Desc="獲得貓銀票 40",Gold=0,Money=-40,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 41: return new ItemDef{ID="I41030",Type="MM",Prefab="MMIT03",Name="貓銀票 10",Desc="獲得貓銀票 10",Gold=0,Money=-10,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 42: return new ItemDef{ID="I41040",Type="MM",Prefab="MMIT04",Name="貓銀票 5",Desc="獲得貓銀票 5",Gold=0,Money=-5,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 43: return new ItemDef{ID="I41050",Type="MM",Prefab="MMIT05",Name="貓銀票 2",Desc="獲得貓銀票 2",Gold=0,Money=-2,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 44: return new ItemDef{ID="I41060",Type="MM",Prefab="MMIT06",Name="貓銀票 1",Desc="獲得貓銀票 1",Gold=0,Money=-1,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 45: return new ItemDef{ID="I41070",Name="預留空位",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 46: return new ItemDef{ID="I41080",Name="預留空位",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 47: return new ItemDef{ID="I41090",Name="預留空位",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 48: return new ItemDef{ID="I41100",Name="預留空位",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 49: return new ItemDef{ID="I41110",Name="預留空位",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 50: return new ItemDef{ID="I41120",Name="預留空位",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 51: return new ItemDef{ID="I50010",Type="F",Prefab="FIT10",Name="鮭魚",Desc="大型貓專用食物，在互動模式與遊玩關卡食用，可恢復HP150。",Gold=500,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=1,CatTypeCN=1};
case 52: return new ItemDef{ID="I50020",Type="F",Prefab="FIT11",Name="帶骨肉",Desc="大型貓專用食物，在互動模式與遊玩關卡食用，可恢復HP300。",Gold=1000,Money=0,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=1,CatTypeCN=1};
case 53: return new ItemDef{ID="I50030",Type="F",Prefab="FIT12",Name="烤乳豬",Desc="大型貓專用食物，在互動模式與遊玩關卡食用，可恢復HP1000並增加EXP500。",Gold=0,Money=30,Enable=1,Hp=0,Exp=500,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=1,CatTypeCN=1};
case 54: return new ItemDef{ID="I60010",Type="T",Prefab="TIT10",Name="南瓜",Desc="猞猁體型專用玩具，在互動模式使用，可增加經驗值100。",Gold=1000,Money=0,Enable=1,Hp=0,Exp=100,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=1,CatTypeCN=0};
case 55: return new ItemDef{ID="I60020",Type="T",Prefab="TIT11",Name="木人樁",Desc="猞猁體型專用玩具，在互動模式使用，可增加經驗值300。",Gold=3000,Money=0,Enable=1,Hp=0,Exp=300,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=1,CatTypeCN=0};
case 56: return new ItemDef{ID="I60030",Type="T",Prefab="TIT12",Name="取消中",Gold=0,Money=0,Enable=0,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=0,CatTypeCN=0};
case 57: return new ItemDef{ID="I62010",Type="TC",Prefab="TCIT04",Name="雪橇",Desc="猞猁體型專用道具，在探索模式使用，可捕捉到猞猁。",Gold=0,Money=24,Enable=1,Hp=0,Exp=0,Time=0f,ToPhoto=0f,BPhoto=0f,CatTypeCA=0,CatTypeCB=0,CatTypeCC=0,CatTypeCD=0,CatTypeCE=0,CatTypeCF=0,CatTypeCM=1,CatTypeCN=0};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}