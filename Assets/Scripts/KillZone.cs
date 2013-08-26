using UnityEngine;
using System.Collections;

public class KillZone : MonoBehaviour {

	public void OnTriggerEnter(Collider collider) {
		if (collider.tag == "Player") {
			PlayerManager.Instance.CurPlayer.DealDamage(100f);
		}
	}
	
}
