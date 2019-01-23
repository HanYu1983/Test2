using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Linq;

namespace Remix{
	public class AlbumDlgCtrl : MonoBehaviour 
	{
	    private const float TOP_X = -185.0f;
	    private const float TOP_Y = 377.0f;
	    private const float PHOTO_WIDTH = 185.0f;
	    private const float PHOTO_HEIGHT = 177.0f;

	    public Text uiTitle;
	    public GameObject tutorialPhotoHint;
		public GameObject photoRoot, photoLayout, BPhotoObj, SPhotoObj;

	    public int smallPhotoCount;
	    public int bigPhotoCount;

		public delegate bool GetPhotoEnable(PhotoKey photoKey);

		public void LoadData (string mapIdx, GetPhotoEnable GetPhotoEnable) 
		{
			if (photoLayout == null) {
				Debug.LogWarning ("沒有設定photoLayout");
				return;
			}

			smallPhotoCount = GameRecord.GetPhotosCount (mapIdx, PhotoKey.TypeSmallPhoto);
			bigPhotoCount = GameRecord.GetPhotosCount (mapIdx, PhotoKey.TypeBigPhoto);

	        GameObject layoutObj = null;
	        
	        for (int i=0;i<smallPhotoCount;++i)
	        {
	            int row = i / 3;
	            int col = i % 3;
	            if (col == 0)
	            {
					layoutObj = Instantiate(photoLayout) as GameObject;
	                layoutObj.transform.SetParent(photoRoot.transform);
	                layoutObj.transform.localPosition = new Vector3(0, TOP_Y - row * PHOTO_HEIGHT, 0);
	                layoutObj.transform.localScale = Vector3.one;
	            }

	            float posX = TOP_X + col * PHOTO_WIDTH;
				var photoKey = new PhotoKey () {
					MapIdx = mapIdx,
					Idx = i,
					Type = PhotoKey.TypeSmallPhoto
				};
				InitialPhoto(layoutObj, posX, photoKey, GetPhotoEnable(photoKey));
	        }
	        for (int i=0;i<bigPhotoCount;++i)
	        {
	            int row = smallPhotoCount / 3 + i;
				layoutObj = Instantiate(photoLayout) as GameObject;
	            layoutObj.transform.SetParent(photoRoot.transform);
	            layoutObj.transform.localPosition = new Vector3(0, TOP_Y - row * PHOTO_HEIGHT, 0);
	            layoutObj.transform.localScale = Vector3.one;
				var photoKey = new PhotoKey () {
					MapIdx = mapIdx,
					Idx = i,
					Type = PhotoKey.TypeBigPhoto
				};
				InitialPhoto(layoutObj, 0.0f, photoKey, GetPhotoEnable(photoKey));
	        }
		}

		void InitialPhoto(GameObject layoutObj, float posX, PhotoKey photo, bool isPhotoEnable)
	    {
			GameObject prefab = (photo.IsBigPhoto == true) ? BPhotoObj : SPhotoObj;
			GameObject photoObj = Instantiate(prefab) as GameObject;
			var temp = photoObj.transform.localPosition;
			temp.x = posX;
			photoObj.transform.localPosition = temp;
			photoObj.transform.localScale = Vector3.one;
			photoObj.transform.SetParent(layoutObj.transform, false);

			var obj = photoObj.GetComponent<PhotoObj> ();
			obj.titleText.text = photo.PhotoDisplayName;

			ButtonCtrl lockedBtn = photoObj.GetComponentInChildren<ButtonCtrl>();
			lockedBtn.command = "AlbumDlgBtnPhoto_"+photo.StringKey;

			lockedBtn.SetEnable(isPhotoEnable);
			if (isPhotoEnable == true)
	        {
				var img = Util.Instance.GetPhoto(photo.PhotoNameForS);
                if(img != null)
                {
                    obj.photoAnchor.GetComponent<Image>().sprite = img;
                }
	        }
			photoObj.SetActive (true);
			layoutObj.SetActive (true);
	    }

		public void SetTitle(string text){
	        uiTitle.text = text;
		}
	}
}