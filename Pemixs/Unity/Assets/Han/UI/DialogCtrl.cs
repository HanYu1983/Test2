using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DialogCtrl : MonoBehaviour 
{
	public void InitialDialog(GameObject linkObj, string text)
	{
		Text uiText = GetComponentInChildren<Text>();
		uiText.text = text;
	}
}
