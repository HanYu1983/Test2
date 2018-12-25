using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Remix{
	public class PosNodeCtrl : MonoBehaviour
	{
		public GameObject itemAnchorObj;
		public GameObject catAnchorObj;
		public GameObject photoSprite;
		public GameObject boxSprite;
		public ColliderMouseUpTrigger btnOK;
		public ColliderMouseUpTrigger btnItem;

		public string nodeIdx;
		public Text textTime;
		public int countDownTime = -1;
		public BoxCollider2D unlockArea, playArea;

		public string NodeId{ get { return nodeIdx; } }

		public void SetEnable(bool enable)
		{
			/*
			Collider2D collider = GetComponent<Collider2D>();
			collider.enabled = enable;
			*/
		}

		public Vector3 PositionInRoot{
			get{
				var parent = transform.parent.gameObject;
				if (parent.name.Contains ("Pos") == false) {
					throw new UnityException ("結構不正確，無法計算正確位置");
				}
				return parent.transform.localPosition;
			}
		}

		public void Clear(){
			// 沒有在探索
			SetEnable(true);
			textTime.gameObject.SetActive(false);
			btnOK.gameObject.SetActive(false);
			btnItem.gameObject.SetActive(false);
			catAnchorObj.SetActive (false);
			photoSprite.SetActive (false);
			ClearItem ();
			if (catObj != null) {
				GameObject.DestroyObject(catObj);
				catObj = null;
			}
			boxSprite.SetActive (true);
		}

		void ClearItem(){
			if (imageObj != null) {
				GameObject.DestroyObject(imageObj);
				imageObj = null;
			}
		}

		public bool IsUnlockAreaEanble{
			get{
				return unlockArea.enabled;
			}
			set{
				unlockArea.enabled = value;
			}
		}

		public bool IsPlayAreaEnable{
			get{
				return playArea.enabled;
			}
			set{
				playArea.enabled = value;
			}
		}

		BoxCollider2D CreatePlayArea(){
			// 轉蛋點擊區hard code在這
			BoxCollider2D myCollider = this.gameObject.AddComponent<BoxCollider2D>();
			myCollider.offset = new Vector2 (204f,-102f);
			myCollider.size = new Vector2 (431f,324f);
			return myCollider;
		}

		public void Initialize(string nodeIdx)
		{
			unlockArea = this.gameObject.GetComponentInParent<BoxCollider2D>();
			playArea = CreatePlayArea ();
			// 關閉unlock點擊區
			IsUnlockAreaEanble = true;
			IsPlayAreaEnable = false;
			this.textTime = this.GetComponentInChildren<Text>();
			this.nodeIdx = nodeIdx;
		}
		// 探索倒計結束
		// 這會將勾勾按鈕打開並決定探索的結果
		public void CaptureEnd()
		{
			SetEnable(false);
			textTime.gameObject.SetActive(true);
			UpdateTimeText (0);
			btnOK.gameObject.SetActive(true);
			btnOK.SetEnable(true);
			btnItem.gameObject.SetActive(false);
			ClearItem ();
		}

		public void ShowPhoto(){
			photoSprite.SetActive(true);
		}

		GameObject imageObj;
		public void SetCapture(ItemKey itemKey)
		{
			btnItem.gameObject.SetActive(true);
			btnItem.SetEnable(true);
			btnOK.gameObject.SetActive(false);
			SetEnable(false);
			textTime.gameObject.SetActive(true);
			if (imageObj != null) {
				return;
			}
			imageObj = Util.Instance.GetPrefab(itemKey.BuyItemPrefabName, itemAnchorObj);
			imageObj.transform.localScale = new Vector3 (0.01f, 0.01f, 1f);
		}

		public void UpdateTimeText(int offsetTime){
			int minute = offsetTime / 60;
			int second = offsetTime % 60;
			textTime.text = string.Format ("{0:00}:{1:00}", minute, second);
		}

		GameObject catObj;
		public void SetCat(ItemKey cat){
			if (catObj != null) {
				return;
			}
			catObj = Util.Instance.GetPrefab(cat.CatBSCPrefabName, catAnchorObj);
			catObj.transform.localScale = new Vector3 (1.12f, 1.12f, 1f);
			catAnchorObj.SetActive (true);
			boxSprite.SetActive (false);
		}
	}
}