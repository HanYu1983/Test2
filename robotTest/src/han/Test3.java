package han;

import java.awt.event.KeyEvent;
import java.io.Serializable;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

import org.jbox2d.common.MathUtils;
import org.jbox2d.common.Vec2;

import han.component.ComponentList;
import han.component.ComponentRobot;
import han.component.DrawCoord;
import han.component.JustScan;
import han.component.MemoryTargetPosition;
import han.component.MemoryTargetPosition.MemoryPoint;
import han.component.SaveComponent;
import han.component.action.ActionStack;
import han.component.action.MoveTo;

public class Test3 extends ComponentRobot {
	{
		coms.addComponent(new Control());
	}

	private class Control extends SaveComponent implements Serializable {
		private static final long serialVersionUID = 4298473476186474647L;

		@Override
		protected Serializable onNew() {
			System.out.println("onNew");
			Main main = new Main(null);
			main.robot = Test3.this;
			Test3.this.coms.addComponent(main);
			return main;
		}

		@Override
		protected Serializable onLoad(Serializable obj) {
			System.out.println("onLoad");
			// return this.onNew();

			Main main = (Main) obj;
			main.robot = Test3.this;
			Test3.this.coms.addComponent(main);
			return obj;
		}
	}

	public static class Main extends ComponentList {
		private static final long serialVersionUID = -791284919112248791L;
		private transient ComponentRobot robot;
		private transient final String robotKey = "robot";
		private MemoryTargetPosition memory;
		private ActionStack actionStack;

		public Main(String ignore) {
			super(null);
			memory = new MemoryTargetPosition(robotKey);
			actionStack = new ActionStack(null);
			this.addComponent(actionStack);
			this.addComponent(memory);
			this.addComponent(new DrawCoord(robotKey));
			this.addComponent(new JustScan(robotKey, null));
		}

		@Override
		public void onRegister(Map<String, Object> pool) {
			super.onRegister(pool);
			pool.put(robotKey, robot);
		}

		@Override
		public void onKeyReleased(KeyEvent arg0) {
			super.onKeyReleased(arg0);
			switch (arg0.getKeyCode()) {
			case KeyEvent.VK_G: {
				double dw = this.robot.getBattleFieldWidth() / 10;
				double dh = this.robot.getBattleFieldHeight() / 10;
				List<Vec2> points = new LinkedList<>();
				for (double x = dw; x < this.robot.getBattleFieldWidth() - dw; x += dw) {
					for (double y = dh; y < this.robot.getBattleFieldHeight() - dh; y += dh) {
						points.add(new Vec2((float) x, (float) y));
					}
				}

				List<Vec2> enemyPoints = new LinkedList<>();
				for (String name : memory.getRobotNames()) {
					for (MemoryPoint mp : memory.getHistory(name)) {
						enemyPoints.add(mp.point);
					}
				}

				List<Double> allDis = new LinkedList<>();
				for (Vec2 left : points) {
					double dist = 0;
					for (Vec2 right : enemyPoints) {
						dist += MathUtils.distanceSquared(left, right);
					}
					allDis.add(dist);
				}

				double max = 0;
				int maxId = -1;
				for (int i = 0; i < allDis.size(); ++i) {
					if (max < allDis.get(i)) {
						max = allDis.get(i);
						maxId = i;
					}
				}

				actionStack.addAction(new MoveTo(robotKey, robot, points.get(maxId)));

			}
				break;
			}
		}
	}
}
