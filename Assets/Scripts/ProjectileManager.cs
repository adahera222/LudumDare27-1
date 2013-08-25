using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectileManager : LazySingleton<ProjectileManager> {

	public HashSet<Projectile> projectiles = new HashSet<Projectile>();
	public HashSet<Projectile> projectilesToRemove = new HashSet<Projectile>();
	
	void Awake() {
		Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Bullet"), LayerMask.NameToLayer("Bullet"));
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		IEnumerator e = projectiles.GetEnumerator();
		Projectile p;
		while (e.MoveNext()) {
			p = (Projectile)e.Current;
			p.Do();
		}
		
		e = projectilesToRemove.GetEnumerator();
		while (e.MoveNext()) {
			projectiles.Remove((Projectile)e.Current);
		}
		projectilesToRemove.Clear();
	}
	
	public void AddProjectile(Projectile p) {
		projectiles.Add(p);
		p.thisTransform.parent = transform;
	}
	
	public void RemoveProjectile(Projectile p) {
		projectilesToRemove.Add(p);
	}
}
