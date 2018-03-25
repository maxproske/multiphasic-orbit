using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour {

	// Reference to Game Controller
	private GameController gc;

	// Starts when State is instantiated in GameController
	public void SetupState() {
		// Set game controller
		gc = GameObject.Find("Game Manager").GetComponent<GameController>();
	}

	// This function updates the hint indicator
	public void UpdateState(int previous, int current)
	{
		// If previous game state is different than the current state
		if (previous != current)
		{
			//Debug.Log(current);
			// Something has changed...
			disableAllHints();
			// Handle the event
			switch (current)
			{
			case Constants.TURN_1_SKILL_TREE:
				enableHint("Macro Skill Tree Button");
				break;
			case Constants.TURN_1_PLANET_SLOT:
				enableHint("Carbon Planet Slot");
				break;
			case Constants.TURN_1_PLACE_PLANET:
				enableHint("Place Planet Here");
				break;
			case Constants.TURN_1_END_TURN:
				enableHint("End Turn Button");
				break;
			case Constants.TURN_1_WATCH_SIMULATION:
				// Do nothing.
				break;
			case Constants.TURN_2_SKILL_TREE:
				enableHint("Macro Skill Tree Button");
				break;
			case Constants.TURN_2_PLANET_SLOT:
				enableHint("Silicon Planet Slot");
				enableHint("Ammonia Planet Slot");
				enableHint("Methane Planet Slot");
				break;
			case Constants.TURN_2_PLACE_PLANET:
				enableHint("Place Planet Here");
				break;
			case Constants.TURN_2_END_TURN:
				enableHint("End Turn Button");
				break;
			case Constants.TURN_2_WATCH_SIMULATION:
				// Do nothing.
				break;
			case Constants.TURN_3_TECH_TREE:
				//Debug.Log (GameObject.Find ("Sun").GetComponentsInChildren<Transform> () [2].name);
				enablePlanetGlow(0);
				break;
			case Constants.TURN_3_TECH_SLOT:
				enableHint("Mathematics Technology Slot");
				disablePlanetGlow(0);
				break;
			case Constants.TURN_3_TECH_TREE_2:
				enablePlanetGlow(1);
				gc.playButton.interactable = true;
				break;
			case Constants.TURN_3_TECH_SLOT_2:
				enableHint("Mathematics Technology Slot");
				disablePlanetGlow(1);
				gc.playButton.interactable = true;

				break;
			default:
				Debug.Log("We shouldn't be here.");
				break;
			}
		}
		// Set global previous state variable as we exit
		gc.PREVIOUS_GAME_STATE = current;
	}

	void enablePlanetGlow(int index)
	{
		index += 1; // account for sun model
		index *= 2; // account for planet models
		Transform t = GameObject.Find("Sun").GetComponentsInChildren<Transform>()[index];
		t.GetComponent<PlanetHint>().animateHalo = true;
	}

	void disablePlanetGlow(int index)
	{
		index += 1; // account for sun model
		index *= 2; // account for planet models
		Transform t = GameObject.Find("Sun").GetComponentsInChildren<Transform>()[index];
		t.GetComponent<PlanetHint>().animateHalo = false;
	}

	void disableAllHints()
	{
		GameObject hint = GameObject.Find("Hint");
		while (hint != null)
		{
			hint.SetActive(false);
			hint = GameObject.Find("Hint");
		}
	}

	// Call using StartCoroutine(DoSomethingAfterDelay(hint));
	IEnumerator redrawHintAfterDelay(GameObject hint)
	{
		// Wait...
		yield return new WaitForSeconds(0.05f); // The parameter is the number of seconds to wait
		// Do something...
		var pui = hint.GetComponent<UnityEngine.UI.Extensions.PolygonUI>();
		pui.SetAllDirty();
	}

	// Pass a game object name, and get its child hint
	GameObject getChildHintFromGameObjectWithName(string fromGameObjectName)
	{
		GameObject fromGameObject = GameObject.Find(fromGameObjectName);
		//Debug.Log ("Found " + fromGameObject.name);
		Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true); // bool includeInactive = true 
		foreach (Transform t in ts)
		{
			//Debug.Log ("Found " + fromGameObject.name + "'s " + t.gameObject.name);
			if (t.gameObject.name == "Hint")
			{
				return t.gameObject;
			}
		}
		return null;
	}

	// Toggle the hint indicator
	void enableHint(string parentGameObjectName)
	{
		GameObject hint = getChildHintFromGameObjectWithName(parentGameObjectName);
		if (hint != null && hint.name == "Hint")
		{
			//Debug.Log ("Found " + parentGameObjectName + "'s " + hint.name);
			// Toggle the game object
			hint.SetActive(true);
			// Toggle the script component asyncronously
			StartCoroutine(redrawHintAfterDelay(hint));
		}
		else
		{
			Debug.Log("You tried to toggle the hint of a Game Object that doesn't exist!");
		}
	}

}
