package han.demo;

import java.awt.event.KeyEvent;
import java.io.Serializable;
import java.util.Map;

import han.component.AntiGravityMove;
import han.component.ComponentList;
import han.component.ComponentRobot;
import han.component.DrawCoord;
import han.component.JustScan;
import han.component.MemoryTargetPosition;
import han.component.MemoryTargetPosition.MemoryPoint;
import han.component.SaveComponent;
import han.component.SimpleFireControl;

public class EvadeFireRobot extends ComponentRobot {
	{
		coms.addComponent(new Control());
	}

	private class Control extends SaveComponent implements Serializable {
		private static final long serialVersionUID = 4298473476186474647L;

		@Override
		protected Serializable onNew() {
			System.out.println("onNew");
			Main main = new Main(null);
			main.robot = EvadeFireRobot.this;
			EvadeFireRobot.this.coms.addComponent(main);
			return main;
		}

		@Override
		protected Serializable onLoad(Serializable obj) {
			System.out.println("onLoad");
			// return this.onNew();
			Main main = (Main) obj;
			main.robot = EvadeFireRobot.this;
			EvadeFireRobot.this.coms.addComponent(main);
			return obj;
		}
	}

	public static class Main extends ComponentList {
		private static final long serialVersionUID = -791284919112248791L;
		private transient ComponentRobot robot;
		private transient final String robotKey = "robot";
		private MemoryTargetPosition memory;
		private SimpleFireControl fireControl;
		private AntiGravityMove antiMove;

		public Main(String ignore) {
			super(null);
			memory = new MemoryTargetPosition(robotKey);
			fireControl = new SimpleFireControl(robotKey, null, memory);
			antiMove = new AntiGravityMove(robotKey, null);

			this.addComponent(memory);
			this.addComponent(fireControl);
			this.addComponent(antiMove);
			this.addComponent(new DrawCoord(robotKey));
			this.addComponent(new JustScan(robotKey, null));
		}

		@Override
		public void onTick() {
			super.onTick();
			antiMove.getGravityPoints().clear();
			for (String name : memory.getRobotNames()) {
				for (MemoryPoint mp : memory.getHistory(name)) {
					antiMove.getGravityPoints().add(new AntiGravityMove.Point(mp.point, 1));
				}
			}
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

			}
				break;
			}
		}
	}
}
