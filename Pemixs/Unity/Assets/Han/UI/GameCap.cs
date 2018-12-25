using System;
using UnityEngine;
using System.Collections.Generic;

namespace Remix
{
	public class GameCap : MonoBehaviour
	{
		public List<GameObject> posList;
		public List<GameObject> imgLock;

		public RectPosition rectPosition;
		public RectPosition RectPosition{ get { return rectPosition; } }

		public string posNodeLayoutPrefabPath;
		public Vector3 PosNodeLayoutOffset;

		public string mapIdx;
		public string MapIdx{ 
			get { 
				if (mapIdx == null) {
					throw new UnityException ("請先呼叫PrepareMapNodeCtrl");
				}
				return mapIdx; 
			}
		}

		public List<PosNodeCtrl> posNodeArray = new List<PosNodeCtrl>();

		Func<NodeKey, Capture> QueryCapture;

		public void SetPlatformEnable(NodeKey posIdx, bool enable){
			if (posIdx.MapIdx != mapIdx) {
				Debug.LogWarning ("地區不正確:"+posIdx.StringKey);
				return;
			}
			var i = posIdx.Idx;
			// 允許null值
			if (imgLock [i] != null) {
				imgLock [i].SetActive (enable == false);
			}
			var posNode = GetPosNode (posIdx);
			posNode.gameObject.SetActive (enable);
			posNode.IsPlayAreaEnable = enable;
			posNode.IsUnlockAreaEanble = enable == false;
		}

		public PosNodeCtrl GetPosNode(NodeKey posIdx){
			if (posNodeArray == null) {
				throw new UnityException ("請先呼叫PrepareMapNodeCtrl");
			}
			foreach (var p in posNodeArray) {
				if (p.NodeId == posIdx.StringKey) {
					return p;
				}
			}
			throw new UnityException ("沒有這個探索點:"+posIdx);
		}

		public void PrepareMapNodeCtrl(string mapIdx, Func<NodeKey, Capture> QueryCapture){
			if (posNodeArray.Count > 0) {
				Debug.LogWarning ("已經建立節點，直接回傳");
				return;
			}
			this.mapIdx = mapIdx;

			int size = posList.Count;
			var PosNodeLayout = Util.Instance.LoadAsset (posNodeLayoutPrefabPath, typeof(UnityEngine.GameObject));

			for (int i=0;i<size;++i)
			{
				var pos = posList [i];
				BoxCollider2D node = pos.GetComponent<BoxCollider2D> ();

				var nodeKey = new NodeKey (){ 
					Group = NodeKey.GroupGameCap, 
					MapIdx = mapIdx, 
					Idx = i 
				};

				ColliderMouseUpTrigger trigger = node.gameObject.AddComponent<ColliderMouseUpTrigger>();
				trigger.command = "CaptureUIPOS_unlockNode_"+nodeKey.StringKey;

				GameObject nodeAnchorObj = pos;

				GameObject nodeObj = GameObject.Instantiate (PosNodeLayout) as GameObject;

				nodeObj.transform.SetParent (nodeAnchorObj.transform, false);
				nodeObj.transform.localPosition = PosNodeLayoutOffset;

				var trigger2 = nodeObj.gameObject.AddComponent<ColliderMouseUpTrigger> ();
				trigger2.command = "CaptureUIPOS_node_" + nodeKey.StringKey;

				PosNodeCtrl nodeCtrl = nodeObj.GetComponent<PosNodeCtrl> ();
				nodeCtrl.btnItem.command = "CaptureUISpeedUp_node_" + nodeKey.StringKey;
				nodeCtrl.btnOK.command = "CaptureUIOK_node_" + nodeKey.StringKey;

				nodeCtrl.Initialize (nodeKey.StringKey);
				posNodeArray.Add (nodeCtrl);
			}
			this.QueryCapture = QueryCapture;
		}

		public void UpdateUI(){
			for (var i = 0; i < posNodeArray.Count; ++i) {
				var nodeCtrl = posNodeArray [i];
				var nodeKey = new NodeKey (nodeCtrl.NodeId);
				var captureKey = nodeKey.CaptureKey;
				var capture = QueryCapture (nodeKey);
				var offsetTime = capture.GetCaptureTimeOffset ();
				switch (capture.State) {
				case GameConfig.CAPTURE_STATE.PENDING:
					nodeCtrl.Clear ();
					break;
				case GameConfig.CAPTURE_STATE.NORMAL:
					nodeCtrl.IsPlayAreaEnable = true;
					nodeCtrl.Clear ();
					break;
				case GameConfig.CAPTURE_STATE.CAPTURE:
					nodeCtrl.IsPlayAreaEnable = false;
					nodeCtrl.SetCapture (new ItemKey (capture.ItemKey));
					nodeCtrl.UpdateTimeText (offsetTime);
					break;
				case GameConfig.CAPTURE_STATE.COMPLETED:
					nodeCtrl.IsPlayAreaEnable = false;
					nodeCtrl.CaptureEnd ();
					if (capture.IsShouldGetCat) {
						nodeCtrl.SetCat (capture.GetGettedCat ());
					} else if (capture.IsShouldGetPhoto) {
						nodeCtrl.ShowPhoto ();
					}
					break;
				}
			}
		}
	}
}

