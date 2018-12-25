using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Remix{
	public class Util : MonoBehaviour
	{
		public static Util Instance;

		public Canvas canvas = null;
		public HandleAssetBundle handleAssetBundle;
		public HandlePhoto handlePhoto;
		public HandleDebug handleDebug;
		public LanguageText langText;
		public UserSettings userSettings;

		void Awake(){
			Instance = this;
		}

		#region debug
		public void Log(string msg){
			handleDebug.Log (msg);
		}

		public void LogWarning(string msg){
			handleDebug.LogWarning (msg);
		}

		public void LogError(string msg){
			handleDebug.LogError (msg);
		}
		#endregion

		#region tools
		public Vector3 convertWolrdToCanvas(Vector3 wroldPos) 
		{
			Vector3 screenPos = Camera.main.WorldToScreenPoint(wroldPos);
			Vector2 uiPos = Vector2.zero;
			CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
			uiPos.x = (screenPos.x / Screen.width * 2.0f - 1.0f) * canvasScaler.referenceResolution.x * 0.5f;
			uiPos.y = (screenPos.y / Screen.height * 2.0f - 1.0f) * canvasScaler.referenceResolution.y * 0.5f;
			Vector3 canvasPos = Vector3.zero;
			canvasPos.x = uiPos.x;
			canvasPos.y = uiPos.y;
			return canvasPos;
		}

		public Vector2 ConvertCanvasToScreen(Vector3 canvasPos) 
		{
			Vector2 pos2d;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, canvasPos, canvas.GetComponent<Camera>(), out pos2d);
			return pos2d;
		}
		#endregion

		#region asset management
		public void RequestUnloadUnusedAssets(){
			Resources.UnloadUnusedAssets ();
		}

		public UnityEngine.Object LoadAsset(string path, System.Type type){
			return handleAssetBundle.LoadAsset(path, type);
		}

		public void UnloadAsset(){
			handleAssetBundle.Unload ();
		}

		public GameObject GetPrefab(string resPath, GameObject anchorObj)
		{
			Util.Instance.Log("GetPrefab " + resPath);
			var prefab = LoadAsset(resPath, typeof(UnityEngine.Object));
			if (prefab == null) {
				throw new UnityException ("Prefab資源還沒準備好:"+resPath);
			}
			GameObject obj = GameObject.Instantiate(prefab) as GameObject;
			if (obj == null) {
				throw new UnityException ("prefab的type不是GameObject:"+resPath+" is "+prefab.GetType());
			}
			if (anchorObj != null)
			{
				obj.transform.SetParent(anchorObj.transform);
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localScale = Vector3.one;
			}
			return obj;
		}

		public Sprite GetPhoto(string photoName){
			return handlePhoto.GetPhoto (photoName);
		}

		public string[][] ParseCSV(string fileName)
		{
			//读取csv二进制文件  
			TextAsset binAsset = LoadAsset (fileName, typeof(TextAsset)) as TextAsset;         
			if (binAsset == null) {
				throw new UnityException ("檔名錯誤，請檢查:"+fileName);
			}
			//读取每一行的内容  
			string [] lineArray = binAsset.text.Split ("\r"[0]);  
			if (lineArray.Length <= 1) {
				Debug.LogWarning ("csv解析錯誤，使用\\n重解");
				lineArray = binAsset.text.Split ("\n"[0]);  
			}
			if (lineArray.Length <= 1) {
				throw new UnityException ("檔案無法解析");
			}

			//创建二维数组  
			string[][] strArray = new string [lineArray.Length][];  

			//把csv中的数据储存在二位数组中  
			for (int i=0;i<lineArray.Length;i++)  
			{  
				strArray[i] = lineArray[i].Split (',');  
			}
			return strArray;
		}

		public IEnumerator ParseCSVAsync(RemixApi.Either<string[][]> answer, string fileName)
		{
			var binAssetEither = new RemixApi.Either<UnityEngine.Object> ();
			yield return Util.Instance.LoadAssetAsync (binAssetEither, fileName, typeof(TextAsset));

			//读取csv二进制文件  
			TextAsset binAsset = binAssetEither.Ref as TextAsset;
			if (binAsset == null) {
				answer.Exception = new UnityException ("檔名錯誤，請檢查:"+fileName);
				yield break;
			}
			//读取每一行的内容  
			string [] lineArray = binAsset.text.Split ("\r"[0]);  
			if (lineArray.Length <= 1) {
				Debug.LogWarning ("csv解析錯誤，使用\\n重解");
				lineArray = binAsset.text.Split ("\n"[0]);  
			}
			if (lineArray.Length <= 1) {
				answer.Exception = new UnityException ("檔案無法解析");
				yield break;
			}

			//创建二维数组  
			string[][] strArray = new string [lineArray.Length][];  

			//把csv中的数据储存在二位数组中  
			for (int i=0;i<lineArray.Length;i++)  
			{  
				strArray[i] = lineArray[i].Split (',');  
			}
			answer.Ref = strArray;
		}
		#endregion

		#region async asset management
		public IEnumerator LoadAssetAsync(RemixApi.Either<UnityEngine.Object> answer, string path, System.Type type){
			return handleAssetBundle.LoadAssetAsync(answer, path, type);
		}

		public IEnumerator GetPrefabAsync(RemixApi.Either<GameObject> instance, string resPath, GameObject anchorObj)
		{
			var prefab = new RemixApi.Either<UnityEngine.Object> ();
			yield return handleAssetBundle.LoadAssetAsync(prefab, resPath, typeof(UnityEngine.Object));
			if (prefab.Exception != null) {
				instance.Exception = prefab.Exception;
				yield break;
			}
			if (prefab.Ref == null) {
				instance.Exception = new UnityException ("Prefab資源還沒準備好:"+resPath);
				yield break;
			}
			GameObject obj = GameObject.Instantiate(prefab.Ref) as GameObject;
			if (obj == null) {
				instance.Exception = new UnityException ("prefab的type不是GameObject:"+resPath+" is "+prefab.GetType());
				yield break;
			}
			if (anchorObj != null)
			{
				obj.transform.SetParent(anchorObj.transform);
				obj.transform.localPosition = Vector3.zero;
				obj.transform.localScale = Vector3.one;
			}
			instance.Ref = obj;
		}
		#endregion
	}
}