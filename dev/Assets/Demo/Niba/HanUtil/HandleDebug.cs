using System;
using UnityEngine;

namespace HanUtil
{
	public class HandleDebug : MonoBehaviour
	{
		public bool enable = false;
		public int textSize = 2000;
		public string version;

		#if DEBUG1
		string text;

		void OnGUI(){
			GUI.color = Color.red;
			GUILayout.BeginArea (new Rect (0, 0, 700, 700));
			if (GUILayout.Button (version+" debug")) {
				enable = !enable;
			}
			if (enable) {
				GUILayout.BeginScrollView (Vector2.zero);
				GUILayout.Label (text);
				GUILayout.EndScrollView ();
			}
			GUILayout.EndArea ();
		}
		#endif

		void AppendText(string msg){
			#if DEBUG1
			text = "[Debug]"+msg +"\n"+ text;
			if (text.Length > textSize) {
				text = text.Substring (0, textSize);
			}
			#endif
		}

		public void Log(string msg){
			// 注意：Debug.isDebugBuild都是Unity Main Thread的狀態，無法透過其它的Thread呼叫
			// 本遊戲在讀AssetBundle時會用到另一條Thread
			// 不使用Debug.isDebugBuild
			#if UNITY_EDITOR
			Debug.Log ("[Debug]"+msg);
			#endif
			AppendText (msg);
		}

		public void LogWarning(string msg){
			#if UNITY_EDITOR
			Debug.LogWarning ("[Debug]"+msg);
			#endif
			AppendText (msg);
		}

		public void LogError(string msg){
			Debug.LogError ("[Debug]"+msg);
			AppendText (msg);
		}
	}
}

