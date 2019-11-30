package han.component.action;

import java.io.Serializable;

import han.component.ITick;

public class DelayAction implements IAction, ITick, Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = -7420044068526225764L;
	private int tick;
	private int maxTick;
	
	@SuppressWarnings("unused")
	private DelayAction() {
		
	}

	public DelayAction(int t) {
		maxTick = t;
	}

	@Override
	public void onTick() {
		this.tick++;
	}

	@Override
	public boolean isSuccess() {
		return tick >= maxTick;
	}

}
