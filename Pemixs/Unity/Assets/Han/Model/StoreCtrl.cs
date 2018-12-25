using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Remix{
	public class StoreCtrl 
	{
		public const string DATA_FOOD = "F";
		public const string DATA_CAMERA = "C";
		public const string DATA_S_CAMERA = "SC";
		public const string DATA_TOY = "T";
		public const string DATA_S_TOY = "ST";
		public const string DATA_TOY_CAPTURE = "TC";
		public const string DATA_MG = "MG";
		public const string DATA_MM = "MM";
		public const string DATA_CAT = "CAT";
		public const string DATA_CAT_LV = "CLV";
		public const string DATA_IAP = "IAP";

		public static List<ItemKey> GetItemKeys(string type){
			// 貓特殊處理
			if (type == DATA_CAT) {
				var r = new List<ItemKey> ();
				for (var i = 0; i < CatDef.ID_COUNT; ++i) {
					r.Add (new ItemKey (type, i));
				}
				return r;
			}
			if (type == DATA_CAMERA ||
				type == DATA_S_CAMERA ||
				type == DATA_TOY ||
				type == DATA_S_TOY ||
				type == DATA_FOOD ||
				type == DATA_TOY_CAPTURE || 
				type == DATA_MG ||
				type == DATA_MM ) {
				var r = new List<ItemKey> ();
				for (var i = 0; i < ItemDef.ID_COUNT; ++i) {
					var def = ItemDef.Get (i);
					if (def.Type != type) {
						continue;
					}
					// 忽略無效的道具
					if (def.Enable == 0) {
						continue;
					}
					r.Add (new ItemKey (type, i));
				}
				return r;
			}
			if (type == DATA_IAP) {
				var r = new List<ItemKey> ();
				for (var i = 0; i < IAPDefCht.ID_COUNT; ++i) {
					r.Add (new ItemKey (type, i));
				}
				return r;
			}
			throw new UnityException ("不支援的類型:"+type);
		}

		public static ItemKey GetWannaPlayItemKey(ItemKey cat, int idx){
			if (cat.Type != DATA_CAT) {
				throw new UnityException ("主鍵不是貓");
			}
			var catDef = CatDef.Get (cat.Idx);
			var items = new ItemKey[2];
			switch (catDef.Type) {
			case "CA":
				{
					// 雷射光筆
					items[0] = ItemKey.WithItemConfigID ("I30010");
					// 玩具老鼠
					items[1] = ItemKey.WithItemConfigID ("I30020");
				}
				break;
			case "CB":
				{
					// 逗貓棒
					items[0] = ItemKey.WithItemConfigID ("I30040");
					// 毛線球
					items[1] = ItemKey.WithItemConfigID ("I30050");
				}
				break;
			case "CC":
				{
					// 貓抓板
					items[0] = ItemKey.WithItemConfigID ("I30070");
					// 暖爐
					items[1] = ItemKey.WithItemConfigID ("I30080");
				}
				break;
			case "CM":
				{
					// 南瓜
					items[0] = ItemKey.WithItemConfigID ("I60010");
					// 木人樁
					items[1] = ItemKey.WithItemConfigID ("I60020");
				}
				break;
			default:
				throw new UnityException ("沒有這個體型:"+catDef.Type);
			}
			return items [idx];
		}
	}
}
