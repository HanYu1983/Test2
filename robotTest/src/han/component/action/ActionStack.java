package han.component.action;

import java.io.Serializable;
import java.util.Map;
import java.util.Stack;

import han.component.ISave;
import han.component.ITick;

public class ActionStack implements IAction, ITick, Serializable, ISave {
	/**
	 * 
	 */
	private static final long serialVersionUID = 323656336719383505L;
	private Stack<IAction> actions = new Stack<>();

	public ActionStack() {

	}

	public void addAction(IAction action) {
		actions.add(action);
	}

	@Override
	public void onTick() {
		if (actions.isEmpty()) {
			return;
		}
		IAction first = actions.peek();
		if (first.isSuccess()) {
			actions.pop();
			return;
		}
		if (first instanceof ITick) {
			((ITick) first).onTick();
		}
	}

	@Override
	public boolean isSuccess() {
		return actions.isEmpty();
	}

	@Override
	public void onFindRef(Map<String, Object> pool) {
		for (Object save : actions) {
			if (save instanceof ISave) {
				((ISave) save).onFindRef(pool);
			}
		}
	}

	@Override
	public void onRegister(Map<String, Object> pool) {
		
	}
}
