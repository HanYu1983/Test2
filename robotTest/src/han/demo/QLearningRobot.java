package han.demo;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.Serializable;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.Random;

import han.ai.QLearning;
import han.component.ComponentRobot;
import han.component.ComponentList;
import han.component.FSMComponent;
import han.component.IBattleEvents;
import han.component.IFileEvents;
import han.component.ITick;
import han.component.JustScan;
import han.component.MemoryTargetPosition;
import han.component.RamFireControl;
import han.component.RamMovement;
import han.component.RandomForwardMove;
import han.component.SimpleFireControl;
import han.component.SpinMove;
import robocode.BattleEndedEvent;
import robocode.Bullet;
import robocode.BulletHitBulletEvent;
import robocode.BulletHitEvent;
import robocode.BulletMissedEvent;
import robocode.DeathEvent;
import robocode.HitByBulletEvent;
import robocode.HitRobotEvent;
import robocode.HitWallEvent;
import robocode.RobotDeathEvent;
import robocode.RoundEndedEvent;
import robocode.Rules;
import robocode.ScannedRobotEvent;
import robocode.StatusEvent;
import robocode.WinEvent;
import robocode.robotinterfaces.IBasicEvents;

public class QLearningRobot extends ComponentRobot {
	MemoryTargetPosition memory = new MemoryTargetPosition(this);
	// Object radarMovement = new RadarMovement(this);
	Serializable radarMovement = new JustScan(null, this);
	SimpleFireControl simpleFireControl = new SimpleFireControl("", this, memory);
	Serializable ramFireControl = new RamFireControl(this);
	Serializable ramMovement = new RamMovement(this);
	Serializable spinMove = new SpinMove(this);
	Serializable randomFowardMove = new RandomForwardMove(this);

	{
		coms.addComponent(new Control());
	}

	private enum FireStrategy {
		Simple, Ram
	}

	private enum MoveStrategy {
		Spin, Ram, RandomForward
	}

	private static class Action {
		public final FireStrategy fireStrategy;
		public final MoveStrategy moveStrategy;

		public Action(FireStrategy f, MoveStrategy m) {
			this.fireStrategy = f;
			this.moveStrategy = m;
		}

		public String toString() {
			return fireStrategy + "/" + moveStrategy;
		}

		private static Map<String, Action> pool = new HashMap<>();

		public static Action get(FireStrategy f, MoveStrategy m) {
			String key = f + "_" + m;
			if (pool.containsKey(key) == false) {
				Action action = new Action(f, m);
				pool.put(key, action);
				return action;
			}
			return pool.get(key);
		}

		static {
			for (FireStrategy fs : FireStrategy.values()) {
				for (MoveStrategy ms : MoveStrategy.values()) {
					get(fs, ms);
				}
			}
		}

		public static Action random() {
			Action[] actions = pool.values().toArray(new Action[] {});
			return actions[new Random().nextInt(actions.length)];
		}

		/*
		 * @Override public int hashCode() { final int prime = 31; int result = 1;
		 * result = prime * result + fireStrategy; result = prime * result +
		 * moveStrategy; return result; }
		 * 
		 * @Override public boolean equals(Object obj) { if (this == obj) return true;
		 * if (obj == null) return false; if (getClass() != obj.getClass()) return
		 * false; Action other = (Action) obj; return fireStrategy == other.fireStrategy
		 * && moveStrategy == other.moveStrategy; }
		 */
	}

	private class Control extends ComponentList implements IBattleEvents, IFileEvents {
		/**
		 * 
		 */
		private static final long serialVersionUID = 156257709440961271L;
		private final List<Bullet> bullets = QLearningRobot.this.simpleFireControl.getBullets();
		private float MutateRate = 0.2f;

		public Control() {
			super(null);
		}

		@Override
		public void onInputStream(ObjectInputStream ois) throws IOException {
			String version = ois.readUTF();
			System.out.println("version:" + version);
			int size = ois.readByte();
			for (int i = 0; i < size; ++i) {
				int fs = ois.readByte();
				int ms = ois.readByte();
				float value = ois.readFloat();
				Action action = Action.get(FireStrategy.values()[fs], MoveStrategy.values()[ms]);
				qlearn.setQ(0, action, value);
				System.out.println(action + ":" + value);
			}
			super.onInputStream(ois);
		}

		@Override
		public void onOutputStream(ObjectOutputStream oos) throws IOException {
			oos.writeUTF("0.0.2");
			int size = qlearn.qtable.get(0).keySet().size();
			oos.writeByte(size);
			for (Action action : qlearn.qtable.get(0).keySet()) {
				oos.writeByte(action.fireStrategy.ordinal());
				oos.writeByte(action.moveStrategy.ordinal());
				oos.writeFloat(qlearn.qtable.get(0).get(action));
			}
			super.onOutputStream(oos);
		}

		@Override
		public void onTick() {
			super.tick();
			this.tickQ();
		}

		@Override
		public void onRoundStarted() {
			super.onRoundStarted();
			MutateRate = 0.05f;
			this.selectAction();
		}

		@Override
		public void onRoundEnded(RoundEndedEvent arg0) {
			super.onRoundEnded(arg0);
			// boolean isWin = Test5.this.getEnergy() > 0;
			// if (isWin) {
			// qlearn.reinforce(3000);
			// } else {
			// qlearn.reinforce(-5000);
			// }
			qlearn.learn(0, Action.random());
			qlearn.LogInfo(0);
		}

		@Override
		public void onBattleEnded(BattleEndedEvent arg0) {
			super.onBattleEnded(arg0);
		}

		void onAction(Action action) {
			setComponentFor(action);
		}

		private QLearning<Integer, Action> qlearn = new QLearning<>();
		private RewardDetector rewardDetector = new RewardDetector(qlearn);
		private final int LearningTick = 30 * 5;
		private int currTick;
		{
			coms.addComponent(rewardDetector);
		}

		private void tickQ() {
			if (currTick >= LearningTick) {
				this.selectAction();
				currTick = 0;
				return;
			}
			++currTick;
		}

		private void selectAction() {
			Action action = qlearn.selectAction(0, MutateRate);
			if (action == null) {
				action = Action.random();
				System.out.println("mutate action:" + action);
			} else {
				System.out.println("best action:" + action);
			}
			onAction(action);
			qlearn.learn(0, action);
			bullets.clear();
		}

		private FSMComponent<FireStrategy> fireStrategy = new FSMComponent<>(null);
		private FSMComponent<MoveStrategy> moveStrategy = new FSMComponent<>(null);

		{
			ComponentList coms = new ComponentList(null);
			coms.addComponent(memory);
			coms.addComponent(radarMovement);
			coms.addComponent(simpleFireControl);
			fireStrategy.getConfig().put(FireStrategy.Simple, coms);

			coms = new ComponentList(null);
			coms.addComponent(memory);
			coms.addComponent(radarMovement);
			coms.addComponent(ramFireControl);
			fireStrategy.getConfig().put(FireStrategy.Ram, coms);

			this.addComponent(fireStrategy);
			fireStrategy.changeState(FireStrategy.Ram);
		}

		{
			ComponentList coms = new ComponentList(null);
			coms.addComponent(spinMove);

			moveStrategy.getConfig().put(MoveStrategy.Spin, coms);
			moveStrategy.getConfig().put(MoveStrategy.Ram, ramMovement);
			moveStrategy.getConfig().put(MoveStrategy.RandomForward, randomFowardMove);

			this.addComponent(moveStrategy);
			moveStrategy.changeState(MoveStrategy.Ram);
		}

		private void setComponentFor(Action action) {
			fireStrategy.changeState(action.fireStrategy);
			moveStrategy.changeState(action.moveStrategy);
		}
	}

	private class RewardDetector implements IBasicEvents, ITick, Serializable {
		/**
		 * 
		 */
		private static final long serialVersionUID = -952446248046703667L;
		private final QLearning<Integer, Action> qlearn;
		private final List<Bullet> bullets = QLearningRobot.this.simpleFireControl.getBullets();

		public RewardDetector(QLearning<Integer, Action> q) {
			qlearn = q;
		}

		@Override
		public void onTick() {
			qlearn.reinforce(1);
		}

		@Override
		public void onBulletHit(BulletHitEvent arg0) {
			if (bullets.contains(arg0.getBullet()) == false) {
				return;
			}
			qlearn.reinforce((float) (arg0.getBullet().getPower() * 100.0f / Rules.MAX_BULLET_POWER));
		}

		@Override
		public void onBulletHitBullet(BulletHitBulletEvent arg0) {
			if (bullets.contains(arg0.getBullet()) == false) {
				return;
			}
			// qlearn.reinforce(1);
		}

		@Override
		public void onBulletMissed(BulletMissedEvent arg0) {
			if (bullets.contains(arg0.getBullet()) == false) {
				return;
			}
			qlearn.reinforce((float) (arg0.getBullet().getPower() * -10.0f / Rules.MAX_BULLET_POWER));
		}

		@Override
		public void onDeath(DeathEvent arg0) {
			// qlearn.reinforce(-5000);
		}

		@Override
		public void onHitByBullet(HitByBulletEvent arg0) {
			qlearn.reinforce((float) (arg0.getBullet().getPower() * -100.0f / Rules.MAX_BULLET_POWER));
		}

		@Override
		public void onHitRobot(HitRobotEvent arg0) {

		}

		@Override
		public void onHitWall(HitWallEvent arg0) {

		}

		@Override
		public void onRobotDeath(RobotDeathEvent arg0) {
			// qlearn.reinforce(200);
		}

		@Override
		public void onScannedRobot(ScannedRobotEvent arg0) {

		}

		@Override
		public void onStatus(StatusEvent arg0) {

		}

		@Override
		public void onWin(WinEvent arg0) {

		}

	}
}
