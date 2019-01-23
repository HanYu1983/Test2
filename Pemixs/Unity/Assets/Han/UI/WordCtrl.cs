using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WordCtrl : MonoBehaviour 
{
	public string word;
	protected Image textImage;
	protected Animator animator;

	void Start () 
	{
		textImage = this.GetComponent<Image>();
		animator = this.GetComponent<Animator>();
	}

	public virtual void Flash(string word)
	{
		animator.SetTrigger("flash");
	}
	
	public void Show()
	{
		animator.SetTrigger("show");
	}
	
	public void Hide()
	{
		animator.SetTrigger("hide");
	}
}
