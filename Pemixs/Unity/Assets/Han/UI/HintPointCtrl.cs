using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HintPointCtrl : MonoBehaviour 
{
    public WordCtrl[] frameArray;

	public void OnTick(int playIdx)
	{
		switch (playIdx)
		{
		case 1:
		case 2:
		case 3:
		case 4:
			WordCtrl frameA = frameArray[0];
			frameA.Flash("A");
			break;
		case 5:
		case 6:
			WordCtrl frameB = frameArray[1];
			frameB.Flash("B");
			break;
		}
	}
}
