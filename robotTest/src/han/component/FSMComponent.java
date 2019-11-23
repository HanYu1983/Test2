package han.component;

import java.util.HashMap;
import java.util.Map;

import robocode.robotinterfaces.IBasicEvents;
import robocode.robotinterfaces.IPaintEvents;

public class FSMComponent<State> extends Components implements IBasicEvents, IPaintEvents {
	private Map<State, Object> config = new HashMap<>();
	private State state;

	public Map<State, Object> getConfig() {
		return config;
	}

	public void changeState(State state) {
		State lastState = this.state;
		super.removeComponent(config.get(lastState));
		super.addComponent(config.get(state));
		this.state = state;
	}
}
