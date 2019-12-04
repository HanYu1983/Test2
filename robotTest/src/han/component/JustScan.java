package han.component;

import java.io.Serializable;
import java.util.Map;

import robocode.AdvancedRobot;

public class JustScan implements ITick, Serializable, ISave {
	private static final long serialVersionUID = 755165207518324479L;
	private transient AdvancedRobot robot;
	private String robotKey;

	public JustScan(String key, AdvancedRobot robot) {
		robotKey = key;
		this.robot = robot;
	}

	@Override
	public void onTick() {
		robot.setTurnRadarRightRadians(Math.PI);
	}

	@Override
	public void onRegister(Map<String, Object> pool) {

	}

	@Override
	public void onFindRef(Map<String, Object> pool) {
		robot = (AdvancedRobot) pool.get(robotKey);
	}
}
