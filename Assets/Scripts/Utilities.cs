using UnityEngine;
using System.Collections;

public class Utilities {
	
	public static Vector3 RotateAroundPoint(Vector3 point, Vector3 pivot, Quaternion angle) {
		Vector3 finalPos = point - pivot;
		finalPos = angle * finalPos;
		finalPos += pivot;
		return finalPos;
	}
	
	public static float ClampAngle(float angle, float min, float max) {
		if (angle > 360) {
			angle -= 360;
		}
		if (angle < -360) {
			angle += 360;
		}
		return Mathf.Clamp(angle, min, max);
	}
	
}
