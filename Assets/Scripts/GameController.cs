using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    // states
    private State state;
    public int GAME_STATE; // Control which hint indicator is active
    public int PREVIOUS_GAME_STATE; // Done to improve performance

    // turn
    public Button nextTurn;
    public bool simulate;
    public bool canBuild;
    public Transform planetsParent; // Reference to skill tree slots parent
    private Button[] planetButtons;

    // linking 
    public List<GameObject> planets;
    public Button startLinkButton;
    public Button linkButton;
    // flags    
    public bool placing;
    public bool linking; // tracks linking state
    public bool firstPlanet; // tracks which planet about to link
    public bool linkedAlready = false;
    // scripts
    private Planet firstPlanetScript;
    private Planet secondPlanetScript;
    private Planet planetScript;
    private PlanetSlot planetSlotScript;
    // GameObjects
    public GameObject planet1;
    public GameObject planet2;
    private GameObject selected; // hold selected GameObject
                                 // fail
    public bool fail;
    public GameObject roguePrefab;
    private List<GameObject> roguePlanets;

    // micro skill tree
    public GameObject microSkillTreeParent; // Reference to macro skill tree
    public GameObject microSkillTree; // Reference to prefab to create
    private GameObject mst; // Null object for logic
    public List<string> microSkillTreeNames; // To check for duplicates

    public GameObject notBuiltTooltip;
    private int notBuiltTooltipTimer;

    // texts
    public Text linkingText;
    public Text planetText;
    public Text carbonText;
    public Text nitrogenText;
    public Text hydrogenText;

    // track how many of each planet built
    public int carbonIncrement = 0;
    public int siliconIncrement = 0;
    public int ammoniaIncrement = 0;
    public int methaneIncrement = 0;
    public int germaniumIncrement = 0;
    public int acetyleneIncrement = 0;
    public int rogueIncrement = 0;
    public bool storm = false;
    private int count = 0;
    private int failtime = 0;
    private GUIStyle guiStyle = new GUIStyle();
    public bool linksuccessful = false;
    private int linktime = 0;
    // Use this for initialization
    void Start()
    {
        // states
        // Disable hints
        var hintsDisabled = true;

        // Pass state
        GAME_STATE = hintsDisabled ? -1 : 0;
        PREVIOUS_GAME_STATE = -1;
        state = gameObject.AddComponent<State>();
        state.SetupState();

        // planetButtons
        nextTurn = GameObject.Find("End Turn Button").GetComponent<Button>();
        nextTurn.onClick.AddListener(Simulate);

        startLinkButton = GameObject.Find("Start Link Button").GetComponent<Button>();
        startLinkButton.onClick.AddListener(StartLink);

        linkButton = GameObject.Find("Link Button").GetComponent<Button>();
        linkButton.onClick.AddListener(Link);

        planetButtons = planetsParent.GetComponentsInChildren<Button>();

        // initialize lists
        planets = new List<GameObject>();
        roguePlanets = new List<GameObject>();

        // initialize flags
        simulate = false;
        canBuild = true;

        ResetLinking();
    }

    // Update is called once per frame
    void Update()
    {
        if (linksuccessful)
        {
            linktime++;
            if (linktime > 60)
            {
                linksuccessful = false;
            }
        }
        if (fail)
        {
            failtime++;
            if (failtime > 60)
            {
                fail = false;
            }
        }
        if (storm)
        {
            count++;
        }
        // Update state in realtime with GameController
        state.UpdateState(PREVIOUS_GAME_STATE, GAME_STATE);

        // Prevent null object exception
        if (linkingText != null)
        {
            //linkingText.text = "linking: " + linking + "\r\n" +
            //"firstPlanet: " + firstPlanet + "\r\n" +
            //"planet1: " + planet1 + "\r\n" +
            //"planet2: " + planet2 + "\r\n" +
            //"simulate: " + simulate;

            linkingText.text = "planet1: " + planet1 + "\r\n" +
            "planet2: " + planet2;
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
                    // can only select built non-rogue planets to link with other built non-rogue planets
                    // check if is non-rogue and has Planet script
                    if (!selected.CompareTag("Rogue") && selected.GetComponent("Planet") as Planet != null)
                    {
                        if (selected.GetComponent<Planet>().turnsToBuild < 1) // check if is built 
                        {
                            if (firstPlanet)
                            {
                                planet1 = hit.collider.gameObject;
                                firstPlanetScript = planet1.GetComponent<Planet>();
                                firstPlanet = false;
                            }
                            else
                            {
                                // if selected is not planet 1
                                if (selected != planet1)
                                {
                                    foreach (var link in firstPlanetScript.linkedWith)
                                    {
                                        if (link == selected.GetComponent<Planet>())
                                        {
                                            //Debug.Log("Already linked");
                                            linkedAlready = true;
                                        }
                                    }
                                    if (!linkedAlready)
                                    {
                                        planet2 = hit.collider.gameObject;
                                        secondPlanetScript = planet2.GetComponent<Planet>();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // if linking, reset
        if (linking && Input.GetKeyDown(KeyCode.Escape))
        {
            ResetLinking();
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
            if (planetScript.turnsToBuild < 1)
            { // check if is built 

                notBuiltTooltip.SetActive(false);

                if (!linking && !placing && Input.GetMouseButtonUp(0) && !microSkillTreeNames.Contains(mstName) && GAME_STATE == -1 || (GAME_STATE >= Constants.TURN_3_TECH_TREE))
                {
                    // Create a new micro skill tree
                    //Debug.Log("Creating micro skill tree for " + selected.name);
                    mst = Instantiate(microSkillTree) as GameObject;

                    // Upon creation, add it to the list of unique skill trees
                    microSkillTreeNames.Add(mstName);

                    // Name the panel
                    mst.name = mstName;

                    // Associate the game object with its skill tree
                    mst.GetComponent<TechnologySkillTree>().planetScript = planetScript;

                    // Make micro skill tree a child object of parent
                    mst.transform.SetParent(microSkillTreeParent.transform);
                    mst.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                    mst.transform.localPosition = new Vector3(350.0f, -50.0f, 0.0f);
                    mst.SetActive(true);
                    // Access the planet's script and set title
                    mst.transform.Find("Title").Find("Title Text").GetComponent<Text>().text = mstName;
                }
                // Open if the panel has been created, but is disabled
                else if (!linking && !placing && Input.GetMouseButtonUp(0))
                {
                    // Bad programming to enable nested inactive game object
                    Transform[] ts = GameObject.Find("Micro Skill Tree Parent").transform.GetComponentsInChildren<Transform>(true); // bool includeInactive = true 
                    foreach (Transform t in ts)
                    {
                        //Debug.Log ("Found " + fromGameObject.name + "'s " + t.gameObject.name);
                        if (t.gameObject.name == mstName)
                        {
                            // Toggle skill tree on and off by clicking the planet
                            //t.gameObject.SetActive(true);
                            t.gameObject.SetActive(true);
                        }
                    }
                }
            }
            else
            {
                notBuiltTooltip.SetActive(true);
                notBuiltTooltipTimer++;
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
            notBuiltTooltip.SetActive(false);

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
    }

    void StartLink()
    {
        if (!linking)
        {
            linking = true;
            firstPlanet = true;
        }
        else
        {
            linking = false;
            firstPlanet = false;
        }
    }

    void Link()
    {
        // if both variables are set
        if (planet1 != null && planet2 != null)
        {
            // if the other planet is already been linked, no chance to fail the link
            if (secondPlanetScript.linkedWith.Count > 0)
            {
                fail = false;
            }
            else // otherwise chance to fail
            {
                CalculateFail();
            }

            if (!linkedAlready)
            {
                // if fail
                if (fail)
                {
                    //Debug.Log("Failed Link");
                    // instantiate rogue planet with same attributes as planet2
                    GameObject rogueObject = Instantiate(roguePrefab, planet2.transform.position, planet2.transform.localRotation, GameObject.Find("Sun").transform);
                    rogueIncrement++;
                    rogueObject.name = "Rogue " + rogueIncrement;
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
                    rogueScript.linkedWith.Add(planet1.GetComponent<Planet>());
                    // remove planet2 from planet1's linkedWith List
                    firstPlanetScript.linkedWith.Remove(planet2.GetComponent<Planet>());
                    // add rogueObject to planet1's linkedWith List
                    firstPlanetScript.linkedWith.Add(rogueObject.GetComponent<Planet>());
                    planets.Remove(planet2); // remove from Planets List
                    Destroy(planet2);

                }
                else
                {
                    linksuccessful = true;
                    linktime = 0;
                    firstPlanetScript.linkedWith.Add(planet2.GetComponent<Planet>());
                    secondPlanetScript.linkedWith.Add(planet1.GetComponent<Planet>());
                }

            }
            ResetLinking();

        }
    }

    private void ResetLinking()
    {
        linking = false;
        firstPlanet = false;
        simulate = false;
        planet1 = null;
        planet2 = null;
    }

    private void CalculateFail()
    {
        //chance to fail
        int difftier = Mathf.Abs(firstPlanetScript.tier - secondPlanetScript.tier);
        //        		Debug.Log(difftier);
        if (difftier == 0)
        {
            int chance = Random.Range(0, 10);
            //            			Debug.Log (chance);
            if (chance == 0)
            {
                fail = true;
                failtime = 0;
            }
        }
        else if (difftier == 1)
        {
            int chance = Random.Range(0, 10);
            //            			Debug.Log (chance);
            if (chance < 2)
            {
                fail = true;
                failtime = 0;
            }
        }
        else if (difftier == 2)
        {
            int chance = Random.Range(0, 10);
            //            			Debug.Log (chance);
            if (chance < 3)
            {
                fail = true;
                failtime = 0;
            }
        }
        else if (difftier > 2)
        {   //this one for test it work 
            int chance = Random.Range(0, 10);
            //            			Debug.Log (chance);
            if (chance < 10)
            {
                fail = true;
                failtime = 0;
            }
        }
        else
        {
            fail = false;
        }
    }


    //chance to storm
    private void culculateStorm()
    {
        int chance = Random.Range(0, 10);
        //			Debug.Log (chance);
        if (chance < 2)
        {
            storm = true;
            count = 0;

        }
        else
        {
            storm = false;
        }
        //		Debug.Log (chance);
    }
    void OnGUI()
    {
        guiStyle.fontSize = 30;
        guiStyle.normal.textColor = Color.white;
        guiStyle.alignment = TextAnchor.MiddleCenter;
        if (storm)
        {
            guiStyle.normal.textColor = Color.cyan;
        }
        if (linksuccessful == true && linktime < 120)
        {
            GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 300, 300, 50), "Successfully linked. \n Linked planets will now trade resources among each other.", guiStyle);
        }
        if (fail == true && failtime < 120)
        {
            GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 300, 300, 50), "Failed to link. \n Planet has gone rogue. It will now steal some resources.", guiStyle);
        }
        if (storm == true && count < 120)
        {
            GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 300, 300, 50), "There was a storm. \n Resource collection rate decreased by half.", guiStyle);
        }
    }
    void Simulate()
    {
        ResetLinking();
        simulate = true;
        canBuild = true;

        ToggleInteractability(false);

        culculateStorm();

        foreach (var planet in planets)
        {
            if (storm)
            {
                planetScript = planet.GetComponent<Planet>();
                planetScript.addCarbon = planetScript.halfaddCarbon;
                planetScript.addNitrogen = planetScript.halfaddNitrogen;
                planetScript.addHydrogen = planetScript.halfaddHydrogen;

            }
            else if (!storm)
            {
                planetScript = planet.GetComponent<Planet>();
                planetScript.addCarbon = planetScript.OriginaddCarbon;
                planetScript.addNitrogen = planetScript.OriginaddNitrogen;
                planetScript.addHydrogen = planetScript.OriginaddHydrogen;
            }
            if (planet.activeSelf)
            {
                planetScript = planet.GetComponent<Planet>();
                planetScript.turnsToBuild--;
                if (planetScript.turnsToBuild > 0)
                {
                    canBuild = false;
                }
                planetScript.StartCoroutine(planetScript.AnimateOrbit(1));

            }
        }

        // rogue planet steal from dominatedPlanets
        foreach (var roguePlanet in roguePlanets)
        {
            Rogue rogueScript = roguePlanet.GetComponent<Rogue>();
            rogueScript.Steal(1, 1, 1);
        }
    }

    public void ToggleInteractability(bool canInteract)
    {
        if (canInteract && canBuild)
        {
            foreach (Button button in planetButtons)
            {
                //--Unlock Tier 1 planets
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
            // Unlock technologies
            Transform[] ts = GameObject.Find("Micro Skill Tree Parent").transform.GetComponentsInChildren<Transform>(true); // bool includeInactive = true 
            foreach (Transform t in ts)
            {
                if (microSkillTreeNames.Contains(t.gameObject.name))
                {
                    var tech = t.gameObject.GetComponent<TechnologySkillTree>();
                    tech.Unlock();
                }
            }
        }
    }
}

