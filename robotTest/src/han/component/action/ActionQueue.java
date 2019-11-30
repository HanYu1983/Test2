package han.component.action;

import java.io.Serializable;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

import han.component.ISave;
import han.component.ITick;

public class ActionQueue implements IAction, ITick, Serializable, ISave {

	/**
	 * 
	 */
	private static final long serialVersionUID = -2074253156934974600L;
	private final List<IAction> actions = new LinkedList<>();
	private boolean isLoop;

	@SuppressWarnings("unused")
	private ActionQueue() {

	}

	public ActionQueue(boolean isLoop) {
		this.isLoop = isLoop;
	}

	public void addAction(IAction action) {
		actions.add(action);
	}

	public void setAction(IAction action) {
		actions.clear();
		addAction(action);
	}

	@Override
	public void onTick() {
		if (actions.isEmpty()) {
			return;
		}
		IAction first = actions.get(0);
		if (first.isSuccess()) {
			actions.remove(first);
			if (isLoop) {
				addAction(first);
			}
			return;
		}
		if(first instanceof ITick) {
			((ITick)first).onTick();
		}
	}

	@Override
	public boolean isSuccess() {
		return actions.isEmpty();
	}

	@Override
	public void onRegister(Map<String, Object> pool) {
		
	}

	@Override
	public void onFindRef(Map<String, Object> pool) {
		for(Object save : actions) {
			if(save instanceof ISave) {
				((ISave)save).onFindRef(pool);
			}
		}
	}
}
