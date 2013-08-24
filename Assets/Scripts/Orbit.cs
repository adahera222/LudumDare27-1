using UnityEngine;
using System.Collections;

public class Orbit : MonoBehaviour {
	
	public Transform thisTransform;
	public Vector3 rotationMask = new Vector3(0f, 1f, 0f);
	public float speed = 36;
	public float distance = 1;
	public Transform target;
	public Vector3 rotation = new Vector3();
	
	void FixedUpdate () {
		rotation += rotationMask * speed * Time.deltaTime;
		rotation.x = Utilities.ClampAngle(rotation.x, -360, 360);
		rotation.y = Utilities.ClampAngle(rotation.y, -360, 360);
		rotation.z = Utilities.ClampAngle(rotation.z, -360, 360);
		
		thisTransform.localPosition = Utilities.RotateAroundPoint(Vector3.up * distance + target.position, target.position, Quaternion.Euler(rotation.x, rotation.y, rotation.z));
	}
	
}
