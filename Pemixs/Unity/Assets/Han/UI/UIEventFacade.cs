using System;
using UniRx;
using System.Collections.Generic;
using UnityEngine;

namespace Remix
{
	public class UIEventFacade
	{
		public static int paramIdx;
		public static Dictionary<string, object> paramsHolder = new Dictionary<string, object>();
		public static string RegisterParams(object v){
			var id = "params"+(paramIdx++)+"";
			paramsHolder [id] = v;
			return id;
		}

		public static object PopParams(string id){
			var param = paramsHolder [id];
			paramsHolder.Remove (id);
			return param;
		}

		public static Subject<string> OnPointerClick = new Subject<string>();

		public static Subject<string> OnPointerSlide = new Subject<string>();

		public static Subject<string> OnTransformGesture = new Subject<string>();

		public static Subject<string> OnTapGesture = new Subject<string>();

		public class ButtonDownAfterHandleResult{
			public string Command{get;set;}
			public bool IsSucceed{get;set;}
		}
		public static Subject<ButtonDownAfterHandleResult> OnButtonDownAfterHandle = new Subject<ButtonDownAfterHandleResult>();

		public class AudioClipRequest{
			public const string TypeMusic = "music";
			public const string TypeSound = "sound";
			public const string TypeNormalSound = "normalSound";
			public const string TypeGameMusic = "gameMusic";
			public string Type{ get; set; }
			public string TrackID{ get; set; }
			public bool IsPause{ get; set; }
			public bool IsMute{ get; set; }
			public bool IsLoop{ get; set; }
			public AudioClip Clip{ get; set; }
		}
		public static Subject<AudioClipRequest> OnAudioClipRequest = new Subject<AudioClipRequest>();

		public static Subject<string> OnGameEvent = new Subject<string>();

		public static Subject<string> OnTutorialModelEvent = new Subject<string>();
	}
}

