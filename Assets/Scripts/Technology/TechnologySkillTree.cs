using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechnologySkillTree : MonoBehaviour {

	private GameController gc;
	private Text title;

	void Awake() {
		// Make reference to the Game Controller
		gc = GameObject.Find ("Game Manager").GetComponent<GameController> ();
	}

	// Use this for initialization
	void Start () {
		// Make reference to the panel title
		title = GameObject.Find("Title Text").GetComponent<Text>();
	}

	public void setTitle(string name) {
		// Change panel title
		if (name != null) {
			title.text = name + " Skill Tree";
		} else {
			Debug.Log("Skill tree name is empty!");
		}
	}
}
