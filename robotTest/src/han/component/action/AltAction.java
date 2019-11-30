package han.component.action;

import java.io.Serializable;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;

import han.component.ISave;
import han.component.ITick;

public class AltAction implements IAction, ITick, Serializable, ISave {
	/**
	 * 
	 */
	private static final long serialVersionUID = 938412223749529927L;
	private final List<IAction> actions = new LinkedList<>();

	public void addAction(IAction action) {
		actions.add(action);
	}

	@Override
	public boolean isSuccess() {
		for (IAction action : actions) {
			if (action.isSuccess()) {
				return true;
			}
		}
		return false;
	}

	@Override
	public void onRegister(Map<String, Object> pool) {
		// TODO Auto-generated method stub

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
	public void onTick() {
		for (Object save : actions) {
			if (save instanceof ITick) {
				((ITick) save).onTick();
			}
		}
	}
}
