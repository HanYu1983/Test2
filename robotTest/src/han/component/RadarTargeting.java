package han.component;

import robocode.AdvancedRobot;

public class RadarTargeting extends Components {
	public final MemoryTargetPosition memory;
	public final RadarMovement movement;

	public RadarTargeting(AdvancedRobot robot) {
		memory = new MemoryTargetPosition(robot);
		movement = new RadarMovement(robot);
		this.addComponent(memory);
		this.addComponent(movement);
	}
}
