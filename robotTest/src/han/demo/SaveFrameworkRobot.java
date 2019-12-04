package han.demo;

import java.io.Serializable;
import java.util.Map;

import org.jbox2d.common.Vec2;

import han.component.ComponentList;
import han.component.ComponentRobot;
import han.component.DrawCoord;
import han.component.SaveComponent;
import han.component.action.ActionQueue;
import han.component.action.ActionStack;
import han.component.action.AltAction;
import han.component.action.MoveTo;

public class SaveFrameworkRobot extends ComponentRobot {

	{
		coms.addComponent(new Control());
	}

	private class Control extends SaveComponent implements Serializable {
		/**
		 * 
		 */
		private static final long serialVersionUID = 4298473476186474647L;

		@Override
		protected Serializable onNew() {
			System.out.println("onNew");
			Main main = new Main(null);
			main.robot = SaveFrameworkRobot.this;
			SaveFrameworkRobot.this.coms.addComponent(main);
			return main;
		}

		@Override
		protected Serializable onLoad(Serializable obj) {
			System.out.println("onLoad");
			Main main = (Main) obj;
			main.robot = SaveFrameworkRobot.this;
			SaveFrameworkRobot.this.coms.addComponent(main);
			return obj;
		}
	}

	public static class Main extends ComponentList {
		private static final long serialVersionUID = -791284919112248791L;
		public transient ComponentRobot robot;
		private transient final String robotKey = "robot";

		public Main(String ignore) {
			super(null);
			ActionStack actionStack = new ActionStack(null);

			ActionQueue loopMove = new ActionQueue(true);
			loopMove.addAction(new MoveTo(robotKey, null, new Vec2(100, 100)));
			loopMove.addAction(new MoveTo(robotKey, null, new Vec2(100, 300)));
			loopMove.addAction(new MoveTo(robotKey, null, new Vec2(300, 300)));
			loopMove.addAction(new MoveTo(robotKey, null, new Vec2(300, 100)));

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
