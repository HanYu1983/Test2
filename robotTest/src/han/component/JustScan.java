package han.component;

import robocode.AdvancedRobot;

public class JustScan implements ITick {
	private final AdvancedRobot robot;

	public JustScan(AdvancedRobot robot) {
		this.robot = robot;
	}
	@Override
	public void onTick() {
		robot.setTurnRadarRightRadians(Math.PI);
	}
}
