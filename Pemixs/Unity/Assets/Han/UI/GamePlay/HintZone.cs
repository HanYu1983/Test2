using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Remix
{
	public class HintZone : MonoBehaviour, IHintZone
	{
		public HintPointCtrl hitPoint;
		public GameObject hintShining;
		public Sprite[] showSpriteArray;
		public Sprite[] hideSpriteArray;
		public Sprite[] shiningSpriteArray;
		public Sprite[] feverSpriteArray;
		public Sprite[] seqSpriteArray;
		public Image hintImage;
		public GameObject frontLayer;
		public GameObject backLayer;

		public float timePerBeat;
		public int beatCntPerTurn;
		public int turnCntPerLevel;
		public float startTime;
		public float hintWidth;

		public float timer;
		public float startPos;
		public float endPos;
		public float moveTime;
		public HintCtrl[] hintArray;

		public void InitHintZone(){
			
		}
			
		public float TimePerBeat{ 
			get{
				return timePerBeat;
			}
			set{ 
				timePerBeat = value;
			}
		}
		public int BeatCntPerTurn{ 
			get{
				return beatCntPerTurn;
			}
			set{ 
				beatCntPerTurn = value;
			}
		}
		public int TurnCntPerLevel{ 
			get{
				return turnCntPerLevel;
			}
			set{ 
				turnCntPerLevel = value;
			}
		}
		public float StartTime{
			get{
				return startTime;
			}
			set{ 
				startTime = value;
			}
		}
		public float HintWidth{
			get{
				return hintWidth;
			}
			set{ 
				hintWidth = value;
			}
		}

		void Awake(){
			hintArray = this.GetComponentsInChildren<HintCtrl>();
		}



		public IEnumerator ShiningHint(){
			hintShining.SetActive (true);
			yield return new WaitForSeconds (0.1f);
			hintShining.SetActive (false);
		}

		public void ComputeBasicVar(float offset){
			timer = 0;
			startPos = hitPoint.transform.localPosition.x + offset;
			// 移到整條尾部都消失在左邊
			// 位置 = -右邊位置
			endPos = hitPoint.transform.localPosition.x - HintWidth/2 * (TurnCntPerLevel * BeatCntPerTurn);
			moveTime = TimePerBeat * TurnCntPerLevel * BeatCntPerTurn;
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
				HintCtrl hint = hintArray[i];
				GameObject obj = hint.gameObject;
				Image image = obj.GetComponent<Image>();
				Vector3 pos = image.rectTransform.localPosition;
				pos.x = i * (dist / count);
				image.rectTransform.localPosition = pos;
			}
			Vector3 hintPos = hintImage.rectTransform.localPosition;
			hintPos.x = startPos;
			hintImage.rectTransform.localPosition = hintPos;
		}

		public void StepHintArray(float t){
			foreach (var hint in hintArray) {
				hint.Step (t);
			}
		}

		// 呼叫這個方法前記得先呼叫ComputeBasicVar
		public void SyncTimer(float timer){
			float scale = timer / moveTime;
			// 不用內建的Mathf.Lerp
			// 內建的Mathf.Lerp不支援負數的scale
			Vector3 pos = hintImage.rectTransform.localPosition;
			var offset = endPos - startPos;
			var lerp = offset * scale;
			pos.x = startPos + lerp;
			hintImage.rectTransform.localPosition = pos;
			float delta = timer - this.timer;
			this.timer = timer;

			StepHintArray (delta);
		}

		public void InitHintSprite(int[][] idxAry, int[][] mashAry){
			int playIdx = 0;
			int lastPlayIdx = 0;
			int seqCount = 0;
			int seqHintIdx = 0;
			for (int i=0;i<hintArray.Length;++i)
			{
				hintArray[i].SetSprite(null, null, null, null);
				// 取得前半端還是後半段
				int turn = i / BeatCntPerTurn;
				// 取得第幾拍
				int idx = (i % BeatCntPerTurn);
				if (turn >= idxAry.Length) {
					// 持續SetSprite(null,...)
					continue;
				}
				// 注意：只有0Turn和1Turn是真正的打擊點
				// 之後的就是假的，單純用來無縫連接下一個Turn的打擊點
				//      |link|
				//      |----|fake|
				// |----|fake|
				playIdx = idxAry[turn][idx];
				if (playIdx == 0)
					continue;

				int btnMashIdx = mashAry[turn][idx];
				HintCtrl hint = hintArray[i];
				hint.playIdx = playIdx;

				if (playIdx == 7)
				{
					seqCount++;
					HintCtrl seqHintCtrl = hintArray[seqHintIdx];
					hint.SetLinkHintCtrl(seqHintCtrl);
				}
				else if (playIdx == 8)
				{
					seqCount++;

					hint.gameObject.transform.SetParent(frontLayer.transform);
					bool bMashingSliding = (btnMashIdx == 5 || btnMashIdx == 6);
					Sprite showSprite = (bMashingSliding == true) ? showSpriteArray[6] : showSpriteArray[lastPlayIdx - 1];
					Sprite hideSprite = (bMashingSliding == true) ? hideSpriteArray[6] : hideSpriteArray[lastPlayIdx - 1];
					Sprite shiningSprite = (bMashingSliding == true) ? shiningSpriteArray[6] : shiningSpriteArray[lastPlayIdx - 1];
					Sprite feverSprite = feverSpriteArray[lastPlayIdx - 1];
					hint.SetSprite(showSprite, hideSprite, shiningSprite, feverSprite);

					HintCtrl seqHintCtrl = hintArray[seqHintIdx];
					Sprite seqSprite = seqSpriteArray[lastPlayIdx - 1];
					// 本來的設計只有1/2拍(hint數16個)
					// 改為1/4拍(hint數有32個)後長度要除2
					float hintW = HintWidth/2;
					// 100是因為圖像的長度就是100
					float imgLen = 100f;
					// 縮放比
					float scaleW = hintW / imgLen;
					float scaleX = seqCount * scaleW;
					seqHintCtrl.SetSequence(seqSprite, scaleX, seqCount);
					hint.SetLinkHintCtrl(seqHintCtrl);

					// 子物件順序會影響繪製順序
					// 所以有連擊條的hint要移到第一個，這樣連擊條才不會蓋到下一個打擊點
					seqHintCtrl.transform.SetAsFirstSibling ();
				}
				else
				{
					seqCount = 0;
					seqHintIdx = i;
					hint.gameObject.transform.SetParent(frontLayer.transform);
					bool bMashingSliding = (btnMashIdx == 5 || btnMashIdx == 6);
					Sprite showSprite = (bMashingSliding == true) ? showSpriteArray[6] : showSpriteArray[playIdx - 1];
					Sprite hideSprite = (bMashingSliding == true) ? hideSpriteArray[6] : hideSpriteArray[playIdx - 1];
					Sprite shiningSprite = (bMashingSliding == true) ? shiningSpriteArray[6] : shiningSpriteArray[playIdx - 1];
					Sprite feverSprite = feverSpriteArray[playIdx - 1];
					hint.SetSprite(showSprite, hideSprite, shiningSprite, feverSprite);
				}

				if (playIdx > 0 && playIdx < 7)
					lastPlayIdx = playIdx;
			}
		}

		public void HintPlayGood(int hintIdx, int clickIdx, bool isPerfect, bool isFever, Game.ClickType clickType){
			var hint = hintArray [hintIdx];
			hint.PlayGood (isPerfect, isFever);
			hitPoint.OnTick (clickIdx);
		}

		public void HintPlayMiss(int hintIdx, int clickIdx, bool isFever){
			var hint = hintArray[hintIdx];
			hint.PlayMiss (clickIdx, isFever);
			hitPoint.OnTick (clickIdx);
		}

		public void ArrangePos(float offset){
			ComputeBasicVar (offset);
			ResetPos ();
		}
	}
}

