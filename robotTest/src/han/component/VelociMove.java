package han.component;

import robocode.BattleEndedEvent;
import robocode.BulletHitBulletEvent;
import robocode.BulletHitEvent;
import robocode.BulletMissedEvent;
import robocode.DeathEvent;
import robocode.HitByBulletEvent;
import robocode.HitRobotEvent;
import robocode.HitWallEvent;
import robocode.RateControlRobot;
import robocode.RobotDeathEvent;
import robocode.RoundEndedEvent;
import robocode.ScannedRobotEvent;
import robocode.StatusEvent;
import robocode.WinEvent;
import robocode.robotinterfaces.IBasicEvents;

public class VelociMove implements IBasicEvents, ITick, IBattleEvents {
	private final RateControlRobot robot;

	public VelociMove(RateControlRobot robot) {
		this.robot = robot;
	}

	int turnCounter;

	@Override
	public void onRoundStarted() {
		turnCounter = 0;
		robot.setGunRotationRate(15);
	}

	@Override
	public void onTick() {
		if (turnCounter % 64 == 0) {
			// Straighten out, if we were hit by a bullet and are turning
			robot.setTurnRate(0);
			// Go forward with a velocity of 4
			robot.setVelocityRate(4);
		}
		if (turnCounter % 64 == 32) {
			// Go backwards, faster
			robot.setVelocityRate(-6);
		}
		turnCounter++;
		robot.execute();
	}

	@Override
	public void onHitByBullet(HitByBulletEvent e) {
		robot.setTurnRate(5);
	}

	@Override
	public void onHitWall(HitWallEvent e) {
		robot.setVelocityRate(-1 * robot.getVelocityRate());
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
	public void onHitRobot(HitRobotEvent arg0) {
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

	@Override
	public void onBattleEnded(BattleEndedEvent arg0) {
		// TODO Auto-generated method stub

	}

	@Override
	public void onRoundEnded(RoundEndedEvent arg0) {
		// TODO Auto-generated method stub

	}

	@Override
	public void onScannedRobot(ScannedRobotEvent arg0) {
		// TODO Auto-generated method stub
		
	}

}
