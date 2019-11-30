package han.component.action;

import java.util.LinkedList;
import java.util.List;

import han.component.Components;

public class ActionQueue extends Components implements IAction {

	private final List<IAction> actions = new LinkedList<>();
	private final boolean isLoop;

	public ActionQueue(boolean isLoop) {
		this.isLoop = isLoop;
	}

	public void addAction(IAction action) {
		if (actions.size() == 0) {
			super.addComponent(action);
		}
		actions.add(action);
	}

	public void setAction(IAction action) {
		super.getComponents().clear();
		actions.clear();
		addAction(action);
	}

	@Override
	public void onTick() {
		super.onTick();
		if (actions.isEmpty()) {
			return;
		}
		IAction first = actions.get(0);
		if (first.isSuccess()) {
			super.removeComponent(first);
			actions.remove(first);

			if (actions.size() > 0) {
				super.addComponent(actions.get(0));
			}

			if (isLoop) {
				addAction(first);
			}
			return;
		}
	}

	@Override
	public boolean isSuccess() {
		return actions.isEmpty();
	}
}
