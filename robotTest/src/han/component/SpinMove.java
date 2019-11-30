package han.component;

import java.io.Serializable;

import robocode.AdvancedRobot;
import robocode.BulletHitBulletEvent;
import robocode.BulletHitEvent;
import robocode.BulletMissedEvent;
import robocode.DeathEvent;
import robocode.HitByBulletEvent;
import robocode.HitRobotEvent;
import robocode.HitWallEvent;
import robocode.RobotDeathEvent;
import robocode.ScannedRobotEvent;
import robocode.StatusEvent;
import robocode.WinEvent;
import robocode.robotinterfaces.IBasicEvents;

public class SpinMove implements IBasicEvents, ITick, Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = -6204398725346772702L;
	private final AdvancedRobot robot;

	public SpinMove(AdvancedRobot robot) {
		this.robot = robot;
	}
	@Override
	public void onTick() {
		robot.setTurnRight(10000);
		//robot.setMaxVelocity(5);
		robot.setAhead(10000);
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
	public void onHitRobot(HitRobotEvent e) {
		if (e.isMyFault()) {
			robot.turnRight(10);
		}
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

}
