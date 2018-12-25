using System;
using UnityEngine;
using TouchScript.Gestures;
using System.Collections.Generic;

namespace Remix
{
	public class TransformGestureTrigger : MonoBehaviour
	{
		public string startCommand, doingCommand, completedCommand;
		private List<ITransformGesture> gestures = new List<ITransformGesture>();
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
				transformGesture.TransformStarted += transformStartHandler;
				transformGesture.TransformCompleted += transformCompletedHandler;
			}
		}

		void OnDisable()
		{
			for (var i = 0; i < gestures.Count; i++)
			{
				var transformGesture = gestures[i];
				transformGesture.Transformed -= transformHandler;
				transformGesture.TransformStarted -= transformStartHandler;
				transformGesture.TransformCompleted -= transformCompletedHandler;
			}
			gestures.Clear();
		}

		#endregion

		void transformStartHandler(object sender, EventArgs e){
			if (startCommand == null || startCommand.Length == 0) {
				return;
			}
			UIEventFacade.OnTransformGesture.OnNext (startCommand);
		}

		void transformCompletedHandler(object sender, EventArgs e){
			if (completedCommand == null || completedCommand.Length == 0) {
				return;
			}
			UIEventFacade.OnTransformGesture.OnNext (completedCommand);
		}

		void transformHandler(object sender, EventArgs e)
		{
			if (doingCommand == null || doingCommand.Length == 0) {
				return;
			}
			var cmd = doingCommand + UIEventFacade.RegisterParams (sender as ITransformGesture);
			UIEventFacade.OnTransformGesture.OnNext (cmd);
		}
	}
}

