using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Remix{
	public class BuyItemDlgCtrl : MonoBehaviour
	{
		public const string TypeFood = "Food";
		public const string TypeCamera = "Camera";
		public const string TypeToy = "Toy";
		public const string TypeCToy = "CToy";

		public Text nameText;
		public Text ownCountText;
		public Text costText;
		public Text itemDescText;
		public Text pageText;
		public GameObject iconPivot;
		public ButtonCtrl buyBtn;
		public Image imgCostMoney, imgCostGold;

		LanguageText lt;
		int lang;

		private GameObject imageObj = null;
		private List<ItemKey> itemDataList = new List<ItemKey>();
		private int currentIdx;

		public void Left(QueryItemCount QueryItemCount){
			currentIdx = (currentIdx + itemDataList.Count - 1) % itemDataList.Count;
			SetItemData(currentIdx, QueryItemCount);
		}

		public void Right(QueryItemCount QueryItemCount){
			currentIdx = (currentIdx + 1) % itemDataList.Count;
			SetItemData(currentIdx, QueryItemCount);
		}

		public void ShowCostGold(){
			imgCostMoney.gameObject.SetActive (false);
			imgCostGold.gameObject.SetActive (true);
		}

		public void ShowCostMoney(){
			imgCostMoney.gameObject.SetActive (true);
			imgCostGold.gameObject.SetActive (false);
		}

		public delegate int QueryItemCount(ItemKey key);

		public void SetLanguage(LanguageText lt, int lang){
			this.lt = lt;
			this.lang = lang;
		}

		public void InitDlgType(string name, QueryItemCount QueryItemCount)
		{
			currentIdx = 0;
			itemDataList.Clear();
			switch (name)
			{
			case TypeFood:
				itemDataList.AddRange(StoreCtrl.GetItemKeys(StoreCtrl.DATA_FOOD));
				break;
			case TypeToy:
				itemDataList.AddRange(StoreCtrl.GetItemKeys(StoreCtrl.DATA_TOY));
				itemDataList.AddRange(StoreCtrl.GetItemKeys(StoreCtrl.DATA_S_TOY));
				break;
			case TypeCToy:
				itemDataList.AddRange(StoreCtrl.GetItemKeys(StoreCtrl.DATA_TOY_CAPTURE));
				itemDataList.AddRange(StoreCtrl.GetItemKeys(StoreCtrl.DATA_S_TOY));
				break;
			case TypeCamera:
				itemDataList.AddRange(StoreCtrl.GetItemKeys(StoreCtrl.DATA_CAMERA));
				itemDataList.AddRange(StoreCtrl.GetItemKeys(StoreCtrl.DATA_S_CAMERA));
				break;
			}

			SetItemData(currentIdx, QueryItemCount);
		}

		public void SetItemData(int index, QueryItemCount QueryItemCount)
		{
			var itemKey = itemDataList[index];
			string itemPrefab = itemKey.BuyItemPrefabName;

			if (imageObj != null)
			{
				imageObj.transform.SetParent(null);
				GameObject.DestroyObject(imageObj);
				imageObj = null;
			}
			imageObj = Util.Instance.GetPrefab(itemPrefab, iconPivot);
			var itemData = ItemDef.Get (itemKey.Idx);
			if (lt != null) {
				nameText.text = lt.GetItemName (lang, itemKey);
				itemDescText.text = lt.GetItemDesc (lang, itemKey);
			} else {
				Debug.LogWarning ("沒有呼叫SetLanguage, 忽略國際化文字設定");
			}

			int costGold = itemData.Gold;
			int costMoney = itemData.Money;
			var isCostGold = costGold > 0;

			if (isCostGold) {
				ShowCostGold ();
				costText.text = costGold + "";
				buyBtn.SetEnable(costGold > 0);
			} else {
				ShowCostMoney ();
				costText.text = costMoney + "";
				buyBtn.SetEnable(costMoney > 0);
			}

			int ownCount = QueryItemCount (itemKey);
			ownCountText.text = (ownCount <= 0) ? "0" : ownCount + "";

			pageText.text = (index+1) + "/" + itemDataList.Count;
		}

		public int CurrentItemIdx(){
			return currentIdx;
		}

		public ItemKey CurrentItemData(){
			return itemDataList[currentIdx];
		}
	}
}