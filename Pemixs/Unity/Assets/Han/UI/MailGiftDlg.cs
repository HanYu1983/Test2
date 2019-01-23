using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Remix
{
	public class MailGiftDlg : MonoBehaviour{
		public Text textTitle;
		public GameObject itemDaysHolder;
		public int maxDay;

		public List<GameObject> itemDays;

		void Awake(){
			for (var i = 0; i < maxDay; ++i) {
				var name = string.Format ("ItemDay-{0:00}", i+1);
				var day = itemDaysHolder.transform.Find (name);
				if (day == null) {
					throw new UnityException ("沒有找到這個名字:"+name);
				}
				itemDays.Add (day.gameObject);
			}
		}

		public void SetTitle(string title){
			textTitle.text = title;
		}

		public void SetGetDay(int day, bool v){
			SetItemDay (itemDays [day], v);
		}

		void SetItemDay(GameObject itemDay, bool isGet){
			var getObj = itemDay.transform.Find ("Get");
			var newObj = itemDay.transform.Find ("New");
			getObj.gameObject.SetActive (isGet);
			newObj.gameObject.SetActive (isGet == false);
		}

		void SetItemImage(GameObject itemDay, ItemKey key){
			var image = itemDay.GetComponent<Image> ();
			if (key == null) {
				image.enabled = false;
				return;
			}
			var obj = Util.Instance.GetPrefab (key.StorageItemPrefabName, null);
			image.sprite = obj.GetComponent<Image> ().sprite;
			image.enabled = true;
			Destroy (obj);
		}

		public void SetItem(List<ItemKey> items){
			for (var i = 0; i < itemDays.Count; ++i) {
				SetItemImage (itemDays [i], i < items.Count ? items[i] : null);
			}
		}
	}
}

