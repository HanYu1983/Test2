using System;
namespace HanRPGAPI{
public class ConfigTerrian {
public string ID { get; set; }
public string Name { get; set; }
public string Description { get; set; }
public string Require { get; set; }
public int Class { get; set; }
public string MoveRequire { get; set; }
public const int ID_COUNT = 11;
public const string ID_wasteland = "wasteland";
public const string ID_frozenSoil = "frozenSoil";
public const string ID_desert = "desert";
public const string ID_plain = "plain";
public const string ID_iceField = "iceField";
public const string ID_hills = "hills";
public const string ID_forest = "forest";
public const string ID_mountant = "mountant";
public const string ID_coast = "coast";
public const string ID_epicontinentalSea = "epicontinentalSea";
public const string ID_deepSea = "deepSea";
public static ConfigTerrian Get(int key){
switch(key){
case 0: return new ConfigTerrian {ID="wasteland",Name="荒地"};
case 1: return new ConfigTerrian {ID="frozenSoil",Name="凍土",Require="temperature1",Class=1};
case 2: return new ConfigTerrian {ID="desert",Name="沙漠",Require="temperature3",Class=1};
case 3: return new ConfigTerrian {ID="plain",Name="平原",Require="grass,temperature2",Class=2};
case 4: return new ConfigTerrian {ID="iceField",Name="冰原",Require="grass,temperature1",Class=2};
case 5: return new ConfigTerrian {ID="hills",Name="丘陵",Require="smallMountain",Class=2};
case 6: return new ConfigTerrian {ID="forest",Name="森林",Require="tree_2",Class=3};
case 7: return new ConfigTerrian {ID="mountant",Name="高山",Require="smallMountain_3",Class=4};
case 8: return new ConfigTerrian {ID="coast",Name="沿岸",Require="water_1",Class=5};
case 9: return new ConfigTerrian {ID="epicontinentalSea",Name="淺海",Require="water_2",Class=6,MoveRequire="woodBoat"};
case 10: return new ConfigTerrian {ID="deepSea",Name="深海",Require="water_3",Class=7,MoveRequire="woodBoat"};
default: throw new Exception(key+"");
}}public static ConfigTerrian Get(string key){
switch(key){
case "wasteland": return new ConfigTerrian {ID="wasteland",Name="荒地"};
case "frozenSoil": return new ConfigTerrian {ID="frozenSoil",Name="凍土",Require="temperature1",Class=1};
case "desert": return new ConfigTerrian {ID="desert",Name="沙漠",Require="temperature3",Class=1};
case "plain": return new ConfigTerrian {ID="plain",Name="平原",Require="grass,temperature2",Class=2};
case "iceField": return new ConfigTerrian {ID="iceField",Name="冰原",Require="grass,temperature1",Class=2};
case "hills": return new ConfigTerrian {ID="hills",Name="丘陵",Require="smallMountain",Class=2};
case "forest": return new ConfigTerrian {ID="forest",Name="森林",Require="tree_2",Class=3};
case "mountant": return new ConfigTerrian {ID="mountant",Name="高山",Require="smallMountain_3",Class=4};
case "coast": return new ConfigTerrian {ID="coast",Name="沿岸",Require="water_1",Class=5};
case "epicontinentalSea": return new ConfigTerrian {ID="epicontinentalSea",Name="淺海",Require="water_2",Class=6,MoveRequire="woodBoat"};
case "deepSea": return new ConfigTerrian {ID="deepSea",Name="深海",Require="water_3",Class=7,MoveRequire="woodBoat"};
default: throw new Exception(key);
}}}}