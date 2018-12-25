using System;
using UnityEngine;
using TouchScript.Gestures;

namespace Remix
{
	public class WorldCapRoot : MonoBehaviour
	{
		public void LoadMap(int idx){
			var pg = GetComponent<PageGroup> ();
			pg.ChangePage (idx);
		}

		public WorldCap GetWorldCap(){
			var pg = GetComponent<PageGroup> ();
			return pg.CurrentPage.GetComponent<WorldCap> ();
		}

		public void ApplyTransform(ITransformGesture gesture){
			gesture.ApplyTransform(transform);
		}
	}
}

