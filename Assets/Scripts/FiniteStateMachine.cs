using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FiniteStateMachine {
	
	public delegate bool CanEnter();
	public delegate void OnEnter();
	public delegate void OnExit();
	
	class StateTransition {
		
		readonly int currentState;
		readonly int command;
		
		public StateTransition(int _currentState, int _command) {
			currentState = _currentState;
			command = _command;
		}
		
		public override int GetHashCode() {
			return 17 + 31 * currentState.GetHashCode() + 31 * command.GetHashCode();
		}
		
		public override bool Equals(object obj) {
			StateTransition other = obj as StateTransition;
			return other != null && this.currentState == other.currentState && this.command == other.command;
		}
		
	}
	
	class StateProcess {
		
		public StateProcess() {;}
		public StateProcess(CanEnter _canEnterFn, OnEnter _onEnterFn, OnExit _onExitFn) {
			canEnterFn = _canEnterFn;
			onEnterFn = _onEnterFn;
			onExitFn = _onExitFn;
		}
		
		public CanEnter canEnterFn;
		public OnEnter onEnterFn;
		public OnExit onExitFn;
		
	};
	
	Dictionary<int, StateProcess> states = new Dictionary<int, StateProcess>();
	Dictionary<int, bool> commands = new Dictionary<int, bool>();
	Dictionary<StateTransition, int> transitions = new Dictionary<StateTransition, int>();
	public int CurrentState { get; private set; }
	public int PreviousState { get; private set; }
	
	public void AddState(int state) {
		states.Add(state, new StateProcess());
	}
	
	public void AddCommand(int command) {
		commands.Add(command, true);
	}
	
	public void AddTransition(int state, int command, int nextState) {
		if (!states.ContainsKey(state) || !states.ContainsKey(nextState) || !commands.ContainsKey(command)) {
			Logger.LogWarning("Failed to add transition:"
				+ ((!states.ContainsKey(state)) ? " Bad state: " + state : "")
				+ ((!states.ContainsKey(nextState)) ? " Bad nextState: " + nextState : "")
				+ ((!commands.ContainsKey(command)) ? " Bad command: " + command : ""));
			return;
		}
		transitions.Add (new StateTransition(state, command), nextState);
	}
	
	public void SetStateProcessCanEnter(int state, CanEnter fn) {
		if (!states.ContainsKey(state)) {
			Logger.LogWarning("Failed to add state process can enter: Bad state: " + state);
			return;
		}
		states[state] = new StateProcess(fn, states[state].onEnterFn, states[state].onExitFn);
	}
	
	public void SetStateProcessOnEnter(int state, OnEnter fn) {
		if (!states.ContainsKey(state)) {
			Logger.LogWarning("Failed to add state process on enter: Bad state: " + state);
			return;
		}
		states[state] = new StateProcess(states[state].canEnterFn, fn, states[state].onExitFn);
	}
	
	public void SetStateProcessOnExit(int state, OnExit fn) {
		if (!states.ContainsKey(state)) {
			Logger.LogWarning("Failed to add state process on exit: Bad state: " + state);
			return;
		}
		states[state] = new StateProcess(states[state].canEnterFn, states[state].onEnterFn, fn);
	}
	
	public void Begin(int? state = null) {
		if (states.Keys.Count == 0) {
			Logger.LogWarning("Failed to begin: No states");
		}
		
		bool searchFirstState = false;
		if (state.HasValue) {
			if (!states.ContainsKey(state.Value)) {
				searchFirstState = true;
			}
		}
		else {
			searchFirstState = true;
		}
		
		if (searchFirstState) {
			IEnumerator e = states.Keys.GetEnumerator();
			e.MoveNext();
			state = (int)e.Current;
		}
		
		CurrentState = state.Value;
	}
	
	public int GetNext(int command) {
		StateTransition transition = new StateTransition(CurrentState, command);
		int nextState;
		if (!transitions.TryGetValue(transition, out nextState)) {
			Logger.LogWarning("Invalid transition: " + CurrentState + " -> " + command);
			return -1;
		}
		if (states[nextState].canEnterFn != null && !states[nextState].canEnterFn()) {
			return -1;
		}
		return nextState;
	}
	
	public bool MoveNext(int command) {
		int result = GetNext(command);
		if (result == -1) {
			return false;
		}
		PreviousState = CurrentState;
		CurrentState = result;
		
		if (states[PreviousState].onExitFn != null) {
			states[PreviousState].onExitFn();
		}
		if (states[CurrentState].onEnterFn != null) {
			states[CurrentState].onEnterFn();
		}
		return true;
	}
	
}
