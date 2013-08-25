using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	
	void Update() {
		if (Input.anyKeyDown) {
			Application.LoadLevel(1);
		}
	}
	
}
