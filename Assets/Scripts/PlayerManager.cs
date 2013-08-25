using UnityEngine;
using System.Collections;

public class PlayerManager : LazySingleton<PlayerManager> {

	public Player CurPlayer { get; private set; }
	
	void Awake() {
		if (CurPlayer == null) {
			GameObject GO = GameObject.FindWithTag("Player");
			if (GO != null) {
				CurPlayer = GO.GetComponent<Player>();
			}
		}
	}
	
}
