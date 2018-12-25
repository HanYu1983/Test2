using System;
using UnityEngine;

namespace Remix
{
	public class Sessions : MonoBehaviour
	{
		#region play
		public string CurrentPlayingLevel;
		public bool HasCurrentPlayingLevel {
			get { 
				return CurrentPlayingLevel != null;
			}
		}
		public void ClearCurrentPlayingLevel(){
			CurrentPlayingLevel = null;
		}
		#endregion

		#region play game item
		public string UsedItem;
		#endregion
	}
}

