using UnityEngine;
using System.Collections;
using System;

namespace Remix
{
	public class BeatTool : MonoBehaviour{
		public static void Time2TurnAndCount(float beatCntPerTurn, float timePerBeat, float timer, ref int turn, ref float count){
			var timePerTurn = beatCntPerTurn * timePerBeat;
			var tmp = timer;
			while (tmp >= timePerTurn) {
				tmp -= timePerTurn;
			}
			count = tmp / timePerBeat;
			if (timer < 0) {
				turn = -1;
				return;
			}
			turn = (int)(timer / timePerTurn);
		}
	}

}