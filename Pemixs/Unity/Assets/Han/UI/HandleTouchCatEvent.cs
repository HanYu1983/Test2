using System;
using UnityEngine;

namespace Remix
{
	public class HandleTouchCatEvent : MonoBehaviour
	{
		public long startTick;
		public long durationTick;
		public bool isTouch;
		public float timer;

		public void StartTouch(){
			startTick = System.DateTime.Now.Ticks;
			isTouch = true;
		}

		public void EndTouch(){
			var endTick = System.DateTime.Now.Ticks;
			durationTick = endTick - startTick;
			isTouch = false;
		}

		public delegate void OnDoingTouchCat(HandleTouchCatEvent sender);

		public void Step(float t, OnDoingTouchCat OnDoingTouchCat){
			if (isTouch == false) {
				return;
			}
			timer += t;
			if (timer > 1) {
				timer -= 1;
				OnDoingTouchCat (this);
			}
		}

		public long DurationTick{ get{ return durationTick; } }
		public bool IsTouch{ get{ return isTouch; } }
	}
}

