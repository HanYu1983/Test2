using System;
namespace HanRPGAPI{
public class ConfigConst {
public string ID { get; set; }
public float Float { get; set; }
public int Int { get; set; }
public string String { get; set; }
public const int ID_COUNT = 3;
public const string ID_moveConsumpation = "moveConsumpation";
public const string ID_workConsumpation = "workConsumpation";
public const string ID_visibleRange = "visibleRange";
public static ConfigConst Get(int key){
switch(key){
case 0: return new ConfigConst {ID="moveConsumpation",Int=2};
case 1: return new ConfigConst {ID="workConsumpation",Int=2};
case 2: return new ConfigConst {ID="visibleRange",Int=3};
default: throw new Exception(key+"");
}}public static ConfigConst Get(string key){
switch(key){
case "moveConsumpation": return new ConfigConst {ID="moveConsumpation",Int=2};
case "workConsumpation": return new ConfigConst {ID="workConsumpation",Int=2};
case "visibleRange": return new ConfigConst {ID="visibleRange",Int=3};
default: throw new Exception(key);
}}}}