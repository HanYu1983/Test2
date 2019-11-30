package han.component.action;

import java.io.Serializable;

import han.component.ComponentQueue;
import han.component.ITick;

public class ActionQueue extends ComponentQueue implements IAction {
	/**
	 * 
	 */
	private static final long serialVersionUID = -2074253156934974600L;
	private boolean isLoop;

	@SuppressWarnings("unused")
	protected ActionQueue() {

	}

	public ActionQueue(boolean isLoop) {
		super(null);
		this.isLoop = isLoop;
	}

	public void addAction(IAction action) {
		super.addComponent((Serializable) action);
	}

	@Override
	public void addComponent(Serializable obj) {
		throw new UnsupportedOperationException();
	}

	public void setAction(IAction action) {
		super.clearComponent();
		addAction(action);
	}

	@Override
	public void onTick() {
		if (isSuccess()) {
			return;
		}
		IAction first = (IAction) getQueue().get(0);
		if (first.isSuccess()) {
			super.removeComponent(first);
			if (isLoop) {
				addAction(first);
			}
			return;
		}
		if (first instanceof ITick) {
			((ITick) first).onTick();
		}
	}

	@Override
	public boolean isSuccess() {
		return getQueue().isEmpty();
	}
}
