using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace Remix{
	public class ItemStorageDlgCtrl : MonoBehaviour 
	{
		private const float TOP_X = -185.0f;
		private const float TOP_Y = 160.0f;
		private const float PHOTO_WIDTH = 122.0f;
		private const float PHOTO_HEIGHT = 160.0f;
		private const int COUNT_PER_PAGE = 12;

		public GameObject ItemLayout;
		public Text uiTitle;
		public Text pageNumText;

		private GameConfig.ITEM_TYPE itemType;
		private List<ItemData> itemDataList = new List<ItemData>();
		private int pageId;
		private GameObject tutorialItemHint;

		public Dictionary<string,object> MetaData{ get; set; }

		void SetPageItem()
		{
			ItemLayout[] itemCtrlArray = this.GetComponentsInChildren<ItemLayout>();
			for (int i=0;i<itemCtrlArray.Length;++i)
			{
				GameObject itemObj = itemCtrlArray[i].gameObject;
				// 別刪了自己!!
				if (itemObj == ItemLayout) {
					continue;
				}
				if (itemObj != null)
				{
					itemObj.transform.SetParent(null);
					GameObject.DestroyObject(itemObj);
					itemObj = null;
				}
			}

			int startIdx = pageId * COUNT_PER_PAGE;
			int endIdx = (pageId+1) * COUNT_PER_PAGE;
			int count = (itemDataList.Count > endIdx) ? COUNT_PER_PAGE : (itemDataList.Count - startIdx);
			for (int i=0;i<count;++i)
			{
				var idx = i + startIdx;
				var itemData = itemDataList[idx];
				var row = i / 4;
				var col = i % 4;
				InitialItem(row, col, itemData);
			}

			int maxPageId = (itemDataList.Count - 1) / COUNT_PER_PAGE + 1;
			pageNumText.text = string.Format ("{0}/{1}", pageId+1, maxPageId);
		}

		Func<ItemKey, int> queryItemCount;
		Func<ItemKey, bool> queryItemEnable;

		public void InitialDialog(Func<ItemKey, int> QueryItemCount, Func<ItemKey, bool> QueryItemEnable)
		{
			queryItemCount = QueryItemCount;
			queryItemEnable = QueryItemEnable;

			var keys = new List<ItemKey> ();
			keys.AddRange (StoreCtrl.GetItemKeys (StoreCtrl.DATA_FOOD));
			keys.AddRange (StoreCtrl.GetItemKeys (StoreCtrl.DATA_CAMERA));
			keys.AddRange (StoreCtrl.GetItemKeys (StoreCtrl.DATA_S_CAMERA));
			keys.AddRange (StoreCtrl.GetItemKeys (StoreCtrl.DATA_TOY));
			keys.AddRange (StoreCtrl.GetItemKeys (StoreCtrl.DATA_TOY_CAPTURE));
			keys.AddRange (StoreCtrl.GetItemKeys (StoreCtrl.DATA_S_TOY));
			keys.AddRange (StoreCtrl.GetItemKeys (StoreCtrl.DATA_MG));
			keys.AddRange (StoreCtrl.GetItemKeys (StoreCtrl.DATA_MM));

			itemDataList.Clear();
			foreach(var key in keys){
				var itemCount = queryItemCount (key);
				if (itemCount <= 0) {
					continue;
				}
				ItemData itemData = new ItemData(itemCount, key);
				itemDataList.Add(itemData);
			}

			pageId = 0;
			SetPageItem();
		}

		public void UpdateUI(LanguageText lt, int lang){
			uiTitle.text = lt.GetDlgNote(lang, "StorageDlg");
		}

		public void Left(){
			int maxPageId = (itemDataList.Count - 1) / COUNT_PER_PAGE + 1;
			pageId = (pageId + maxPageId - 1) % maxPageId;
			SetPageItem();
		}

		public void Right(){
			int maxPageId = (itemDataList.Count - 1) / COUNT_PER_PAGE + 1;
			pageId = (pageId + 1) % maxPageId;
			SetPageItem();
		}

		void InitialItem(int row, int col, ItemData itemData)
		{
			var itemCount = itemData.count;
			var itemKey = itemData.itemKey;

			GameObject itemObj = Instantiate(ItemLayout) as GameObject;
			itemObj.transform.SetParent(this.gameObject.transform);
			float posX = TOP_X + col * PHOTO_WIDTH;
			float posY = TOP_Y - row * PHOTO_HEIGHT;
			itemObj.transform.localPosition = new Vector3(posX, posY, 0);
			itemObj.transform.localScale = Vector3.one;

			ItemLayout itemBtn = itemObj.GetComponent<ItemLayout>();

			ButtonCtrl btn = itemObj.GetComponent<ButtonCtrl> ();
			btn.command = "StorageDlgBtn"+itemKey.StringKey;

			bool itemEnable = queryItemEnable (itemKey);

			string prefabName = itemKey.StorageItemPrefabName;
			itemBtn.SetData (itemEnable, itemCount, prefabName);
			itemBtn.gameObject.SetActive (true);
			btn.SetEnable (itemEnable);
		}
	}

	class ItemData
	{
		public ItemData(int count, ItemKey key)
		{
			this.count = count;
			this.itemKey = key;
		}
		public int count;
		public ItemKey itemKey;
	}
}