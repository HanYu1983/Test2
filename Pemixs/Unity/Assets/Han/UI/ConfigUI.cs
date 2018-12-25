using System;
using UnityEngine;
using UnityEngine.UI;

namespace Remix
{
	public class ConfigUI : MonoBehaviour
	{
		public Text txtTur, txtSync, txtData, txtLang, txtStaff;

		public void UpdateUI(LanguageText lt, int lang){
			txtTur.text = lt.GetMainuiNote (lang, "MUI5B01");
			txtSync.text = lt.GetMainuiNote (lang, "MUI5B02");
			txtData.text = lt.GetMainuiNote (lang, "MUI5B03");
			txtLang.text = lt.GetMainuiNote (lang, "MUI5B04");
			txtStaff.text = lt.GetMainuiNote (lang, "MUI5B05");
		}
	}
}

