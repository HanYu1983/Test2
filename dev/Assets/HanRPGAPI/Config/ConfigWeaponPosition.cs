using System;
namespace HanRPGAPI{
public class ConfigWeaponPosition {
public string ID { get; set; }
public string Name { get; set; }
public int SlotCount { get; set; }
public const int ID_COUNT = 5;
public const string ID_head = "head";
public const string ID_body = "body";
public const string ID_foot = "foot";
public const string ID_hand = "hand";
public const string ID_accessory = "accessory";
public static ConfigWeaponPosition Get(int key){
switch(key){
case 0: return new ConfigWeaponPosition {ID="head",Name="頭",SlotCount=1};
case 1: return new ConfigWeaponPosition {ID="body",Name="身",SlotCount=1};
case 2: return new ConfigWeaponPosition {ID="foot",Name="腳",SlotCount=1};
case 3: return new ConfigWeaponPosition {ID="hand",Name="手",SlotCount=2};
case 4: return new ConfigWeaponPosition {ID="accessory",Name="配件",SlotCount=3};
default: throw new Exception(key+"");
}}public static ConfigWeaponPosition Get(string key){
switch(key){
case "head": return new ConfigWeaponPosition {ID="head",Name="頭",SlotCount=1};
case "body": return new ConfigWeaponPosition {ID="body",Name="身",SlotCount=1};
case "foot": return new ConfigWeaponPosition {ID="foot",Name="腳",SlotCount=1};
case "hand": return new ConfigWeaponPosition {ID="hand",Name="手",SlotCount=2};
case "accessory": return new ConfigWeaponPosition {ID="accessory",Name="配件",SlotCount=3};
default: throw new Exception(key);
}}}}