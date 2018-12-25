using UnityEngine;
namespace Remix{
	public class DlgMessageEng{
		public const int ID_COUNT = 27;

		public string ID {get; set;}
public string Dlg {get; set;}
public string Desc {get; set;}

		public static DlgMessageEng Get(string key){
			switch (key) {
			case "MesgText_I01": return new DlgMessageEng{ID="MesgText_I01",Dlg="InviteDlg",Desc="Your Name"};
case "MesgText_I02": return new DlgMessageEng{ID="MesgText_I02",Dlg="InviteDlg",Desc="Your Invite ID"};
case "MesgText_I03": return new DlgMessageEng{ID="MesgText_I03",Dlg="InviteDlg",Desc="Enter Invite ID"};
case "MesgText_I04": return new DlgMessageEng{ID="MesgText_I04",Dlg="InviteDlg",Desc="Success"};
case "MesgText_I05": return new DlgMessageEng{ID="MesgText_I05",Dlg="InviteDlg",Desc="Success reward:"};
case "MesgText_I06": return new DlgMessageEng{ID="MesgText_I06",Dlg="MessegnDlg",Desc="Invite ID is incorrect"};
case "MesgText_I07": return new DlgMessageEng{ID="MesgText_I07",Dlg="MessegnDlg",Desc="Invite ID has reached the maximum number"};
case "MesgText_T01": return new DlgMessageEng{ID="MesgText_T01",Dlg="TransferDlg",Desc="Current Data ID"};
case "MesgText_T02": return new DlgMessageEng{ID="MesgText_T02",Dlg="TransferDlg",Desc="Enter transfer Data ID"};
case "MesgText_T03": return new DlgMessageEng{ID="MesgText_T03",Dlg="TransferDlg",Desc="Enter transfer name"};
case "MesgText_T04": return new DlgMessageEng{ID="MesgText_T04",Dlg="TransferDlg",Desc="Warning: Transfer data will replace current data!!"};
case "MesgText_T05": return new DlgMessageEng{ID="MesgText_T05",Dlg="MessegnDlg",Desc="Transfer data success"};
case "MesgText_T06": return new DlgMessageEng{ID="MesgText_T06",Dlg="MessegnDlg",Desc="Name or data ID is incorrect"};
case "MesgText_G01": return new DlgMessageEng{ID="MesgText_G01",Dlg="BuyGachaDlg",Desc="Unlock more cat trees"};
case "MesgText_L01": return new DlgMessageEng{ID="MesgText_L01",Dlg="LoginDlg",Desc="Unable to create account. Please check internet connection."};
case "MesgText_D01": return new DlgMessageEng{ID="MesgText_D01",Dlg="DownloadDlg",Desc="Failed to install. Please restart the APP."};
case "MesgText_D02": return new DlgMessageEng{ID="MesgText_D02",Dlg="DownloadDlg",Desc="File completion check"};
case "MesgText_D03": return new DlgMessageEng{ID="MesgText_D03",Dlg="DownloadDlg",Desc="Checking for updates"};
case "MesgText_M01": return new DlgMessageEng{ID="MesgText_M01",Dlg="MessegnDlg",Desc="Successful invite. Reward is in the Mailbox"};
case "MesgText_M02": return new DlgMessageEng{ID="MesgText_M02",Dlg="MessegnDlg",Desc="Successful purchase"};
case "MesgText_M03": return new DlgMessageEng{ID="MesgText_M03",Dlg="MessegnDlg",Desc="Unlock new map"};
case "MesgText_M04": return new DlgMessageEng{ID="MesgText_M04",Dlg="MessegnDlg",Desc="Please check internet connection."};
case "MesgText_GD1": return new DlgMessageEng{ID="MesgText_GD1",Dlg="GameDelayTool",Desc="Please adjust the timing of the input spot"};
case "MesgText_M05": return new DlgMessageEng{ID="MesgText_M05",Dlg="MessegnDlg",Desc="Maximum length 20 words"};
case "MesgText_M06": return new DlgMessageEng{ID="MesgText_M06",Dlg="MessegnDlg",Desc="Account is banned"};
case "MesgText_M07": return new DlgMessageEng{ID="MesgText_M07",Dlg="MessegnDlg",Desc="Application error"};
case "MesgText_M08": return new DlgMessageEng{ID="MesgText_M08",Dlg="MessegnDlg",Desc="此版本不支持更改語言功能。"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static DlgMessageEng Get(int key){
			switch (key) {
			case 0: return new DlgMessageEng{ID="MesgText_I01",Dlg="InviteDlg",Desc="Your Name"};
case 1: return new DlgMessageEng{ID="MesgText_I02",Dlg="InviteDlg",Desc="Your Invite ID"};
case 2: return new DlgMessageEng{ID="MesgText_I03",Dlg="InviteDlg",Desc="Enter Invite ID"};
case 3: return new DlgMessageEng{ID="MesgText_I04",Dlg="InviteDlg",Desc="Success"};
case 4: return new DlgMessageEng{ID="MesgText_I05",Dlg="InviteDlg",Desc="Success reward:"};
case 5: return new DlgMessageEng{ID="MesgText_I06",Dlg="MessegnDlg",Desc="Invite ID is incorrect"};
case 6: return new DlgMessageEng{ID="MesgText_I07",Dlg="MessegnDlg",Desc="Invite ID has reached the maximum number"};
case 7: return new DlgMessageEng{ID="MesgText_T01",Dlg="TransferDlg",Desc="Current Data ID"};
case 8: return new DlgMessageEng{ID="MesgText_T02",Dlg="TransferDlg",Desc="Enter transfer Data ID"};
case 9: return new DlgMessageEng{ID="MesgText_T03",Dlg="TransferDlg",Desc="Enter transfer name"};
case 10: return new DlgMessageEng{ID="MesgText_T04",Dlg="TransferDlg",Desc="Warning: Transfer data will replace current data!!"};
case 11: return new DlgMessageEng{ID="MesgText_T05",Dlg="MessegnDlg",Desc="Transfer data success"};
case 12: return new DlgMessageEng{ID="MesgText_T06",Dlg="MessegnDlg",Desc="Name or data ID is incorrect"};
case 13: return new DlgMessageEng{ID="MesgText_G01",Dlg="BuyGachaDlg",Desc="Unlock more cat trees"};
case 14: return new DlgMessageEng{ID="MesgText_L01",Dlg="LoginDlg",Desc="Unable to create account. Please check internet connection."};
case 15: return new DlgMessageEng{ID="MesgText_D01",Dlg="DownloadDlg",Desc="Failed to install. Please restart the APP."};
case 16: return new DlgMessageEng{ID="MesgText_D02",Dlg="DownloadDlg",Desc="File completion check"};
case 17: return new DlgMessageEng{ID="MesgText_D03",Dlg="DownloadDlg",Desc="Checking for updates"};
case 18: return new DlgMessageEng{ID="MesgText_M01",Dlg="MessegnDlg",Desc="Successful invite. Reward is in the Mailbox"};
case 19: return new DlgMessageEng{ID="MesgText_M02",Dlg="MessegnDlg",Desc="Successful purchase"};
case 20: return new DlgMessageEng{ID="MesgText_M03",Dlg="MessegnDlg",Desc="Unlock new map"};
case 21: return new DlgMessageEng{ID="MesgText_M04",Dlg="MessegnDlg",Desc="Please check internet connection."};
case 22: return new DlgMessageEng{ID="MesgText_GD1",Dlg="GameDelayTool",Desc="Please adjust the timing of the input spot"};
case 23: return new DlgMessageEng{ID="MesgText_M05",Dlg="MessegnDlg",Desc="Maximum length 20 words"};
case 24: return new DlgMessageEng{ID="MesgText_M06",Dlg="MessegnDlg",Desc="Account is banned"};
case 25: return new DlgMessageEng{ID="MesgText_M07",Dlg="MessegnDlg",Desc="Application error"};
case 26: return new DlgMessageEng{ID="MesgText_M08",Dlg="MessegnDlg",Desc="此版本不支持更改語言功能。"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}