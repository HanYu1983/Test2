using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Remix{
	public class StartGameDlgCtrl : MonoBehaviour 
	{
		public Text textTitle, textArtistTitle, textSongTitle;
		public Text textGameMode;
		public Image levelImage;
		public Image itemImage;
		public List<Image> rankObjs;
		public Sprite[] rankSpriteArray;

		public Dictionary<string,object> MetaData{ get; set; }

		public void SetLevelImage(Sprite image){
			levelImage.sprite = image;
		}

		public void SetTitle(string v){
			textTitle.text = v;
		}

		public void SetGameModeText(string text){
			textGameMode.text = text;
		}

		public void SetArtistTitle(string text){
			textArtistTitle.text = text;
		}

		public void SetSongTitle(string text){
			textSongTitle.text = text;
		}

		ItemKey useItem;

		public void SetUseItem(ItemKey key){
			if (key == null) {
				itemImage.enabled = false;
				return;
			}
			var obj = Util.Instance.GetPrefab (key.StorageItemPrefabName, null);
			itemImage.sprite = obj.GetComponent<Image> ().sprite;
			GameObject.Destroy (obj);
			itemImage.enabled = true;
			useItem = key;
		}

		public ItemKey GetUseItem(){
			return useItem;
		}

		public void SetHistoryEasyRank(int idx){
			if (idx == -1) {
				rankObjs [0].enabled = false;
				return;
			}
			rankObjs [0].sprite = rankSpriteArray [idx];
			rankObjs [0].enabled = true;
		}

		public void SetHistoryNormalRank(int idx){
			if (idx == -1) {
				rankObjs [1].enabled = false;
				return;
			}
			rankObjs [1].sprite = rankSpriteArray [idx];
			rankObjs [1].enabled = true;
		}

		public void SetHistoryHardRank(int idx){
			if (idx == -1) {
				rankObjs [2].enabled = false;
				return;
			}
			rankObjs [2].sprite = rankSpriteArray [idx];
			rankObjs [2].enabled = true;
		}
	}
}