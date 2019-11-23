package han.component;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;

public interface IFileEvents {
	void onInputStream(ObjectInputStream ois) throws IOException;

	void onOutputStream(ObjectOutputStream oos) throws IOException;
}
