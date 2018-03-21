using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using System.Linq;
using Common;
using HanRPGAPI;

namespace Model
{
	// 下[Serializable]的話，會自動存入本機和自動讀取，真是奇怪(方便?)的功能
	// 不使用上述的功能
	// 地圖的部分另外存，因為資料量比較大
	// 所以把比較長變動的資料移到PlayerDataStore
	public class MapDataStore{
		#region map
		// Dictionary無法Serializable
		public List<MapObject> mapObjects = new List<MapObject>();
		public List<ResourceInfo> resourceInfo = new List<ResourceInfo>();
		public List<MonsterInfo> monsterInfo = new List<MonsterInfo>();

		MapType genMapType;


		public void GenMapStart(MapType mapType){
			genMapType = mapType;

		}

		public void GenMapWithPlayerVisible(PlayerDataStore player){
			GenMapWithPositions (mapObjects.Select (o => o.position), player);
		}

		public void GenMapWithPositions(IEnumerable<Position> posList, PlayerDataStore player){
			if (genMapType == MapType.Unknown) {
				throw new Exception ("還沒指定地圖類型");
			}
			Func<Position, IEnumerable<AbstractItem>> genResFn = null;
			Func<Position, IEnumerable<AbstractItem>, IEnumerable<AbstractItem>> genMonFn = null;
			switch (genMapType) {
			case MapType.Pattern:
				{
					genResFn = HanRPGAPI.Alg.GenResourcePattern(4000, 25);
				}
				break;
			}
			genMonFn = HanRPGAPI.Alg.GenMonsterPattern;

			var alreadyPos = new HashSet<Position> (posList);
			foreach (var pos in player.isPositionVisible) {
				if (alreadyPos.Contains (pos)) {
					continue;
				}
				alreadyPos.Add (pos);

				switch (genMapType) {
				case MapType.Random:
					{
						for (var i = 0; i < ConfigResource.ID_COUNT; ++i) {
							var dice = UnityEngine.Random.Range (0, 100);
							if (dice < 60) {
								var id = ConfigResource.Get (i).ID;
								GenResource (player, pos, id);
							}
						}

						for (var i = 0; i < ConfigMonster.ID_COUNT; ++i) {
							var dice = UnityEngine.Random.Range (0, 100);
							if (dice < 60) {
								var id = ConfigMonster.Get (i).ID;
								GenMonster (player, pos, id);
							}
						}
					}
					break;
				case MapType.Pattern:
					{
						var res = genResFn (pos);
						var mon = genMonFn (pos, res);
						foreach (var ai in res) {
							for (var i = 0; i < ai.count; ++i) {
								GenResource (player, pos, ai.prototype);
							}
						}
						foreach (var ai in mon) {
							for (var i = 0; i < ai.count; ++i) {
								GenMonster (player, pos, ai.prototype);
							}
						}
					}
					break;
				}
			}
		}

		public void ClearMap(){
			mapObjects.Clear ();
			resourceInfo.Clear ();
			monsterInfo.Clear ();
		}
		public int GenObject(MapObjectType type, string strKey){
			if (strKey != null) {
				if(FindObject (strKey).Equals(MapObject.Empty) == false){
					throw new UnityException ("item already exists!"+strKey);
				}
			}
			MapObject item = MapObject.Empty;
			item.type = type;
			if (strKey != null) {
				item.strKey = strKey;
			}
			switch (item.type) {
			case MapObjectType.Resource:
				item.infoKey = resourceInfo.Count;
				resourceInfo.Add (ResourceInfo.Empty);
				break;
			case MapObjectType.Monster:
				item.infoKey = monsterInfo.Count;
				monsterInfo.Add (MonsterInfo.Empty);
				break;
			}
			// 先取得數字鍵
			item.key = mapObjects.Count;
			// 再加入到串列
			mapObjects.Add (item);
			return item.key;
		}
		public int GenResource(PlayerDataStore player, Position pos, string resourceType){
			var key = GenObject (MapObjectType.Resource, null);
			var obj = mapObjects [key];
			obj.position = pos;
			mapObjects [key] = obj;

			var info = resourceInfo [mapObjects [key].infoKey];
			info.type = resourceType;
			resourceInfo [mapObjects [key].infoKey] = info;
			return key;
		}
		public int GenMonster(PlayerDataStore player, Position pos, string monsterType){
			var m1Key = GenObject (MapObjectType.Monster, null);

			var m1Object = mapObjects [m1Key];
			m1Object.position = pos;
			mapObjects [m1Key] = m1Object;

			var m1Info = monsterInfo [m1Object.infoKey];
			m1Info.type = monsterType;
			var basic = Common.Common.GetBasicAbility (m1Info);
			var fight = basic.FightAbility;
			m1Info.hp = (int)fight.hp;
			m1Info.mp = (int)fight.mp;
			m1Info.basicAbility = basic;
			m1Info.bufs = new List<Buf> ();
			monsterInfo [m1Object.infoKey] = m1Info;
			return m1Key;
		}

		public int WorkConsumpation(PlayerDataStore player, Description work){
			return ConfigConst.Get(ConfigConst.ID_workConsumpation).Int;
		}

		public int MoveConsumption(PlayerDataStore player, Position a, Position b){
			if (a.Equals (b)) {
				return 0;
			}
			var offset = b.Add (a.Negative);
			if (Math.Abs (offset.x) > 1 || Math.Abs (offset.y) > 1) {
				return int.MaxValue;
			}
			return ConfigConst.Get(ConfigConst.ID_moveConsumpation).Int;;
		}

		public List<Item> Collect(PlayerDataStore player, int objKey){
			var ret = new List<Item> ();
			var obj = mapObjects [objKey];
			if (obj.died == true) {
				throw new Exception ("不能採集已消除的物件:"+objKey);
			}
			if (obj.type != MapObjectType.Resource) {
				throw new Exception ("非資源不得採集:"+obj.type);
			}
			obj.died = true;
			// assign back
			mapObjects [objKey] = obj;
			foreach (var item in ret) {
				player.AddItem (item, Place.Map);
			}
			return ret;
		}
		public MapObject FindObject(string strKey){
			return mapObjects.Find (item => {
				return item.strKey == strKey;
			});
		}
		public IEnumerable<MapObject> FindObjects(Position pos, bool filterDied = true){
			return mapObjects.Where (item => {
				if(filterDied & item.died){
					return false;
				}
				return item.position.Equals(pos);
			});
		}
		#endregion

		#region event
		public IEnumerable<Description> GenEvent(PlayerDataStore player, Position pos){
			var objs = FindObjects (pos);
			return objs.Aggregate(
				new List<Description>(),
				(accu, obj)=>{
					switch(obj.type){
					case MapObjectType.Resource:
						{
							/*
							var des = Description.Empty;
							des.description = Description.EventLucklyFind;
							des.values = new NameValueCollection();
							des.values.Add("itemPrototype", ConfigItem.ID_wood);
							des.values.Add("count", 10+"");
							accu.Add(des);
							*/
						}
						break;
					case MapObjectType.Monster:
						break;
					}
					return accu;
				}
			);
		}
		public void ApplyEvents(PlayerDataStore player, IEnumerable<Description> events){
			foreach (var evt in events) {
				switch (evt.description) {
				case Description.EventLucklyFind:
					{
						var itemPrototype = evt.values.Get("itemPrototype");
						var cnt = int.Parse(evt.values.Get("count"));
						Item item;
						item.prototype = itemPrototype;
						item.count = cnt;
						player.playerInMap.storage = HanRPGAPI.Alg.AddItem (player.playerInMap.storage, item);
					}
					break;
				default:
					throw new NotImplementedException ("event:"+evt.description);
				}
			}
		}
		#endregion

		// 武器壞掉
		IEnumerable<Description> ProcessWeaponBroken(PlayerDataStore player){			
			// 注意：使用ToList將陣列Copy
			var brokenWeapons = player.playerInMap.CheckHandWeaponBroken ().ToList ();
			// 刪除壞掉武器
			foreach (var broken in brokenWeapons) {
				player.playerInMap.weapons.Remove (broken);
			}
			// === 回傳壞掉資訊 === //
			if (brokenWeapons.Count () == 0) {
				return new List<Description> ();
			}
			var des = Description.Empty;
			des.description = Description.InfoWeaponBroken;
			des.values = new NameValueCollection ();
			foreach (var broken in brokenWeapons) {
				var jsonstr = JsonUtility.ToJson (broken);
				des.values.Add ("items", jsonstr);
			}
			return new List<Description> (){ des };
		}

		// 傷害
		IEnumerable<Description> ProccessDamageWork(PlayerDataStore player, int mapObjectId, int damage){
			var ret = new List<Description>();

			var mapObject = mapObjects [mapObjectId];
			var monsterInf = monsterInfo[mapObject.infoKey];
			var monsterCfg = ConfigMonster.Get (monsterInf.type);

			monsterInf.hp -= damage;
			// 計算仇恨值
			monsterInf.BeAttacked (damage);
			if (monsterInf.IsDied) {
				mapObject.died = true;
				// 獎勵
				var rewards = HanRPGAPI.Alg.ParseItem (monsterCfg.Item);
				foreach (var reward in rewards) {
					player.playerInMap.storage = HanRPGAPI.Alg.AddItem (player.playerInMap.storage, reward);
				}
			}
			monsterInfo [mapObject.infoKey] = monsterInf;
			mapObjects [mapObjectId] = mapObject;

			// 傷害事件
			var des = Description.Empty;
			des.description = Description.InfoAttack;
			des.values = new NameValueCollection ();
			des.values.Set ("mapObjectId", mapObjectId + "");
			des.values.Set ("damage", damage + "");
			des.values.Set ("isCriHit", "0");
			ret.Add (des);

			// 怪物死亡事件
			if (monsterInf.IsDied) {
				des = Description.Empty;
				des.description = Description.InfoMonsterDied;
				des.values = new NameValueCollection ();
				des.values.Set ("mapObjectId", mapObjectId + "");
				// 獎勵資訊
				var rewards = HanRPGAPI.Alg.ParseItem (monsterCfg.Item);
				foreach (var json in rewards.Select(i=>JsonUtility.ToJson(i))) {
					des.values.Add ("rewards", json);
				}
				ret.Add (des);
			}

			return ret;
		}

		public IEnumerable<Description> ProcessWork(PlayerDataStore player, Description work){
			var ret = new List<Description>();
			LabelProcessWork:
			switch (work.description) {
			case Description.WorkUseSkillForEnemyAll:
				{
					var skillId = work.values.Get ("skillId");
					var mapObjectIds = work.values.GetValues ("mapObjectIds").Select (int.Parse).ToList ();
					var skill = ConfigSkill.Get (skillId);

					var playerBasic = BasicAbility.Zero;
					var playerAbility = FightAbility.Zero;
					Helper.CalcAbility (player, this, Place.Map, ref playerBasic, ref playerAbility);
					var damage = playerBasic.agi * playerBasic.dex;
					damage = Math.Max (0, damage);

					var des = Description.Empty;
					des.description = Description.InfoUseSkill;
					des.values = new NameValueCollection ();
					des.values.Add ("skills", skill.ID);
					ret.Add (des);

					foreach (var mapObjectId in mapObjectIds) {
						var mapObject = mapObjects [mapObjectId];
						var monsterInf = monsterInfo[mapObject.infoKey];
						if (monsterInf.IsDied) {
							Debug.LogWarning ("怪物已無法戰鬥");
							continue;
						}
						switch (skill.ID) {
						case ConfigSkill.ID_spinAttack:
							{
								var infos = ProccessDamageWork (player, mapObjectId, (int)damage);
								infos.Concat (ProcessWeaponBroken (player));

								ret.AddRange (infos);
							}
							break;
						}
					}
				}
				break;
			case Description.WorkSelectSkillForEnemy:
				{
					var skillId = work.values.Get ("skillId");
					var mapObjectId = int.Parse(work.values.Get("mapObjectId"));
					var mapObject = mapObjects [mapObjectId];
					var monsterInf = monsterInfo[mapObject.infoKey];
					if (monsterInf.IsDied) {
						Debug.LogWarning ("怪物已無法戰鬥");
						goto LabelProcessWork;
					}

					var des = Description.Empty;
					des.description = Description.InfoUseSkill;
					des.values = new NameValueCollection ();
					des.values.Add ("skills", skillId);
					ret.Add (des);

					var skill = ConfigSkill.Get (skillId);
					switch (skill.ID) {
					case ConfigSkill.ID_karadaAttack:
						{
							// 身體撞擊
							var playerBasic = BasicAbility.Zero;
							var playerAbility = FightAbility.Zero;
							Helper.CalcAbility (player, this, Place.Map, ref playerBasic, ref playerAbility);
							var damage = playerBasic.agi * playerAbility.def;
							damage = (int)Math.Max (0, damage);

							var infos = ProccessDamageWork (player, mapObjectId, (int)damage);
							ret.AddRange (infos);
						}
						break;
					case ConfigSkill.ID_bokyoryokuhakai:
						{
							// 防禦破壞
							monsterInf.AddBuf (Helper.GetBuf (skill));
							monsterInfo [mapObject.infoKey] = monsterInf;
						}
						break;
					}
				}
				break;
			case Description.WorkUseTurnSkill:
				{
					var skillId = work.values.Get ("skillId");
					var skill = ConfigSkill.Get (skillId);
					switch (skill.Effect) {
					case "{0}+{1}":
						player.playerInMap.hp += 20;
						var des = Description.Empty;
						des.description = Description.InfoUseSkill;
						des.values = new NameValueCollection ();
						des.values.Add ("skills", skill.ID);
						ret.Add (des);
						break;
					default:
						throw new Exception ("未處理的回合性招式:"+skill.ID);
					}
				}
				break;
			case Description.WorkCollectResource:
				{
					var mapObjectId = int.Parse(work.values.Get("mapObjectId"));
					var obj = mapObjects [mapObjectId];
					obj.died = true;
					mapObjects [mapObjectId] = obj;
					var info = resourceInfo [obj.infoKey];
					var config = ConfigResource.Get (info.type);
					var hasItem = config.Item != null;
					if (hasItem) {
						var items = HanRPGAPI.Alg.ParseItemFromResource (config);
						foreach (var item in items) {
							player.AddItem (item, Place.Map);
						}
						// 增加採集經驗
						foreach (var item in items) {
							var cfg = ConfigItem.Get (item.prototype);
							if (cfg.SkillType != null) {
								player.playerInMap.AddExp (cfg.SkillType, 1);
							}
						}

						// 發送任務更新
						foreach (var item in items) {
							player.NotifyMissionAddItemFromCollect (item);
						}

						var des = Description.Empty;
						des.description = Description.InfoCollectResource;
						des.values = new NameValueCollection ();
						foreach (var itemJson in items.Select(i=>JsonUtility.ToJson(i))) {
							des.values.Add ("items", itemJson);
						}
						ret.Add (des);
					}
				}
				break;
			case Description.WorkAttack:
				{
					var mapObjectId = int.Parse(work.values.Get("mapObjectId"));
					var mapObject = mapObjects [mapObjectId];
					var monsterInf = monsterInfo[mapObject.infoKey];
					// 怪物逃走了
					if (monsterInf.IsDied) {
						Debug.LogWarning ("怪物已逃走");
						break;
					}
					// === 計算傷害 === //
					var monsterCfg = ConfigMonster.Get(monsterInf.type);
					var monsterAbility = Helper.CalcMonsterAbility (player, this, mapObjectId).FightAbility;
					var playerBasic = BasicAbility.Zero;
					var playerAbility = FightAbility.Zero;
					// 先計算非針對怪物的能力
					Helper.CalcAbility (player, this, Place.Map, ref playerBasic, ref playerAbility);
					// 計算針對怪物的能力，例：如果對像是鳥，攻擊力*1.1之類
					var enforceEff = player.playerInMap.weapons.SelectMany (it => it.Effects).Where(it=>it.EffectOperator==ItemEffectType.Enforce);
					playerAbility = enforceEff.Aggregate (playerAbility, (accu, curr) => {
						// TODO 實做針對性能力
						return playerAbility;
					});
					// === 技能加成 === //
					var skills = Helper.AvailableSkills (player, Place.Map);
					// 只計算攻擊性技能
					skills = skills.Where (s => s.Condition == ConfigConditionType.ID_attack);
					// 取出發動的技能
					var triggered = skills.Where (s => {
						var rate = (int)(Helper.ComputeSkillTriggerRate (player.playerInMap, s) * 100);
						return UnityEngine.Random.Range(1, 101) < rate;
					});
					// 取出技能效果
					var triggeredEffect = triggered.SelectMany (HanRPGAPI.Alg.Effect);
					playerAbility = triggeredEffect.Aggregate(playerAbility, (v,effect)=>{
						switch(effect.EffectOperator){
						case ItemEffectType.Unknown:
							{
								switch(effect.value){
								default:
									throw new NotImplementedException("未確定的招式:"+effect.value);
								}
							}
						default:
							{
								playerAbility = effect.Effect(playerAbility);
							}
							break;
						}
						return playerAbility;
					});

					// 判斷是否命中
					var accuracyRate = playerAbility.AccuracyRate (monsterAbility);
					var isHit = UnityEngine.Random.Range (1, 101) < (int)(accuracyRate * 100);
					if (isHit == false) {
						var des = Description.Empty;
						// === 回傳使用招式 === //
						if (triggered.Count () > 0) {
							des = Description.Empty;
							des.description = Description.InfoUseSkill;
							des.values = new NameValueCollection ();
							foreach (var s in triggered) {
								des.values.Add ("skills", s.ID);
							}
							ret.Add (des);
						}

						// === 回傳怪物回避攻擊 === //
						des = Description.Empty;
						des.description = Description.InfoMonsterDodge;
						des.values = new NameValueCollection ();
						des.values.Set ("mapObjectId", mapObjectId + "");
						ret.Add (des);
					} else {
						// 總傷害
						var damage = playerAbility.Damage (monsterAbility);
						var criRate = playerAbility.CriticalHitRate (monsterAbility);
						var isCriHit = UnityEngine.Random.Range (1, 101) < (int)(criRate * 100);
						if (isCriHit) {
							damage = (int)(damage*1.5f);
						}
						damage = Math.Max (0, damage);
						// === 套用傷害 === //
						ret.AddRange (ProccessDamageWork(player, mapObjectId, damage));
						// === 處理武器壞掉 === //
						ret.AddRange (ProcessWeaponBroken (player));

						// 增加武器經驗
						foreach (var item in player.playerInMap.weapons) {
							var cfg = ConfigItem.Get (item.prototype);
							if (cfg.Position != ConfigWeaponPosition.ID_hand) {
								continue;
							}
							if (cfg.SkillType != null) {
								player.playerInMap.AddExp (cfg.SkillType, 1);
							}
						}

						// ====== 以下處理回傳 ====== //
						var des = Description.Empty;
						// === 回傳使用招式 === //
						if (triggered.Count () > 0) {
							des = Description.Empty;
							des.description = Description.InfoUseSkill;
							des.values = new NameValueCollection ();
							foreach (var s in triggered) {
								des.values.Add ("skills", s.ID);
							}
							ret.Add (des);
						}
					}
				}
				break;
			case Description.EventMonsterAttackYou:
				{
					var mapObjectId = int.Parse(work.values.Get("mapObjectId"));
					var mapObject = mapObjects [mapObjectId];
					var monsterInf = monsterInfo[mapObject.infoKey];

					//var monsterCfg = ConfigMonster.Get(monsterInf.type);
					var monsterAbility = Helper.CalcMonsterAbility (player, this, mapObjectId).FightAbility;
					var playerBasic = BasicAbility.Zero;
					var playerAbility = FightAbility.Zero;
					// 先計算非針對怪物的能力
					Helper.CalcAbility (player, this, Place.Map, ref playerBasic, ref playerAbility);
					// 計算針對怪物的能力，例：如果對像是鳥，防禦力*1.1之類
					var enforceEff = player.playerInMap.weapons.SelectMany (it => it.Effects).Where(it=>it.EffectOperator==ItemEffectType.Enforce);
					playerAbility = enforceEff.Aggregate (playerAbility, (accu, curr) => {
						// TODO 實做針對性能力
						return playerAbility;
					});
					// 判斷是否命中
					var accuracyRate = playerAbility.AccuracyRate (monsterAbility);
					var isHit = UnityEngine.Random.Range (1, 101) < (int)(accuracyRate * 100);
					if (isHit == false) {
						// === 回傳回避攻擊 === //
						var des = Description.Empty;
						des.description = Description.InfoDodge;
						des.values = new NameValueCollection ();
						des.values.Set ("mapObjectId", mapObjectId + "");
						ret.Add (des);
						// 回避會增加速度經驗
						player.playerInMap.AddExp (ConfigAbility.ID_speed, 1);

					} else {
						var des = Description.Empty;

						// === 技能加成 === //
						var skills = Helper.AvailableSkills (player, Place.Map);
						// 只計算防禦性技能
						skills = skills.Where (s => s.Condition == ConfigConditionType.ID_deffence);
						// 取出發動的技能
						var triggered = skills.Where (s => {
							var rate = (int)(Helper.ComputeSkillTriggerRate (player.playerInMap, s) * 100);
							return UnityEngine.Random.Range(1, 101) < rate;
						});
						if (triggered.Count () > 0) {
							var onlyOneSkillCanTrigger = triggered.First ();
							switch (onlyOneSkillCanTrigger.Effect) {
							case "取消對方的攻擊，並對對方造成{1}倍普攻傷害.":
								{
									var counterDamage = playerAbility.Damage (monsterAbility);
									monsterInf.hp -= counterDamage;
									if (monsterInf.IsDied) {
										mapObject.died = true;
									}
									mapObjects [mapObjectId] = mapObject;
									monsterInfo [mapObject.infoKey] = monsterInf;

									des = Description.Empty;
									des.description = Description.InfoUseSkill;
									des.values = new NameValueCollection ();
									des.values.Add ("skills", onlyOneSkillCanTrigger.ID);
									ret.Add (des);
								}
								goto LabelProcessWork;
							case "取消對方的攻擊.":
								{
									des = Description.Empty;
									des.description = Description.InfoUseSkill;
									des.values = new NameValueCollection ();
									des.values.Add ("skills", onlyOneSkillCanTrigger.ID);
									ret.Add (des);
								}
								goto LabelProcessWork;
							}
						}


						var damage = monsterAbility.Damage (playerAbility);
						var criRate = playerAbility.CriticalHitRate (monsterAbility);
						var isCriHit = UnityEngine.Random.Range (1, 101) < (int)(criRate * 100);
						if (isCriHit) {
							damage = (int)(damage*1.5f);
						}
						damage = Math.Max (0, damage);
						player.playerInMap.hp -= damage;
						// 計算仇恨值
						monsterInf.AttackYou (playerAbility, Math.Max (0, damage));
						// 套用
						monsterInfo [mapObject.infoKey] = monsterInf;

						des.description = Description.InfoMonsterAttack;
						des.values = new NameValueCollection ();
						des.values.Set ("mapObjectId", mapObjectId + "");
						des.values.Set ("damage", damage + "");
						des.values.Set ("isCriHit", isCriHit ? "1": "0");
						ret.Add (des);

						if (player.playerInMap.IsDied == false) {
							// 增加防具經驗
							foreach (var item in player.playerInMap.weapons) {
								var cfg = ConfigItem.Get (item.prototype);
								if (cfg.Position == ConfigWeaponPosition.ID_hand) {
									continue;
								}
								if (cfg.SkillType != null) {
									player.playerInMap.AddExp (cfg.SkillType, 1);
								}
							}
						}
					}
				}
				break;
			case Description.EventMonsterEscape:
				{
					var mapObjectId = int.Parse(work.values.Get("mapObjectId"));
					var mapObject = mapObjects [mapObjectId];
					var monsterInf = monsterInfo[mapObject.infoKey];
					monsterInf.hp = 0;
					monsterInfo [mapObject.infoKey] = monsterInf;

					var des = Description.Empty;
					des.description = Description.InfoMonsterEscape;
					des.values = new NameValueCollection ();
					des.values.Set ("mapObjectId", mapObjectId + "");
					ret.Add (des);
				}
				break;
			case Description.EventMonsterIdle:
				{
					var mapObjectId = int.Parse(work.values.Get("mapObjectId"));
					var mapObject = mapObjects [mapObjectId];
					var monsterInf = monsterInfo[mapObject.infoKey];
					monsterInf.hp = 0;
					monsterInfo [mapObject.infoKey] = monsterInf;

					var des = Description.Empty;
					des.description = Description.InfoMonsterIdle;
					des.values = new NameValueCollection ();
					des.values.Set ("mapObjectId", mapObjectId + "");
					ret.Add (des);
				}
				break;
			default:
				throw new NotImplementedException (work.description);
			}
			return ret;
		}

		#region interaction
		// 應用所有互動並回傳結果
		public IEnumerable<Description> ApplyInteraction(PlayerDataStore player, IEnumerable<Interaction> interations){
			var sorted = interations.OrderByDescending (curr => {
				return curr.priority;
			});
			return sorted.SelectMany (work => { return ProcessWork (player, work.description);}).ToList(); // 呼叫ToList強迫墮性序列立即求值
		}

		// 玩家選擇一個工作後呼叫這個將選擇的工作轉為互動
		// 當中會計算這個互動的優先權
		public Interaction MakeInteraction(PlayerDataStore player, Description work){
			var ret = Interaction.Empty;
			ret.description = work;
			ret.priority = player.playerInMap.basicAbility.FightAbility.dodge;
			return ret;
		}

		// 場地與玩家互動列表
		// 玩家每執行一個互動前都要呼叫這個方法
		public IEnumerable<Interaction> GetInteraction(PlayerDataStore player){
			return FindObjects (player.playerInMap.position).Aggregate (
				new List<Interaction> (),
				(actions, currItem) => {
					switch(currItem.type){
					case MapObjectType.Monster:
						{
							var info = monsterInfo[currItem.infoKey];
							// 怪物已死亡，不產生任何動作
							if (info.IsDied) {
								break;
							}
							// 增減勇氣值
							info.StepBrave();
							info.StepHate();
							monsterInfo[currItem.infoKey] = info;

							var cfg = ConfigMonster.Get(info.type);
							var ability = HanRPGAPI.Alg.GetBasicAbility(cfg);
							var fightAbility = ability.FightAbility;
							var priority = fightAbility.dodge;

							var thinking = Helper.MonsterThink(this, player, currItem.key);
							switch(thinking){
							case MonsterThinking.None:
								{
									var action = Description.Empty;
									action.description = Description.EventMonsterIdle;
									action.values = new NameValueCollection();
									action.values.Add("mapObjectId", currItem.key+"");

									var ret = Interaction.Empty;
									ret.description = action;
									ret.priority = priority;
									actions.Add(ret);
								}
								break;
							case MonsterThinking.AttackYou:
								{
									var action = Description.Empty;
									action.description = Description.EventMonsterAttackYou;
									action.values = new NameValueCollection();
									action.values.Add("mapObjectId", currItem.key+"");

									var ret = Interaction.Empty;
									ret.description = action;
									ret.priority = priority;
									actions.Add(ret);
								}
								break;
							case MonsterThinking.Escape:
								{
									var action = Description.Empty;
									action.description = Description.EventMonsterEscape;
									action.values = new NameValueCollection();
									action.values.Add("mapObjectId", currItem.key+"");

									var ret = Interaction.Empty;
									ret.description = action;
									ret.priority = priority;
									actions.Add(ret);
								}
								break;
							}
						}
						break;
					}
					return actions;
				}
			);
		}
		#endregion

		#region works
		public void StartWork(PlayerDataStore player, Description work){
			if (player.playerInMap.IsDied) {
				throw new Exception ("冒險掛點，無法工作");
			}
			if (player.playerInMap.IsWorking) {
				throw new Exception ("目前有工作在身:"+work.description);
			}
			var consumpation = WorkConsumpation (player, work);
			if (player.playerInMap.hp <= consumpation) {
				throw new Exception ("體力不足，無法工作");
			}
			player.playerInMap.currentWork = work;
			player.playerInMap.workFinishedTime = DateTime.Now.Add (TimeSpan.FromSeconds (5)).Ticks;
		}

		public void CancelWork(PlayerDataStore player){
			if (player.playerInMap.IsWorking == false) {
				Debug.LogWarning ("沒有工作，不必取消");
				return;
			}
			player.playerInMap.ClearWork ();
		}

		public IEnumerable<Description> ApplyWork(PlayerDataStore player){
			if (player.playerInMap.IsDied) {
				throw new Exception ("冒險掛點，無法工作");
			}
			if (player.playerInMap.IsWorking == false) {
				throw new Exception ("沒有工作，不能應用");
			}
			player.playerInMap.ClearWork ();
			// 回合性招式
			var turnSkills = 
				Helper.AvailableSkills (player, Place.Map)
					.Where(s=>s.Condition == ConfigConditionType.ID_turn)
					.Select(MakeTurnSkillWork)
					.Select(w=>MakeInteraction(player,w));
			var work = player.playerInMap.currentWork;
			// 將工作轉為互動
			var interaction = MakeInteraction (player, work);
			// 取得場景互動
			var envirInteraction = GetInteraction (player);
			// 匯總所有互動
			var allInter = envirInteraction.Concat (Enumerable.Repeat (interaction, 1)).Concat(turnSkills);
			// 依優先權處理互動
			var ret = ApplyInteraction (player, allInter);
			// 減體力, 這裡可能導致冒險者死亡
			var consumpation = WorkConsumpation (player, work);
			player.AddPlayerHp (this, -consumpation);
			return ret;
		}

		static Description MakeTurnSkillWork(ConfigSkill skill){
			var des = Description.Empty;
			des.description = Description.WorkUseTurnSkill;
			des.values.Set ("skillId", skill.ID);
			return des;
		}

		public IEnumerable<Description> GetWorks(PlayerDataStore player){
			var works = FindObjects (player.playerInMap.position).Aggregate (
				new List<Description> (),
				(actions, currItem) => {
					switch(currItem.type){
					case MapObjectType.Resource:
						{
							var action = Description.Empty;
							action.description = Description.WorkCollectResource;
							action.values = new NameValueCollection();
							action.values.Add("mapObjectId", currItem.key+"");
							actions.Add(action);
						}
						break;
					case MapObjectType.Monster:
						{
							var action = Description.Empty;
							action.description = Description.WorkAttack;
							action.values = new NameValueCollection();
							action.values.Add("mapObjectId", currItem.key+"");
							actions.Add(action);
						}
						break;
					}
					return actions;
				}
			);
			var monsters = FindObjects (player.playerInMap.position).Where (i => i.type == MapObjectType.Monster);
			var hasMonster = monsters.Count () > 0;
			// 這格沒怪物就直接回傳
			if (hasMonster == false) {
				return works;
			}
			// 判斷有沒有單體招式
			var oneEnemySkills = Helper.AvailableSkills (player, Place.Map)
				.Where (s => s.Condition == ConfigConditionType.ID_menu)
				.Where (s => s.Characteristic != null)
				.Where (s => s.Characteristic.Contains (ConfigCharacteristic.ID_enemy));
			var hasOneEnemySkill = oneEnemySkills.Count () > 0;
			if (hasOneEnemySkill) {
				var skillWorks = monsters.Select (m => {
					var monInfo = monsterInfo[m.infoKey];
					var monCfg = ConfigMonster.Get(monInfo.type);
					var canUseSkill = oneEnemySkills.Where(s=>{
						// 由怪物特徵來算，這樣剛好能忽略ID_enemy特徵的比對
						var charItem = HanRPGAPI.Alg.ParseAbstractItem(monCfg.Characteristic);
						// 技能特徵必須包含所有怪物特徵
						foreach(var ci in charItem){
							if(s.Characteristic.Contains(ci.prototype) == false){
								return false;
							}
						}
						return true;
					});
					var d = Description.Empty;
					d.description = Description.WorkSelectSkillForEnemy;
					d.values = new NameValueCollection();
					foreach(var sid in canUseSkill.Select(s=>s.ID)){
						d.values.Add("skillIds", sid);
					}
					d.values.Set("mapObjectId", m.key+"");
					return d;
				});
				works.AddRange (skillWorks);
			}
			// 判斷有沒有全體招式
			var allEnemySkills = Helper.AvailableSkills (player, Place.Map)
				.Where (s => s.Condition == ConfigConditionType.ID_menu)
				.Where (s => s.Characteristic != null)
				.Where (s => s.Characteristic.Contains (ConfigCharacteristic.ID_enemyAll));
			var hasAllEnemySkill = allEnemySkills.Count () > 0;
			if (hasAllEnemySkill) {
				var skillWorks = allEnemySkills.Select (s => {
					var canTarget = monsters.Where (m => {
						var monInfo = monsterInfo [m.infoKey];
						var monCfg = ConfigMonster.Get (monInfo.type);
						// 由怪物特徵來算，這樣剛好能忽略ID_enemy特徵的比對
						var charItem = HanRPGAPI.Alg.ParseAbstractItem (monCfg.Characteristic);
						// 技能特徵必須包含所有怪物特徵
						foreach (var ci in charItem) {
							if (s.Characteristic.Contains (ci.prototype) == false) {
								return false;
							}
						}
						return true;
					});
					var d = Description.Empty;
					d.description = Description.WorkUseSkillForEnemyAll;
					d.values = new NameValueCollection();
					foreach(var m in canTarget){
						d.values.Add("mapObjectIds", m.key+"");
					}
					d.values.Set("skillId", s.ID+"");
					return d;
				});
				works.AddRange (skillWorks);
			}
			return works;
		}
		#endregion


		#region store
		public string GetMemonto(){
			string json = JsonUtility.ToJson(this);
			return json;
		}
		public static MapDataStore FromMemonto(string json){
			return JsonUtility.FromJson<MapDataStore>(json);
		}
		#endregion
	}

	public class PlayerDataStore
	{
		public MapPlayer playerInStorage = new MapPlayer();
		public MapPlayer player = new MapPlayer();
		public MapPlayer playerInMap = new MapPlayer();

		public MapPlayer GetMapPlayer (Place place){
			switch (place) {
			case Place.Storage:
				return playerInStorage;
			case Place.Pocket:
				return player;
			case Place.Map:
				return playerInMap;
			}
			throw new Exception ("xxx:"+place.ToString());
		}

		public void AddPlayerHp(MapDataStore map, int v){
			var basic = BasicAbility.Zero;
			var fight = FightAbility.Zero;
			Helper.CalcAbility (this, map, Place.Map, ref basic, ref fight);
			var maxHp = (int)fight.hp;
			playerInMap.hp += v;
			if (playerInMap.hp >= maxHp) {
				playerInMap.hp = maxHp;
			}
			if (playerInMap.hp < 0) {
				playerInMap.hp = 0;
			}
		}

		#region skill
		public void EquipSkill (Place who, string skillId){
			var cfg = ConfigSkill.Get (skillId);
			if (cfg.SkillTypeRequire != null) {
				var requires = HanRPGAPI.Alg.ParseAbstractItem (cfg.SkillTypeRequire);
				foreach (var r in requires) {
					var skillTypeRequire = r.prototype;
					var levelRequire = r.count;
					var skillExp = GetMapPlayer (who).Exp (skillTypeRequire);
					if (skillExp <= levelRequire) {
						var skillTypeName = ConfigSkillType.Get (skillTypeRequire).Name;
						throw new Exception ("技能需求經驗不足:"+skillTypeName);
					}
				}
			}
			switch (who) {
			case Place.Storage:
			case Place.Pocket:
				player.AddSkill (skillId);
				break;
			case Place.Map:
				playerInMap.AddSkill (skillId);
				break;
			default:
				throw new Exception ("EquipSkill:"+skillId);
			}
		}

		public void UnequipSkill (Place who, string skillId){
			switch (who) {
			case Place.Storage:
			case Place.Pocket:
				player.RemoveSkill (skillId);
				break;
			case Place.Map:
				playerInMap.RemoveSkill (skillId);
				break;
			default:
				throw new Exception ("EquipSkill:"+skillId);
			}
		}
		#endregion

		#region weapon


		public string IsCanEquip(Item item, Place who, Place whosStorage){
			if (who == Place.Storage) {
				throw new Exception ("只能裝備在家裡口袋或出門的冒險者");
			}
			/*
			var cfg = ConfigItem.Get (item.prototype);
			if (cfg.Type != ConfigItemType.ID_weapon) {
				return "只能裝備weapon類型，請檢查程式";
			}
			var weaponPosition = cfg.Position;
			var maxCount = ConfigWeaponPosition.Get (weaponPosition).SlotCount;
			Func<List<Item>, List<Item>, string> canEquip = (weapons, items) => {
				var haveCount = items.Count(i=>{
					return i.Equals(item);
				});
				if(haveCount <1){
					return "沒有那個道具:"+item;
				}
				var alreadyEquipCount = weapons.Count(i=>{
					return ConfigItem.Get (i.prototype).Position == weaponPosition;
				});
				if(alreadyEquipCount >= maxCount){
					return "那個位置已經滿, 最大為"+maxCount+":"+weaponPosition+". 所使用Weapon:"+who;
				}
				return null;
			};*/
			Func<List<Item>, List<Item>, string> canEquip = (weapons, items) => {
				var haveCount = items.Count(i=>{
					return i.Equals(item);
				});
				if(haveCount <1){
					return "沒有那個道具:"+item;
				}
				return HanRPGAPI.Alg.IsCanEquip(weapons, item);
			};
			var useStorage = 
				whosStorage == Place.Pocket ? player.storage :
				whosStorage == Place.Map ? playerInMap.storage :
				playerInStorage.storage;
			var useWeapon = 
				whosStorage == Place.Pocket ? player.weapons :
				playerInMap.weapons;
			return canEquip (useWeapon, useStorage);
		}

		public void EquipWeapon(Item item, Place whosWeapon, Place whosStorage){
			var err = IsCanEquip (item, whosWeapon, whosStorage);
			if (err != null) {
				throw new Exception("無法裝備，請檢查:"+err);
			}
			if (whosStorage == Place.Pocket) {
				player.storage.Remove (item);
			} else if (whosStorage == Place.Map) {
				playerInMap.storage.Remove (item);
			} else {
				playerInStorage.storage.Remove (item);
			}

			if (whosWeapon == Place.Pocket) {
				player.weapons.Add (item);
			} else if (whosWeapon == Place.Map) {
				playerInMap.weapons.Add (item);
			} else {
				throw new Exception ("無法裝備在Place.Home");
			}
		}

		public void UnequipWeapon(Item item, Place whosWeapon, Place whosStorage){
			if (whosWeapon == Place.Pocket) {
				var isCanUnequip = player.weapons.IndexOf (item) != -1;
				if (isCanUnequip == false) {
					throw new Exception ("無法拆掉：沒有那個裝備");
				}
				player.weapons.Remove (item);
			} else if (whosWeapon == Place.Map) {
				var isCanUnequip = playerInMap.weapons.IndexOf (item) != -1;
				if (isCanUnequip == false) {
					throw new Exception ("無法拆掉：沒有那個裝備");
				}
				playerInMap.weapons.Remove (item);
			} else {
				throw new Exception ("無法拆掉裝備在unknow");
			}
			if (whosStorage == Place.Pocket) {
				player.storage.Add (item);
			} else if (whosStorage == Place.Map) {
				playerInMap.storage.Add (item);
			} else {
				playerInStorage.storage.Add (item);
			}
		}
		#endregion

		#region playerInMap
		public void InitPlayerPosition(){
			playerInMap.position.x = 5;
			playerInMap.position.y = 3;
		}
		public MoveResult Move(MapDataStore mapData, Position offset, ref bool mapDirtyFlag, ref bool playerDirtyFlag){
			if (playerInMap.IsDied) {
				throw new Exception ("冒險者掛點，無法移動");
			}
			if (playState != PlayState.Play) {
				throw new Exception ("這時必須是Play狀態，請檢查程式:"+playState.ToString());
			}
			var originPos = playerInMap.position;
			var newPos = originPos.Add (offset);

			var infoList = mapData.FindObjects (newPos)
				.Where (obj => obj.type == MapObjectType.Resource)
				.Select (obj => mapData.resourceInfo [obj.infoKey])
				.Select (info => new AbstractItem () {
					prototype = info.type,
					count = 1
			});;
			var terrianId = HanRPGAPI.Alg.Terrian (infoList);
			var terrian = ConfigTerrian.Get (terrianId);
			if (terrian.MoveRequire != null) {
				var moveRequire = HanRPGAPI.Alg.ParseAbstractItem (terrian.MoveRequire);
				var isMatch = HanRPGAPI.Alg.IsCanFusion (moveRequire, playerInMap.storage.Select(i=>i.AbstractItem));
				if (isMatch <= HanRPGAPI.Alg.REQUIREMENT_NOT_ALLOW) {
					throw new Exception ("缺少必要交通工具，無法移動");
				}
			}

			var moveConsumpation = mapData.MoveConsumption (this, originPos, newPos);
			if (playerInMap.hp - moveConsumpation <= 0) {
				throw new Exception ("體力不足，無法移動:"+playerInMap.hp);
			}
			MoveResult rs = MoveResult.Empty;
			var isPositionDirty = newPos.Equals (playerInMap.position) == false;
			// 移動位置
			playerInMap.position = newPos;
			// 新增視野
			var isMapDirty = VisitPosition (playerInMap.position);
			// 產生事件
			var events = mapData.GenEvent (this, newPos);
			if (isMapDirty) {
				// 生成新地圖
				mapData.GenMapWithPlayerVisible (this);
			}
			if (isPositionDirty) {
				// 增加移動經驗
				playerInMap.AddExp (ConfigAbility.ID_move, 1);
				// 體力減少
				playerInMap.hp -= moveConsumpation;
			}
			// 準備回傳物件
			rs.isMoveSuccess = isPositionDirty;
			rs.events = events.ToList();

			mapDirtyFlag = isMapDirty;
			playerDirtyFlag = isPositionDirty;
			return rs;
		}
		#endregion

		#region storage
		//public List<Item> storage = new List<Item>();
		public void AddItem(Item item, Place who){
			switch (who) {
			case Place.Storage:
				playerInStorage.storage = HanRPGAPI.Alg.AddItem (playerInStorage.storage, item);
				// 從家裡新增的Item才要更新任務
				NotifyMissionAddItem (item);
				break;
			case Place.Pocket:
				player.storage = HanRPGAPI.Alg.AddItem (player.storage, item);
				break;
			case Place.Map:
				playerInMap.storage = HanRPGAPI.Alg.AddItem (playerInMap.storage, item);
				break;
			default:
				throw new Exception (who.ToString ());
			}
		}
		public void MoveItem(Place a, Place b, Item item){
			List<Item> fromStorage;
			switch (a) {
			case Place.Storage:
				fromStorage = playerInStorage.storage;
				break;
			case Place.Pocket:
				fromStorage = player.storage;
				break;
			case Place.Map:
				throw new Exception ("冒險中不能移動道具");
			default:
				throw new Exception ("XXX");
			}
			if (fromStorage.Contains (item) == false) {
				throw new Exception ("沒有這個道具，不能移動:"+item);
			}
			fromStorage.Remove (item);
			switch (b) {
			case Place.Storage:
				playerInStorage.storage = HanRPGAPI.Alg.AddItem (playerInStorage.storage, item);
				break;
			case Place.Pocket:
				player.storage = HanRPGAPI.Alg.AddItem (player.storage, item);
				break;
			case Place.Map:
				throw new Exception ("道具不能直接移動到冒險者");
			default:
				throw new Exception ("XXX");
			}
		}
		#endregion
		#region fusion
		public void Fusion(Item fusionTarget, Place who){
			if (who == Place.Storage) {
				throw new Exception("不能在倉庫里合成");
			}
			if (who == Place.Pocket) {
				player.storage = HanRPGAPI.Alg.Fusion (fusionTarget, player.storage);
				// 從家裡新增的Item才要更新任務
				NotifyMissionAddItem (fusionTarget);
			} else if (who == Place.Map) {
				playerInMap.storage = HanRPGAPI.Alg.Fusion (fusionTarget, playerInMap.storage);
			}
			/*
			Func<List<Item>, List<Item>> fusion = (storage_)=>{
				var requires = Common.Common.ParseItem (ConfigItem.Get (fusionTarget.prototype).FusionRequire);
				var formatForSubstrct = requires.Select (item => {
					item.count = -(item.count*fusionTarget.count);
					return item;
				});
				var tempStorage = Enumerable.Aggregate (formatForSubstrct, storage_, Common.Common.AddItem);

				var fusionItem = fusionTarget;
				tempStorage = Common.Common.AddItem (tempStorage, fusionItem);

				if(who == Place.Storage){
					// 從家裡新增的Item才要更新任務
					NotifyMissionAddItem (fusionItem);
				}
				return tempStorage;
			};
			if (who == Place.Pocket) {
				player.storage = fusion (player.storage);
			} else if (who == Place.Map) {
				playerInMap.storage = fusion (playerInMap.storage);
			}
			*/
		}

		public int IsCanFusion(string prototype, Place who){
			if (who == Place.Pocket) {
				return Common.Common.IsCanFusion (player, prototype, player.storage);
			} else if (who == Place.Map) {
				return Common.Common.IsCanFusion (playerInMap, prototype, playerInMap.storage);
			} else {
				return Common.Common.IsCanFusion (playerInStorage, prototype, playerInStorage.storage);
			}
		}
		#endregion

		#region visible map
		public List<Position> isPositionVisible = new List<Position>();
		public IEnumerable<MapObject> VisibleMapObjects(MapDataStore mapData) {
			var posSet = new HashSet<Position> (isPositionVisible);
			var visiblePosition = mapData.mapObjects.Where (obj => {
				return obj.died == false && posSet.Contains(obj.position);
			});
			return visiblePosition;
		}
		public bool VisitPosition(Position pos){
			var expend = ConfigConst.Get (ConfigConst.ID_visibleRange).Int;
			var posSet = new HashSet<Position> (isPositionVisible);
			for (var x = -expend; x <= expend; ++x) {
				var yexpend = expend - Math.Abs (x);
				for (var y = -yexpend; y <= yexpend; ++y) {
					var newPos = pos.Add(x, y);
					posSet.Add (newPos);
				}
			}
			var newVisiblePosition = posSet.ToList ();
			var oldCnt = isPositionVisible.Count;
			var isDirty = newVisiblePosition.Count != oldCnt;

			isPositionVisible = newVisiblePosition;
			return isDirty;
		}
		public void ClearVisibleMapObjects(){
			isPositionVisible.Clear ();
		}
		#endregion

		#region status
		public int money;
		public int advLevel;
		public PlayState playState;

		public void ClearPlayState(){
			playState = PlayState.Home;
		}
		public void EnterMap(MapDataStore map){
			if (playState != PlayState.Home) {
				throw new Exception ("這時必須是Home狀態，請檢查程式:"+playState.ToString());
			}
			// 轉移道具
			playerInMap.GetData (player);
			// 回滿體力
			AddPlayerHp (map, 99999999);
			playState = PlayState.Play;
		}

		public void ExitMap(){
			player.GetData (playerInMap);
			playState = PlayState.Home;
		}
		#endregion

		#region mission
		// 領取過的任務
		public List<string> missions = new List<string>();
		// 完成的任務
		public List<string> completedMission = new List<string>();
		// 執行中的任務與狀態，完成後要從這個列表移除
		public List<NpcMission> missionStatus = new List<NpcMission>();

		public bool MissionCompleted(string id){
			return completedMission.Contains (id);
		}

		public IEnumerable<string> AvailableNpcMissions {
			get {
				return HanRPGAPI.Alg.AvailableNpcMissions (MissionCompleted, advLevel);
				/*
				return 
					Enumerable.Range (0, ConfigNpcMission.ID_COUNT)
						.Select (ConfigNpcMission.Get)
						.Where (cfg => cfg.Level <= advLevel)
						.Where(cfg=>MissionCompleted(cfg.ID) == false)
						.Where (cfg => {
							if (cfg.Dependency != null) {
								var items = Common.Common.ParseAbstractItem (cfg.Dependency);
								foreach (var item in items) {
									var missionId = item.prototype;
									var isCompleted = MissionCompleted (missionId);
									if (isCompleted == false) {
										return false;
									}
								}
								return true;
							}
							return true;
						})
						.Select (cfg => cfg.ID);
				*/
			}
		}
		/// <summary>
		/// 領取任務
		/// </summary>
		/// <param name="id">Identifier.</param>
		public void AcceptMission(string id){
			var cfg = ConfigNpcMission.Get(id);
			if (cfg.Level > advLevel) {
				throw new Exception ("等級不足");
			}
			if (missions.Exists (i => {
				return i == id;
			})) {
				Debug.LogWarning ("任務已領取:"+id);
				return;
			}
			missions.Add (id);

			var mission = NpcMission.Default;
			mission.prototype = id;
			missionStatus.Add (mission);
		}

		public void NotifyMissionAddItemFromCollect(Item item){
			for (var i = 0; i < missionStatus.Count; ++i) {
				missionStatus [i].itemGot.Add (item);
			}
		}

		public void NotifyMissionAddItem(Item item){
			// ignore
		}

		public void NotifyMissionMonsterKill(string monsterPrototype){
			for (var i = 0; i < missionStatus.Count; ++i) {
				missionStatus [i].monsterSkilled.Add (monsterPrototype);
			}
		}

		public void ClearMissionStatus(){
			for (var i = 0; i < missionStatus.Count; ++i) {
				missionStatus [i].itemGot.Clear ();
				missionStatus [i].monsterSkilled.Clear ();
			}
		}
		/// <summary>
		/// 判斷任務是否完成，每次互動後可以呼叫一次
		/// </summary>
		/// <returns>完成的任務</returns>
		public List<string> CheckMissionStatus(){
			return HanRPGAPI.Alg.CheckMissionStatus (missionStatus);
			/*
			var completedMission = new List<string> ();
			for (var i = 0; i < missionStatus.Count; ++i) {
				var mission = missionStatus [i];
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
						completedMission.Add (mission.prototype);
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
						completedMission.Add (mission.prototype);
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
						completedMission.Add (mission.prototype);
					}
				}
			}
			return completedMission;
			*/
		}
		/// <summary>
		/// 將呼叫CheckMissionStatus取得的任務輸入這個方法，完成那個任務並取得獲得的獎勵資訊
		/// </summary>
		/// <returns>The mission.</returns>
		/// <param name="id">Identifier.</param>
		public IEnumerable<AbstractItem> CompleteMission(string id){
			if (missions.Contains (id) == false) {
				throw new Exception ("這個任務沒有領取:"+id);
			}
			if (completedMission.Contains (id)) {
				throw new Exception ("這個任務已完成過了:"+id);
			}
			var checkAgain = CheckMissionStatus ().Contains (id);
			if (checkAgain == false) {
				throw new Exception ("這個任務還沒達成條件:"+id);
			}
			completedMission.Add (id);
			// 刪去任務狀態
			for (var i = 0; i < missionStatus.Count; ++i) {
				var m = missionStatus [i];
				if (m.prototype == id) {
					missionStatus.RemoveAt (i);
					break;
				}
			}
			var cfg = ConfigNpcMission.Get (id);
			var rewards = HanRPGAPI.Alg.ParseAbstractItem (cfg.Reward);
			foreach (var reward in rewards) {
				switch (reward.prototype) {
				case "money":
					{
						// TODO
					}
					break;
				default:
					{
						AddItem (reward.Item, Place.Pocket);
					}
					break;
				}
			}
			return rewards;
		}

		#endregion

		#region store
		public string GetMemonto(){
			string json = JsonUtility.ToJson(this);
			return json;
		}
		public static PlayerDataStore FromMemonto(string json){
			return JsonUtility.FromJson<PlayerDataStore>(json);
		}
		#endregion
	}

	public class UserSettings{
		public List<string> mark = new List<string>();
		public void Mark(string v){
			if (IsMark (v)) {
				return;
			}
			mark.Add (v);
		}
		public bool IsMark(string v){
			return mark.Contains (v);
		}
		public string GetMemonto(){
			string json = JsonUtility.ToJson(this);
			return json;
		}
		public static UserSettings FromMemonto(string json){
			return JsonUtility.FromJson<UserSettings>(json);
		}
	}

	public class Helper{
		public static Buf GetBuf(ConfigSkill skill){
			switch (skill.ID) {
			case ConfigSkill.ID_bokyoryokuhakai:
				return new Buf(){
					skillId = skill.ID,
					turn = 3
				};
			default:
				throw new Exception ("這招沒buf:"+skill.ID);
			}
		}

		public static MonsterThinking MonsterThink(MapDataStore map, PlayerDataStore player, int monsterId){
			if (true) {
				return MonsterThinking.AttackYou;
			}
			var objInfo = map.mapObjects [monsterId];
			var monsterInfo = map.monsterInfo [objInfo.infoKey];
			//var cfg = ConfigMonster.Get (monsterInfo.type);
			// 想要攻擊主要是仇恨值
			var wantAttack = 
				monsterInfo.NormalHate * 0.8f + 
				monsterInfo.NormalHate > 0 ? (monsterInfo.NormalBrave*0.2f) : 0;
			var wantEscape = 1 - (
				monsterInfo.NormalBrave * (2/3f) +
				monsterInfo.NormalHate * (1/3f)
			);

			var actions = new MonsterThinking[] {
				MonsterThinking.AttackYou, MonsterThinking.Escape
			};
			var values = new float[]{ wantAttack, wantEscape };
			var selectValue = values.OrderByDescending (v => v).First ();
			if (selectValue < 0.2) {
				return MonsterThinking.None;
			}
			for (var i = 0; i < values.Length; ++i) {
				if (selectValue == values [i]) {
					return actions [i];
				}
			}
			throw new Exception ("必須要回傳一個動作");
		}

		/// <summary>
		/// 計算招式發動率
		/// </summary>
		/// <returns>The trigger rate.</returns>
		/// <param name="who">Who.</param>
		/// <param name="skill">Skill.</param>
		public static float ComputeSkillTriggerRate(MapPlayer who, ConfigSkill skill){
			var ais = HanRPGAPI.Alg.ParseAbstractItem (skill.SkillTypeRequire);
			var needExp = ais.Sum (ai => ai.count);
			var maxExp = needExp << 1;
			var haveExp = ais.Sum (ai => who.Exp(ai.prototype));
			var rate = (float)(haveExp - needExp) / (maxExp - needExp);
			var bouns = skill.TriggerBouns;
			return rate + bouns;
		}

		/// <summary>
		/// 取得可用招式列表
		/// </summary>
		/// <returns>The skills.</returns>
		/// <param name="player">Player.</param>
		/// <param name="who">Who.</param>
		public static IEnumerable<ConfigSkill> AvailableSkills(PlayerDataStore player, Place who_){
			var who = player.GetMapPlayer (who_);
			// 先取得欄位上的招式
			var slotSkills = who.skills;
			return HanRPGAPI.Alg.AvailableSkills (slotSkills, who.weapons).Select(ConfigSkill.Get);
			/*
			// 再取得武器本身的招式
			var handWeapons = who.weapons.Select(i=>ConfigItem.Get(i.prototype)).Where(i=>i.Position == ConfigWeaponPosition.ID_hand);
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
				});
			}).Distinct();
			return slotSkills.Concat (weaponSkills);
			*/
		}
		/// <summary>
		/// 計算基礎能力和戰鬥能力
		/// </summary>
		/// <param name="player">Player.</param>
		/// <param name="map">Map.</param>
		/// <param name="who">Who.</param>
		/// <param name="basic">Basic.</param>
		/// <param name="fight">Fight.</param>
		public static void CalcAbility(PlayerDataStore player, MapDataStore map, Place who_, ref BasicAbility basic, ref FightAbility fight){
			if (who_ == Place.Storage) {
				throw new Exception ("計算能力時不能傳入UnknowPlayer");
			}
			var who = player.GetMapPlayer (who_);
			if (who.weapons == null) {
				throw new Exception ("weapon不該為null");
			}
			var tmpFight = FightAbility.Zero;
			var tmpBasic = HanRPGAPI.Alg.CalcAbility (Common.Common.SkillExpFn(who), who.weapons, null, who.basicAbility, ref tmpFight);
			basic = tmpBasic;
			fight = tmpFight;
			/*
			var tmpBasic = BasicAbility.Default;
			var skillbonus = Enumerable.Range (0, ConfigSkillType.ID_COUNT).Select (ConfigSkillType.Get)
				.Select (cfg => cfg.ID).Select (ConfigAbility.Get)
				.Select (cfg => {
					var exp = who.Exp(cfg.ID);
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
			tmpBasic = tmpBasic.Add(skillbonus);
			tmpBasic = tmpBasic.Add(who.basicAbility);
			var effects = who.weapons.SelectMany (it => it.Effects);
			var addEffect = effects.Where (ef => ef.EffectOperator == "+" || ef.EffectOperator == "-");
			var multiEffect = effects.Where (ef => ef.EffectOperator == "*");
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
			basic = tmpBasic;
			fight = tmpFight;
			*/
		}



		public static BasicAbility CalcMonsterAbility(PlayerDataStore player, MapDataStore map, int mapObjectId){
			var mapObject = map.mapObjects [mapObjectId];
			var monsterInfo = map.monsterInfo [mapObject.infoKey];
			return Common.Common.CalcMonsterAbility (monsterInfo);
			/*
			var tmpBasic = monsterInfo.basicAbility;
			var effects = monsterInfo.bufs.SelectMany (it => it.Effects);
			var addEffect = effects.Where (ef => ef.EffectOperator == "+" || ef.EffectOperator == "-");
			var multiEffect = effects.Where (ef => ef.EffectOperator == "*");
			// 先處理基本能力
			// 先加減
			tmpBasic = addEffect.Aggregate (tmpBasic, (accu, curr) => {
				return curr.Effect(accu);
			});
			// 後乘除
			tmpBasic = multiEffect.Aggregate (tmpBasic, (accu, curr) => {
				return curr.Effect(accu);
			});
			return tmpBasic;
			*/
		}
		/// <summary>
		/// 計算普攻傷害
		/// </summary>
		/// <returns>The basic damage.</returns>
		/// <param name="player">Player.</param>
		/// <param name="map">Map.</param>
		/// <param name="mapObjectId">Map object identifier.</param>
		public static int GetBasicDamage(PlayerDataStore player, MapDataStore map, int mapObjectId){
			var a = player.playerInMap.basicAbility.FightAbility;
			var monsterInfo = map.monsterInfo [map.mapObjects [mapObjectId].infoKey];
			var b = Common.Common.GetBasicAbility (monsterInfo).FightAbility;
			return (int)(a.atk - b.def);
		}



	}
}