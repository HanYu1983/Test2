package han;

import static java.awt.event.KeyEvent.VK_1;
import static java.awt.event.KeyEvent.VK_2;
import static java.awt.event.KeyEvent.VK_3;
import static java.awt.event.KeyEvent.VK_4;

import java.awt.event.KeyEvent;
import java.awt.event.MouseEvent;
import java.awt.event.MouseWheelEvent;

import han.component.ComponentRobot;
import han.component.ITick;
import han.component.MemoryTargetPosition;
import han.component.RadarMovement;
import han.component.SpinMove;
import robocode.robotinterfaces.IInteractiveEvents;

public class Test4 extends ComponentRobot {
	private MemoryTargetPosition memory = new MemoryTargetPosition(this);

	{
		coms.addComponent(memory);
		coms.addComponent(new RadarMovement(this));
		coms.addComponent(new SpinMove(this));
		coms.addComponent(new Control());
	}

	private class Control implements IInteractiveEvents, ITick {
		private void syncGun() {
			double heading = memory.getBestHeading(robocode.Rules.getBulletSpeed(1.5));
			double oa = robocode.util.Utils.normalRelativeAngle(heading - Test4.this.getGunHeadingRadians());
			Test4.this.setTurnGunRightRadians(oa);
			if (Math.abs(oa) < Math.PI / 10) {
				if (getGunHeat() == 0) {
					fire(Math.min(3 - (Math.abs(oa) * 180 / Math.PI), getEnergy() - .1));
				}
			}
		}

		@Override
		public void onTick() {
			this.syncGun();
		}

		@Override
		public void onKeyPressed(KeyEvent e) {
			switch (e.getKeyCode()) {
			case VK_1:
				Test4.this.setAhead(10);
				break;
			case VK_2:
				Test4.this.setTurnRadarLeft(360);
				break;
			case VK_3:
				Test4.this.setTurnLeft(10);
				break;
			case VK_4:
				Test4.this.fire(4);
				break;
			}
		}

		@Override
		public void onKeyReleased(KeyEvent arg0) {
			// TODO Auto-generated method stub

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
