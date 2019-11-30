package han.component.action;

import java.io.Serializable;

import han.component.ComponentList;

public class AltAction extends ComponentList implements IAction {
	/**
	 * 
	 */
	private static final long serialVersionUID = 938412223749529927L;

	@SuppressWarnings("unused")
	protected AltAction() {

	}

	public AltAction(Object hook) {
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
	public boolean isSuccess() {
		for (Object obj : getList()) {
			if (((IAction) obj).isSuccess()) {
				return true;
			}
		}
		return false;
	}
}
