using UnityEngine;
namespace Remix{
	public class DlgMessageCht{
		public const int ID_COUNT = 27;

		public string ID {get; set;}
public string Dlg {get; set;}
public string Desc {get; set;}

		public static DlgMessageCht Get(string key){
			switch (key) {
			case "MesgText_I01": return new DlgMessageCht{ID="MesgText_I01",Dlg="InviteDlg",Desc="你的暱稱"};
case "MesgText_I02": return new DlgMessageCht{ID="MesgText_I02",Dlg="InviteDlg",Desc="你的招待ID"};
case "MesgText_I03": return new DlgMessageCht{ID="MesgText_I03",Dlg="InviteDlg",Desc="輸入招待ID"};
case "MesgText_I04": return new DlgMessageCht{ID="MesgText_I04",Dlg="InviteDlg",Desc="成功人數"};
case "MesgText_I05": return new DlgMessageCht{ID="MesgText_I05",Dlg="InviteDlg",Desc="招待成功後雙方可獲得:"};
case "MesgText_I06": return new DlgMessageCht{ID="MesgText_I06",Dlg="MessegnDlg",Desc="招待ID不正確"};
case "MesgText_I07": return new DlgMessageCht{ID="MesgText_I07",Dlg="MessegnDlg",Desc="該招待ID已達人數上限"};
case "MesgText_T01": return new DlgMessageCht{ID="MesgText_T01",Dlg="TransferDlg",Desc="現在的資料ID"};
case "MesgText_T02": return new DlgMessageCht{ID="MesgText_T02",Dlg="TransferDlg",Desc="輸入要轉移的資料ID"};
case "MesgText_T03": return new DlgMessageCht{ID="MesgText_T03",Dlg="TransferDlg",Desc="輸入要轉移的暱稱"};
case "MesgText_T04": return new DlgMessageCht{ID="MesgText_T04",Dlg="TransferDlg",Desc="警告:資料轉移後將會覆蓋現有進度!!"};
case "MesgText_T05": return new DlgMessageCht{ID="MesgText_T05",Dlg="MessegnDlg",Desc="資料轉移成功"};
case "MesgText_T06": return new DlgMessageCht{ID="MesgText_T06",Dlg="MessegnDlg",Desc="暱稱與資料ID不正確"};
case "MesgText_G01": return new DlgMessageCht{ID="MesgText_G01",Dlg="BuyGachaDlg",Desc="解鎖更多的貓咪遊樂場"};
case "MesgText_L01": return new DlgMessageCht{ID="MesgText_L01",Dlg="LoginDlg",Desc="無法建立帳號   請檢查網路連線"};
case "MesgText_D01": return new DlgMessageCht{ID="MesgText_D01",Dlg="DownloadDlg",Desc="未完整安裝   請重新啟動遊戲"};
case "MesgText_D02": return new DlgMessageCht{ID="MesgText_D02",Dlg="DownloadDlg",Desc="檢查檔案完整性"};
case "MesgText_D03": return new DlgMessageCht{ID="MesgText_D03",Dlg="DownloadDlg",Desc="檢查檔案是否最新版本"};
case "MesgText_M01": return new DlgMessageCht{ID="MesgText_M01",Dlg="MessegnDlg",Desc="邀請成功，獲得獎勵在信箱中"};
case "MesgText_M02": return new DlgMessageCht{ID="MesgText_M02",Dlg="MessegnDlg",Desc="內置購買成功"};
case "MesgText_M03": return new DlgMessageCht{ID="MesgText_M03",Dlg="MessegnDlg",Desc="解鎖新地圖"};
case "MesgText_M04": return new DlgMessageCht{ID="MesgText_M04",Dlg="MessegnDlg",Desc="請檢查網路連線"};
case "MesgText_GD1": return new DlgMessageCht{ID="MesgText_GD1",Dlg="GameDelayTool",Desc="請調整提示點到達時機與音樂節奏相符。"};
case "MesgText_M05": return new DlgMessageCht{ID="MesgText_M05",Dlg="MessegnDlg",Desc="字數必須小於20"};
case "MesgText_M06": return new DlgMessageCht{ID="MesgText_M06",Dlg="MessegnDlg",Desc="帳號被禁止"};
case "MesgText_M07": return new DlgMessageCht{ID="MesgText_M07",Dlg="MessegnDlg",Desc="程式錯誤"};
case "MesgText_M08": return new DlgMessageCht{ID="MesgText_M08",Dlg="MessegnDlg",Desc="此版本不支持更改語言功能。"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static DlgMessageCht Get(int key){
			switch (key) {
			case 0: return new DlgMessageCht{ID="MesgText_I01",Dlg="InviteDlg",Desc="你的暱稱"};
case 1: return new DlgMessageCht{ID="MesgText_I02",Dlg="InviteDlg",Desc="你的招待ID"};
case 2: return new DlgMessageCht{ID="MesgText_I03",Dlg="InviteDlg",Desc="輸入招待ID"};
case 3: return new DlgMessageCht{ID="MesgText_I04",Dlg="InviteDlg",Desc="成功人數"};
case 4: return new DlgMessageCht{ID="MesgText_I05",Dlg="InviteDlg",Desc="招待成功後雙方可獲得:"};
case 5: return new DlgMessageCht{ID="MesgText_I06",Dlg="MessegnDlg",Desc="招待ID不正確"};
case 6: return new DlgMessageCht{ID="MesgText_I07",Dlg="MessegnDlg",Desc="該招待ID已達人數上限"};
case 7: return new DlgMessageCht{ID="MesgText_T01",Dlg="TransferDlg",Desc="現在的資料ID"};
case 8: return new DlgMessageCht{ID="MesgText_T02",Dlg="TransferDlg",Desc="輸入要轉移的資料ID"};
case 9: return new DlgMessageCht{ID="MesgText_T03",Dlg="TransferDlg",Desc="輸入要轉移的暱稱"};
case 10: return new DlgMessageCht{ID="MesgText_T04",Dlg="TransferDlg",Desc="警告:資料轉移後將會覆蓋現有進度!!"};
case 11: return new DlgMessageCht{ID="MesgText_T05",Dlg="MessegnDlg",Desc="資料轉移成功"};
case 12: return new DlgMessageCht{ID="MesgText_T06",Dlg="MessegnDlg",Desc="暱稱與資料ID不正確"};
case 13: return new DlgMessageCht{ID="MesgText_G01",Dlg="BuyGachaDlg",Desc="解鎖更多的貓咪遊樂場"};
case 14: return new DlgMessageCht{ID="MesgText_L01",Dlg="LoginDlg",Desc="無法建立帳號   請檢查網路連線"};
case 15: return new DlgMessageCht{ID="MesgText_D01",Dlg="DownloadDlg",Desc="未完整安裝   請重新啟動遊戲"};
case 16: return new DlgMessageCht{ID="MesgText_D02",Dlg="DownloadDlg",Desc="檢查檔案完整性"};
case 17: return new DlgMessageCht{ID="MesgText_D03",Dlg="DownloadDlg",Desc="檢查檔案是否最新版本"};
case 18: return new DlgMessageCht{ID="MesgText_M01",Dlg="MessegnDlg",Desc="邀請成功，獲得獎勵在信箱中"};
case 19: return new DlgMessageCht{ID="MesgText_M02",Dlg="MessegnDlg",Desc="內置購買成功"};
case 20: return new DlgMessageCht{ID="MesgText_M03",Dlg="MessegnDlg",Desc="解鎖新地圖"};
case 21: return new DlgMessageCht{ID="MesgText_M04",Dlg="MessegnDlg",Desc="請檢查網路連線"};
case 22: return new DlgMessageCht{ID="MesgText_GD1",Dlg="GameDelayTool",Desc="請調整提示點到達時機與音樂節奏相符。"};
case 23: return new DlgMessageCht{ID="MesgText_M05",Dlg="MessegnDlg",Desc="字數必須小於20"};
case 24: return new DlgMessageCht{ID="MesgText_M06",Dlg="MessegnDlg",Desc="帳號被禁止"};
case 25: return new DlgMessageCht{ID="MesgText_M07",Dlg="MessegnDlg",Desc="程式錯誤"};
case 26: return new DlgMessageCht{ID="MesgText_M08",Dlg="MessegnDlg",Desc="此版本不支持更改語言功能。"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}