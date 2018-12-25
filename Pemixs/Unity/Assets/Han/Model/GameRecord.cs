using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using System.Collections;
using System.IO;
using System.Threading;

namespace Remix
{
	public class GameRecord : MonoBehaviour
	{
		#region iap
		// 也可用來當標記
		public List<string> unlockIAP;
		public bool UnlockIAP(string sku){
			if (unlockIAP.Contains (sku)) {
				return false;
			}
			unlockIAP.Add (sku);
			return true;
		}
		public bool IsUnlockIAP(string sku){
			return unlockIAP.Contains (sku);
		}
		public void Memonto2UnlockIAP(JsonData json){
			if (json == null) {
				return;
			}
			if (json.IsArray == false) {
				throw new UnityException ("格式不對");
			}
			unlockIAP.Clear ();
			for (var i = 0; i < json.Count; ++i) {
				var o = (string)json [i];
				unlockIAP.Add (o);
			}
		}
		public void ClearUnlockIAP(){
			unlockIAP.Clear ();
		}
		#endregion

		#region player
		public int gold;
		public int money;
		public long wannaPlayTimeTick;
		public float audioOffsetTime;

		public int Gold{
			get{ return gold; }
			set{
				gold = value;
				if (gold < 0) {
					gold = 0;
				}
			}
		}
		public int Money{
			get{ return money; }
			set{ 
				money = value;
				if (money < 0) {
					money = 0;
				}
			}
		}
		public float AudioOffsetTime{
			get{ return audioOffsetTime; }
			set{ audioOffsetTime = value; }
		}
		public long WannaPlayTimeTick{
			get{ return wannaPlayTimeTick; }
		}
		public void NextWannaPlayTimeTick(){
			wannaPlayTimeTick = DateTime.Now.AddMinutes (GameConfig.WANNA_PLAY_COUNT_TIME).Ticks;
		}
		public void ClearPlayer(){
			gold = 0;
			money = 0;
			wannaPlayTimeTick = 0;
			audioOffsetTime = 0;
		}
		public void ForceSetNormal(){
			var cat = GetCat (GetSelectCat ());
			cat.Status = GameConfig.CAT_STATE_ID.STATE_IDLE;
		}
		public void ForceSetWinnaPlay(){
			wannaPlayTimeTick = DateTime.Now.Ticks;
		}
		public void ForceSetHungry(){
			var cat = GetCat (GetSelectCat ());
			cat.Hp = 50;
			cat.Status = GameConfig.CAT_STATE_ID.STATE_HUNGRY;
		}
		public void ForceSetSleep(){
			var cat = GetCat (GetSelectCat ());
			cat.Status = GameConfig.CAT_STATE_ID.STATE_SLEEP;
			// 定義睡眠結束時間
			DateTime endDateTime = DateTime.Now.AddMinutes (GameConfig.CAT_SLEEP_TIME);
			cat.TimeOfEnd = endDateTime.Ticks;
		}
		#endregion

		#region level
		public const int Easy = 0;
		public const int Normal = 1;
		public const int Hard = 2;

		public List<string> passLevels = new List<string>();
		public void PassLevel(LevelKey key){
			if (passLevels.Contains (key.StringKey)) {
				return;
			}
			passLevels.Add (key.StringKey);
		}
		public IEnumerable<string> PassLevels{ get { return passLevels; } }
		public string ComputeCurrentMap(){
			var currmap = GameConfig.MAP_IDXS [0];
			foreach (var mapIdx in GameConfig.MAP_IDXS) {
				var ls = ComputeCurrentLevel (mapIdx).First();
				if (ls.Idx == 0) {
					break;
				} else {
					currmap = mapIdx;
				}
			}
			return currmap;
		}
		public IEnumerable<LevelKey> ComputeCurrentLevel(string mapIdx){
			// 取得Pass的最後一關
			var maxLevel = -1;
			foreach (var strKey in passLevels) {
				var key = new LevelKey (strKey);
				if (key.MapIdx != mapIdx) {
					continue;
				}
				if (maxLevel < key.Idx) {
					maxLevel = key.Idx;
				}
			}
			// 移到下一關
			maxLevel += 1;
			var ret = new List<LevelKey> ();
			for (var i = 0; i < 3; ++i) {
				var key = new LevelKey{ MapIdx = mapIdx, Idx = maxLevel, Difficulty=i };
				ret.Add (key);
			}
			return ret;
		}
		public void Memonto2PassLevels(JsonData json){
			if (json == null) {
				return;
			}
			if (json.IsArray == false) {
				throw new UnityException ("格式不對");
			}
			passLevels.Clear ();
			for (var i = 0; i < json.Count; ++i) {
				var o = (string)json [i];
				passLevels.Add (o);
			}
		}
		public void ClearPassLevels(){
			passLevels.Clear ();
		}
		#endregion

		#region pass level history
		public class PassLevelInfo : MonoBehaviour{
			public long createTime;
			public string levelKey;
			public int score;
			public int rank;
		}

		public List<PassLevelInfo> passLevelInfos;
		public PassLevelInfo NewPassLevelInfo(LevelKey key){
			var info = new GameObject ().AddComponent<PassLevelInfo> ();
			info.levelKey = key.StringKey;
			info.transform.SetParent (transform);
			info.name = "PassLevelInfo_"+info.levelKey;
			info.createTime = DateTime.Now.Ticks;
			info.rank = -1;
			return info;
		}

		// 本來是將所有歴史資料存下來，但考慮到記憶體用量，所以改成每關只存一筆
		// 這個修改不影響其它相關方法
		public void AddPassLevelInfo(PassLevelInfo info){
			var search = GetPassLevelInfos (new LevelKey (info.levelKey));
			if (search.Count () == 0) {
				// 新增一筆
				info.createTime = DateTime.Now.Ticks;
				passLevelInfos.Add (info);
			} else {
				// 修改原始資料
				var origin = search.First ();
				origin.createTime = DateTime.Now.Ticks;

				if (origin.score < info.score) {
					origin.score = info.score;
				}
				if (info.rank != -1 && origin.rank > info.rank) {
					origin.rank = info.rank;
				}
			}
		}
		public IEnumerable<PassLevelInfo> GetPassLevelInfos(LevelKey key){
			return from info in passLevelInfos
			       where info.levelKey == key.StringKey
			       select info;
		}
		public PassLevelInfo GetMaxScoreLevelInfo(LevelKey key){
			var ordered = 
				from info in GetPassLevelInfos (key)
				orderby info.score descending
				select info;
			if (ordered.Count() == 0) {
				return null;
			}
			return ordered.First ();
		}
		public PassLevelInfo GetMaxRankLevelInfo(LevelKey key){
			var ordered = 
				from info in GetPassLevelInfos (key)
				where info.rank >= 0	// -1不要排進來
				orderby info.rank
				select info;
			if (ordered.Count() == 0) {
				return null;
			}
			return ordered.First ();
		}
		public void Memonto2PassLevelInfos(JsonData json){
			if (json.IsArray == false) {
				throw new UnityException ("格式不對");
			}
			ClearPassLevelInfos ();
			for (var i = 0; i < json.Count; ++i) {
				var jo = json [i];
				var key = (string)jo ["key"];

				var info = NewPassLevelInfo (new LevelKey(key));
				info.score = (int)jo ["score"];
				if (jo ["createTime"] != null) {
					if (jo ["createTime"].IsInt) {
						info.createTime = (int)jo ["createTime"];
					} else if (jo ["createTime"].IsLong) {
						info.createTime = (long)jo ["createTime"];
					}
				}
				if (jo ["rank"] != null) {
					if (jo ["rank"].IsInt) {
						info.rank = (int)jo ["rank"];
					} else if (jo ["rank"].IsLong) {
						info.rank = (int)(long)jo ["rank"];
					}
				}
				AddPassLevelInfo (info);
			}
		}
		public IEnumerable<Dictionary<string,object>> PassLevelInfos2Memonto(){
			return 
				from info in passLevelInfos
				select new Dictionary<string,object> () {
				{"key", info.levelKey},
				{"score", info.score},
				{"createTime", info.createTime},
				{"rank", info.rank},
			};
		}
		public void ClearPassLevelInfos(){
			foreach (var info in passLevelInfos) {
				GameObject.Destroy (info.gameObject);
			}
			passLevelInfos.Clear ();
		}
		#endregion

		#region item
		public List<string> items = new List<string>();
		public bool ConsumeItem(ItemKey key){
			for (var i = items.Count - 1; i >= 0; --i) {
				var strkey = items[i];
				if (strkey == key.StringKey) {
					items.RemoveAt (i);
					return true;
				}
			}
			return false;
		}

		public void AddItem(ItemKey key, int count){
			for (var i = 0; i < count; ++i) {
				items.Add (key.StringKey);
			}
		}

		public int QueryItemCount(ItemKey key){
			return items.Count (strkey => {
				return strkey == key.StringKey;
			});
		}

		public void Memonto2Items(JsonData json){
			if (json == null) {
				return;
			}
			if (json.IsArray == false) {
				throw new UnityException ("格式不對");
			}
			items.Clear ();
			for (var i = 0; i < json.Count; ++i) {
				var o = (string)json [i];
				items.Add (o);
			}
		}

		public void ClearItems(){
			items.Clear ();
		}
		#endregion

		#region photo
		public List<string> enablePhotos = new List<string>();
		public void EnablePhoto(PhotoKey key){
			if (enablePhotos.Contains (key.StringKey)) {
				// Util.Instance.LogError ("開啟到重複的照片，這不可能，請檢查程式:"+key.StringKey);
				return;
			}
			enablePhotos.Add (key.StringKey);
		}
		public IEnumerable<PhotoKey> GetEnablePhotoKey(){
			return 
				from s in enablePhotos
				select new PhotoKey(s);
		}
		public bool IsPhotoEnable(PhotoKey key){
			if (enablePhotos.Contains (key.StringKey)) {
				return true;
			}
			return false;
		}
		public void Memonto2Photos(JsonData json){
			if (json == null) {
				return;
			}
			if (json.IsArray == false) {
				throw new UnityException ("格式不對");
			}
			enablePhotos.Clear ();
			for (var i = 0; i < json.Count; ++i) {
				var o = (string)json [i];
				enablePhotos.Add (o);
			}
		}

		public void ClearPhotos(){
			enablePhotos.Clear ();
		}
		#endregion

		#region capture enable
		public List<string> captureEnables = new List<string> ();
		public IEnumerable<string> EnableCaptures{
			get{
				return captureEnables;
			}
		}
		public void EnableCapture(CaptureKey key){
			foreach (var c in captureEnables) {
				if (c == key.StringKey){
					return;
				}
			}
			captureEnables.Add (key.StringKey);
		}
		public bool IsCaptureEnable(CaptureKey key){
			return captureEnables.Contains (key.StringKey);
		}
		public void Memonto2CaptureEnable(JsonData json){
			if (json == null) {
				return;
			}
			if (json.IsArray == false) {
				throw new UnityException ("格式不對");
			}
			ClearCaptureEnable ();
			for (var i = 0; i < json.Count; ++i) {
				var o = (string)json [i];
				captureEnables.Add (o);
			}
		}
		public void ClearCaptureEnable(){
			captureEnables.Clear ();
		}
		#endregion

		#region capture
		public List<Capture> captures = new List<Capture>();
		public Capture GetCapture(CaptureKey key){
			if (IsCaptureEnable (key) == false) {
				throw new UnityException ("還沒解鎖，程式有誤，請檢查:"+key.StringKey);
			}
			foreach (var c in captures) {
				if (c.Key == key.StringKey){
					return c;
				}
			}
			var ret = new GameObject ().AddComponent<Capture> ();
			ret.gameObject.transform.SetParent (this.transform);
			ret.name = key.StringKey;
			ret.State = GameConfig.CAPTURE_STATE.NORMAL;
			ret.Key = key.StringKey;
			captures.Add (ret);
			return ret;
		}
		public IEnumerable<CaptureKey> GetCaptures(){
			return from c in captures select new CaptureKey(c.Key);
		}
		public bool CheckCaptureCompleted(){
			var isDirty = false;
			foreach (var ck in GetCaptures ()) {
				var c = GetCapture (ck);
				if (c.State != GameConfig.CAPTURE_STATE.CAPTURE) {
					continue;
				}
				if (DateTime.Now.Ticks >= c.TimeOfEndTick) {
					isDirty |= CompletedCapture (ck);
				}
			}
			return isDirty;
		}
		// 在指定的點(idx)開始探索
		// 記錄倒數計時多久(countDown)和所使用的道具
		public void StartCapture (CaptureKey key, ItemKey item)
		{
			Capture cpData = GetCapture (key);
			if (cpData.State != GameConfig.CAPTURE_STATE.NORMAL) {
				throw new UnityException ("現在不能探索:"+key.StringKey);
			}

			var itemDef = ItemDef.Get (item.Idx);
			switch (itemDef.ID) {
			// 相機類
			case "I20010":
			case "I20020":
			case "I20030":
			case "I20040":
			case "I20050":
			case "I20060":
			case "I20070":
			case "I20080":
				break;
			// 抓貓類
			case "I32010":
			case "I32020":
			case "I32030":
				{
					if (key.MapIdx == "S1") {
						throw new UnityException ("一般關卡只能使用一般道具");
					}
				}
				break;
			case "I62010":
				{
					if (key.MapIdx != "S1") {
						throw new UnityException ("Boss道具只能在Boss關卡中用");
					}
				}
				break;
			default:
				throw new UnityException ("用錯道具!:"+itemDef.ID);
			}

			cpData.ItemKey = item.StringKey;
			var gcDef = GachaDef.Get (key.GachaConfigID);

			var countDownMins = 0;
			switch (item.Type) {
			case StoreCtrl.DATA_CAMERA:
				countDownMins = gcDef.PhotoTime;
				break;
			case StoreCtrl.DATA_TOY_CAPTURE:
				countDownMins = gcDef.CatTime;
				break;
			}
			// 拍立得相機與運動相機會減時間(%)
			var hasReduceTimeAbility = itemDef.Time > 0;
			if (hasReduceTimeAbility) {
				countDownMins = (int)(countDownMins * (1-itemDef.Time));
			}
			cpData.TimeOfEndTick = DateTime.Now.AddMinutes (countDownMins).Ticks;
			cpData.State = GameConfig.CAPTURE_STATE.CAPTURE;
		}

		// 取得當時使用的道具類型
		public static GameConfig.ITEM_TYPE GetCaptureType (ItemKey item)
		{
			switch (item.Type) {
			case StoreCtrl.DATA_CAMERA:
			case StoreCtrl.DATA_TOY:
				return GameConfig.ITEM_TYPE.ITEM_BASE;
			case StoreCtrl.DATA_S_CAMERA:
				return GameConfig.ITEM_TYPE.ITEM_EXPLORE;
			case StoreCtrl.DATA_S_TOY:
				return GameConfig.ITEM_TYPE.ITEM_CATCH;
			case StoreCtrl.DATA_FOOD:
				return GameConfig.ITEM_TYPE.ITEM_FOOD;
			default:
				throw new UnityException ("這種Type不能用來探索:"+item.Type);
			}
		}

		public bool SpeedUpCaptureWithItem(CaptureKey key, ItemKey item){
			var cap = GetCapture (key);
			var captureItem = new ItemKey (cap.ItemKey);
			if (captureItem.Type == StoreCtrl.DATA_CAMERA) {
				if (item.Type != StoreCtrl.DATA_S_CAMERA) {
					Debug.LogError ("無法使用這個道具，請檢查程式"+item.StringKey);
					return false;
				}
			}
			if (captureItem.Type == StoreCtrl.DATA_TOY_CAPTURE) {
				if (item.Type != StoreCtrl.DATA_S_TOY) {
					Debug.LogError ("無法使用這個道具，請檢查程式:"+item.StringKey);
					return false;
				}
			}
			var itemDef = ItemDef.Get (item.Idx);
			switch (itemDef.ID) {
			case "I21010":
			case "I21020":
			case "I31010":
			case "I31020":
				break;
			default:
				Debug.LogError ("無法使用這個道具，請檢查程式"+itemDef.ID);
				return false;
			}
			var gcDef = GachaDef.Get (key.GachaConfigID);
			var reduceMins = 0;
			// 貓草噴劑
			// 罐裝乾貓草
			if (itemDef.ID == "I31010" || itemDef.ID == "I31020" ) {
				reduceMins = -(int)(gcDef.CatTime * itemDef.Time);
			}
			// 自拍棒
			// 相機腳架
			if (itemDef.ID == "I21010" || itemDef.ID == "I21020") {
				reduceMins = -(int)(gcDef.PhotoTime * itemDef.Time);
			}
			if (reduceMins == 0) {
				Debug.LogError ("沒有定義這個道具的效果:"+itemDef.ID);
				return false;
			}
			cap.TimeOfEndTick = new DateTime (cap.TimeOfEndTick).AddMinutes (reduceMins).Ticks;
			return true;
		}

		public bool CompletedCapture(CaptureKey key){
			var cap = GetCapture (key);
			if (cap.State != GameConfig.CAPTURE_STATE.CAPTURE) {
				throw new UnityException ("你還沒呼叫StartCapture:"+key.StringKey);
			}
			if (cap.State == GameConfig.CAPTURE_STATE.COMPLETED) {
				Debug.LogWarning ("已準備好給予道具，等待取得");
				return false;
			}
			var useItem = new ItemKey (cap.ItemKey);
			if (useItem.Type == StoreCtrl.DATA_CAMERA) {

				var itemDef = ItemDef.Get (useItem.Idx);

				var hasGetBigPhotoAiblity = itemDef.BPhoto > 0;
				var hasGetTwoPhotoAiblity = itemDef.ToPhoto > 0;
				PhotoKey onePhoto = null;
				PhotoKey bigPhoto = null;
				PhotoKey twoPhoto = null;
				if (hasGetBigPhotoAiblity) {
					// 環景圖
					var win = UnityEngine.Random.Range (0, 100) <= itemDef.BPhoto * 100;
					if (win) {
						var bigPhotos = 
							from photo in PhotoKey.Keys (key.MapIdx, PhotoKey.TypeBigPhoto)
							where IsPhotoEnable (photo) == false
							select photo;
						var bigPhotosCount = bigPhotos.Count ();
						if (bigPhotosCount == 0) {
							Util.Instance.LogWarning ("環景照已取得所有");
							// ignore
						} else {
							bigPhoto = bigPhotos.Skip (UnityEngine.Random.Range (0, bigPhotosCount)).First ();
						}
					}
				}
				var smallPhotos = 
					from photo in PhotoKey.Keys (key.MapIdx, PhotoKey.TypeSmallPhoto)
						where IsPhotoEnable (photo) == false
					select photo;
				var smallPhotosCount = smallPhotos.Count ();
				if (smallPhotosCount == 0) {
					var allSmallPhotos = PhotoKey.Keys (key.MapIdx, PhotoKey.TypeSmallPhoto);
					onePhoto = allSmallPhotos.Skip (UnityEngine.Random.Range (0, allSmallPhotos.Count())).First();
					Util.Instance.LogWarning ("一般照已取得所有，隨便給予一張");
				} else if (smallPhotosCount == 1) {
					onePhoto = smallPhotos.First ();
				} else if (smallPhotosCount == 2) {
					var list = smallPhotos.ToList ();
					onePhoto = list [0];
					if (hasGetTwoPhotoAiblity) {
						var win = UnityEngine.Random.Range (0, 100) <= itemDef.ToPhoto * 100;
						if (win) {
							twoPhoto = list [1];
						}
					}
				} else {
					if (hasGetTwoPhotoAiblity) {
						// skip到長度-1的位置，-1是預留第2張
						var cutInOnePhoto = smallPhotos.Skip (UnityEngine.Random.Range (0, smallPhotosCount - 1));
						onePhoto = cutInOnePhoto.First ();
						// skip掉抽出的
						cutInOnePhoto = cutInOnePhoto.Skip (1);
						var win = UnityEngine.Random.Range (0, 100) <= itemDef.ToPhoto * 100;
						if (win) {
							var cutInTwoPhoto = cutInOnePhoto.Skip (UnityEngine.Random.Range (0, cutInOnePhoto.Count ()));
							twoPhoto = cutInTwoPhoto.First ();
						}
					} else {
						onePhoto = smallPhotos.Skip (UnityEngine.Random.Range (0, smallPhotosCount)).First();
					}
				}
				var photoList = new List<string> ();
				if(onePhoto != null){
					photoList.Add (onePhoto.StringKey);
				}
				if (bigPhoto != null) {
					photoList.Add (bigPhoto.StringKey);
				}
				if (twoPhoto != null) {
					photoList.Add (twoPhoto.StringKey);
				}
				cap.GettedItem = string.Join(",", photoList.ToArray());
			} else if (useItem.Type == StoreCtrl.DATA_TOY_CAPTURE) {
				var cat = SelectCaptureCat (key, useItem);
				cap.GettedItem = cat.StringKey;
			} else {
				throw new UnityException ("你使用了錯誤的道具。請檢查程式");
			}
			cap.State = GameConfig.CAPTURE_STATE.COMPLETED;
			return true;
		}

		static string GetCachaPmID(ItemKey item){
			var itemDef = ItemDef.Get (item.Idx);
			// 宅配紙箱
			// 取得檔案(gachapm01~04)中Type為CA的資料列，都剛好是Gacha01
			if (itemDef.ID == "I32010") {
				return "Gacha01";
			}
			// 掃地機器人
			// 取得Type為CB的資料列
			// 取得檔案(gachapm01~04)中Type為CB的資料列，都剛好是Gacha02
			if (itemDef.ID == "I32020") {
				return "Gacha02";
			}
			// 沙發床
			// 取得Type為CC的資料列
			// 取得檔案(gachapm01~04)中Type為CC的資料列，都剛好是Gacha03
			if (itemDef.ID == "I32030") {
				return "Gacha03";
			}
			// 雪橇
			// 取得Type為CM的資料列
			// 取得檔案(gachapmS1)中Type為CM的資料列，是Gacha01
			if (itemDef.ID == "I62010") {
				return "Gacha01";
			}
			throw new Exception ("錯誤的道具:"+itemDef.ID);
		}

		static ItemKey SelectCaptureCat(CaptureKey key, ItemKey item){
			var def = GachaDef.Get (key.GachaConfigID);
			var pmId = GetCachaPmID (item);
			var cat1 = "";
			var cat2 = "";
			var cat1P = 0;
			var cat2P = 0;
			// 只有gachapmS1會有cat3，但計算流桯和其它的一樣，因為cat3P為0的時候，不影響計算
			var cat3 = "";
			var cat3P = 0;
			switch (def.CatFile) {
			case "gachapm01":
				{
					var pmDef = GachaPm01.Get (pmId);
					cat1 = pmDef.Cat01;
					cat2 = pmDef.Cat02;
					cat1P = pmDef.Cat01Weights;
					cat2P = pmDef.Cat02Weights;
				}
				break;
			case "gachapm02":
				{
					var pmDef = GachaPm02.Get (pmId);
					cat1 = pmDef.Cat01;
					cat2 = pmDef.Cat02;
					cat1P = pmDef.Cat01Weights;
					cat2P = pmDef.Cat02Weights;
				}
				break;
			case "gachapm03":
				{
					var pmDef = GachaPm03.Get (pmId);
					cat1 = pmDef.Cat01;
					cat2 = pmDef.Cat02;
					cat1P = pmDef.Cat01Weights;
					cat2P = pmDef.Cat02Weights;
				}
				break;
			case "gachapm04":
				{
					var pmDef = GachaPm04.Get (pmId);
					cat1 = pmDef.Cat01;
					cat2 = pmDef.Cat02;
					cat1P = pmDef.Cat01Weights;
					cat2P = pmDef.Cat02Weights;
				}
				break;
			case "gachapmS1":
				{
					var pmDef = GachaPmS1.Get (pmId);
					cat1 = pmDef.Cat01;
					cat2 = pmDef.Cat02;
					cat3 = pmDef.Cat03;
					cat1P = pmDef.Cat01Weights;
					cat2P = pmDef.Cat02Weights;
					cat3P = pmDef.Cat03Weights;
				}
				break;
			default:
				throw new UnityException ("沒支援到的檔案:"+def.CatFile+", 所使用的Capture:"+key.GachaConfigID);
			}
			var total = cat1P + cat2P + cat3P;
			if (total == 0) {
				var useItemDef = ItemDef.Get(item.Idx);
				throw new UnityException ("檔案"+def.CatFile+"的點"+pmId+"的權重合計不能為0，不然無法生成貓。所使用的道具:"+useItemDef.Name);
			}
			var inputs = new List<object[]> (){ 
				new object[]{cat1, cat1P}, 
				new object[]{cat2, cat2P},
				new object[]{cat3, cat3P},
			};
			var selectCat = inputs.SelectMany (info => {
				var cat = (string)info [0];
				var p = (int)info [1];
				return Enumerable.Repeat (cat, p);
			}).Skip (UnityEngine.Random.Range (0, total)).First ();
			return ItemKey.WithCatConfigID (selectCat);
		}

		// 完成探索
		// 時間到或許強制完成時都可以呼叫它
		public void ClearCapture (CaptureKey key)
		{
			Capture cpData = GetCapture (key);
			cpData.TimeOfEndTick = DateTime.Now.Ticks;
			// 使用道具設為沒有代表沒有探索
			cpData.ItemKey = null;
			cpData.GettedItem = null;
			cpData.State = GameConfig.CAPTURE_STATE.NORMAL;
		}

		// 取得當時使用的道具名稱
		public string GetCaptureItemKey (CaptureKey key)
		{
			Capture cpData = GetCapture(key);
			if (cpData.ItemKey == null) {
				throw new UnityException ("沒有安置道具，程式有誤。");
			}
			return cpData.ItemKey;
		}
		public IEnumerable<Dictionary<string,object>> Captures2Memonto(){
			return 
				from cap in captures
				select new Dictionary<string,object> () {
				{"key", cap.Key},
				{"itemKey", cap.ItemKey},
				{"timeOfEndTick", cap.TimeOfEndTick},
				{"gettedItem", cap.GettedItem},
				{"state", System.Convert.ToInt32(cap.State)},
			};
		}
		public void Memonto2Captures(JsonData json){
			if (json == null) {
				return;
			}
			if (json.IsArray == false) {
				throw new UnityException ("Capture格式不對");
			}
			ClearCaptures ();
			for (var i = 0; i < json.Count; ++i) {
				var o = json [i];
				var cap = new GameObject ().AddComponent<Capture> ();
				cap.gameObject.transform.SetParent (this.transform);
				cap.Key = (string)o ["key"];
				if (o ["itemKey"] != null) {
					cap.ItemKey = (string)o ["itemKey"];
				}
				if (o ["timeOfEndTick"].IsInt) {
					cap.TimeOfEndTick = (int)o ["timeOfEndTick"];
				} else if(o ["timeOfEndTick"].IsLong){
					cap.TimeOfEndTick = (long)o ["timeOfEndTick"];
				}
				if (o ["gettedItem"] != null) {
					cap.GettedItem = (string)o ["gettedItem"];
				}
				cap.State = (GameConfig.CAPTURE_STATE)(int)o ["state"];
				cap.name = cap.Key;
				captures.Add (cap);
			}
		}

		public void ClearCaptures(){
			foreach (var c in captures) {
				GameObject.Destroy (c.gameObject);
			}
			captures.Clear ();
		}
		#endregion

		#region cat
		public string selectCat = null;
		public List<Cat> cats = new List<Cat> ();
		Cat CreateCat(ItemKey key){
			if (key.Type != StoreCtrl.DATA_CAT) {
				throw new UnityException ("不是貓的主鍵");
			}
			foreach (var c in cats) {
				if (c.Key == key.StringKey) {
					return c;
				}
			}
			var cat = new GameObject ().AddComponent<Cat> ();
			cat.gameObject.transform.SetParent (this.transform);
			cat.name = key.StringKey;
			cat.Key = key.StringKey;
			cats.Add (cat);
			return cat;
		}
		public Cat GetCat(ItemKey key){
			if (key.Type != StoreCtrl.DATA_CAT) {
				throw new UnityException ("不是貓的主鍵");
			}
			foreach (var c in cats) {
				if (c.Key == key.StringKey) {
					return c;
				}
			}
			throw new UnityException ("還沒擁有這隻貓:"+key.StringKey);
		}
		public bool HasCat(ItemKey key){
			foreach (var c in cats) {
				if (c.Key == key.StringKey) {
					return true;
				}
			}
			return false;
		}
		public void SetCat(ItemKey key){
			if (key.Type != StoreCtrl.DATA_CAT) {
				throw new UnityException ("不是貓的主鍵");
			}
			selectCat = key.StringKey;
		}
		public ItemKey GetSelectCat(){
			if (selectCat.Equals(ItemKey.Empty)) {
				throw new UnityException ("你還沒選一隻貓");
			}
			return new ItemKey(selectCat);
		}
		public void SetSelectCat(ItemKey key){
			selectCat = key.StringKey;
		}
		public IEnumerable<ItemKey> GetOwnedCats(){
			return 
				from c in cats
				select new ItemKey(c.Key);
		}
		// 開啟新貓
		// 若那隻貓早已開啟，則增加經驗值
		public bool EnableCat (ItemKey key){
			// 先檢查有沒有開啟
			if (HasCat (key)) {
				Debug.LogWarning ("此貓已經擁有");
				return false;
			}
			// 開啟新貓
			CreateCat(key);
			return true;
		}
		public bool AddCatExp(Cat cat, int exp){
			var maxExpRef = 0;
			var maxHpRef = 0;
			var maxLvRef = 0;

			GameRecord.GetCatLvInfo (new ItemKey(cat.Key), cat.Lv, ref maxExpRef, ref maxHpRef, ref maxLvRef);

			// max lv
			if (cat.Lv >= maxLvRef && cat.Exp >= maxExpRef) {
				Util.Instance.LogWarning ("已封頂:"+cat.Lv);
				return false;
			}

			// 切掉超過的經驗值
			cat.Exp += exp;
			if (cat.Lv >= maxLvRef) {
				if (cat.Exp >= maxExpRef) {
					cat.Exp = maxExpRef;
				}
			}

			var isLevelUp = UpdateCatLv (cat);
			if (isLevelUp) {
				GameRecord.GetCatLvInfo (new ItemKey(cat.Key), cat.Lv, ref maxExpRef, ref maxHpRef, ref maxLvRef);
				cat.MaxHp = maxHpRef;
				cat.Hp = maxHpRef;
			}
			return isLevelUp;
		}

		// 減少貓的生氣時間
		public bool ReduceCatTimeOfEnd (ItemKey key, int reduceTime){
			Cat catData = GetCat (key);
			if (catData.Status != GameConfig.CAT_STATE_ID.STATE_ANGRY) {
				throw new UnityException ("非生氣的貓不必減少生氣時間。現在狀態是" + catData.Status);
			}
			DateTime endDateTime = new DateTime (catData.TimeOfEnd);
			endDateTime = endDateTime.AddMinutes (-reduceTime);
			catData.TimeOfEnd = endDateTime.Ticks;
			return endDateTime.Ticks <= DateTime.Now.Ticks;
		}
		public bool IsCanEnterGame (ItemKey key)
		{
			var selectCatData = GetCat (key);
			switch (selectCatData.Status) {
			case GameConfig.CAT_STATE_ID.STATE_IDLE:
			case GameConfig.CAT_STATE_ID.STATE_ANGRY:
			case GameConfig.CAT_STATE_ID.STATE_WANNA_PLAY:
			case GameConfig.CAT_STATE_ID.STATE_HUNGRY:
				if (selectCatData.Hp <= 0) {
					Debug.LogWarning("沒血了，無法遊玩");
					break;
				}
				return true;
			default:
				Debug.LogWarning("睡眠中無法遊玩");
				break;
			}
			return false;
		}
		// 貓要進入遊戲
		// 進入遊戲前呼叫
		// 注意：睡眠狀態的貓無法進入遊戲
		public void CatEnterGame(ItemKey key){
			var selectCatData = GetCat (key);
			switch (selectCatData.Status) {
			case GameConfig.CAT_STATE_ID.STATE_IDLE:
			case GameConfig.CAT_STATE_ID.STATE_ANGRY:
			case GameConfig.CAT_STATE_ID.STATE_WANNA_PLAY:
			case GameConfig.CAT_STATE_ID.STATE_HUNGRY:
				break;
			default:
				throw new UnityException ("睡眠中無法遊玩");
			}
			// TODO 如果是飢餓狀態，血量應該會變低
		}
		// 貓結束遊戲
		// 結束遊戲時呼叫
		public void CatEndGame (ItemKey catId){
			var selectCatData = GetCat (catId);
			switch (selectCatData.Status) {
			case GameConfig.CAT_STATE_ID.STATE_IDLE:
			case GameConfig.CAT_STATE_ID.STATE_ANGRY:
			case GameConfig.CAT_STATE_ID.STATE_WANNA_PLAY:
			case GameConfig.CAT_STATE_ID.STATE_HUNGRY:
				break;
			default:
				throw new UnityException ("非特定狀態是無法玩遊戲而呼叫EndGame，請檢查程式");
			}
			NextWannaPlayTimeTick();
			// 新增遊玩次數
			selectCatData.BattleCount++;

			switch (selectCatData.Status) {
			case GameConfig.CAT_STATE_ID.STATE_IDLE:
			case GameConfig.CAT_STATE_ID.STATE_ANGRY:
				{
					// 遊戲中因為有扣過血量，所以在遊戲結束後判斷有沒有到飢餓狀態
					// 血量低於80%就變成飢餓狀態
					// 飢餓狀態不會進入睡眠
					int hpThreshold = (int)(selectCatData.MaxHp * GameConfig.HUNGRY_THRESHOLD / 100.0f);
					if (selectCatData.Hp < hpThreshold) {
						selectCatData.Status = GameConfig.CAT_STATE_ID.STATE_HUNGRY;
					} else {
						// 使用同一隻貓遊玩超過極限值就會進入睡眠狀態
						if (selectCatData.BattleCount >= GameConfig.BATTLE_COUNT_TO_SLEEP) {
							selectCatData.BattleCount = 0;
							// 切換到睡眠狀態
							selectCatData.Status = GameConfig.CAT_STATE_ID.STATE_SLEEP;
							// 定義睡眠結束時間
							DateTime endDateTime = DateTime.Now.AddMinutes (GameConfig.CAT_SLEEP_TIME);
							selectCatData.TimeOfEnd = endDateTime.Ticks;
						}
					}
				}
				break;
			case GameConfig.CAT_STATE_ID.STATE_WANNA_PLAY:
				{
					// 遊戲中因為有扣過血量，所以在遊戲結束後判斷有沒有到飢餓狀態
					// 血量低於80%就變成飢餓狀態
					int hpThreshold = (int)(selectCatData.MaxHp * GameConfig.HUNGRY_THRESHOLD / 100.0f);
					if (selectCatData.Hp < hpThreshold) {
						selectCatData.Status = GameConfig.CAT_STATE_ID.STATE_HUNGRY;
					} else {
						// 單純切換為普通狀態
						selectCatData.Status = GameConfig.CAT_STATE_ID.STATE_IDLE;
					}
				}
				break;
			case GameConfig.CAT_STATE_ID.STATE_HUNGRY:
				{
					// 飢餓狀態不會變成睡眠狀態
					if (selectCatData.BattleCount >= GameConfig.BATTLE_COUNT_TO_SLEEP) {
						Debug.LogWarning ("飢餓狀態不會變成睡眠狀態");
					}
					// 如果在遊戲中變滿血，就回復到正常狀態
					if (selectCatData.Hp >= selectCatData.MaxHp) {
						// 單純切換為普通狀態
						selectCatData.Status = GameConfig.CAT_STATE_ID.STATE_IDLE;
					}
				}
				break;
			}
		}

		public bool IsCanTouch(ItemKey catId){
			Cat catData = GetCat (catId);
			GameConfig.CAT_STATE_ID stateId = catData.Status;
			switch (stateId) {
			case GameConfig.CAT_STATE_ID.STATE_IDLE:
			case GameConfig.CAT_STATE_ID.STATE_SLEEP:
				return true;
			default:
				return false;
			}
		}

		void MakeCatAngry(Cat catData){
			catData.TouchCount = 0;
			catData.Status = GameConfig.CAT_STATE_ID.STATE_ANGRY;
			// 切換到生氣狀態
			// 定義生氣結束時間，這段時間內無法摸貓
			DateTime endDateTime = DateTime.Now.AddMinutes (GameConfig.CAT_ANGRY_TIME);
			catData.TimeOfEnd = endDateTime.Ticks;
		}

		public AnimationResult SetTouchCat (ItemKey catId)
		{
			var result = new AnimationResult();
			Cat catData = GetCat (catId);
			GameConfig.CAT_STATE_ID stateId = catData.Status;
			switch (stateId) {
			case GameConfig.CAT_STATE_ID.STATE_IDLE:
				{
					catData.TouchCount++;
					// 摸貓次數超過極限值就會驚嚇而後生氣
					if (catData.TouchCount > GameConfig.MAX_TOUCH_CAT_COUNT) {
						MakeCatAngry (catData);
						result.isAngry = true;
					} else {
						var addExp = GameConfig.GetTouchCatAddExp (catData);
						// 普通情況就增加Exp
						var isLevelUp = AddCatExp (catData, addExp);
						result.addExp = addExp;
						result.isLevelUp = isLevelUp;
					}
				}
				break;
			case GameConfig.CAT_STATE_ID.STATE_SLEEP:
				{
					var isAwake = WakeUpCatButMayCauseAngry (catData);
					// 記得是播生氣的動畫, 醒來的動畫不要播
					result.isAngry = isAwake;
					result.reduceTime = GameConfig.REDUCE_CAT_SLEEP_TIME;
				}
				break;
			default:
				throw new UnityException ("在錯誤的狀態摸貓，請檢查程式");
			}
			return result;
		}

		public AnimationResult UseSToyForCat (ItemKey catId, ItemKey item)
		{
			var result = new AnimationResult();
			var cat = GetCat (catId);
			GameConfig.CAT_STATE_ID stateId = cat.Status;
			switch (stateId) {
			case GameConfig.CAT_STATE_ID.STATE_ANGRY:
				{
					if (item.Type != StoreCtrl.DATA_S_TOY) {
						throw new UnityException ("使用道具類型檢查錯誤，請確認這是你要使用的類型");
					}
					var itemDef = ItemDef.Get (item.Idx);
					switch (itemDef.ID) {
					case "I31010":
					case "I31020":
						break;
					default:
						throw new UnityException ("沒有這個安撫道具:"+item.StringKey);
					}
					var reduceTime = (int)(GameConfig.CAT_ANGRY_TIME * itemDef.Time);
					var isEndOfTime = ReduceCatTimeOfEnd (cat, reduceTime);
					result.reduceTime = reduceTime;

					if (isEndOfTime) {
						cat.Status = GameConfig.CAT_STATE_ID.STATE_IDLE;
					}
					result.isEndOfTime = isEndOfTime;
				}
				break;
			default:
				throw new UnityException ("在錯誤的狀態中使用道具，請檢查程式");
			}
			return result;
		}

		public AnimationResult UseFoodForCat (ItemKey catId, ItemKey item)
		{
			var result = new AnimationResult();
			var cat = GetCat (catId);
			GameConfig.CAT_STATE_ID stateId = cat.Status;
			switch (stateId) {
			case GameConfig.CAT_STATE_ID.STATE_HUNGRY:
				{
					if (item.Type != StoreCtrl.DATA_FOOD) {
						throw new UnityException ("使用道具類型檢查錯誤，請確認這是你要使用的類型");
					}
					// 檢查使用道具
					var itemDef = ItemDef.Get (item.Idx);
					if (itemDef.Type != StoreCtrl.DATA_FOOD) {
						throw new UnityException ("沒有這個食物:"+item.StringKey);
					}
					/*switch (itemDef.ID) {
					case "I10010":
					case "I10020":
					case "I10030":
					case "I10040":
					case "I10050":
					case "I10060":
					case "I10070":
					case "I10080":
						break;
					default:
						throw new UnityException ("沒有這個食物:"+item.StringKey);
					}*/
					// 動畫編號
					var animIdx = 0;
					switch (itemDef.ID) {
					case ItemDef.I10010:
					case ItemDef.I50010:
						animIdx = 0;
						break;
					case ItemDef.I10020:
					case ItemDef.I50020:
						animIdx = 1;
						break;
					case ItemDef.I10030:
					case ItemDef.I50030:
						animIdx = 2;
						break;
					case ItemDef.I10040:
						animIdx = 3;
						break;
					case ItemDef.I10050:
						animIdx = 4;
						break;
					case ItemDef.I10060:
						animIdx = 5;
						break;
					case ItemDef.I10070:
						animIdx = 6;
						break;
					case ItemDef.I10080:
						animIdx = 7;
						break;
					}

					// 加血
					var isFullHp = cat.AddHp (itemDef.Hp);
					if (isFullHp) {
						cat.Status = GameConfig.CAT_STATE_ID.STATE_IDLE;
					}
					// 加經驗
					var isLevelUp = AddCatExp(cat, itemDef.Exp);

					result.feedItemIdx = animIdx;
					result.addHp = itemDef.Hp;
					result.isFullHp = isFullHp;
					result.isLevelUp = isLevelUp;
				}
				break;
			default:
				throw new UnityException ("在錯誤的狀態中使用道具，請檢查程式");
			}
			return result;
		}

		public AnimationResult UseToyForCat (ItemKey catId, ItemKey item)
		{
			var result = new AnimationResult();
			var cat = GetCat (catId);
			GameConfig.CAT_STATE_ID stateId = cat.Status;
			switch (stateId) {
			case GameConfig.CAT_STATE_ID.STATE_WANNA_PLAY:
				{
					if (item.Type != StoreCtrl.DATA_TOY) {
						throw new UnityException ("使用道具類型檢查錯誤，請確認這是你要使用的類型");
					}
					#if RESTRICT_WANNA_PLAY_ITEM1
					// 判斷選取的道具是不是想玩的道具
					var wannaPlayItem = new ItemKey (cat.WannaPlayToyItemKey);
					if (wannaPlayItem.StringKey != item.StringKey){
						throw new UnityException ("這隻貓不是想玩這個道具");
					}
					#else
					var wannaPlayItem = item;
					#endif
					var itemDef = ItemDef.Get (wannaPlayItem.Idx);
					var addExp = itemDef.Exp;
					// 正確。增加經驗
					var isLevelUp = AddCatExp(cat, addExp);
					// 改變狀態
					cat.Status = GameConfig.CAT_STATE_ID.STATE_IDLE;
					// 設定動畫
					var item1 = StoreCtrl.GetWannaPlayItemKey (catId, 0);
					var item2 = StoreCtrl.GetWannaPlayItemKey (catId, 1);

					result.addExp = addExp;
					result.isLevelUp = isLevelUp;
					result.wannaPlayItemIdx = 
						wannaPlayItem.StringKey == item1.StringKey ? 0 :
						wannaPlayItem.StringKey == item2.StringKey ? 1 : 1;
				}
				break;
			default:
				throw new UnityException ("在錯誤的狀態中使用道具，請檢查程式");
			}
			return result;
		}

		public bool UpdateWannaPlayTime (Action<ItemKey> OnCatWannaPlay)
		{
			// 超過一定的時間
			if (DateTime.Now.Ticks >= WannaPlayTimeTick) {
				// 計算擁有貓數量的3成
				int maxWannaPlayCatCount = GetOwnedCats ().Count() * 3 / 10;		// 30%
				// 最少一隻
				if (maxWannaPlayCatCount == 0) {
					maxWannaPlayCatCount = 1;
				}
				int wannaPlayCatCount = GetOwnedCats ().Count (key => {
					var cat = GetCat(key);
					return cat.Status == GameConfig.CAT_STATE_ID.STATE_WANNA_PLAY;
				});
				// 如果有超過3成的貓都是想玩狀態，就不必選擇新貓變成想玩狀態
				if (wannaPlayCatCount >= maxWannaPlayCatCount)
					return false;
				// 會隨機選擇一隻貓變成想玩狀態
				int idx = UnityEngine.Random.Range (0, GetOwnedCats().Count());
				ItemKey chooseCatKey = GetOwnedCats ().Skip (idx).First ();
				Cat chooseCatData = GetCat (chooseCatKey);
				// 生氣狀態無法變成想玩狀態
				if (chooseCatData.Status == GameConfig.CAT_STATE_ID.STATE_ANGRY) {
					// 取消這次的判斷
					NextWannaPlayTimeTick ();
					return false;
				}
				chooseCatData.Status = GameConfig.CAT_STATE_ID.STATE_WANNA_PLAY;
				int itemIdx = UnityEngine.Random.Range (0, 2);
				var catKey = new ItemKey (chooseCatData.Key);
				var wannaPlayItem = StoreCtrl.GetWannaPlayItemKey (catKey, itemIdx);
				chooseCatData.WannaPlayToyItemKey = wannaPlayItem.StringKey;
				NextWannaPlayTimeTick ();
				if (OnCatWannaPlay != null) {
					OnCatWannaPlay (catKey);
				}
				return true;
			}
			return false;
		}

		public bool UpdateSleepTime (Action<ItemKey> OnCatSleepTimeOver)
		{
			var shouldWakeup = 
				from c in from k in GetOwnedCats ()
				          select GetCat (k)
				where c.Status == GameConfig.CAT_STATE_ID.STATE_SLEEP && DateTime.Now.Ticks >= c.TimeOfEnd
				select c;
			foreach (var c in shouldWakeup) {
				c.Status = GameConfig.CAT_STATE_ID.STATE_IDLE;
				c.TouchCount = 0;
				OnCatSleepTimeOver (new ItemKey (c.Key));
			}
			return shouldWakeup.Count () > 0;
		}

		public bool UpdateAngryTime (Action<ItemKey> OnCatAngryTimeOver)
		{
			var shouldNormal = 
				from c in from k in GetOwnedCats ()
				select GetCat (k)
					where c.Status == GameConfig.CAT_STATE_ID.STATE_ANGRY && DateTime.Now.Ticks >= c.TimeOfEnd
				select c;
			foreach (var c in shouldNormal) {
				c.Status = GameConfig.CAT_STATE_ID.STATE_IDLE;
				OnCatAngryTimeOver (new ItemKey (c.Key));
			}
			return shouldNormal.Count () > 0;
		}

		// 減少貓的生氣時間
		bool ReduceCatTimeOfEnd (Cat catData, int reduceTime){
			if (catData.Status != GameConfig.CAT_STATE_ID.STATE_ANGRY) {
				throw new UnityException ("非生氣的貓不必減少生氣時間。現在狀態是" + catData.Status);
			}
			DateTime endDateTime = new DateTime (catData.TimeOfEnd);
			endDateTime = endDateTime.AddMinutes (-reduceTime);
			catData.TimeOfEnd = endDateTime.Ticks;
			return endDateTime.Ticks <= DateTime.Now.Ticks;
		}

		// 睡眼狀態時摸貓，加速貓醒來的時間
		// 摸醒的話貓會進入生氣狀態
		bool WakeUpCatButMayCauseAngry (Cat catData){
			if (catData.Status != GameConfig.CAT_STATE_ID.STATE_SLEEP) {
				throw new UnityException ("非睡眠狀態無法叫醒");
			}
			long reduceDateTick = DateTime.Now.AddMinutes (GameConfig.REDUCE_CAT_SLEEP_TIME).Ticks - DateTime.Now.Ticks;
			catData.TimeOfEnd -= reduceDateTick;
			var isAwake = catData.TimeOfEnd <= DateTime.Now.Ticks;
			if (isAwake) {
				MakeCatAngry (catData);
			}
			return isAwake;
		}

		public IEnumerable<Dictionary<string,object>> Cats2Memonto(){
			return 
				from cat in cats
				select new Dictionary<string,object> () {
				{"key", cat.Key},
				{"lv", cat.Lv},
				{"exp", cat.Exp},
				{"hp", cat.Hp},
				{"maxHp", cat.MaxHp},
				{"status", System.Convert.ToInt32(cat.Status)},
				{"battleCount", cat.BattleCount},
				{"wannaPlayToyItemKey", cat.WannaPlayToyItemKey},
				{"touchCount", cat.TouchCount},
				{"timeOfEnd", cat.TimeOfEnd}
			};
		}
		public void Memonto2Cats(JsonData json){
			if (json.IsArray == false) {
				throw new UnityException ("貓格式不對");
			}
			ClearCats ();
			for (var i = 0; i < json.Count; ++i) {
				var o = json [i];
				var cat = new GameObject ().AddComponent<Cat> ();
				cat.gameObject.transform.SetParent (this.transform);
				cat.Key = (string)o ["key"];
				cat.Lv = (int)o ["lv"];
				cat.Exp = (int)o ["exp"];
				cat.Hp = (int)o ["hp"];
				cat.MaxHp = (int)o ["maxHp"];
				cat.Status = (GameConfig.CAT_STATE_ID)(int)o ["status"];
				cat.BattleCount = (int)o ["battleCount"];
				if(o ["wannaPlayToyItemKey"]!=null){
					cat.WannaPlayToyItemKey = (string)o ["wannaPlayToyItemKey"];
				}
				cat.TouchCount = (int)o ["touchCount"];
				if (o ["timeOfEnd"].IsInt) {
					cat.TimeOfEnd = (int)o ["timeOfEnd"];
				} else if (o ["timeOfEnd"].IsLong) {
					cat.TimeOfEnd = (long)o ["timeOfEnd"];
				}
				cat.name = cat.Key;
				cats.Add (cat);
			}
		}

		public void ClearCats(){
			foreach (var c in cats) {
				GameObject.Destroy (c.gameObject);
			}
			cats.Clear ();
		}
		#endregion

		#region daily gift
		public List<long> dailyGift;
		public IEnumerable<ItemKey> GetTodayGiftAndRecordIt(){
			var now = DateTime.Now;
			if (IsAlreadyGetGift (now.Ticks)) {
				Debug.LogWarning ("這天的禮物已領過");
				return new List<ItemKey>(); 
			}
			dailyGift.Add (now.Ticks);
			// 只緩衝31天
			if (dailyGift.Count > 31) {
				dailyGift.RemoveAt (0);
			}
			// 刪掉非同一月份
			for (var i = dailyGift.Count - 1; i >= 0; --i) {
				var thatDay = new DateTime (dailyGift [i]);
				if (thatDay.Month == now.Month && thatDay.Year == now.Year) {
					continue;
				}
				dailyGift.RemoveAt (i);
			}
			// 取得第幾個獎勵
			// 注意：count已被加1
			var giftIdx = GetTopGiftId();
			var giftKey = string.Format ("Mg{0:00}", giftIdx);
			try{
				var def = GiftDef.Get(giftKey);
				var itemKey = ItemKey.WithItemConfigID(def.Item);
				return Enumerable.Repeat(itemKey, def.Quantity);
			}catch(Exception e){
				Debug.LogWarning ("沒有這天的禮物："+e.Message);
				return new List<ItemKey>();
			}
		}

		public int GetTopGiftId(){
			return dailyGift.Count;
		}

		public bool IsAlreadyGetGift(long time){
			var day = new DateTime (time);
			foreach (var t in dailyGift) {
				var getDay = new DateTime (t);
				if (getDay.Year != day.Year) {
					continue;
				}
				if (getDay.Month != day.Month) {
					continue;
				}
				if (getDay.Day != day.Day) {
					continue;
				}
				return true;
			}
			return false;
		}

		public void Memonto2Gift(JsonData json){
			if (json == null) {
				return;
			}
			if (json.IsArray == false) {
				throw new UnityException ("格式不對");
			}
			ClearDailyGift ();
			for (var i = 0; i < json.Count; ++i) {
				if (json [i].IsInt) {
					var o = (int)json [i];
					dailyGift.Add (o);
				} else if(json[i].IsLong) {
					var o = (long)json [i];
					dailyGift.Add (o);
				}
			}
		}

		public void ClearDailyGift(){
			dailyGift.Clear ();
		}
		#endregion

		#region persistance
		string GetMemonto(){
			// 可能有以下例外
			// 代表不支援轉成Json
			// Max allowed object depth reached while trying to export from type System.Single
			// http://support.gamedonia.com/kb/common-problems/i-get-jsonexception-max-allowed-object-depth-reached-while-trying-to-export-from-type-systemsingle-error
			var version = 0;
			var dict = new Dictionary<string,object> () {
				{ "version", version },
				{ "gold", gold },
				{ "money", money },
				{ "wannaPlayTime", wannaPlayTimeTick },
				// 轉成Json時不支援float
				{ "audioOffsetTime", (double)audioOffsetTime },
				{ "selectCat", selectCat},
				{ "cats", Cats2Memonto().ToList() },
				{ "unlockCapture", captureEnables },
				{ "captures", Captures2Memonto().ToList() },
				{ "photos", enablePhotos },
				{ "passLevels", passLevels },
				{ "passLevelInfos", PassLevelInfos2Memonto().ToList() },
				{ "items", items },
				{ "dailyGift", dailyGift },
				{ "unlockIAP", unlockIAP }
			};
			return JsonMapper.ToJson (dict);
		}

		void SetMemonto(string json){
			JsonData jo = JsonMapper.ToObject (json);
			var version = (int)jo ["version"];
			switch (version) {
			case 0:
				{
					gold = (int)jo ["gold"];
					money = (int)jo ["money"];
					if (jo ["wannaPlayTime"].IsInt) {
						wannaPlayTimeTick = (int)jo ["wannaPlayTime"];
					} else if (jo ["wannaPlayTime"].IsLong) {
						wannaPlayTimeTick = (long)jo ["wannaPlayTime"];
					}
					// 一定要先轉成double
					audioOffsetTime = (float)(double)jo ["audioOffsetTime"];
					selectCat = (string)jo ["selectCat"];
					Memonto2Cats (jo ["cats"]);
					Memonto2Captures (jo ["captures"]);
					Memonto2CaptureEnable (jo ["unlockCapture"]);
					Memonto2Photos (jo ["photos"]);
					Memonto2PassLevels (jo ["passLevels"]);
					Memonto2PassLevelInfos (jo ["passLevelInfos"]);
					Memonto2Items (jo ["items"]);
					Memonto2Gift (jo ["dailyGift"]);
					Memonto2UnlockIAP (jo ["unlockIAP"]);
				}
				break;
			};
		}

		Thread savingThread;
		bool saveThreadingRequest = false;
		public void RequestSaveThreading(string deviceId){
			// 申請儲存需求
			saveThreadingRequest = true;
			// 還在儲存中，回傳。因已申請儲存需求
			if (savingThread != null) {
				Util.Instance.LogWarning ("還在儲存中，回傳");
				return;
			}
			var json = GetMemonto ();
			ProcessSaveThreading (deviceId, json);
		}

		void ProcessSaveThreading(string deviceId, string json){
			saveThreadingRequest = false;
			savingThread = new Thread (() => {
				// 先儲存
				Save(deviceId, json);
				// 存完後交給MainThread判斷是否又有儲存需求
				UnityMainThreadDispatcher.Instance().Enqueue(()=>{
					// 有。立刻再儲存一次
					if(saveThreadingRequest){
						Util.Instance.LogWarning ("接著再存一次");
						var currJson = GetMemonto();
						ProcessSaveThreading(deviceId, currJson);
					} else {
						// 直到沒有儲存需求
						savingThread = null;
					}
				});
			});
			savingThread.Start ();
		}


		Coroutine savingInstance;
		bool saveRequest = false;
		public void RequestSaveRemote(string deviceId){
			saveRequest = true;
			if (savingInstance != null) {
				return;
			}
			savingInstance = StartCoroutine (SavingRemote (deviceId));
		}

		IEnumerator SavingRemote(string deviceId){
			while (saveRequest) {
				yield return new WaitForEndOfFrame ();
				saveRequest = false;
				var json = GetMemonto();
				// 備份雲端
				// UnityWebRequest不能在另一條執行緒中創建
				// 所以代碼放在這
				var request = RemixApi.CreateURLRequest (RemixApi.API_HOST+"/api/device/"+deviceId+"/record", RemixApi.GetAuthorizationString (""), "data="+WWW.EscapeURL (json));
				request.SendWebRequest ();
				while (request.isDone == false) {
					yield return null;
				}
				if (request.isNetworkError) {
					Util.Instance.LogError (request.error);
				} else {
					var data = System.Text.Encoding.UTF8.GetString (request.downloadHandler.data);
					var jo = JsonMapper.ToObject (data);
					if (jo [0] != null) {
						Util.Instance.LogError ((string)jo [0]);
					}
				}
			}
			savingInstance = null;
		}

		public void Save (string deviceId, string json){
			if (!Directory.Exists (GameConfig.SAVE_PATH))
				Directory.CreateDirectory (GameConfig.SAVE_PATH);
			try{
				var sr = File.CreateText(GameConfig.SAVE_PATH + "/record.json");
				sr.WriteLine(json);
				sr.Close ();
			}catch(Exception e2){
				throw e2;
			}
		}

		public void Override(string json){
			SetMemonto (json);
		}

		public void Clear(){
			if (Directory.Exists (GameConfig.SAVE_PATH) && File.Exists (GameConfig.SAVE_PATH + "/record.json")) {
				File.Delete(GameConfig.SAVE_PATH + "/record.json");
			}
			SetFirstPlayData ();
		}

		public void Load (){
			// 不從雲端讀資取。本質上這款遊戲屬本機遊戲
			// 雲端記錄只是拿來備份或修復用
			// 直接讀取本機
			if (Directory.Exists (GameConfig.SAVE_PATH) && File.Exists (GameConfig.SAVE_PATH + "/record.json")) {
				var saveFile = File.OpenText (GameConfig.SAVE_PATH + "/record.json");
				if (saveFile != null) {
					try{
						var json = saveFile.ReadToEnd ();
						SetMemonto(json);
					}catch(Exception e2){
						Debug.LogWarning ("資料儲存格式板本錯誤，清掉重建:"+e2.Message);
						SetFirstPlayData ();
					}finally{
						saveFile.Close ();
					}
				}
			} else {
				Debug.LogWarning ("沒有存檔，建立第一次玩資料");
				SetFirstPlayData ();
			}
		}

		public void SetBasicData(){
			ClearPlayer ();
			ClearPassLevels ();
			ClearPhotos ();
			ClearItems ();
			ClearCats ();
			ClearCaptures ();
			ClearCaptureEnable ();
			ClearPassLevelInfos ();
			ClearDailyGift ();
			ClearUnlockIAP ();
			// 起始金錢
			Money = 50;
			Gold = 2000;
			// 預設開第一隻貓
			var firstCat = new ItemKey (StoreCtrl.DATA_CAT, 0);
			EnableCat (firstCat);
			// 選擇第一隻貓
			SetSelectCat(firstCat);
			// 設定下一次想玩的時間
			NextWannaPlayTimeTick ();
			// 第一關的第一個轉蛋點預設解鎖
			EnableCapture (new CaptureKey () {
				MapIdx = "01",
				Idx = 0
			});
		}

		public void SetFirstPlayData(){
			SetBasicData ();
			// 起始道具
			// 罐頭5個
			AddItem (ItemKey.WithItemConfigID ("I10010"), 5);
			#if FULL_OPEN_VERSION1
			FullOpen();
			#endif
		}

		void FullOpen(){
			// 金錢無限
			Money = Gold = 999999;
			// 貓全開
			for (var i = 0; i < GameConfig.CAT_IDXS.Count; ++i) {
				var cat = new ItemKey (StoreCtrl.DATA_CAT, i);
				EnableCat (cat);
			}
			// 探索全開
			foreach (var mapIdx in GameConfig.MAP_IDXS) {
				for (var i = 0; i < 3; ++i) {
					var capture = new CaptureKey () {
						MapIdx = mapIdx,
						Idx = i
					};
					EnableCapture (capture);
				}
			}
			// 關卡全開
			foreach (var mapIdx in GameConfig.MAP_IDXS) {
				for (var i = 0; i < 10; ++i) {
					var levelKey = new LevelKey () {
						MapIdx = mapIdx,
						Idx = i,
						Difficulty = GameRecord.Easy
					};
					PassLevel (levelKey);
				}
			}
			// 照片全開
			foreach (var mapIdx in GameConfig.MAP_IDXS) {
				try{
					var sc = GetPhotosCount (mapIdx, PhotoKey.TypeSmallPhoto);
					var bc = GetPhotosCount (mapIdx, PhotoKey.TypeBigPhoto);
					var total = sc + bc;
					var firstPhoto = new PhotoKey () {
						MapIdx = mapIdx,
						Type = PhotoKey.TypeSmallPhoto,
						Idx = 0
					};
					var currPhoto = firstPhoto;
					for (var i = 0; i < total; ++i) {
						EnablePhoto (currPhoto);
						currPhoto = currPhoto.NextKey;
					}
				}catch(Exception){
					// ignore
				}
			}
		}
		#endregion

		public static int GetExpOffset(ItemKey cat, int lv){
			var maxExp = 0;
			var maxHp = 0;
			var maxLv = 0;
			var maxExp2 = 0;
			var maxHp2 = 0;
			var maxLv2 = 0;
			GameRecord.GetCatLvInfo (cat, lv, ref maxExp, ref maxHp, ref maxLv);
			GameRecord.GetCatLvInfo (cat, lv+1, ref maxExp2, ref maxHp2, ref maxLv2);
			var expOffset = maxExp2 - maxExp;
			return expOffset;
		}

		public static void GetCatLvInfo(ItemKey cat, int lv, ref int maxExpRef, ref int maxHpRef, ref int maxLvRef){
			if (lv < 0) {
				return;
			}
			// 封項
			if (lv >= CatLvRate1.ID_COUNT - 1) {
				lv = CatLvRate1.ID_COUNT - 1;
			}
			CatDef catDef = CatDef.Get (cat.Idx);
			var maxExp = 0;
			var maxHp = 0;
			var maxLv = 0;
			switch (catDef.Rare) {
			case 1:
				{
					var lvDef = CatLvRate1.Get (lv);
					maxExp = lvDef.Exp;
					maxHp = lvDef.Hp;
					maxLv = CatLvRate1.ID_COUNT - 1;
				}
				break;
			case 2:
				{
					var lvDef = CatLvRate2.Get (lv);
					maxExp = lvDef.Exp;
					maxHp = lvDef.Hp;
					maxLv = CatLvRate2.ID_COUNT - 1;
				}
				break;
			case 3:
				{
					var lvDef = CatLvRate3.Get (lv);
					maxExp = lvDef.Exp;
					maxHp = lvDef.Hp;
					maxLv = CatLvRate3.ID_COUNT - 1;
				}
				break;
			case 4:
				{
					var lvDef = CatLvRate4.Get (lv);
					maxExp = lvDef.Exp;
					maxHp = lvDef.Hp;
					maxLv = CatLvRate4.ID_COUNT - 1;
				}
				break;
			case 5:
				{
					var lvDef = CatLvRate5.Get (lv);
					maxExp = lvDef.Exp;
					maxHp = lvDef.Hp;
					maxLv = CatLvRate5.ID_COUNT - 1;
				}
				break;
			case 6:
				{
					var lvDef = CatLvRate6.Get (lv);
					maxExp = lvDef.Exp;
					maxHp = lvDef.Hp;
					maxLv = CatLvRate6.ID_COUNT - 1;
				}
				break;
			}
			maxExpRef = maxExp;
			maxHpRef = maxHp;
			maxLvRef = maxLv;
		}
		// 更新貓的等級，在貓增加經驗值時一定要呼叫這個方法
		public static bool UpdateCatLv (Cat catData){
			var itemKey = new ItemKey (catData.Key);

			var maxExp = 0;
			var maxHp = 0;
			var maxLv = 0;
			GetCatLvInfo (itemKey, catData.Lv, ref maxExp, ref maxHp, ref maxLv);

			if (catData.Lv >= maxLv && catData.Exp >= maxExp) {
				Debug.LogWarning ("已經封頂，無法升級");
				return false;
			}
			if (catData.Exp < maxExp) {
				return false;
			}
			catData.Lv += 1;
			// recur呼叫自己直到不能升級
			UpdateCatLv (catData);
			// 不管升了幾級，回傳true
			return true;
		}

		public static string GetWmapSConfigID(string mapIdx, int difficulty){
			var difStr = 
				difficulty == GameRecord.Easy ? "E" :
				difficulty == GameRecord.Normal ? "N" : "H";
			return string.Format ("Map-{0}{1}", mapIdx, difStr);
		}

		// 取得照片數量
		// 注意：定義檔中只有定義出的行數有意義，其中的內容在程式中完全用不到，也請不要使用
		public static int GetPhotosCount(string mapIdx, string photoKeyType){
			var fixType = photoKeyType == PhotoKey.TypeBigPhoto ? "B" : "S";
			switch (mapIdx) {
			case "01":
				{
					var ret = new List<string> ();
					for (var i = 0; i < Photom01.ID_COUNT; ++i) {
						var def = Photom01.Get (i);
						if (def.Type == fixType) {
							ret.Add (def.ID);
						}
					}
					return ret.Count();
				}
			case "02":
				{
					var ret = new List<string> ();
					for (var i = 0; i < Photom02.ID_COUNT; ++i) {
						var def = Photom02.Get (i);
						if (def.Type == fixType) {
							ret.Add (def.ID);
						}
					}
					return ret.Count();
				}
			case "03":
				{
					var ret = new List<string> ();
					for (var i = 0; i < Photom03.ID_COUNT; ++i) {
						var def = Photom03.Get (i);
						if (def.Type == fixType) {
							ret.Add (def.ID);
						}
					}
					return ret.Count();
				}
			case "04":
				{
					var ret = new List<string> ();
					for (var i = 0; i < Photom04.ID_COUNT; ++i) {
						var def = Photom04.Get (i);
						if (def.Type == fixType) {
							ret.Add (def.ID);
						}
					}
					return ret.Count();
				}
			case "S1":
				{
					var ret = new List<string> ();
					for (var i = 0; i < PhotomS1.ID_COUNT; ++i) {
						var def = PhotomS1.Get (i);
						if (def.Type == fixType) {
							ret.Add (def.ID);
						}
					}
					return ret.Count();
				}
			default:
				throw new UnityException ("這張地圖的抓道具定義檔未實做>mapIdx:"+mapIdx);
			}
		}

		public static string EventPicPrefabName(MapDef md){
			return string.Format ("event/{0};{1}", md.Prefab.ToLower(), md.Prefab);
		}

		public static GameObject GetForeground(string mapIdx, string mode){
			var prefabName = string.Format("map/{0}/FG{0}{1}", mapIdx, mode);
			return Util.Instance.GetPrefab (prefabName, null);
		}

		public static GameObject GetBackground(string mapIdx, string mode){
			var prefabName = string.Format("map/{0}/BG{0}{1}", mapIdx, mode);
			return Util.Instance.GetPrefab (prefabName, null);
		}
	}

	public class NodeKey{
		public const string GroupGameMap = "GameMap";
		public const string GroupGameCap = "GameCap";
		public string Group{ get; set; }
		public string MapIdx{ get; set; }
		public int Idx{ get; set; }
		public NodeKey(){}
		public NodeKey(string strKey){
			var token = strKey.Split ('_');
			Group = token [0];
			MapIdx = token [1];
			Idx = System.Convert.ToInt32 (token [2]);
		}
		public String StringKey { 
			get {
				return string.Format ("{0}_{1}_{2}", Group, MapIdx, Idx);
			}
		}
		public IEnumerable<LevelKey> LevelKeys{
			get{
				if (Group != GroupGameMap) {
					throw new UnityException ("Group不正確，不能取得LevelKey:"+Group);
				}
				return new List<LevelKey> () {
					new LevelKey(){MapIdx=MapIdx, Idx=Idx, Difficulty=GameRecord.Easy},
					new LevelKey(){MapIdx=MapIdx, Idx=Idx, Difficulty=GameRecord.Normal},
					new LevelKey(){MapIdx=MapIdx, Idx=Idx, Difficulty=GameRecord.Hard},
				};
			}
		}
		public CaptureKey CaptureKey {
			get {
				if (Group != GroupGameCap) {
					throw new UnityException ("Group不正確，不能取得CaptureKey:"+Group);
				}
				return new CaptureKey{ MapIdx = MapIdx, Idx = Idx };
			}
		}
	}

	public class CaptureKey{
		public string MapIdx{ set; get; }
		public int Idx{ set; get; }
		public CaptureKey(){}
		public CaptureKey(string strKey){
			var token = strKey.Split ('_');
			MapIdx = token [0];
			Idx = System.Convert.ToInt32 (token [1]);
		}
		public String StringKey { 
			get {
				return string.Format ("{0}_{1}", MapIdx, Idx);
			}
		}
		public string GachaConfigID{
			get{
				return string.Format ("M{0}GC{1:00}", MapIdx, Idx+1);
			}
		}
		public NodeKey NodeKey{
			get{
				return new NodeKey { 
					Group = NodeKey.GroupGameCap, 
					MapIdx = MapIdx, 
					Idx = Idx 
				};
			}
		}
	}

	public class LevelKey{
		public static LevelKey TutorialLevelKey = new LevelKey () {
			// 必須是01
			MapIdx = "01",
		};
		public static LevelKey DelayToolLevelKey = new LevelKey () {
			// 必須是01和0
			MapIdx = "01",
			Idx = 0
		};
		public string MapIdx{ set; get; }
		public int Idx{ set; get; }
		public int Difficulty{ set; get; }
		public LevelKey(){}
		public LevelKey(string strKey){
			var token = strKey.Split ('_');
			MapIdx = token [0];
			Idx = System.Convert.ToInt32 (token [1]);
			Difficulty = System.Convert.ToInt32 (token [2]);
		}
		public bool IsNormalLevel{
			get{
				return this != TutorialLevelKey && this != DelayToolLevelKey;
			}
		}
		public bool IsSurpass(LevelKey key){
			if (MapIdx != key.MapIdx) {
				Debug.LogWarning ("不是同一張地圖不能比較:"+MapIdx+"/"+key.MapIdx);
				return false;
			}
			return Idx > key.Idx;
		}
		public static bool IsNormalMap(string mapIdx) {
			return mapIdx != "S1" && mapIdx != "S2";
		}
		public static int CompareableMapIdx(string mapIdx) {
			if (IsNormalMap(mapIdx) == false) {
				throw new UnityException("一般地圖才能使用這個方法:"+mapIdx);
			}
			try{
				// 正規化為從0開始
				return int.Parse (mapIdx) - 1;
			}catch(Exception){
				throw new UnityException("進來這裡的MapIdx都必須能轉為數字，請檢查LevelKey.IsNormalMap:"+mapIdx);
			}
		}
		public LevelKey ChangeDifficulty(int dif){
			return new LevelKey () {
				MapIdx = MapIdx,
				Idx = Idx,
				Difficulty = dif
			};
		}
		public string StringKey { 
			get {
				return string.Format ("{0}_{1}_{2}", MapIdx, Idx, Difficulty);
			}
		}
		public string LevelFileName {
			get {
				if (this == TutorialLevelKey) {
					return "Training/M01L0T/M01L0T";
				}
				if (this == DelayToolLevelKey) {
					return "DelayTool/level_delaytool";
				}
				var basicName = string.Format("level/m{0}l{1:00};M{2}L{3:00}", MapIdx.ToLower(), Idx+1, MapIdx, Idx+1);
				switch (Difficulty) {
				case GameRecord.Easy:
					basicName += "_E";
					break;
				case GameRecord.Normal:
					basicName += "_N";
					break;
				}
				return basicName;
			}
		}
		public string LevelPicPrefabName {
			get {
				return string.Format("level/m{0}l{1:00};LPM{2}L{3:00}", MapIdx.ToLower(), Idx+1, MapIdx, Idx+1);
			}
		}
		public string LevelSoundPrefabName(string filename) {
			if (this == TutorialLevelKey) {
				return "Training/M01L0T/"+filename;
			}
			return string.Format("level/m{0}l{1:00};{2}", MapIdx.ToLower(), Idx+1, filename);
		}
		public string ConfigID{
			get{
				var difStr = 
					Difficulty == GameRecord.Easy ? "e" :
					Difficulty == GameRecord.Normal ? "n" : "h";
				return string.Format ("Map{0}_{1:00}{2}", MapIdx, Idx+1, difStr);
			}
		}
		public static string MapIdxWithMapConfigID(string configId){
			var mapId = configId.Substring ("Map".Length);
			return mapId;
		}
	}

	public class PhotoKey{
		// BP SP are just word, is nothing
		public const string TypeBigPhoto = "BP";
		public const string TypeSmallPhoto = "SP";
		public string Type{ get; set; }
		public string MapIdx{ get; set; }
		public int Idx{ get; set; }
		public bool IsBigPhoto{ get { return Type == TypeBigPhoto; } }
		public PhotoKey(){}
		public PhotoKey(string strKey){
			var token = strKey.Split ('_');
			Type = token [0];
			MapIdx = token [1];
			Idx = System.Convert.ToInt32 (token [2]);
		}
		public static IEnumerable<PhotoKey> Keys(string mapIdx){
			return Keys (mapIdx, TypeBigPhoto).Concat (Keys (mapIdx, TypeSmallPhoto));
		}
		public static IEnumerable<PhotoKey> Keys(string mapIdx, string type){
			var ret = new List<PhotoKey> ();
			var smallPhotoCount = GameRecord.GetPhotosCount (mapIdx, PhotoKey.TypeSmallPhoto);
			var bigPhotoCount = GameRecord.GetPhotosCount (mapIdx, PhotoKey.TypeBigPhoto);
			var max = type == TypeBigPhoto ? bigPhotoCount : smallPhotoCount;
			for (var i = 0; i < max; ++i) {
				ret.Add (new PhotoKey () {
					Type = type,
					MapIdx = mapIdx,
					Idx = i
				});
			}
			return ret;
		}
		public PhotoKey NextKey{
			get{
				var smallPhotoCount = GameRecord.GetPhotosCount (MapIdx, PhotoKey.TypeSmallPhoto);
				var bigPhotoCount = GameRecord.GetPhotosCount (MapIdx, PhotoKey.TypeBigPhoto);

				var nextId = Idx + 1;
				var max = Type == TypeBigPhoto ? bigPhotoCount : smallPhotoCount;
				var nextType = Type;
				if (nextId >= max) {
					nextType = Type == TypeBigPhoto ? TypeSmallPhoto : TypeBigPhoto;
					nextId = 0;
				}
				return new PhotoKey () {
					Type = nextType,
					MapIdx = MapIdx,
					Idx = nextId
				};
			}
		}
		public PhotoKey PrevKey{
			get{
				var smallPhotoCount = GameRecord.GetPhotosCount (MapIdx, PhotoKey.TypeSmallPhoto);
				var bigPhotoCount = GameRecord.GetPhotosCount (MapIdx, PhotoKey.TypeBigPhoto);

				var nextId = Idx - 1;
				var nextType = Type;
				if (nextId < 0) {
					nextType = Type == TypeBigPhoto ? TypeSmallPhoto : TypeBigPhoto;
					var max = nextType == TypeBigPhoto ? bigPhotoCount : smallPhotoCount;
					nextId = max - 1;
				}
				return new PhotoKey () {
					Type = nextType,
					MapIdx = MapIdx,
					Idx = nextId
				};
			}
		}
		public string StringKey{
			get{
				return string.Format ("{0}_{1}_{2}", Type, MapIdx, Idx);
			}
		}
		public string PhotoNameForB{
			get{
				switch (Type) {
				case TypeSmallPhoto:
					return string.Format("UIP{0}B{1:00}.JPG", MapIdx, Idx+1);
				case TypeBigPhoto:
					return string.Format("UIP{0}BS{1:0}.JPG", MapIdx, Idx+1);
				default:
					throw new UnityException ("沒有這個Type:"+Type);
				}
			}
		}
		public string PhotoNameForM{
			get{
				switch (Type) {
				case TypeSmallPhoto:
					return string.Format("UIP{0}M{1:00}.png", MapIdx, Idx+1);
				case TypeBigPhoto:
					return string.Format("UIP{0}MS{1:0}.png", MapIdx, Idx+1);
				default:
					throw new UnityException ("沒有這個Type:"+Type);
				}
			}
		}
		public string PhotoNameForS{
			get{
				switch (Type) {
				case TypeSmallPhoto:
					return string.Format("UIP{0}S{1:00}.png", MapIdx, Idx+1);
				case TypeBigPhoto:
					return string.Format("UIP{0}SS{1:0}.png", MapIdx, Idx+1);
				default:
					throw new UnityException ("沒有這個Type:"+Type);
				}
			}
		}
		public string PhotoDisplayName{
			get{
				return PhotoNameForS.Replace (".png", "");
			}
		}
	}

	public class ItemKey{
		public static ItemKey Empty = new ItemKey (null, -1);
		public string Type;
		public int Idx;
		public static ItemKey WithItemConfigID(string configID){
			for (var i = 0; i < ItemDef.ID_COUNT; ++i) {
				var def = ItemDef.Get (i);
				if (def.ID == configID) {
					return new ItemKey (def.Type, i);
				}
			}
			throw new UnityException ("沒有這個ItemConfigID:"+configID);
		}
		public ItemKey(string type, int idx){
			Type = type;
			Idx = idx;
		}
		public ItemKey(string stringKey){
			var token = stringKey.Split (new char[]{'_'}, 2);
			Type = token [0];
			Idx = System.Convert.ToInt32 (token [1]);
		}
		public string StringKey{
			get{
				return string.Format ("{0}_{1}", Type, Idx);
			}
		}
		public string StorageItemPrefabName{
			get{
				var def = ItemDef.Get (Idx);
				return string.Format("Item/{0}", def.Prefab);
			}
		}
		// 取得貓所在的房間。注意：貓的定義順序不可變
		public int HousePageMapIdx{
			get{
				if (Type != StoreCtrl.DATA_CAT) {
					throw new UnityException ("錯誤的類型:"+Type);
				}
				return Idx / 6;
			}
		}
		// 取得貓在的房間的序號。注意：貓的定義順序不可變
		public int CatSeqIdInHousePageMap{
			get{
				if (Type != StoreCtrl.DATA_CAT) {
					throw new UnityException ("錯誤的類型:"+Type);
				}
				return Idx % 6;
			}
		}
		public string IAPPrefabName{
			get{
				if (Type != StoreCtrl.DATA_IAP) {
					throw new UnityException ("錯誤的類型:"+Type);
				}
				return string.Format ("item/IAP{0:00}", Idx+1);
			}
		}
		public string BuyItemPrefabName{
			get{
				if (Type != StoreCtrl.DATA_CAMERA &&
					Type != StoreCtrl.DATA_S_CAMERA &&
					Type != StoreCtrl.DATA_TOY &&
					Type != StoreCtrl.DATA_S_TOY &&
					Type != StoreCtrl.DATA_FOOD &&
					Type != StoreCtrl.DATA_TOY_CAPTURE &&
					Type != StoreCtrl.DATA_MG &&
					Type != StoreCtrl.DATA_MM ) {
					throw new UnityException ("錯誤的類型:"+Type);
				}
				var def = ItemDef.Get (Idx);
				return string.Format("item/B{0}", def.Prefab);
			}
		}
		public string CatBSCPrefabName{
			get{
				if (Type != StoreCtrl.DATA_CAT) {
					throw new UnityException ("錯誤的類型:"+Type);
				}
				var resourceId = CatResourceId;
				return string.Format ("caticon/BSC{0}", resourceId);
			}
		}
		public string CatSoundPrefab {
			get {
				var resourceId = CatResourceId;
				return string.Format ("cat/{0}/CS{0}", resourceId);
			}
		}
		// gameModeResourceStr = "P1" | "P2" | "P3" | "I" | "F"
		public string CatSpinePrefab(string gameModeResourceStr){
			var resourceId = CatResourceId;
			return string.Format("cat/{0}/C{0}{1}", resourceId, gameModeResourceStr);
		}
		public string CatIconPrefabName(string prefix, GameConfig.CAT_STATE_ID catStateId){
			if (Type != StoreCtrl.DATA_CAT) {
				throw new UnityException ("錯誤的類型:"+Type);
			}
			var stateId = 0;
			switch (catStateId)
			{
			case GameConfig.CAT_STATE_ID.STATE_NONE:
			case GameConfig.CAT_STATE_ID.STATE_IDLE:
				stateId = 1;
				break;
			case GameConfig.CAT_STATE_ID.STATE_ANGRY:
				stateId = 2;
				break;
			case GameConfig.CAT_STATE_ID.STATE_WANNA_PLAY:
				stateId = 3;
				break;
			case GameConfig.CAT_STATE_ID.STATE_SLEEP:
				stateId = 4;
				break;
			case GameConfig.CAT_STATE_ID.STATE_HUNGRY:
				stateId = 5;
				break;
			default:
				stateId = 1;
				break;
			}
			var resourceId = CatResourceId;
			// CI的資源在CatIcon中
			if (prefix == "CI") {
				return string.Format ("caticon/{1}{2}_{3:00}", resourceId, prefix, resourceId, stateId);	
			}
			return string.Format ("cat/{0}/{1}{2}_{3:00}", resourceId, prefix, resourceId, stateId);	
		}

		string CatResourceId{
			get{
				if (Type != StoreCtrl.DATA_CAT) {
					throw new UnityException ("錯誤的類型:"+Type);
				}
				if (IsBossCat) {
					return GameConfig.CAT_IDXS [Idx];
				}
				return string.Format ("{0:00}", Idx+1);
			}
		}

		public bool IsBossCat{
			get{
				if (Type != StoreCtrl.DATA_CAT) {
					throw new UnityException ("錯誤的類型:"+Type);
				}
				switch (GameConfig.CAT_IDXS [Idx]) {
				case "M1":
				case "M2":
				case "M3":
				case "M4":
					{
						return true;
					}
				default:
					return false;
				}
			}
		}

		public string CatIdVer2InPrefab(bool UseCommonType) {
			if (Type != StoreCtrl.DATA_CAT) {
				throw new UnityException ("錯誤的類型:"+Type);
			}
			if (UseCommonType) {
				if (IsBossCat) {
					return "M1";
				}
				if (Idx % 3 == 0) {
					return "A1";
				}
				if (Idx % 3 == 1) {
					return "B1";
				}
				if (Idx % 3 == 2) {
					return "C1";
				}
			} else {
				return GameConfig.CAT_IDXS [Idx];
			}
			throw new UnityException ("沒有這個貓Idx:" + Idx);
		}
		public string ObstaclePrefabName(string mapIdx, int obstacleIdx){
			if (Type != StoreCtrl.DATA_CAT) {
				throw new UnityException ("錯誤的類型:"+Type);
			}
			if (obstacleIdx < 4) {
				return string.Format ("catclass/{0}/C{1}_{2:00}",CatIdVer2InPrefab(true).ToLower(), CatIdVer2InPrefab(true), obstacleIdx+1);
			} else {
				return string.Format ("map/{0}/M{1}_{2:00}", mapIdx.ToLower(), mapIdx, obstacleIdx+1);
			}
		}
		public static ItemKey WithCatConfigID(string configId){
			for(var i=0; i<CatDef.ID_COUNT; ++i){
				var def = CatDef.Get (i);
				if (def.ID == configId) {
					return new ItemKey (StoreCtrl.DATA_CAT, i);
				}
			}
			throw new UnityException("沒有這個CatConfigID:"+configId);
		}
	}

	public class Capture : MonoBehaviour{
		public string Key;
		public string ItemKey;
		public long TimeOfEndTick;
		public string GettedItem;
		public GameConfig.CAPTURE_STATE State = GameConfig.CAPTURE_STATE.NORMAL;

		public bool IsShouldGetCat{
			get{
				if (new Remix.ItemKey (ItemKey).Type == StoreCtrl.DATA_TOY_CAPTURE) {
					return true;
				}
				return false;
			}
		}

		public bool IsShouldGetPhoto{
			get{
				if (new Remix.ItemKey (ItemKey).Type == StoreCtrl.DATA_CAMERA) {
					return true;
				}
				return false;
			}
		}

		public ItemKey GetGettedCat(){
			if (State != GameConfig.CAPTURE_STATE.COMPLETED) {
				throw new UnityException ("現在還不能採收");
			}
			if (new Remix.ItemKey (ItemKey).Type != StoreCtrl.DATA_TOY_CAPTURE) {
				throw new UnityException ("你不是使用玩具抓貓");
			}
			if (GettedItem == null) {
				throw new UnityException ("沒有設到GettedItem，這是過去程式錯誤，請清除記錄重來一次");
			}
			return new Remix.ItemKey (GettedItem);
		}

		public IEnumerable<PhotoKey> GetGettedPhoto(){
			if (State != GameConfig.CAPTURE_STATE.COMPLETED) {
				throw new UnityException ("現在還不能採收");
			}
			if (new Remix.ItemKey (ItemKey).Type != StoreCtrl.DATA_CAMERA) {
				throw new UnityException ("你不是使用照相工具");
			}
			if (GettedItem == "") {
				return new List<PhotoKey> ();
			}
			var keys = GettedItem.Split (new char[]{','});
			return 
				from k in keys
				select new PhotoKey (k);
		}

		public int GetCaptureTimeOffset (){
			if (State != GameConfig.CAPTURE_STATE.CAPTURE) {
				// 沒有探索
				return -1;
			}
			DateTime endOfTime = new DateTime (TimeOfEndTick);
			TimeSpan offsetTime = endOfTime.Subtract (DateTime.Now);
			if (offsetTime.TotalSeconds < 0)
				return 0;
			return Convert.ToInt32 (offsetTime.TotalSeconds);
		}
	}

	public class Cat : MonoBehaviour{
		public string Key;
		public int Lv;
		public int Exp;
		public int Hp = 100;
		public int MaxHp = 100;
		// 貓的狀態
		public GameConfig.CAT_STATE_ID Status = GameConfig.CAT_STATE_ID.STATE_IDLE;
		// 貓的遊玩次數
		// 超過一定次數後就會進入睡眠狀態，並且重設為0
		public int BattleCount;
		// 貓想玩的玩具
		// 每次觸發想玩狀態時會決定一個值(0 or 1)
		// 視覺端用這個值來顯示道具
		// 注意：0與1代表的是那個貓所屬想玩道具的第幾個，所以用時要另做轉換
		public string WannaPlayToyItemKey;
		// 摸貓的次數
		// 超過一定次數後就會進入生氣狀態，並且重設為0
		public int TouchCount;
		// 狀態的結束時間
		// 若目前的狀態是有時效性的，就會設定這個值
		// 以固定的頻率判斷現在的時間是不是超過這個時間。若超過，狀態轉為IDEL
		public long TimeOfEnd;

		public bool AddHp (int hp)
		{
			Hp += hp;
			if (Hp > MaxHp) {
				Hp = MaxHp;
			}
			return Hp >= MaxHp;
		}
	}

	public class AnimationResult{
		public int addExp;
		public bool isAngry;
		public bool isAwake;
		public bool isLevelUp;

		public int reduceTime;
		public bool isEndOfTime;

		public int addHp;
		public bool isFullHp;

		public int feedItemIdx = -1;
		public int wannaPlayItemIdx = -1;
	}
}

