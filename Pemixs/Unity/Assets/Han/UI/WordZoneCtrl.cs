using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WordZoneCtrl : MonoBehaviour 
{
	public Text textScore;
	private Hashtable wordMap;
	private int targetScore;
	private int currentScore;
	private Animator scoreAnimator;

	// Use this for initialization
	void Start () 
	{
		wordMap = new Hashtable();
		WordCtrl[] wordArray = this.GetComponentsInChildren<WordCtrl>();
		for (int i=0;i<wordArray.Length;++i)
		{
			WordCtrl wordCtrl = wordArray[i];
			wordMap[wordCtrl.word] = wordCtrl;
		}

		targetScore = currentScore = 0;
		textScore.text = "0000000";
		scoreAnimator = textScore.gameObject.GetComponent<Animator>();
		scoreAnimator.SetTrigger("hide");
	}
	
	// Update is called once per frame
	void Update () 
	{
        if (targetScore == currentScore)
            return;

        currentScore += (int)(Time.deltaTime * 300000.0f);
        if (currentScore > targetScore)
        {
			scoreAnimator.SetTrigger("hide");
            currentScore = targetScore;
        }
        textScore.text = string.Format("{0:0000000}", currentScore);
	}
	
	public void ShowWord(string word)
	{
		WordCtrl wordCtrl = wordMap[word] as WordCtrl;
		wordCtrl.Show();
	}
	
	public void HideWord(string word)
	{
		WordCtrl wordCtrl = wordMap[word] as WordCtrl;
		wordCtrl.Hide();
	}
	
	public void FlashWord(string word)
	{
		WordCtrl wordCtrl = wordMap[word] as WordCtrl;
		wordCtrl.Flash(word);
	}

	public void ShowCombo(int comboCount)
	{
		ComboWordCtrl wordCtrl = wordMap["Combo"] as ComboWordCtrl;
		string word = "" + comboCount;
		wordCtrl.Flash(word);
	}

	public void UpdateScore(int score)
	{
        if (targetScore != currentScore)
        {
			scoreAnimator.SetTrigger("hide");
            currentScore = targetScore;
            textScore.text = string.Format("{0:0000000}", currentScore);
        }

		scoreAnimator.SetTrigger("show");
        targetScore = score;
//		textScore.text = string.Format("{0:0000000}", score);
	}
}