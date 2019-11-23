package han;

import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.geom.Point2D;
import java.util.LinkedList;
import java.util.List;

import han.component.ITick;
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

public class RadarSystem implements IBasicEvents, ITick, IPaintEvents {
	private final AdvancedRobot robot;

	public static enum State {
		scan, targeting
	}

	private State state = State.scan;
	public State getState() {
		return state;
	}

	public void update() {
		switch (state) {
		case scan: {
			robot.setTurnRadarLeft(5);
		}
			break;
		case targeting: {
			double oa = this.calcOffset();
			double heading = this.getHeadingToLastTargetPoint(robot);
			double shouldTurnTo = heading + oa;

			double offset = robocode.util.Utils.normalRelativeAngle(shouldTurnTo - this.robot.getRadarHeadingRadians());
			this.robot.setTurnRadarRightRadians(offset);
			if (this.updateValue(Math.PI / 10) == false) {
				this.state = State.scan;
			}

		}
			break;
		}
	}

	@Override
	public void onTick() {
		this.update();
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

	public static class MemoryPoint {
		public final Point2D point;
		public final long time;

		public MemoryPoint(Point2D point, long time) {
			this.point = point;
			this.time = time;
		}
	}

	private Point2D lastTargetPoint = new Point2D.Double();
	private Point2D bestTargetPoint = new Point2D.Double();
	private List<MemoryPoint> history = new LinkedList<MemoryPoint>();
	
	public Point2D getLastTargetPoint() {
		return lastTargetPoint;
	}
	
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
			if(dist < bulletMoveRange) {
				break;
			}
			if(i >= 5) {
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

	public double getHeadingToLastTargetPoint(AdvancedRobot robot) {
		if (history.size() <= 0) {
			return 0;
		}
		double dx = history.get(history.size() - 1).point.getX() - robot.getX();
		double dy = history.get(history.size() - 1).point.getY() - robot.getY();
		double heading = robocode.util.Utils.normalAbsoluteAngle(Math.atan2(dx, dy));
		return heading;
	}

	@Override
	public void onScannedRobot(ScannedRobotEvent event) {
		String targetName = event.getName();
		double dist = event.getDistance();
		double bearing = event.getBearingRadians();
		double headingToTarget = robocode.util.Utils.normalAbsoluteAngle(robot.getHeadingRadians() + bearing);

		double x = robot.getX();
		double y = robot.getY();

		double ox = Math.sin(headingToTarget) * dist;
		double oy = Math.cos(headingToTarget) * dist;

		double tx = x + ox;
		double ty = y + oy;

		this.lastTargetPoint.setLocation(tx, ty);
		this.history.add(new MemoryPoint((Point2D) this.lastTargetPoint.clone(), robot.getTime()));
		if (this.history.size() > 3) {
			this.history.remove(0);
		}

		double factor = (48 * robocode.Rules.MAX_VELOCITY) / (2 * Math.PI * dist);
		double radiansRange = factor * (Math.PI * 2);

		switch (state) {
		case scan:
			this.clearValue();
			this.setTargetingRange(radiansRange);
			this.state = State.targeting;
			break;
		case targeting:
			this.find();
			this.setTargetingRange(radiansRange);
			break;
		}
	}

	@Override
	public void onStatus(StatusEvent arg0) {
		// TODO Auto-generated method stub

	}

	@Override
	public void onWin(WinEvent arg0) {
		// TODO Auto-generated method stub

	}

	private double targetingRange = Math.PI;
	public static int maxTargetingLevel = 2;
	private int targetingLevel = 1;
	private double targetingValue;
	private boolean findFlag;

	public int getLevel() {
		return targetingLevel;
	}

	public void setTargetingRange(double v) {
		this.targetingRange = v;
	}

	public double calcOffset() {
		double range = targetingRange / Math.pow(targetingLevel, 2);
		double oa = Math.cos(targetingValue) * range;
		return oa;
	}

	public boolean updateValue(double a) {
		this.targetingValue += a;
		if (this.targetingValue > (Math.PI * 2)) {
			if (this.findFlag == false) {
				this.targetingValue %= (Math.PI * 2);
				this.targetingLevel -= 1;
				if (this.targetingLevel < 1) {
					this.targetingLevel = 1;
					return false;
				}
			} else {
				this.findFlag = false;
			}
		}
		return true;
	}

	public void find() {
		this.findFlag = true;
		this.targetingLevel += 1;
		if (this.targetingLevel > maxTargetingLevel) {
			this.targetingLevel = maxTargetingLevel;
		}
	}

	public void clearValue() {
		this.targetingValue = 0;
		this.findFlag = false;
	}

	public RadarSystem(AdvancedRobot robot) {
		this.robot = robot;
	}

	@Override
	public void onPaint(Graphics2D g) {
		g.setColor(Color.red);

		double ty = Math.cos(robot.getRadarHeadingRadians()) * 100;
		double tx = Math.sin(robot.getRadarHeadingRadians()) * 100;

		g.setColor(Color.red);
		double oa = this.calcOffset();
		double heading = this.getHeadingToLastTargetPoint(robot);
		double shouldTurnTo = heading + oa;

		g.drawString("state:" + this.state + "/" + this.targetingLevel + "/" + (this.targetingValue / (Math.PI * 2)),
				(int) robot.getX() + 50, (int) robot.getY() + 50);
		g.drawString("oa:" + oa, (int) robot.getX() + 50, (int) robot.getY() + 70);
		// g.drawString(robot.getRadarHeading() + "", (int) robot.getX() + 50, (int)
		// robot.getY() + 50);
		// g.drawString(robot.getGunHeading() + "", (int) robot.getX() + 50, (int)
		// robot.getY() + 30);
		g.drawLine((int) robot.getX(), (int) robot.getY(), (int) (robot.getX() + tx), (int) (robot.getY() + ty));

		for (int x = 0; x < robot.getBattleFieldWidth(); x += 50) {
			g.drawLine(x, 0, x, (int) robot.getBattleFieldHeight());
			g.drawString("x:" + x, x, 20);
		}

		for (int y = 0; y < robot.getBattleFieldHeight(); y += 50) {
			g.drawLine(0, y, (int) robot.getBattleFieldWidth(), y);
			g.drawString("y:" + y, 20, y);
		}

		for(int i=0; i<history.size(); ++i) {
			Point2D p = history.get(i).point;
			g.fillOval((int) p.getX() - 25, (int) p.getY() - 25, 50, 50);
		}
		
		g.setColor(Color.green);
		g.fillOval((int) this.bestTargetPoint.getX() - 25, (int) this.bestTargetPoint.getY() - 25, 50, 50);

		
		ty = Math.cos(heading) * 100;
		tx = Math.sin(heading) * 100;
		g.setColor(Color.black);
		g.drawLine((int) robot.getX(), (int) robot.getY(), (int) (robot.getX() + tx), (int) (robot.getY() + ty));

		ty = Math.cos(shouldTurnTo) * 100;
		tx = Math.sin(shouldTurnTo) * 100;
		g.setColor(Color.blue);
		g.drawLine((int) robot.getX(), (int) robot.getY(), (int) (robot.getX() + tx), (int) (robot.getY() + ty));
	}

}
