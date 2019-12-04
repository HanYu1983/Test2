package han.component.action;

import java.io.Serializable;

import han.component.ComponentStack;
import han.component.ITick;

public class ActionStack extends ComponentStack implements IAction {
	/**
	 * 
	 */
	private static final long serialVersionUID = 323656336719383505L;

	public ActionStack(Object hook) {
		super(null);
	}
	
	public void addAction(IAction action) {
		super.addComponent((Serializable) action);
	}

	@Override
	public void addComponent(Serializable obj) {
		throw new UnsupportedOperationException();
	}

	@Override
	public void onTick() {
		if (isSuccess()) {
			return;
		}
		IAction first = (IAction) getStack().peek();
		if (first.isSuccess()) {
			getStack().pop();
			return;
		}
		if (first instanceof ITick) {
			((ITick) first).onTick();
		}
	}

	@Override
	public boolean isSuccess() {
		return getStack().isEmpty();
	}
}
