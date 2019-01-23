using System;
using UnityEngine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;

namespace Remix
{
	public class StageView : MonoBehaviour
	{
		public GameObject sceneAnchor;
		public int leftCatIdx, rightCatIdx;
		public string mode;

		public const int P1_MISS_IDLE_IDX = 1;
		public const int P1_DEFAULT_IDX = 2;
		public const int P1_PASS_IDLE_IDX = 3;
		public const int P1_GOOD_IDX = 4;
		public const int P1_BAD_IDX = 5;
		public const int P1_BTN_01_IDX = 6;
		public const int P1_BTN_02_IDX = 7;
		public const int P1_BTN_03_IDX = 8;
		public const int P1_BTN_04_IDX = 9;
		public const int P1_BTN_05_IDX = 10;
		public const int P1_BTN_06_IDX = 11;
		public const int P1_BTN_LOOP_1_IDX = 12;
		public const int P1_BTN_LOOP_2_IDX = 13;

		public const int P2_MISS_IDLE_IDX = 1;
		public const int P2_DEFAULT_IDX = 2;
		public const int P2_PASS_IDLE_IDX = 3;
		public const int P2_GOOD_IDX = 4;
		public const int P2_BAD_IDX = 5;
		public const int P2_UPPER_ATK_IDX = 6;
		public const int P2_LOWER_ATK_IDX = 7;
		public const int P2_UPPER_DEF_IDX = 8;
		public const int P2_LOWER_DEF_IDX = 9;
		public const int P2_HURT_IDX = 10;
		public const int P2_FEVER_UPPER_ATK_IDX = 11;
		public const int P2_FEVER_LOWER_ATK_IDX = 12;
		public const int P2_FEVER_HURT_IDX = 13;

		public const int P3_MISS_IDLE_IDX = 1;
		public const int P3_DEFAULT_IDX = 2;
		public const int P3_PASS_IDLE_IDX = 3;
		public const int P3_FEVER_IDLE_IDX = 4;
		public const int P3_FEVER_UP_IDX = 5;
		public const int P3_FEVER_DOWN_IDX = 6;
		public const int P3_GOOD_IDX = 7;
		public const int P3_BAD_IDX = 8;
		public const int P3_BTN_01_IDX = 9;
		public const int P3_BTN_02_IDX = 10;
		public const int P3_BTN_03_IDX = 11;
		public const int P3_BTN_04_IDX = 12;
		public const int P3_BTN_05_IDX = 13;
		public const int P3_BTN_06_IDX = 14;
		public const int P3_HURT_IDX = 15;
		public const int P3_BTN_LOOP_1_IDX = 16;
		public const int P3_BTN_LOOP_2_IDX = 17;

		GameObject feverSpineObj, leftCat, rightCat;
		GameObject foreground, background;

		// dancing mode
		GameObject cat3, cat4, cat5;
		// 回傳左貓群
		public List<GameObject> LeftCats{
			get{
				var ret = new List<GameObject> (){ leftCat };
				if (cat4 != null) {
					ret.Add (cat4);
				}
				if (cat5 != null) {
					ret.Add (cat5);
				}
				return ret;
			}
		}
		// 回傳右貓群
		public List<GameObject> RightCats{
			get{
				var ret = new List<GameObject> (){ rightCat };
				if (cat3 != null) {
					ret.Add (cat3);
				}
				return ret;
			}
		} 

		public GameObject LeftCat{ get { return leftCat; } }
		public GameObject RightCat{ get { return rightCat; } }

		public void InitComponents(){
			feverSpineObj = Util.Instance.GetPrefab(new ItemKey(StoreCtrl.DATA_CAT, leftCatIdx).CatSpinePrefab("F"), null);
			feverSpineObj.transform.SetParent(sceneAnchor.transform, false);
			feverSpineObj.transform.localPosition = new Vector3(0.0f, 300.0f, -2.0f);
			feverSpineObj.transform.localScale = Vector3.one;
			feverSpineObj.SetActive (false);

			leftCat = Util.Instance.GetPrefab(new ItemKey(StoreCtrl.DATA_CAT, leftCatIdx).CatSpinePrefab(mode), null);
			leftCat.transform.SetParent(sceneAnchor.transform, false);
			leftCat.transform.localPosition = new Vector3(-1.5f,1.5f,0);
			leftCat.transform.localScale = new Vector3(0.1f, 0.1f, 1.0f);

			// 跑酷模式沒有右邊貓
			if (mode != "P3") {
				rightCat = Util.Instance.GetPrefab(new ItemKey(StoreCtrl.DATA_CAT, rightCatIdx).CatSpinePrefab(mode), null);
				rightCat.transform.SetParent(sceneAnchor.transform);
				rightCat.transform.localPosition = new Vector3(1.5f,1.5f,0);
				rightCat.transform.localScale = new Vector3(0.1f, 0.1f, 1.0f);
			}

			if (mode == "P1") {
				cat3 = GameObject.Instantiate (rightCat);
				cat3.transform.SetParent (sceneAnchor.transform, false);

				cat4 = GameObject.Instantiate (leftCat);
				cat4.transform.SetParent (sceneAnchor.transform, false);

				cat5 = GameObject.Instantiate (leftCat);
				cat5.transform.SetParent (sceneAnchor.transform, false);
			}

			if (mode == "P1") {
				ChangeCatOrderInModeP1 ();
			}
		}

		public void LoadBackground(string mapIdx){
			foreground = GameRecord.GetForeground (mapIdx, mode);
			foreground.transform.SetParent(sceneAnchor.transform);
			foreground.transform.localPosition = new Vector3 (0, 300, -1);
			foreground.transform.localScale = Vector3.one;

			background = GameRecord.GetBackground (mapIdx, mode);
			background.transform.SetParent(sceneAnchor.transform);
			background.transform.localPosition = new Vector3 (0, 300, 1);
			background.transform.localScale = Vector3.one;
		}

		float timeScale = 1f;
		public float TimeScale{ 
			set { 
				var animObjs = new GameObject[]{ feverSpineObj, leftCat, rightCat, background, foreground, cat3, cat4, cat5 };
				foreach (var obj in animObjs) {
					// rightCat 在跑酷模式時是null
					if (obj == null) {
						continue;
					}
					var s = obj.GetComponent<SkeletonAnimation> ();
					s.timeScale = value;
					timeScale = value;
				}
			}
			get{
				return timeScale;
			}
		}

		// 貓會跟著Spine中的骨架設定走
		public void SyncCatTransform () {
			SkeletonAnimation skeletonAnim = foreground.GetComponent<SkeletonAnimation>();
			switch (mode) {
			case "P3":
				{
					Spine.Bone bone01 = skeletonAnim.skeleton.FindBone("ChNu01");
					SetCatUpdate (leftCat, bone01, 0.2f, 1);
				}
				break;
			case "P2":
				{
					Spine.Bone bone01 = skeletonAnim.skeleton.FindBone("ChNu01");
					Spine.Bone bone02 = skeletonAnim.skeleton.FindBone("ChNu02");
					SetCatUpdate (leftCat, bone01, 0.2f, 1);
					SetCatUpdate (rightCat, bone02, 0.2f, -1);
				}
				break;
			default:
				{
					Spine.Bone bone01 = skeletonAnim.skeleton.FindBone("ChNu01");
					Spine.Bone bone011 = skeletonAnim.skeleton.FindBone("ChNu01-1");

					Spine.Bone bone02 = skeletonAnim.skeleton.FindBone("ChNu02");
					Spine.Bone bone021 = skeletonAnim.skeleton.FindBone("ChNu02-1");
					Spine.Bone bone022 = skeletonAnim.skeleton.FindBone("ChNu02-2");

					SetCatUpdate (rightCat, bone01, 0.2f, 1);
					SetCatUpdate (cat3, bone011, 0.2f, 1);

					SetCatUpdate (leftCat, bone02, 0.2f, 1);
					SetCatUpdate (cat4, bone021, 0.2f, 1);
					SetCatUpdate (cat5, bone022, 0.2f, 1);
				}
				break;
			}
		}

		void SetCatUpdate(GameObject cat, Spine.Bone attachBone, float scaleXY, float scaleX){
			Vector3 pos = cat.transform.localPosition;
			pos.x = attachBone.WorldX + foreground.transform.localPosition.x;
			pos.y = attachBone.WorldY + foreground.transform.localPosition.y;
			cat.transform.localPosition = pos;
			Vector3 scale = cat.transform.localScale;
			scale.x = attachBone.WorldScaleX * scaleXY * scaleX;
			scale.y = attachBone.WorldScaleY * scaleXY;
			cat.transform.localScale = scale;
		}

		public Spine.TrackEntry ChangeAnimation(GameObject scene, int index, bool isLoop){
			if (scene == null) {
				return null;
			}
			SkeletonAnimation skeletonAnim = scene.GetComponent<SkeletonAnimation>();
			skeletonAnim.timeScale = TimeScale;
			if (index >= skeletonAnim.skeleton.Data.Animations.Items.Length) {
				throw new UnityException (mode+"模式中沒有這個"+index+"動畫!");
			}
			Spine.Animation anim = skeletonAnim.skeleton.Data.Animations.Items[index];
			return skeletonAnim.state.SetAnimation(0, anim, isLoop);
		}

		public void ChangeCatOrderInModeP1(){
			if (mode != "P1") {
				throw new UnityException ("請確定在P1的模式下才使用這個方法ChangeCatOrderInModeP1");
			}
			leftCat.GetComponent<MeshRenderer> ().sortingOrder = 4;
			cat4.GetComponent<MeshRenderer> ().sortingOrder = 2;
			cat5.GetComponent<MeshRenderer> ().sortingOrder = 2;

			rightCat.GetComponent<MeshRenderer> ().sortingOrder = 3;
			cat3.GetComponent<MeshRenderer> ().sortingOrder = 3;
		}

		public void ChangeCatInTopLayer(GameObject top){
			if (leftCat == top) {
				leftCat.GetComponent<MeshRenderer> ().sortingOrder = 4;
				if (rightCat != null) {
					rightCat.GetComponent<MeshRenderer> ().sortingOrder = 3;
				}
			} else if (rightCat == top) {
				leftCat.GetComponent<MeshRenderer> ().sortingOrder = 3;
				if (rightCat != null) {
					rightCat.GetComponent<MeshRenderer> ().sortingOrder = 4;
				}
			} else {
				leftCat.GetComponent<MeshRenderer> ().sortingOrder = 3;
				if (rightCat != null) {
					rightCat.GetComponent<MeshRenderer> ().sortingOrder = 3;
				}
			}
		}

		public Spine.TrackEntry Fever(){
			feverSpineObj.gameObject.SetActive (true);
			return ChangeAnimation (feverSpineObj, 0, false);
		}

		public const int LeftAndBack = -1;
		public const int RightAndBack = 1;
		public const int Left = -2;
		public const int Right = 2;
		public const int Back = 0;

		public IEnumerator MoveStage(int dir){
			if (mode != "P2") {
				throw new UnityException ("模式2才能呼叫MoveStage");
			}
			switch (dir) {
			case StageView.Right:
				{
					//ChangeAnimation (foreground, 6, false);
					var track = ChangeAnimation (background, 4, false);
					yield return new WaitForSpineAnimationComplete (track);

					ChangeAnimation (foreground, 7, true);
					track = ChangeAnimation (background, 5, false);
					yield return new WaitForSpineAnimationComplete (track);
				}
				break;
			case StageView.Left:
				{
					throw new UnityException ("unsupport: StageView.Left");
					/*
					ChangeAnimation (foreground, 6, false);
					var track = ChangeAnimation (background, 6, false);
					yield return new WaitForSpineAnimationComplete (track);

					ChangeAnimation (foreground, 7, false);
					track = ChangeAnimation (background, 7, false);
					yield return new WaitForSpineAnimationComplete (track);
					*/
				}
				break;
			case StageView.RightAndBack:
				{
					ChangeAnimation (foreground, 2, false);
					var track = ChangeAnimation (background, 2, false);
					yield return new WaitForSpineAnimationComplete (track);
				}
				break;
			case StageView.LeftAndBack:
				{
					ChangeAnimation (foreground, 1, false);
					var track = ChangeAnimation (background, 1, false);
					yield return new WaitForSpineAnimationComplete(track);
				}
				break;
			case StageView.Back:
				{
					// 等同於ResetStage
					ChangeAnimation (foreground, 0, false);
					var track = ChangeAnimation (background, 0, false);
					yield return new WaitForSpineAnimationComplete(track);
				}
				break;
			default:
				throw new UnityException ("no dir:"+dir);
			}
		}

		uint moveIdx = 0;
		public Spine.TrackEntry StepMoveStage(){
			if (mode != "P1") {
				throw new UnityException ("模式1才能呼叫StepMoveStage");
			}
			uint activeIdx = moveIdx % 3;
			moveIdx += 1;
			switch (activeIdx % 3) {
			case 0:
				{
					var track1 = ChangeAnimation (foreground, 0, false);
					ChangeAnimation (background, 0, false);
					return track1;
				}
			case 1:
				{
					var track1 = ChangeAnimation (foreground, 1, false);
					ChangeAnimation (background, 1, false);
					return track1;
				}
			case 2:
				{
					var track1 = ChangeAnimation (foreground, 2, false);
					ChangeAnimation (background, 2, false);
					return track1;
				}
			}
			throw new UnityException ("這個錯誤不會丟出");
		}

		uint feverIdx = 0;

		public Spine.TrackEntry StepFever(){
			var idx1 = new Dictionary<string,int> () {
				{ "P1",4 },
				{ "P2",4 },
				{ "P3",2 }
			};
			var idx2 = new Dictionary<string,int> () {
				{ "P1",5 },
				{ "P2",5 },
				{ "P3",3 }
			};
			var idx3 = new Dictionary<string,int> () {
				{ "P1",6 },
				{ "P2",6 },
				{ "P3",4 }
			};
			uint activeIdx = feverIdx % 3;
			feverIdx += 1;
			switch (activeIdx % 3) {
			case 0:
				{
					ChangeAnimation (foreground, idx1[mode], false);
					var track = ChangeAnimation (background, idx1[mode], false);
					return track;
				}
			case 1:
				{
					ChangeAnimation (foreground, idx2[mode], true);
					var track = ChangeAnimation (background, idx2[mode], true);
					return track;
				}
			case 2:
				{
					ChangeAnimation (foreground, idx3[mode], false);
					var track = ChangeAnimation (background, idx3[mode], false);
					return track;
				}
			}
			throw new UnityException ("這個錯誤不會丟出");
		}

		public void StopStage(){
			if (mode != "P3") {
				Debug.LogWarning ("P3才能停止Stage");
				return;
			}
			ChangeAnimation (foreground, 1, true);
			ChangeAnimation (background, 1, true);
		}

		public void ResetStage(){
			if (mode == "P3") {
				ChangeAnimation (foreground, 0, true);
				ChangeAnimation (background, 0, true);
				return;
			}
			ChangeAnimation (foreground, 3, false);
			ChangeAnimation (background, 3, false);
			moveIdx = feverIdx = 0;
		}

		public Spine.TrackEntry ResetCat(){
			foreach (var cat in LeftCats) {
				ChangeAnimation (cat, P1_DEFAULT_IDX, true);
			}
			Spine.TrackEntry lastEntry = null;
			foreach (var cat in RightCats) {
				lastEntry = ChangeAnimation (cat, P1_DEFAULT_IDX, true);
			}
			return lastEntry;
		}

		public IEnumerator InvokeCatAnim(GameObject cat, int idx, int originIdx){
			var track = ChangeAnimation (cat, idx, false);
			yield return new WaitForSpineAnimationComplete (track);
			ChangeAnimation (cat, originIdx, true);
		}

		public IEnumerator InvokeFeverStart(){
			// 要等待的只有Fever大貓的動畫
			var track = Fever ();
			yield return new WaitForSpineAnimationComplete (track);
			// 背景的移動等到大貓動畫貓完就立刻播
			StartCoroutine (StageStartFever ());
		}

		IEnumerator StageStartFever(){
			feverIdx = 0;
			var track = StepFever ();
			yield return new WaitForSpineAnimationComplete (track);
			StepFever ();
		}

		public IEnumerator InvokeCatAttack(GameObject cat, int idx, int reactIdx, float reactDelay, SoundDataCtrl leftSound = null, SoundDataCtrl rightSound = null){
			if (mode != "P2") {
				throw new UnityException ("模式2才能攻擊");
			}
			SoundDataCtrl atkSnd = null;
			SoundDataCtrl defSnd = null;
			atkSnd = cat == LeftCat ? leftSound : rightSound;
			defSnd = cat == RightCat ? leftSound : rightSound;
			if (atkSnd != null) {
				atkSnd.PlaySound (SoundDataCtrl.ATK_SOUND);
			}
			ChangeAnimation (cat, idx, false);
			yield return new WaitForSeconds (reactDelay);
			var isGuarded = reactIdx == StageView.P2_LOWER_DEF_IDX || reactIdx == StageView.P2_UPPER_DEF_IDX;
			if (defSnd != null) {
				defSnd.PlaySound (isGuarded ? SoundDataCtrl.DEF_SOUND : SoundDataCtrl.HURT_SOUND);
			}
			var opponentCat = cat == LeftCat ? RightCat : LeftCat;
			var track = ChangeAnimation (opponentCat, reactIdx, false);
			yield return new WaitForSpineAnimationComplete (track);
			ChangeAnimation (cat, P1_DEFAULT_IDX, true);
			track = ChangeAnimation (opponentCat, P1_DEFAULT_IDX, true);
			yield return new WaitForSpineAnimationComplete (track);
		}
			
		public IEnumerator SpineAnimationCallback(Spine.TrackEntry track, Action<Spine.TrackEntry> cb){
			yield return new WaitForSpineAnimationComplete (track);
			cb (track);
		}
	}
}

