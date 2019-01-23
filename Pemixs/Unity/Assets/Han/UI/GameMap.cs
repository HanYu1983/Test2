using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

namespace Remix
{
	public class GameMap : MonoBehaviour
	{
		public Image catIcon;
		public float moveSpeed;

		public WaypointManager waypointMgr;
		public WaypointManager WaypointManager{ get { return waypointMgr; } }

		public RectPosition rectPosition;
		public RectPosition RectPosition{ get { return rectPosition; } }

		public GameObject adIcon;
		public bool IsAdIconVisible{
			set{
				adIcon.SetActive (value);
			}
		}

		public void SetCatImage(Sprite img){
			if (catIcon == null) {
				Debug.LogWarning ("沒有設定貓錨點，無法顯示貓");
				return;
			}
			catIcon.sprite = img;
		}

		public Waypoint JumpCatToWaypoint(string id){
			if (waypointMgr == null) {
				throw new UnityException ("路徑節點還沒編輯");
			}
			waypointMgr.PrepareWaypoint ();
			var p = waypointMgr.GetWaypoint (id);
			catIcon.transform.localPosition = p.transform.localPosition;
			return p;
		}

		Func<float, bool> StepMoveToWaypoint;
		public void PerformStepMoveToWaypoint(float t){
			if (StepMoveToWaypoint != null) {
				if (StepMoveToWaypoint (t)) {
					StepMoveToWaypoint = null;
				}
			}
		}

		public void PrepareStepMoveToWaypoint(Waypoint p, string untilId){
			var nextP = p;
			this.StepMoveToWaypoint = t => {
				nextP = WaypointManager.MoveStep (t, catIcon.transform, nextP, moveSpeed, untilId);
				// 到底了
				if(nextP == null){
					return true;
				}
				return false;
			};
		}

		// 準備AdIcon的可按性。記得要呼叫，不然按了沒有效果
		public void PrepareAdIconButton(){
			if (adIcon == null) {
				Debug.LogWarning ("adIcon未設定");
				return;
			}
			var btn = adIcon.gameObject.AddComponent<ButtonCtrl> ();
			btn.touchable = true;
			btn.command = "MapUIBtnAdicon";
		}
			
		public void PrepareMapNodeCtrl(string mapIdx, int currentLevel, bool bEnablePlay){
			BoxCollider2D[] nodeArray = GetComponentsInChildren<BoxCollider2D>();
			for (int i=0;i<nodeArray.Length;++i)
			{
				BoxCollider2D node = nodeArray[i];
				// 命外為Levelxx的node才是我們要的關卡節點
				// 起始為Level01
				var name = node.name;
				var isLevelPos = name.Contains ("Level");
				if (isLevelPos == false) {
					continue;
				}
				var posIdxStr = name.Substring ("Level".Length);
				var posIdx = System.Convert.ToInt32 (posIdxStr)-1;
				var isAdded = node.gameObject.GetComponent<ColliderMouseUpTrigger> ();
				if (isAdded) {
					Debug.LogWarning ("已經建立節點，直接回傳");
					break;
				}
				var nodeCtrl = node.gameObject.AddComponent<ColliderMouseUpTrigger>();
				nodeCtrl.command = "MapUIGameMapBtn_" + new NodeKey () {
					Group = NodeKey.GroupGameMap, 
					MapIdx = mapIdx, 
					Idx = posIdx
				}.StringKey;
				nodeCtrl.SetEnable(true);
			}
		}
	}
}

