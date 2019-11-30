package han.component.action;

import java.util.Stack;

import han.component.Components;

public class ActionStack  extends Components implements IAction{
	private Stack<IAction> actions = new Stack<>();
	private Object lastAction;
	
	public void addAction(IAction action) {
		if(lastAction != null) {
			super.removeComponent(lastAction);
		}
		actions.add(action);
		super.addComponent(action);
		lastAction = action;
	}
	
	@Override
	public void onTick() {
		super.onTick();
		if (actions.isEmpty()) {
			return;
		}
		IAction first = actions.peek();
		if (first.isSuccess()) {
			super.removeComponent(actions.pop());
			lastAction = null;
			
			if (actions.size() > 0) {
				super.addComponent(actions.peek());
				lastAction = actions.peek();
			}
			return;
		}
	}
	
	@Override
	public boolean isSuccess() {
		return actions.isEmpty();
	}
}
