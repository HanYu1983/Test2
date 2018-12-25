using UnityEngine;
namespace Remix{
	public class DlgNoteCht{
		public const int ID_COUNT = 27;

		public string ID {get; set;}
public string Desc {get; set;}

		public static DlgNoteCht Get(string key){
			switch (key) {
			case "IAPDlg": return new DlgNoteCht{ID="IAPDlg",Desc="取得貓鈔票與金幣"};
case "LoadDlg": return new DlgNoteCht{ID="LoadDlg",Desc="讀取中"};
case "NewCatDlg": return new DlgNoteCht{ID="NewCatDlg",Desc="獲得貓咪"};
case "NewSPhotoDlg": return new DlgNoteCht{ID="NewSPhotoDlg",Desc="獲得照片"};
case "NewBPhotoDlg": return new DlgNoteCht{ID="NewBPhotoDlg",Desc="獲得環景照片"};
case "StorageDlg": return new DlgNoteCht{ID="StorageDlg",Desc="道具倉庫"};
case "BuyGachaDlg": return new DlgNoteCht{ID="BuyGachaDlg",Desc="解鎖貓咪遊樂場"};
case "MailGiftDlg": return new DlgNoteCht{ID="MailGiftDlg",Desc="每日禮物"};
case "NewItemDlg": return new DlgNoteCht{ID="NewItemDlg",Desc="獲得道具"};
case "LoginDlg": return new DlgNoteCht{ID="LoginDlg",Desc="輸入暱稱"};
case "DownloadDlg": return new DlgNoteCht{ID="DownloadDlg",Desc="資料更新中..."};
case "TutorialDlg": return new DlgNoteCht{ID="TutorialDlg",Desc="遊戲教學"};
case "StaffDlg": return new DlgNoteCht{ID="StaffDlg",Desc="製作人員名單"};
case "LanguageDlg": return new DlgNoteCht{ID="LanguageDlg",Desc="更換語言"};
case "InviteDlg": return new DlgNoteCht{ID="InviteDlg",Desc="招待好友"};
case "MessegnDlg": return new DlgNoteCht{ID="MessegnDlg",Desc="訊息"};
case "TransferDlg": return new DlgNoteCht{ID="TransferDlg",Desc="資料轉移"};
case "AlbumDlg01": return new DlgNoteCht{ID="AlbumDlg01",Desc="猴硐的照片"};
case "AlbumDlg02": return new DlgNoteCht{ID="AlbumDlg02",Desc="座間味的照片"};
case "AlbumDlg03": return new DlgNoteCht{ID="AlbumDlg03",Desc="渡嘉敷的照片"};
case "AlbumDlg04": return new DlgNoteCht{ID="AlbumDlg04",Desc="台鹼宿舍的照片"};
case "AlbumDlg05": return new DlgNoteCht{ID="AlbumDlg05",Desc="蘭嶼的照片"};
case "AlbumDlg06": return new DlgNoteCht{ID="AlbumDlg06",Desc="竹富的照片"};
case "AlbumDlgS1": return new DlgNoteCht{ID="AlbumDlgS1",Desc="澎湖的照片"};
case "AlbumDlgS2": return new DlgNoteCht{ID="AlbumDlgS2",Desc="那霸的照片"};
case "MusicDlg": return new DlgNoteCht{ID="MusicDlg",Desc="音樂播放"};
case "StageitemDlg": return new DlgNoteCht{ID="StageitemDlg",Desc="過關獎勵機率表"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static DlgNoteCht Get(int key){
			switch (key) {
			case 0: return new DlgNoteCht{ID="IAPDlg",Desc="取得貓鈔票與金幣"};
case 1: return new DlgNoteCht{ID="LoadDlg",Desc="讀取中"};
case 2: return new DlgNoteCht{ID="NewCatDlg",Desc="獲得貓咪"};
case 3: return new DlgNoteCht{ID="NewSPhotoDlg",Desc="獲得照片"};
case 4: return new DlgNoteCht{ID="NewBPhotoDlg",Desc="獲得環景照片"};
case 5: return new DlgNoteCht{ID="StorageDlg",Desc="道具倉庫"};
case 6: return new DlgNoteCht{ID="BuyGachaDlg",Desc="解鎖貓咪遊樂場"};
case 7: return new DlgNoteCht{ID="MailGiftDlg",Desc="每日禮物"};
case 8: return new DlgNoteCht{ID="NewItemDlg",Desc="獲得道具"};
case 9: return new DlgNoteCht{ID="LoginDlg",Desc="輸入暱稱"};
case 10: return new DlgNoteCht{ID="DownloadDlg",Desc="資料更新中..."};
case 11: return new DlgNoteCht{ID="TutorialDlg",Desc="遊戲教學"};
case 12: return new DlgNoteCht{ID="StaffDlg",Desc="製作人員名單"};
case 13: return new DlgNoteCht{ID="LanguageDlg",Desc="更換語言"};
case 14: return new DlgNoteCht{ID="InviteDlg",Desc="招待好友"};
case 15: return new DlgNoteCht{ID="MessegnDlg",Desc="訊息"};
case 16: return new DlgNoteCht{ID="TransferDlg",Desc="資料轉移"};
case 17: return new DlgNoteCht{ID="AlbumDlg01",Desc="猴硐的照片"};
case 18: return new DlgNoteCht{ID="AlbumDlg02",Desc="座間味的照片"};
case 19: return new DlgNoteCht{ID="AlbumDlg03",Desc="渡嘉敷的照片"};
case 20: return new DlgNoteCht{ID="AlbumDlg04",Desc="台鹼宿舍的照片"};
case 21: return new DlgNoteCht{ID="AlbumDlg05",Desc="蘭嶼的照片"};
case 22: return new DlgNoteCht{ID="AlbumDlg06",Desc="竹富的照片"};
case 23: return new DlgNoteCht{ID="AlbumDlgS1",Desc="澎湖的照片"};
case 24: return new DlgNoteCht{ID="AlbumDlgS2",Desc="那霸的照片"};
case 25: return new DlgNoteCht{ID="MusicDlg",Desc="音樂播放"};
case 26: return new DlgNoteCht{ID="StageitemDlg",Desc="過關獎勵機率表"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}