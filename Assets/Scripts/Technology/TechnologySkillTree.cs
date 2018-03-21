using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechnologySkillTree : MonoBehaviour {

	private GameController gc;
	private Text title;
	public Planet planetScript;

	public int mathematics = 0;
	public int interplanetaryNetworking = 0;
	public int massParticleDisplacement = 0;

	private Button[] buttons;

	void Awake() {
		// Get all buttons in children
		buttons = gameObject.transform.Find("Technology Parent").GetComponentsInChildren<Button>();

		// Make reference to the Game Controller
		gc = GameObject.Find ("Game Manager").GetComponent<GameController> ();
	}

	// Use this for initialization
	void Start () {
		// Make reference to the panel title
		title = GameObject.Find("Title Text").GetComponent<Text>();
	}

	public void Unlock() {
		//Debug.Log ("Updating unlocks for " + title.text + " (" + mathematics + ", " + interplanetaryNetworking + ", " + massParticleDisplacement + ")");

		for (int i = 0; i < buttons.Length; i++)
		{
			// Mathematics has been unlocked
			if (mathematics == 0) {
				if (buttons [i].transform.parent.name == "Mathematics") buttons[i].interactable = false;
			}
			if (mathematics > 0&&planetScript.hydrogen>15) {
				Debug.Log (buttons [i].transform.parent.name);
				if (buttons [i].transform.parent.name == "Mathematics Technology Slot") buttons[i].interactable = false;
				if (buttons [i].transform.parent.name == "Interplanetary Networking Technology Slot") buttons[i].interactable = true;
			}
			if (interplanetaryNetworking > 0) {
				if (buttons [i].transform.parent.name == "Interplanetary Networking Technology Slot") buttons[i].interactable = false;
				if (buttons [i].transform.parent.name == "Mass Particle Displacement Technology Slot") buttons[i].interactable = true;
			}
			if (massParticleDisplacement > 0) {
				if (buttons [i].transform.parent.name == "Mass Particle Displacement Technology Slot") buttons[i].interactable = false;
			}
		}
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
