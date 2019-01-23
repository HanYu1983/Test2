using System;
using UnityEngine;
namespace Remix
{
	public class ColliderMouseUpTrigger : MonoBehaviour
	{
		public string command;
		// 只有OnMouseUp可以有反應
		// OnMouseClick沒有
		void OnMouseUp(){
			UIEventFacade.OnPointerClick.OnNext(command);
		}

		public void SetEnable(bool v){

		}
	}
}

