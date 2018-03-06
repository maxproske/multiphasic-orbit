using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
	public int GAME_STATE; // Control which hint indicator is active
    public int PREVIOUS_GAME_STATE; // Done to improve performance
	public Button nextTurn;

	// Linking Buttons
    public Button startLinkButton;
    public Button linkButton;
	public bool placing;
    public bool linking; // tracks linking state
    public bool firstPlanet; // tracks which planet about to link
    public bool linkedAlready = false;
    public GameObject planet1;
    public GameObject planet2;
    private Planet firstPlanetScript;
    private Planet secondPlanetScript;
    public Text linkingText;

    public Text planetText;
    public Text carbonText;
    public Text nitrogenText;
    public Text hydrogenText;

    // Fail Link
    public bool fail;
    public GameObject roguePrefab;
    private List<GameObject> roguePlanets;

    public int carbonIncrement = 0; // incrementer for planet names
    public int siliconIncrement = 0; // incrementer for planet names
    public int ammoniaIncrement = 0; // incrementer for planet names
    public int methaneIncrement = 0; // incrementer for planet names
    public int germaniumIncrement = 0; // incrementer for planet names
    public int acetyleneIncrement = 0; // incrementer for planet names

    private Planet planetScript;
    private PlanetSlot planetSlotScript;
    private GameObject selected; // hold selected GameObject

	public GameObject microSkillTreeParent; // Reference to macro skill tree
    public GameObject microSkillTree; // Reference to prefab to create
	private GameObject mst; // Null object for logic
	public Button nextTurnButton;
	public List<GameObject> planets;
	public List<string> microSkillTreeNames; // To check for duplicates

    public bool simulate;
    private float startTime;
    private EventSystem es;
    private bool esgo = false; // handles un/locking of UI
    private int i;
    public bool canBuild;
    // gets all of the buttons
    Button[] buttons;

    public Transform planetsParent; // Reference to skill tree slots parent

	private State state;

    // Use this for initialization
    void Start()
    { 
		// Disable hints
		var hintsDisabled = true;

		// Pass state
		GAME_STATE = hintsDisabled ? -1 : 0;
		PREVIOUS_GAME_STATE = -1;
		state = gameObject.AddComponent<State>();
		state.SetupState ();

        nextTurn = GameObject.Find("End Turn Button").GetComponent<Button>();
        nextTurn.onClick.AddListener(Simulate);

        startLinkButton = GameObject.Find("Start Link Button").GetComponent<Button>();
        startLinkButton.onClick.AddListener(StartLink);

        linkButton = GameObject.Find("Link Button").GetComponent<Button>();
        linkButton.onClick.AddListener(Link);

        planets = new List<GameObject>();
        es = EventSystem.current;
        simulate = false;
        canBuild = true;
        buttons = planetsParent.GetComponentsInChildren<Button>();
        //        fail = true;
        roguePlanets = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
		// Update state in realtime with GameController
		state.UpdateState(PREVIOUS_GAME_STATE, GAME_STATE);

        // Prevent null object exception
        if (linkingText != null)
        {
            linkingText.text = "linking: " + linking + "\r\n" +
            "firstPlanet: " + firstPlanet + "\r\n" +
            "planet1: " + planet1 + "\r\n" +
            "planet2: " + planet2 + "\r\n" +
            "linkedAlready: " + linkedAlready + "\r\n" +
            "simulate: " + simulate;
        }

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

                // get first and second planets to link
                if (linking)
                {
                    if (firstPlanet)
                    {
                        planet1 = hit.collider.gameObject;
                        firstPlanet = false;
                    } else
                    {
                        planet2 = hit.collider.gameObject;
                    }
                    
                }
            }
        }

        if (planets.Contains(selected))
        {
            planetScript = selected.GetComponent<Planet>(); // get Planet script to access attributes

            // update UI
            if (planetScript.turnsToBuild < 1)
            {
                planetText.text = planetScript.name;
            }
			// Do not allow the player to click on the planet while it's rotating
			else if (!placing)
            {
                planetText.text = planetScript.turnsToBuild + " turns left to build: " + planetScript.name;
                // Enable text
                carbonText.enabled = true;
                nitrogenText.enabled = true;
                hydrogenText.enabled = true;
                // Enable parent text
                carbonText.transform.parent.GetComponent<Text>().enabled = true;
                nitrogenText.transform.parent.GetComponent<Text>().enabled = true;
                hydrogenText.transform.parent.GetComponent<Text>().enabled = true;
            }

            carbonText.text = planetScript.carbon.ToString();
            nitrogenText.text = planetScript.nitrogen.ToString();
            hydrogenText.text = planetScript.hydrogen.ToString();

			// What is the name of the game object to create
			var mstName = selected.name + " Skill Tree";

			// Open Skill Tree only if it hasn't been created yet
			if (!microSkillTreeNames.Contains(mstName) && GAME_STATE == -1 || (GAME_STATE >= Constants.TURN_3_TECH_TREE))
            {
				// Upon creation, add it to the list of unique skill trees
				microSkillTreeNames.Add (mstName);

				// Create a new micro skill tree
				//Debug.Log("Creating micro skill tree for " + selected.name);
				mst = Instantiate(microSkillTree) as GameObject;
				mst.name = mstName;

				// Associate the game object with its skill tree
				mst.GetComponent<TechnologySkillTree>().planetScript = planetScript;

				// Make micro skill tree a child object of parent
				mst.transform.SetParent(microSkillTreeParent.transform);
				mst.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
				mst.transform.localPosition = new Vector3 (350.0f, -50.0f, 0.0f);
				mst.SetActive(true);
				// Access the planet's script and set title
				mst.transform.Find("Title").Find("Title Text").GetComponent<Text>().text = mstName;
            }

            // Change game state when selecting Carbon 1
            if (GAME_STATE == Constants.TURN_3_TECH_TREE && selected.name == "Carbon 1")
            {
                GAME_STATE = Constants.TURN_3_TECH_SLOT;
            }
            // There can only be two planets at this state, so clicking Carbon 1 again shouldn't progress the game
            else if (GAME_STATE == Constants.TURN_3_TECH_TREE_2 && selected.name != "Carbon 1")
            {
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
            // Disable text
            carbonText.enabled = false;
            nitrogenText.enabled = false;
            hydrogenText.enabled = false;
            // Disable parent text
            carbonText.transform.parent.GetComponent<Text>().enabled = false;
            nitrogenText.transform.parent.GetComponent<Text>().enabled = false;
            hydrogenText.transform.parent.GetComponent<Text>().enabled = false;
        }

        if (simulate)
        {
            foreach (Button button in buttons)
            {
                button.interactable = false;
            }
        }
        else
        {
            if (!canBuild)
            {
                foreach (Button button in buttons)
                {
                    button.interactable = false;
                }
            }
            if (carbonIncrement > 0)
            {
                nextTurn.interactable = true;
            }
        }

        if (canBuild)
        {
            //Debug.Log(carbonIncrement);
            foreach (Button button in buttons)
            {
                // -- Unlock Tier 1 planets
                // Carbon is always unlocked
                if (button.name == "Carbon")
                {
                    button.interactable = true;
                }
                // -- Unlock Tier 2 planets
                // Silicon, Ammonia, Methane require Carbon to unlock
                if (carbonIncrement > 0)
                {
                    if (button.name == "Silicon" || button.name == "Ammonia" || button.name == "Methane")
                    {
                        button.interactable = true;
                    }
                }
                // -- Unlock Tier 3 planets
                // Germanium requires Silicon to unlock
                if (siliconIncrement > 0)
                {
                    if (button.name == "Germanium")
                    {
                        button.interactable = true;
                    }
                }
                // Acetylene requires Ammonia or Germanium to unlock
                if (ammoniaIncrement > 0 || germaniumIncrement > 0)
                {
                    if (button.name == "Acetylene")
                    {
                        button.interactable = true;
                    }
                }
            }
        }
        else
        {
            //foreach (Button button in buttons)
            //{
            //    button.interactable = false;
            //}
            //nextTurn.interactable = true;
        }


    }

    void StartLink()
    {
        if (!linking)
        {
            linking = true;
            firstPlanet = true;
        } else
        {
            linking = false;
            firstPlanet = false;
        }
    }

    void Link()
    {
        firstPlanetScript = planet1.GetComponent<Planet>();
        secondPlanetScript = planet2.GetComponent<Planet>();
        //Debug.Log("Link Button Clicked");
        foreach (var link in firstPlanetScript.linkedWith)
        {
            if (link == planet2)
            {
                Debug.Log("Already linked");
                linkedAlready = true;
            }
        }

        if (!linkedAlready)
        {
            if (fail)
            {
                //Debug.Log("Failed Link");
                // instantiate rogue planet with same attributes as planet2
                GameObject rogueObject = Instantiate(roguePrefab, planet2.transform.position, planet2.transform.localRotation, GameObject.Find("Sun").transform);
                planets.Add(rogueObject); // add to Planets List
                roguePlanets.Add(rogueObject); // add to roguePlanets list
                Rogue rogueScript = rogueObject.GetComponent<Rogue>(); // get Planet script to access attributes
                rogueScript.planetPlaced = true;
                rogueScript.orbitProgress = secondPlanetScript.orbitProgress; // restore original orbitProgress
                // restore segments
                rogueScript.segments = 36;
                // restore original orbitPath
                rogueScript.orbitPath.xAxis = secondPlanetScript.orbitPath.xAxis;
                rogueScript.orbitPath.yAxis = secondPlanetScript.orbitPath.yAxis;
                rogueScript.dominatedPlanet = planet1;
                // remove planet2 from planet1's linkedWith List
                firstPlanetScript.linkedWith.Remove(planet2.GetComponent<Planet>());
                // add rogueObject to planet1's linkedWith List
                firstPlanetScript.linkedWith.Add(rogueObject.GetComponent<Planet>());
                planets.Remove(planet2); // remove from Planets List
                Destroy(planet2);

            }
            else
            {
                firstPlanetScript.linkedWith.Add(planet2.GetComponent<Planet>());
                secondPlanetScript.linkedWith.Add(planet1.GetComponent<Planet>());
            }

        }

        linking = false;
        firstPlanet = false;
        simulate = false;
    }


    string name = "";

    void Simulate()
    {
        simulate = true;
        canBuild = true;
        //startTime = Time.time;
        //es.enabled = false;
        //esgo = true;

        foreach (var planet in planets)
        {
            if (planet.active)
            {
                planetScript = planet.GetComponent<Planet>();
                //print(planetScript.name);
                planetScript.turnsToBuild--;
                if (planetScript.turnsToBuild > 0)
                {
                    canBuild = false;
                }
                planetScript.StartCoroutine(planetScript.AnimateOrbit(1));

            }
        }

        foreach (var roguePlanet in roguePlanets)
        {
            Rogue rogueScript = roguePlanet.GetComponent<Rogue>();
            if (rogueScript.dominatedPlanets.Count == 0)
            {
                rogueScript.Steal(1, 1, 1);
            }
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
}

