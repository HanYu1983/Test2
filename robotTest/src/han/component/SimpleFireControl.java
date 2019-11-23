package han.component;

import java.util.LinkedList;
import java.util.List;

import robocode.AdvancedRobot;
import robocode.Bullet;

public class SimpleFireControl implements ITick {
	public interface IQuery {
		double getBestHeading(double speed);
	}

	private final AdvancedRobot robot;
	private final IQuery query;
	public List<Bullet> bullets = new LinkedList<>();

	public SimpleFireControl(AdvancedRobot robot, IQuery query) {
		this.robot = robot;
		this.query = query;
	}

	private void syncGun() {
		double heading = query.getBestHeading(robocode.Rules.getBulletSpeed(1.5));
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