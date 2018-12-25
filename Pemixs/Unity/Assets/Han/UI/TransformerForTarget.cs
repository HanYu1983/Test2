using System;
using TouchScript.Gestures;
using UnityEngine;
using System.Collections.Generic;

namespace Remix
{
	public class TransformerForTarget : MonoBehaviour
	{
		#region Private variables

		public Transform cachedTransform;
		private List<ITransformGesture> gestures = new List<ITransformGesture>();

		#endregion

		#region Unity methods
		void OnEnable()
		{
			var g = GetComponents<Gesture>();
			for (var i = 0; i < g.Length; i++)
			{
				var transformGesture = g[i] as ITransformGesture;
				if (transformGesture == null) continue;

				gestures.Add(transformGesture);
				transformGesture.Transformed += transformHandler;
			}
		}

		void OnDisable()
		{
			for (var i = 0; i < gestures.Count; i++)
			{
				var transformGesture = gestures[i];
				transformGesture.Transformed -= transformHandler;
			}
			gestures.Clear();
		}

		#endregion

		#region Event handlers

		void transformHandler(object sender, EventArgs e)
		{
			var gesture = sender as ITransformGesture;
			gesture.ApplyTransform(cachedTransform);
		}

		#endregion
	}
}

