using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSlot : MonoBehaviour {

	public Transform sun;
	public GameObject planet;

	public void createPlanet () {
		// Create a planet as a child of the sun
		GameObject go = Instantiate(planet) as GameObject;
		go.transform.parent = sun.transform;

		// Generate a random orbit animation
		OrbitMotion om = go.GetComponent<OrbitMotion>();
		om.enabled = true;

		om.orbitPath.xAxis = Random.Range(1.0f,10.0f);
		om.orbitPath.yAxis = Random.Range(2.0f,11.0f);
		om.orbitPeriod = Random.Range(6.0f,20.0f);

		float scale = Random.Range(1.0f, 2.0f); 
		om.transform.localScale = new Vector3 (scale, scale, scale);
	}
}
