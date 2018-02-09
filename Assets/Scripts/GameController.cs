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

	public GameObject microSkillTree; // Reference to macro skill tree

    public bool simulate;
    private float startTime;
    private Button nextTurn;
    private EventSystem es;
    private bool esgo = false; // handles un/locking of UI
    private int i;
    public bool canBuild;
    // gets all of the buttons

	private int turnNum = 0;
	private bool ifBlackhole = false;
	private bool ifWarning=false;
	private int count = 0;
	private GameObject blackHole;
	private GameObject comet;
	private bool ifLose=false;
	private bool ifComet=false;
	private int comeMove = 0;
	public GameObject temPlanet;
	private Vector3 movedirection;

    Button[] buttons;


    public Transform planetsParent; // Reference to skill tree slots parent

    // Use this for initialization
    void Start()
    {
        // Start indicator pointing at the skill tree button
        GAME_STATE = 0;
        PREVIOUS_GAME_STATE = -1;

        nextTurn = GameObject.Find("End Turn Button").GetComponent<Button>();
        nextTurn.onClick.AddListener(Simulate);

        planets = new List<GameObject>();
        es = EventSystem.current;
        simulate = false;
        buttons = planetsParent.GetComponentsInChildren<Button>();

	


    }

    // Update is called once per frame
    void Update()
    {
		
		if (ifComet == true&&comeMove<=150) {
			comeMove++;
			movedirection = temPlanet.transform.position - comet.transform.position;
			movedirection = movedirection.normalized;
			comet.transform.position += movedirection*0.5f ;

		}
		if (comeMove > 150) {
			comeMove = 0;
			ifComet = false;

		}
		if (ifBlackhole == true) {
			blackHole.transform.localScale += new Vector3 (turnNum * 1, turnNum * 1, turnNum * 1);
			ifBlackhole = false;
		}
		if (ifComet == true) {
			
		}
        // Update the hint indicator
        UpdateHintIndicator(PREVIOUS_GAME_STATE, GAME_STATE);

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
		if (ifWarning == true) {
			count++;

			if (count > 300) {
				count = 0;
				ifWarning = false;
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
            if (planetScript.turnsToBuild < 1)
            {
                planetText.text = planetScript.name;
            } else
            {
                planetText.text = planetScript.turnsToBuild + " turns left to build: " + planetScript.name;
            }
            
            carbonText.text = planetScript.carbon.ToString();
            nitrogenText.text = planetScript.nitrogen.ToString();
            hydrogenText.text = planetScript.hydrogen.ToString();

			// Open Skill Tree after a certain turn
			if (GAME_STATE >= Constants.TURN_3_TECH_TREE) {
				microSkillTree.SetActive (true);
			}

			// Change Game State
			if (GAME_STATE == Constants.TURN_3_TECH_TREE) {
				GAME_STATE = Constants.TURN_3_TECH_SLOT;
			}
			else if (GAME_STATE == Constants.TURN_3_TECH_TREE_2) {
				GAME_STATE = Constants.TURN_3_TECH_SLOT_2;
			}
        }
        else
        {
            // update UI when no planet is selected
            planetText.text = "No Planet Selected";
            carbonText.text = 0.ToString();
            nitrogenText.text = 0.ToString();
            hydrogenText.text = 0.ToString();
        }

        if (simulate)
        {
            es.enabled = false;
        }
        else
        {
            es.enabled = true;
        }



        if (canBuild)
        {
            if (simulate)
            {
                foreach (Button button in buttons)
                {
                    button.interactable = false;
                }
            }
            else
            {
                foreach (Button button in buttons)
                {
                    button.interactable = true;
                }
            }
        }


    }


    string name = "";

	void OnGUI(){
		if (ifWarning == true && count > 30)
		{
			GUI.color = Color.red;
			GUI.skin.label.fontSize = 30;
			GUI.Label(new Rect(Screen.width/2-250, 100, 500, 100), "Warning: the black hole is coming!!!" );
			GUI.skin.label.fontSize = 12;
		}
		if (ifLose == true )
		{
			GUI.color = Color.red;
			GUI.skin.label.fontSize = 30;
			GUI.Label(new Rect(Screen.width/2-100, 100, 500, 100), "You Lose!!!" );
			Time.timeScale = 0;
			GUI.skin.label.fontSize = 12;
		}

	}

    void Simulate()
    {
        simulate = true;
		turnNum++;


		if (turnNum == 1) {
			ifWarning = true;


		}
		if (turnNum == 1) {
			blackHole = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			blackHole.transform.position = new Vector3(0, 1.5F, 0);
			comet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			comet.transform.position = new Vector3(50, 50, 0);
			
	
			temPlanet = planets[Random.Range (0, planets.Count)];
			movedirection = temPlanet.transform.position - comet.transform.position;
			movedirection = movedirection.normalized;
//
		}


		if (turnNum > 0) {

			ifBlackhole = true;
			ifComet = true;
		}


		if (turnNum == 3) {
			ifLose = true;
		}
        //startTime = Time.time;
        //es.enabled = false;
        //esgo = true;

        foreach (var planet in planets)
        {
            planetScript = planet.GetComponent<Planet>();
            //print(planetScript.name);
            planetScript.turnsToBuild--;
            planetScript.StartCoroutine(planetScript.AnimateOrbit(1));
            
            //planetScript.planetPlaced = false;
        }

        //foreach (Button button in buttons)
        //{
        //    button.interactable = true;
        //}

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
    void UpdateHintIndicator(int previous, int current)
    {
        // If previous game state is different than the current state
        if (previous != current)
        {
			Debug.Log (current);
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
				enableHint ("Mathematics Technology Slot");
				disablePlanetGlow(0);
				break;
			case Constants.TURN_3_TECH_TREE_2:
				enablePlanetGlow(1);
				break;
			case Constants.TURN_3_TECH_SLOT_2:
				enableHint ("Mathematics Technology Slot");
				disablePlanetGlow(1);
				break;
            default:
                Debug.Log("We shouldn't be here.");
                break;
            }
        }
        // Set global previous state variable as we exit
        PREVIOUS_GAME_STATE = current;
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

	void enablePlanetGlow(int index) {
		index += 1; // account for sun model
		index *= 2; // account for planet models
		Transform t = GameObject.Find ("Sun").GetComponentsInChildren<Transform> () [index];
		t.GetComponent<PlanetHint> ().animateHalo = true;
	}

	void disablePlanetGlow(int index) {
		index += 1; // account for sun model
		index *= 2; // account for planet models
		Transform t = GameObject.Find ("Sun").GetComponentsInChildren<Transform> () [index];
		t.GetComponent<PlanetHint> ().animateHalo = false;
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
}

