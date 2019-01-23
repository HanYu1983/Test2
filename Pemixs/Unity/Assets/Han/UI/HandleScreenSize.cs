using System;
using UnityEngine;

namespace Remix
{
	public class HandleScreenSize : MonoBehaviour
	{
		public Vector2 gameResolution;
		public Vector2 resolution;

		void Awake(){
			resolution.x = Screen.width;
			resolution.y = Screen.height;
		}

		public void Resize(Transform canvas){
			var targetY = gameResolution.x * resolution.y / resolution.x;
			var scale = targetY / gameResolution.y;
			canvas.localScale = new Vector3(scale, scale, 1f);
		}
	}
}

