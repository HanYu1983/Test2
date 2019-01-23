using System;
using UnityEngine;
using TouchScript.Gestures;

namespace Remix
{
	public class TapGestureTrigger : MonoBehaviour
	{
		public TapGesture tapGesture;
		public string command;

		void OnEnable(){
			tapGesture.Tapped += TapGesture_Tapped;
		}
		void OnDisable(){
			tapGesture.Tapped -= TapGesture_Tapped;
		}

		void TapGesture_Tapped (object sender, EventArgs e)
		{
			UIEventFacade.OnTapGesture.OnNext (command);
		}
	}
}

