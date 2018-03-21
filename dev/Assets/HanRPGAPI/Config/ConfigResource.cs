using System;
namespace HanRPGAPI{
public class ConfigResource {
public string ID { get; set; }
public string Name { get; set; }
public string Description { get; set; }
public string Item { get; set; }
public int UseCount { get; set; }
public const int ID_COUNT = 9;
public const string ID_grass = "grass";
public const string ID_tree = "tree";
public const string ID_stone = "stone";
public const string ID_iron = "iron";
public const string ID_water = "water";
public const string ID_temperature1 = "temperature1";
public const string ID_temperature2 = "temperature2";
public const string ID_temperature3 = "temperature3";
public const string ID_smallMountain = "smallMountain";
public static ConfigResource Get(int key){
switch(key){
case 0: return new ConfigResource {ID="grass",Name="草",Item="grass_3",UseCount=3};
case 1: return new ConfigResource {ID="tree",Name="樹",Item="wood_1",UseCount=3};
case 2: return new ConfigResource {ID="stone",Name="石",Item="stone_1",UseCount=3};
case 3: return new ConfigResource {ID="iron",Name="鐵礦",Item="iron_1",UseCount=3};
case 4: return new ConfigResource {ID="water",Name="水"};
case 5: return new ConfigResource {ID="temperature1",Name="低溫"};
case 6: return new ConfigResource {ID="temperature2",Name="適溫"};
case 7: return new ConfigResource {ID="temperature3",Name="高溫"};
case 8: return new ConfigResource {ID="smallMountain",Name="小山丘"};
default: throw new Exception(key+"");
}}public static ConfigResource Get(string key){
switch(key){
case "grass": return new ConfigResource {ID="grass",Name="草",Item="grass_3",UseCount=3};
case "tree": return new ConfigResource {ID="tree",Name="樹",Item="wood_1",UseCount=3};
case "stone": return new ConfigResource {ID="stone",Name="石",Item="stone_1",UseCount=3};
case "iron": return new ConfigResource {ID="iron",Name="鐵礦",Item="iron_1",UseCount=3};
case "water": return new ConfigResource {ID="water",Name="水"};
case "temperature1": return new ConfigResource {ID="temperature1",Name="低溫"};
case "temperature2": return new ConfigResource {ID="temperature2",Name="適溫"};
case "temperature3": return new ConfigResource {ID="temperature3",Name="高溫"};
case "smallMountain": return new ConfigResource {ID="smallMountain",Name="小山丘"};
default: throw new Exception(key);
}}}}