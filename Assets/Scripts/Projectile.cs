using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	public Transform thisTransform;
	public Rigidbody thisRigidbody;
	public Collider thisCollider;
	public MeshRenderer thisRenderer;
	
	public Vector3 velocity;
	public Vector3 acceleration;
	public float life;
	
	void Awake() {
		ProjectileManager.Instance.AddProjectile(this);
		thisRenderer.material.color = Color.yellow;
	}
	
	public void Do() {
		life -= Time.fixedDeltaTime;
		if (life <= 0f) {
			Die();
			return;
		}
		/*velocity += acceleration * Time.fixedDeltaTime;
		thisTransform.localPosition += velocity * Time.fixedDeltaTime;*/
	}
	
	public void Die() {
		GameObject.Destroy(gameObject);
		ProjectileManager.Instance.RemoveProjectile(this);
	}
	
	public void OnCollisionEnter() {
		Die();
	}
	
}
