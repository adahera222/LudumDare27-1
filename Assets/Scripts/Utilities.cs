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
	
	public static void ResetGameObject(GameObject GO) {
		Transform t = GO.transform;
		t.parent = null;
		t.localPosition = Vector3.zero;
		t.localRotation = Quaternion.identity;
		t.localScale = Vector3.one;
	}
	
	public static void SetLayerRecursive(GameObject GO, int layer) {
		Transform t = GO.transform;
		GO.layer = layer;
		for (int i = 0; i < t.childCount; ++i)
		{
			SetLayerRecursive(t.GetChild(i).gameObject, layer);
		}
	}
	
}
