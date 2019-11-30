package han.component;

import java.awt.Graphics2D;
import java.awt.event.KeyEvent;
import java.awt.event.MouseEvent;
import java.awt.event.MouseWheelEvent;
import java.io.EOFException;
import java.io.FileInputStream;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.Serializable;
import java.io.UnsupportedEncodingException;
import java.net.URLEncoder;

import org.jbox2d.common.Vec2;

import robocode.BattleEndedEvent;
import robocode.BulletHitBulletEvent;
import robocode.BulletHitEvent;
import robocode.BulletMissedEvent;
import robocode.CustomEvent;
import robocode.DeathEvent;
import robocode.HitByBulletEvent;
import robocode.HitRobotEvent;
import robocode.HitWallEvent;
import robocode.MessageEvent;
import robocode.RobocodeFileOutputStream;
import robocode.RobotDeathEvent;
import robocode.RoundEndedEvent;
import robocode.ScannedRobotEvent;
import robocode.SkippedTurnEvent;
import robocode.StatusEvent;
import robocode.TeamRobot;
import robocode.WinEvent;

public class ComponentRobot extends TeamRobot {
	public final ComponentList coms = new ComponentList(null);
	private String fileName;
	private String opponent;
	private Vec2 pos = new Vec2();

	public String getOpponent() {
		return opponent;
	}

	public void setOpponent(String name) {
		opponent = name;
	}

	public Vec2 getPosition() {
		pos.x = (float) getX();
		pos.y = (float) getY();
		return pos;
	}

	public boolean simpleSend(Serializable msg) {
		try {
			this.broadcastMessage(msg);
			return true;
		} catch (IOException ex) {
			ex.printStackTrace(out);
		}
		return false;
	}

	public boolean simpleSend(String name, Serializable msg) {
		try {
			this.sendMessage(name, msg);
			return true;
		} catch (IOException ex) {
			ex.printStackTrace(out);
		}
		return false;
	}

	public void run() {
		String fileName = getName() + ".txt";
		try {
			fileName = URLEncoder.encode(getName(), "UTF-8") + ".txt";
		} catch (UnsupportedEncodingException e1) {
			System.out.println("encode file name error. ignore");
		}
		this.fileName = fileName;

		ObjectInputStream reader = null;
		try {
			reader = new ObjectInputStream(new FileInputStream(getDataFile(this.fileName)));
			onInputStream(reader);
		} catch (Exception e) {
			if (e instanceof EOFException) {
				System.out.println("no content");
			} else {
				e.printStackTrace();
			}
		} finally {
			if (reader != null) {
				try {
					reader.close();
				} catch (Exception e) {
				}
			}
		}
		onRoundStarted();
		this.setAdjustRadarForGunTurn(true);
		this.setAdjustGunForRobotTurn(true);
		this.setTurnRadarRightRadians(Math.PI * 2);
		while (true) {
			coms.tick();
			execute();
		}
	}

	public void onInputStream(ObjectInputStream ois) throws IOException {
		coms.onInputStream(ois);
	}

	public void onOutputStream(ObjectOutputStream oos) throws IOException {
		coms.onOutputStream(oos);
	}

	public void onRoundStarted() {
		coms.onRoundStarted();
	}

	@Override
	public void onCustomEvent(CustomEvent event) {

	}

	@Override
	public void onDeath(DeathEvent event) {
		coms.onDeath(event);
	}

	@Override
	public void onSkippedTurn(SkippedTurnEvent event) {

	}

	@Override
	public void onKeyPressed(KeyEvent e) {
		// TODO Auto-generated method stub
		coms.onKeyPressed(e);
	}

	@Override
	public void onKeyReleased(KeyEvent e) {
		coms.onKeyReleased(e);
	}

	@Override
	public void onKeyTyped(KeyEvent e) {
		coms.onKeyTyped(e);
	}

	@Override
	public void onMouseClicked(MouseEvent e) {
		coms.onMouseClicked(e);
	}

	@Override
	public void onMouseDragged(MouseEvent e) {
		coms.onMouseDragged(e);
	}

	@Override
	public void onMouseEntered(MouseEvent e) {
		coms.onMouseEntered(e);
	}

	@Override
	public void onMouseExited(MouseEvent e) {
		coms.onMouseExited(e);
	}

	@Override
	public void onMouseMoved(MouseEvent e) {
		coms.onMouseMoved(e);
	}

	@Override
	public void onMousePressed(MouseEvent e) {
		coms.onMousePressed(e);
	}

	@Override
	public void onMouseReleased(MouseEvent e) {
		coms.onMouseReleased(e);
	}

	@Override
	public void onMouseWheelMoved(MouseWheelEvent e) {
		coms.onMouseWheelMoved(e);
	}

	@Override
	public void onRobotDeath(RobotDeathEvent event) {
		coms.onRobotDeath(event);
		if (event.getName() == opponent) {
			this.opponent = null;
		}
	}

	@Override
	public void onStatus(StatusEvent e) {
		coms.onStatus(e);
	}

	@Override
	public void onWin(WinEvent event) {
		coms.onWin(event);
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
		if (opponent == null) {
			if (this.isTeammate(event.getName())) {
				return;
			}
			opponent = event.getName();
		}
	}

	@Override
	public void onPaint(Graphics2D arg0) {
		coms.onPaint(arg0);
	}

	@Override
	public void onBattleEnded(BattleEndedEvent arg0) {
		coms.onBattleEnded(arg0);
	}

	@Override
	public void onRoundEnded(RoundEndedEvent arg0) {
		coms.onRoundEnded(arg0);
		ObjectOutputStream w = null;
		try {
			w = new ObjectOutputStream(new RobocodeFileOutputStream(getDataFile(fileName)));
			onOutputStream(w);
		} catch (IOException e) {
			e.printStackTrace(out);
		} finally {
			if (w != null) {
				try {
					w.close();
				} catch (Exception e) {

				}
			}
		}
	}

	@Override
	public void onMessageReceived(MessageEvent arg0) {
		coms.onMessageReceived(arg0);
	}
}
