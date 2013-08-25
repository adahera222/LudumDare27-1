using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public Transform thisTransform;
	public Collider thisCollider;
	public CharacterController thisController;
	public CharacterMotor thisMotor;
	public Shoot thisShoot;
	
	public AudioClip hurtClip;
	
	public float health;
	public bool dead;
	public float score;
	
	void Awake() {
		dead = false;
		health = 100f;
		score = 0f;
	}
	
	public void DealDamage(float damage) {
		health = Mathf.Clamp(health - damage, 0f, 100f);
		if (health <= 0f) {
			Die();
		}
		
		UIManager.Instance.ShowHurtOverlay();
		AudioSource.PlayClipAtPoint(hurtClip, thisTransform.position);
	}
	
	public void Die() {
		UIManager.Instance.ShowGameOver();
		thisMotor.canControl = false;
		thisShoot.enabled = false;
		dead = true;
	}
	
}
