package han.demo.team1;

import org.jbox2d.common.Vec2;

import han.component.ComponentRobot;
import han.component.ITick;
import han.component.JustScan;
import han.component.MemoryTargetPosition;
import han.component.RandomForwardMove;
import robocode.Rules;

public class Scouter extends ComponentRobot {
	private final MemoryTargetPosition memory = new MemoryTargetPosition(this);
	private final JustScan justScan = new JustScan(this);
	private final RandomForwardMove randomForwardMove = new RandomForwardMove(this);

	{
		coms.addComponent(memory);
		coms.addComponent(justScan);
		coms.addComponent(randomForwardMove);
		coms.addComponent(new Control());
	}

	private class Control implements ITick {
		private ComponentRobot robot = Scouter.this;

		@Override
		public void onTick() {
			double power = 3;
			double speed = Rules.getBulletSpeed(power);
			for (String robotName : memory.getRobotNames()) {
				Vec2 p = memory.computeBestPoint(robotName, speed);
				Message msg = new Message();
				msg.report = Message.Report.OpponentPosition;
				msg.robot = robotName;
				msg.vec1 = p;
				robot.simpleSend(msg);
			}
		}
	}
}
