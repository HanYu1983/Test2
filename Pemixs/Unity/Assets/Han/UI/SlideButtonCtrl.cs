using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Remix;

public class SlideButtonCtrl : MonoBehaviour
{
	private static int STATUS_NONE = 0;
	private static int STATUS_UP = 1;
	private static int STATUS_DOWN = 2;
	/*
    public Image shiningImage;
	public Image buttonImage;
	public Image feverImage;
	*/
	public KeyCode upKeyCode;
	public KeyCode downKeyCode;
	public bool touchable;
	
	public AudioClip upSound;
	public string command;
	public string upCommand = "";

	public GameObject subImage;
	public GameObject subImageRoot;

	private int currentStatus;
	private float initPosY = -90.0f;

	float lastY;

	// Update is called once per frame
	void Update () 
	{
		/*if (Input.GetKeyDown(upKeyCode) == true)
		{
			UIEventFacade.OnPointerSlide.OnNext (upCommand);
		}
		else if (Input.GetKeyDown(downKeyCode) == true)
		{
			UIEventFacade.OnPointerSlide.OnNext (command);
		}
		else if (Input.GetKeyUp(upKeyCode) == true || Input.GetKeyUp(downKeyCode) == true)
		{
			UIEventFacade.OnPointerSlide.OnNext ("Stop");
		}*/

		/*
		if (currentStatus == STATUS_NONE)
			return;

		if (currentStatus == STATUS_UP)
			UIEventFacade.OnPointerSlide.OnNext (upCommand);
		else
			UIEventFacade.OnPointerSlide.OnNext (command);
		*/

	}
	
	public void OnPointerDown() 
	{
		if (touchable == false)
			return;
		subImage.SetActive(true);
		lastY = Input.mousePosition.y;
	}

	public void OnDrag(){
		if (touchable == false)
			return;

		var currY = Input.mousePosition.y;
		if (currY > lastY) {
			UIEventFacade.OnPointerSlide.OnNext (upCommand);
		} else if( currY < lastY ) {
			UIEventFacade.OnPointerSlide.OnNext (command);
		}
		lastY = currY;

		SetSubImagePosition (Input.mousePosition);
		/*
		var offset = SetSubImagePosition (Input.mousePosition);
		if (offset.y > 0) {
			currentStatus = STATUS_UP;
		} else if (offset.y < 0) {
			currentStatus = STATUS_DOWN;
		}
		*/
		/*
		if (currentStatus == STATUS_UP)
			UIEventFacade.OnPointerSlide.OnNext (upCommand);
		else
			UIEventFacade.OnPointerSlide.OnNext (command);
		*/
		/*
		Animator animator = shiningImage.gameObject.GetComponent<Animator>();
		animator.SetTrigger("flash");
		if (bFever == true) {
			animator = feverImage.gameObject.GetComponent<Animator> ();
			animator.SetTrigger ("flash");
		}
		*/
	}

	Vector3 SetSubImagePosition(Vector3 mousePosition){
		Image image = subImage.GetComponent<Image>();
	// Input.mousePosition相當於所有手指Touch的平均位置
	// 所以若只有一個Touch就剛好等於MousePosition
		var worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		var canvasPos = Remix.Util.Instance.convertWolrdToCanvas(worldPos);
		var fixLength = 70f;
	// 反運算算出相對座標
		var newPos = canvasPos - subImageRoot.transform.localPosition;
		var centerPos = new Vector3 (0f, -90f, 0f);
	// 限制在圓內
		if (Vector3.Distance (newPos, centerPos) > fixLength) {
			var dir = Vector3.Normalize(newPos - centerPos);
			newPos = centerPos + dir * fixLength;
		}
		var oldPos = image.rectTransform.localPosition;
		image.rectTransform.localPosition = newPos;
		return newPos - oldPos;
	}
	
	public void OnPointerUp() 
	{
		if (touchable == false)
			return;

		Image image = subImage.GetComponent<Image>();
		Vector3 pos = image.rectTransform.localPosition;
		pos.y = initPosY;
		image.rectTransform.localPosition = pos;
		subImage.SetActive(false);
		// UIEventFacade.OnPointerSlide.OnNext ("Stop");
		currentStatus = STATUS_NONE;
	}

	/*
	public void SetVisible(bool visible)
	{
		buttonImage.enabled = visible;
		shiningImage.enabled = visible;
		feverImage.enabled = visible;
	}
	*/

	public void SetEnable(bool enable)
	{
		touchable = enable;
		if (enable == false)
		{
			Image image = subImage.GetComponent<Image>();
			Vector3 pos = image.rectTransform.localPosition;
			pos.y = initPosY;
			image.rectTransform.localPosition = pos;
			subImage.SetActive(false);
			currentStatus = STATUS_NONE;
		}
	}

	/// <summary>
	/// Delegate from GameFlowCtrl
	/// </summary>
	void OnFever()
	{
		
	}
}
