package han.component;

import java.io.Serializable;
import java.util.LinkedList;
import java.util.List;
import java.util.Stack;

public class ComponentStack extends ComponentsBasicAdapter implements Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = -2074253156934974600L;
	private Stack<Serializable> coms;

	@SuppressWarnings("unused")
	protected ComponentStack() {
		System.out.println("ComponentStack");
	}

	public ComponentStack(Object ignore) {
		coms = new Stack<>();
	}

	@Override
	public void addComponent(Serializable obj) {
		coms.add(obj);
	}

	@Override
	public void clearComponent() {
		coms.clear();
	}

	@Override
	public void removeComponent(Object obj) {
		coms.remove(obj);
	}

	public Stack<Serializable> getStack() {
		return coms;
	}

	@Override
	public Iterable<Object> getUpdateComponents() {
		if (coms.isEmpty()) {
			return new LinkedList<Object>();
		}
		List<Object> ret = new LinkedList<>();
		ret.add(coms.peek());
		return ret;
	}

	@Override
	public Iterable<Serializable> getContainer() {
		return coms;
	}
}
