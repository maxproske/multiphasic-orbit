using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorUI : MonoBehaviour {
	
	private float speed = 0.2f; // Closer to zero = faster
	private float bounceDistance = 0.333f; // Pixels to bounce left

	private float lastX = 0;
	private float lastY = 0;
	private float addX = 0;
	private float addY = 0;
	public bool rotated = false;

	/// Coroutines will stop when we disable the whole game object
	void OnEnable() {
		// Restart coroutine when indicator is re-enabled
		StartCoroutine (Start ());
	}

	IEnumerator Start () {
		Vector3 pointA = new Vector3 (0, 0, 0);
		Vector3 pointB = new Vector3 (0, 0, 0);
		// IT'S PURE MAGIC
		if (!rotated) {
			pointA = new Vector3 (bounceDistance / 2, 0, 0);
			pointB = pointA + new Vector3 (-bounceDistance, 0, 0);
		} else {
			pointA = new Vector3 (0, bounceDistance / 2, 0);
			pointB = pointA + new Vector3 (0, -bounceDistance, 0);
		}

		while (true) {
			yield return StartCoroutine(MoveObject(transform, pointA, pointB, speed));
			yield return StartCoroutine(MoveObject(transform, pointB, pointA, speed));
		}
	}

	IEnumerator MoveObject (Transform thisTransform, Vector3 startPos, Vector3 endPos, float time) {
		float i = 0.0f;
		float rate = 1.0f / time;
		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			thisTransform.position = Vector3.Lerp(startPos + thisTransform.position, endPos + thisTransform.position, i);
			yield return null;
		}
	}
}
