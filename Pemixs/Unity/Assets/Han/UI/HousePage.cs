using System;
using UnityEngine;
using System.Collections;

namespace Remix
{
	public class HousePage : MonoBehaviour
	{
		public PageGroup mapsGroup;

		#region async
		public IEnumerator SelectHousePageMapAsync(int idx){
			yield return mapsGroup.ChangePageAsync (idx);
		}
		#endregion

		public void SelectHousePageMap(int idx){
			mapsGroup.ChangePage (idx);
		}

		public void NextHousePageMap(){
			mapsGroup.NextPage ();
		}

		public void PrevHousePageMap(){
			mapsGroup.PrevPage ();
		}

		public bool IsInHousePageMap(int idx){
			return mapsGroup.CurrentPageIdx == idx;
		}

		public bool IsInHousePageMap(string mapIdx){
			var idx = GameConfig.MAP_IDXS.IndexOf(mapIdx);
			return mapsGroup.CurrentPageIdx == idx;
		}

		public HousePageMap GetHousePageMap(){
			var pg = GetComponent<PageGroup> ();
			if (pg.HasCurrentPage == false) {
				throw new UnityException ("請先呼叫Load");
			}
			var houseMap = pg.CurrentPage.GetComponent<HousePageMap> ();
			if (houseMap == null) {
				throw new UnityException ("你忘了加入HousePageMap");
			}
			return houseMap;
		}
	}
}

