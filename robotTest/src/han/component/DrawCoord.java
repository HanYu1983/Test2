package han.component;

import java.awt.Color;
import java.awt.Graphics2D;
import java.io.Serializable;
import java.util.Map;

import robocode.robotinterfaces.IPaintEvents;

public class DrawCoord implements IPaintEvents, Serializable, ISave {
	/**
	 * 
	 */
	private static final long serialVersionUID = -2922739108672535690L;
	private String robotKey;
	private transient ComponentRobot robot;

	@SuppressWarnings("unused")
	private DrawCoord() {

	}

	public DrawCoord(String robotKey) {
		this.robotKey = robotKey;
	}

	@Override
	public void onPaint(Graphics2D g) {
		g.setColor(Color.red);
		for (int x = 0; x < robot.getBattleFieldWidth(); x += 50) {
			g.drawLine(x, 0, x, (int) robot.getBattleFieldHeight());
			g.drawString("x:" + x, x, 20);
		}

		for (int y = 0; y < robot.getBattleFieldHeight(); y += 50) {
			g.drawLine(0, y, (int) robot.getBattleFieldWidth(), y);
			g.drawString("y:" + y, 20, y);
		}
	}

	@Override
	public void onRegister(Map<String, Object> pool) {
		// TODO Auto-generated method stub

	}

	@Override
	public void onFindRef(Map<String, Object> pool) {
		robot = (ComponentRobot) pool.get(robotKey);
	}
}
