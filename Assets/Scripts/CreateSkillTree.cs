using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSkillTree : MonoBehaviour {

	public GameObject emptyPlanetSlot;
	public GameObject carbonPlanetSlot;
	public GameObject siliconPlanetSlot;
	public GameObject ammoniaPlanetSlot;
	public GameObject methanePlanetSlot;

	// Use this for initialization
	void Start () {
		createPlanet (emptyPlanetSlot);		createPlanet (carbonPlanetSlot);	createPlanet (emptyPlanetSlot);
		createPlanet (siliconPlanetSlot);	createPlanet (ammoniaPlanetSlot); 	createPlanet (methanePlanetSlot);
	}

	void createPlanet(GameObject prefab) {
		GameObject go = Instantiate (prefab, transform.position, Quaternion.identity) as GameObject;
		go.transform.parent = transform;
		go.transform.localScale = new Vector3 (1, 1, 1);
	}
}
