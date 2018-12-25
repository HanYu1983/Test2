using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace Remix
{
	public class Tips : MonoBehaviour {
		public GameObject root;
		public Image imgCatIcon;
		public Text txtTitle, txtDesc;
		// 使用全域變數用來記得上次的狀態
		static int currentTip = -1;

		int lastUpdateTip = -1;

		public bool Visible{
			set{
				root.SetActive (value);
			}
		}

		public void RandomTip(){
			currentTip = UnityEngine.Random.Range (0, TipsNoteCht.ID_COUNT);
		}

		public void NextTip(){
			currentTip += 1;
			if (currentTip >= TipsNoteCht.ID_COUNT) {
				currentTip = 0;
			}
		}

		public void UpdateUI(LanguageText lt, int lang, IModel model){
			if (currentTip == -1) {
				currentTip = 0;
			}
			var title = lt.GetTipsTitle (lang, currentTip);
			var desc = lt.GetTipsDesc (lang, currentTip);
			var ch = lt.GetTipsCh (lang, currentTip);

			txtTitle.text = title;
			txtDesc.text = desc;

			if (lastUpdateTip != currentTip) {
				lastUpdateTip = currentTip;

				var numstr = "";
				string pattern = @"CI(?<num>\d{2})"; // 規則字串
				Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
				MatchCollection matches = regex.Matches (ch);
				foreach (Match match in matches) {
					GroupCollection groups = match.Groups;
					numstr = groups ["num"].Value;
					break;
				}

				if (numstr == "") {
					throw new UnityException ("資料輸入錯誤，無法取得貓索引:"+ch);
				}

				var catId = int.Parse (numstr) - 1;
				var catKey = new ItemKey (StoreCtrl.DATA_CAT, catId);
				var imgName = catKey.CatIconPrefabName ("CI", GameConfig.CAT_STATE_ID.STATE_IDLE);
				var tmp = Util.Instance.GetPrefab (imgName, null);
				imgCatIcon.sprite = tmp.GetComponent<Image> ().sprite;
				GameObject.Destroy (tmp);
			}
		}
	}
}