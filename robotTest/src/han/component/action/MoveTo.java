package han.component.action;

import java.io.Serializable;
import java.util.Map;

import org.jbox2d.common.MathUtils;
import org.jbox2d.common.Vec2;

import han.component.ComponentRobot;
import han.component.ISave;
import han.component.ITick;

public class MoveTo implements ITick, IAction, ISave, Serializable {
	private static final long serialVersionUID = -5567962435237025243L;
	public static final int MinRange = 50;
	private String robotKey;
	private transient ComponentRobot robot;
	private Vec2 position;

	public Vec2 getMoveToPosition() {
		return position;
	}

	public MoveTo(String robotKey, ComponentRobot robot) {
		this(robotKey, robot, new Vec2(0, 0));
	}

	public MoveTo(String robotKey, ComponentRobot robot, Vec2 firstPlace) {
		this.robotKey = robotKey;
		this.robot = robot;
		this.position = new Vec2(firstPlace);
	}

	public void targeting() {
		Vec2 dir = position.sub(robot.getPosition());
		double targetHeading = MathUtils.fastAtan2(dir.x, dir.y);
		double bearing = robocode.util.Utils.normalRelativeAngle(targetHeading - robot.getHeadingRadians());
		robot.setTurnRightRadians(bearing);
		robot.setAhead(dir.length());
	}

	public boolean checkSuccess() {
		Vec2 dir = position.sub(robot.getPosition());
		double len = dir.length();
		return len < MinRange;
	}

	private boolean isStop() {
		boolean isNoMove = Math.abs(robot.getVelocity()) < 1 && Math.abs(robot.getTurnRemainingRadians()) < 0.1;
		return isNoMove;
	}

	@Override
	public void onTick() {
		if (isStop()) {
			if (checkSuccess() == false) {
				targeting();
			}
		}
	}

	@Override
	public boolean isSuccess() {
		return checkSuccess();
	}

	@Override
	public void onRegister(Map<String, Object> pool) {

	}

	@Override
	public void onFindRef(Map<String, Object> pool) {
		robot = (ComponentRobot) pool.get(robotKey);
	}

}
