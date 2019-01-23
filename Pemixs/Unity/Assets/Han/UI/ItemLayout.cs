using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Remix{
	public class ItemLayout : MonoBehaviour
	{
		public GameObject maskObj;
		public GameObject anchorObj;
		public Text textCount;

		public void SetData(bool enable, int itemCount, string prefabName){
			textCount.text = itemCount + "";
			maskObj.SetActive(!enable);
			var imageObj = Util.Instance.GetPrefab (prefabName, null);
			// 為了向下相容
			anchorObj.GetComponent<Image> ().sprite = imageObj.GetComponent<Image> ().sprite;
			GameObject.Destroy (imageObj);
		}
	}
}