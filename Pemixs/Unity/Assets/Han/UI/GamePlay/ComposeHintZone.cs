using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Remix
{
	public class ComposeHintZone : MonoBehaviour, IHintZone
	{
		public List<MonoBehaviour> hintZones;
		public float TimePerBeat{ 
			get{
				throw new UnityException ("ComposeHintZone不能使用Getter");
			}
			set{ 
				foreach (var hint in hintZones) {
					var obj = hint as IHintZone;
					if (obj == null) {
						return;
					}
					obj.TimePerBeat = value;
				}
			}
		}
		public int BeatCntPerTurn{ 
			get{
				throw new UnityException ("ComposeHintZone不能使用Getter");
			}
			set{ 
				foreach (var hint in hintZones) {
					var obj = hint as IHintZone;
					if (obj == null) {
						return;
					}
					obj.BeatCntPerTurn = value;
				}
			}
		}
		public int TurnCntPerLevel{ 
			get{
				throw new UnityException ("ComposeHintZone不能使用Getter");
			}
			set{ 
				foreach (var hint in hintZones) {
					var obj = hint as IHintZone;
					if (obj == null) {
						return;
					}
					obj.TurnCntPerLevel = value;
				}
			}
		}
		public float StartTime{
			get{
				throw new UnityException ("ComposeHintZone不能使用Getter");
			}
			set{ 
				foreach (var hint in hintZones) {
					var obj = hint as IHintZone;
					if (obj == null) {
						return;
					}
					obj.StartTime = value;
				}
			}
		}
		public float HintWidth{
			get{
				throw new UnityException ("ComposeHintZone不能使用Getter");
			}
			set{ 
				foreach (var hint in hintZones) {
					var obj = hint as IHintZone;
					if (obj == null) {
						return;
					}
					obj.HintWidth = value;
				}
			}
		}
		public void InitHintZone(){
			foreach (var hint in hintZones) {
				var obj = hint as IHintZone;
				if (obj == null) {
					return;
				}
				obj.InitHintZone ();
			}
		}
		public void ArrangePos(float offset){
			foreach (var hint in hintZones) {
				var obj = hint as IHintZone;
				if (obj == null) {
					return;
				}
				obj.ArrangePos (offset);
			}
		}
		public void SyncTimer(float timer){
			foreach (var hint in hintZones) {
				var obj = hint as IHintZone;
				if (obj == null) {
					return;
				}
				obj.SyncTimer (timer);
			}
		}
		public void InitHintSprite (int[][] idxAry, int[][] mashAry){
			foreach (var hint in hintZones) {
				var obj = hint as IHintZone;
				if (obj == null) {
					return;
				}
				obj.InitHintSprite (idxAry, mashAry);
			}
		}
		public void HintPlayGood(int hintIdx, int clickIdx, bool isPerfect, bool isFever, Game.ClickType clickType){
			foreach (var hint in hintZones) {
				var obj = hint as IHintZone;
				if (obj == null) {
					return;
				}
				obj.HintPlayGood (hintIdx, clickIdx, isPerfect, isFever, clickType);
			}
		}
		public void HintPlayMiss(int hintIdx, int clickIdx, bool isFever){
			foreach (var hint in hintZones) {
				var obj = hint as IHintZone;
				if (obj == null) {
					return;
				}
				obj.HintPlayMiss (hintIdx, clickIdx, isFever);
			}
		}
		public IEnumerator ShiningHint(){
			throw new UnityException ("");
		}
	}
}

