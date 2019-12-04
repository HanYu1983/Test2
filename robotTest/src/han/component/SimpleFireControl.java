package han.component;

import java.io.Serializable;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

import robocode.Bullet;

public class SimpleFireControl implements ITick, Serializable, ISave {
	/**
	 * 
	 */
	private static final long serialVersionUID = -206651305467081758L;

	public interface IQuery {
		double getBestHeading(String robotName, double speed);
	}

	private transient ComponentRobot robot;
	private final IQuery query;
	private String robotKey;
	// transient的初始化不能寫在這裡, 必須用getter. 因為在反序列化時transient的欄位一定是空值
	private transient List<Bullet> bullets;

	public List<Bullet> getBullets() {
		if (bullets == null) {
			bullets = new LinkedList<>();
		}
		return bullets;
	}

	public SimpleFireControl(String robotKey, ComponentRobot robot, IQuery query) {
		this.robotKey = robotKey;
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
				getBullets().add(bullet);
			}
		}
	}

	@Override
	public void onTick() {
		this.syncGun();
	}

	@Override
	public void onRegister(Map<String, Object> pool) {

	}

	@Override
	public void onFindRef(Map<String, Object> pool) {
		ComponentRobot robot = (ComponentRobot) pool.get(robotKey);
		this.robot = robot;
	}
}