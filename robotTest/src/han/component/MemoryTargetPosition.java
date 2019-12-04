package han.component;

import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.geom.Point2D;
import java.io.Serializable;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import java.util.Set;

import org.jbox2d.common.MathUtils;
import org.jbox2d.common.Vec2;

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

public class MemoryTargetPosition
		implements IBasicEvents, ITick, IPaintEvents, SimpleFireControl.IQuery, ISave, Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = 5978017709912969161L;
	private transient ComponentRobot robot;
	private String robotKey;

	protected MemoryTargetPosition() {

	}

	public MemoryTargetPosition(String robotKey) {
		this.robotKey = robotKey;
	}

	public MemoryTargetPosition(ComponentRobot robot) {
		this.robot = robot;
	}

	public static class MemoryPoint implements Serializable {
		private static final long serialVersionUID = -6482749681320510336L;
		public Vec2 point;
		public long time;

		protected MemoryPoint() {

		}

		public MemoryPoint(Vec2 point, long time) {
			this.point = point;
			this.time = time;
		}
	}

	// private Point2D bestTargetPoint = new Point2D.Double();
	private Map<String, Vec2> robotBestPoint = new HashMap<>();
	private Map<String, List<MemoryPoint>> robotPositionHistory = new HashMap<>();

	public Vec2 getBestTargetPoint(String robotName) {
		if (this.robotBestPoint.containsKey(robotName) == false) {
			this.robotBestPoint.put(robotName, new Vec2());
		}
		return this.robotBestPoint.get(robotName);
	}

	public List<MemoryPoint> getHistory(String robotName) {
		if (this.robotPositionHistory.containsKey(robotName) == false) {
			this.robotPositionHistory.put(robotName, new LinkedList<>());
		}
		return this.robotPositionHistory.get(robotName);
	}

	public Set<String> getRobotNames() {
		return this.robotPositionHistory.keySet();
	}

	public Vec2 computeBestPoint(String robotName, double speed) {
		if (getHistory(robotName).size() < 3) {
			return null;
		}
		List<MemoryPoint> robotHistory = getHistory(robotName);
		MemoryPoint a = robotHistory.get(0);
		MemoryPoint b = robotHistory.get(1);
		MemoryPoint c = robotHistory.get(2);
		long dt = c.time - b.time;

		float dd = MathUtils.distance(c.point, b.point);
		float v = dd / dt;

		Vec2 ab = b.point.sub(a.point);
		Vec2 bc = c.point.sub(b.point);
		float abAngle = MathUtils.atan2(ab.x, ab.y);
		float cbAngle = MathUtils.atan2(bc.x, bc.y);
		float bearing = (float) robocode.util.Utils.normalRelativeAngle(cbAngle - abAngle);
		float vrot = bearing / dt;

		float bulletMoveRange = 0;
		Vec2 nextPoint = c.point.clone();
		float heading = cbAngle;
		for (int i = 0; i < 200; ++i) {
			float dist = (float) Point2D.distance(robot.getX(), robot.getY(), nextPoint.x, nextPoint.y);
			if (dist < bulletMoveRange) {
				break;
			}
			if (i >= 5) {
				bulletMoveRange += speed;
			}
			heading += vrot;
			nextPoint.set(nextPoint.x + MathUtils.sin(heading) * v, nextPoint.y + MathUtils.cos(heading) * v);
		}
		getBestTargetPoint(robotName).set(nextPoint);
		return getBestTargetPoint(robotName);
	}

	public double getBestHeading(String robotName, double speed) {
		Vec2 p = computeBestPoint(robotName, speed);
		if (p == null) {
			return 0;
		}
		float hx = p.x - (float) robot.getX();
		float hy = p.y - (float) robot.getY();
		return robocode.util.Utils.normalAbsoluteAngle(MathUtils.atan2(hx, hy));
	}

	@Override
	public void onScannedRobot(ScannedRobotEvent event) {
		if (robot.isTeammate(event.getName())) {
			return;
		}
		String robotName = event.getName();

		double dist = event.getDistance();
		double bearing = event.getBearingRadians();
		double headingToTarget = robocode.util.Utils.normalAbsoluteAngle(robot.getHeadingRadians() + bearing);

		double x = robot.getX();
		double y = robot.getY();

		double ox = Math.sin(headingToTarget) * dist;
		double oy = Math.cos(headingToTarget) * dist;

		double tx = x + ox;
		double ty = y + oy;

		List<MemoryPoint> robotHistory = this.getHistory(robotName);
		robotHistory.add(new MemoryPoint(new Vec2((float) tx, (float) ty), robot.getTime()));
		if (robotHistory.size() > 3) {
			robotHistory.remove(0);
		}
	}

	@Override
	public void onPaint(Graphics2D g) {
		for (String robotName : this.robotPositionHistory.keySet()) {
			List<MemoryPoint> robotHistory = this.robotPositionHistory.get(robotName);
			for (int i = 0; i < robotHistory.size(); ++i) {
				Vec2 p = robotHistory.get(i).point;
				g.setColor(Color.red);
				g.fillOval((int) p.x - 25, (int) p.y - 25, 50, 50);
				g.setColor(Color.black);
				g.drawString(robotName, (int) p.x, (int) p.y);
			}
		}

		for (String robotName : this.robotBestPoint.keySet()) {
			Vec2 p = robotBestPoint.get(robotName);
			g.setColor(Color.green);
			g.fillOval((int) p.x - 25, (int) p.y - 25, 50, 50);
			g.setColor(Color.black);
			g.drawString(robotName, (int) p.x, (int) p.y);
		}

	}

	@Override
	public void onTick() {
		// TODO Auto-generated method stub

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
		this.robotPositionHistory.remove(arg0.getName());
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
	public void onRegister(Map<String, Object> pool) {
		// TODO Auto-generated method stub

	}

	@Override
	public void onFindRef(Map<String, Object> pool) {
		this.robot = (ComponentRobot) pool.get(robotKey);
	}

}
