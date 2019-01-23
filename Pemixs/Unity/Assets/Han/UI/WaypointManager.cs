using System;
using System.Collections.Generic;
using UnityEngine;

namespace Remix
{
	public class WaypointManager : MonoBehaviour
	{
		public List<Waypoint> points;

		public void PrepareWaypoint(){
			/*
			if (points.Count > 0) {
				return;
			}
			var ps = GetComponentsInChildren<Waypoint> ();
			points.AddRange (ps);
			*/
			if (points.Count == 0) {
				return;
			}
			var left = points [0];
			for (var i = 1; i < points.Count; ++i) {
				var right = points [i];
				left.next = right;
				right.prev = left;
				left = right;
			}
			this.gameObject.SetActive (false);
		}

		public Waypoint GetWaypoint(string id){
			foreach (Waypoint p in points) {
				if (p.name == id) {
					return p;
				}
			}
			throw new UnityException ("沒有這個名稱的Waypoint:"+id+"。可能Waypoint還沒編好");
		}

		public Waypoint GetWaypoint(Vector3 pos){
			Waypoint minP = null;
			var minDis = 99999999f;
			foreach (Waypoint p in points) {
				var dist = Vector3.Distance (p.transform.localPosition, pos);
				if (dist < minDis) {
					minDis = dist;
					minP = p;
				}
			}
			return minP;
		}

		public static Waypoint MoveStep(float t, Transform target, Waypoint p, float speed, string untilId){
			var dir = p.transform.localPosition - target.localPosition;
			var offset = Vector3.Normalize(dir) * speed * t;
			var movepos = target.localPosition + offset;
			target.localPosition = movepos;
			if (offset.magnitude >= dir.magnitude) {
				target.localPosition = p.transform.localPosition;
				if (p.name == untilId) {
					return null;
				}
				return p.next;
			}
			return p;
		}
	}
}

