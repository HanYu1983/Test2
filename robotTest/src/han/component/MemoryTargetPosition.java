package han.component;

import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.geom.Point2D;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import java.util.Set;

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

public class MemoryTargetPosition implements IBasicEvents, ITick, IPaintEvents, SimpleFireControl.IQuery {
	private final ComponentRobot robot;

	public MemoryTargetPosition(ComponentRobot robot) {
		this.robot = robot;
	}

	public static class MemoryPoint {
		public final Point2D point;
		public final long time;

		public MemoryPoint(Point2D point, long time) {
			this.point = point;
			this.time = time;
		}
	}

	// private Point2D bestTargetPoint = new Point2D.Double();
	private Map<String, Point2D> robotBestPoint = new HashMap<>();
	private Map<String, List<MemoryPoint>> robotPositionHistory = new HashMap<>();

	public Point2D getBestTargetPoint(String robotName) {
		if (this.robotBestPoint.containsKey(robotName) == false) {
			this.robotBestPoint.put(robotName, new Point2D.Double());
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

	public Point2D calcBestPoint(String robotName, double speed) {
		if (getHistory(robotName).size() < 3) {
			return null;
		}
		List<MemoryPoint> robotHistory = getHistory(robotName);
		MemoryPoint a = robotHistory.get(0);
		MemoryPoint b = robotHistory.get(1);
		MemoryPoint c = robotHistory.get(2);
		long dt = c.time - b.time;

		double dd = Point2D.distance(c.point.getX(), c.point.getY(), b.point.getX(), b.point.getY());
		double v = dd / dt;

		Point2D ab = new Point2D.Double(b.point.getX() - a.point.getX(), b.point.getY() - a.point.getY());
		Point2D bc = new Point2D.Double(c.point.getX() - b.point.getX(), c.point.getY() - b.point.getY());
		double abAngle = Math.atan2(ab.getX(), ab.getY());
		double cbAngle = Math.atan2(bc.getX(), bc.getY());
		double bearing = robocode.util.Utils.normalRelativeAngle(cbAngle - abAngle);
		double vrot = bearing / dt;

		double bulletMoveRange = 0;
		Point2D nextPoint = (Point2D) c.point.clone();
		double heading = cbAngle;
		for (int i = 0; i < 200; ++i) {
			double dist = Point2D.distance(robot.getX(), robot.getY(), nextPoint.getX(), nextPoint.getY());
			if (dist < bulletMoveRange) {
				break;
			}
			if (i >= 5) {
				bulletMoveRange += speed;
			}
			heading += vrot;
			nextPoint.setLocation(nextPoint.getX() + Math.sin(heading) * v, nextPoint.getY() + Math.cos(heading) * v);
		}
		getBestTargetPoint(robotName).setLocation(nextPoint);
		return getBestTargetPoint(robotName);
	}

	public double getBestHeading(String robotName, double speed) {
		Point2D p = calcBestPoint(robotName, speed);
		if (p == null) {
			return 0;
		}
		double hx = p.getX() - robot.getX();
		double hy = p.getY() - robot.getY();
		return robocode.util.Utils.normalAbsoluteAngle(Math.atan2(hx, hy));
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
		robotHistory.add(new MemoryPoint(new Point2D.Double(tx, ty), robot.getTime()));
		if (robotHistory.size() > 3) {
			robotHistory.remove(0);
		}
	}

	@Override
	public void onPaint(Graphics2D g) {
		for (String robotName : this.robotPositionHistory.keySet()) {
			List<MemoryPoint> robotHistory = this.robotPositionHistory.get(robotName);
			for (int i = 0; i < robotHistory.size(); ++i) {
				Point2D p = robotHistory.get(i).point;
				g.setColor(Color.red);
				g.fillOval((int) p.getX() - 25, (int) p.getY() - 25, 50, 50);
				g.setColor(Color.black);
				g.drawString(robotName, (int) p.getX(), (int) p.getY());
			}
		}

		for (String robotName : this.robotBestPoint.keySet()) {
			Point2D p = robotBestPoint.get(robotName);
			g.setColor(Color.green);
			g.fillOval((int) p.getX() - 25, (int) p.getY() - 25, 50, 50);
			g.setColor(Color.black);
			g.drawString(robotName, (int) p.getX(), (int) p.getY());
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
