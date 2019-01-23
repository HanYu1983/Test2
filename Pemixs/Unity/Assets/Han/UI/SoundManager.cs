using System;
using UnityEngine;
using System.Collections.Generic;
using UniRx;

namespace Remix
{
	/*
	public class SoundManager : MonoBehaviour
	{
		public List<AudioClip> clips;

		void Start(){
			BindEvent ();
		}

		public void BindEvent(){
			UIEventFacade.OnButtonDownAfterHandle.Subscribe (
				info => {
					var buf = GetComponent<SoundBuffer> ();
					if(info.IsSucceed){
						buf.PlayClipInIdleChannel (clips [0], false, 0);
					} else {
						buf.PlayClipInIdleChannel (clips [1], false, 0);
					}
				},
				ex => {
				}
			);

			UIEventFacade.OnAudioClipRequest.Subscribe (
				request => {
					var buf = GetComponent<SoundBuffer> ();
					buf.PlayClipInIdleChannel (request.Clip, request.IsLoop, 0);
				},
				ex => {
				}
			);
		}
	}
	*/
}

