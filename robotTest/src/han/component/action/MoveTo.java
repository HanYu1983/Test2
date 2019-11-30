package han.component.action;

import org.jbox2d.common.MathUtils;
import org.jbox2d.common.Vec2;

import han.component.ComponentRobot;
import han.component.ITick;

public class MoveTo implements ITick, IAction{
	public static final int MinRange = 50;
	private final ComponentRobot robot;
	public final Vec2 position = new Vec2();

	public MoveTo(ComponentRobot robot) {
		this(robot, new Vec2(0, 0));
	}

	public MoveTo(ComponentRobot robot, Vec2 firstPlace) {
		this.robot = robot;
		this.position.set(firstPlace);
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

}