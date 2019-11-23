package han.component;

import java.util.Random;

import robocode.AdvancedRobot;
import robocode.BattleEndedEvent;
import robocode.BulletHitBulletEvent;
import robocode.BulletHitEvent;
import robocode.BulletMissedEvent;
import robocode.DeathEvent;
import robocode.HitByBulletEvent;
import robocode.HitRobotEvent;
import robocode.HitWallEvent;
import robocode.RobotDeathEvent;
import robocode.RoundEndedEvent;
import robocode.ScannedRobotEvent;
import robocode.StatusEvent;
import robocode.WinEvent;
import robocode.robotinterfaces.IBasicEvents;

public class RandomFowardMove implements IBasicEvents, IBattleEvents, ITick {
	private final AdvancedRobot robot;
	private Random rand = new Random();
	private int direction = 1;
	private int tick = 0;

	public RandomFowardMove(AdvancedRobot robot) {
		this.robot = robot;
	}

	@Override
	public void onRoundStarted() {

	}

	@Override
	public void onTick() {
		if (tick >= 60) {
			tick = 0;
			robot.setTurnRightRadians(rand.nextFloat() * (Math.PI / 4) - (Math.PI / 8));
			robot.setAhead(rand.nextInt(2000) * direction);
			direction = rand.nextFloat() < 0.5f ? -direction : direction;
		}
		++tick;
	}

	@Override
	public void onHitRobot(HitRobotEvent arg0) {
		robot.setTurnRightRadians(rand.nextFloat() * Math.PI);
		robot.setBack(200);
	}

	@Override
	public void onHitWall(HitWallEvent arg0) {
		robot.setTurnRightRadians(rand.nextFloat() * Math.PI);
		robot.setBack(200);
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
	public void onBattleEnded(BattleEndedEvent arg0) {
		// TODO Auto-generated method stub

	}

	@Override
	public void onRoundEnded(RoundEndedEvent arg0) {
		// TODO Auto-generated method stub

	}

}
