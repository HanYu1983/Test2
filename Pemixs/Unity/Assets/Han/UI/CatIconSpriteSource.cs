using System;
using UnityEngine;
using System.Collections.Generic;

namespace Remix
{
	public class CatIconSpriteSource : MonoBehaviour
	{
		public List<Sprite> sprite;

		public Sprite GetCatIcon(int catId, GameConfig.CAT_STATE_ID state){
			var imageId = catId * 5;
			switch (state) {
			case GameConfig.CAT_STATE_ID.STATE_ANGRY:
				imageId += 1;
				break;
			case GameConfig.CAT_STATE_ID.STATE_WANNA_PLAY:
				imageId += 2;
				break;
			case GameConfig.CAT_STATE_ID.STATE_SLEEP:
				imageId += 3;
				break;
			case GameConfig.CAT_STATE_ID.STATE_HUNGRY:
				imageId += 4;
				break;
			}
			if (imageId < 0 || imageId >= sprite.Count) {
				throw new UnityException ("貓ICON沒有設定齊全");
			}
			return sprite [imageId];
		}
	}
}

