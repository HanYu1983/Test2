package han.component;

import java.io.Serializable;
import java.util.LinkedList;
import java.util.List;

public class ComponentQueue extends ComponentsBasicAdapter implements Serializable {
	/**
	 * 
	 */
	private static final long serialVersionUID = -2074253156934974600L;
	private List<Serializable> coms;

	@SuppressWarnings("unused")
	protected ComponentQueue() {
		System.out.println("ComponentQueue");
	}

	public ComponentQueue(Object ignore) {
		coms = new LinkedList<>();
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

	public List<Serializable> getQueue() {
		return coms;
	}

	@Override
	public Iterable<Object> getUpdateComponents() {
		if (coms.isEmpty()) {
			return new LinkedList<Object>();
		}
		List<Object> ret = new LinkedList<>();
		ret.add(coms.get(0));
		return ret;
	}

	@Override
	public Iterable<Serializable> getContainer() {
		return coms;
	}
}
