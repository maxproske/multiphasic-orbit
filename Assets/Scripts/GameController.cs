using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	private void Update() {
		// On mouse up
		/*
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			// create a horizontal logic plane at 0,0,0 whose normal points to +Y:
			Plane hPlane = new Plane(Vector3.up, Vector3.zero);
			// Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
			float distance = 0; 
			// if the ray hits the logic plane...
			if (hPlane.Raycast (ray, out distance)) {
				// if the ray hits the physical orbit plane
				RaycastHit hit = new RaycastHit ();
				if (Physics.Raycast (ray, out hit)) {
					if (hit.collider.gameObject.name == "Orbit Plane") {
						Debug.Log ("I'll allow it.");
					}
				}

				
				// get the hit point:
				Vector3 location = ray.GetPoint(distance);
				Debug.Log (location);

				Debug.Log(distance);

				GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
				sphere.transform.localScale*=1;
				sphere.transform.position = location;
			}
		}

		/*
		// Get name of game object clicked
		if (Input.GetMouseButtonDown (0)) {
			var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			if (Physics.Raycast (ray, out hit))
				Debug.Log(hit.collider.gameObject.name);
		}

	}
	*/
	}
}
