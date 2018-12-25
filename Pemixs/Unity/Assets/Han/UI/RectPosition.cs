using System;
using UnityEngine;

namespace Remix
{
	public class RectPosition : MonoBehaviour
	{
		public Transform min, max;

		public Vector3 Max(Camera camera){
			if (camera == null) {
				return max.transform.position;
			}
			return camera.WorldToScreenPoint (max.position);
		}

		public Vector3 Min(Camera camera){
			if (camera == null) {
				return min.transform.position;
			}
			return camera.WorldToScreenPoint (min.position);
		}

		public float Width(Camera camera){
			var p1 = Min (camera);
			var p2 = Max (camera);
			return Mathf.Abs (p1.x - p2.x);
		}

		public float Height(Camera camera){
			var p1 = Min (camera);
			var p2 = Max (camera);
			return Mathf.Abs (p1.y - p2.y);
		}
	}
}

