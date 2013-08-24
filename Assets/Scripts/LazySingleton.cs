using UnityEngine;
using System.Collections;

public class LazySingleton<T> : MonoBehaviour where T : MonoBehaviour {
	
	private static T instance;
	
	public static T Instance {
		get {
			if (instance == null) {
				GameObject GO = GameObject.Find(typeof(T).ToString());
				if (GO == null) {
					GO = new GameObject(typeof(T).ToString());
					GO.AddComponent<T>();
				}
				instance = GO.GetComponent<T>();
			}
			return instance;
		}
	}
	
	public static bool HasInstance {
		get {
			return instance != null;
		}
	}
	
}
