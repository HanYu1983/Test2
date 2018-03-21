using System;
namespace HanRPGAPI{
public class ConfigSkill {
public string ID { get; set; }
public string Name { get; set; }
public string Description { get; set; }
public string SkillTypeRequire { get; set; }
public float TriggerBouns { get; set; }
public string Condition { get; set; }
public int SlotCount { get; set; }
public string Effect { get; set; }
public string Values { get; set; }
public string Characteristic { get; set; }
public const int ID_COUNT = 12;
public const string ID_seiken = "seiken";
public const string ID_tripleKick = "tripleKick";
public const string ID_renzokugen = "renzokugen";
public const string ID_hanngeki = "hanngeki";
public const string ID_tiauci = "tiauci";
public const string ID_zengmianpi = "zengmianpi";
public const string ID_zyuzizan = "zyuzizan";
public const string ID_zihazan = "zihazan";
public const string ID_bukikakuto = "bukikakuto";
public const string ID_bokyoryokuhakai = "bokyoryokuhakai";
public const string ID_karadaAttack = "karadaAttack";
public const string ID_spinAttack = "spinAttack";
public static ConfigSkill Get(int key){
switch(key){
case 0: return new ConfigSkill {ID="seiken",Name="正拳",SkillTypeRequire="karate_5",Condition="attack",Effect="atk*1.2, critical*1.2"};
case 1: return new ConfigSkill {ID="tripleKick",Name="三段踢",SkillTypeRequire="karate_10",Condition="attack",Effect="atk*1.4, critical*1.2"};
case 2: return new ConfigSkill {ID="renzokugen",Name="無呼吸連打",SkillTypeRequire="karate_20",Condition="attack",Effect="atk*1.8, critical*1.2"};
case 3: return new ConfigSkill {ID="hanngeki",Name="迎擊",SkillTypeRequire="karate_30",Condition="deffence",SlotCount=3,Effect="取消對方的攻擊，並對對方造成{1}倍普攻傷害.",Values="1.1"};
case 4: return new ConfigSkill {ID="tiauci",Name="調息",SkillTypeRequire="karate_20",Condition="turn",SlotCount=3,Effect="{0}+{1}",Values="hp,5%"};
case 5: return new ConfigSkill {ID="zengmianpi",Name="正面劈",SkillTypeRequire="fencingArt_5",Condition="attack"};
case 6: return new ConfigSkill {ID="zyuzizan",Name="十字斬",SkillTypeRequire="fencingArt_10",Condition="attack"};
case 7: return new ConfigSkill {ID="zihazan",Name="居合斬",SkillTypeRequire="fencingArt_20",Condition="attack"};
case 8: return new ConfigSkill {ID="bukikakuto",Name="武器格檔",SkillTypeRequire="fencingArt_10",TriggerBouns=0.3f,Condition="deffence",SlotCount=3,Effect="取消對方的攻擊."};
case 9: return new ConfigSkill {ID="bokyoryokuhakai",Name="防禦力破壞",SkillTypeRequire="fencingArt_10",Condition="menu",SlotCount=3,Effect="對象防禦力下降{0}倍。",Values="0.1",Characteristic="enemy"};
case 10: return new ConfigSkill {ID="karadaAttack",Name="身體撞擊",SkillTypeRequire="karate",Condition="menu",SlotCount=2,Characteristic="enemy"};
case 11: return new ConfigSkill {ID="spinAttack",Name="回轉拳",SkillTypeRequire="karate",Condition="menu",SlotCount=2,Characteristic="enemyAll"};
default: throw new Exception(key+"");
}}public static ConfigSkill Get(string key){
switch(key){
case "seiken": return new ConfigSkill {ID="seiken",Name="正拳",SkillTypeRequire="karate_5",Condition="attack",Effect="atk*1.2, critical*1.2"};
case "tripleKick": return new ConfigSkill {ID="tripleKick",Name="三段踢",SkillTypeRequire="karate_10",Condition="attack",Effect="atk*1.4, critical*1.2"};
case "renzokugen": return new ConfigSkill {ID="renzokugen",Name="無呼吸連打",SkillTypeRequire="karate_20",Condition="attack",Effect="atk*1.8, critical*1.2"};
case "hanngeki": return new ConfigSkill {ID="hanngeki",Name="迎擊",SkillTypeRequire="karate_30",Condition="deffence",SlotCount=3,Effect="取消對方的攻擊，並對對方造成{1}倍普攻傷害.",Values="1.1"};
case "tiauci": return new ConfigSkill {ID="tiauci",Name="調息",SkillTypeRequire="karate_20",Condition="turn",SlotCount=3,Effect="{0}+{1}",Values="hp,5%"};
case "zengmianpi": return new ConfigSkill {ID="zengmianpi",Name="正面劈",SkillTypeRequire="fencingArt_5",Condition="attack"};
case "zyuzizan": return new ConfigSkill {ID="zyuzizan",Name="十字斬",SkillTypeRequire="fencingArt_10",Condition="attack"};
case "zihazan": return new ConfigSkill {ID="zihazan",Name="居合斬",SkillTypeRequire="fencingArt_20",Condition="attack"};
case "bukikakuto": return new ConfigSkill {ID="bukikakuto",Name="武器格檔",SkillTypeRequire="fencingArt_10",TriggerBouns=0.3f,Condition="deffence",SlotCount=3,Effect="取消對方的攻擊."};
case "bokyoryokuhakai": return new ConfigSkill {ID="bokyoryokuhakai",Name="防禦力破壞",SkillTypeRequire="fencingArt_10",Condition="menu",SlotCount=3,Effect="對象防禦力下降{0}倍。",Values="0.1",Characteristic="enemy"};
case "karadaAttack": return new ConfigSkill {ID="karadaAttack",Name="身體撞擊",SkillTypeRequire="karate",Condition="menu",SlotCount=2,Characteristic="enemy"};
case "spinAttack": return new ConfigSkill {ID="spinAttack",Name="回轉拳",SkillTypeRequire="karate",Condition="menu",SlotCount=2,Characteristic="enemyAll"};
default: throw new Exception(key);
}}}}