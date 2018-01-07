using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class UniverseRenderer : MonoBehaviour {

	[Range(-2,2)]
	public int position;
	[Range(3,128)]
	public int segments;
	LineRenderer lr;
	public Ellipse ellipse;

	void Awake(){
		lr = GetComponent<LineRenderer> ();
		CalculateEllipse ();
	}

	void CalculateEllipse() {
		Vector3[] points = new Vector3[segments + 1];
		for (int i = 0; i < segments; i++) {
			Vector2 position2D = ellipse.Evaluate((float)i / (float)segments);

			Vector3 worldDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 10));
			Debug.Log("World Dimensions " + worldDimensions);

			points[i] = new Vector3(position2D.x + worldDimensions.x * position, 0f, position2D.y);
		}
		points[segments] = points[0];
		lr.positionCount = segments + 1;
		lr.SetPositions(points);
	}

	void OnValidate() {
		if (Application.isPlaying && lr != null) {
			CalculateEllipse ();
		}
	}
}
