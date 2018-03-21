using System;
namespace HanRPGAPI{
public class ConfigCharacteristic {
public string ID { get; set; }
public string Name { get; set; }
public string Description { get; set; }
public const int ID_COUNT = 5;
public const string ID_me = "me";
public const string ID_enemy = "enemy";
public const string ID_enemyAll = "enemyAll";
public const string ID_fly = "fly";
public const string ID_boss = "boss";
public static ConfigCharacteristic Get(int key){
switch(key){
case 0: return new ConfigCharacteristic {ID="me",Name="本身"};
case 1: return new ConfigCharacteristic {ID="enemy",Name="敵單體"};
case 2: return new ConfigCharacteristic {ID="enemyAll",Name="敵全體"};
case 3: return new ConfigCharacteristic {ID="fly",Name="飛行"};
case 4: return new ConfigCharacteristic {ID="boss",Name="魔王"};
default: throw new Exception(key+"");
}}public static ConfigCharacteristic Get(string key){
switch(key){
case "me": return new ConfigCharacteristic {ID="me",Name="本身"};
case "enemy": return new ConfigCharacteristic {ID="enemy",Name="敵單體"};
case "enemyAll": return new ConfigCharacteristic {ID="enemyAll",Name="敵全體"};
case "fly": return new ConfigCharacteristic {ID="fly",Name="飛行"};
case "boss": return new ConfigCharacteristic {ID="boss",Name="魔王"};
default: throw new Exception(key);
}}}}