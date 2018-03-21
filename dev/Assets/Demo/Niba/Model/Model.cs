using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HanUtil;
using Common;
using System.Linq;
using System.IO;
using System.Threading;
using HanRPGAPI;

namespace Model
{
	public class Model : MonoBehaviour, IModel
	{
		public HandleDebug debug;
		MapDataStore mapData = new MapDataStore();
		PlayerDataStore playerData = new PlayerDataStore();
		UserSettings user = new UserSettings();

		public void NewGame(){
			ClearMoveResult ();
			mapData = new MapDataStore();
			playerData = new PlayerDataStore();
			playerData.player.basicAbility.vit = 100;
			playerData.AddItem (new Item () {
				prototype = ConfigItem.ID_woodBoat,
				count = 1
			}, Place.Storage);
			/*
			playerData.player.AddExp (ConfigAbility.ID_karate, 5);
			playerData.player.AddExp (ConfigAbility.ID_tailor, 1);
			*/
			RequestSaveMap ();
			RequestSavePlayer ();
			RequestSaveUserSettings ();
		}

		public bool LoadGame(){
			return Load ();
		}

		public void NewMap(MapType type){
			mapData.GenMapStart(type);
		}

		public IEnumerable<string> CheckMissionNotification(){
			var ret = new List<string> ();
			var missions = AvailableNpcMissions.Where(m=>{
				return user.IsMark("MissionNotify_"+m) == false;
			});
			ret.AddRange (missions);
			return ret;
		}

		public void MarkMissionNotification(string mid){
			user.Mark ("MissionNotify_"+mid);
			RequestSaveUserSettings ();
		}

		public void EnterMap (){
			ClearMoveResult ();
			// 重設位置
			playerData.playerInMap.position = Position.Zero;
			// 先探明初始視野
			playerData.ClearVisibleMapObjects ();
			playerData.VisitPosition (playerData.playerInMap.position);
			// 清除地圖
			mapData.ClearMap();
			// 自動生成視野內的地圖
			mapData.GenMapWithPlayerVisible (playerData);
			// 進入地圖
			playerData.EnterMap (mapData);
			RequestSavePlayer ();
			RequestSaveMap ();
		}
		public void ExitMap (){
			playerData.ExitMap ();
			RequestSavePlayer ();
		}
		public List<MapObject> MapObjects{ get{ return mapData.mapObjects; } }
		public List<ResourceInfo> ResourceInfos{ get { return mapData.resourceInfo; } }
		public List<MonsterInfo> MonsterInfos{ get { return mapData.monsterInfo; } }
		public IEnumerable<MapObject> VisibleMapObjects{ get { return playerData.VisibleMapObjects(mapData); } }
		public IEnumerable<MapObject> MapObjectsAt (Position pos){
			return mapData.FindObjects (pos);
		}
		public MapPlayer GetMapPlayer (Place place){
			return playerData.GetMapPlayer (place);
		}
		public IEnumerable<Description> Works{ get { return mapData.GetWorks (playerData); } }

		IEnumerable<Description> workResult;
		public IEnumerable<Description> WorkResults{ get{ return workResult; } }

		public void StartWork (Description work){
			mapData.StartWork (playerData, work);
			RequestSaveMap ();
		}
		public void CancelWork (){
			mapData.CancelWork (playerData);
			RequestSaveMap ();
		}
		public void ApplyWork(){
			workResult = mapData.ApplyWork (playerData);
			RequestSaveMap ();
			RequestSavePlayer ();
		}

		MoveResult tempMoveResult;
		bool hasMoveResult;

		public void MoveUp(){
			Position p;
			p.x = 0; p.y = -1;
			Move (p);
		}
		public void MoveDown(){
			Position p;
			p.x = 0; p.y = 1;
			Move (p);
		}
		public void MoveLeft(){
			Position p;
			p.x = -1; p.y = 0;
			Move (p);
		}
		public void MoveRight(){
			Position p;
			p.x = 1; p.y = 0;
			Move (p);
		}
		public MoveResult MoveResult{ 
			get{
				if (hasMoveResult == false) {
					throw new UnityException ("沒有move result");
				}
				return tempMoveResult;
			} 
		}
		public void ClearMoveResult(){
			hasMoveResult = false;
		}

		public void ApplyMoveResult(){
			if (tempMoveResult.HasEvent) {
				mapData.ApplyEvents (playerData, tempMoveResult.events);
			}
		}

		public void AddItemToStorage(Item item, Place who){
			playerData.AddItem (item, who);
			RequestSavePlayer ();
		}

		public void MoveItem(Place a, Place b, Item item){
			playerData.MoveItem (a, b, item);
			RequestSavePlayer ();
		}

		public int IsCanFusion (string prototype, Place who){
			return playerData.IsCanFusion (prototype, who);
		}

		public void Fusion (Item item, Place who){
			playerData.Fusion (item, who);
			RequestSavePlayer ();
		}

		public void EquipWeapon (Item item, Place whosWeapon, Place whosStorage){
			playerData.EquipWeapon (item, whosWeapon, whosStorage);
			RequestSavePlayer ();
		}
		public void UnequipWeapon (Item item, Place whosWeapon, Place whosStorage){
			playerData.UnequipWeapon (item, whosWeapon, whosStorage);
			RequestSavePlayer ();
		}
		public void ClearStorage(Place who){
			if (who == Place.Pocket) {
				playerData.player.storage.Clear ();
			} else if (who == Place.Map) {
				playerData.playerInMap.storage.Clear ();
			} else {
				playerData.playerInStorage.storage.Clear ();
			}
		}
		public IEnumerable<string> AvailableNpcMissions {
			get {
				return playerData.AvailableNpcMissions;
			}
		}
		public IEnumerable<string> AvailableSkills(Place who){
			return Helper.AvailableSkills (playerData, who).Select(cfg=>cfg.ID);
		}
		public void AcceptMission(string id){
			playerData.AcceptMission (id);
			RequestSavePlayer ();
		}
		public List<string> CheckMissionStatus(){
			return playerData.CheckMissionStatus ();
		}
		public IEnumerable<AbstractItem> CompleteMission (string id){
			var ret = playerData.CompleteMission (id);
			RequestSavePlayer ();
			return ret;
		}
		public void ClearMissionStatus(){
			playerData.ClearMissionStatus ();
		}

		public void EquipSkill (Place who, string skillId){
			playerData.EquipSkill (who, skillId);
			RequestSavePlayer ();
		}

		public void UnequipSkill (Place who, string skillId){
			playerData.UnequipSkill (who, skillId);
			RequestSavePlayer ();
		}

		BasicAbility tmpBasic;
		FightAbility tmpFight;

		public BasicAbility PlayerBasicAbility(Place who){
			Helper.CalcAbility (playerData, mapData, who, ref tmpBasic, ref tmpFight);
			return tmpBasic;
		}

		public FightAbility PlayerFightAbility(Place who){
			Helper.CalcAbility (playerData, mapData, who, ref tmpBasic, ref tmpFight);
			return tmpFight;
		}

		public IEnumerable<Item> CanFusionItems{ 
			get { 
				var ret = new List<Item> ();
				Item item;
				item.count = 1;
				for (var i = 0; i < ConfigItem.ID_COUNT; ++i) {
					var cfg = ConfigItem.Get (i);
					if (cfg.FusionRequire == null) {
						continue;
					}
					item.prototype = ConfigItem.Get (i).ID;
					ret.Add (item);
				}
				return ret;
			} 
		}

		public PlayState PlayState{ 
			get{
				return playerData.playState;
			}
		}

		void Move(Position offset){
			if (hasMoveResult) {
				throw new Exception ("必須先處理之前的move result並且呼叫ClearMoveResult");
			}
			var isPositionDirty = false;
			var isMapDirty = false;
			tempMoveResult = playerData.Move (mapData, offset, ref isMapDirty, ref isPositionDirty);
			hasMoveResult = true;
			// 有更動就儲存
			if (isPositionDirty) {
				RequestSavePlayer ();
			}
			if (isMapDirty) {
				RequestSaveMap ();
			}
		}

		#region persistent
		bool Load(){
			var persistentDataPath = Application.persistentDataPath;

			var playerPath = persistentDataPath + "/playerData.json";
			if (File.Exists (playerPath) == false) {
				return false;
			} else {
				var playerMemoto = File.ReadAllText (playerPath);
				playerData = PlayerDataStore.FromMemonto (playerMemoto);
			}

			var mapPath = persistentDataPath + "/mapData.json";
			if (File.Exists (mapPath) == false) {
				return false;
			} else {
				var mapMemoto = File.ReadAllText (mapPath);
				mapData = MapDataStore.FromMemonto (mapMemoto);
			}

			var userSettingsPath = persistentDataPath + "/userSettings.json";
			// userSettings不存在的話沒差
			if (File.Exists (userSettingsPath) == false) {
				// ignore
			} else {
				var userMemoto = File.ReadAllText (userSettingsPath);
				user = UserSettings.FromMemonto (userMemoto);	
			}
			return true;
		}
		HashSet<string> saveTargets = new HashSet<string>();
		void RequestSavePlayer(){
			SavePlayerDiskWorker (Application.persistentDataPath);
			saveTargets.Add ("player");
			lock (saveTargets) {
				Monitor.PulseAll (saveTargets);
			}
		}
		void RequestSaveMap(){
			SavePlayerDiskWorker (Application.persistentDataPath);
			saveTargets.Add ("map");
			lock (saveTargets) {
				Monitor.PulseAll (saveTargets);
			}
		}
		void RequestSaveUserSettings(){
			SavePlayerDiskWorker (Application.persistentDataPath);
			saveTargets.Add ("user");
			lock (saveTargets) {
				Monitor.PulseAll (saveTargets);
			}
		}
		Thread savingThread;
		void SavePlayerDiskWorker(string persistentDataPath){
			if (savingThread != null) {
				return;
			}
			savingThread = new Thread (() => {
				while(true){
					if(saveTargets.Contains("user")){
						Debug.LogWarning("save user settings...");
						var memonto = playerData.GetMemonto ();
						var path = persistentDataPath + "/userSettings.json";
						File.WriteAllText(path, memonto);
						saveTargets.Remove("user");
					}
					if(saveTargets.Contains("player")){
						Debug.LogWarning("save player...");
						var memonto = playerData.GetMemonto ();
						var path = persistentDataPath + "/playerData.json";
						File.WriteAllText(path, memonto);
						saveTargets.Remove("player");
					}
					if(saveTargets.Contains("map")){
						Debug.LogWarning("save map...");
						var memonto = mapData.GetMemonto ();
						var path = persistentDataPath + "/mapData.json";
						File.WriteAllText(path, memonto);
						saveTargets.Remove("map");
					}
					lock (saveTargets) {
						if(saveTargets.Count == 0){
							Debug.LogWarning("waiting for save...");
							Monitor.Wait(saveTargets);
						}
					}
				}
			});
			savingThread.Start ();
		}
		#endregion
	}
}

