using UnityEngine;
namespace Remix{
	public class DlgMessageChs{
		public const int ID_COUNT = 27;

		public string ID {get; set;}
public string Dlg {get; set;}
public string Desc {get; set;}

		public static DlgMessageChs Get(string key){
			switch (key) {
			case "MesgText_I01": return new DlgMessageChs{ID="MesgText_I01",Dlg="InviteDlg",Desc="你的昵称"};
case "MesgText_I02": return new DlgMessageChs{ID="MesgText_I02",Dlg="InviteDlg",Desc="你的招待ID"};
case "MesgText_I03": return new DlgMessageChs{ID="MesgText_I03",Dlg="InviteDlg",Desc="输入招待ID"};
case "MesgText_I04": return new DlgMessageChs{ID="MesgText_I04",Dlg="InviteDlg",Desc="成功人数"};
case "MesgText_I05": return new DlgMessageChs{ID="MesgText_I05",Dlg="InviteDlg",Desc="招待成功后双方可获得:"};
case "MesgText_I06": return new DlgMessageChs{ID="MesgText_I06",Dlg="MessegnDlg",Desc="招待ID不正确"};
case "MesgText_I07": return new DlgMessageChs{ID="MesgText_I07",Dlg="MessegnDlg",Desc="该招待ID已达人数上限"};
case "MesgText_T01": return new DlgMessageChs{ID="MesgText_T01",Dlg="TransferDlg",Desc="现在的资料ID"};
case "MesgText_T02": return new DlgMessageChs{ID="MesgText_T02",Dlg="TransferDlg",Desc="输入要转移的资料ID"};
case "MesgText_T03": return new DlgMessageChs{ID="MesgText_T03",Dlg="TransferDlg",Desc="输入要转移的昵称"};
case "MesgText_T04": return new DlgMessageChs{ID="MesgText_T04",Dlg="TransferDlg",Desc="警告:资料转移后将会覆盖现有进度!!"};
case "MesgText_T05": return new DlgMessageChs{ID="MesgText_T05",Dlg="MessegnDlg",Desc="资料转移成功"};
case "MesgText_T06": return new DlgMessageChs{ID="MesgText_T06",Dlg="MessegnDlg",Desc="昵称与资料ID不正确"};
case "MesgText_G01": return new DlgMessageChs{ID="MesgText_G01",Dlg="BuyGachaDlg",Desc="解锁更多的猫咪游乐场"};
case "MesgText_L01": return new DlgMessageChs{ID="MesgText_L01",Dlg="LoginDlg",Desc="无法建立帐号 请检查网路连线"};
case "MesgText_D01": return new DlgMessageChs{ID="MesgText_D01",Dlg="DownloadDlg",Desc="未完整安装 请重新启动游戏"};
case "MesgText_D02": return new DlgMessageChs{ID="MesgText_D02",Dlg="DownloadDlg",Desc="检查档案完整性"};
case "MesgText_D03": return new DlgMessageChs{ID="MesgText_D03",Dlg="DownloadDlg",Desc="检查档案是否最新版本"};
case "MesgText_M01": return new DlgMessageChs{ID="MesgText_M01",Dlg="MessegnDlg",Desc="邀请成功，获得奖励在信箱中"};
case "MesgText_M02": return new DlgMessageChs{ID="MesgText_M02",Dlg="MessegnDlg",Desc="内置购买成功"};
case "MesgText_M03": return new DlgMessageChs{ID="MesgText_M03",Dlg="MessegnDlg",Desc="解锁新地图"};
case "MesgText_M04": return new DlgMessageChs{ID="MesgText_M04",Dlg="MessegnDlg",Desc="请检查网路连线"};
case "MesgText_GD1": return new DlgMessageChs{ID="MesgText_GD1",Dlg="GameDelayTool",Desc="请调整提示点到达时机与音乐节奏相符。"};
case "MesgText_M05": return new DlgMessageChs{ID="MesgText_M05",Dlg="MessegnDlg",Desc="字数必须小于20"};
case "MesgText_M06": return new DlgMessageChs{ID="MesgText_M06",Dlg="MessegnDlg",Desc="帐号被禁止"};
case "MesgText_M07": return new DlgMessageChs{ID="MesgText_M07",Dlg="MessegnDlg",Desc="程式错误"};
case "MesgText_M08": return new DlgMessageChs{ID="MesgText_M08",Dlg="MessegnDlg",Desc="此版本不支持更改语言功能。"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static DlgMessageChs Get(int key){
			switch (key) {
			case 0: return new DlgMessageChs{ID="MesgText_I01",Dlg="InviteDlg",Desc="你的昵称"};
case 1: return new DlgMessageChs{ID="MesgText_I02",Dlg="InviteDlg",Desc="你的招待ID"};
case 2: return new DlgMessageChs{ID="MesgText_I03",Dlg="InviteDlg",Desc="输入招待ID"};
case 3: return new DlgMessageChs{ID="MesgText_I04",Dlg="InviteDlg",Desc="成功人数"};
case 4: return new DlgMessageChs{ID="MesgText_I05",Dlg="InviteDlg",Desc="招待成功后双方可获得:"};
case 5: return new DlgMessageChs{ID="MesgText_I06",Dlg="MessegnDlg",Desc="招待ID不正确"};
case 6: return new DlgMessageChs{ID="MesgText_I07",Dlg="MessegnDlg",Desc="该招待ID已达人数上限"};
case 7: return new DlgMessageChs{ID="MesgText_T01",Dlg="TransferDlg",Desc="现在的资料ID"};
case 8: return new DlgMessageChs{ID="MesgText_T02",Dlg="TransferDlg",Desc="输入要转移的资料ID"};
case 9: return new DlgMessageChs{ID="MesgText_T03",Dlg="TransferDlg",Desc="输入要转移的昵称"};
case 10: return new DlgMessageChs{ID="MesgText_T04",Dlg="TransferDlg",Desc="警告:资料转移后将会覆盖现有进度!!"};
case 11: return new DlgMessageChs{ID="MesgText_T05",Dlg="MessegnDlg",Desc="资料转移成功"};
case 12: return new DlgMessageChs{ID="MesgText_T06",Dlg="MessegnDlg",Desc="昵称与资料ID不正确"};
case 13: return new DlgMessageChs{ID="MesgText_G01",Dlg="BuyGachaDlg",Desc="解锁更多的猫咪游乐场"};
case 14: return new DlgMessageChs{ID="MesgText_L01",Dlg="LoginDlg",Desc="无法建立帐号 请检查网路连线"};
case 15: return new DlgMessageChs{ID="MesgText_D01",Dlg="DownloadDlg",Desc="未完整安装 请重新启动游戏"};
case 16: return new DlgMessageChs{ID="MesgText_D02",Dlg="DownloadDlg",Desc="检查档案完整性"};
case 17: return new DlgMessageChs{ID="MesgText_D03",Dlg="DownloadDlg",Desc="检查档案是否最新版本"};
case 18: return new DlgMessageChs{ID="MesgText_M01",Dlg="MessegnDlg",Desc="邀请成功，获得奖励在信箱中"};
case 19: return new DlgMessageChs{ID="MesgText_M02",Dlg="MessegnDlg",Desc="内置购买成功"};
case 20: return new DlgMessageChs{ID="MesgText_M03",Dlg="MessegnDlg",Desc="解锁新地图"};
case 21: return new DlgMessageChs{ID="MesgText_M04",Dlg="MessegnDlg",Desc="请检查网路连线"};
case 22: return new DlgMessageChs{ID="MesgText_GD1",Dlg="GameDelayTool",Desc="请调整提示点到达时机与音乐节奏相符。"};
case 23: return new DlgMessageChs{ID="MesgText_M05",Dlg="MessegnDlg",Desc="字数必须小于20"};
case 24: return new DlgMessageChs{ID="MesgText_M06",Dlg="MessegnDlg",Desc="帐号被禁止"};
case 25: return new DlgMessageChs{ID="MesgText_M07",Dlg="MessegnDlg",Desc="程式错误"};
case 26: return new DlgMessageChs{ID="MesgText_M08",Dlg="MessegnDlg",Desc="此版本不支持更改语言功能。"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}