using System;
using UnityEngine;
using System.Collections.Generic;

namespace Remix
{
	public interface IModel
	{
		void Load();
		void Clear();
		void Save();
		void Override (string json);
		//
		bool IsMarkRead(string key);
		void MarkRead(string key);
		// player
		int Money{ get; set; }
		int Gold{ get; set; }
		float AudioOffsetTime{get; set;}
		// cat
		IEnumerable<ItemKey> OwnedCats{ get; }
		ItemKey SelectCatKey{ get; set;}
		Cat GetCat(ItemKey catId);
		bool EnableCat (ItemKey catId);
		bool AddCatExp (ItemKey catId, int exp);
		void CatEnterGame (ItemKey catId);
		void CatExitGame(ItemKey catId);
		bool IsCatCanEnterGame(ItemKey catId);
		bool IsCanTouch (ItemKey catId);
		void UpdateCatState(Action<ItemKey> OnCatWannaPlay, Action<ItemKey> OnCatSleepOver, Action<ItemKey> OnCatAngryOver);
		AnimationResult TouchCat(ItemKey catId);
		AnimationResult UseSToyForCat (ItemKey catId, ItemKey itemId);
		AnimationResult UseFoodForCat (ItemKey catId, ItemKey itemId);
		AnimationResult UseToyForCat (ItemKey catId, ItemKey itemId);
		// photo
		bool IsPhotoEnable (PhotoKey key);
		void EnablePhoto (PhotoKey key);
		// item
		int ItemCount (ItemKey key);
		bool ConsumeItem (ItemKey key);
		void AddItem(ItemKey key, int count);
		// capture
		Capture Capture (CaptureKey key);
		void StartCapture (CaptureKey key, ItemKey item);
		bool CheckCaptureCompleted();
		bool SpeedUpCaptureWithItem(CaptureKey key, ItemKey item);
		void CompletedCapture(CaptureKey key);
		void ClearCapture(CaptureKey key);
		// unlock capture
		IEnumerable<string> EnableCaptures{ get; }
		void EnableCapture(CaptureKey key);
		bool IsCaptureEnable (CaptureKey key);
		// level
		string GetCurrentMapIdx();
		IEnumerable<LevelKey> GetCurrentLevel(string mapIdx);
		void LevelClear (LevelKey level);
		bool IsAllLevelDifficultyClear(string mapIdx, int difficulty);
		void DebugCommand (string cmd);
		// level info
		GameRecord.PassLevelInfo NewPassLevelInfo(LevelKey key);
		void AddPassLevelInfo(GameRecord.PassLevelInfo info);
		GameRecord.PassLevelInfo GetMaxRankLevelInfo(LevelKey key);
		// daily gift
		IEnumerable<ItemKey> GetTodayGift();
		bool IsAlreadyGetGift(long time);
		int GetTopGiftId ();
		// unlock iap
		bool IsUnlockBannerAd{ get; set; }
	}
}

