package han.demo;

import java.io.Serializable;
import java.util.HashMap;
import java.util.Map;

import org.jbox2d.common.Vec2;

import han.component.ComponentList;
import han.component.ComponentRobot;
import han.component.DrawCoord;
import han.component.IBattleEvents;
import han.component.SaveComponent;
import han.component.action.ActionQueue;
import han.component.action.ActionStack;
import han.component.action.AltAction;
import han.component.action.MoveTo;
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

public class SaveFrameworkRobot extends ComponentRobot {

	{
		coms.addComponent(new Control());
	}

	private class Control extends SaveComponent implements Serializable, IBattleEvents {
		/**
		 * 
		 */
		private static final long serialVersionUID = 4298473476186474647L;

		@Override
		protected Serializable onNew() {
			Main main = new Main(null);
			main.robot = SaveFrameworkRobot.this;
			SaveFrameworkRobot.this.coms.addComponent(main);
			System.out.println("onNew");
			return main;
		}

		@Override
		protected void onLoad(Object obj) {
			System.out.println("onLoad");
			Main main = (Main) obj;
			main.robot = SaveFrameworkRobot.this;
			SaveFrameworkRobot.this.coms.addComponent(main);
		}

		@Override
		public void onRoundStarted() {
			if (this.getSaveObject() == null) {
				Main main = new Main(null);
				main.robot = SaveFrameworkRobot.this;
				SaveFrameworkRobot.this.coms.addComponent(main);
				this.setSaveObjectManually(main);

				Map<String, Object> pool = new HashMap<>();
				main.onRegister(pool);
				main.onFindRef(pool);
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

	public static class Main extends ComponentList {
		private static final long serialVersionUID = -791284919112248791L;
		public transient ComponentRobot robot;
		private transient final String robotKey = "robot";

		@SuppressWarnings("unused")
		private Main() {
			System.out.println("Main");
		}

		public Main(String ignore) {
			super(null);
			ActionStack actionStack = new ActionStack(null);

			ActionQueue loopMove = new ActionQueue(true);
			loopMove.addAction(new MoveTo(robotKey, new Vec2(100, 100)));
			loopMove.addAction(new MoveTo(robotKey, new Vec2(100, 300)));
			loopMove.addAction(new MoveTo(robotKey, new Vec2(300, 300)));
			loopMove.addAction(new MoveTo(robotKey, new Vec2(300, 100)));

			AltAction alt = new AltAction(null);
			alt.addAction(loopMove);
			// 拿掉這個注解的話, 下一場坦克就不會動了, 因為DelayAction也被記錄了
			// alt.addAction(new DelayAction(150));

			actionStack.addAction(alt);

			this.addComponent(actionStack);
			this.addComponent(new DrawCoord(robotKey));
		}

		@Override
		public void onRegister(Map<String, Object> pool) {
			pool.put(robotKey, robot);
			super.onRegister(pool);
		}
	}
}
