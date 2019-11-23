package han.component;

import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.geom.Point2D;
import java.util.LinkedList;
import java.util.List;

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

public class MemoryTargetPosition implements IBasicEvents, ITick, IPaintEvents, SimpleFireControl.IQuery {
	private final AdvancedRobot robot;

	public MemoryTargetPosition(AdvancedRobot robot) {
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

	private Point2D bestTargetPoint = new Point2D.Double();
	private List<MemoryPoint> history = new LinkedList<MemoryPoint>();

	public Point2D getBestTargetPoint() {
		return bestTargetPoint;
	}

	private void calcBestPoint(double speed) {
		if (history.size() < 3) {
			return;
		}
		MemoryPoint a = history.get(0);
		MemoryPoint b = history.get(1);
		MemoryPoint c = history.get(2);
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
		bestTargetPoint.setLocation(nextPoint);
	}

	public double getBestHeading(double speed) {
		calcBestPoint(speed);
		double hx = bestTargetPoint.getX() - robot.getX();
		double hy = bestTargetPoint.getY() - robot.getY();
		return robocode.util.Utils.normalAbsoluteAngle(Math.atan2(hx, hy));
	}

	@Override
	public void onScannedRobot(ScannedRobotEvent event) {
		double dist = event.getDistance();
		double bearing = event.getBearingRadians();
		double headingToTarget = robocode.util.Utils.normalAbsoluteAngle(robot.getHeadingRadians() + bearing);

		double x = robot.getX();
		double y = robot.getY();

		double ox = Math.sin(headingToTarget) * dist;
		double oy = Math.cos(headingToTarget) * dist;

		double tx = x + ox;
		double ty = y + oy;

		this.history.add(new MemoryPoint(new Point2D.Double(tx, ty), robot.getTime()));
		if (this.history.size() > 3) {
			this.history.remove(0);
		}
	}

	@Override
	public void onPaint(Graphics2D g) {
		g.setColor(Color.red);
		for (int i = 0; i < history.size(); ++i) {
			Point2D p = history.get(i).point;
			g.fillOval((int) p.getX() - 25, (int) p.getY() - 25, 50, 50);
		}

		g.setColor(Color.green);
		g.fillOval((int) this.bestTargetPoint.getX() - 25, (int) this.bestTargetPoint.getY() - 25, 50, 50);
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
