package han.component;

import java.awt.Graphics2D;
import java.awt.event.KeyEvent;
import java.awt.event.MouseEvent;
import java.awt.event.MouseWheelEvent;
import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.Serializable;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

import robocode.BattleEndedEvent;
import robocode.BulletHitBulletEvent;
import robocode.BulletHitEvent;
import robocode.BulletMissedEvent;
import robocode.DeathEvent;
import robocode.HitByBulletEvent;
import robocode.HitRobotEvent;
import robocode.HitWallEvent;
import robocode.MessageEvent;
import robocode.RobotDeathEvent;
import robocode.RoundEndedEvent;
import robocode.ScannedRobotEvent;
import robocode.StatusEvent;
import robocode.WinEvent;
import robocode.robotinterfaces.IBasicEvents;
import robocode.robotinterfaces.IInteractiveEvents;
import robocode.robotinterfaces.IPaintEvents;
import robocode.robotinterfaces.ITeamEvents;

public class Components implements IBasicEvents, IBattleEvents, IInteractiveEvents, IPaintEvents, ITick, IFileEvents,
		ITeamEvents, ISave, Serializable {
	private static final long serialVersionUID = 7373097744376595809L;

	public void tick() {
		for (Object obj : components) {
			if (obj instanceof ITick) {
				((ITick) obj).onTick();
			}
		}
	}

	private List<Serializable> components = new LinkedList<>();

	public void addComponent(Serializable obj) {
		this.components.add(obj);
	}

	public void clearComponent() {
		this.components.clear();
	}

	public void removeComponent(Serializable obj) {
		this.components.remove(obj);
	}

	public List<Serializable> getComponents() {
		return components;
	}

	@Override
	public void onBulletHit(BulletHitEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IBasicEvents) {
				((IBasicEvents) obj).onBulletHit(arg0);
			}
		}
	}

	@Override
	public void onBulletHitBullet(BulletHitBulletEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IBasicEvents) {
				((IBasicEvents) obj).onBulletHitBullet(arg0);
			}
		}
	}

	@Override
	public void onBulletMissed(BulletMissedEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IBasicEvents) {
				((IBasicEvents) obj).onBulletMissed(arg0);
			}
		}
	}

	@Override
	public void onDeath(DeathEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IBasicEvents) {
				((IBasicEvents) obj).onDeath(arg0);
			}
		}
	}

	@Override
	public void onHitByBullet(HitByBulletEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IBasicEvents) {
				((IBasicEvents) obj).onHitByBullet(arg0);
			}
		}
	}

	@Override
	public void onHitRobot(HitRobotEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IBasicEvents) {
				((IBasicEvents) obj).onHitRobot(arg0);
			}
		}
	}

	@Override
	public void onHitWall(HitWallEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IBasicEvents) {
				((IBasicEvents) obj).onHitWall(arg0);
			}
		}
	}

	@Override
	public void onRobotDeath(RobotDeathEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IBasicEvents) {
				((IBasicEvents) obj).onRobotDeath(arg0);
			}
		}
	}

	@Override
	public void onScannedRobot(ScannedRobotEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IBasicEvents) {
				((IBasicEvents) obj).onScannedRobot(arg0);
			}
		}
	}

	@Override
	public void onStatus(StatusEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IBasicEvents) {
				((IBasicEvents) obj).onStatus(arg0);
			}
		}
	}

	@Override
	public void onWin(WinEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IBasicEvents) {
				((IBasicEvents) obj).onWin(arg0);
			}
		}
	}

	@Override
	public void onPaint(Graphics2D arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IPaintEvents) {
				((IPaintEvents) obj).onPaint(arg0);
			}
		}
	}

	@Override
	public void onKeyPressed(KeyEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IInteractiveEvents) {
				((IInteractiveEvents) obj).onKeyPressed(arg0);
			}
		}
	}

	@Override
	public void onKeyReleased(KeyEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IInteractiveEvents) {
				((IInteractiveEvents) obj).onKeyReleased(arg0);
			}
		}
	}

	@Override
	public void onKeyTyped(KeyEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IInteractiveEvents) {
				((IInteractiveEvents) obj).onKeyReleased(arg0);
			}
		}
	}

	@Override
	public void onMouseClicked(MouseEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IInteractiveEvents) {
				((IInteractiveEvents) obj).onMouseClicked(arg0);
			}
		}
	}

	@Override
	public void onMouseDragged(MouseEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IInteractiveEvents) {
				((IInteractiveEvents) obj).onMouseDragged(arg0);
			}
		}
	}

	@Override
	public void onMouseEntered(MouseEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IInteractiveEvents) {
				((IInteractiveEvents) obj).onMouseEntered(arg0);
			}
		}
	}

	@Override
	public void onMouseExited(MouseEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IInteractiveEvents) {
				((IInteractiveEvents) obj).onMouseExited(arg0);
			}
		}
	}

	@Override
	public void onMouseMoved(MouseEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IInteractiveEvents) {
				((IInteractiveEvents) obj).onMouseMoved(arg0);
			}
		}
	}

	@Override
	public void onMousePressed(MouseEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IInteractiveEvents) {
				((IInteractiveEvents) obj).onMousePressed(arg0);
			}
		}
	}

	@Override
	public void onMouseReleased(MouseEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IInteractiveEvents) {
				((IInteractiveEvents) obj).onMouseReleased(arg0);
			}
		}
	}

	@Override
	public void onMouseWheelMoved(MouseWheelEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IInteractiveEvents) {
				((IInteractiveEvents) obj).onMouseWheelMoved(arg0);
			}
		}
	}

	@Override
	public void onTick() {
		this.tick();
	}

	@Override
	public void onRoundEnded(RoundEndedEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IBattleEvents) {
				((IBattleEvents) obj).onRoundEnded(arg0);
			}
		}
	}

	@Override
	public void onBattleEnded(BattleEndedEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IBattleEvents) {
				((IBattleEvents) obj).onBattleEnded(arg0);
			}
		}
	}

	@Override
	public void onRoundStarted() {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IBattleEvents) {
				((IBattleEvents) obj).onRoundStarted();
			}
		}
	}

	@Override
	public void onInputStream(ObjectInputStream ois) throws IOException {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IFileEvents) {
				((IFileEvents) obj).onInputStream(ois);
			}
		}
	}

	@Override
	public void onOutputStream(ObjectOutputStream oos) throws IOException {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof IFileEvents) {
				((IFileEvents) obj).onOutputStream(oos);
			}
		}
	}

	@Override
	public void onMessageReceived(MessageEvent arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof ITeamEvents) {
				((ITeamEvents) obj).onMessageReceived(arg0);
			}
		}
	}

	@Override
	public void onRegister(Map<String, Object> arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof ISave) {
				((ISave) obj).onRegister(arg0);
			}
		}
	}

	@Override
	public void onFindRef(Map<String, Object> arg0) {
		for (Object obj : new LinkedList<Object>(components)) {
			if (obj instanceof ISave) {
				((ISave) obj).onFindRef(arg0);
			}
		}
	}
}
