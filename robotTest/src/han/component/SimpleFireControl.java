package han.component;

import java.util.LinkedList;
import java.util.List;

import robocode.Bullet;

public class SimpleFireControl implements ITick {
	public interface IQuery {
		double getBestHeading(String robotName, double speed);
	}

	private final ComponentRobot robot;
	private final IQuery query;
	public final List<Bullet> bullets = new LinkedList<>();

	public SimpleFireControl(ComponentRobot robot, IQuery query) {
		this.robot = robot;
		this.query = query;
	}

	private void syncGun() {
		if (robot.getOpponent() == null) {
			return;
		}
		double heading = query.getBestHeading(robot.getOpponent(), robocode.Rules.getBulletSpeed(1.5));
		double oa = robocode.util.Utils.normalRelativeAngle(heading - robot.getGunHeadingRadians());
		robot.setTurnGunRightRadians(oa);
		if (Math.abs(oa) < Math.PI / 10) {
			if (robot.getGunHeat() == 0) {
				Bullet bullet = robot.fireBullet(Math.min(3 - (Math.abs(oa) * 180 / Math.PI), robot.getEnergy() - .1));
				bullets.add(bullet);
			}
		}
	}

	@Override
	public void onTick() {
		this.syncGun();
	}
}