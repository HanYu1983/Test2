package han.ai;

import java.util.HashMap;
import java.util.Map;
import java.util.Random;

public class QLearning<State, Action> {
	public static float DiscountRate = 0;
	public static float LearningRate = 0.05f;

	public final Map<State, Map<Action, Float>> qtable = new HashMap<>();

	private State lastState;
	private Action lastAction;
	private float reward;
	private final Random rand = new Random();

	public State getLastState() {
		return lastState;
	}

	public Action getLastAction() {
		return lastAction;
	}

	public void reinforce(float v) {
		reward += v;
	}

	public void LogInfo(State state) {
		System.out.println("[QLearning]========");
		if (qtable.containsKey(state) == false) {
			System.out.println("[QLearning] state not found");
			return;
		}
		for (Action action : qtable.get(state).keySet()) {
			float curr = qtable.get(state).get(action);
			System.out.println("[QLearning]" + action + ":" + curr);
		}
		System.out.println("[QLearning]========");
	}

	public Action selectAction(State state, float mutateRate) {
		if (qtable.containsKey(state) == false) {
			System.out.println("state not found.");
			return null;
		}

		if (rand.nextFloat() < mutateRate) {
			System.out.println("mutate!");
			return null;
		}

		float left = Float.MIN_VALUE;
		for (Action action : qtable.get(state).keySet()) {
			float right = qtable.get(state).get(action);
			left = Math.max(left, right);
		}
		float max = left;
		if (left < 0) {
			System.out.println("not good action:" + max);
			return null;
		}

		left = Float.MAX_VALUE;
		for (Action action : qtable.get(state).keySet()) {
			float right = qtable.get(state).get(action);
			left = Math.min(left, right);
		}
		float min = left;

		float total = 0;
		for (Action action : qtable.get(state).keySet()) {
			float curr = qtable.get(state).get(action);
			float width = (curr - min) + 1;
			total += width;
		}

		float index = rand.nextFloat() * total;
		float acc = 0;
		for (Action action : qtable.get(state).keySet()) {
			float curr = qtable.get(state).get(action);
			float width = (curr - min) + 1;

			float start = acc;
			float end = start + width;
			if (start <= index && end > index) {
				return action;
			}
			acc = end;
		}
		return null;
	}

	public void setQ(State state, Action action, float q) {
		if (qtable.containsKey(state) == false) {
			Map<Action, Float> at = new HashMap<>();
			at.put(action, q);
			qtable.put(state, at);
			return;
		}
		if (qtable.get(state).containsKey(action) == false) {
			qtable.get(state).put(action, q);
			return;
		}
		qtable.get(state).put(action, q);
	}

	public void learn(State state, Action action) {
		System.out.println("[QLearning]reward:" + reward);
		if (lastState == null || lastAction == null) {
			System.out.println("[QLearning] lastState or lastAction is null. no learn anything!");
			lastState = state;
			lastAction = action;
			return;
		}

		if (qtable.containsKey(lastState) == false) {
			System.out.println("[QLearning] state not found.");
			Map<Action, Float> at = new HashMap<>();
			at.put(lastAction, reward);
			qtable.put(lastState, at);
			return;
		}
		if (qtable.get(lastState).containsKey(lastAction) == false) {
			System.out.println("[QLearning] action not found");
			qtable.get(lastState).put(lastAction, reward);
			return;
		}

		float left = 0;
		float max = 0;
		for (float right : qtable.get(state).values()) {
			max = Math.max(left, right);
			left = right;
		}

		float newQ = reward + DiscountRate * max;
		float oldQ = qtable.containsKey(lastState) == false ? 0f
				: qtable.get(lastState).containsKey(lastAction) == false ? 0f : qtable.get(lastState).get(lastAction);
		float nextQ = (1f - LearningRate) * oldQ + LearningRate * newQ;
		qtable.get(lastState).put(lastAction, nextQ);

		reward = 0;
		lastState = state;
		lastAction = action;
	}
}
