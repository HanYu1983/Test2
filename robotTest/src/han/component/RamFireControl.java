package han.component;

import java.awt.Graphics2D;
import java.io.Serializable;

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

public class RamFireControl implements IBasicEvents, ITick, IPaintEvents, Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = 712716079119477667L;
	private final ComponentRobot robot;

	public RamFireControl(ComponentRobot robot) {
		this.robot = robot;
	}

	public void onHitRobot(HitRobotEvent e) {
		if(robot.isTeammate(e.getName())) {
			return;
		}
		double heading = robocode.util.Utils.normalAbsoluteAngle(robot.getHeadingRadians() + e.getBearingRadians());
		double gunBearing = robocode.util.Utils.normalRelativeAngle(heading - robot.getGunHeadingRadians());
		robot.turnGunRightRadians(gunBearing);
		if (e.getEnergy() > 16) {
			robot.fire(3);
		} else if (e.getEnergy() > 10) {
			robot.fire(2);
		} else if (e.getEnergy() > 4) {
			robot.fire(1);
		} else if (e.getEnergy() > 2) {
			robot.fire(.5);
		} else if (e.getEnergy() > .4) {
			robot.fire(.1);
		}
		robot.ahead(40);
	}

	@Override
	public void onPaint(Graphics2D arg0) {
		// TODO Auto-generated method stub
		
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
	public void onHitWall(HitWallEvent arg0) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onRobotDeath(RobotDeathEvent arg0) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onScannedRobot(ScannedRobotEvent arg0) {
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
