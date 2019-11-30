package han.demo.save;

import java.io.IOException;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import java.io.Serializable;
import java.util.HashMap;
import java.util.Map;

import han.component.ComponentRobot;
import han.component.IFileEvents;

public class SaveRobot extends ComponentRobot {

	{
		coms.addComponent(new Control());
	}

	private class Control implements IFileEvents {
		private World world;

		@Override
		public void onInputStream(ObjectInputStream ois) throws IOException {
			try {
				World obj = (World) ois.readObject();
				Map<String, Object> pool = new HashMap<>();
				obj.onRegister(pool);
				obj.onFindRef(pool);

				this.world = obj;
			} catch (Exception e) {
				e.printStackTrace();

				System.out.println("create new world");
				World obj = new World("world");
				this.world = obj;
			}
		}

		@Override
		public void onOutputStream(ObjectOutputStream oos) throws IOException {
			oos.writeObject(world);
		}
	}
}
