package han.component;

import java.io.Serializable;
import java.util.HashMap;
import java.util.Map;

import robocode.robotinterfaces.IBasicEvents;
import robocode.robotinterfaces.IPaintEvents;

public class FSMComponent<State> extends ComponentList implements IBasicEvents, IPaintEvents, Serializable {

	/**
	 * 
	 */
	private static final long serialVersionUID = 7330756357632210649L;

	public FSMComponent(Object ignore) {
		super(ignore);
	}

	private Map<State, Serializable> config = new HashMap<>();
	private State state;

	public Map<State, Serializable> getConfig() {
		return config;
	}

	public void changeState(State state) {
		State lastState = this.state;
		super.removeComponent(config.get(lastState));
		super.addComponent(config.get(state));
		this.state = state;
	}
}
