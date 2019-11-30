package han.component.action;

import han.component.ITick;

public class DelayAction implements IAction, ITick {
	private int tick;
	private final int maxTick;

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
