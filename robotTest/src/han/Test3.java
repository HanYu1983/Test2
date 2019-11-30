package han;

import java.awt.event.KeyEvent;
import java.awt.event.MouseEvent;
import java.awt.event.MouseWheelEvent;

import org.jbox2d.common.Vec2;

import han.component.ComponentRobot;
import han.component.DrawCoord;
import han.component.action.ActionQueue;
import han.component.action.ActionStack;
import han.component.action.AltAction;
import han.component.action.DelayAction;
import han.component.action.MoveTo;
import robocode.robotinterfaces.IInteractiveEvents;

public class Test3 extends ComponentRobot {
	private ActionStack actionStack = new ActionStack();
	private MoveTo moveTo = new MoveTo(this);

	{
		ActionQueue loopMove = new ActionQueue(true);
		loopMove.addAction(new MoveTo(this, new Vec2(100, 100)));
		loopMove.addAction(new MoveTo(this, new Vec2(100, 300)));
		loopMove.addAction(new MoveTo(this, new Vec2(300, 300)));
		loopMove.addAction(new MoveTo(this, new Vec2(300, 100)));

		AltAction alt = new AltAction();
		alt.addAction(loopMove);
		alt.addAction(new DelayAction(150));

		actionStack.addAction(alt);

		coms.addComponent(new Control());
		coms.addComponent(actionStack);
		coms.addComponent(new DrawCoord(this));
	}

	private class Control implements IInteractiveEvents {
		private final ComponentRobot robot = Test3.this;

		@Override
		public void onKeyPressed(KeyEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onKeyReleased(KeyEvent arg0) {
			switch (arg0.getKeyCode()) {
			case KeyEvent.VK_F: {
				moveTo.position.set(50, 100);
				moveTo.targeting();
			}
				break;
			case KeyEvent.VK_G: {
				moveTo.position.set(100, 50);
				moveTo.targeting();
			}
				break;
			case KeyEvent.VK_J: {

			}
				break;
			}
		}

		@Override
		public void onKeyTyped(KeyEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onMouseClicked(MouseEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onMouseDragged(MouseEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onMouseEntered(MouseEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onMouseExited(MouseEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onMouseMoved(MouseEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onMousePressed(MouseEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onMouseReleased(MouseEvent arg0) {
			// TODO Auto-generated method stub

		}

		@Override
		public void onMouseWheelMoved(MouseWheelEvent arg0) {
			// TODO Auto-generated method stub

		}

	}
}
