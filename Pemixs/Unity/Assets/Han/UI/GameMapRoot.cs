using System;
using UnityEngine;
using TouchScript.Behaviors;
using TouchScript.Gestures;
using System.Collections.Generic;
using System.Collections;

namespace Remix
{
	public class GameMapRoot : MonoBehaviour
	{
		public List<string> mapIdx2ArrayIdx;

		void Awake(){
			mapIdx2ArrayIdx = GameConfig.MAP_IDXS;
		}

		public IEnumerator LoadMapAsync(string mapIdx){
			var idx = mapIdx2ArrayIdx.IndexOf(mapIdx);
			if(idx == -1){
				throw new UnityException ("沒有這張地圖:"+mapIdx);
			}
			var page = GetComponent<PageGroup> ();
			yield return page.ChangePageAsync (idx);
		}

		public void LoadMap(string mapIdx){
			var idx = mapIdx2ArrayIdx.IndexOf(mapIdx);
			if(idx == -1){
				throw new UnityException ("沒有這張地圖:"+mapIdx);
			}
			var page = GetComponent<PageGroup> ();
			page.ChangePage (idx);
		}

		public bool IsInMap(string mapIdx){
			var idx = mapIdx2ArrayIdx.IndexOf(mapIdx);
			var page = GetComponent<PageGroup> ();
			return page.CurrentPageIdx == idx;
		}

		public GameMap GetGameMap(){
			var page = GetComponent<PageGroup> ();
			if (page.HasCurrentPage == false) {
				throw new UnityException ("請先呼叫LoadMap");
			}
			return page.CurrentPage.GetComponent<GameMap>();
		}

		public void ApplyTransform(ITransformGesture gesture){
			gesture.ApplyTransform(transform);
		}
	}
}

