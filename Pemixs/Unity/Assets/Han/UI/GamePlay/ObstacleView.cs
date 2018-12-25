using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Remix
{
	public class ObstacleView : MonoBehaviour, IHintZone
	{
		public string mapIdx;
		public int catIdx;
		public GameObject anchor, frontLayer;
		public Vector3 obstacleFlySpeed;

		public float startPos, endPos, moveTime;
		public List<GameObject> obstacleArray;

		GameObject[] hintArray;

		public void InitHintZone(){
			LoadObstacleObj (mapIdx, catIdx);
		}

		void LoadObstacleObj(string mapIdx, int catIdx){
			obstacleArray = new List<GameObject> ();
			var catKey = new ItemKey (StoreCtrl.DATA_CAT, catIdx);
			for (var i = 0; i < 6; ++i) {
				GameObject prefab = null;
				if (i < 4) {
					prefab = Util.Instance.GetPrefab (catKey.ObstaclePrefabName (mapIdx, i), null);
				} else {
					prefab = Util.Instance.GetPrefab (catKey.ObstaclePrefabName (mapIdx, i), null);
				}
				// 設定父層讓它可以自動隨著父層被刪除
				prefab.transform.SetParent (transform, false);
				prefab.SetActive (false);
				obstacleArray.Add (prefab);
			}

			var count = BeatCntPerTurn * 3;
			hintArray = new GameObject[count];
			for (var i = 0; i < count; ++i) {
				var obj = new GameObject();
				obj.name = "Obstracle_" + (i+1);
				obj.transform.SetParent (anchor.transform, false);
				obj.transform.localPosition = Vector3.zero;
				hintArray [i] = obj;
			}
		}

		void SetObjTo(Transform anchor, bool clear, int objIdx){
			var obj = obstacleArray [objIdx];
			if (obj == null) {
				Debug.LogWarning ("沒有這個obj:"+objIdx);
				return;
			}
			if (clear) {
				if (anchor.childCount > 0) {
					for (var i = anchor.childCount - 1; i >= 0; --i) {
						var c = anchor.GetChild (i);
						GameObject.Destroy (c.gameObject);
					}
				}
			}
			var go = GameObject.Instantiate (obj);
			go.SetActive (true);
			go.transform.SetParent (anchor, false);
			// 有Respawn的，不會被撞飛
			// 注意：tag系統在這款遊戲中沒有用到，所以可以偷懶處理
			if (objIdx == 4) {
				// 注意：要設在父層
				anchor.gameObject.tag = "Respawn";
			} else {
				anchor.gameObject.tag = "Untagged";
			}
		}

		public void ComputeBasicVar(float offset){
			startPos = offset;
			// 移到整條尾部都消失在左邊
			// 位置 = -右邊位置
			endPos = - HintWidth * (TurnCntPerLevel * BeatCntPerTurn + 1);
			moveTime = TimePerBeat * TurnCntPerLevel * BeatCntPerTurn * 2;
		}

		// 呼叫這個方法前記得先呼叫ComputeBasicVar
		public void ResetPos(){
			int count = BeatCntPerTurn * TurnCntPerLevel;
			// 長度 = Hint寬 * 拍數(maybe 8) * turn數(maybe 2)
			// 本來的設計只有1/2拍(hint數16個)
			// 改為1/4拍(hint數有32個)後長度要除2
			float dist = HintWidth/2 * TurnCntPerLevel * BeatCntPerTurn;
			for (int i=0;i<hintArray.Length;++i)
			{
				GameObject obj = hintArray[i];
				Vector3 pos = obj.transform.localPosition;
				pos.x = i * (dist / (count - 1));
				obj.transform.localPosition = pos;
			}
		}


		public float TimePerBeat{ get; set; }
		public int BeatCntPerTurn{ get; set; }
		public int TurnCntPerLevel{ get; set; }
		public float StartTime{ get; set; }
		public float HintWidth{ get; set; }
		public void ArrangePos(float offset){
			ComputeBasicVar (offset);
			ResetPos ();
		}
		public void SyncTimer(float timer){
			float scale = timer / moveTime;
			// 不用內建的Mathf.Lerp
			// 內建的Mathf.Lerp不支援負數的scale
			Vector3 pos = frontLayer.transform.localPosition;
			var offset = endPos - startPos;
			var lerp = offset * scale;
			pos.x = startPos + lerp;
			frontLayer.transform.localPosition = pos;
		}
		public void InitHintSprite (int[][] idxAry, int[][] mashAry){
			int playIdx = 0;
			int lastPlayIdx = 0;
			var lastSpriteIdx = -1;

			for (int i=0;i<hintArray.Length;++i)
			{
				var hint = hintArray [i];
				hint.transform.SetParent(anchor.transform, false);
				hint.SetActive (false);

				// 取得前半端還是後半段
				int turn = i / BeatCntPerTurn;
				// 取得第幾拍
				int idx = (i % BeatCntPerTurn);
				if (turn >= idxAry.Length) {
					// 持續SetSprite(null,...)
					continue;
				}
				playIdx = idxAry[turn][idx];
				if (playIdx == 0)
					continue;

				if (playIdx == 8 || playIdx == 7)
				{
					hint.transform.SetParent(frontLayer.transform);
					hint.SetActive (true);

					int spriteIdx = lastPlayIdx - 1;
					var isLoopAnim = lastPlayIdx == 5 || lastPlayIdx == 6;
					if (isLoopAnim) {
						// 處理第一個
						if (lastSpriteIdx == -1) {
							// 先等於上一個，可能是4或5
							lastSpriteIdx = spriteIdx;
						}
						// 判斷現在的
						spriteIdx = lastSpriteIdx == 4 ? 5 : 4;
						lastSpriteIdx = spriteIdx;
					}
					SetObjTo (hint.transform, true, spriteIdx);
					/*
					Sprite obstacleSprite = spriteDataCtrl.spriteArray[spriteIdx];
					float offsetY = spriteDataCtrl.offsetYArray[spriteIdx];

					spriteRender.sprite = obstacleSprite;
					spriteRender.enabled = true;

					var pos = hint.transform.localPosition;
					pos.y = offsetY;
					// 編號4要在背景
					// z值 = [前景,貓,背景] = [-1,0,1]
					if (spriteIdx == 4) {
						pos.z = 0;
					} else {
						pos.z = -1;
					}
					hint.transform.localPosition = pos;
					*/
				}
				else
				{
					// 重設為-1讓LoopAnim重新計算
					lastSpriteIdx = -1;

					hint.transform.SetParent(frontLayer.transform);
					hint.SetActive (true);
					int spriteIdx = playIdx - 1;

					SetObjTo (hint.transform, true, spriteIdx);
					/*
					Sprite obstacleSprite = spriteDataCtrl.spriteArray[spriteIdx];
					float offsetY = spriteDataCtrl.offsetYArray[spriteIdx];

					spriteRender.sprite = obstacleSprite;
					spriteRender.enabled = true;

					var pos = hint.transform.localPosition;
					pos.y = offsetY;
					// 編號4要在背景
					// z值 = [前景,貓,背景] = [-1,0,1]
					if (spriteIdx == 4) {
						pos.z = 0;
					} else {
						pos.z = -1;
					}
					hint.transform.localPosition = pos;
					*/
				}

				if (playIdx > 0 && playIdx < 7)
					lastPlayIdx = playIdx;
			}
		}

		IEnumerator FlyAway(int idx){
			// Run in XCode: 
			// NullReferenceException at Remix.ObstacleView+<FlyAway>c__Iterator0.MoveNext () [0x00000] in <filename unknown>:0 
			if (idx < 0 || idx >= hintArray.Length) {
				yield break;
			}
			if (hintArray [idx] == null) {
				yield break;
			}
			// 有Respawn的，不會被撞飛
			if (hintArray [idx].tag == "Respawn") {
				yield break;
			}
			var hint = hintArray [idx].transform.GetChild(0).gameObject;
			var timer = 0f;
			var duration = 0.5f;
			var xspeed = obstacleFlySpeed.x / duration;
			var yspeed = obstacleFlySpeed.y / duration;
			while (true) {
				try{
					var delta = Time.deltaTime;
					timer += delta;
					if (timer > 0.5f) {
						break;
					}
					var pos = hint.transform.localPosition;
					pos.x += delta * xspeed;
					pos.y += delta * yspeed;
					hint.transform.localPosition = pos;
				}catch(MissingReferenceException){
					// 物件刪除了，不理這個錯誤
					break;
				}
				yield return new WaitForEndOfFrame ();
			}
		}

		void HideHint(int idx){
			hintArray [idx].SetActive (false);
		}

		public void HintPlayGood(int hintIdx, int clickIdx, bool isPerfect, bool isFever, Game.ClickType clickType){
			switch (clickType) {
			case Game.ClickType.Long:
			case Game.ClickType.Single:
				{
					if (isFever) {
						StartCoroutine (FlyAway(hintIdx));
					} else {
						// 直立物和地洞之類的不會消失
						if (clickIdx == 5 || clickIdx == 6) {
							return;
						}
						HideHint (hintIdx);
					}
				}
				break;
			}
		}

		public void HintPlayMiss(int hintIdx, int clickIdx, bool isFever){
			// 地洞之類的不會彈飛
			if (clickIdx == 5 || clickIdx == 6 || isFever == true) {
				return;
			}
			StartCoroutine (FlyAway(hintIdx));
		}
		public IEnumerator ShiningHint(){
			yield return null;
		}
	}
}

