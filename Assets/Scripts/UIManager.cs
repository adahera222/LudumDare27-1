using UnityEngine;
using System.Collections;

public class UIManager : LazySingleton<UIManager> {
	
	public GUIText healthText;
	public GUIText clockText;
	public GUIText scoreText;
	public GUIText crosshair;
	public GUIText gameOverText;
	public Timer clockTimer;
	
	enum ClockState {
		SPAWNING,
		SPAWNED,
		DESPAWNING,
		DESPAWNED,
	}
	enum ClockCommand {
		NEXT,
	}
	FiniteStateMachine clockMachine = new FiniteStateMachine();
	
	void Awake() {
		Screen.showCursor = false;
		Screen.lockCursor = true;
		
		foreach (var value in System.Enum.GetValues(typeof(ClockState))) {
			clockMachine.AddState((int)value);
		}
		foreach (var value in System.Enum.GetValues(typeof(ClockCommand))) {
			clockMachine.AddCommand((int)value);
		}
		clockMachine.AddTransition((int)ClockState.SPAWNING, (int)ClockCommand.NEXT, (int)ClockState.SPAWNED);
		clockMachine.AddTransition((int)ClockState.SPAWNED, (int)ClockCommand.NEXT, (int)ClockState.DESPAWNING);
		clockMachine.AddTransition((int)ClockState.DESPAWNING, (int)ClockCommand.NEXT, (int)ClockState.DESPAWNED);
		clockMachine.AddTransition((int)ClockState.DESPAWNED, (int)ClockCommand.NEXT, (int)ClockState.SPAWNING);
		clockMachine.SetStateProcessOnEnter((int)ClockState.SPAWNING, MapSpawn);
		clockMachine.SetStateProcessOnEnter((int)ClockState.DESPAWNING, MapDespawn);
		clockMachine.Begin((int)ClockState.SPAWNING);
		MapSpawn();
		
		Countdown();
	}
	
	void FixedUpdate() {
		healthText.text = string.Format("{0}", Mathf.FloorToInt(PlayerManager.Instance.CurPlayer.health));
		scoreText.text = string.Format("{0}", Mathf.FloorToInt(PlayerManager.Instance.CurPlayer.score));
		clockText.text = string.Format("{0}:{1:00}", Mathf.FloorToInt(clockTimer.CurTime), (clockTimer.CurTime - Mathf.FloorToInt(clockTimer.CurTime)) * 100f);
		
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}
		else if (Input.GetKeyDown(KeyCode.F1)) {
			Application.LoadLevel(0);
		}
	}
		
	void Countdown() {
		int numEnemies = Random.Range(1, 10);
		for (int i = 0; i < numEnemies; ++i) {
			EnemyManager.Instance.SpawnEnemy();
		}
		
		clockTimer.SetCountdown(10f, delegate() {
			clockMachine.MoveNext((int)ClockCommand.NEXT);
			Countdown();
		});
		clockTimer.Start();
	}
		
	void MapSpawn() {
		Map.Instance.SpawnMap();
	}
	
	void MapDespawn() {
		Map.Instance.DespawnMap();
	}
	
}
