package han;

import static java.awt.event.KeyEvent.VK_1;
import static java.awt.event.KeyEvent.VK_2;
import static java.awt.event.KeyEvent.VK_3;
import static java.awt.event.KeyEvent.VK_4;

import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.event.KeyEvent;

import han.component.Components;
import robocode.AdvancedRobot;
import robocode.BulletHitBulletEvent;
import robocode.BulletHitEvent;
import robocode.BulletMissedEvent;
import robocode.HitByBulletEvent;
import robocode.HitRobotEvent;
import robocode.HitWallEvent;
import robocode.ScannedRobotEvent;

public class Test02 extends AdvancedRobot {

	private RadarSystem radarSystem = new RadarSystem(this);

	private Components coms = new Components();
	{
		coms.addComponent(radarSystem);
		coms.addComponent(new CrazyMoveSystem(this));
		// coms.addComponent(new RamFireSystem(this));
	}

	private void syncGun() {
		if (radarSystem.getState() != RadarSystem.State.targeting) {
			return;
		}
		double heading = radarSystem.getBestHeading(robocode.Rules.getBulletSpeed(robocode.Rules.MAX_BULLET_POWER));
		double oa = robocode.util.Utils.normalRelativeAngle(heading - this.getGunHeadingRadians());
		this.setTurnGunRightRadians(oa);
		if (Math.abs(oa) < Math.PI / 10) {
			fire(robocode.Rules.MAX_BULLET_POWER);
		}
	}

	public void run() {
		this.setAdjustRadarForGunTurn(true);
		this.setAdjustGunForRobotTurn(true);
		setColors(Color.BLACK, Color.WHITE, Color.RED);
		while (true) {
			coms.tick();
			syncGun();
			execute();
		}
	}

	@Override
	public void onKeyPressed(KeyEvent e) {
		switch (e.getKeyCode()) {
		case VK_1:
			super.setAhead(10);
			break;
		case VK_2:
			super.setTurnRadarLeft(360);
			break;
		case VK_3:
			super.setTurnLeft(10);
			break;
		case VK_4:
			super.fire(4);
			break;
		}
	}

	@Override
	public void onHitWall(HitWallEvent event) {
		coms.onHitWall(event);
	}

	@Override
	public void onBulletHit(BulletHitEvent event) {
		coms.onBulletHit(event);
	}

	@Override
	public void onBulletHitBullet(BulletHitBulletEvent event) {
		coms.onBulletHitBullet(event);
	}

	@Override
	public void onHitByBullet(HitByBulletEvent event) {
		coms.onHitByBullet(event);
	}

	@Override
	public void onHitRobot(HitRobotEvent event) {
		coms.onHitRobot(event);
	}

	@Override
	public void onBulletMissed(BulletMissedEvent event) {
		coms.onBulletMissed(event);
	}

	@Override
	public void onScannedRobot(ScannedRobotEvent event) {
		coms.onScannedRobot(event);
	}

	@Override
	public void onPaint(Graphics2D arg0) {
		coms.onPaint(arg0);
	}
}
