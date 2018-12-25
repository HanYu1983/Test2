using UnityEngine;
using System.Collections;
using Remix;

public class PauseZoneCtrl : MonoBehaviour 
{
	public GameObject linkObj;
	
	private ButtonCtrl[] btnArray;

	// Use this for initialization
	void Start () 
	{
		btnArray = this.GetComponentsInChildren<ButtonCtrl>();
		for (int i=0;i<btnArray.Length;++i)
		{
			ButtonCtrl btn = btnArray[i];
			// btn.SetLinkObj(this.gameObject);
			btn.SetEnable(false);
			btn.SetVisible(false);
		}
	}
	/*
	// Update is called once per frame
	void Update () 
	{
	}
	
	private void OnButtonClick(string command)
	{
		linkObj.SendMessage("OnButtonClick", command);
	}
	*/
	public void SetPause(bool pause)
	{
		for (int i=0;i<btnArray.Length;++i)
		{
			ButtonCtrl btn = btnArray[i];
			btn.SetEnable(pause);
			btn.SetVisible(pause);
		}
	}

	/*
	public void SetLinkObj(GameObject obj){
		linkObj = obj;
	}
	*/
}
