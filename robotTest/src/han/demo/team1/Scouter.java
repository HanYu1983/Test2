package han.demo.team1;

import han.component.ComponentRobot;
import han.component.ITick;
import han.component.JustScan;
import han.component.MemoryTargetPosition;
import han.component.RandomForwardMove;

public class Scouter extends ComponentRobot {
	private final MemoryTargetPosition memory = new MemoryTargetPosition(this);
	private final JustScan justScan = new JustScan(this);
	private final RandomForwardMove randomForwardMove = new RandomForwardMove(this);
	
	{
		coms.addComponent(memory);
		coms.addComponent(justScan);
		coms.addComponent(randomForwardMove);
		coms.addComponent(new Control());
	}

	private class Control implements ITick{
		@Override
		public void onTick() {
			
			
			/*
			try {
				Scouter.this.broadcastMessage(new Point(enemyX, enemyY));
			} catch (IOException ex) {
				out.println("Unable to send order: ");
				ex.printStackTrace(out);
			}*/
		}
	}
	
}
