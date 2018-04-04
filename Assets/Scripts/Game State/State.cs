using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State : MonoBehaviour {

	// Reference to Game Controller
	private GameController gc;

    private UIController ui;

	// Starts when State is instantiated in GameController
	public void SetupState() {
		// Set game controller
		gc = GameObject.Find("Game Manager").GetComponent<GameController>();
        // Set ui controller
        ui = GameObject.Find("Canvas").GetComponent<UIController>();
	}

	// This function updates the hint indicator
	public void UpdateState(int previous, int current)
	{
		// If previous game state is different than the current state
		if (previous != current)
		{
            //Debug.Log(current);

			// Something has changed...
			DisableAllHints();

			// Handle the event
            switch (GameController.level)
			{
                case 1:
                switch (current)
                {
                    // Opened level select screen
                    case Constants.LEARNERS_MISSION_1:
                        // Start the learner's test.
                        break;
                    // Learners test turn 1 started
                    case Constants.LEARNERS_MISSION_2:
                        // Build a stone planet.
                        ui.SetHintActive((RectTransform)ui.rightStoneButton.transform, true);
                        break;
                    // Placed stone planet
                    case Constants.LEARNERS_MISSION_3:
                        // Play end turn simulation.
                        ui.SetHintActive((RectTransform)ui.rightNextTurnButton.transform, true);
                        break;
                    // Played end turn simulation.
                    case Constants.LEARNERS_MISSION_4:
                        // Learn high energy magnetics.
                        StartCoroutine(PauseThenActivateMission4());
                        break;
                    case Constants.LEARNERS_MISSION_5:
                        // Nothing yet.
                        break;
                    case Constants.LEARNERS_MISSION_6:
                        // Nothing yet.
                        break;
                    default:
                        Debug.Log("Default state. We shouldn't be here.");
                        break;
                }
                break;
                default:
                    //Debug.Log("Default level. We shouldn't be here.");
                    break;
            }
		}
		// Set global previous state variable as we exit
		gc.PREVIOUS_GAME_STATE = current;
	}

    public void DisableAllHints()
	{
        foreach(RectTransform hint in ui.hintContainer.transform)
        {
            hint.gameObject.SetActive(false);
        }
	}

	IEnumerator PauseThenActivateMission4()
	{
		// Wait...
		yield return new WaitForSeconds(0.05f); // The parameter is the number of seconds to wait
		// Do something...
		ui.SetHintActive((RectTransform)ui.leftTechnology1Button.transform, true);
	}

	// void enablePlanetGlow(int index)
	// {
	// 	index += 1; // account for sun model
	// 	index *= 2; // account for planet models
	// 	Transform t = GameObject.Find("Sun").GetComponentsInChildren<Transform>()[index];
	// 	t.GetComponent<PlanetHint>().animateHalo = true;
	// }

	// void disablePlanetGlow(int index)
	// {
	// 	index += 1; // account for sun model
	// 	index *= 2; // account for planet models
	// 	Transform t = GameObject.Find("Sun").GetComponentsInChildren<Transform>()[index];
	// 	t.GetComponent<PlanetHint>().animateHalo = false;
	// }

	// void disableAllHints()
	// {
	// 	GameObject hint = GameObject.Find("Hint");
	// 	while (hint != null)
	// 	{
	// 		hint.SetActive(false);
	// 		hint = GameObject.Find("Hint");
	// 	}
	// }

	// // Call using StartCoroutine(DoSomethingAfterDelay(hint));
	// IEnumerator redrawHintAfterDelay(GameObject hint)
	// {
	// 	// Wait...
	// 	yield return new WaitForSeconds(0.05f); // The parameter is the number of seconds to wait
	// 	// Do something...
	// 	var pui = hint.GetComponent<UnityEngine.UI.Extensions.PolygonUI>();
	// 	pui.SetAllDirty();
	// }

    //                 StartCoroutine(redrawHintAfterDelay(hint));

	// // Pass a game object name, and get its child hint
	// GameObject getChildHintFromGameObjectWithName(string fromGameObjectName)
	// {
	// 	GameObject fromGameObject = GameObject.Find(fromGameObjectName);
	// 	//Debug.Log ("Found " + fromGameObject.name);
	// 	Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true); // bool includeInactive = true 
	// 	foreach (Transform t in ts)
	// 	{
	// 		//Debug.Log ("Found " + fromGameObject.name + "'s " + t.gameObject.name);
	// 		if (t.gameObject.name == "Hint")
	// 		{
	// 			return t.gameObject;
	// 		}
	// 	}
	// 	return null;
	// }

	// // Toggle the hint indicator
	// void enableHint(string parentGameObjectName)
	// {
	// 	GameObject hint = getChildHintFromGameObjectWithName(parentGameObjectName);
	// 	if (hint != null && hint.name == "Hint")
	// 	{
	// 		//Debug.Log ("Found " + parentGameObjectName + "'s " + hint.name);
	// 		// Toggle the game object
	// 		hint.SetActive(true);
	// 		// Toggle the script component asyncronously
	// 		StartCoroutine(redrawHintAfterDelay(hint));
	// 	}
	// 	else
	// 	{
	// 		Debug.Log("You tried to toggle the hint of a Game Object that doesn't exist!");
	// 	}
	// }
}
