package han.component;

import robocode.robotinterfaces.IBasicEvents2;
import robocode.robotinterfaces.IBasicEvents3;

public interface IBattleEvents extends IBasicEvents2, IBasicEvents3{
	void onRoundStarted();
}
