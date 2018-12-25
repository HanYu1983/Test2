using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace Remix
{
	public class GameRecordModel : MonoBehaviour, IModel
	{
		public GameRecord record;
		public DeviceData deviceData;

		public void Load(){
			record.Load ();
		}
		public void Clear(){
			record.Clear ();
		}
		public void Override (string json){
			record.Clear ();
			record.Override (json);
			Save ();
		}
		public void Save(){
			if (deviceData.DeviceID == "") {
				Util.Instance.LogWarning ("還沒設定deviceId，無法備份");
			}
			record.RequestSaveThreading (deviceData.DeviceID);
			record.RequestSaveRemote (deviceData.DeviceID);
		}

		public bool IsMarkRead(string key){
			return record.IsUnlockIAP (key);
		}

		public void MarkRead(string key){
			var isDirty = record.UnlockIAP (key);
			if (isDirty) {
				Save ();
			}
		}

		public float AudioOffsetTime{
			get{ return record.AudioOffsetTime; } 
			set{ 
				record.AudioOffsetTime = value;
				Save ();
			}
		}

		public int Money{
			get{ return record.Money; }
			set{ 
				record.Money = value;
				Save ();
			}
		}
		public int Gold{
			get{ return record.Gold; }
			set{ 
				record.Gold = value;
				Save ();
			}
		}

		public IEnumerable<ItemKey> OwnedCats{ get{ return record.GetOwnedCats (); } }
		public ItemKey SelectCatKey{
			get{ return record.GetSelectCat(); }
			set{ 
				record.SetSelectCat (value); 
				Save ();
			}
		}
		public Cat GetCat(ItemKey catId){
			return record.GetCat (catId);
		}
		public bool EnableCat (ItemKey catId){
			var ok = record.EnableCat (catId);
			if (ok) {
				Save ();
			}
			return ok;
		}
		public bool AddCatExp (ItemKey catId, int exp){
			var isLevelUp = record.AddCatExp (GetCat(catId), exp);
			Save ();
			return isLevelUp;
		}
		public void CatEnterGame (ItemKey catId){
			record.CatEnterGame (catId);
		}
		public void CatExitGame(ItemKey catId){
			record.CatEndGame (catId);
		}
		public bool IsCatCanEnterGame(ItemKey catId){
			return record.IsCanEnterGame (catId);
		}
		public bool IsCanTouch (ItemKey catId){
			return record.IsCanTouch(catId);
		}
		public void UpdateCatState(Action<ItemKey> OnCatWannaPlay, Action<ItemKey> OnCatSleepOver, Action<ItemKey> OnCatAngryOver){
			var dirty = false;
			dirty |= record.UpdateWannaPlayTime (OnCatWannaPlay);
			dirty |= record.UpdateSleepTime (OnCatSleepOver);
			dirty |= record.UpdateAngryTime (OnCatAngryOver);
			if (dirty) {
				Save ();
			}
		}
		public AnimationResult TouchCat(ItemKey catId){
			var anim = record.SetTouchCat (catId);
			Save ();
			return anim;
		}
		public AnimationResult UseSToyForCat (ItemKey catId, ItemKey itemId){
			var anim = record.UseSToyForCat (catId, itemId);
			Save ();
			return anim;
		}
		public AnimationResult UseFoodForCat (ItemKey catId, ItemKey itemId){
			var anim = record.UseFoodForCat (catId, itemId);
			Save ();
			return anim;
		}
		public AnimationResult UseToyForCat (ItemKey catId, ItemKey itemId){
			var anim = record.UseToyForCat (catId, itemId);
			Save ();
			return anim;
		}

		public bool IsPhotoEnable (PhotoKey key){
			return record.GetEnablePhotoKey ().Any (p => {
				return p.StringKey == key.StringKey;
			});
		}
		public void EnablePhoto (PhotoKey key){
			record.EnablePhoto (key);
			Save ();
		}

		public int ItemCount (ItemKey key){
			return record.QueryItemCount (key);
		}
		public bool ConsumeItem (ItemKey key){
			var ok = record.ConsumeItem (key);
			Save ();
			return ok;
		}

		bool HandleItemForTypeMGMM(ItemKey key, int count){
			switch (key.Type) {
			case StoreCtrl.DATA_MG:
			case StoreCtrl.DATA_MM:
				{
					var def = ItemDef.Get (key.Idx);
					Money += -(def.Money * count);
					Gold += -(def.Gold * count);
					return true;
				}
			}
			return false;
		}

		public void AddItem(ItemKey key, int count){
			// MG/MM類型的不要加到道具裡，直接使用掉
			if (HandleItemForTypeMGMM (key, count)) {
				Util.Instance.LogWarning ("MG道具，直接使用掉");
				return;
			}
			record.AddItem (key, count);
			Save ();
		}

		public Capture Capture (CaptureKey key){
			return record.GetCapture(key);
		}
		public bool CheckCaptureCompleted(){
			var dirty = record.CheckCaptureCompleted ();
			if (dirty) {
				Save ();
			}
			return dirty;
		}
		public void StartCapture (CaptureKey key, ItemKey item){
			record.StartCapture(key, item);
			Save ();
		}
		public bool SpeedUpCaptureWithItem(CaptureKey key, ItemKey item){
			var ok = record.SpeedUpCaptureWithItem (key, item);
			if (ok) {
				Save ();
			}
			return ok;
		}
		public void CompletedCapture(CaptureKey key){
			if (record.CompletedCapture (key)) {
				Save ();
			}
		}
		public void ClearCapture(CaptureKey key){
			record.ClearCapture (key);
			Save ();
		}
		public IEnumerable<string> EnableCaptures{ get{ return record.EnableCaptures; }}

		public void EnableCapture(CaptureKey key){
			record.EnableCapture (key);
			Save ();
		}
		public bool IsCaptureEnable(CaptureKey key){
			return record.IsCaptureEnable (key);
		}

		public string GetCurrentMapIdx(){
			return record.ComputeCurrentMap ();
		}

		public IEnumerable<LevelKey> GetCurrentLevel(string mapIdx) { 
			return record.ComputeCurrentLevel (mapIdx);
		}
		public void LevelClear (LevelKey level){
			record.PassLevel (level);
			Save ();
		}

		public bool IsAllLevelDifficultyClear(string mapIdx, int difficulty){
			var clearLvs = 
				from lv in from k in record.PassLevels
				           select new LevelKey (k)
				where lv.MapIdx == mapIdx && lv.Difficulty == difficulty
				select lv;
			var mapId = GameConfig.MapIdx2Int (mapIdx);
			var levelCount = GameConfig.MAP_LEVEL_COUNT [mapId];
			return clearLvs.Count () == levelCount;
		}

		public void DebugCommand (string cmd){
			switch (cmd) {
			case "ForceSetPlayerWinnaPlayTime":
				record.ForceSetWinnaPlay ();
				break;
			}
		}

		public GameRecord.PassLevelInfo NewPassLevelInfo(LevelKey key){
			return record.NewPassLevelInfo (key);
		}

		public void AddPassLevelInfo(GameRecord.PassLevelInfo info){
			record.AddPassLevelInfo (info);
			Save ();
		}
		public GameRecord.PassLevelInfo GetMaxRankLevelInfo(LevelKey key){
			return record.GetMaxRankLevelInfo (key);
		}

		public IEnumerable<ItemKey> GetTodayGift(){
			var gift = record.GetTodayGiftAndRecordIt ();
			Save ();
			return gift;
		}
		public bool IsAlreadyGetGift(long time){
			return record.IsAlreadyGetGift (time);
		}
		public int GetTopGiftId(){
			return record.GetTopGiftId ();
		}

		public bool IsUnlockBannerAd{ 
			get{
				return record.IsUnlockIAP (GameConfig.UNLOCK_BANNER_IAP_SKU);
			}
			set{
				if (value == true) {
					record.UnlockIAP (GameConfig.UNLOCK_BANNER_IAP_SKU);
					Save ();
				} else {
					throw new UnityException ("無法把已解鎖的重設:IsUnlockBannerAd = false");
				}
			}
		}
	}
}

