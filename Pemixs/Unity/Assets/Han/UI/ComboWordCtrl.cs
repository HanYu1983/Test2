using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ComboWordCtrl : WordCtrl 
{
    public Image shiningImage;
	private Animator[] animatorArray;
    private Text textCombo;
	
	// Use this for initialization
	void Start () 
	{
		animatorArray = this.GetComponentsInChildren<Animator>();
		textCombo = this.GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	
	public override void Flash(string word)
	{
		textCombo.text = word;
		foreach (Animator animator in animatorArray)
		{
			animator.SetTrigger("flash");
		}
	}
}
