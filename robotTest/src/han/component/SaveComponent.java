package han.component;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.Serializable;
import java.util.HashMap;
import java.util.Map;

import robocode.BattleEndedEvent;
import robocode.BulletHitBulletEvent;
import robocode.BulletHitEvent;
import robocode.BulletMissedEvent;
import robocode.DeathEvent;
import robocode.HitByBulletEvent;
import robocode.HitRobotEvent;
import robocode.HitWallEvent;
import robocode.RobotDeathEvent;
import robocode.RoundEndedEvent;
import robocode.ScannedRobotEvent;
import robocode.StatusEvent;
import robocode.WinEvent;

public abstract class SaveComponent implements IFileEvents, IBattleEvents {

	protected abstract Serializable onNew();

	protected abstract Serializable onLoad(Serializable obj);

	private Serializable saveObj;

	public Object getSaveObject() {
		return saveObj;
	}

	@Override
	public void onInputStream(ObjectInputStream ois) throws IOException {
		try {
			Serializable obj = (Serializable) ois.readObject();
			this.saveObj = obj;
			this.saveObj = onLoad(this.saveObj);
		} catch (Exception e) {
			e.printStackTrace();
			Serializable obj = onNew();
			this.saveObj = obj;
		}

		Map<String, Object> pool = new HashMap<>();
		if (this.saveObj instanceof ISave) {
			ISave save = (ISave) this.saveObj;
			save.onRegister(pool);
			save.onFindRef(pool);
		}
	}

	@Override
	public void onOutputStream(ObjectOutputStream oos) throws IOException {
		oos.writeObject(saveObj);
	}

	@Override
	public void onRoundStarted() {
		if (this.getSaveObject() == null) {
			Serializable obj = onNew();
			this.saveObj = obj;
			Map<String, Object> pool = new HashMap<>();
			if (this.saveObj instanceof ISave) {
				ISave save = (ISave) this.saveObj;
				save.onRegister(pool);
				save.onFindRef(pool);
			}
		}
	}

	@Override
	public void onRoundEnded(RoundEndedEvent arg0) {
		// TODO Auto-generated method stub

	}

	@Override
	public void onBattleEnded(BattleEndedEvent arg0) {
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
