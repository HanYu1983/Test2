using System;
using UnityEngine;

namespace Common
{
	public class UIEvent : MonoBehaviour
	{
		public void Notify(string msg){
			Debug.Log ("[UIEvent]:"+msg);
			Common.Notify (msg, null);
		}
	}
}

