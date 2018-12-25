using System;
using UnityEngine;
using TouchScript.Gestures;

namespace Remix
{
	public class WorldMapRoot : MonoBehaviour
	{
		public void LoadMap(int idx){
			var pg = GetComponent<PageGroup> ();
			pg.ChangePage (idx);
		}

		public WorldMap GetWorldMap(){
			var pg = GetComponent<PageGroup> ();
			return pg.CurrentPage.GetComponent<WorldMap> ();
		}

		public void ApplyTransform(ITransformGesture gesture){
			gesture.ApplyTransform(transform);
		}
	}
}

