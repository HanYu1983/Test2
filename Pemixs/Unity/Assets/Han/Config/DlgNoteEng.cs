using UnityEngine;
namespace Remix{
	public class DlgNoteEng{
		public const int ID_COUNT = 27;

		public string ID {get; set;}
public string Desc {get; set;}

		public static DlgNoteEng Get(string key){
			switch (key) {
			case "IAPDlg": return new DlgNoteEng{ID="IAPDlg",Desc="Get More Ticket & Gold"};
case "LoadDlg": return new DlgNoteEng{ID="LoadDlg",Desc="Loading"};
case "NewCatDlg": return new DlgNoteEng{ID="NewCatDlg",Desc="Get Cat"};
case "NewSPhotoDlg": return new DlgNoteEng{ID="NewSPhotoDlg",Desc="Get Photo"};
case "NewBPhotoDlg": return new DlgNoteEng{ID="NewBPhotoDlg",Desc="Get Panorama Photo"};
case "StorageDlg": return new DlgNoteEng{ID="StorageDlg",Desc="Item Storage"};
case "BuyGachaDlg": return new DlgNoteEng{ID="BuyGachaDlg",Desc="Ulock Cat Tree"};
case "MailGiftDlg": return new DlgNoteEng{ID="MailGiftDlg",Desc="Gift List"};
case "NewItemDlg": return new DlgNoteEng{ID="NewItemDlg",Desc="Get Item"};
case "LoginDlg": return new DlgNoteEng{ID="LoginDlg",Desc="Enter Your Name"};
case "DownloadDlg": return new DlgNoteEng{ID="DownloadDlg",Desc="Download Game Data..."};
case "TutorialDlg": return new DlgNoteEng{ID="TutorialDlg",Desc="Tutorial"};
case "StaffDlg": return new DlgNoteEng{ID="StaffDlg",Desc="Staff List"};
case "LanguageDlg": return new DlgNoteEng{ID="LanguageDlg",Desc="Change Language"};
case "InviteDlg": return new DlgNoteEng{ID="InviteDlg",Desc="Invite Friend"};
case "MessegnDlg": return new DlgNoteEng{ID="MessegnDlg",Desc="Message"};
case "TransferDlg": return new DlgNoteEng{ID="TransferDlg",Desc="Data Transfer"};
case "AlbumDlg01": return new DlgNoteEng{ID="AlbumDlg01",Desc="Photo of Houtong"};
case "AlbumDlg02": return new DlgNoteEng{ID="AlbumDlg02",Desc="Photo of Zamami"};
case "AlbumDlg03": return new DlgNoteEng{ID="AlbumDlg03",Desc="Photo of Tokashiki"};
case "AlbumDlg04": return new DlgNoteEng{ID="AlbumDlg04",Desc="Photo of CPDC"};
case "AlbumDlg05": return new DlgNoteEng{ID="AlbumDlg05",Desc="Photo of Orchid"};
case "AlbumDlg06": return new DlgNoteEng{ID="AlbumDlg06",Desc="Photo of Taketomi"};
case "AlbumDlgS1": return new DlgNoteEng{ID="AlbumDlgS1",Desc="Photo of Penghu"};
case "AlbumDlgS2": return new DlgNoteEng{ID="AlbumDlgS2",Desc="Photo of Naha"};
case "MusicDlg": return new DlgNoteEng{ID="MusicDlg",Desc="Music Player"};
case "StageitemDlg": return new DlgNoteEng{ID="StageitemDlg",Desc="Level Reward Probability"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static DlgNoteEng Get(int key){
			switch (key) {
			case 0: return new DlgNoteEng{ID="IAPDlg",Desc="Get More Ticket & Gold"};
case 1: return new DlgNoteEng{ID="LoadDlg",Desc="Loading"};
case 2: return new DlgNoteEng{ID="NewCatDlg",Desc="Get Cat"};
case 3: return new DlgNoteEng{ID="NewSPhotoDlg",Desc="Get Photo"};
case 4: return new DlgNoteEng{ID="NewBPhotoDlg",Desc="Get Panorama Photo"};
case 5: return new DlgNoteEng{ID="StorageDlg",Desc="Item Storage"};
case 6: return new DlgNoteEng{ID="BuyGachaDlg",Desc="Ulock Cat Tree"};
case 7: return new DlgNoteEng{ID="MailGiftDlg",Desc="Gift List"};
case 8: return new DlgNoteEng{ID="NewItemDlg",Desc="Get Item"};
case 9: return new DlgNoteEng{ID="LoginDlg",Desc="Enter Your Name"};
case 10: return new DlgNoteEng{ID="DownloadDlg",Desc="Download Game Data..."};
case 11: return new DlgNoteEng{ID="TutorialDlg",Desc="Tutorial"};
case 12: return new DlgNoteEng{ID="StaffDlg",Desc="Staff List"};
case 13: return new DlgNoteEng{ID="LanguageDlg",Desc="Change Language"};
case 14: return new DlgNoteEng{ID="InviteDlg",Desc="Invite Friend"};
case 15: return new DlgNoteEng{ID="MessegnDlg",Desc="Message"};
case 16: return new DlgNoteEng{ID="TransferDlg",Desc="Data Transfer"};
case 17: return new DlgNoteEng{ID="AlbumDlg01",Desc="Photo of Houtong"};
case 18: return new DlgNoteEng{ID="AlbumDlg02",Desc="Photo of Zamami"};
case 19: return new DlgNoteEng{ID="AlbumDlg03",Desc="Photo of Tokashiki"};
case 20: return new DlgNoteEng{ID="AlbumDlg04",Desc="Photo of CPDC"};
case 21: return new DlgNoteEng{ID="AlbumDlg05",Desc="Photo of Orchid"};
case 22: return new DlgNoteEng{ID="AlbumDlg06",Desc="Photo of Taketomi"};
case 23: return new DlgNoteEng{ID="AlbumDlgS1",Desc="Photo of Penghu"};
case 24: return new DlgNoteEng{ID="AlbumDlgS2",Desc="Photo of Naha"};
case 25: return new DlgNoteEng{ID="MusicDlg",Desc="Music Player"};
case 26: return new DlgNoteEng{ID="StageitemDlg",Desc="Level Reward Probability"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}