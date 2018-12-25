using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Net;

namespace Remix{
	public class HandlePhoto : MonoBehaviour{
		public string photoPath;
		public int pixelPerUnit = 100;

		void Awake(){
			photoPath = RemixApi.API_HOST + "/Photos/";
		}

		public IEnumerator GetPhotoCoroutine(string photoName, Action<Exception, Sprite> after){
			yield return null;
			Exception error = null;
			Sprite ret = null;
			try{
				ret = GetPhoto(photoName);
			}catch(Exception e){
				error = new ShowMessageException (e.Message, e);
			}
			after (error, ret);
		}

		public Sprite GetPhoto(string photoName){
			try{
				var tex = RemixApi.GetPhoto (photoPath, photoName);
				var size = new Rect(0, 0, tex.width, tex.height);
				return Sprite.Create(tex,size,new Vector2(0.5f,0.5f), pixelPerUnit);
			}catch(Exception e){
				throw new ShowMessageException (e.Message, e);
			}
		}
	}
}

