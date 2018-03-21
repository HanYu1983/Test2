using System;
namespace HanRPGAPI{
public class ConfigNpc {
public string ID { get; set; }
public string Name { get; set; }
public string Description { get; set; }
public const int ID_COUNT = 1;
public const string ID_guide = "guide";
public static ConfigNpc Get(int key){
switch(key){
case 0: return new ConfigNpc {ID="guide",Name="新手指引"};
default: throw new Exception(key+"");
}}public static ConfigNpc Get(string key){
switch(key){
case "guide": return new ConfigNpc {ID="guide",Name="新手指引"};
default: throw new Exception(key);
}}}}