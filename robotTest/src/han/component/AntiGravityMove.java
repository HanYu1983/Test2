package han.component;

import java.awt.Color;
import java.awt.Graphics2D;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

import org.jbox2d.common.MathUtils;
import org.jbox2d.common.Vec2;

import han.component.action.MoveTo;
import robocode.robotinterfaces.IPaintEvents;

public class AntiGravityMove extends MoveTo implements IPaintEvents {
	private static final long serialVersionUID = -5557924474228349327L;

	public static class Point {
		public Vec2 value;
		public double gravity;

		public Point(Vec2 value, double gravity) {
			this.value = value;
			this.gravity = gravity;
		}
	}

	private List<Point> gravityPoints;

	public List<Point> getGravityPoints() {
		return this.gravityPoints;
	}

	public enum Mode {
		Gravity, AntiGravity
	}

	public Mode mode;
	public boolean isUseDistance;
	public int duration;
	private transient int tick = 0;

	private transient List<Vec2> points = new LinkedList<>();
	private String robotKey;

	public AntiGravityMove(String robotKey, ComponentRobot robot) {
		super(robotKey, robot);
		this.robotKey = robotKey;
		if (robot != null) {
			this.setRobot(robot);
		}
		gravityPoints = new LinkedList<>();
		duration = 30;
		mode = Mode.Gravity;
	}

	// 建構子呼叫的方法使用多型是反模式, 所以直接宣告成final, 不得有任何覆寫的機會
	private final void setRobot(ComponentRobot robot) {
		double dw = robot.getBattleFieldWidth() / 10;
		double dh = robot.getBattleFieldHeight() / 10;
		points.clear();
		for (double x = dw; x < robot.getBattleFieldWidth() - dw; x += dw) {
			for (double y = dh; y < robot.getBattleFieldHeight() - dh; y += dh) {
				points.add(new Vec2((float) x, (float) y));
			}
		}
	}

	private transient Vec2 lastTargetPoint = new Vec2();

	public Vec2 getTargetPoint() {
		if (points.isEmpty()) {
			return null;
		}
		if (gravityPoints.isEmpty()) {
			return null;
		}
		List<Double> gs = new LinkedList<>();
		for (Vec2 left : points) {
			double dist = 0;
			for (Point right : gravityPoints) {
				if (this.isUseDistance) {
					dist += MathUtils.distanceSquared(left, right.value);
				} else {
					dist += right.gravity;
				}
			}
			gs.add(dist);
		}

		switch (mode) {
		case AntiGravity: {
			double min = Double.MAX_VALUE;
			int minId = 0;
			for (int i = 0; i < gs.size(); ++i) {
				if (min > gs.get(i)) {
					min = gs.get(i);
					minId = i;
				}
			}
			Vec2 ret = points.get(minId);
			lastTargetPoint.set(ret);
			return ret;
		}
		case Gravity: {
			double max = 0;
			int maxId = 0;
			for (int i = 0; i < gs.size(); ++i) {
				if (max < gs.get(i)) {
					max = gs.get(i);
					maxId = i;
				}
			}
			Vec2 ret = points.get(maxId);
			lastTargetPoint.set(ret);
			return ret;
		}
		default:
			return points.get(0);
		}
	}

	@Override
	public void onTick() {
		super.onTick();
		if (tick == 0) {
			Vec2 target = this.getTargetPoint();
			if (target == null) {
				return;
			}
			super.getMoveToPosition().set(target);
		}

		if (++tick > duration) {
			tick = 0;
		}
	}

	@Override
	public void onFindRef(Map<String, Object> pool) {
		super.onFindRef(pool);
		ComponentRobot robot = (ComponentRobot) pool.get(robotKey);
		this.setRobot(robot);
	}

	@Override
	public void onPaint(Graphics2D g) {
		g.setColor(Color.green);
		g.fillOval((int) lastTargetPoint.x - 25, (int) lastTargetPoint.y - 25, 50, 50);
	}
}
