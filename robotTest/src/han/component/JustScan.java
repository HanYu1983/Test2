package han.component;

import java.io.Serializable;

import robocode.AdvancedRobot;

public class JustScan implements ITick, Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = 755165207518324479L;
	private final AdvancedRobot robot;

	public JustScan(AdvancedRobot robot) {
		this.robot = robot;
	}
	@Override
	public void onTick() {
		robot.setTurnRadarRightRadians(Math.PI);
	}
}
