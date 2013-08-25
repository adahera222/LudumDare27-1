using UnityEngine;
using System.Collections;

public class DirectionalLightManager : MonoBehaviour {
	
	public Light thisLight;
	Timer timer;
	
	Color colorFrom;
	Color colorTo;
	Quaternion rotateFrom;
	Quaternion rotateTo;
	
	void Awake() {
		Countdown();
	}
	
	void Update() {
		float t = (timer.CountdownTime - timer.CurTime) / timer.CountdownTime;
		thisLight.color = Color.Lerp(colorFrom, colorTo, t);
		thisLight.transform.rotation = Quaternion.Lerp(rotateFrom, rotateTo, t);
	}
	
	void Countdown() {
		if (timer == null) {
			timer = gameObject.AddComponent<Timer>();
		}
		timer.SetCountdown(10f, delegate() {
			Countdown();
		});
		timer.Start();
		
		colorFrom = thisLight.color;
		colorTo = new Color((float)Random.Range(0f, 1f), (float)Random.Range(0f, 1f), (float)Random.Range(0f, 1f));
		rotateFrom = thisLight.transform.rotation;
		rotateTo = Quaternion.Euler((float)Random.Range(30f, 60f), (float)Random.Range(-180f, 180f), 0f);
	}
	
}
