using System;
using UnityEngine;
using System.Collections.Generic;

namespace Remix
{
	public class HousePageMap : MonoBehaviour
	{
		public List<ButtonCtrl> catButtons;

		public void SetCatEnable(int idx, bool enable){
			if (idx >= catButtons.Count) {
				Debug.LogWarning ("還沒有這隻貓的按鈕，直接回傳");
				return;
			}
			if (catButtons [idx] == null) {
				Debug.LogWarning (idx+"沒有指定貓按鈕");
				return;
			}
			catButtons [idx].SetEnable (enable);
		}
	}
}

