package han.component;

import static robocode.util.Utils.normalRelativeAngleDegrees;

import java.awt.Graphics2D;
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
import robocode.robotinterfaces.IPaintEvents;

public class RadarMovementV2 implements IBasicEvents, ITick, IPaintEvents, Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = 1116005165952977049L;
	private final AdvancedRobot robot;

	public RadarMovementV2(AdvancedRobot robot) {
		this.robot = robot;
	}

	@Override
	public void onScannedRobot(ScannedRobotEvent e) {
		double absoluteBearing = robot.getHeading() + e.getBearing();
		double bearingFromGun = normalRelativeAngleDegrees(absoluteBearing - robot.getRadarHeadingRadians());
		robot.setTurnRadarRightRadians(bearingFromGun);
		if (bearingFromGun == 0) {
			robot.scan();
		}
	}

	@Override
	public void onPaint(Graphics2D g) {

	}

	@Override
	public void onTick() {

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
	public void onStatus(StatusEvent arg0) {
		// TODO Auto-generated method stub

	}

	@Override
	public void onWin(WinEvent arg0) {
		// TODO Auto-generated method stub

	}

}
