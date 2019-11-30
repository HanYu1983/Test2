package han.component;

import java.io.Serializable;
import java.util.LinkedList;
import java.util.List;

public class ComponentList extends ComponentsBasicAdapter implements Serializable {
	private static final long serialVersionUID = 7373097744376595809L;
	private List<Serializable> coms;

	@SuppressWarnings("unused")
	protected ComponentList() {
		System.out.println("ComponentList");
	}

	public ComponentList(Object ignore) {
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

	@Override
	public Iterable<Object> getUpdateComponents() {
		return new LinkedList<Object>(coms);
	}

	public List<Serializable> getList() {
		return coms;
	}

	@Override
	public Iterable<Serializable> getContainer() {
		return coms;
	}
}
