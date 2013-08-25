using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {
	
	public GameObject projectilePrefab;
	
	public Transform projectileRoot;
	public AudioClip laserClip;
	
	public float cooldown = 1f;
	public float cooldownTimer;
	public float projectileForce = 1000f;
	public float projectileLife = 10f;

	void FixedUpdate() {
		if (cooldownTimer > 0f) {
			cooldownTimer -= Time.fixedDeltaTime;
		}
		
		bool fire1Down = Input.GetButton("Fire1");
		if (fire1Down && cooldownTimer <= 0f) {
			Fire();
		}
	}
	
	void Fire() {
		cooldownTimer = cooldown;
		GameObject newGO = (GameObject)Instantiate(projectilePrefab, Vector3.zero, Quaternion.identity);
		newGO.transform.position = projectileRoot.position;
		newGO.transform.rotation = Camera.main.transform.rotation;
		Projectile projectile = newGO.GetComponent<Projectile>();
		if (projectile != null) {
			//projectile.velocity = dir * projectileSpeed;
			projectile.life = projectileLife;
			projectile.thisRigidbody.AddForce(projectile.thisTransform.forward * projectileForce);
			Physics.IgnoreCollision(projectile.thisCollider, collider);
			AudioSource.PlayClipAtPoint(laserClip, projectile.thisTransform.position, 0.25f);
		}
	}
	
}
