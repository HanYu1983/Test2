package han;

import static java.awt.event.KeyEvent.*;

import java.awt.Color;
import java.awt.Graphics2D;
import java.awt.event.KeyEvent;
import java.awt.geom.Point2D;

import robocode.AdvancedRobot;
import robocode.BulletHitBulletEvent;
import robocode.BulletHitEvent;
import robocode.BulletMissedEvent;
import robocode.HitByBulletEvent;
import robocode.HitRobotEvent;
import robocode.HitWallEvent;
import robocode.ScannedRobotEvent;

public class Test01 extends AdvancedRobot {
	public void run() {
		setColors(Color.BLACK, Color.WHITE, Color.RED);
		ahead(100);
		while (true) {
			super.setTurnRadarLeft(5);
			this.turnGunToTarget();
			this.fireIfTargeted();
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
		//super.getX();
		//super.getY();
		
	}

	@Override
	public void onBulletHit(BulletHitEvent event) {
		event.getBullet().getName();
		event.getBullet().getHeading();
	}

	@Override
	public void onBulletHitBullet(BulletHitBulletEvent event) {
		event.getBullet().getName();
	}

	@Override
	public void onHitByBullet(HitByBulletEvent event) {

	}

	@Override
	public void onHitRobot(HitRobotEvent event) {

	}

	@Override
	public void onBulletMissed(BulletMissedEvent event) {
		System.out.println("onBulletMissed:"+event.getBullet().getName());
		System.out.println("onBulletMissed:"+event.getBullet().getPower());
		System.out.println("onBulletMissed:"+event.getBullet().getX());
		System.out.println("onBulletMissed:"+event.getBullet().getY());
	}

	@Override
	public void onScannedRobot(ScannedRobotEvent event) {
		System.out.println("onScannedRobot:"+event.getName());
		System.out.println("onScannedRobot:"+event.getDistance());
		System.out.println("onScannedRobot:"+super.getRadarHeading());
		
		
		double x = getX();
		double y = getY();
		
		double ox = Math.sin(getRadarHeadingRadians())* event.getDistance();
		double oy = Math.cos(getRadarHeadingRadians())* event.getDistance();
		
		double tx = x + ox;
		double ty = y + oy;
		
		this.targetPoint.setLocation(tx, ty);
		
		double oa = this.getRadarHeadingRadians() - this.getGunHeadingRadians();
		boolean shouldOtherSize = Math.abs(oa) >= Math.PI;
		if(shouldOtherSize) {
			oa = -(oa % Math.PI);
		}
		//this.setTurnGunRight(oa*180/Math.PI);
	}
	
	private Point2D targetPoint = new Point2D.Float();
	private double targetHeading = 0;
	private double oa = 0;
	
	
	private void turnGunToTarget() {
		double dx = targetPoint.getX() - getX();
		double dy = targetPoint.getY() - getY();
		
		double heading = robocode.util.Utils.normalAbsoluteAngle(Math.atan2(dx, dy));
		//double heading = (Math.atan2(dx, dy) + (Math.PI*2)) % (Math.PI*2);
		
		double oa = robocode.util.Utils.normalRelativeAngle(heading - this.getGunHeadingRadians());
		/*
		double oa = heading - this.getGunHeadingRadians();
		boolean shouldOtherSize = Math.abs(oa) > Math.PI;
		if(shouldOtherSize) {
			oa = -(oa % Math.PI);
		}*/
		this.setTurnGunRight(oa > 0 ? 1 : -1);
		
		this.oa = oa;
		this.targetHeading = heading;
	}
	
	private void fireIfTargeted() {
		boolean ifTargeted = Math.abs(this.oa) < 0.1;
		if(ifTargeted) {
			fire(10);
		}
	}
	
	public void onPaint(Graphics2D g) {
		g.setColor(Color.red);
		g.drawString("ver1", 50, 50);
		
		g.drawOval((int) (getX() - 50), (int) (getY() - 50), 100, 100);
		
		g.setColor(new Color(0, 0xFF, 0, 30));
		g.fillOval((int) (getX() - 60), (int) (getY() - 60), 120, 120);
		
		double ty = Math.cos(getRadarHeadingRadians())* 100;
		double tx = Math.sin(getRadarHeadingRadians())* 100;
		
		g.setColor(Color.red);
		g.drawString(this.getRadarHeading()+"", (int)getX()+50, (int)getY()+50);
		g.drawString(this.getGunHeading()+"", (int)getX()+50, (int)getY()+30);
		g.drawLine((int)getX(), (int)getY(), (int)(getX()+tx), (int)(getY()+ty));
		
		
		for(int x=0; x<this.getBattleFieldWidth(); x+=50) {
			g.drawLine(x, 0, x, (int)this.getBattleFieldHeight());
			g.drawString("x:"+x, x, 20);
		}
		
		for(int y=0; y<this.getBattleFieldHeight(); y+=50) {
			g.drawLine(0, y, (int)this.getBattleFieldWidth(), y);
			g.drawString("y:"+y, 20, y);
		}
		
		g.fillOval((int)this.targetPoint.getX()-25, (int)this.targetPoint.getY()-25, 50, 50);
		
		
		ty = Math.cos(targetHeading)* 100;
		tx = Math.sin(targetHeading)* 100;
		g.setColor(Color.black);
		g.drawLine((int)getX(), (int)getY(), (int)(getX()+tx), (int)(getY()+ty));
		g.drawString("oa:"+(oa*180/Math.PI), (int)getX()+50, (int)getY()+70);
	}
	
	
}
