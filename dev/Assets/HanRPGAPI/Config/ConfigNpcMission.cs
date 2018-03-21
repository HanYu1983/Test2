using System;
namespace HanRPGAPI{
public class ConfigNpcMission {
public string ID { get; set; }
public string Npc { get; set; }
public int Level { get; set; }
public string RequireItem { get; set; }
public string RequireKill { get; set; }
public string RequireStatus { get; set; }
public string Reward { get; set; }
public string Dependency { get; set; }
public string Dialog { get; set; }
public const int ID_COUNT = 5;
public const string ID_0 = "0";
public const string ID_1 = "1";
public const string ID_2 = "2";
public const string ID_3 = "3";
public const string ID_4 = "4";
public static ConfigNpcMission Get(int key){
switch(key){
case 0: return new ConfigNpcMission {ID="0",Npc="guide",Level=0,RequireItem="grass_50",Reward="money_100",Dialog="去採50個草"};
case 1: return new ConfigNpcMission {ID="1",Npc="guide",Level=0,RequireItem="wood_10",Reward="woodworkerBook1",Dependency="0",Dialog="去採10個木"};
case 2: return new ConfigNpcMission {ID="2",Npc="guide",Level=0,RequireKill="ant_10",Dialog="去殺10隻螞蟻"};
case 3: return new ConfigNpcMission {ID="3",Npc="guide",Level=1,RequireStatus="money_1000",Dialog="請存到1000元"};
case 4: return new ConfigNpcMission {ID="4",Npc="guide",Level=1,RequireStatus="atk_10",Dialog="請將攻擊力加到10"};
default: throw new Exception(key+"");
}}public static ConfigNpcMission Get(string key){
switch(key){
case "0": return new ConfigNpcMission {ID="0",Npc="guide",Level=0,RequireItem="grass_50",Reward="money_100",Dialog="去採50個草"};
case "1": return new ConfigNpcMission {ID="1",Npc="guide",Level=0,RequireItem="wood_10",Reward="woodworkerBook1",Dependency="0",Dialog="去採10個木"};
case "2": return new ConfigNpcMission {ID="2",Npc="guide",Level=0,RequireKill="ant_10",Dialog="去殺10隻螞蟻"};
case "3": return new ConfigNpcMission {ID="3",Npc="guide",Level=1,RequireStatus="money_1000",Dialog="請存到1000元"};
case "4": return new ConfigNpcMission {ID="4",Npc="guide",Level=1,RequireStatus="atk_10",Dialog="請將攻擊力加到10"};
default: throw new Exception(key);
}}}}