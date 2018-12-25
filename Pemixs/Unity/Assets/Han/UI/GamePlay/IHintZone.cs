using System;
using System.Collections;

namespace Remix
{
	public interface IHintZone
	{
		void InitHintZone();
		float TimePerBeat{ get; set; }
		int BeatCntPerTurn{ get; set; }
		int TurnCntPerLevel{ get; set; }
		float StartTime{ get; set; }
		float HintWidth{ get; set; }
		void ArrangePos(float offset);
		void SyncTimer(float timer);
		void InitHintSprite (int[][] idxAry, int[][] mashAry);
		void HintPlayGood(int hintIdx, int clickIdx, bool isPerfect, bool isFever, Game.ClickType clickType);
		void HintPlayMiss(int hintIdx, int clickIdx, bool isFever);
		IEnumerator ShiningHint();
	}
}

