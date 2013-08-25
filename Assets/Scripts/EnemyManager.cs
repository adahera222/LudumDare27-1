using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : LazySingleton<EnemyManager> {

	public GameObject enemyPrefab;
	
	public HashSet<Enemy> enemies = new HashSet<Enemy>();
	HashSet<Enemy> enemiesToRemove = new HashSet<Enemy>();
	
	void FixedUpdate() {
		IEnumerator e = enemiesToRemove.GetEnumerator();
		while (e.MoveNext()) {
			enemies.Remove((Enemy)e.Current);
		}
		enemiesToRemove.Clear();
	}
	
	public void SpawnEnemy() {
		GameObject newGO = (GameObject)Instantiate(enemyPrefab, Vector3.zero, Quaternion.identity);
		Utilities.ResetGameObject(newGO);
		Enemy enemy = newGO.GetComponent<Enemy>();
		AddEnemy(enemy);
		
		int mapWidth = Map.Instance.handler.MapWidth;
		int mapHeight = Map.Instance.handler.MapHeight;
		
		enemy.thisTransform.position = new Vector3((float)Random.Range(-(mapWidth - 1) / 2, (mapWidth - 1) / 2), (float)Random.Range(1f, 12f), (float)Random.Range(-(mapHeight - 1) / 2, (mapHeight - 1) / 2));
	}
	
	public void AddEnemy(Enemy e) {
		enemies.Add(e);
		e.thisTransform.parent = transform;
	}
	
	public void RemoveEnemy(Enemy e) {
		enemiesToRemove.Add(e);
	}
	
}
