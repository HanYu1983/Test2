using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace Remix{
	public class PhotoDlgCtrl : DialogCtrl 
	{
		public Text titleText;
		public GameObject photoAnchor;
		GameObject imageObj = null;

		public string key;
		Func<PhotoKey, bool> GetPhotoEnable;

		public void LoadPhoto(PhotoKey key, Func<PhotoKey, bool> GetPhotoEnable){
			if (imageObj != null)
			{
				imageObj.transform.SetParent(null);
				GameObject.DestroyObject(imageObj);
				imageObj = null;
			}
			titleText.text = key.PhotoDisplayName;
			var img = Util.Instance.GetPhoto (key.PhotoNameForB);
            if (img != null)
            {
                var imgCom = photoAnchor.GetComponent<Image>();
                if (imgCom == null)
                {
                    imgCom = photoAnchor.gameObject.AddComponent<Image>();
                }
                imgCom.rectTransform.sizeDelta = new Vector2(img.texture.width, img.texture.height);
                imgCom.sprite = img;
            }
			this.key = key.StringKey;
			this.GetPhotoEnable = GetPhotoEnable;
		}

		public void NextAndLoad(){
			var nextKey = new PhotoKey (key).NextKey;
			while (GetPhotoEnable(nextKey) == false) {
				nextKey = nextKey.NextKey;
			}
			LoadPhoto (nextKey, GetPhotoEnable);
		}

		public void PrevAndLoad(){
			var nextKey = new PhotoKey (key).PrevKey;
			while (GetPhotoEnable(nextKey) == false) {
				nextKey = nextKey.PrevKey;
			}
			LoadPhoto (nextKey, GetPhotoEnable);
		}
	}

}
