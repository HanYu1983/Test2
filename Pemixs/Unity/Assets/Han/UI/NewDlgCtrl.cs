using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Remix
{
	public class NewDlgCtrl : DialogCtrl
	{
		public Text nameText, titleText;
		public Image pic;
		public GameObject imgNew, imgOld;

		public bool IsNew{
			set{
				imgNew.SetActive (value);
				if (imgOld != null) {
					imgOld.SetActive (value == false);
				}
			}
		}

		public void SetTitle(string t){
			titleText.text = t;
		}

		public void SetName(string t){
			nameText.text = t;
		}

		public void LoadData(PhotoKey photo){
			var img = Util.Instance.GetPhoto (photo.PhotoNameForM);
            if (img != null)
            {
                pic.sprite = img;
            }
		}

		public void LoadData(ItemKey cat){
			if (cat.Type == StoreCtrl.DATA_CAT) {
				
			}
			switch (cat.Type) {
			case StoreCtrl.DATA_CAT:
				{
					var imageObj = Util.Instance.GetPrefab (cat.CatIconPrefabName("CM", GameConfig.CAT_STATE_ID.STATE_IDLE), null);
					pic.sprite = imageObj.GetComponent<Image> ().sprite;
					GameObject.Destroy (imageObj);
				}
				break;
			default:
				{
					var imageObj = Util.Instance.GetPrefab (cat.BuyItemPrefabName, null);
					pic.sprite = imageObj.GetComponent<Image> ().sprite;
					GameObject.Destroy (imageObj);
				}
				break;
			}
		}
	}
}