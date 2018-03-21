using System;
using UnityEngine;
using System.Collections;

namespace Common
{
	public class HandleFungus : MonoBehaviour
	{
        /*
		public Flowchart flowchat;
        */
		public IEnumerator MissionDialog(string missionId){
            /*
			//flowchat.SetStringVariable ("missionId", missionId);
			flowchat.SendFungusMessage("missionDialog");
			yield return new WaitUntil (() => {
				var ret = flowchat.GetBooleanVariable("done") == true;
				if(ret == true){
					flowchat.SetBooleanVariable ("done", false);
				}
				return ret;
			});
            */
            yield return null;
		}
	}
}

