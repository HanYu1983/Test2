using System;

namespace Remix
{
	public interface IGamePlayModeControl
	{
		void InitComponent();
		int CurrentLevel{ get;}
		bool IsFeverMode{ get; }
		bool IsGameEnd{ get; }
		void InitMode();
		void Step(float audioTime, float audioOffset);
		void OnGameButtonClick(string command);
		void OnGameButtonSlide(string command);
		GamePlayView GamePlayView{ get; }
		GamePlayModel GamePlayModel{ get; } 
	}
}

