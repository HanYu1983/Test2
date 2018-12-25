using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

namespace Remix{
	public class ScoreDlgCtrl : MonoBehaviour 
	{
		public Text textTitle;
		public Text textGameMode;
		public Text textRecord;
		public Text textCombo;
		public Text textGold;
		public Image difficultySprite;
		public Image rankSprite;
		public GameObject[] clearOrFail;
		public Image itemSprite;
		public Sprite[] rankSpriteArray;
		public Sprite[] difficultySpriteArray;
		public Image levelPic;

		public void SetDifficulty(int dif){
			switch (dif) {
			case GameRecord.Easy:
				difficultySprite.sprite = difficultySpriteArray [0];
				break;
			case GameRecord.Normal:
				difficultySprite.sprite = difficultySpriteArray [1];
				break;
			case GameRecord.Hard:
				difficultySprite.sprite = difficultySpriteArray [2];
				break;
			}
		}

		public Sprite LevelPic {
			set {
				levelPic.sprite = value;
			}
		}

		public void SetGold(int gold){
			textGold.text = gold + "";
		}

		public void SetCombo(int combo){
			textCombo.text = string.Format ("Combo:{0}", combo);
		}

		public void SetRank(int rank){
			rankSprite.sprite = rankSpriteArray [rank];
		}

		public void SetTitle(string text){
			textTitle.text = text;
		}

		public void SetGameModeText(string text)
		{
			textGameMode.text = text;
		}

		public void SetIsWin(bool isWin){
			clearOrFail [0].SetActive (isWin == true);
			clearOrFail [1].SetActive (isWin == false);
		}

		public void SetLevelScore(int score)
		{
			if (textRecord != null) {
				textRecord.text = string.Format("Score:{0}", score);
			}
		}

		public void SetLevelReward(ItemKey item, ref Exception error)
		{
			if (item == null) {
				itemSprite.enabled = false;
				return;
			}
			try{
				var obj = Util.Instance.GetPrefab(item.StorageItemPrefabName, null);
				itemSprite.sprite = obj.GetComponent<Image> ().sprite;
				Destroy (obj);
				itemSprite.enabled = true;
			} catch(Exception e){
				error = e;
			}
		}
	}

}
