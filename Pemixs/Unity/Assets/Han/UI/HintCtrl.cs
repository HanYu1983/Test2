using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HintCtrl : MonoBehaviour 
{
	public Image hintImage;
	public Image seqImage;
    public Image circleImage;
	public Image shiningImage;
	public Image feverImage;

	protected Sprite normalSprite;
	protected Sprite pressSprite;
	[SerializeField]
	protected float accumTime;
	protected bool bPause;
	protected float seqScaleX;
	// linkHintCtrl是指向長按的起始節點
	protected HintCtrl linkHintCtrl;
	public int playIdx;

    protected Vector3 stopPos = Vector3.zero;

	// Use this for initialization
	void Awake () 
	{
		accumTime = 0.0f;
		hintImage.enabled = false;
		seqImage.enabled = false;
		Animator animator = circleImage.gameObject.GetComponent<Animator>();
		animator.enabled = false;
		circleImage.enabled = false;
		animator = shiningImage.gameObject.GetComponent<Animator>();
		animator.enabled = false;
        shiningImage.enabled = false;
		animator = feverImage.gameObject.GetComponent<Animator>();
		animator.enabled = false;
		feverImage.enabled = false;
		linkHintCtrl = null;
	}
	
	// Update is called once per frame
	public void Step (float t) 
	{
		if (bPause == true)
			return;

		if (accumTime <= 0.0f)
			return;

		accumTime -= t;
		if (accumTime <= 0.0f)
		{
			hintImage.enabled = false;
			Animator  animator = circleImage.gameObject.GetComponent<Animator>();
			animator.enabled = false;
            shiningImage.enabled = false;
			animator = shiningImage.gameObject.GetComponent<Animator>();
			animator.enabled = false;
			circleImage.enabled = false;
			animator = feverImage.gameObject.GetComponent<Animator>();
			animator.enabled = false;
			feverImage.enabled = false;
		}
        else if (stopPos != Vector3.zero)
        {
            this.gameObject.transform.position = stopPos;
			if (seqImage.enabled == true)
			{
				// 原始比例 x 時間上的縮放比
				var targetScale = ComputeSeqImgLength(seqScaleX, seqCount, accumTime);
				// 確保長按條會消失到看不見
				if (targetScale < 0.2f) {
					targetScale = 0f;
				}
				Vector3 scale = seqImage.transform.localScale;
				scale.x = targetScale;
				seqImage.transform.localScale = scale;
			}
        }
	}

	public virtual void SetSprite(Sprite showSprite, Sprite hideSprite, Sprite shiningSprite, Sprite feverSprite)
	{
		if (showSprite == null)
		{
			normalSprite = null;
			accumTime = 0.0f;
			hintImage.enabled = false;
			seqImage.enabled = false;
            shiningImage.sprite = null;
            shiningImage.enabled = false;
			circleImage.sprite = null;
			circleImage.enabled = false;
			feverImage.sprite = null;
			feverImage.enabled = false;
			linkHintCtrl = null;
			return;
		}

		normalSprite = showSprite;
		circleImage.sprite = hideSprite;
		circleImage.enabled = false;
        shiningImage.sprite = shiningSprite;
        shiningImage.enabled = false;
		feverImage.sprite = feverSprite;
		feverImage.enabled = false;

        stopPos = Vector3.zero;

		hintImage.sprite = normalSprite;
		hintImage.enabled = true;
		Color colorFrom = hintImage.color;
        colorFrom.a = 1.0f;
		hintImage.color = colorFrom;
		hintImage.SetNativeSize();
		linkHintCtrl = null;
	}

	[SerializeField]
	int seqCount;

	public void SetSequence(Sprite seqSprite, float scaleX, int seqCount)
	{
		this.seqCount = seqCount;
		seqScaleX = scaleX;
		seqImage.sprite = seqSprite;
		seqImage.enabled = true;
		Vector3 scale = seqImage.transform.localScale;
		scale.x = seqScaleX;
		scale.y = 0.8f;
		seqImage.transform.localScale = scale;
	}

	public void SetLinkHintCtrl(HintCtrl hintCtrl)
	{
		this.linkHintCtrl = hintCtrl;
	}
	
	public virtual void SetPause(bool pause)
	{
		bPause = pause;
	}

	public void OnTick(bool bRepeat)
	{
//		if (normalSprite == null)
//			return;
//
//		Image image = this.GetComponent<Image>();
//		image.enabled = true;
//		image.sprite = pressSprite;
//		image.SetNativeSize();
//		accumTime = RhythmCtrl.HALF_BEAT_TIME;
	}

	public void PlayGood(bool isPerfect, bool bFever)
    {
		#if LONGCLICK_OLD_VERSION1
		if (linkHintCtrl != null)
		{
			if (playIdx != 8) {
				return;
			}
		}
		#else
		// 連擊的其它點都不能停止移動
		if (linkHintCtrl != null){
			return;
		}
		// 連擊的第一點不能停止移動
		if (seqImage.enabled == true){
			return;
		}
		#endif
		if (seqImage.enabled == true)
			accumTime = RhythmCtrl.HALF_BEAT_TIME * seqCount;
		else
			accumTime = RhythmCtrl.HALF_BEAT_TIME;
        stopPos = this.gameObject.transform.position;
		Animator  animator = circleImage.gameObject.GetComponent<Animator>();
		animator.enabled = true;
		circleImage.enabled = true;

        if (isPerfect == true)
        {
			animator = shiningImage.gameObject.GetComponent<Animator>();
			animator.enabled = true;
            shiningImage.enabled = true;
			if (bFever == true)
			{
				animator = feverImage.gameObject.GetComponent<Animator>();
				animator.enabled = true;
				feverImage.enabled = true;
			}
        }
    }
		
	static float ComputeSeqImgLength(float originScale, int seqCount, float accumTime){
		// 原始比例 x 時間上的縮放比
		var targetScale = originScale * accumTime / (RhythmCtrl.HALF_BEAT_TIME * seqCount);
		return targetScale;
	}

	public virtual void PlayMiss(int clickIdx, bool bFever)
	{
		if (linkHintCtrl != null)
		{
			linkHintCtrl.PlayMiss(clickIdx, bFever);
			return;
		}

		if (seqImage.enabled == true && accumTime > 0.0f)
		{
			
			this.gameObject.transform.position = stopPos;
			Vector3 scale = seqImage.transform.localScale;
			scale.x = ComputeSeqImgLength(seqScaleX, seqCount, accumTime);
			seqImage.transform.localScale = scale;

			accumTime = 0.0f;
			Animator  animator = circleImage.gameObject.GetComponent<Animator>();
			animator.enabled = false;
			shiningImage.enabled = false;
			animator = shiningImage.gameObject.GetComponent<Animator>();
			animator.enabled = false;
			circleImage.enabled = false;
			animator = feverImage.gameObject.GetComponent<Animator>();
			animator.enabled = false;
			feverImage.enabled = false;
		}
	}

	public virtual void PlayBad(int clickIdx, bool bFever)
	{
	}
}
