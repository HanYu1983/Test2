package han.component;

import java.util.Map;

public interface ISave {
	public void onRegister(Map<String, Object> pool);
	public void onFindRef(Map<String, Object> pool);
}
