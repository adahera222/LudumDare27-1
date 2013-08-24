using UnityEngine;
using System.Collections;

public class LazySingleton<T> : MonoBehaviour where T : Component {
	
	private static T instance;
	
	public static T Instance {
		get {
			if (instance == null) {
				GameObject GO = GameObject.Find(typeof(T).ToString());
				if (GO == null) {
					GO = new GameObject(typeof(T).ToString());
				}
				instance = GO.GetComponent<T>();
			}
			return instance;
		}
	}
	
}
