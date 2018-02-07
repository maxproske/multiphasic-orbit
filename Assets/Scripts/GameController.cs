using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
	public int GAME_STATE; // Control which hint indicator is active
	private int PREVIOUS_GAME_STATE; // Done to improve performance

    public Button nextTurnButton;
    public List<GameObject> planets;

    public Text planetText;
    public Text carbonText;
    public Text nitrogenText;
    public Text hydrogenText;

    private Planet planetScript;
    private PlanetSlot planetSlotScript;
    private GameObject selected; // hold selected GameObject

    public bool simulate;
    private float startTime;
    private Button nextTurn;
    private EventSystem es;
    private bool esgo = false; // handles un/locking of UI
    private int i;

    public Transform planetsParent; // Reference to skill tree slots parent

    // Use this for initialization
    void Start()
    {
		// Start indicator pointing at the skill tree button
		GAME_STATE = 0;
		PREVIOUS_GAME_STATE = 0;

        nextTurn = GameObject.Find("End Turn Button").GetComponent<Button>();
        nextTurn.onClick.AddListener(Simulate);

        planets = new List<GameObject>();
        es = EventSystem.current;
        simulate = false;
    }

    // Update is called once per frame
    void Update()
    {
		// Update the hint indicator
		UpdateHintIndicator (PREVIOUS_GAME_STATE, GAME_STATE);

        //print(simulate);
        // handles un/locking of UI
        if (esgo == true)
        {
            i++;
            if (i > 150)
            {
                es.enabled = true;
                i = 0;
                esgo = false;
            }
        }

        // This code enables player to click any object in the scene
        // Also updating text is here so moved selecting planets here
        // https://answers.unity.com/questions/332085/how-do-you-make-an-object-respond-to-a-click-in-c.html
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // https://docs.unity3d.com/ScriptReference/Input-mousePosition.html
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                selected = hit.collider.gameObject; // put GameObject hit by ray in variable
            }
        }

        if (planets.Contains(selected))
        {
            planetScript = selected.GetComponent<Planet>(); // get Planet script to access attributes

            // update UI
            planetText.text = planetScript.name;
            carbonText.text = planetScript.carbon.ToString();
            nitrogenText.text = planetScript.nitrogen.ToString();
            hydrogenText.text = planetScript.hydrogen.ToString();
        }
        else
        {
            // update UI when no planet is selected
            planetText.text = "No Planet Selected";
            carbonText.text = 0.ToString();
            nitrogenText.text = 0.ToString();
            hydrogenText.text = 0.ToString();
        }
    }


    string name = "";

    void Simulate()
    {
        simulate = true;
        //startTime = Time.time;
        //es.enabled = false;
        //esgo = true;

        foreach (var planet in planets)
        {
            planetScript = planet.GetComponent<Planet>();
            //print(planetScript.name);
            planetScript.StartCoroutine(planetScript.AnimateOrbit(1));
            planetScript.turnsToBuild--;
            //planetScript.planetPlaced = false;
        }

        // gets all of the buttons
        Button[] buttons = planetsParent.GetComponentsInChildren<Button>();

        foreach (Button button in buttons)
        {
                button.interactable = true;
        }

        // for now, interactability is toggled for carbon only
        //buttons[1].interactable = true;
        //planetSlotScript = buttons[1].GetComponent<PlanetSlot>();
        //planetSlotScript.go = null;
        //planetSlotScript.planetPlaced = false;
        //Button[] buttons = planetsParent.GetComponentsInChildren<Button>(false);
        //foreach (var button in buttons)
        //{
        //    Debug.Log(button.interactable);
        //    if (button.interactable == false)
        //    {
        //        button.interactable = true;
        //        planetSlotScript = button.GetComponent<PlanetSlot>();
        //        planetSlotScript.go = null;
        //        planetSlotScript.planetPlaced = false;
        //    }
        //}

    }

	// This function updates the hint indicator
	void UpdateHintIndicator(int previous, int current) {

		// If previous game state is different than the current state
		if (previous != current) {
		// Something has changed...
			switch (current) {
			case Constants.TURN_1_SKILL_TREE:
				Debug.Log ("TURN_1_SKILL_TREE (we should never be here)");
				break;
			case Constants.TURN_1_PLANET_SLOT:
				Debug.Log ("TURN_1_PLANET_SLOT");
				toggleHint("Macro Skill Tree Button");
				toggleHint("Carbon Planet Slot");
				break;
			default:
				Debug.Log ("default");
				break;
			}
		}

		// Set global previous state variable as we exit
		PREVIOUS_GAME_STATE = current;
	}

	// Toggle the hint indicator
	void toggleHint(string parentGameObjectName) {
		GameObject hint = getChildHintFromGameObjectWithName(parentGameObjectName);
		if (hint != null) {
			//Debug.Log ("Found " + parentGameObjectName + "'s " + hint.name);
			// Toggle the game object
			hint.SetActive (!hint.activeSelf);
			// Toggle the script component asyncronously
			//var pui = hint.GetComponent<UnityEngine.UI.Extensions.PolygonUI> ();
			//pui.enabled = !pui.enabled;
		} else {
			Debug.Log ("You tried to toggle the hint of a Game Object that doesn't exist!");
		}
	}

	// Call using StartCoroutine(DoSomethingAfterDelay(hint));
	IEnumerator DoSomethingAfterDelay(GameObject hint)
	{
		yield return new WaitForSeconds(1f); // The parameter is the number of seconds to wait
		// Do something...
		//var pui = hint.GetComponent<UnityEngine.UI.Extensions.PolygonUI> ();
		//pui.enabled = !pui.enabled;
	}

	// Pass a game object name, and get its child hint
	GameObject getChildHintFromGameObjectWithName(string fromGameObjectName) {
		GameObject fromGameObject = GameObject.Find (fromGameObjectName);
		//Debug.Log ("Found " + fromGameObject.name);
		Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true); // bool includeInactive = true 
		foreach (Transform t in ts) {
			//Debug.Log ("Found " + fromGameObject.name + "'s " + t.gameObject.name);
			if (t.gameObject.name == "Hint") {
				return t.gameObject;
			}
		}
		return null;
	}
}

