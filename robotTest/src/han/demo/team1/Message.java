package han.demo.team1;

import java.io.Serializable;

import org.jbox2d.common.Vec2;

public class Message implements Serializable {
	public enum Order {
		Pending, AttackIt
	}

	public enum Report {
		Pending, OpponentPosition, State
	}

	public enum State {
		Pending, Idle
	}

	private static final long serialVersionUID = 6480589243385664285L;
	public Order order;
	public Report report;
	public String robot;
	public Vec2 vec1 = new Vec2();
	public State state1;
}
