using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Make class require a LineRenderer to be attached whenever we run this script
[RequireComponent(typeof(LineRenderer))]
public class Orbit : MonoBehaviour {

	// Will reference the motion of the planet model
	public Transform orbitingObject;

	// Get a reference to the LineRenderer
	LineRenderer lr;

	// Range attribute between 3 and 36 to determine # of segments in ellipse
	[Range(3,36)]
	public int segments;

	// Serialized ellipse object
	public Ellipse orbitPath;

	// How far along the path of the ellipse we are
	// Clamp between 0-1
	[Range(0f, 1f)]
	public float orbitProgress = 0f;

	// How long it will take in seconds to complete one orbit
	public float orbitPeriod = 3f;

	public float orbitSpeedMultiplier = 1f;

	// Allows us to toggle the orbit in-editor
	public bool orbitActive = true;



	void Awake() {
		// Get reference when we start the game
		lr = GetComponent<LineRenderer> ();

		// Calculate ellipse right when we start the game
		if (orbitingObject.transform.parent != null) {
			CalculateEllipse ();
		}
	}

	void FixedUpdate ()
	{
		if (Application.isPlaying && lr != null) {
			CalculateEllipse ();
		}
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
				Vector2 position2D = orbitPath.Evaluate ((float)i / (float)segments);

				// Debug
				//Debug.Log ("Position of sun: (" + orbitingObject.transform.parent.localPosition.x + ", " + orbitingObject.transform.parent.localPosition.z + ")");

				// Set point at i equal to a new Vector2 of (x,y) and 0 for z value
				points [i] = new Vector3 (position2D.x + orbitingObject.transform.parent.localPosition.x, 0f, position2D.y + orbitingObject.transform.parent.localPosition.z);
			}
			// Remember we have segments + 1, and are 0-indexing
			// Very last point in the array is equal to first point, completing the ellipse
			points [segments] = points [0];

			// Set LineRenderer using newer method positionCount
			lr.positionCount = segments + 1;
			// Pass in points array
			lr.SetPositions (points);
	}

	// Call-back method. If we change values in-editor while we are playing the game, we can see those
	void OnValidate() {
		if (Application.isPlaying && lr != null) {
			CalculateEllipse ();
		}
	}

	// Use this for initialization
	void Start () {
		// Check there are no objects to move around
		if (orbitingObject == null) {
			orbitActive = false;
			// Return early
			return;
		}

		// Set orbiting object position
		SetOrbitingObjectPosition();

		// If orbit is active, start orbit animation
		StartCoroutine(AnimateOrbit());
	}

	void SetOrbitingObjectPosition() {
		// Pass in current orbit progress
		Vector2 orbitPos = orbitPath.Evaluate (orbitProgress);

		// Set its local position to a new vector position
		// Set z axis as orbitPos.y
		orbitingObject.localPosition = new Vector3(orbitPos.x, 0, orbitPos.y);
	}

	IEnumerator AnimateOrbit() {
		// Is the orbit really close to 0? We don't want it to move too fast.
		// Set it to a more reasonable minimum (every 1/10 of a second) so it won't divide by 0
		if (orbitPeriod < 0.1f) {
			orbitPeriod = 0.1f;
		}

		// If orbit is active, start orbit animation
		while (orbitActive) {
			// Make orbit faster closer to sun
			float linearMultiplier = 2f;
			int exponentialMultiplier = 3;
			float orbitSpeedMultiplier = Mathf.Pow(Mathf.Max(Mathf.Abs(orbitPath.xAxis),Mathf.Abs(orbitPath.yAxis))/linearMultiplier,exponentialMultiplier);

			// Division is one of the least efficient thing in basic C#
			// So use time.deltatime to see how far we're moving every frame
			// We want the inverse of orbitPeriod to see how fast we need to catch up
			float orbitSpeed = 1f / (orbitPeriod * orbitSpeedMultiplier);
			//Debug.Log (orbitSpeed);

			// (Amount of time frame has taken) * calculated orbit speed
			orbitProgress += Time.deltaTime * orbitSpeed;

			// Do not exceed float value if we add to the orbit progress over time
			// If we go beyond 1f, reset to between 0-1.
			orbitProgress %= 1f;

			// Set planet position based on orbit position
			SetOrbitingObjectPosition ();

			// Repeat until orbitActive is false
			yield return null;
		}
	}
}
