using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace HanRPGAPI{
	#region ability
	/// <summary>
	/// 基礎能力
	/// 可用來升級加點
	/// </summary>
	[Serializable]
	public struct BasicAbility {
		public float str, vit, agi, dex, Int, luc;
		public static BasicAbility Zero;
		public static BasicAbility Default{
			get{
				BasicAbility ret;
				ret.str = 8;
				ret.vit = 10;
				ret.agi = 5;
				ret.dex = 5;
				ret.Int = 8;
				ret.luc = 5;
				return ret;
			}
		}
		public BasicAbility Negative{
			get{
				var a = this;
				a.str = -str;
				a.vit = -vit;
				a.agi = -agi;
				a.dex = -dex;
				a.Int = -Int;
				a.luc = -luc;
				return a;
			}
		}
		public BasicAbility Add(BasicAbility b){
			var a = this;
			a.str += b.str;
			a.vit += b.vit;
			a.agi += b.agi;
			a.dex += b.dex;
			a.Int += b.Int;
			a.luc += b.luc;
			return a;
		}
		public BasicAbility Multiply(float b){
			var a = this;
			a.str *= b;
			a.vit *= b;
			a.agi *= b;
			a.dex *= b;
			a.Int *= b;
			a.luc *= b;
			return a;
		}
		public override string ToString(){
			return string.Format (@"腕力:{0} 體質:{1} 敏捷:{2} 技巧:{3} 知識:{4} 幸運:{5}", str, vit, agi, dex, Int, luc);
		}

		/// <summary>
		/// 轉換成戰鬥能力
		/// </summary>
		/// <value>The fight ability.</value>
		public FightAbility FightAbility{
			get{
				ConfigAbility config = null;
				FightAbility ret;

				config = ConfigAbility.Get (ConfigAbility.ID_hp);
				ret.hp = str * config.Str + vit*config.Vit + agi*config.Agi + dex*config.Dex + Int*config.Int + luc*config.Luc;

				config = ConfigAbility.Get (ConfigAbility.ID_mp);
				ret.mp = str * config.Str + vit*config.Vit + agi*config.Agi + dex*config.Dex + Int*config.Int + luc*config.Luc;

				config = ConfigAbility.Get (ConfigAbility.ID_atk);
				ret.atk = str * config.Str + vit*config.Vit + agi*config.Agi + dex*config.Dex + Int*config.Int + luc*config.Luc;

				config = ConfigAbility.Get (ConfigAbility.ID_def);
				ret.def = str * config.Str + vit*config.Vit + agi*config.Agi + dex*config.Dex + Int*config.Int + luc*config.Luc;

				config = ConfigAbility.Get (ConfigAbility.ID_matk);
				ret.matk = str * config.Str + vit*config.Vit + agi*config.Agi + dex*config.Dex + Int*config.Int + luc*config.Luc;

				config = ConfigAbility.Get (ConfigAbility.ID_mdef);
				ret.mdef = str * config.Str + vit*config.Vit + agi*config.Agi + dex*config.Dex + Int*config.Int + luc*config.Luc;

				config = ConfigAbility.Get (ConfigAbility.ID_accuracy);
				ret.accuracy = str * config.Str + vit*config.Vit + agi*config.Agi + dex*config.Dex + Int*config.Int + luc*config.Luc;

				config = ConfigAbility.Get (ConfigAbility.ID_dodge);
				ret.dodge = str * config.Str + vit*config.Vit + agi*config.Agi + dex*config.Dex + Int*config.Int + luc*config.Luc;

				config = ConfigAbility.Get (ConfigAbility.ID_critical);
				ret.critical = str * config.Str + vit*config.Vit + agi*config.Agi + dex*config.Dex + Int*config.Int + luc*config.Luc;
				return ret;
			}
		}
	}

	/// <summary>
	/// 戰鬥能力
	/// 用來計算戰鬥傷害結果
	/// </summary>
	public struct FightAbility {
		public float hp, mp, atk, def, matk, mdef, accuracy, dodge, critical;

		public static FightAbility Zero;

		public FightAbility Add(FightAbility b){
			var a = this;
			a.hp += b.hp;
			a.mp += b.mp;
			a.atk += b.atk;
			a.def += b.def;
			a.matk += b.matk;
			a.mdef += b.mdef;
			a.accuracy += b.accuracy;
			a.dodge += b.dodge;
			a.critical += b.critical;
			return a;
		}

		public FightAbility Negative{
			get{
				var a = this;
				a.hp = -hp;
				a.mp = -mp;
				a.atk = -atk;
				a.def = -def;
				a.matk = -matk;
				a.mdef = -mdef;
				a.accuracy = -accuracy;
				a.dodge = -dodge;
				a.critical = -critical;
				return a;
			}
		}

		public float CriticalHitRate(FightAbility other){
			return critical / other.mdef;
		}

		public float AccuracyRate(FightAbility other){
			return accuracy / other.dodge;
		}

		public int Damage(FightAbility other){
			return (int)(atk - other.def);
		}
	}

	public enum ItemEffectType{
		Unknown, Plus, Multiply, Minus, Enforce
	}

	public struct ItemEffect{
		public string value;
		public ItemEffectType EffectOperator{
			get{
				var op = 
					value.IndexOf ("+") != -1 ? ItemEffectType.Plus :
					value.IndexOf ("*") != -1 ? ItemEffectType.Multiply :
					value.IndexOf ("-") != -1 ? ItemEffectType.Minus :
					value.IndexOf("@") != -1 ? ItemEffectType.Enforce :
					ItemEffectType.Unknown;
				return op;
			}
		}
		// 請參考CalcAbility
		public BasicAbility Effect(BasicAbility ability){
			var op = EffectOperator;
			if (op == ItemEffectType.Enforce) {
				throw new HanRPGAPIException ("強化效果不能來這裡計算", null);
			}
			var idx = value.Split (new char[]{ '+', '*', '-' });
			if (idx.Length != 2) {
				throw new HanRPGAPIException ("格式錯誤:"+value, null);
			}
			try{
				float.Parse (idx[1]);
			}catch{
				throw new HanRPGAPIException ("格式錯誤:"+value, null);
			}
			var target = idx [0];
			if (op == ItemEffectType.Plus) {
				var effectValue = int.Parse (idx [1]);
				switch (target) {
				case "str":
					ability.str += effectValue;
					break;
				case "vit":
					ability.vit += effectValue;
					break;
				case "dex":
					ability.dex += effectValue;
					break;
				case "agi":
					ability.agi += effectValue;
					break;
				case "int":
					ability.Int += effectValue;
					break;
				case "luc":
					ability.luc += effectValue;
					break;
				}
			} 

			if (op == ItemEffectType.Minus) {
				var effectValue = int.Parse (idx [1]);
				switch (target) {
				case "str":
					ability.str -= effectValue;
					break;
				case "vit":
					ability.vit -= effectValue;
					break;
				case "dex":
					ability.dex -= effectValue;
					break;
				case "agi":
					ability.agi -= effectValue;
					break;
				case "int":
					ability.Int -= effectValue;
					break;
				case "luc":
					ability.luc -= effectValue;
					break;
				}
			} 

			if (EffectOperator == ItemEffectType.Multiply) {
				var effectValue = float.Parse (idx [1]);
				switch (target) {
				case "str":
					ability.str = (int)(ability.str * effectValue);
					break;
				case "vit":
					ability.vit = (int)(ability.vit * effectValue);
					break;
				case "dex":
					ability.dex = (int)(ability.dex * effectValue);
					break;
				case "agi":
					ability.agi = (int)(ability.agi * effectValue);
					break;
				case "int":
					ability.Int = (int)(ability.Int * effectValue);
					break;
				case "luc":
					ability.luc = (int)(ability.luc * effectValue);
					break;
				}
			}
			return ability;
		}

		// 請參考Helper.CalcAbility
		public FightAbility Effect(FightAbility ability){
			/*
			var idx = value.Split (new char[]{ '+', '*', '-' });
			if (idx.Length != 2) {
				throw new Exception ("format error:" + value);
			}
			try {
				float.Parse (idx [1]);
			} catch {
				throw new Exception ("format error:" + value);
			}
			var target = idx [0];
			var op = EffectOperator;
			*/
			var op = EffectOperator;
			if (op == ItemEffectType.Enforce) {
				throw new HanRPGAPIException ("強化效果不能來這裡計算", null);
			}
			var idx = value.Split (new char[]{ '+', '*', '-' });
			if (idx.Length != 2) {
				throw new HanRPGAPIException ("格式錯誤:"+value, null);
			}
			try{
				float.Parse (idx[1]);
			}catch{
				throw new HanRPGAPIException ("格式錯誤:"+value, null);
			}
			var target = idx [0];
			if (op == ItemEffectType.Plus) {
				var effectValue = int.Parse (idx [1]);
				switch (target) {
				case "hp":
					ability.hp += effectValue;
					break;
				case "mp":
					ability.mp += effectValue;
					break;
				case "atk":
					ability.atk += effectValue;
					break;
				case "def":
					ability.def += effectValue;
					break;
				case "matk":
					ability.matk += effectValue;
					break;
				case "mdef":
					ability.mdef += effectValue;
					break;
				case "accuracy":
					ability.accuracy += effectValue;
					break;
				case "dodge":
					ability.dodge += effectValue;
					break;
				case "critical":
					ability.critical += effectValue;
					break;
				}
			}

			if (op == ItemEffectType.Minus) {
				var effectValue = int.Parse (idx [1]);
				switch (target) {
				case "hp":
					ability.hp -= effectValue;
					break;
				case "mp":
					ability.mp -= effectValue;
					break;
				case "atk":
					ability.atk -= effectValue;
					break;
				case "def":
					ability.def -= effectValue;
					break;
				case "matk":
					ability.matk -= effectValue;
					break;
				case "mdef":
					ability.mdef -= effectValue;
					break;
				case "accuracy":
					ability.accuracy -= effectValue;
					break;
				case "dodge":
					ability.dodge -= effectValue;
					break;
				case "critical":
					ability.critical -= effectValue;
					break;
				}
			}

			if (op == ItemEffectType.Minus) {
				var effectValue = float.Parse (idx [1]);
				switch (target) {
				case "hp":
					ability.hp *= effectValue;
					break;
				case "mp":
					ability.mp *= effectValue;
					break;
				case "atk":
					ability.atk *= effectValue;
					break;
				case "def":
					ability.def *= effectValue;
					break;
				case "matk":
					ability.matk *= effectValue;
					break;
				case "mdef":
					ability.mdef *= effectValue;
					break;
				case "accuracy":
					ability.accuracy *= effectValue;
					break;
				case "dodge":
					ability.dodge *= effectValue;
					break;
				case "critical":
					ability.critical *= effectValue;
					break;
				}
			}
			return ability;
		}
	}

	public partial class Alg{
		public static BasicAbility GetBasicAbility(ConfigMonster monster){
			BasicAbility ret;
			ret.str = monster.Str;
			ret.vit = monster.Vit;
			ret.agi = monster.Agi;
			ret.dex = monster.Dex;
			ret.Int = monster.Int;
			ret.luc = monster.Luc;
			return ret;
		}

		public static IEnumerable<ItemEffect> Effect(ConfigSkill skill){
			return skill.Effect.Split (new char[]{ ',' }).Select (v => {
				ItemEffect ef;
				ef.value = v;
				return ef;
			});
		}

		public static BasicAbility CalcAbility(Func<string, int> expFn, IEnumerable<Item> weapons, IEnumerable<ItemEffect> bufs, BasicAbility basic, ref FightAbility fight){
			// 招式加成
			var skillbonus = Enumerable.Range (0, ConfigSkillType.ID_COUNT).Select (ConfigSkillType.Get)
				.Select (cfg => cfg.ID).Select (ConfigAbility.Get)
				.Select (cfg => {
					var exp = expFn == null ? 0 : expFn(cfg.ID);
					var bonus = new BasicAbility(){
						str = cfg.Str,
						vit = cfg.Vit,
						agi = cfg.Agi,
						dex = cfg.Dex,
						Int = cfg.Int,
						luc = cfg.Luc
					};
					return bonus.Multiply(exp);
				})
				.Aggregate (BasicAbility.Zero, (total, cur) => {
					return total.Add(cur);
				});
			// 取得所有效果
			IEnumerable<ItemEffect> effects = new List<ItemEffect> ();
			if (weapons != null) {
				effects = effects.Concat(weapons.SelectMany (it => it.Effects));
			}
			if (bufs != null) {
				effects = effects.Concat (bufs);
			}
			// 分類效果
			var addEffect = effects.Where (ef => ef.EffectOperator == ItemEffectType.Plus || ef.EffectOperator == ItemEffectType.Minus);
			var multiEffect = effects.Where (ef => ef.EffectOperator == ItemEffectType.Multiply);
			// 準備初值
			var tmpBasic = BasicAbility.Default;
			tmpBasic = tmpBasic.Add(skillbonus);
			tmpBasic = tmpBasic.Add(basic);
			// 先處理基本能力
			// 先加減
			tmpBasic = addEffect.Aggregate (tmpBasic, (accu, curr) => {
				return curr.Effect(accu);
			});
			// 後乘除
			tmpBasic = multiEffect.Aggregate (tmpBasic, (accu, curr) => {
				return curr.Effect(accu);
			});
			// 處理後的基本能力轉成戰鬥力
			// 再處理戰鬥力
			var tmpFight = tmpBasic.FightAbility;
			// 先加減
			tmpFight = addEffect.Aggregate (tmpFight, (accu, curr) => {
				return curr.Effect(accu);
			});
			// 後乘除
			tmpFight = multiEffect.Aggregate (tmpFight, (accu, curr) => {
				return curr.Effect (accu);
			});
			fight = tmpFight;
			return tmpBasic;
		}
	}
	#endregion

	#region item
	[Serializable]
	public struct Item : IEquatable<Item>{
		public string prototype;
		public int count;
		public bool Equals(Item other){
			return prototype == other.prototype && count == other.count;
		}
		public AbstractItem AbstractItem{
			get{
				AbstractItem ret;
				ret.prototype = prototype;
				ret.count = count;
				return ret;
			}
		}
		/// <summary>
		/// 方便用來計量道具減量
		/// </summary>
		/// <value>The negative.</value>
		public Item Negative{
			get{
				var a = this;
				a.prototype = prototype;
				a.count = -count;
				return a;
			}
		}
		public IEnumerable<ItemEffect> Effects {
			get {
				var cfg = ConfigItem.Get (prototype);
				if (cfg.Type != ConfigItemType.ID_weapon) {
					throw new Exception ("必須是武器:"+prototype);
				}
				return cfg.Ability.Split (new char[]{ ',' }).Select (v => {
					ItemEffect ef;
					ef.value = v;
					return ef;
				});
			}
		}
		public override string ToString(){
			var config = ConfigItem.Get (prototype);
			return string.Format ("({0}, {1})", config.Name, count);
		}
		public static Item Empty;
		// 向下相容
		public override bool Equals(object obj){
			if (!(obj is Item)){
				return false;
			}
			Item other = (Item) obj;
			return this.Equals(other);
		}
		// 必須同Equals(object)一同實做
		public override int GetHashCode(){
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + prototype.GetHashCode();
				hash = hash * 23 + count.GetHashCode();
				return hash;
			}
		}
	}

	public partial class Alg{
		public static IEnumerable<Item> ParseItem(string itemString){
			return ParseAbstractItem (itemString).Select (i => i.Item);
		}

		public static IEnumerable<Item> ParseItemFromResource(ConfigResource res){
			var hasItem = string.IsNullOrEmpty (res.Item) == false;
			if (hasItem == false) {
				return new List<Item> ();
			}
			return HanRPGAPI.Alg.ParseItem (res.Item);
		}

		public static Func<int> GetMaxCountFromItem(Item item){
			return () => {
				var config = ConfigItem.Get (item.prototype);
				var maxCount = config.MaxCount;
				return maxCount;
			};
		}

		/// <summary>
		///  新增道具，使用List強制Copy列表
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="input">Input.</param>
		/// <param name="item">Item.</param>
		public static List<Item> AddItem(List<Item> input, Item item){
			return AddItemWithFn (input, item, GetMaxCountFromItem (item));
		}

		/// <summary>
		/// 加入道具到指定列表
		/// </summary>
		/// <returns>The item.</returns>
		/// <param name="input">Input.</param>
		/// <param name="item">Item.</param>
		public static List<Item> AddItemWithFn(List<Item> input, Item item, Func<int> maxCountFn){
			var container = new List<Item> (input);
			var shouldArrange = true;
			var maxCount = maxCountFn ();
			if (item.count < 0) {
				// 處理減
				var allCount = input.Sum (it => {
					return it.prototype == item.prototype ? it.count : 0;
				});
				var isEnougth = allCount + item.count >= 0;
				if (isEnougth == false) {
					throw new HanRPGAPIException ("道具數量不足", null);
				}
				shouldArrange = true;
			} else {
				// 處理加
				for (var i = 0; i < container.Count; ++i) {
					var adjItem = container [i];
					if (adjItem.prototype != item.prototype) {
						continue;
					}
					if (adjItem.count + item.count > maxCount) {
						continue;
					}
					adjItem.count += item.count;
					container [i] = adjItem;
					shouldArrange = false;
					break;
				}
			}
			if (shouldArrange == false) {
				return container;
			}
			container.Add (item);
			// 計算同一種類的道具總數
			var sumOfCount = container.Where (obj => {
				return obj.prototype == item.prototype;
			}).Aggregate (0, (sum, obj) => {
				return sum + obj.count;
			});
			// 一個道具在一格中的最大數量限制
			var maxOfItem = maxCount;
			// 依最大限制重新計算分組
			var num = sumOfCount / maxOfItem;
			// 最後一個剩餘
			var remain = sumOfCount % maxOfItem;
			// 將拿來計算的道具抽出來
			var itemExcludeAddedItemPrototype = container.Where (obj => {
				return obj.prototype != item.prototype;
			});
			// 重建要新加入的道具
			var originItem = item;
			originItem.count = maxOfItem;
			var itemsShouldReAdd = Enumerable.Repeat (originItem, num);
			if (remain > 0) {
				originItem.count = remain;
				itemsShouldReAdd = itemsShouldReAdd.Concat (Enumerable.Repeat (originItem, 1));
			}
			// 加回去
			var newItems = itemExcludeAddedItemPrototype.Concat (itemsShouldReAdd);
			return newItems.ToList();
		}
	}
	#endregion

	#region fusion
	public partial class Alg{
		public static List<Item> Fusion(Item fusionTarget, List<Item> storage){
			var requires = ParseItem (ConfigItem.Get (fusionTarget.prototype).FusionRequire);
			var formatForSubstrct = requires.Select (item => {
				item.count = -(item.count*fusionTarget.count);
				return item;
			});
			var tempStorage = Enumerable.Aggregate (formatForSubstrct, storage, AddItem);
			var fusionItem = fusionTarget;
			tempStorage = AddItem (tempStorage, fusionItem);
			return tempStorage;
		}

		public static int CAN_NOT = -1;
		public static int REQUIREMENT_NOT_ALLOW = 0;

		public static int IsCanFusion(Func<string, int> expFn, string prototype, IEnumerable<Item> items){
			var cfg = ConfigItem.Get (prototype);
			// 判斷技能經驗是否符合
			var ais = ParseAbstractItem (cfg.SkillRequire);
			foreach (var ai in ais) {
				var st = ai.prototype;
				var needExp = ai.count;
				var haveExp = expFn (st);
				if (haveExp < needExp) {
					return CAN_NOT;
				}
			}
			// 判斷所需道具數量
			var requires = ParseItem (ConfigItem.Get (prototype).FusionRequire);
			return IsCanFusion (requires.Select (i => i.AbstractItem), items.Select (i => i.AbstractItem));
			/*
			int minCnt = int.MaxValue;
			foreach (var requireItem in requires) {
				var search = items.Where (it => {
					return it.prototype == requireItem.prototype && it.count >= requireItem.count;
				});
				var isNotFound = search.Count () == 0;
				if (isNotFound) {
					return CAN_NOT;
				}
				var total = search.Sum (it => it.count);
				var maxFusionCnt = total / requireItem.count;
				if (minCnt > maxFusionCnt) {
					minCnt = maxFusionCnt;
				}
			}
			return minCnt;
			*/
		}

		public static int IsCanFusion(IEnumerable<AbstractItem> requires, IEnumerable<AbstractItem> items){
			// 判斷所需道具數量
			int minCnt = int.MaxValue;
			foreach (var requireItem in requires) {
				var search = items.Where (it => {
					return it.prototype == requireItem.prototype && it.count >= requireItem.count;
				});
				var isNotFound = search.Count () == 0;
				if (isNotFound) {
					return CAN_NOT;
				}
				var total = search.Sum (it => it.count);
				var maxFusionCnt = total / requireItem.count;
				if (minCnt > maxFusionCnt) {
					minCnt = maxFusionCnt;
				}
			}
			return minCnt;
		}
	}
	#endregion


	#region npc
	[Serializable]
	public struct NPC{
		public string prototype;
	}

	[Serializable]
	public struct NpcMission{
		public string prototype;
		public List<string> monsterSkilled;
		public List<Item> itemGot;
		public static NpcMission Default = new NpcMission {
			monsterSkilled = new List<string> (),
			itemGot = new List<Item> ()
		};
	}

	public partial class Alg{
		public static IEnumerable<string> AvailableNpcMissions(Func<string, bool> missionCompletedFn, int advLevel) {
			return 
				Enumerable.Range (0, ConfigNpcMission.ID_COUNT)
					.Select (ConfigNpcMission.Get)
					.Where (cfg => cfg.Level <= advLevel)
					.Where(cfg=>missionCompletedFn(cfg.ID) == false)
					.Where (cfg => {
						if (cfg.Dependency != null) {
							var items = ParseAbstractItem (cfg.Dependency);
							foreach (var item in items) {
								var missionId = item.prototype;
								var isCompleted = missionCompletedFn (missionId);
								if (isCompleted == false) {
									return false;
								}
							}
							return true;
						}
						return true;
					})
					.Select (cfg => cfg.ID);
		}

		/// <summary>
		/// 判斷任務是否完成，每次互動後可以呼叫一次
		/// </summary>
		/// <returns>完成的任務</returns>
		public static List<string> CheckMissionStatus(IEnumerable<NpcMission> missionStatus){
			return missionStatus.Aggregate (new List<string> (), (ret, mission) => {
				var cfg = ConfigNpcMission.Get (mission.prototype);
				if (cfg.RequireItem != null) {
					var isCompleted = true;
					var requireItems = HanRPGAPI.Alg.ParseItem (cfg.RequireItem);
					foreach (var requireItem in requireItems) {
						var itemCount = mission.itemGot
							.Where(item=>item.prototype == requireItem.prototype)
							.Sum(item=>item.count);
						if (itemCount < requireItem.count) {
							isCompleted = false;
							break;
						}
					}
					if (isCompleted) {
						ret.Add (mission.prototype);
					}
				}

				if (cfg.RequireKill != null) {
					var isCompleted = true;
					var requireItems = HanRPGAPI.Alg.ParseAbstractItem (cfg.RequireKill);
					foreach (var requireItem in requireItems) {
						var itemCount = mission.monsterSkilled
							.Where (id => id == requireItem.prototype)
							.Count ();
						if (itemCount < requireItem.count) {
							isCompleted = false;
							break;
						}
					}
					if (isCompleted) {
						ret.Add (mission.prototype);
					}
				}

				if (cfg.RequireStatus != null) {
					var isCompleted = true;
					var requireItems = HanRPGAPI.Alg.ParseAbstractItem (cfg.RequireKill);
					foreach (var requireItem in requireItems) {
						if (requireItem.prototype == "money") {
							// TODO
						}
					}
					if (isCompleted) {
						ret.Add (mission.prototype);
					}
				}
				return ret;
			});
		}
	}
	#endregion

	#region weapon and skill
	public partial class Alg{
		
		public static IEnumerable<string> AvailableSkills(IEnumerable<string> slotSkills, List<Item> weapons){
			// 再取得武器本身的招式
			var handWeapons = weapons.Select(i=>ConfigItem.Get(i.prototype)).Where(i=>i.Position == ConfigWeaponPosition.ID_hand);
			var hasHandWeapons = handWeapons.Count () > 0;
			if (hasHandWeapons == false) {
				return slotSkills;
			}
			var skills = Enumerable
				.Range(0, ConfigSkill.ID_COUNT)
				.Select(ConfigSkill.Get);
			var weaponSkills = handWeapons.SelectMany (w => {
				return skills.Where(s=>{
					return s.SlotCount == 0 && s.SkillTypeRequire.Contains(w.SkillType);
				}).Select(s=>s.ID);
			}).Distinct();
			return slotSkills.Concat (weaponSkills);

			/*
			var skills = Enumerable
				.Range(0, ConfigSkill.ID_COUNT)
				.Select(ConfigSkill.Get);
			var who = player.GetMapPlayer (who_);
			var weapons = who.weapons;
			var useWeaponTypes = weapons.Select (i => ConfigItem.Get (i.prototype).SkillType);
			return skills.Where (cfg => {
				// 判斷技能類型需求
				// 比如：需要拳術5級和劍術3級
				var ais = Common.Common.ParseAbstractItem(cfg.SkillTypeRequire);
				foreach(var ai in ais){
					var skillType = ai.prototype;
					var skillLevel = ai.count;
					// 其中一項不符就回傳
					if(who.Exp(skillType) < skillLevel){
						return false;
					}
				}
				// 判斷這個技能是不是需要武器
				var isNeedWeapon = cfg.SlotCount == 0;
				if(isNeedWeapon == true){
					// 以第一個需求技能的代表武器為主
					var firstSkill = ais.FirstOrDefault();
					// 有需要武器一定要有所需技能類型
					var isInvalidConfig = firstSkill.Item.Equals(Item.Empty);
					if(isInvalidConfig){
						throw new Exception("錯誤的設定:"+cfg.SkillTypeRequire);
					}
					var skillType = firstSkill.prototype;
					// 判斷有沒有裝備該類技能類型的武器
					var isMatch = useWeaponTypes.Where(st=>st==skillType).Count()>0;
					if(isMatch == false){
						return false;
					}
				} else {
					// slot的招式
					// TODO 判斷有沒有裝備這個招式
				}
				return true;
			});
			*/
		}

		public static string IsCanEquip(List<Item> weapons, Item weapon){
			var cfg = ConfigItem.Get (weapon.prototype);
			if (cfg.Type != ConfigItemType.ID_weapon) {
				return "只能裝備weapon類型，請檢查程式";
			}
			var weaponPosition = cfg.Position;
			var maxCount = ConfigWeaponPosition.Get (weaponPosition).SlotCount;
			var alreadyEquipCount = weapons.Count(i=>{
				return ConfigItem.Get (i.prototype).Position == weaponPosition;
			});
			if(alreadyEquipCount >= maxCount){
				return "那個位置已經滿, 最大為"+maxCount+":"+weaponPosition+". 所使用Weapon:"+cfg.Name;
			}
			return null;
		}
		/// <summary>
		/// 判斷武器有沒有壞，每次擊中對手時呼叫
		/// </summary>
		/// <returns>壞掉的武器</returns>
		public static IEnumerable<Item> CheckHandWeaponBroken(IEnumerable<Item> weapons){
			return weapons
				.Select (i => new Tuple2<Item, ConfigItem> (i, ConfigItem.Get (i.prototype)))
				.Where (info => info.t2.Position == ConfigWeaponPosition.ID_hand)
				.Select (info => new Tuple2<Item, int> (info.t1, (int)((1.0f / info.t2.UseCount) * 100)))
				.Where (info => {
					var dice = UnityEngine.Random.Range (1, 101);
					return dice < info.t2;
				})
				.Select (info => info.t1);
		}
		/// <summary>
		/// 判斷防具有沒有壞，每次被擊中時呼叫
		/// </summary>
		/// <returns>壞掉的防具</returns>
		public static IEnumerable<Item> CheckElseWeaponBroken(IEnumerable<Item> weapons){
			return weapons
				.Select (i => new Tuple2<Item, ConfigItem> (i, ConfigItem.Get (i.prototype)))
				.Where (info => info.t2.Position != ConfigWeaponPosition.ID_hand)
				.Select (info => new Tuple2<Item, int> (info.t1, (int)((1.0f / info.t2.UseCount) * 100)))
				.Where (info => {
					var dice = UnityEngine.Random.Range (1, 101);
					return dice < info.t2;
				})
				.Select (info => info.t1);
		}
	}

	#endregion

	#region map

	// 有Serializable才能json字串化
	[Serializable]
	public struct Position : IEquatable<Position>{
		public int x, y;
		public Position Add(int x, int y){
			var ret = this;
			ret.x += x;
			ret.y += y;
			return ret;
		}
		public Position Add(Position b){
			return Add (b.x, b.y);
		}
		public Position Negative{
			get{
				return Zero.Add (-x, -y);
			}
		}
		public Position Max(int x, int y){
			var ret = this;
			ret.x = Math.Max (x, this.x);
			ret.y = Math.Max (y, this.y);
			return ret;
		}
		public Position Max(Position b){
			return Max (b.x, b.y);
		}
		public Position Min(int x, int y){
			var ret = this;
			ret.x = Math.Min (x, this.x);
			ret.y = Math.Min (y, this.y);
			return ret;
		}
		public bool Equals(Position other){
			return x == other.x && y == other.y;
		}
		// 向下相容
		public override bool Equals(object obj){
			if (!(obj is Position)){
				return false;
			}
			Position other = (Position) obj;
			return this.Equals(other);
		}
		// 必須同Equals(object)一同實做
		public override int GetHashCode(){
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + x.GetHashCode();
				hash = hash * 23 + y.GetHashCode();
				return hash;
			}
		}
		public override string ToString(){
			return string.Format ("({0}, {1})", x, y);
		}
		public static Position Zero;
	}

	public partial class Alg{
		public static string Terrian(IEnumerable<AbstractItem> infoList){
			if (infoList.Count () == 0) {
				throw new Exception ("沒有合適的地形");
			}

			// 將地上物轉為虛擬物件，方便計算是否符合地形需求
			var resList = infoList.Select (info => info.Item).Aggregate (new List<Item> (), (ret, i) => {
				return HanRPGAPI.Alg.AddItemWithFn(ret, i, ()=>9999999);
			}).Select(i=>i.AbstractItem);

			// 地形判斷依Class為優先順序判斷
			var checkTypes = Enumerable.Range (0, ConfigTerrian.ID_COUNT)
				.Select (ConfigTerrian.Get)
				.OrderByDescending (cfg => cfg.Class);

			var terrians = checkTypes.SkipWhile (t => {
				var resRequire = HanRPGAPI.Alg.ParseAbstractItem (t.Require);
				var check = HanRPGAPI.Alg.IsCanFusion (resRequire, resList);
				if (check <= HanRPGAPI.Alg.REQUIREMENT_NOT_ALLOW) {
					return true;
				}
				return false;
			});

			var isNoMatch = terrians.Count () == 0;
			if (isNoMatch) {
				throw new Exception ("沒有合適的地形");
			}

			return terrians.First ().ID;
		}

		/// <summary>
		/// AbstractItem{
		/// 	prototype // ConfigResource.ID
		/// 	count // amount
		/// }
		/// </summary>
		/// <returns>The monster2.</returns>
		/// <param name="pos">Position.</param>
		/// <param name="genInfo">Gen info.</param>
		public static IEnumerable<AbstractItem> GenMonsterPattern(Position pos, IEnumerable<AbstractItem> genInfo){
			var ret = new List<string>();
			// 生成怪物
			var terrianId = Terrian (genInfo);
			for (var i = 0; i < ConfigMonster.ID_COUNT; ++i) {
				var monster = ConfigMonster.Get (i);
				if (monster.Terrian != null) {
					if (monster.Terrian.Contains (terrianId)) {
						var dice = UnityEngine.Random.Range (0, 100);
						if (dice < 20) {
							ret.Add (monster.ID);
						}
					}
				}
			}
			return ret.Select(str=>new AbstractItem{
				prototype = str,
				count = 1
			}).Select(i=>i.Item).Aggregate(new List<Item>(), (total,i)=>{
				return HanRPGAPI.Alg.AddItemWithFn(total, i, ()=>999999);
			}).Select(i=>i.AbstractItem);
		}

		/// <summary>
		/// AbstractItem{
		/// 	prototype // ConfigResource.ID
		/// 	count // amount
		/// }
		/// </summary>
		/// <returns>The resource2.</returns>
		/// <param name="sea">Sea.</param>
		/// <param name="temperature">Temperature.</param>
		public static Func<Position, IEnumerable<AbstractItem>> GenResourcePattern(float sea = 2000f, float temperature = 20f){
			var heightFactor = 1 / 8000f;
			var factor = 1 / 10f;

			return (Position pos) => {
				var ret = new List<string>();

				var seaHeight = sea*heightFactor;
				var height = Mathf.PerlinNoise (pos.x * factor, pos.y * factor);

				var heightOffset = height - seaHeight;
				var currTemp = temperature + (heightOffset / heightFactor / 100);

				// 加入氣溫
				if (currTemp < 10) {
					ret.Add(ConfigResource.ID_temperature1);
				} else if (currTemp < 30) {
					ret.Add(ConfigResource.ID_temperature2);
				} else {
					ret.Add(ConfigResource.ID_temperature3);
				}

				// 隨機加水
				var waterNoisePos = pos.Add (50, 50);
				var waterWeight = Mathf.PerlinNoise (waterNoisePos.x * factor, waterNoisePos.y * factor);
				var waterCnt = (int)(waterWeight / 0.4f);
				for (var i = 0; i < waterCnt; ++i) {
					ret.Add(ConfigResource.ID_water);
				}
				// 隨機山丘
				var mountantNoisePos = pos.Add (100, 50);
				var mountantWeight = Mathf.PerlinNoise (mountantNoisePos.x * factor, mountantNoisePos.y * factor);
				var mountantCnt = (int)(mountantWeight / 0.4f);
				for (var i = 0; i < mountantCnt; ++i) {
					ret.Add(ConfigResource.ID_smallMountain);
				}

				// 小於海平面，加二個水
				if (height <= seaHeight) {
					ret.Add(ConfigResource.ID_water);
					ret.Add(ConfigResource.ID_water);

				} else if(height - seaHeight >= 0.5){
					ret.Add(ConfigResource.ID_smallMountain);
					ret.Add(ConfigResource.ID_smallMountain);

				} else {
					for (var i = 0; i < 3; ++i) {
						var dice = UnityEngine.Random.Range (0, 100);
						if (dice < 20) {
							ret.Add(ConfigResource.ID_grass);
						}
						dice = UnityEngine.Random.Range (0, 100);
						if (dice < 20) {
							ret.Add(ConfigResource.ID_tree);
						}
						dice = UnityEngine.Random.Range (0, 100);
						if (dice < 20) {
							ret.Add(ConfigResource.ID_stone);
						}
					}
				}

				return ret.Select(str=>new AbstractItem{
					prototype = str,
					count = 1
				}).Select(i=>i.Item).Aggregate(new List<Item>(), (total,i)=>{
					return HanRPGAPI.Alg.AddItemWithFn(total, i, ()=>999999);
				}).Select(i=>i.AbstractItem);
			};
		}
	}
	#endregion

	#region util
	[Serializable]
	public struct AbstractItem : IEquatable<AbstractItem>{
		public string prototype;
		public int count;
		public bool Equals(AbstractItem other){
			return prototype == other.prototype && count == other.count;
		}
		public Item Item{
			get{
				Item ret;
				ret.prototype = prototype;
				ret.count = count;
				return ret;
			}
		}
		public static AbstractItem Empty;
	}

	public class HanRPGAPIException : Exception{
		public HanRPGAPIException(string msg, Exception inner) : base(msg, inner){}
	}

	public class Tuple2<T,T2>{
		public T t1;
		public T2 t2;
		public Tuple2(T t1, T2 t2){
			this.t1 = t1;
			this.t2 = t2;
		}
	}

	public partial class Alg{
		public static IEnumerable<AbstractItem> ParseAbstractItem(string itemString){
			Func<string, AbstractItem> parseOne = str => {
				var prototype = str;
				var count = 1;
				var hasCount = str.IndexOf ("_") != -1;
				if (hasCount) {
					var info = str.Split (new char[]{ '_' }, StringSplitOptions.None);
					prototype = info[0];
					try{
						count = int.Parse (info [1]);
					}catch(Exception){
						throw new Exception ("Resource中的Item欄位格式定義錯誤:"+str);
					}
				}
				AbstractItem item;
				item.prototype = prototype;
				item.count = count;
				return item;
			};
			if (string.IsNullOrEmpty(itemString)) {
				return new List<AbstractItem> ();
			}
			var hasMulti = itemString.IndexOf (",") != -1;
			if (hasMulti) {
				var strs = itemString.Split (new char[]{ ',' }, StringSplitOptions.None);
				return strs.Select (parseOne);
			}
			return Enumerable.Repeat (parseOne (itemString), 1);
		}
	}
	#endregion
}