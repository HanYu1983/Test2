using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using TouchScript.Gestures;

namespace Remix{
	public class ButtonCtrl : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
	{
		public Sprite normalSprite;
		public Sprite pressSprite;
		public Sprite disableSprite;
		public string command;
		public bool changeToPointerDown;

		public bool touchable = false;
		// 如果有button按了沒反應
		// 注意是不是有CanvasGroup把射線檔住了
		// 也有可能是被哪一層的UI檔住，要仔細檢查每一層。若被那層檔住，把它的Raycast Target勾掉
		public void OnPointerDown(PointerEventData evt) {
			Util.Instance.Log ("OnPointerDown:"+command);
			if (touchable == false)
				return;

			if (changeToPointerDown) {
				UIEventFacade.OnPointerClick.OnNext (command);
			}

			if (pressSprite != null)
			{
				Image image = this.GetComponent<Image>();
				if (image != null) {
					image.sprite = pressSprite;
				}
				SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
				if (renderer != null) {
					renderer.sprite = pressSprite;
				}
			}
		}

		public void OnPointerUp(PointerEventData evt) {
			Util.Instance.Log ("OnPointerUp:"+command);
			if (touchable == false)
				return;

			if (normalSprite != null) {
				Image image = this.GetComponent<Image>();
				if (image != null) {
					image.sprite = normalSprite;
				}
				SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
				if (renderer != null) {
					renderer.sprite = normalSprite;
				}
			}
		}

		public void OnPointerClick(PointerEventData evt){
			Util.Instance.Log ("OnPointerClick:"+command);
			if (changeToPointerDown) {
				return;
			}
			if (touchable == false)
				return;
			UIEventFacade.OnPointerClick.OnNext (command);
		}

		public void SetEnable(bool enable)
		{
			Image image = this.GetComponent<Image>();
			if (image != null) {
				if (enable == true)
				{
					if(normalSprite != null)
						image.sprite = normalSprite;
				}
				else
				{
					if(disableSprite != null)
						image.sprite = disableSprite;
				}
			}
			SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
			if (renderer != null) {
				if (enable == true)
				{
					if(normalSprite != null)
						renderer.sprite = normalSprite;
				}
				else
				{
					if(disableSprite != null)
						renderer.sprite = disableSprite;
				}
			}

			touchable = enable;
		}

		public void SetVisible(bool visible)
		{
			Image image = this.GetComponent<Image>();
			if (image != null) {
				image.enabled = visible;
			}
			SpriteRenderer renderer = this.GetComponent<SpriteRenderer>();
			if (renderer != null) {
				renderer.enabled = visible;
			}
		}
	}
}