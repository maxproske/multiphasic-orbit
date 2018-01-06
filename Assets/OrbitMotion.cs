using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitMotion : MonoBehaviour {

	// Will reference the motion of the planet model
	public Transform orbitingObject;

	// Serialized ellipse object
	public Ellipse orbitPath;

	// How far along the path of the ellipse we are
	// Clamp between 0-1
	[Range(0f, 1f)]
	public float orbitProgress = 0f;

	// How long it will take in seconds to complete one orbit
	public float orbitPeriod = 3f;

	// Allows us to toggle the orbit in-editor
	public bool orbitActive = true;

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

		// Division is one of the least efficient thing in basic C#
		// So use time.deltatime to see how far we're moving every frame
		// We want the inverse of orbitPeriod to see how fast we need to catch up
		float orbitSpeed = 1f / orbitPeriod;

		// If orbit is active, start orbit animation
		while (orbitActive) {
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
