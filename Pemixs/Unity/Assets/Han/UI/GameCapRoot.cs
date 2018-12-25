using System;
using UnityEngine;
using TouchScript.Gestures;
using System.Collections.Generic;

namespace Remix
{
	public class GameCapRoot : MonoBehaviour
	{
		public List<string> mapIdx2ArrayIdx;

		void Awake(){
			mapIdx2ArrayIdx = GameConfig.MAP_IDXS;
		}

		public string CurrentMapIdx{
			get{
				var page = GetComponent<PageGroup> ();
				return mapIdx2ArrayIdx [page.CurrentPageIdx];
			}
		}

		public GameCap LoadMap(string mapIdx){
			var idx = mapIdx2ArrayIdx.IndexOf(mapIdx);
			if(idx == -1){
				throw new UnityException ("沒有這張地圖:"+mapIdx);
			}
			var page = GetComponent<PageGroup> ();
			page.ChangePage (idx);
			return GetGameCap ();
		}

		public bool IsInMap(string mapIdx){
			var idx = mapIdx2ArrayIdx.IndexOf(mapIdx);
			var page = GetComponent<PageGroup> ();
			return page.CurrentPageIdx == idx;
		}

		public GameCap GetGameCap(){
			var page = GetComponent<PageGroup> ();
			if (page.HasCurrentPage == false) {
				throw new UnityException ("請先呼叫LoadMap");
			}
			return page.CurrentPage.GetComponent<GameCap>();
		}

		public void ApplyTransform(ITransformGesture gesture){
			gesture.ApplyTransform(transform);
		}
	}
}

