using System;
namespace HanRPGAPI{
public class ConfigMonster {
public string ID { get; set; }
public string Name { get; set; }
public string Description { get; set; }
public string Item { get; set; }
public string Terrian { get; set; }
public int Str { get; set; }
public int Vit { get; set; }
public int Agi { get; set; }
public int Dex { get; set; }
public int Int { get; set; }
public int Luc { get; set; }
public string Characteristic { get; set; }
public const int ID_COUNT = 6;
public const string ID_ant = "ant";
public const string ID_snack = "snack";
public const string ID_bear = "bear";
public const string ID_bird = "bird";
public const string ID_toxinSnack = "toxinSnack";
public const string ID_bigBear = "bigBear";
public static ConfigMonster Get(int key){
switch(key){
case 0: return new ConfigMonster {ID="ant",Name="螞蟻",Terrian="plain,wasteland,desert",Str=3,Vit=3,Agi=2,Dex=0,Int=3,Luc=0};
case 1: return new ConfigMonster {ID="snack",Name="蛇",Terrian="forest,wasteland,desert",Str=10,Vit=5,Agi=5,Dex=20,Int=1,Luc=5};
case 2: return new ConfigMonster {ID="bear",Name="熊",Terrian="forest",Str=20,Vit=20,Agi=10,Dex=10,Int=1,Luc=0};
case 3: return new ConfigMonster {ID="bird",Name="鳥",Terrian="plain,forest",Str=10,Vit=10,Agi=30,Dex=20,Int=0,Luc=5};
case 4: return new ConfigMonster {ID="toxinSnack",Name="毒蛇",Terrian="desert",Str=40,Vit=10,Agi=10,Dex=20,Int=1,Luc=10};
case 5: return new ConfigMonster {ID="bigBear",Name="大熊",Terrian="forest",Str=25,Vit=40,Agi=15,Dex=5,Int=0,Luc=0};
default: throw new Exception(key+"");
}}public static ConfigMonster Get(string key){
switch(key){
case "ant": return new ConfigMonster {ID="ant",Name="螞蟻",Terrian="plain,wasteland,desert",Str=3,Vit=3,Agi=2,Dex=0,Int=3,Luc=0};
case "snack": return new ConfigMonster {ID="snack",Name="蛇",Terrian="forest,wasteland,desert",Str=10,Vit=5,Agi=5,Dex=20,Int=1,Luc=5};
case "bear": return new ConfigMonster {ID="bear",Name="熊",Terrian="forest",Str=20,Vit=20,Agi=10,Dex=10,Int=1,Luc=0};
case "bird": return new ConfigMonster {ID="bird",Name="鳥",Terrian="plain,forest",Str=10,Vit=10,Agi=30,Dex=20,Int=0,Luc=5};
case "toxinSnack": return new ConfigMonster {ID="toxinSnack",Name="毒蛇",Terrian="desert",Str=40,Vit=10,Agi=10,Dex=20,Int=1,Luc=10};
case "bigBear": return new ConfigMonster {ID="bigBear",Name="大熊",Terrian="forest",Str=25,Vit=40,Agi=15,Dex=5,Int=0,Luc=0};
default: throw new Exception(key);
}}}}