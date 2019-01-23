using UnityEngine;
namespace Remix{
	public class DlgNoteChs{
		public const int ID_COUNT = 27;

		public string ID {get; set;}
public string Desc {get; set;}

		public static DlgNoteChs Get(string key){
			switch (key) {
			case "IAPDlg": return new DlgNoteChs{ID="IAPDlg",Desc="取得猫钞票与金币"};
case "LoadDlg": return new DlgNoteChs{ID="LoadDlg",Desc="读取中"};
case "NewCatDlg": return new DlgNoteChs{ID="NewCatDlg",Desc="获得猫咪"};
case "NewSPhotoDlg": return new DlgNoteChs{ID="NewSPhotoDlg",Desc="获得照片"};
case "NewBPhotoDlg": return new DlgNoteChs{ID="NewBPhotoDlg",Desc="获得环景照片"};
case "StorageDlg": return new DlgNoteChs{ID="StorageDlg",Desc="道具仓库"};
case "BuyGachaDlg": return new DlgNoteChs{ID="BuyGachaDlg",Desc="解锁猫咪游乐场"};
case "MailGiftDlg": return new DlgNoteChs{ID="MailGiftDlg",Desc="每日礼物"};
case "NewItemDlg": return new DlgNoteChs{ID="NewItemDlg",Desc="获得道具"};
case "LoginDlg": return new DlgNoteChs{ID="LoginDlg",Desc="输入昵称"};
case "DownloadDlg": return new DlgNoteChs{ID="DownloadDlg",Desc="资料更新中..."};
case "TutorialDlg": return new DlgNoteChs{ID="TutorialDlg",Desc="游戏教学"};
case "StaffDlg": return new DlgNoteChs{ID="StaffDlg",Desc="制作人员名单"};
case "LanguageDlg": return new DlgNoteChs{ID="LanguageDlg",Desc="更换语言"};
case "InviteDlg": return new DlgNoteChs{ID="InviteDlg",Desc="招待好友"};
case "MessegnDlg": return new DlgNoteChs{ID="MessegnDlg",Desc="讯息"};
case "TransferDlg": return new DlgNoteChs{ID="TransferDlg",Desc="资料转移"};
case "AlbumDlg01": return new DlgNoteChs{ID="AlbumDlg01",Desc="猴硐的照片"};
case "AlbumDlg02": return new DlgNoteChs{ID="AlbumDlg02",Desc="座间味的照片"};
case "AlbumDlg03": return new DlgNoteChs{ID="AlbumDlg03",Desc="渡嘉敷的照片"};
case "AlbumDlg04": return new DlgNoteChs{ID="AlbumDlg04",Desc="台碱宿舍的照片"};
case "AlbumDlg05": return new DlgNoteChs{ID="AlbumDlg05",Desc="兰屿的照片"};
case "AlbumDlg06": return new DlgNoteChs{ID="AlbumDlg06",Desc="竹富的照片"};
case "AlbumDlgS1": return new DlgNoteChs{ID="AlbumDlgS1",Desc="澎湖的照片"};
case "AlbumDlgS2": return new DlgNoteChs{ID="AlbumDlgS2",Desc="那霸的照片"};
case "MusicDlg": return new DlgNoteChs{ID="MusicDlg",Desc="音乐播放"};
case "StageitemDlg": return new DlgNoteChs{ID="StageitemDlg",Desc="过关奖励机率表"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static DlgNoteChs Get(int key){
			switch (key) {
			case 0: return new DlgNoteChs{ID="IAPDlg",Desc="取得猫钞票与金币"};
case 1: return new DlgNoteChs{ID="LoadDlg",Desc="读取中"};
case 2: return new DlgNoteChs{ID="NewCatDlg",Desc="获得猫咪"};
case 3: return new DlgNoteChs{ID="NewSPhotoDlg",Desc="获得照片"};
case 4: return new DlgNoteChs{ID="NewBPhotoDlg",Desc="获得环景照片"};
case 5: return new DlgNoteChs{ID="StorageDlg",Desc="道具仓库"};
case 6: return new DlgNoteChs{ID="BuyGachaDlg",Desc="解锁猫咪游乐场"};
case 7: return new DlgNoteChs{ID="MailGiftDlg",Desc="每日礼物"};
case 8: return new DlgNoteChs{ID="NewItemDlg",Desc="获得道具"};
case 9: return new DlgNoteChs{ID="LoginDlg",Desc="输入昵称"};
case 10: return new DlgNoteChs{ID="DownloadDlg",Desc="资料更新中..."};
case 11: return new DlgNoteChs{ID="TutorialDlg",Desc="游戏教学"};
case 12: return new DlgNoteChs{ID="StaffDlg",Desc="制作人员名单"};
case 13: return new DlgNoteChs{ID="LanguageDlg",Desc="更换语言"};
case 14: return new DlgNoteChs{ID="InviteDlg",Desc="招待好友"};
case 15: return new DlgNoteChs{ID="MessegnDlg",Desc="讯息"};
case 16: return new DlgNoteChs{ID="TransferDlg",Desc="资料转移"};
case 17: return new DlgNoteChs{ID="AlbumDlg01",Desc="猴硐的照片"};
case 18: return new DlgNoteChs{ID="AlbumDlg02",Desc="座间味的照片"};
case 19: return new DlgNoteChs{ID="AlbumDlg03",Desc="渡嘉敷的照片"};
case 20: return new DlgNoteChs{ID="AlbumDlg04",Desc="台碱宿舍的照片"};
case 21: return new DlgNoteChs{ID="AlbumDlg05",Desc="兰屿的照片"};
case 22: return new DlgNoteChs{ID="AlbumDlg06",Desc="竹富的照片"};
case 23: return new DlgNoteChs{ID="AlbumDlgS1",Desc="澎湖的照片"};
case 24: return new DlgNoteChs{ID="AlbumDlgS2",Desc="那霸的照片"};
case 25: return new DlgNoteChs{ID="MusicDlg",Desc="音乐播放"};
case 26: return new DlgNoteChs{ID="StageitemDlg",Desc="过关奖励机率表"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}