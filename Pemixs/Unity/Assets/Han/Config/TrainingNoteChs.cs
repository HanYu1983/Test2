using UnityEngine;
namespace Remix{
	public class TrainingNoteChs{
		public const int ID_COUNT = 43;

		public string ID {get; set;}
public string Page {get; set;}
public string Desc {get; set;}

		public static TrainingNoteChs Get(string key){
			switch (key) {
			
			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}

		public static TrainingNoteChs Get(int key){
			switch (key) {
			case 0: return new TrainingNoteChs{Desc="欢迎来到猫咪混音，现在开始教学关卡。"};
case 1: return new TrainingNoteChs{Desc="随着节奏按下对应的颜色猫掌，猫就会做出可爱的动作喔。"};
case 2: return new TrainingNoteChs{Desc="当提示点向左边到达判定框时，就是按下猫掌的时机。"};
case 3: return new TrainingNoteChs{Desc="再试一次，这一次要变得比较难喔。"};
case 4: return new TrainingNoteChs{Desc="当提示点向左边到达判定框时，就是按下猫掌的时机。"};
case 5: return new TrainingNoteChs{Desc="接下来要连打猫掌喔。"};
case 6: return new TrainingNoteChs{Desc="当提示点向左边到达判定框时，就是连打猫掌的时机。"};
case 7: return new TrainingNoteChs{Desc="再来是红色的上滑动滑动提示点。"};
case 8: return new TrainingNoteChs{Desc="当提示点向左边到达判定框时，就是向上滑动猫掌的时机。"};
case 9: return new TrainingNoteChs{Desc="再来是红色的下滑动滑动提示点。"};
case 10: return new TrainingNoteChs{Desc="当提示点向左边到达判定框时，就是向下滑动猫掌的时机。"};
case 11: return new TrainingNoteChs{Desc="接下来要持续的搓猫掌喔。"};
case 12: return new TrainingNoteChs{Desc="当提示点向左边到达判定框时，就是搓猫掌的时机。"};
case 13: return new TrainingNoteChs{Desc="太厉害了，你根本不用我教嘛。"};
case 14: return new TrainingNoteChs{Desc="现在开始互动模式教学，猫咪会有各种不同的心情喔。"};
case 15: return new TrainingNoteChs{Desc="点击猫咪的头像可以就跟猫咪互动。"};
case 16: return new TrainingNoteChs{Desc="在猫咪开心时，持续抚摸猫咪可以增加EXP。"};
case 17: return new TrainingNoteChs{Desc="摸太多猫咪会生气喔。"};
case 18: return new TrainingNoteChs{Desc="使用猫草可以让猫咪快速冷静下来。"};
case 19: return new TrainingNoteChs{Desc="猫咪有时会兴奋，想要玩玩具。"};
case 20: return new TrainingNoteChs{Desc="跟猫咪玩玩具可以增加EXP。"};
case 21: return new TrainingNoteChs{Desc="不同体型的猫咪喜欢的玩具不一样。"};
case 22: return new TrainingNoteChs{Desc="游玩关卡后，猫咪的HP减少时会肚子饿。"};
case 23: return new TrainingNoteChs{Desc="给猫咪吃饭可以回复HP。"};
case 24: return new TrainingNoteChs{Desc="游玩关卡太多时，猫咪会累到睡着。"};
case 25: return new TrainingNoteChs{Desc="猫咪睡着时可以把猫咪摸醒。"};
case 26: return new TrainingNoteChs{Desc="猫咪被吵醒虽然会生气，但可以继续游玩关卡。"};
case 27: return new TrainingNoteChs{Desc="记得每天都来关心一下你的猫咪喔。"};
case 28: return new TrainingNoteChs{Desc="现在开始探索模式教学，每个地图都可以遇见不同的猫咪喔。"};
case 29: return new TrainingNoteChs{Desc="点击探索模式按钮进入探索地图。"};
case 30: return new TrainingNoteChs{Desc="请在猫咪游乐场使用相机。"};
case 31: return new TrainingNoteChs{Desc="使用脚架可加速等待时间。"};
case 32: return new TrainingNoteChs{Desc="恭喜你获得新照片，获得照片时也会同时获得猫银票。"};
case 33: return new TrainingNoteChs{Desc="每个不同的地图都有照片可以收集喔。"};
case 34: return new TrainingNoteChs{Desc="请在猫咪游乐场使用补猫道具。"};
case 35: return new TrainingNoteChs{Desc="使用猫草可加速等待时间。"};
case 36: return new TrainingNoteChs{Desc="恭喜你获得新猫咪，不同的补猫道具可以获得不同体型的猫咪。"};
case 37: return new TrainingNoteChs{Desc="来试试更换猫咪吧。"};
case 38: return new TrainingNoteChs{Desc="点击猫咪的头像进入户动模式。"};
case 39: return new TrainingNoteChs{Desc="点击下一页可以更换使用猫咪。"};
case 40: return new TrainingNoteChs{Desc="点击选择猫咪按钮就可以使用新猫咪来游玩关卡喔。"};
case 41: return new TrainingNoteChs{Desc="猫钞票不足时记得来这里补给喔。"};
case 42: return new TrainingNoteChs{Desc="恭喜你完成所有教学，来领取礼物吧。"};

			default:
				throw new UnityException ("沒有這個定義:"+key);
			}
		}
	}
}