package han.demo.team1;

import java.io.Serializable;

import org.jbox2d.common.MathUtils;
import org.jbox2d.common.Vec2;

import han.component.ComponentRobot;
import robocode.MessageEvent;
import robocode.robotinterfaces.ITeamEvents;

public class Attacker extends ComponentRobot {
	{
		coms.addComponent(new Control());
	}

	private class Control implements ITeamEvents, Serializable {
		/**
		 * 
		 */
		private static final long serialVersionUID = 4855659638362811961L;
		private final ComponentRobot robot = Attacker.this;

		@Override
		public void onMessageReceived(MessageEvent arg0) {
			if (arg0.getMessage() instanceof Message == false) {
				return;
			}
			Message msg = (Message) arg0.getMessage();
			if (msg.order == Message.Order.Pending) {
				return;
			}
			if (msg.robot != null && msg.robot == Attacker.this.getName()) {
				return;
			}
			if (msg.order == Message.Order.AttackIt) {
				Vec2 opponentPosition = msg.vec1;
				Vec2 dir = opponentPosition.sub(robot.getPosition());
				// 因為機器人的的零度是正上方所以atan2的xy要倒反
				double heading = MathUtils.atan2(dir.x, dir.y);
				double bearing = robocode.util.Utils.normalRelativeAngle(heading - robot.getGunHeadingRadians());
				robot.setTurnGunRightRadians(bearing);
			}
		}
	}
}
