using System;
namespace HanRPGAPI{
public class ConfigConditionType {
public string ID { get; set; }
public string Name { get; set; }
public string Description { get; set; }
public const int ID_COUNT = 4;
public const string ID_attack = "attack";
public const string ID_deffence = "deffence";
public const string ID_turn = "turn";
public const string ID_menu = "menu";
public static ConfigConditionType Get(int key){
switch(key){
case 0: return new ConfigConditionType {ID="attack",Name="攻擊時"};
case 1: return new ConfigConditionType {ID="deffence",Name="防守時"};
case 2: return new ConfigConditionType {ID="turn",Name="回合自動"};
case 3: return new ConfigConditionType {ID="menu",Name="選單"};
default: throw new Exception(key+"");
}}public static ConfigConditionType Get(string key){
switch(key){
case "attack": return new ConfigConditionType {ID="attack",Name="攻擊時"};
case "deffence": return new ConfigConditionType {ID="deffence",Name="防守時"};
case "turn": return new ConfigConditionType {ID="turn",Name="回合自動"};
case "menu": return new ConfigConditionType {ID="menu",Name="選單"};
default: throw new Exception(key);
}}}}