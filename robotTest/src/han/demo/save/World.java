package han.demo.save;

import java.io.Serializable;
import java.util.Map;

public class World implements Serializable {
	private static final long serialVersionUID = -5401905740679935669L;
	Attr move;

	private String id;

	@SuppressWarnings("unused")
	private World() {
		
	}

	public World(String id) {
		this.id = id;
		move = new Attr(id);
	}

	public void onRegister(Map<String, Object> pool) {
		pool.put(id, this);
		move.onRegister(pool);
	}

	public void onFindRef(Map<String, Object> pool) {
		move.onFindRef(pool);
		System.out.println(move.world);
	}
}