package han.component;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.Serializable;
import java.util.HashMap;
import java.util.Map;

public abstract class SaveComponent implements IFileEvents {

	protected abstract Serializable onNew();

	protected abstract void onLoad(Object obj);

	private Serializable saveObj;

	public Object getSaveObject() {
		return saveObj;
	}

	@Override
	public void onInputStream(ObjectInputStream ois) throws IOException {
		try {
			Serializable obj = (Serializable) ois.readObject();
			this.saveObj = obj;
			onLoad(this.saveObj);
		} catch (Exception e) {
			// e.printStackTrace();
			System.out.println("create new world");
			Serializable obj = onNew();
			this.saveObj = obj;
		}

		Map<String, Object> pool = new HashMap<>();
		if (this.saveObj instanceof ISave) {
			ISave save = (ISave) this.saveObj;
			save.onRegister(pool);
			save.onFindRef(pool);
		}
	}

	@Override
	public void onOutputStream(ObjectOutputStream oos) throws IOException {
		oos.writeObject(saveObj);
	}
}
