using System;
using UnityEngine;
using System.Collections.Generic;

namespace Remix
{
	// 處理不會被Raycast檔住的物件
	public class PopupTracker : MonoBehaviour
	{
		public List<MonoBehaviour> popups = new List<MonoBehaviour>();

		public IEnumerable<MonoBehaviour> Popups {
			get {
				return popups;
			}
		}

		public void Track(MonoBehaviour popup){
			popups.Add (popup);
		}

		public bool IsPopupActive(MonoBehaviour popup){
			if (popups.Count == 0) {
				return true;
			}
			return popups[popups.Count-1] == popup;
		}

		public void Untrack(MonoBehaviour popup, bool force = false){
			if (force) {
				popups.Remove (popup);
				return;
			}
			if (popups[popups.Count-1] != popup) {
				Debug.LogWarning ("沒有正確的關閉Popup");
				popups.Remove (popup);
				return;
			}
			popups.RemoveAt (popups.Count - 1);
		}
	}
}

