using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using UnityEngine;
using System.Linq;
using HanRPGAPI;

namespace Common
{
	

	[Serializable]
	public enum MapObjectType{
		Unknown = 0, 
		Resource = 1, 
		Monster = 2
	}

	[Serializable]
	public struct MapObject{
		public int key;
		public string strKey;
		public Position position;
		// info的主鍵為(type, infoKey)
		public MapObjectType type;
		public int infoKey;
		public bool died;
		public static MapObject Empty;
	}

	[Serializable]
	public struct ResourceInfo{
		public string type;
		public static ResourceInfo Empty;
	}

	public enum MonsterThinking{
		None, AttackYou, Escape
	}

	[Serializable]
	public struct Buf{
		public int turn;
		public string skillId;
		public IEnumerable<ItemEffect> Effects {
			get {
				var ret = new List<ItemEffect> ();
				var skill = ConfigSkill.Get (skillId);
				switch (skill.ID) {
				case ConfigSkill.ID_bokyoryokuhakai:
					ret.Add (new ItemEffect () {
						value = "def*0.7"
					});
					break;
				}
				return ret;
			}
		}
	}

	[Serializable]
	public struct MonsterInfo {
		public string type;
		public int hp, mp;
		public BasicAbility basicAbility;
		public List<Buf> bufs;
		public void AddBuf(Buf buf){
			bufs.Add (buf);
		}

		public bool IsDied{ get { return hp <= 0; } }
		/// <summary>
		/// 勇氣值-0.5~0.5
		/// </summary>
		float brave;
		public float NormalBrave {
			get {
				return Mathf.Max (0.5f, Mathf.Min (-0.5f, brave + 0.5f));
			}
		}
		// 打你越痛勇氣值越高
		public void AttackYou(FightAbility you, int damage){
			var maxHp = you.hp;
			var rate = Mathf.Max(0, Mathf.Min(1, damage / maxHp)) - 0.5f;
			brave += rate;
		}
		// 每回合勇氣值會自動增減
		public void StepBrave(){
			var offset = (brave - 0) / 10f;
			brave += offset;
		}
		/// <summary>
		/// 仇恨值0~1
		/// </summary>
		float hate;
		public float NormalHate {
			get {
				return Mathf.Max (0, Mathf.Min (1, hate));
			}
		}
		// 越痛仇恨值越大
		public void BeAttacked(int damage){
			var rate = Mathf.Max(0, Mathf.Min(1, (float)damage / MaxHP));
			hate += rate;
		}
		public void StepHate(){
			hate -= 0.05f;
			if (hate < 0) {
				hate = 0;
			}
		}

		public int MaxHP{
			get{
				return (int)HanRPGAPI.Alg.GetBasicAbility (ConfigMonster.Get (type)).FightAbility.hp;
			}
		}

		public static MonsterInfo Empty;
	}

	[Serializable]
	public enum MapType{
		Unknown, Random, Pattern
	}



	public enum Place{
		Storage, 	// 倉庫
		Pocket,		// 準備中的背包或口袋
		Map			// 地圖中
	}

	/// <summary>
	/// 使用Serializable在struct時，對JsonUtility.FromJson的支援度很像不太完整
	/// 會有其怪的問題，比如
	/// class A{
	/// 	MapPlayer a, b; // MapPlayer is struct
	/// }
	/// 在解析回來後，裡面如果有陣列的話，陣列的值很像會指到同一個
	/// 所以要將MapPlayer改為class
	/// </summary>
	[Serializable]
	public class MapPlayer{
		public Position position;
		public BasicAbility basicAbility;
		public int hp, mp;
		public bool IsDied {
			get {
				return hp <= 0;
			}
		}
		public List<Item> storage;
		// === work === //
 		public Description currentWork;
		public long workFinishedTime;
		public bool IsWorking{
			get{
				return DateTime.Now.Ticks < workFinishedTime;
			}
		}
		public void ClearWork(){
			workFinishedTime = 0;
		}
		// === weapon === //
		public List<Item> weapons;
		/// <summary>
		/// 判斷武器有沒有壞，每次擊中對手時呼叫
		/// </summary>
		/// <returns>壞掉的武器</returns>
		public IEnumerable<Item> CheckHandWeaponBroken(){
			return HanRPGAPI.Alg.CheckHandWeaponBroken (weapons);
			/*
			return weapons
				.Select (i => new Tuple2<Item, ConfigItem> (i, ConfigItem.Get (i.prototype)))
				.Where (info => info.t2.Position == ConfigWeaponPosition.ID_hand)
				.Select (info => new Tuple2<Item, int> (info.t1, (int)((1.0f / info.t2.UseCount) * 100)))
				.Where (info => {
				var dice = UnityEngine.Random.Range (1, 101);
				return dice < info.t2;
			})
				.Select (info => info.t1);
			*/
		}
		/// <summary>
		/// 判斷防具有沒有壞，每次被擊中時呼叫
		/// </summary>
		/// <returns>壞掉的防具</returns>
		public IEnumerable<Item> CheckElseWeaponBroken(){
			return HanRPGAPI.Alg.CheckElseWeaponBroken (weapons);
			/*
			return weapons
				.Select (i => new Tuple2<Item, ConfigItem> (i, ConfigItem.Get (i.prototype)))
				.Where (info => info.t2.Position != ConfigWeaponPosition.ID_hand)
				.Select (info => new Tuple2<Item, int> (info.t1, (int)((1.0f / info.t2.UseCount) * 100)))
				.Where (info => {
					var dice = UnityEngine.Random.Range (1, 101);
					return dice < info.t2;
				})
				.Select (info => info.t1);
				*/
		}
		// === skill exp === //
		public List<AbstractItem> exps;
		public void AddExp(string id, int exp){
			var ai = new Item () {
				prototype = id,
				count = exp
			};
			exps = HanRPGAPI.Alg.AddItemWithFn (
				exps.Select (i => i.Item).ToList(), 
				ai, 
				() => int.MaxValue
			).Select(i=>i.AbstractItem).ToList();
		}
		public int Exp(string skillType){
			var ai = exps.Where (i => i.prototype == skillType).FirstOrDefault ();
			if (ai.Equals (AbstractItem.Empty)) {
				return 0;
			}
			return ai.count;
		}
		public int MaxSkillSlotCount {
			get {
				if (exps == null) {
					Debug.LogWarning ("exps還沒初始化, 回傳0");
					return 0;
				}
				var total = exps.Sum (i => i.count)/5;
				return total;
			}
		}

		// === skill === //
		public List<string> skills;
		public void AddSkill(string id){
			if (skills.Contains (id)) {
				throw new Exception (string.Format("招式已裝備:{0}", id));
			}
			var cfg = ConfigSkill.Get (id);
			var totalCnt = cfg.SlotCount + SkillSlotUsed;
			if (totalCnt > MaxSkillSlotCount) {
				throw new Exception (string.Format("招式欄位不足:{0}/{1}, 所新加招式為{2}", totalCnt, MaxSkillSlotCount, id));
			}
			skills.Add (id);
		}
		public void RemoveSkill(string id){
			if (skills.Contains (id) == false) {
				throw new Exception (string.Format("招式已取消", id));
			}
			skills.Remove (id);
		}

		public int SkillSlotUsed {
			get {
				return skills.Select (ConfigSkill.Get).Select(cfg=>cfg.SlotCount).Sum();
			}
		}

		/// <summary>
		/// 這個方法很重要
		/// 必須要注意那些資料要deep copy
		/// </summary>
		/// <param name="other">Other.</param>
		public void GetData(MapPlayer other){
			basicAbility = other.basicAbility;
			exps = new List<AbstractItem> (other.exps);
			storage = new List<Item> (other.storage);
			weapons = new List<Item> (other.weapons);
			skills = new List<string> (other.skills);
		}
		/*
		public static MapPlayer Empty = new MapPlayer{
			weapons = new List<Item>(), 
			storage = new List<Item>(),
			exps = new List<AbstractItem>(),
			skills = new List<string>()
		};
		*/
		public MapPlayer(){
			weapons = new List<Item> (); 
			storage = new List<Item> ();
			exps = new List<AbstractItem> ();
			skills = new List<string> ();
		}
	}

	[Serializable]
	public struct Description{
		public const string WorkAttack = "[work]attack {mapObjectId}";
		public const string WorkUseTurnSkill = "[work]use turn skill {skillId}";
		public const string WorkSelectSkillForEnemy = "[work]select {skillId} in {skillIds} for {mapObjectId}";
		public const string WorkUseSkillForEnemyAll = "[work]use {skillId} for {mapObjectIds}";
		public const string WorkCollectResource = "[work]collect resource {mapObjectId}";
		public const string EventLucklyFind = "[event]luckly find {itemPrototype} {count}";
		public const string EventMonsterAttackYou = "[event]{mapObjectId} attack you";
		public const string EventMonsterEscape = "[event]{mapObjectId} escape you";
		public const string EventMonsterIdle = "[event]{mapObjectId} idle";
		public const string InfoAttack = "[info]you attack {mapObjectId} and deal damage {damage}. {isCriHit}";
		public const string InfoDodge = "[info]you dodge the attack from {mapObjectId}";
		public const string InfoMonsterDied = "[info]{mapObjectId} is died. you get {rewards}"; // rewards is array of json string
		public const string InfoMonsterDodge = "[info]{mapObjectId} is dodge.";
		public const string InfoMonsterEscape = "[info]{mapObjectId} is escape.";
		public const string InfoMonsterIdle = "[info]{mapObjectId} is idle.";
		public const string InfoMonsterAttack = "[info]{mapObjectId} attack you and deal damage {damage}";
		public const string InfoWeaponBroken = "[info]{items} is broken.";	// items is array of json string
		public const string InfoUseSkill = "[info]you use {skills}.";
		public const string InfoCollectResource = "[info]you collect {items}."; // items is array of json string
		public string description;
		public NameValueCollection values;
		public static Description Empty;
	}

	public struct Interaction{
		public Description description;
		public float priority;
		public static Interaction Empty;
	}

	public struct MoveResult{
		public bool isMoveSuccess;
		public List<Description> events;
		public bool HasEvent{
			get{
				return events != null && events.Count > 0;
			}
		}
		/*public List<string> events;
		public bool HasEvent{
			get{
				return events != null && events.Count > 0;
			}
		}
		public static string BuildEvent(String eventName, NameValueCollection args){
			return string.Format ("?eventName={0}&{1}", eventName, args.ToString ());
		}
		public static string ParseEvent(string eventDescription, NameValueCollection args){
			HanUtil.Native.ParseQueryString (eventDescription, Encoding.UTF8, args);
			var hasEventName = new List<string> (args.AllKeys).Contains("eventName");
			if (hasEventName == false) {
				throw new UnityException ("unknown event");
			}
			var eventName = args.Get ("eventName");
			return eventName;
		}*/
		public static MoveResult Empty;
	}

	public enum Page{
		Unknown, Title, Home, Game
	}

	public enum Info{
		Unknown, 
		Event, 
		Work, 
		WorkResult, 
		Map, 
		Ability, 
		Item, 
		Fusion, 
		Mission, 
		Skill, 
		SelectSkill, 
		Storage, 
		Npc,
		SelectMap
	}

	public enum PlayState{
		Home, Play
	}

	public interface IView {
		IModelGetter ModelGetter{ set; }
		/// <summary>
		/// 切換頁面
		/// callback(null if exception == null else exception)
		/// </summary>
		/// <returns>The page.</returns>
		/// <param name="page">Page.</param>
		/// <param name="callback">Callback.</param>
		IEnumerator ChangePage(Page page, Action<Exception> callback);
		IEnumerator ShowInfo(Info page, Action<Exception> callback);
		IEnumerator ShowInfo(Info page, object args, Action<Exception> callback);
		IEnumerator HideInfo(Info page);
		void Alert (string msg);
		IEnumerator MissionDialog (string mid);
		IEnumerator HandleCommand(string msg, object args, Action<Exception> callback);
	}

	public interface IModelGetter{
		/// <summary>
		/// 地圖狀態
		/// </summary>
		/// <value>The map objects.</value>
		/// <code>
		/// var obj1 = MapObjects[0];
		/// switch(obj1.type){
		/// case MapObjectType.Resource:{
		/// 	var info = ResourceInfos[obj1.infoKey]
		/// 	// your process
		/// 	} break;
		/// case MapObjectType.Monster:{
		/// 	var info = MonsterInfos[obj1.infoKey]
		/// 	// your process
		/// 	} break;
		/// }
		/// </code>
		List<MapObject> MapObjects{ get; }
		List<ResourceInfo> ResourceInfos{ get; }
		List<MonsterInfo> MonsterInfos{ get; }
		/*
		int MapWidth{ get; }
		int MapHeight{ get; }
		*/
		/// <summary>
		/// 取得移動結果
		/// 呼叫任何移動後就必須處理MoveResult，並呼叫ClearMoveResult來清除暫存
		/// </summary>
		/// <value>The move result.</value>
		MoveResult MoveResult{ get; }
		/// <summary>
		/// 取得可視的tile
		/// </summary>
		/// <value>The visible map objects.</value>
		IEnumerable<MapObject> VisibleMapObjects{ get; }
		/// <summary>
		/// 指定位置的物件列表
		/// </summary>
		/// <returns>The <see cref="System.Collections.Generic.IEnumerable`1[[Common.MapObject]]"/>.</returns>
		/// <param name="pos">Position.</param>
		IEnumerable<MapObject> MapObjectsAt (Position pos);
		MapPlayer GetMapPlayer (Place place);
		/// <summary>
		/// 取得玩家所在格的工作列表
		/// </summary>
		/// <value>The player actions.</value>
		IEnumerable<Description> Works{ get; }
		IEnumerable<Description> WorkResults{ get; }

		int IsCanFusion (string prototype, Place who);

		BasicAbility PlayerBasicAbility (Place who);
		FightAbility PlayerFightAbility (Place who);

		IEnumerable<Item> CanFusionItems{ get; }

		IEnumerable<string> AvailableNpcMissions{ get; }
		IEnumerable<string> CheckMissionNotification ();

		IEnumerable<string> AvailableSkills(Place who);

		PlayState PlayState{ get; }
	}

	public interface IModel : IModelGetter{
		void NewGame();
		bool LoadGame();
		/// <summary>
		/// 讀取地圖
		/// 任何一張地圖就是臨時創建的
		/// 同時間只能存在一張地圖
		/// 使用MapObjects取得地圖狀態
		/// </summary>
		/// <returns>The map.</returns>
		/// <param name="type">Type.</param>
		/// <param name="callback">Callback.</param>
		void NewMap(MapType type);
		void EnterMap ();
		void ExitMap ();
		/// <summary>
		/// 向上移動一格
		/// </summary>
		void MoveUp();
		/// <summary>
		/// 向下移動一格
		/// </summary>
		void MoveDown();
		/// <summary>
		/// 向左移動一格
		/// </summary>
		void MoveLeft();
		/// <summary>
		/// 向右移動一格
		/// </summary>
		void MoveRight();
		/// <summary>
		/// 清除移動結果的暫存
		/// </summary>
		void ClearMoveResult();
		void ApplyMoveResult();

		void StartWork (Description work);
		void CancelWork ();
		void ApplyWork();

		void AddItemToStorage(Item item, Place who);
		void MoveItem (Place a, Place b, Item item);

		void Fusion (Item item, Place who);
		void EquipWeapon (Item item, Place whosWeapon, Place whosStorage);
		void UnequipWeapon (Item item, Place whosWeapon, Place whosStorage);
		void ClearStorage (Place who);

		void AcceptMission(string id);
		List<string> CheckMissionStatus();
		IEnumerable<AbstractItem> CompleteMission (string id);
		void ClearMissionStatus();
		void MarkMissionNotification (string mid);

		void EquipSkill (Place who, string skillId);
		void UnequipSkill (Place who, string skillId);
	}

	public class Common
	{
		public static event Action<string, object> OnEvent = delegate{};
        public static void Notify(string cmd, object args)
        {
            OnEvent(cmd, args);
        }

		public static Func<string, int> SkillExpFn(MapPlayer who){
			return skillId => {
				return who.Exp (skillId);
			};
		}
		/// <summary>
		/// Determines if is can fusion the specified prototype items.
		/// </summary>
		/// <returns>
		/// 	<c>more then 1</c></c> if is can fusion the specified prototype items; otherwise, 
		/// 	<c>0</c>可以合成，但道具不夠.
		/// 	</c>-1</c>不能合成
		/// </returns>
		/// <param name="prototype">Prototype.</param>
		/// <param name="items">Items.</param>
		public static int IsCanFusion(MapPlayer who, string prototype, IEnumerable<Item> items){
			return HanRPGAPI.Alg.IsCanFusion (SkillExpFn (who), prototype, items);
			/*
			var cfg = ConfigItem.Get (prototype);
			// 判斷技能經驗是否符合
			var ais = ParseAbstractItem (cfg.SkillRequire);
			foreach (var ai in ais) {
				var st = ai.prototype;
				var needExp = ai.count;
				var haveExp = who.Exp (st);
				if (haveExp < needExp) {
					return -1;
				}
			}
			// 判斷所需道具數量
			var requires = ParseItem (ConfigItem.Get (prototype).FusionRequire);
			int minCnt = int.MaxValue;
			foreach (var requireItem in requires) {
				var search = items.Where (it => {
					return it.prototype == requireItem.prototype && it.count >= requireItem.count;
				});
				var isNotFound = search.Count () == 0;
				if (isNotFound) {
					return -1;
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
		/// <summary>
		/// 平面化地圖物件
		/// </summary>
		/// <param name="model">Model.</param>
		/// <param name="data">Data.</param>
		/*
		MapObject[,] mapObjs;
		var leftTop = model.MapPlayer.position.Add (-5, -5).Max (0, 0);
		var rightBottom = leftTop.Add(10, 10).Min(model.MapWidth, model.MapHeight);
		FlattenMapObjects(model, MapObjectType.Resource, leftTop, rightBottom, out mapObjs);
		for (var x = 0; x < mapObjs.GetLength (1); ++x) {
			for (var y = 0; y < mapObjs.GetLength (2); ++y) {
				var obj = mapObjs[x,y];
				if(obj == MapObject.Empty){
					// 不可視的tile
				}
			}
		}
		*/ 
		public static void FlattenMapObjects(IModelGetter model, MapObjectType type, Position leftTop, Position rightBottom, out MapObject[,] data){
			var w = rightBottom.x - leftTop.x;
			var h = rightBottom.y - leftTop.y;
			data = new MapObject[w, h];
			for (var x = 0; x < w; ++x) {
				for (var y = 0; y < h; ++y) {
					var curr = Position.Zero.Add(x, y).Add(leftTop);
					var sg = model.VisibleMapObjects.Where (obj => {
						return obj.type == type && obj.position.Equals (curr);
					})
						.GroupBy (obj => {
						if (type == MapObjectType.Resource) {
							return model.ResourceInfos [obj.infoKey].type;
						}
						if (type == MapObjectType.Resource) {
							return model.MonsterInfos [obj.infoKey].type;
						}
						return "";
					})
						.OrderByDescending (g => g.Count ())
						.FirstOrDefault ();
					// 沒半個物件所以沒有半個分類
					if (sg == null) {
						continue;
					}
					var first = sg.FirstOrDefault ();
					data [x, y] = first;
				}
			}
		}

		public static void Terrian(IModelGetter model, Position leftTop, Position rightBottom, out string[,] data){
			var w = rightBottom.x - leftTop.x;
			var h = rightBottom.y - leftTop.y;
			data = new string[w, h];
			for (var x = 0; x < w; ++x) {
				for (var y = 0; y < h; ++y) {
					var curr = Position.Zero.Add(x, y).Add(leftTop);
					var infoList = model.VisibleMapObjects.Where (obj => {
						return obj.type == MapObjectType.Resource && obj.position.Equals (curr);
					}).Select (o => {
						var info = model.ResourceInfos [o.infoKey];
						return info;
					}).Select (info => new AbstractItem {
						prototype = info.type,
						count = 1
					});;

					var isNotVisible = infoList.Count () == 0;
					if (isNotVisible) {
						data [x, y] = null;
						continue;
					}

					data [x, y] = HanRPGAPI.Alg.Terrian (infoList);
					// 將地上物轉為虛擬物件，方便計算是否符合地形需求
					/*
					var resList = model.VisibleMapObjects.Where (obj => {
						return obj.type == MapObjectType.Resource && obj.position.Equals (curr);
					}).Select (o => {
						var info = model.ResourceInfos [o.infoKey];
						return new Item () {
							prototype = info.type,
							count = 1
						};
					}).Aggregate (new List<Item> (), (ret, i) => {
						return HanRPGAPI.Alg.AddItemWithFn(ret, i, ()=>{ return 9999; });
					}).Select(i=>i.AbstractItem);

					var isNotVisible = resList.Count () == 0;
					if (isNotVisible) {
						data [x, y] = null;
						continue;
					}

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

					data [x, y] = terrians.First().ID;
					*/
				}
			}
		}

		public static Place PlaceAt(PlayState state){
			switch (state) {
			case PlayState.Home:
				return Place.Pocket;
			case PlayState.Play:
				return Place.Map;
			default:
				throw new Exception ("PlaceAt:"+state);
			}
		}

		public static BasicAbility CalcMonsterAbility(MonsterInfo monsterInfo){
			var tmpBasic = monsterInfo.basicAbility;
			var effects = monsterInfo.bufs.SelectMany (it => it.Effects);
			var fight = FightAbility.Zero;
			return HanRPGAPI.Alg.CalcAbility (null, null, effects, tmpBasic, ref fight);
			/*
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

		public static BasicAbility GetBasicAbility(MonsterInfo info){
			if (string.IsNullOrEmpty (info.type)) {
				throw new Exception ("沒有指定type:"+info.type);
			}
			return HanRPGAPI.Alg.GetBasicAbility (ConfigMonster.Get (info.type));
		}
	}



}

