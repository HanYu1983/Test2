using System;
using UnityEngine;

namespace Remix
{
	public class LanguageText : MonoBehaviour
	{
		public const int En = 0;
		public const int Ch = 1;
		public const int Chs = 2;

		public string GetMainuiNote(int lang, string key){
			switch (lang) {
			case Ch:
				return MainuiNoteCht.Get (key).Desc;
			case Chs:
				return MainuiNoteChs.Get (key).Desc;
			default:
				return MainuiNoteEng.Get (key).Desc; 
			}
		}

		public string GetArtistTitle(int lang, LevelKey key){
			switch (lang) {
			case Ch:
				return LevelNoteCht.Get (key.ConfigID).TextArtistTitle;
			case Chs:
				return LevelNoteChs.Get (key.ConfigID).TextArtistTitle;
			default:
				return LevelNoteEng.Get (key.ConfigID).TextArtistTitle; 
			}
		}

		public string GetSongTitle(int lang, LevelKey key){
			switch (lang) {
			case Ch:
				return LevelNoteCht.Get (key.ConfigID).TextSongTitle;
			case Chs:
				return LevelNoteChs.Get (key.ConfigID).TextSongTitle;
			default:
				return LevelNoteEng.Get (key.ConfigID).TextSongTitle; 
			}
		}

		public string GetLevelName(int lang, LevelKey key){
			switch (lang) {
			case Ch:
				return LevelNoteCht.Get (key.ConfigID).Name;
			case Chs:
				return LevelNoteChs.Get (key.ConfigID).Name;
			default:
				return LevelNoteEng.Get (key.ConfigID).Name; 
			}
		}

		public string GetGameModeText(int lang, LevelKey key){
			switch (lang) {
			case Ch:
				return LevelNoteCht.Get (key.ConfigID).TextGameMode;
			case Chs:
				return LevelNoteChs.Get (key.ConfigID).TextGameMode;
			default:
				return LevelNoteEng.Get (key.ConfigID).TextGameMode; 
			}
		}

		public string GetCatName(int lang, ItemKey key){
			switch (lang) {
			case Ch:
				return CatNoteCht.Get (key.Idx).Name;
			case Chs:
				return CatNoteChs.Get (key.Idx).Name;
			default:
				return CatNoteEng.Get (key.Idx).Name; 
			}
		}

		public string GetCatSkillNote(int lang, ItemKey key){
			switch (lang) {
			case Ch:
				return CatNoteCht.Get (key.Idx).SkillNote;
			case Chs:
				return CatNoteChs.Get (key.Idx).SkillNote;
			default:
				return CatNoteEng.Get (key.Idx).SkillNote; 
			}
		}

		public string GetItemName(int lang, ItemKey key){
			try{
				switch (lang) {
				case Ch:
					return ItemNoteCht.Get (key.Idx).Name;
				case Chs:
					return ItemNoteChs.Get (key.Idx).Name;
				default:
					return ItemNoteEng.Get (key.Idx).Name; 
				}
			}catch(Exception e){
				Debug.LogError ("item_note_[lang]中定義的資料缺少:"+e.Message);
				return "";
			}
		}

		public string GetItemDesc(int lang, ItemKey key){
			try{
				switch (lang) {
				case Ch:
					return ItemNoteCht.Get (key.Idx).Desc;
				case Chs:
					return ItemNoteChs.Get (key.Idx).Desc;
				default:
					return ItemNoteEng.Get (key.Idx).Desc; 
				}
			}catch(Exception e){
				Debug.LogError ("item_note_[lang]中定義的資料缺少:"+e.Message);
				return "";
			}
		}

		public string GetWorldMapDesc(int lang, string key){
			for (var i = 0; i < WmapSCht.ID_COUNT; ++i) {
				if (WmapSCht.Get (i).Name == key) {
					switch (lang) {
					case Ch:
						return WmapSCht.Get (i).Desc;
					case Chs:
						return WmapSChs.Get (i).Desc;
					default:
						return WmapSEng.Get (i).Desc; 
					}
				}
			}
			Debug.LogError ("map中定義的資料缺少");
			return "";
		}

		public string GetDlgNote(int lang, string key){
			try{
				switch (lang) {
				case Ch:
					return DlgNoteCht.Get (key).Desc;
				case Chs:
					return DlgNoteChs.Get (key).Desc;
				default:
					return DlgNoteEng.Get (key).Desc; 
				}
			}catch(Exception e){
				Debug.LogError ("item_note_[lang]中定義的資料缺少:"+e.Message);
				return "";
			}
		}

		public string GetDlgMessage(int lang, string key){
			try{
				switch (lang) {
				case Ch:
					return DlgMessageCht.Get (key).Desc;
				case Chs:
					return DlgMessageChs.Get (key).Desc;
				default:
					return DlgMessageEng.Get (key).Desc; 
				}
			}catch(Exception e){
				Debug.LogError ("item_note_[lang]中定義的資料缺少:"+e.Message);
				return "";
			}
		}

		public string GetIAPName(int lang, int key){
			try{
				switch (lang) {
				case Ch:
					return IAPDefCht.Get (key).Name;
				case Chs:
					return IAPDefChs.Get (key).Name;
				default:
					return IAPDefEng.Get (key).Name; 
				}
			}catch(Exception e){
				Debug.LogError ("iap_def_[lang]中定義的資料缺少:"+e.Message);
				return "";
			}
		}

		public string GetIAPDesc(int lang, int key){
			try{
				switch (lang) {
				case Ch:
					return IAPDefCht.Get (key).Desc;
				case Chs:
					return IAPDefChs.Get (key).Desc;
				default:
					return IAPDefEng.Get (key).Desc; 
				}
			}catch(Exception e){
				Debug.LogError ("iap_def_[lang]中定義的資料缺少:"+e.Message);
				return "";
			}
		}

		public string GetIAPCost(int lang, int key){
			try{
				switch (lang) {
				case Ch:
					return IAPDefCht.Get (key).Cost;
				case Chs:
					return IAPDefChs.Get (key).Cost;
				default:
					return IAPDefEng.Get (key).Cost; 
				}
			}catch(Exception e){
				Debug.LogError ("iap_def_[lang]中定義的資料缺少:"+e.Message);
				return "";
			}
		}

		public string GetMusicArtist(int lang, string key){
			try{
				switch (lang) {
				case Ch:
					return MusicNoteCht.Get (key).ArtistTitle;
				case Chs:
					return MusicNoteChs.Get (key).ArtistTitle;
				default:
					return MusicNoteEng.Get (key).ArtistTitle; 
				}
			}catch(Exception e){
				Debug.LogError ("musicnote_def_[lang]中定義的資料缺少:"+e.Message);
				return "";
			}
		}

		public string GetMusicSong(int lang, string key){
			try{
				switch (lang) {
				case Ch:
					return MusicNoteCht.Get (key).SongTitle;
				case Chs:
					return MusicNoteChs.Get (key).SongTitle;
				default:
					return MusicNoteEng.Get (key).SongTitle; 
				}
			}catch(Exception e){
				Debug.LogError ("musicnote_def_[lang]中定義的資料缺少:"+e.Message);
				return "";
			}
		}

		public string GetTutorialNoteDesc(int lang, string key){
			try{
				switch (lang) {
				case Ch:
					return TutorialNoteCht.Get (key).Desc;
				case Chs:
					return TutorialNoteChs.Get (key).Desc;
				default:
					return TutorialNoteEng.Get (key).Desc; 
				}
			}catch(Exception e){
				Debug.LogError ("tutorialNote_def_[lang]中定義的資料缺少:"+e.Message);
				return "";
			}
		}

		public string GetTipsTitle(int lang, int key){
			try{
				switch (lang) {
				case Ch:
					return TipsNoteCht.Get (key).Title;
				case Chs:
					return TipsNoteChs.Get (key).Title;
				default:
					return TipsNoteEng.Get (key).Title; 
				}
			}catch(Exception e){
				Debug.LogError (e.Message);
				return "";
			}
		}

		public string GetTipsDesc(int lang, int key){
			try{
				switch (lang) {
				case Ch:
					return TipsNoteCht.Get (key).Desc;
				case Chs:
					return TipsNoteChs.Get (key).Desc;
				default:
					return TipsNoteEng.Get (key).Desc; 
				}
			}catch(Exception e){
				Debug.LogError (e.Message);
				return "";
			}
		}

		public string GetTipsCh(int lang, int key){
			try{
				switch (lang) {
				case Ch:
					return TipsNoteCht.Get (key).Ch;
				case Chs:
					return TipsNoteChs.Get (key).Ch;
				default:
					return TipsNoteEng.Get (key).Ch; 
				}
			}catch(Exception e){
				Debug.LogError (e.Message);
				return "";
			}
		}
			
		public string GetTrainingDesc(int lang, int key){
			try{
				switch (lang) {
				case Ch:
					return TrainingNoteCht.Get (key).Desc;
				case Chs:
					return TrainingNoteChs.Get (key).Desc;
				default:
					return TrainingNoteEng.Get (key).Desc; 
				}
			}catch(Exception e){
				Debug.LogError (e.Message);
				return "";
			}
		}

		public string FormatExceptionString(int lang, string msg){
			try{
				switch (lang) {
				case Ch:
					if (msg.Contains ("device not found with inviteCode")) {
						return DlgMessageCht.Get("MesgText_I06").Desc;
					}
					if(msg.Contains("invite count reach limit")){
						return DlgMessageCht.Get("MesgText_I07").Desc;
					}
					if(msg.Contains("device not found with transferCode")){
						return DlgMessageCht.Get("MesgText_T06").Desc;
					}
					if(msg.Contains("can not transfer myself")){
						return DlgMessageCht.Get("MesgText_T06").Desc;
					}
					break;
				case Chs:
					if (msg.Contains ("device not found with inviteCode")) {
						return DlgMessageChs.Get("MesgText_I06").Desc;
					}
					if(msg.Contains("invite count reach limit")){
						return DlgMessageChs.Get("MesgText_I07").Desc;
					}
					if(msg.Contains("device not found with transferCode")){
						return DlgMessageChs.Get("MesgText_T06").Desc;
					}
					if(msg.Contains("can not transfer myself")){
						return DlgMessageChs.Get("MesgText_T06").Desc;
					}
					break;
				case En:
					if (msg.Contains ("device not found with inviteCode")) {
						return DlgMessageEng.Get("MesgText_I06").Desc;
					}
					if(msg.Contains("invite count reach limit")){
						return DlgMessageEng.Get("MesgText_I07").Desc;
					}
					if(msg.Contains("device not found with transferCode")){
						return DlgMessageEng.Get("MesgText_T06").Desc;
					}
					if(msg.Contains("can not transfer myself")){
						return DlgMessageEng.Get("MesgText_T06").Desc;
					}
					break;
				}
				return msg;
			}catch(Exception e){
				Debug.LogError (e.Message);
				return "";
			}
		}
	}
}

