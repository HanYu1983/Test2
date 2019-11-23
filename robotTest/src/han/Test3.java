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
import han.component.RadarMovement;
import han.component.RadarTargeting;
import han.component.SpinMove;
import robocode.robotinterfaces.IInteractiveEvents;

public class Test3 extends ComponentRobot {
	private RadarTargeting radarSystem = new RadarTargeting(this);

	{
		coms.addComponent(radarSystem);
		coms.addComponent(new SpinMove(this));
		coms.addComponent(new Control());
	}

	private class Control implements IInteractiveEvents, ITick {
		private void syncGun() {
			if (radarSystem.movement.getState() != RadarMovement.State.targeting) {
				return;
			}
			double heading = radarSystem.memory.getBestHeading(robocode.Rules.getBulletSpeed(1));
			double oa = robocode.util.Utils.normalRelativeAngle(heading - Test3.this.getGunHeadingRadians());
			Test3.this.setTurnGunRightRadians(oa);
			if (Math.abs(oa) < Math.PI / 10) {
				fire(1);
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
				Test3.this.setAhead(10);
				break;
			case VK_2:
				Test3.this.setTurnRadarLeft(360);
				break;
			case VK_3:
				Test3.this.setTurnLeft(10);
				break;
			case VK_4:
				Test3.this.fire(4);
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
