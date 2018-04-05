using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent( typeof(LineRenderer))]

public class Ellipsetest : MonoBehaviour {

	LineRenderer lr;

	[Range(100,100)]
	public int segments;
	public float xAxis;
	public float yAxis;

	void Awake(){
		lr = GetComponent<LineRenderer> ();
	}


	void CalculateEllipse(){
		Vector3[] points = new Vector3[segments + 1];
		for (int i = 0; i < segments; i++) {
			float angle = ((float)i / (float)segments) * 360 * Mathf.Deg2Rad;
			float x = Mathf.Sin (angle) * xAxis;
			float y = Mathf.Cos (angle) * yAxis;
			points [i] = new Vector3 (x, y, 0f);
		}
		points [segments] = points [0];
		lr.positionCount = segments + 1;
		lr.SetPositions (points);

	}

	void Onvalidate(){
		CalculateEllipse ();
	}
	
}
