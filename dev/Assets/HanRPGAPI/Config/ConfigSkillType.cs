using System;
namespace HanRPGAPI{
public class ConfigSkillType {
public string ID { get; set; }
public string Name { get; set; }
public string Description { get; set; }
public const int ID_COUNT = 10;
public const string ID_tailor = "tailor";
public const string ID_woodworker = "woodworker";
public const string ID_ironworker = "ironworker";
public const string ID_fencingArt = "fencingArt";
public const string ID_karate = "karate";
public const string ID_shield = "shield";
public const string ID_armor = "armor";
public const string ID_speed = "speed";
public const string ID_move = "move";
public const string ID_subtle = "subtle";
public static ConfigSkillType Get(int key){
switch(key){
case 0: return new ConfigSkillType {ID="tailor",Name="裁縫"};
case 1: return new ConfigSkillType {ID="woodworker",Name="木工"};
case 2: return new ConfigSkillType {ID="ironworker",Name="鐵匠"};
case 3: return new ConfigSkillType {ID="fencingArt",Name="劍術"};
case 4: return new ConfigSkillType {ID="karate",Name="拳術"};
case 5: return new ConfigSkillType {ID="shield",Name="盾術"};
case 6: return new ConfigSkillType {ID="armor",Name="重裝"};
case 7: return new ConfigSkillType {ID="speed",Name="輕裝"};
case 8: return new ConfigSkillType {ID="move",Name="移動"};
case 9: return new ConfigSkillType {ID="subtle",Name="細微"};
default: throw new Exception(key+"");
}}public static ConfigSkillType Get(string key){
switch(key){
case "tailor": return new ConfigSkillType {ID="tailor",Name="裁縫"};
case "woodworker": return new ConfigSkillType {ID="woodworker",Name="木工"};
case "ironworker": return new ConfigSkillType {ID="ironworker",Name="鐵匠"};
case "fencingArt": return new ConfigSkillType {ID="fencingArt",Name="劍術"};
case "karate": return new ConfigSkillType {ID="karate",Name="拳術"};
case "shield": return new ConfigSkillType {ID="shield",Name="盾術"};
case "armor": return new ConfigSkillType {ID="armor",Name="重裝"};
case "speed": return new ConfigSkillType {ID="speed",Name="輕裝"};
case "move": return new ConfigSkillType {ID="move",Name="移動"};
case "subtle": return new ConfigSkillType {ID="subtle",Name="細微"};
default: throw new Exception(key);
}}}}