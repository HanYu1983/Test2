using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HpBarUICtrl : MonoBehaviour 
{
	private const float UPDATE_TIME = 0.25f;
	private const int FULL_HP_SCALE = 20;

	public int[] hpIntervalArray = {60, 30, 15, 0};
	public Sprite[] headHpSpriteArray;
	public Sprite[] midHpSpriteArray;
	public Sprite[] tailHpSpriteArray;
    public Sprite[] boardSpriteArray;
    public GameObject shiningHp;
    public GameObject shiningBoard;

	private int fullHp;
	private int hpScale;
	private float accumTime;
	private float startHp;
	private float endHp;
	private float currentHp;
	private int hp;

	private Image[] hpArray;
	private int hpIndex;
    private int changingHpIndex;

	// Use this for initialization
	void Start () 
	{
		accumTime = 0.0f;
        changingHpIndex = -1;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (startHp == endHp)
			return;

		accumTime += Time.deltaTime;
		if (accumTime >= UPDATE_TIME)
		{
			currentHp = endHp;
			startHp = endHp;
		}
		else
		{
			float t = accumTime / UPDATE_TIME;
			currentHp = (int)Mathf.Lerp(startHp, endHp, t);
		}
		UpdateHP();
	}

	public void InitialHp(int initHp, int maxHp)
	{
		hpArray = this.GetComponentsInChildren<Image>();
		fullHp = maxHp;
		hpScale = fullHp / FULL_HP_SCALE;
		startHp = endHp = currentHp = hp = initHp;
		UpdateHP();
	}

	public int GetHP()
	{
		return hp;
	}

	public float GetFullHP(){
		return fullHp;
	}

	public void AddHP(int addHp)
	{
		hp = Mathf.Clamp(hp + addHp, 0, fullHp);
		endHp = hp;
        accumTime = 0.0f;

        if (startHp != endHp)
		{
			currentHp = startHp;

			int startIndex = (int)startHp / hpScale;
			int endIndex = (int)endHp / hpScale;
            if (startIndex != endIndex)
            {
                //Debug.Log("startHp " + startHp + " / startIndex " + startIndex);
                //Debug.Log("endHp " + endHp + " / endIndex " + endIndex);

                int boardIndex;
                if (addHp > 0)
                {
                    boardIndex = 0;
					changingHpIndex = (int)endHp / hpScale;
                }
                else
                {
                    boardIndex = 1;
					changingHpIndex = (int)startHp / hpScale;
                }

                if (changingHpIndex >= hpArray.Length)
                    return;

                Image currentHpImage = hpArray[changingHpIndex];
                shiningHp.transform.position = currentHpImage.transform.position;

				Animator animator = shiningHp.GetComponent<Animator>();
				animator.SetTrigger("flash");

				Image image = shiningBoard.GetComponent<Image>();
				image.sprite = boardSpriteArray[boardIndex];
				animator = shiningBoard.GetComponent<Animator>();
				animator.SetTrigger("flash");
            }
		}
	}

	private void ResetHpSprite()
	{
		for (int i=0;i<hpArray.Length;++i)
		{
			Image image = hpArray[i];
			if (i == 0)
				image.sprite = headHpSpriteArray[hpIndex];
			else if (i == hpArray.Length - 1)
				image.sprite = tailHpSpriteArray[hpIndex];
			else
				image.sprite = midHpSpriteArray[hpIndex];
		}
	}

	private void UpdateHP()
	{
		for (int i=0;i<hpArray.Length;++i)
		{
			Image image = hpArray[i];
			if (i * hpScale > currentHp || currentHp == 0)
				image.enabled = false;
			else
				image.enabled = true;
		}

		for (int i=0;i<hpIntervalArray.Length;++i)
		{
			int hpInterval = hpIntervalArray[i];
			if (hp > hpInterval)
			{
				if (hpIndex != i)
				{
					hpIndex = i;
					ResetHpSprite();
				}
				break;
			}
		}
	}
}
