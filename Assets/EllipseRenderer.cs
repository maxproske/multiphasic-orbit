using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Orbital Paths in Unity Tutorial: https://www.youtube.com/watch?v=mQKGRoV_jBc

// Make class require a LineRenderer to be attached whenever we run this script
[RequireComponent(typeof(LineRenderer))]
public class EllipseRenderer : MonoBehaviour {

	// Get a reference to the LineRenderer
	LineRenderer lr;

	// Range attribute between 3 and 36 to determine # of segments in ellipse
	// (6 will look like a polygon, 36 will look more like an ellipse)
	[Range(3,36)]
	public int segments;

	// Height and width of ellipse contained in Ellipse as a Vector2
	public Ellipse ellipse;

	// Call-back method on game start
	void Awake() {
		// Get reference when we start the game
		lr = GetComponent<LineRenderer> ();

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

			// Pass in t value (i/segments)
			Vector2 position2D = ellipse.Evaluate((float)i / (float)segments);

			// Set point at i equal to a new Vector2 of (x,y) and 0 for z value
			points[i] = new Vector3(position2D.x, position2D.y, 0f);
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
		if (Application.isPlaying && lr != null) {
			CalculateEllipse ();
		}
	}
}
