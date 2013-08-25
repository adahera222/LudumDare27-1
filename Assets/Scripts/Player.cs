using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public Transform thisTransform;
	public Collider thisCollider;
	public CharacterController thisController;
	public CharacterMotor thisMotor;
	
	public float health;
	
	void Awake() {
		health = 100f;
	}
	
	public void DealDamage(float damage) {
		health = Mathf.Clamp(health - damage, 0f, 100f);
		if (health <= 0f) {
			Die();
		}
	}
	
	public void Die() {
		UIManager.Instance.gameOverText.gameObject.SetActive(true);
		thisMotor.canControl = false;
	}
	
}
