package han.component;

import java.awt.Color;
import java.awt.Graphics2D;

import robocode.robotinterfaces.IPaintEvents;

public class DrawCoord implements IPaintEvents {
	private final ComponentRobot robot;

	public DrawCoord(ComponentRobot robot) {
		this.robot = robot;
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
}
