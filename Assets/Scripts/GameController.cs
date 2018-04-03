using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    // states
    private State state;
    public int GAME_STATE; // Control which hint indicator is active
    public int PREVIOUS_GAME_STATE; // Done to improve performance

    // turn
    public Button playButton;
    public bool simulate;
    public bool canBuild;
    public Transform planetsParent; // Reference to skill tree slots parent
    private Button[] planetButtons;

    public RectTransform rightPlanetaryPanel; // New UI


    public bool attacking;
    public Button AttackButton;

    // linking 
    public List<GameObject> planets;

    public Button linkButton;
    // flags    
    public bool placing;
    public bool linking; // tracks linking state
    public bool firstPlanet; // tracks which planet about to link
    public bool linkedAlready = false;
    // scripts
    private Planet firstPlanetScript;
    private Planet secondPlanetScript;
    public Planet planetScript;
    private PlanetSlot planetSlotScript;
    // GameObjects
    public GameObject planet1;
    public GameObject planet2;
    public GameObject selected; // hold selected GameObject
                                // fail
    public bool fail;
    public int turn;
    public GameObject roguePrefab;
    public List<GameObject> roguePlanets;

    // micro skill tree
    public GameObject microSkillTreeParent; // Reference to macro skill tree
    public GameObject microSkillTree; // Reference to prefab to create
    private GameObject mst; // Null object for logic
    public List<string> microSkillTreeNames; // To check for duplicates

    public Button tech1;
    public Button tech2;
    public Button tech3;
    public Button tech4;
    public Button tech5;


    //public GameObject notBuiltTooltip;
    private int notBuiltTooltipTimer;

    // texts
    public Text linkingText;
    public Text planetText;
    public Text carbonText;
    public Text nitrogenText;
    public Text hydrogenText;

    // track how many of each planet built
    public int stoneIncrement = 0;
    public int gasIncrement = 0;
    public int waterIncrement = 0;
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

    // planet placing variables
    public Transform sun; // Reference to planet objects parent
    public GameObject stone; // Reference to prefab carbon to generate
    public GameObject water; // Reference to prefab carbon to generate
    public GameObject gas; // Reference to prefab carbon to generate
    public GameObject go; // New planet game object
    private Planet p; // Access new planet script
    public bool planetPlaced; // Flag for drawing planet orbit in realtime

    public UIController ui;

    // planetary ui buttons
    public Button stoneButton;
    public Button waterButton;
    public Button gasButton;

    public int level;

    // missions
    public Missions m;

    bool buildingActive;

    public GameObject shot;


    // missions
    public GameObject missionPanel;
    public GameObject genericMissionPanel;
    private int missionIncrement;

    private Scene currentScene;

	private float camerasize=600;
    // log
    //public GameObject log;
    public Log l;


    public ConfirmationPanel cp;

	public ChoosePanel cp2;
	public int choosecase = 0;

	public string summary="";
	public bool showsummary=false;

	public GameObject leftpanel;
	public GameObject rightpanel;
    // Use this for initialization
    void Start()
    {
        // when the script has started, then assign these scripts
        //ui = GameObject.Find("Canvas").GetComponent<UIController>();
        //m = GameObject.Find("Missions").GetComponent<Missions>();
        //cp = GameObject.Find("Confirmation Panel").GetComponent<ConfirmationPanel>();
        //l = log.GetComponent<Log>();

        

        currentScene = SceneManager.GetActiveScene();

        switch (currentScene.name)
        {
            case "Main":
                level = 1;
                break;
            case "Level2":
                level = 2;
                break;
            case "Final":
                level = 3;
                break;
            default:
                level = 0;
                break;
        }

        m.CPShownAtStartOfLevel();

        //l.ToggleLog();

        turn = 1;

        missionIncrement = 0;

        // states
        // Disable hints
        var hintsDisabled = false;

        // Pass state
        GAME_STATE = hintsDisabled ? -1 : 0;
        PREVIOUS_GAME_STATE = -1;
        state = gameObject.AddComponent<State>();
        state.SetupState();

        // planetButtons
        //playButton = GameObject.Find("End Turn Button").GetComponent<Button>();
        //playButton.onClick.AddListener(Simulate);

        linkButton = ui.leftLinkToButton.GetComponent<Button>();
        linkButton.onClick.AddListener(StartLink);

        AttackButton = ui.leftAttackToButton.GetComponent<Button>();
        AttackButton.onClick.AddListener(StartAttack);

        tech1 = ui.leftTechnology1Button.GetComponent<Button>();
        tech1.onClick.AddListener(settech1);

        tech2 = ui.leftTechnology2Button.GetComponent<Button>();
        tech2.onClick.AddListener(settech2);

        tech3 = ui.leftTechnology3Button.GetComponent<Button>();
        tech3.onClick.AddListener(settech3);

        tech4 = ui.leftTechnology4Button.GetComponent<Button>();
        tech4.onClick.AddListener(settech4);

        tech5 = ui.leftTechnology5Button.GetComponent<Button>();
        tech5.onClick.AddListener(settech5);



        //planetButtons = planetsParent.GetComponentsInChildren<Button>();

        if (level != 0)
        {
            // initialize lists
            planets = new List<GameObject>();
            roguePlanets = new List<GameObject>();
        }
        
        ui.SetPhase("Planning");

        // initialize flags
        simulate = false;
        canBuild = true;

        stoneButton.interactable = true;
        waterButton.interactable = false;
        gasButton.interactable = false;

        ResetLinking();
    }

    public void AddMissionsToUI(GameObject mission)
    {
        m.missions.Add(mission);
        GameObject go = Instantiate(genericMissionPanel, missionPanel.transform); // create Mission Panel for mission
        GameObject ms = Instantiate(mission, go.transform); // used for Missions.cs to find the button to update appearance

        // set all buttons to incomplete red 
        Button button = go.transform.GetChild(0).GetComponent<Button>(); // get the particular mission button
        ColorBlock cb = button.GetComponent<Button>().colors;
        cb.disabledColor = new Color(0.894f, 0.266f, 0.266f); // set the button color to same green as play button
        button.colors = cb;

        Text panelText = go.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>(); // get Text of Mission Panel

        panelText.text = mission.GetComponent<Mission>().missionName; // change text of Mission Panel to missionName
        //missionIncrement++;
        //panelText.text = "Mission " + missionIncrement;


        // Set tooltip for Mission Panel
        GameObject goButton = go.transform.GetChild(0).gameObject; // get Mission Panel button
        string tooltipText = "";
        tooltipText += "<b>" + mission.GetComponent<Mission>().missionName + "</b>"; // add mission name to tooltip text

        if (mission.GetComponent<Mission>().missionDescription != "") // if there is a reward, add onto tooltip
        {
            tooltipText += ";;" + mission.GetComponent<Mission>().missionDescription;
        }

        if (mission.GetComponent<Mission>().missionReward != "") // if there is a reward, add onto tooltip
        {
            tooltipText += ";;" + "<b>Reward:</b> " + mission.GetComponent<Mission>().missionReward;
        }
        ui.SetTooltip((RectTransform)goButton.transform, tooltipText); // set tooltip with final text 
    }

    private void settech1()
    {
        if (planetScript != null)
        {
            if (planetScript.iftech1 == 0)
            {
                planetScript.iftech1 = 1;
            }
        }
    }
    private void settech2()
    {
        if (planetScript != null)
        {
            if (planetScript.iftech2 == 0)
            {
                planetScript.iftech2 = 1;
            }
        }
    }
    private void settech3()
    {
        if (planetScript != null)
        {
            if (planetScript.iftech3 == 0)
            {
                planetScript.iftech3 = 1;
            }
        }
    }
    private void settech4()
    {
        if (planetScript != null)
        {
            if (planetScript.iftech4 == 0)
            {
                planetScript.iftech4 = 1;
            }
        }
    }
    private void settech5()
    {
        if (planetScript != null)
        {
            if (planetScript.iftech5 == 0)
            {
                planetScript.iftech5 = 1;
            }
        }
    }
    public void AddTurn()
    {
        turn++;
        //Debug.Log(planets.Count);
        //Debug.Log("Turn " + turn);
        ui.SetTurn(turn);
    }

    // Update is called once per frame
    void Update()
    {
		if (simulate == true) {
//			ui.gameObject.SetActive (false);

			leftpanel.SetActive(false);
			rightpanel.SetActive(false);
			Camera.main.orthographicSize = 310;
		} else {
//			ui.gameObject.SetActive (true);
			leftpanel.SetActive(true);
			rightpanel.SetActive(true);
			Camera.main.orthographicSize = camerasize;
		}

		if (showsummary == true) {
			cp.ShowPanel("Summary", summary);
			showsummary = false;
		}
		//if click nobutton
		if  (cp2.yesorno == -1) {
			//try to link 
			if (choosecase == 1) {
					linking = false;
					firstPlanet = false;
					linkedAlready = false;
					planet1 = null;
					planet2 = null;
					choosecase = 0;
					cp2.yesorno = 0;
			}

			//try to attack
			if (choosecase == 4) {
				attacking = false;
				firstPlanet = false;

				planet1 = null;
				planet2 = null;
				choosecase = 0;
				cp2.yesorno = 0;
			}
			cp2.yesorno = 0;
			choosecase = 0;
		}

		//if click yesbutton
		if (cp2.yesorno == 1) {
			////try to cancel link
			if (choosecase == 2) {
	
				linking = false;
				firstPlanet = false;
				linkedAlready = false;
				planet1 = null;
				planet2 = null;
				choosecase = 0;
				cp2.yesorno = 0;
			}
		

			//try to cancel attacking
			if (choosecase == 3) {
		
				attacking = false;
				firstPlanet = false;

				planet1 = null;
				planet2 = null;
				choosecase = 0;
				cp2.yesorno = 0;
			
			}
			cp2.yesorno=0;
			choosecase = 0;
		}
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (playButton.interactable)
            {
                Simulate();
            }
            
        }
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (attacking) {
				cp2.ShowPanel("Cancel Attacking", "Do you want to cancel the attacking?");
				choosecase = 3;
			}
			if (linking) {
				cp2.ShowPanel("Cancel Linking", "Do you want to cancel the linking?");
				choosecase = 2;
			}

		}


        if (shot != null)
        {
            shot.transform.position = Vector3.MoveTowards(shot.transform.position, planet2.transform.position, 120 * Time.deltaTime);
            if (shot.transform.position == planet2.transform.position)
            {
                secondPlanetScript.health -= 50;
                if (secondPlanetScript.health <= 0)
                {
                    secondPlanetScript.die = true;
                    m.CheckMissions(m.missions);

                }
                attacking = false;
                firstPlanet = false;

                planet1 = null;
                planet2 = null;
                Destroy(shot);
            }
        }
        // toggle log
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            l.ToggleLog();
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            l.ToggleLog();
        }


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
            linkingText.text = "linking: " + linking + "\r\n" +
            "firstPlanet: " + firstPlanet + "\r\n" +
            "planet1: " + planet1 + "\r\n" +
            "planet2: " + planet2 + "\r\n" +
            "simulate: " + simulate;

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

                m.CheckMissions(m.missions);

                // get first and second planets to link
                if (linking)
                {
                    // can only select built non-rogue planets to link with other built non-rogue planets
                    // check if is non-rogue and has Planet script
                    if (selected != planet1 && !selected.CompareTag("Rogue") && selected.GetComponent("Planet") as Planet != null)
                    {

                        if (selected.GetComponent<Planet>().turnsToBuild > 1 && !selected.GetComponent<Planet>().iflinkactive)
                        {
                            cp.ShowPanel("Planet Not Built", selected.name + " must finish building before linking.");
                        }

                        if (selected.GetComponent<Planet>().turnsToBuild < 1 && !selected.GetComponent<Planet>().iflinkactive)
                        {
                            cp.ShowPanel("Link Cannot Be Assigned", selected.name + " must also have Interplanetary Linking learned");
                        }

                        if (selected.GetComponent<Planet>().turnsToBuild < 1 && selected.GetComponent<Planet>().iflinkactive)
                        { // check if is built 


                            foreach (var link in firstPlanetScript.linkedWith)
                            {
                                if (link == selected.GetComponent<Planet>())
                                {
                                    //Debug.Log("Already linked");
                                    linkedAlready = true;
									cp.ShowPanel("Link Cannot Be Assigned", selected.name + " have already linked");
                                }
                            }
                            if (!linkedAlready)
                            {
                                planet2 = hit.collider.gameObject;
                                secondPlanetScript = planet2.GetComponent<Planet>();
                                // if both variables are set

								cp2.ShowPanel("Assigning Linking", "Do you want to link "+ planet1.name + " and " + planet2.name+"?");
								choosecase = 1;
                            }
                        }
                    }
                }
                if (attacking)
                {
                    // can only select built non-rogue planets to link with other built non-rogue planets
                    // check if is non-rogue and has Planet script

                    // if selected is not planet 1
					if (selected != planet1 && !selected.CompareTag("Rogue"))
					{
						//									


				
						if (cp2.yesorno == 0 && choosecase == 0) {
							cp.ShowPanel ("Attacking cannot be assigned", "You can only attack rogue planet. ");
						}

					}

                    if (selected != planet1 && selected.CompareTag("Rogue"))
                    {
                        //									

                        planet2 = hit.collider.gameObject;
                        secondPlanetScript = planet2.GetComponent<Planet>();
                        // if both variables are set

						cp2.ShowPanel("Assigning Attacking", "Do you want to attack "+ planet2.name+"?");
						choosecase = 4;

                    }



                }
            }
        }

//        // if linking, reset
//        if (linking && Input.GetKeyDown(KeyCode.Escape))
//        {
//            ResetLinking();
//        }
//
//        if (attacking && Input.GetKeyDown(KeyCode.Escape))
//        {
//            attacking = false;
//            firstPlanet = false;
//            simulate = false;
//            planet1 = null;
//            planet2 = null;
//        }
//


        if (planets.Contains(selected) || planetScript != null)
        {

			if (!attacking && !linking && !placing && selected.GetComponent("Planet"))
            {
                planetScript = selected.GetComponent<Planet>(); // get Planet script to access attributes
            }
            if (ui.selectedPlanet == null)
            {
                playButton.interactable = true;
            }



            // Prevent SetSelectedPlanet from being called 60 times/second,
            // Only on new selection
            if (ui.selectedPlanet != planetScript)
            {
                ui.SetSelectedPlanet(planetScript);
            }
            ui.leftTechnologyPanel.ForceUpdateRectTransforms();

            if (planetScript.iflinkactive)
            {

                linkButton.gameObject.SetActive(true);

                linkButton.interactable = true;
            }
            else
            {

                linkButton.gameObject.SetActive(false);
            }

            if (planetScript.ifattackactive)
            {

                AttackButton.gameObject.SetActive(true);
                AttackButton.interactable = true;
            }
            else
            {

                AttackButton.gameObject.SetActive(false);
            }


            if (!linking && !placing && Input.GetMouseButtonUp(0))
            {

                if (planetScript.iftech1 == 1)
                {

                    planetScript.iftech1 = 2;
                }
                if (planetScript.iftech2 == 1)
                {

                    planetScript.iftech2 = 2;
                }
                if (planetScript.iftech3 == 1)
                {

                    planetScript.iftech3 = 2;
                }
                if (planetScript.iftech4 == 1)
                {

                    planetScript.iftech4 = 2;
                }
                if (planetScript.iftech5 == 1)
                {

                    planetScript.iftech5 = 2;
                }
            }
            else
            {
                //notBuiltTooltip.SetActive(true);
                notBuiltTooltipTimer++;
            }

            // update UI
            if (planetScript.turnsToBuild < 1)
            {
                //planetText.text = planetScript.name;
            }
            // Do not allow the player to click on the planet while it's rotating
            else if (!placing)
            {
                ui.SetPhase("Ready To Simulate");
                //planetText.text = planetScript.turnsToBuild + " turns left to build: " + planetScript.name;
                // Enable text
                // carbonText.enabled = true;
                // nitrogenText.enabled = true;
                // hydrogenText.enabled = true;
                // // Enable parent text
                // carbonText.transform.parent.GetComponent<Text>().enabled = true;
                // nitrogenText.transform.parent.GetComponent<Text>().enabled = true;
                // hydrogenText.transform.parent.GetComponent<Text>().enabled = true;
            }

            // carbonText.text = planetScript.carbon.ToString();
            // nitrogenText.text = planetScript.nitrogen.ToString();
            // hydrogenText.text = planetScript.hydrogen.ToString();

            // What is the name of the game object to create
            var mstName = selected.name + " Skill Tree";

            // Make play button green after clicking planet for the first time


            tech1.interactable = false;
            tech2.interactable = false;
            tech3.interactable = false;
            tech4.interactable = false;
            tech5.interactable = false;



            if (planetScript.stone >= 10 && planetScript.iftech1 == 0)
            {


                tech1.interactable = true;

            }
            else if (planetScript.gas >= 5 && planetScript.water >= 5 && planetScript.stone >= 5 && planetScript.iftech1 == 4 && planetScript.iftech2 == 0)
            {

                tech2.interactable = true;

            }
            else if (planetScript.gas >= 15 && planetScript.water >= 10 && planetScript.iftech1 == 4 && planetScript.iftech2 == 4 && planetScript.iftech3 == 0)
            {

                tech3.interactable = true;

            }
            else if (planetScript.water >= 15 && planetScript.stone >= 15 && planetScript.iftech1 == 4 && planetScript.iftech2 == 4 && planetScript.iftech3 == 4 && planetScript.iftech4 == 0)
            {

                tech4.interactable = true;

            }
            else if (planetScript.gas >= 20 && planetScript.water >= 20 && planetScript.stone >= 20 && planetScript.iftech1 == 4 && planetScript.iftech2 == 4 && planetScript.iftech3 == 4 && planetScript.iftech4 == 4 && planetScript.iftech5 == 0)
            {

                tech5.interactable = true;

            }


            // Open Skill Tree only if it hasn't been created yet
            // check if is built 
            //                notBuiltTooltip.SetActive(false);



            // Create a new micro skill tree
            //Debug.Log("Creating micro skill tree for " + selected.name);
            //                     mst = Instantiate(microSkillTree) as GameObject;
            //
            //                     // Upon creation, add it to the list of unique skill trees
            //                     microSkillTreeNames.Add(mstName);
            //
            //                     // Name the panel
            //                     mst.name = mstName;
            //
            //                     // Associate the game object with its skill tree
            //                     mst.GetComponent<TechnologySkillTree>().planetScript = planetScript;
            //
            //                     // Make micro skill tree a child object of parent
            //                     mst.transform.SetParent(microSkillTreeParent.transform);
            //                     mst.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            //                     mst.transform.localPosition = new Vector3(350.0f, -50.0f, 0.0f);
            //                     mst.SetActive(true);
            //                     // Access the planet's script and set title
            //                     mst.transform.Find("Title").Find("Title Text").GetComponent<Text>().text = mstName;
            //                }
            // Open if the panel has been created, but is disabled
            //                else if (!linking && !placing && Input.GetMouseButtonUp(0))
            //                {
            //                     // Bad programming to enable nested inactive game object
            //					Transform[] ts = GameObject.Find("Technology 1 Panel").transform.GetComponentsInChildren<Transform>(true); // bool includeInactive = true 
            // //					Debug.Log(mstName);
            // 					foreach (Transform t in ts)
            //                     {
            // 						if (t.gameObject.name.Contains (" Skill Tree")) {
            // //							Debug.Log (t.gameObject.name);
            // 							//Debug.Log ("Found " + fromGameObject.name + "'s " + t.gameObject.name);
            // 							if (t.gameObject.name == mstName) {
            // 								// Toggle skill tree on and off by clicking the planet
            // 								//t.gameObject.SetActive(true);
            // 								t.gameObject.SetActive (true);
            //							} else if (t.gameObject.name == "Technology 1 Panel") {
            //							
            // 							} else {
            // 								t.gameObject.SetActive (false);
            // 							}
            // 						}
            //                     }
            //                }
            //            }

            // Change game state when selecting Carbon 1
            // if (GAME_STATE == Constants.TURN_3_TECH_TREE && selected.name == "Carbon 1")
            // {
            //     GAME_STATE = Constants.TURN_3_TECH_SLOT;
            // }
            // // There can only be two planets at this state, so clicking Carbon 1 again shouldn't progress the game
            // else if (GAME_STATE == Constants.TURN_3_TECH_TREE_2 && selected.name != "Carbon 1")
            // {
            //     GAME_STATE = Constants.TURN_3_TECH_SLOT_2;
            // }
        }
        else
        {
            //if (notBuiltTooltip != null)
            //{
            //    notBuiltTooltip.SetActive(false);
            //}

            // update UI when no planet is selected
            //planetText.text = "No Planet Selected";
            //carbonText.text = 0.ToString();
            //nitrogenText.text = 0.ToString();
            //hydrogenText.text = 0.ToString();
            // Disable text
            //carbonText.enabled = false;
            //nitrogenText.enabled = false;
            //hydrogenText.enabled = false;
            // Disable parent text
            //carbonText.transform.parent.GetComponent<Text>().enabled = false;
            //nitrogenText.transform.parent.GetComponent<Text>().enabled = false;
            //hydrogenText.transform.parent.GetComponent<Text>().enabled = false;
        }


        // When player is placing planet
        if (go != null && !planetPlaced)
        {
            // Update the global placing variable
            placing = true;
            //playButton.interactable = false;

            ui.SetPhase("Placing");

            // Update the game state
            if (GAME_STATE == Constants.LEARNERS_MISSION_2) state.DisableAllHints();

            // // Update the game state
            // if (GAME_STATE == Constants.TURN_1_PLANET_SLOT)
            // {
            //     GAME_STATE = Constants.TURN_1_PLACE_PLANET;
            // }

            // Calculate 3D mouse coordinates
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // if over UI element, cannot place planet
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                // Set final orbit position on mouse up
                if (!planetPlaced && Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit = new RaycastHit();
                    if (Physics.Raycast(ray, out hit))
                    {
                        // Only set orbit if clicking in-bounds
                        if (hit.collider.gameObject.name == "Orbit Plane")
                        {
                            // Allow player to end turn
                            //GameObject.Find("End Turn Button").GetComponent<Button>().interactable = true;

                            // // Update the game state
                            // if (GAME_STATE == Constants.TURN_1_PLACE_PLANET)
                            // {
                            //     GAME_STATE = Constants.TURN_1_END_TURN;
                            // }

                            // // Update the game state
                            // if (GAME_STATE == Constants.TURN_2_PLACE_PLANET)
                            // {
                            //     GAME_STATE = Constants.TURN_2_END_TURN;
                            // }

                            // only stops coroutine if it is running
                            if (p.placingCoroutineRunning)
                            {
                                p.StopCoroutine(p.placing);
                            }

                            //playButton.interactable = true;

                            if (ui.selectedPlanet == null)
                            {
                                // Uncomment to make player have to click on planet to open properties
                                //ui.SetNoPlanetSelected();

                                // Uncomment to open planet properties automatically
                                //Debug.Log("setting from gc");
                                planetScript = p;                                                                
                                ui.SetSelectedPlanet(planetScript); // populate left panel with data
                                ui.OpenLeftPanel(); // open the panel
                                playButton.interactable = true; // make play button interactable
                                GAME_STATE = Constants.LEARNERS_MISSION_3; // advance to mission 3
                            }
                            else
                            {
                                playButton.interactable = true;
                                //ui.UpdateSelectedPlanet();
                            }

                            planetPlaced = true;
                            canBuild = false;
                            ToggleInteractability(false);
                            //clickedSlot = null;

                            // Update the global placing variable
                            placing = false; // Must be false to let skill tree open
                                             //p.planetPlaced = true;
                                             //p.orbitActive = false;
                                             //this.GetComponent<Button>().interactable = false;
                                             //go = null;
                        }
                    }
                }

                // Update planet location with the mouse in realtime
                if (!planetPlaced)
                {
                    Plane hPlane = new Plane(Vector3.up, Vector3.zero);
                    float distance = 0;
                    if (hPlane.Raycast(ray, out distance))
                    {
                        RaycastHit hit = new RaycastHit();
                        if (Physics.Raycast(ray, out hit))
                        {
                            // Only set orbit if clicking in-bounds
                            if (hit.collider.gameObject.name == "Orbit Plane")
                            {
                                Vector3 location = ray.GetPoint(distance);

                                // Commented to prevent planet being placed under mouse
                                //go.transform.position = location;

                                // Simulate orbit path (absolute so the orbit direction doesn't change)
                                p.enabled = true;

                                //Debug.Log("(" + location.x + ", " + location.z + ")");

                                if (location.x > -100 && location.x < 100) location.x = 100;
                                if (location.z > -100 && location.z < 100) location.z = 100;

                                p.orbitPath.xAxis = (Mathf.Abs(location.x));
                                p.orbitPath.yAxis = (Mathf.Abs(location.z));

                                // float radiusX = location.x;
                                // if (radiusX < 100) radiusX = 100;
                                // float radiusZ = location.z;
                                // if (radiusZ < 100) radiusZ = 100;

                                //float scale = 1.0f;
                                //p.transform.localScale = new Vector3(scale, scale, scale);
                            }
                        }
                    }
                }
            }
        }

        // if placing, press esc to reset everything
        if (placing && Input.GetKeyDown(KeyCode.Escape))
        {

            if (go != null)
            {

                Debug.Log(go.name);
                // if button's name is Carbon
                if (go.name.Contains("Carbon"))
                {
                    // Create a new planet
                    //go = Instantiate(carbon) as GameObject;
                    // increment planet name
                    stoneIncrement--;
                    //go.name = "Carbon " + gc.carbonIncrement;
                    // Make planet a child object of the Sun
                    //go.transform.parent = sun.transform;
                    // Add planet to array of planets
                    planets.Remove(go);
                    Destroy(go);
                    ResetPlacing();
                    // Access the planet's script
                    //p = go.GetComponent<Carbon>();
                }

                // if button's name is Silicon
                if (go.name.Contains("Hydrogen"))
                {
                    // Create a new planet
                    //go = Instantiate(carbon) as GameObject;
                    // increment planet name
                    gasIncrement--;
                    //go.name = "Carbon " + gc.carbonIncrement;
                    // Make planet a child object of the Sun
                    //go.transform.parent = sun.transform;
                    // Add planet to array of planets
                    planets.Remove(go);
                    Destroy(go);
                    ResetPlacing();
                    // Access the planet's script
                    //p = go.GetComponent<Carbon>();
                }

                // if button's name is Methane
                if (go.name.Contains("Nitrogen"))
                {
                    // Create a new planet
                    //go = Instantiate(carbon) as GameObject;
                    // increment planet name
                    waterIncrement--;
                    //go.name = "Carbon " + gc.carbonIncrement;
                    // Make planet a child object of the Sun
                    //go.transform.parent = sun.transform;
                    // Add planet to array of planets
                    planets.Remove(go);
                    Destroy(go);
                    ResetPlacing();
                    // Access the planet's script
                    //p = go.GetComponent<Carbon>();
                }
            }

        }
    }

    public void placeCarbonPlanet()
    {
        //Debug.Log("Placing Carbon planet!");
        SetBuildingActive(false);

        // Create a new planet
        go = Instantiate(stone) as GameObject;
        // increment planet name
        stoneIncrement++;
        go.name = "Stone " + stoneIncrement;
        // Make planet a child object of the Sun
        go.transform.parent = sun.transform;
        // Add planet to array of planets
        planets.Add(go);
        // Access the planet's script
        p = go.GetComponent<Carbon>();
        p.orbitPath.xAxis = Random.Range(100f, 300f);
        p.orbitPath.yAxis = Random.Range(100f, 300f);
    }
    public void placeNitrogenPlanet()
    {
        //Debug.Log("Placing Nitrogen planet!");
        SetBuildingActive(false);

        // Create a new planet
        go = Instantiate(water) as GameObject;
        // increment planet name
        waterIncrement++;
        go.name = "Water " + waterIncrement;
        // Make planet a child object of the Sun
        go.transform.parent = sun.transform;
        // Add planet to array of planets
        planets.Add(go);
        // Access the planet's script
        p = go.GetComponent<Ammonia>();
        p.orbitPath.xAxis = Random.Range(100f, 300f);
        p.orbitPath.yAxis = Random.Range(100f, 300f);
    }
    public void placeHydrogenPlanet()
    {
        //Debug.Log("Placing Hydrogen planet!");
        SetBuildingActive(false);

        // Create a new planet
        go = Instantiate(gas) as GameObject;
        // increment planet name
        gasIncrement++;
        go.name = "Gas " + gasIncrement;
        // Make planet a child object of the Sun
        go.transform.parent = sun.transform;
        // Add planet to array of planets
        planets.Add(go);
        // Access the planet's script
        p = go.GetComponent<Silicon>();
        p.orbitPath.xAxis = Random.Range(100f, 300f);
        p.orbitPath.yAxis = Random.Range(100f, 300f);
    }

    public void SetBuildingActive(bool active)
    {
        Button[] _planetaryButtons = rightPlanetaryPanel.GetComponentsInChildren<Button>();
        if (canBuild)
        {
            stoneButton.interactable = active;

            if (stoneIncrement > 0)
            {
                waterButton.interactable = active;
            }
            if (waterIncrement > 0)
            {
                gasButton.interactable = active;
            }
        }
        // Only call once per planet per turn
        if (active && !buildingActive)
        {
            //AddTurn();
            buildingActive = true;
        }
    }

    void StartLink()
    {

        if (!linking)
        {
            firstPlanet = true;
            if (firstPlanet)
            {

                firstPlanetScript = planetScript;
                planet1 = firstPlanetScript.gameObject;
            }
            linking = true;


        }
        else
        {
            linking = false;
            firstPlanet = false;
        }
    }


    private void ResetLinking()
    {
        linking = false;
        firstPlanet = false;
        linkedAlready = false;
        //        simulate = false;
        planet1 = null;
        planet2 = null;
    }


    void StartAttack()
    {
        if (!attacking)
        {

            firstPlanet = true;
            if (firstPlanet)
            {

                firstPlanetScript = planetScript;
                planet1 = firstPlanetScript.gameObject;
            }
            attacking = true;
        }
        else
        {
            attacking = false;
            firstPlanet = false;
        }
    }





    private void CalculateFail()
    {
        //chance to fail
        int difftier = Mathf.Abs(firstPlanetScript.tier - secondPlanetScript.tier) - firstPlanetScript.addlinkchance - secondPlanetScript.addlinkchance;
        //        		Debug.Log(difftier);

        if (difftier < 0)
        {
            fail = false;
        }
        else if (difftier == 0)
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
            //GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 300, 300, 50), "Successfully linked. \n Linked planets will now trade resources among each other.", guiStyle);
        }
        if (fail == true && failtime < 120)
        {
            //GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 300, 300, 50), "Failed to link. \n Planet has gone rogue. It will now steal some resources.", guiStyle);
        }
        if (storm == true && count < 120)
        {
            //GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 300, 300, 50), "There was a storm. \n Resource collection rate decreased by half.", guiStyle);
        }
    }

    void ResetPlacing()
    {
        placing = false;
        //go = null;
        planetPlaced = false;
        ToggleInteractability(true);
        SetBuildingActive(true);
        //clickedSlot = null;
    }

    public void Simulate()
    {
        AddTurn();
		summary = "";
        ui.SetPhase("Simulating...");
		camerasize = Camera.main.orthographicSize;
        simulate = true;
        canBuild = true;
        buildingActive = false;

        if (level == 3)
        {
            foreach (var planet in planets)
            {
                if (planet.GetComponent("Rogue"))
                {
                    var rogueScript = planet.GetComponent<Rogue>();
                    rogueScript.Attack();
                }
            }
            culculateStorm();
        }

        // reset place planet
        if (go != null && planetPlaced)
        {
            go = null;
            planetPlaced = false;
        }

        playButton.interactable = false;

        ToggleInteractability(false);




        if (attacking)
        {
            if (planet1 != null && planet2 != null)
            {
				summary += planet1.GetComponent<Planet> ().planetname + " attacked " + planet2.GetComponent<Planet> ().planetname + " and " + planet2.GetComponent<Planet> ().planetname + " lose 50 hp.\n";
                shot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                shot.transform.localScale = new Vector3(8f, 8f, 8f);
                shot.transform.position = planet1.transform.position;


            }


        }
        if (linking)
        {


            if (planet1 != null && planet2 != null)
            {
                // if the other planet is already been linked, no chance to fail the link
                if (secondPlanetScript.linkedWith.Count > 0)
                {
                    fail = false;
                }
                else
                { // otherwise chance to fail
                    if (level != 1)
                    {
                        CalculateFail();
                    }
                    
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
                        rogueObject.tag = "Rogue";
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
						summary += "The linking between "+firstPlanetScript.planetname+"and " + planet2.GetComponent<Planet>().planetname+" is fail. " +planet2.GetComponent<Planet>().planetname+" became a rogue planet .\n";
					
                        planets.Remove(planet2); // remove from Planets List
                        Destroy(planet2);


                    }
                    else
                    {
                        linksuccessful = true;
                        linktime = 0;
                        firstPlanetScript.linkedWith.Add(planet2.GetComponent<Planet>());
                        secondPlanetScript.linkedWith.Add(planet1.GetComponent<Planet>());
						summary += firstPlanetScript.planetname + " successfully linked with " + secondPlanetScript.planetname+".\n";
                        // show confirmation box successful link
//                        if (level != 1)
//                        {
//                            cp.ShowPanel("Link Successful!", planet1.name + " and " + planet2.name + " have successfully linked!");
//                        }
//
                        // also update log
                    }

                }
                ResetLinking();

            }
            else
            {
                ResetLinking();
            }
        }

        foreach (var planet in planets)
        {
            planetScript = planet.GetComponent<Planet>();
            if (planetScript.iftech1 == 2)
            {
                planetScript.addResourceTechnology();
                planetScript.iftech1 = 3;

                // log when increased resource collection learned
                l.UpdateLogTech(planet.name, "Increased Resource Collection", "Resource collection increased by 2");
            }

            if (planetScript.iftech2 == 2)
            {
                planetScript.stone -= 5;
                planetScript.water -= 5;
                planetScript.gas -= 5;
                planetScript.iflinkactive = true;
                planetScript.iftech2 = 3;

                // log when Interplanetary Networking learned
                l.UpdateLogTech(planet.name, "Interplanetary Networking", "Linking with other planets Interplanetary Networking knowledge now available");
            }
            if (planetScript.iftech3 == 2)
            {
                planetScript.linkchanceTechnology();
                planetScript.iftech3 = 3;
            }
            if (planetScript.iftech4 == 2)
            {
                planetScript.StormShiedTechnology();
                planetScript.iftech4 = 3;
            }
            if (planetScript.iftech5 == 2)
            {
                planetScript.stone -= 20;
                planetScript.water -= 20;
                planetScript.gas -= 20;
                planetScript.ifattackactive = true;
                planetScript.iftech5 = 3;
            }
            if (storm)
            {
                planetScript = planet.GetComponent<Planet>();


                if (planetScript.stormsheid == false)
                {
                    planetScript.addCarbon = planetScript.halfaddCarbon;
                    planetScript.addNitrogen = planetScript.halfaddNitrogen;
                    planetScript.addHydrogen = planetScript.halfaddHydrogen;
                }
                if (planetScript.stormsheid == true)
                {
                    planetScript.addCarbon = planetScript.OriginaddCarbon;
                    planetScript.addNitrogen = planetScript.OriginaddNitrogen;
                    planetScript.addHydrogen = planetScript.OriginaddHydrogen;

                    // log about storm affecting resource collection
                }

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
                if (planetScript.turnsToBuild == 0)
                {
                    l.UpdateLogPlanet(planet.name, "has finished building");
                    l.LogBackLog();
                }
                planetScript.StartCoroutine(planetScript.AnimateOrbit(Constants.ANIMATE_SPEED_TEST));


            }
        }

        // rogue planet steal from dominatedPlanets
        foreach (var roguePlanet in roguePlanets)
        {
            Rogue rogueScript = roguePlanet.GetComponent<Rogue>();
            rogueScript.Steal(1, 1, 1);
        }

        m.CheckMissions(m.missions);

    }

    public void ToggleInteractability(bool canInteract)
    {
        if (canInteract && canBuild)
        {
            // New UI
            //SetBuildingActive(canInteract);

            //foreach (Button button in planetButtons)
            //{
            //--Unlock Tier 1 planets
            // Carbon is always unlocked
            //if (button.name == "Carbon")
            //{
            //    button.interactable = true;
            //}
            // -- Unlock Tier 2 planets
            // Silicon, Ammonia, Methane require Carbon to unlock
            //if (carbonIncrement > 0)
            //{
            //    if (button.name == "Silicon" || button.name == "Ammonia" || button.name == "Methane")
            //    {
            //        button.interactable = true;
            //    }
            //}
            // -- Unlock Tier 3 planets
            // Germanium requires Silicon to unlock
            //if (siliconIncrement > 0)
            //{
            //    if (button.name == "Germanium")
            //    {
            //        button.interactable = true;
            //    }
            //}
            // Acetylene requires Ammonia or Germanium to unlock
            //    if (ammoniaIncrement > 0 || germaniumIncrement > 0)
            //    {
            //        if (button.name == "Acetylene")
            //        {
            //            button.interactable = true;
            //        }
            //    }
            //}
            // Unlock technologies
            //            Transform[] ts = GameObject.Find("Micro Skill Tree Parent").transform.GetComponentsInChildren<Transform>(true); // bool includeInactive = true 
            //            foreach (Transform t in ts)
            //            {
            //                if (microSkillTreeNames.Contains(t.gameObject.name))
            //                {
            //                    var tech = t.gameObject.GetComponent<TechnologySkillTree>();
            //                    tech.Unlock();
            //                }
            //            }
        }
    }
}

