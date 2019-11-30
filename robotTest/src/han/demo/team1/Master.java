package han.demo.team1;

import java.io.Serializable;
import java.util.HashMap;
import java.util.Map;

import han.component.ComponentRobot;
import han.component.IBattleEvents;
import robocode.BattleEndedEvent;
import robocode.BulletHitBulletEvent;
import robocode.BulletHitEvent;
import robocode.BulletMissedEvent;
import robocode.DeathEvent;
import robocode.HitByBulletEvent;
import robocode.HitRobotEvent;
import robocode.HitWallEvent;
import robocode.MessageEvent;
import robocode.RobotDeathEvent;
import robocode.RoundEndedEvent;
import robocode.ScannedRobotEvent;
import robocode.StatusEvent;
import robocode.WinEvent;
import robocode.robotinterfaces.ITeamEvents;

public class Master extends ComponentRobot {

	{
		coms.addComponent(new Control());
	}

	private class RobotInfo {
		Message.State state;
	}

	private class Control implements ITeamEvents, IBattleEvents, Serializable {
		/**
		 * 
		 */
		private static final long serialVersionUID = 482498257855598589L;

		private final ComponentRobot robot = Master.this;

		private Map<String, RobotInfo> teammateInfo = new HashMap<>();

		public void initInfo(String[] teammates) {
			for (String robotName : teammates) {
				teammateInfo.put(robotName, new RobotInfo());
			}
		}

		public RobotInfo getTeammateFirst(Message.State state) {
			for (RobotInfo info : teammateInfo.values()) {
				if (info.state == state) {
					return info;
				}
			}
			return null;
		}

		@Override
		public void onRoundStarted() {
			this.initInfo(robot.getTeammates());
		}

		@Override
		public void onMessageReceived(MessageEvent arg0) {
			if (arg0.getMessage() instanceof Message == false) {
				return;
			}
			Message msg = (Message) arg0.getMessage();
			if (msg.report == Message.Report.Pending) {
				return;
			}
			switch (msg.report) {
			case OpponentPosition: {
				Message order = new Message();
				order.order = Message.Order.AttackIt;
				order.vec1 = msg.vec1;
				robot.simpleSend(order);
				break;
			}
			default: {
				break;
			}
			}
		}

		@Override
		public void onBattleEnded(BattleEndedEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onBulletHit(BulletHitEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onBulletHitBullet(BulletHitBulletEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onBulletMissed(BulletMissedEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onDeath(DeathEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onHitByBullet(HitByBulletEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onHitRobot(HitRobotEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onHitWall(HitWallEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onRobotDeath(RobotDeathEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onScannedRobot(ScannedRobotEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onStatus(StatusEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onWin(WinEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onRoundEnded(RoundEndedEvent arg0) {
			// TODO Auto-generated method stub

		}

	}
}
