using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Orbit Paths in Unity: Understanding Ellipses
// Tutorial: https://www.youtube.com/watch?v=mQKGRoV_jBc

// Modifications: width = 0.1, segments = 24

// Make class require a LineRenderer be attached whenever we run this script
[RequireComponent(typeof(LineRenderer))]
public class Ellipse : MonoBehaviour {

	// Get a reference to LineRenderer
	LineRenderer lr;

	// Range attribute between 3 and 36 to determine # of segments in ellipse
	// (6 will look like a polygon, 36 will look more like an ellipse)
	[Range(3,36)]
	public int segments;

	// Height and width of ellipse.
	// Largest dimension is called the major axis, smallest dimension is the minor axis
	public float xAxis = 5f;
	public float yAxis = 3f;

	// Call-back method on game start
	void Awake() {
		// Get reference when we start the game
		lr = GetComponent<LineRenderer>();

		// Calculate ellipse right when we start the game
		CalculateEllipse();
	}

	// Calculate the ellipse
	void CalculateEllipse() {
		// Create an array of Vector3's.
		// Populate LineRenderer with array of points to render
		// (segments + 1 to complete the ring around. We'll make the last element equal to the first element later.)
		Vector3[] points = new Vector3[segments + 1];

		// Iterate through these points
		for (int i = 0; i < segments; i++) {
			// For each iteration, getting closer to full # of segments 
			// Cast both to floats, to make it a number between 0-1
			// Multiply by 360 to get its equivalent in degrees
			// Use Mathf.Deg2Rad, because Sin and Cosine work in radians, not degrees 
			float angle = ((float)i / (float)segments) * 360 * Mathf.Deg2Rad;

			// Figure out our x point and our y point
			float x = Mathf.Sin (angle) * xAxis;
			float y = Mathf.Cos (angle) * yAxis;

			// Set point at i equal to a new Vector3 of (x,y) and 0 for z value
			points[i] = new Vector3(x, y, 0f);
		}
		// Remember we have segments + 1, and are 0-indexing
		// Very last point in the array is equal to first point, completing the ellipse
		points[segments] = points[0];

		// Set LineRenderer using newer method positionCount
		lr.positionCount = segments + 1;
		// Pass in points array
		lr.SetPositions(points);
	}
		
	// Call-back method. If we change values in-editor while we are playing the game, we can see those
	void OnValidate() {
		CalculateEllipse ();
	}
}
