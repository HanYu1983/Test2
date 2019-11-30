package han;

import java.io.Serializable;
import java.util.Map;

import org.jbox2d.common.Vec2;

import han.component.ComponentRobot;
import han.component.Components;
import han.component.DrawCoord;
import han.component.SaveComponent;
import han.component.action.ActionQueue;
import han.component.action.ActionStack;
import han.component.action.AltAction;
import han.component.action.DelayAction;
import han.component.action.MoveTo;

public class Test4 extends ComponentRobot {

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
			Main main = new Main(null);
			main.robot = Test4.this;
			Test4.this.coms.addComponent(main);
			System.out.println("onNew");
			return main;
		}

		@Override
		protected void onLoad(Object obj) {
			System.out.println("onLoad");
			Main main = (Main) obj;
			main.robot = Test4.this;
			Test4.this.coms.addComponent(main);
		}
	}

	public static class Main extends Components {
		private static final long serialVersionUID = -791284919112248791L;
		public transient ComponentRobot robot;
		private transient final String robotKey = "robot";

		@SuppressWarnings("unused")
		private Main() {

		}

		public Main(String ignore) {
			ActionStack actionStack = new ActionStack();

			ActionQueue loopMove = new ActionQueue(true);
			loopMove.addAction(new MoveTo(robotKey, new Vec2(100, 100)));
			loopMove.addAction(new MoveTo(robotKey, new Vec2(100, 300)));
			loopMove.addAction(new MoveTo(robotKey, new Vec2(300, 300)));
			loopMove.addAction(new MoveTo(robotKey, new Vec2(300, 100)));

			AltAction alt = new AltAction();
			alt.addAction(loopMove);
			alt.addAction(new DelayAction(150));

			actionStack.addAction(alt);

			this.addComponent(actionStack);
			this.addComponent(new DrawCoord(robotKey));
		}

		@Override
		public void onRegister(Map<String, Object> pool) {
			pool.put(robotKey, robot);
		}

		@Override
		public void onFindRef(Map<String, Object> pool) {
			super.onFindRef(pool);
		}
	}
}
