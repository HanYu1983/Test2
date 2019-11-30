package han.component;

import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.geom.Point2D;
import java.io.Serializable;

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

public class RadarMovement implements IBasicEvents, ITick, IPaintEvents, Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = -2341733289391097737L;
	private final ComponentRobot robot;

	public RadarMovement(ComponentRobot robot) {
		this.robot = robot;
	}

	public static enum State {
		scan, targeting
	}

	private double targetingRange = Math.PI;
	public static int maxTargetingLevel = 2;
	private int targetingLevel = 1;
	private double targetingValue;
	private boolean findFlag;
	private Point2D lastTargetPoint = new Point2D.Double();

	private State state = State.scan;

	public State getState() {
		return state;
	}

	public Point2D getLastTargetPoint() {
		return lastTargetPoint;
	}

	public double getHeadingToLastTargetPoint(AdvancedRobot robot) {
		double dx = lastTargetPoint.getX() - robot.getX();
		double dy = lastTargetPoint.getY() - robot.getY();
		double heading = robocode.util.Utils.normalAbsoluteAngle(Math.atan2(dx, dy));
		return heading;
	}

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

	@Override
	public void onScannedRobot(ScannedRobotEvent event) {
		String robotName = event.getName();
		if (robot.getOpponent() != robotName) {
			return;
		}

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
		g.drawLine((int) robot.getX(), (int) robot.getY(), (int) (robot.getX() + tx), (int) (robot.getY() + ty));

		for (int x = 0; x < robot.getBattleFieldWidth(); x += 50) {
			g.drawLine(x, 0, x, (int) robot.getBattleFieldHeight());
			g.drawString("x:" + x, x, 20);
		}

		for (int y = 0; y < robot.getBattleFieldHeight(); y += 50) {
			g.drawLine(0, y, (int) robot.getBattleFieldWidth(), y);
			g.drawString("y:" + y, 20, y);
		}
		ty = Math.cos(heading) * 100;
		tx = Math.sin(heading) * 100;
		g.setColor(Color.black);
		g.drawLine((int) robot.getX(), (int) robot.getY(), (int) (robot.getX() + tx), (int) (robot.getY() + ty));

		ty = Math.cos(shouldTurnTo) * 100;
		tx = Math.sin(shouldTurnTo) * 100;
		g.setColor(Color.blue);
		g.drawLine((int) robot.getX(), (int) robot.getY(), (int) (robot.getX() + tx), (int) (robot.getY() + ty));
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

	@Override
	public void onStatus(StatusEvent arg0) {
		// TODO Auto-generated method stub

	}

	@Override
	public void onWin(WinEvent arg0) {
		// TODO Auto-generated method stub

	}

}
