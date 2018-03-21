using System;
namespace HanRPGAPI{
public class ConfigItemType {
public string ID { get; set; }
public string Name { get; set; }
public string Description { get; set; }
public const int ID_COUNT = 4;
public const string ID_material = "material";
public const string ID_weapon = "weapon";
public const string ID_food = "food";
public const string ID_important = "important";
public static ConfigItemType Get(int key){
switch(key){
case 0: return new ConfigItemType {ID="material",Name="原料"};
case 1: return new ConfigItemType {ID="weapon",Name="武器"};
case 2: return new ConfigItemType {ID="food",Name="食物"};
case 3: return new ConfigItemType {ID="important",Name="重要"};
default: throw new Exception(key+"");
}}public static ConfigItemType Get(string key){
switch(key){
case "material": return new ConfigItemType {ID="material",Name="原料"};
case "weapon": return new ConfigItemType {ID="weapon",Name="武器"};
case "food": return new ConfigItemType {ID="food",Name="食物"};
case "important": return new ConfigItemType {ID="important",Name="重要"};
default: throw new Exception(key);
}}}}