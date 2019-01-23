using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Remix
{
	public class GamePlayModelControlHelper : MonoBehaviour {
		public GamePlayView view;
		public GamePlayModel model;

		void Awake(){
			view = GetComponent<GamePlayView> ();
			model = GetComponent<GamePlayModel> ();
		}

		#region auto sound effect
		public bool useAutoSoundEffect = true;
		public float audioSoundTimeOffset = 0.15f;

		public bool IsUseAutoSoundEffect{
			get{
				return useAutoSoundEffect;
			}
		}

		public void OnAutoSoundEffectStep(float timer, Action<int> callback){
			AppendTimeForComputeBeat (3, timer + audioSoundTimeOffset, callback);
		}

		public void OnAutoSoundEffect(float timer, int pos = GamePlayView.LeftPos){
			timer = timer + audioSoundTimeOffset;
			var turn = 0;
			var count = 0.0f;
			BeatTool.Time2TurnAndCount (16, RhythmCtrl.HALF_BEAT_TIME, timer, ref turn, ref count);

			if (turn >= 0 && turn < RhythmCtrl.MAX_TURN) {
				var idxAry = RhythmCtrl.RHYTHM_SOUND_IDX_ARRAY;
				var typeAry = RhythmCtrl.RHYTHM_SOUND_TYPE_ARRAY;
				var mashAry = RhythmCtrl.RHYTHM_SOUND_BTNMASH_ARRAY;
				try {
					var playIdx = 0;
					var clickType = Game.ClickType.Pending;
					Game.GetRhythmIndex (idxAry, mashAry, turn, (int)count, ref playIdx, ref clickType);
					switch(clickType){
					case Game.ClickType.Single:
						{
							if(playIdx != 0){
								var type = typeAry [turn] [(int)count];
								view.PlayCatSound (pos, playIdx, type);
							}
						}
						break;
					case Game.ClickType.Long:
						{
							var startCnt = 0;
							var endCnt = 0;
							Game.ComputeRhythmCountInLongClickSection (idxAry, turn, (int)count, ref startCnt, ref endCnt);
							var type = typeAry [turn] [startCnt];
							// 5的話要5和6輪播
							if(playIdx == 5){
								var offset = (int)count - startCnt;
								playIdx = offset % 2 == 0 ? 5 : 6;
							}
							view.PlayCatSound (pos, playIdx, type);
						}
						break;
					}
				} catch (IndexOutOfRangeException e) {
					Debug.LogError ("回合拍數不合法，請檢查" + turn + "/" + count);
					throw e;
				}
			}
		}
		#endregion

		#region beat
		List<int> beatStore = new List<int>(){-1,-1,-1,-1};
		public void AppendTimeForComputeBeat(int slot, float timer, Action<int> OnBeatFn){
			var currBeat = (int)Mathf.Floor (timer / RhythmCtrl.HALF_BEAT_TIME);
			if (beatStore[slot] != currBeat) {
				beatStore[slot] = currBeat;
				if (OnBeatFn != null) {
					OnBeatFn (beatStore[slot]);
				}
			}
		}
		#endregion

		#region sound
		// 記住最後打擊成功的旋律Type
		char lastRhythmType = '\0';
		public void AppendRhythmType(int turn, int count){
			var type = RhythmCtrl.RHYTHM_TYPE_ARRAY [turn] [count];
			lastRhythmType = type;
		}
		public void PlayCatSound(int clickIdx){
			if (clickIdx >= SoundDataCtrl.ATK_SOUND) {
				// 超過或等於SoundDataCtrl.ATK_SOUND是指定音效，rhythmType必須是0
				view.PlayCatSound (GamePlayView.LeftPos, clickIdx, '\0');
			} else {
				view.PlayCatSound (GamePlayView.LeftPos, clickIdx, lastRhythmType);
			}
		}
		#endregion

		#region auto play
		public bool isAutoPlay;
		Dictionary<int,string> playIdx2Cmd = new Dictionary<int,string> {
			{1, "Btn01"},
			{2, "Btn02"},
			{3, "Btn03"},
			{4, "Btn04"},
			{5, "Btn05-1"},
			{6, "Btn05-2"}
		};
		public bool IsAutoPlay{
			get{ return isAutoPlay; }
			set{ isAutoPlay = value; }
		}
		public delegate void TriggerCommand (string cmd);

		public void AutoClickHintVer2(float timer, Action<string> fn){
			var checkTime = 0f;
			var distToCenter = 0f;
			if(Game.CheckTimeDist(RhythmCtrl.RHYTHM_IDX_ARRAY, RhythmCtrl.RHYTHM_BTNMASH_ARRAY, 16, RhythmCtrl.HALF_BEAT_TIME, timer, ref checkTime, ref distToCenter)){
				var turn = 0;
				var count = 0f;
				BeatTool.Time2TurnAndCount (16, RhythmCtrl.HALF_BEAT_TIME, timer + RhythmCtrl.HALF_BEAT_TIME/2, ref turn, ref count);
				if (count < 0 || turn >= 2) {
					return;
				}
				var playIdx = 0;
				var clickType = Game.ClickType.Pending;
				var perfectDist = RhythmCtrl.HALF_BEAT_TIME * 0.125;
				Game.GetRhythmIndex (RhythmCtrl.RHYTHM_IDX_ARRAY, RhythmCtrl.RHYTHM_BTNMASH_ARRAY, turn, (int)Mathf.Floor(count), ref playIdx, ref clickType);
				if (playIdx2Cmd.ContainsKey (playIdx)) {
					// 盡可能的按下，不然會有沒按到的可能
					if (distToCenter < perfectDist) {
						var cmd = playIdx2Cmd [playIdx];
						fn (cmd);
						return;
					}
				}
			}
		}
		#endregion
	}
}
