package han;

import java.awt.Graphics2D;

import han.component.ITick;
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

public class CrazyMoveSystem implements IBasicEvents, ITick, IPaintEvents {
	private final AdvancedRobot robot;

	public CrazyMoveSystem(AdvancedRobot robot) {
		this.robot = robot;
	}

	boolean movingForward;

	/**
	 * run: Crazy's main run function
	 */
	private void update() {
		robot.setAhead(40000);
		movingForward = true;
		robot.setTurnRight(90);
		//robot.waitFor(new TurnCompleteCondition(robot));
		robot.setTurnLeft(180);
		//robot.waitFor(new TurnCompleteCondition(robot));
		robot.setTurnRight(180);
		//robot.waitFor(new TurnCompleteCondition(robot));
	}

	/**
	 * onHitWall: Handle collision with wall.
	 */
	public void onHitWall(HitWallEvent e) {
		// Bounce off!
		reverseDirection();
	}

	/**
	 * reverseDirection: Switch from ahead to back & vice versa
	 */
	public void reverseDirection() {
		if (movingForward) {
			robot.setBack(40000);
			movingForward = false;
		} else {
			robot.setAhead(40000);
			movingForward = true;
		}
	}

	/**
	 * onScannedRobot: Fire!
	 */
	public void onScannedRobot(ScannedRobotEvent e) {

	}

	/**
	 * onHitRobot: Back up!
	 */
	public void onHitRobot(HitRobotEvent e) {
		// If we're moving the other robot, reverse!
		if (e.isMyFault()) {
			reverseDirection();
		}
	}

	@Override
	public void onPaint(Graphics2D arg0) {
		// TODO Auto-generated method stub

	}

	@Override
	public void onTick() {
		update();
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
