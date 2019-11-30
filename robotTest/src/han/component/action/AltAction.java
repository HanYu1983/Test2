package han.component.action;

import java.util.LinkedList;
import java.util.List;

import han.component.Components;

public class AltAction extends Components implements IAction {
	private final List<IAction> actions = new LinkedList<>();

	public void addAction(IAction action) {
		super.addComponent(action);
		actions.add(action);
	}

	@Override
	public boolean isSuccess() {
		for(IAction action : actions) {
			if(action.isSuccess()) {
				return true;
			}
		}
		return false;
	}
}
