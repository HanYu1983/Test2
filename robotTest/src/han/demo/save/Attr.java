package han.demo.save;

import java.io.Serializable;
import java.util.Map;

public class Attr implements Serializable {
	private static final long serialVersionUID = -2063436782773121346L;
	private String worldKey;
	public transient World world;

	public Attr() {

	}

	public Attr(String worldKey) {
		this.worldKey = worldKey;
	}

	public void onRegister(Map<String, Object> pool) {

	}

	public void onFindRef(Map<String, Object> pool) {
		world = (World) pool.get(worldKey);
	}
}