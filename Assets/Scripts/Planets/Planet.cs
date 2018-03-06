﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Make class require a LineRenderer to be attached whenever we run this script
[RequireComponent(typeof(LineRenderer))]
public class Planet : MonoBehaviour
{

    // Will reference the motion of the planet model
    public Transform orbitingObject;

    // Get a reference to the LineRenderer
    LineRenderer lr;

    // Range attribute between 3 and 36 to determine # of segments in ellipse
    [Range(3, 36)]
    public int segments;

    // Serialized ellipse object
    public EllipseTester orbitPath;

    // How far along the path of the ellipse we are
    // Clamp between 0-1
    [Range(0f, 1f)]
    public float orbitProgress = 0f;

    // How long it will take in seconds to complete one orbit
    public float orbitPeriod = 3f;

    public float orbitSpeedMultiplier = 1f;

    // Allows us to toggle the orbit in-editor
    public bool orbitActive = false;

    //check if the button pressed
    public bool ifNext = false;

    //set the float for startTime
    private float startTime;

    //declare a button for nextturn
    private Button nextTurn;

    // Checks if in Fast or Slow Universe
    bool fastUniverse;

    // Was the planet placed?
    public bool planetPlaced;

    public bool coroutineFlag = false;

    // Resource Counters
    public int carbon;
    public int nitrogen;
    public int hydrogen;
    // Add how many resources per turn
    public int addCarbon;
    public int addNitrogen;
    public int addHydrogen;
    public int turnsToBuild;
    public int defensePower;
    public int attackPower;
    public int turnsToDie;
    private GameController gc; // Access Game Controller script
    public Coroutine placing;
    //add a collider for the object
    public SphereCollider sc;
    private float rectx, recty;
    private bool collecting = false;
    private int count = 0;
	private bool ifHover=false;
	public int maxResourceType = 0;
	public int maxResource = 0;

	private string planetname=" ";
	private string tier=" ";

    // linking
	public List<Planet> linkedWith = new List<Planet>(); // Each planet will have their own list of planets they have linked with
	public LineRenderer[] links ;
	public GameObject[] lines;

	public Vector3 pos;
	private bool ifdraw=true;

	private int tradecarbon = 0;
	private int tradenitrogen = 0;
	private int tradehydrogen = 0;

    public Planet()
    {
        addCarbon = 0;
        addNitrogen = 0;
        addHydrogen = 0;
        turnsToBuild = 0;
        defensePower = 0;
        attackPower = 0;
        turnsToDie = 0;
    }
 
    public void Awake()
    {
        // Get reference when we start the game
        lr = GetComponent<LineRenderer>();

        // Calculate ellipse right when we start the game
        if (orbitingObject.transform.parent != null)
        {
            CalculateEllipse();
        }
    }

    // Use this for initialization
    public virtual void Start()
    { 
		lines = new GameObject[10];
		links = new LineRenderer[10];
		for (int i = 0; i < 10; i++) {
			lines [i] = new GameObject ();
			links [i] = lines [i].AddComponent<LineRenderer> ();
			links [i].SetWidth (0.1f, 0.1f);
		}
	
		if (addCarbon == 4 && addHydrogen == 1&& addNitrogen == 1) {
			planetname = "Carbon";
			tier = "Tier 1 Planet";
		}
		if (addCarbon == 6 && addHydrogen == 2&& addNitrogen == 2) {
			planetname = "Silicon";
			tier = "Tier 2 Planet";
		}
		if (addCarbon == 2 && addHydrogen == 6&& addNitrogen == 2) {
			planetname = "Hydrogen";
			tier = "Tier 2 Planet";
		}
		if (addCarbon == 2 && addHydrogen == 2&& addNitrogen == 6) {
			planetname = "Nitrogen";
			tier = "Tier 2 Planet";
		}
        // Check there are no objects to move around
        if (orbitingObject == null)
        {
            orbitActive = false;
            // Return early
            return;
        }


        
	
        gc = GameObject.Find("Game Manager").GetComponent<GameController>();

        //nextTurn = GameObject.Find("End Turn Button").GetComponent<Button>();
        //nextTurn.onClick.AddListener(GoNext);

        // Set orbiting object position
        SetOrbitingObjectPosition();

        // If orbit is active, start orbit animation

        if (!planetPlaced)
        {
            // If orbit is active, start orbit animation
            placing = StartCoroutine(AnimateOrbit(1));
        }


        //add a collider for this planet
        sc = gameObject.AddComponent<SphereCollider>();
//		linkline = gameObject.AddComponent<LineRenderer> ();
        sc.radius = 0.5f;
        sc.center = new Vector3(0, 0, 0);
			

    }



    public void FixedUpdate()
    {

		if (linkedWith.Count > 0) {
			for (int i = 0; i < linkedWith.Count; i++) {
				links [i].SetPosition (0, transform.position);
				links [i].SetPosition (1, linkedWith [i].transform.position);
			}
		}
        if (Application.isPlaying && lr != null)
        {
            CalculateEllipse();
            //Debug.Log(fastUniverse);
        }

        float cTime = Time.time;


        //Debug.Log ("planetPlaced: " + planetPlaced + ", orbitActive: " + orbitActive);

        //   // Planet must be placed before pausing its orbit
        //   if (planetPlaced)
        //   {
        //       // if simuate boolean is true in GameController
        //       if (gc.simulate)
        //       {
        //           // same code as GoNext()
        //           // Don't stack button presses
        //           if (!orbitActive)
        //           {
        //               //we can add resouces here
        //               startTime = Time.time;
        //               ifNext = true;
        //           }
        //           if (cTime - startTime < 3)
        //           {
        //               if (ifNext == true)
        //               {
        //                   orbitActive = true;

        //                   // Start coroutine ONCE
        //                   ifNext = false;
        //                   //gc.simulate = false;
        //                   StartCoroutine(AnimateOrbit());
        //                   CollectResources();
        //               }
        //           }
        //           else
        //           {
        //               orbitActive = false;
        //               ifNext = false;
        //               gc.simulate = false;


        //collecting = true;
        //           }
        //       }
        //   }

        // pop up of resources collected after simulation
        if (collecting)
        {
            count++;
            if (count == 30)
            {
				pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
                rectx = pos.x;
                recty = pos.y;
                //Debug.Log(rectx);
                //Debug.Log(recty);


            }
            if (count > 30)
            {

                recty += 1;
            }
            if (count > 60)
            {
                count = 0;
                collecting = false;
                gc.simulate = false;

				// Update Game State
				if (gc.GAME_STATE == Constants.TURN_1_WATCH_SIMULATION) {
					// Go to next step if the skill tree isn't open
					if (GameObject.Find ("Macro Skill Tree") == null) {
						gc.GAME_STATE = Constants.TURN_2_SKILL_TREE;
					} else {
						// Otherwide, skip ahead
						gc.GAME_STATE = Constants.TURN_2_PLANET_SLOT;
					}
				}
				else if (gc.GAME_STATE == Constants.TURN_2_WATCH_SIMULATION) {
					// Make button interactable
					//var button = GameObject.Find("Micro Skill Tree Button").GetComponent<Button>();
					//button.interactable = true;

					// Go to next step if the skill tree isn't open
					gc.GAME_STATE = Constants.TURN_3_TECH_TREE;
				}
            }
        }
    }

	//function that trade 
	void trade(Planet temp){
		getMaxResource ();
		temp.getMaxResource ();

		if (maxResourceType == 1) {
			carbon -= 1;
			tradecarbon -= 1;
			temp.carbon += 1;
			temp.tradecarbon += 1;
		}
		if (maxResourceType == 2) {
			nitrogen -= 1;
			tradenitrogen -= 1;
			temp.nitrogen += 1;
			temp.tradenitrogen += 1;

		}
		if (maxResourceType == 3) {
			hydrogen -= 1;
			tradehydrogen -= 1;
			temp.hydrogen += 1;
			temp.tradehydrogen += 1;
		}
		if (temp.maxResourceType == 1) {
			temp.carbon -= 1;
			temp.tradecarbon -= 1;
			carbon += 1;
			tradecarbon += 1;
		}
		if (temp.maxResourceType == 2) {
			temp.nitrogen -= 1;
			temp.tradenitrogen -= 1;
			nitrogen += 1;
			tradenitrogen += 1;
		}
		if (temp.maxResourceType == 3) {
			temp.hydrogen -= 1;
			temp.tradehydrogen -= 1;
			hydrogen += 1;
			tradehydrogen += 1;
		}	

	}

	void getMaxResource(){
		maxResource = Mathf.Max (carbon, nitrogen, hydrogen);
		if (maxResource == carbon) {
			maxResourceType = 1;
		}
		if (maxResource == nitrogen) {
			maxResourceType = 2;
		}
		if (maxResource == hydrogen) {
			maxResourceType = 3;
		}
	}

    //Function that check if the mouse click this object

    //void OnMouseDown(){

    //	Debug.Log ("123");
    //	if (showInformation == false) {
    //		showInformation = true;
    //	} else {
    //		showInformation = false;
    //	}
    //}

	void OnMouseOver(){
		ifHover = true;
	}
	void OnMouseExit(){

				ifHover = false;

		
	}
    //Function that can show the resource of this object

    void OnGUI()
    {
        //if (showInformation == true&&!orbitActive) {


        //GUI.Label (new Rect (Screen.width - Screen.width / 5, 50, 100, 50), "Carbon: " + carbon);
        //GUI.Label (new Rect (Screen.width - Screen.width / 5, 70, 100, 50), "Nitrogen: " + nitrogen);
        //GUI.Label (new Rect (Screen.width - Screen.width / 5, 90, 100, 50), "Hydrogen: " + hydrogen);

		if (ifHover == true) {
			GUI.Box (new Rect (rectx+20, Screen.height - recty, 300, 50), planetname+": Carbon: " + carbon + ", Nitrogen: " + nitrogen + ", Hydrogen: " + hydrogen+"\n This is a "+tier);

		}
        //}
        // pop up of resources collected after simulation
        if (collecting == true && count > 30)
        {
			int totalcarbon = addCarbon + tradecarbon;
			int totalnit = addNitrogen + tradenitrogen;
			int totalhyd = addHydrogen + tradehydrogen;
			int preCar = carbon - totalcarbon;
			int preNit = nitrogen - totalnit;
			int preHyd = hydrogen - totalhyd;

			GUI.Label(new Rect(rectx, Screen.height - recty - 50, 100, 50), "Carbon: " + preCar + " + " + totalcarbon);
			GUI.Label(new Rect(rectx, Screen.height - recty - 30, 100, 50), "Nitrogen: " + preNit + " + " + totalnit);
			GUI.Label(new Rect(rectx, Screen.height - recty - 10, 100, 50), "Hydrogen: " + preHyd + " + " + totalhyd);
        }
    }
    // Function that can start the Next turn
    public void GoNext()
    {
        // Don't stack button presses
        if (!orbitActive)
        {
            //we can add resouces here
            startTime = Time.time;
            ifNext = true;
        }
    }

    public void CollectResources()
    {
		tradecarbon = 0;
		tradenitrogen = 0;
		tradehydrogen = 0;
		//trade
		for (int i = 0; i < linkedWith.Count; i++) {
			int j = 0;
			if (j == 0) {
				trade (linkedWith [i]);
				j = 1;
			}
		}
        carbon += addCarbon;
        nitrogen += addNitrogen;
        hydrogen += addHydrogen;
        collecting = true;
        //gc.simulate = false;
    }

    // Control the orbit
    // Length determines how long will orbit in seconds
    public IEnumerator AnimateOrbit(float length)
    {
        float starting = 0f;
        // Is the orbit really close to 0? We don't want it to move too fast.
        // Set it to a more reasonable minimum (every 1/10 of a second) so it won't divide by 0
        if (orbitPeriod < 0.1f)
        {
            orbitPeriod = 0.1f;
        }

        //if (turnsToBuild > 0)
        //{
        //    gc.canBuild = false;
        //    //renderer.material = Resources.Load("Carbon") as Material;
        //} else
        //{
        //    gc.canBuild = true;
        //}

        // If orbit is active, start orbit animation
        //while (orbitActive)


        while (starting < length) // https://answers.unity.com/questions/504843/c-make-something-happen-for-x-amount-of-seconds.html
        {
            if (gc.simulate) // only during simulation will orbit for specific duration, else constantly orbits
            {
                starting += Time.deltaTime;
            }
            

            // Make orbit speed be affected by universe (disabled)
            Vector2 orbitPos = orbitPath.Evaluate(orbitProgress);
            int universeMultiplier = orbitPos.x < 0 ? 1 : 1;

            fastUniverse = orbitPos.x < 0 ? false : true;

            // Make orbit faster closer to sun
            float linearMultiplier = 8f;
            int exponentialMultiplier = 3;
            float orbitSpeedMultiplier = universeMultiplier * Mathf.Pow(Mathf.Max(Mathf.Abs(orbitPath.xAxis), Mathf.Abs(orbitPath.yAxis)) / linearMultiplier, exponentialMultiplier);

            // Division is one of the least efficient thing in basic C#
            // So use time.deltatime to see how far we're moving every frame
            // We want the inverse of orbitPeriod to see how fast we need to catch up
            float orbitSpeed = 1f / (orbitPeriod * orbitSpeedMultiplier);
            //Debug.Log (orbitSpeed);

            // (Amount of time frame has taken) * calculated orbit speed
            orbitProgress += Time.deltaTime * orbitSpeed;

            // Do not exceed float value if we add to the orbit progress over time
            // If we go beyond 1f, reset to between 0-1.
            orbitProgress %= 1f;

            // Set planet position based on orbit position
            SetOrbitingObjectPosition();

            // Repeat until orbitActive is false
            yield return null;
        }

        if (gc.simulate && turnsToBuild < 1) // after orbiting and during simulation will collect resources
        {
            CollectResources();
        }
    }

    // Calculate the ellipse
    public void CalculateEllipse()
    {
        // Create an array of Vector3's.
        // Populate LineRenderer with array of points to render
        // (segments + 1 to complete the ring around. We'll make the last element equal to the first element later.)
        Vector3[] points = new Vector3[segments + 1];

        // Iterate through these points
        for (int i = 0; i < segments; i++)
        {

            // Pass in t value (i/segments)
            Vector2 position2D = orbitPath.Evaluate((float)i / (float)segments);

            // Debug
            //Debug.Log ("Position of sun: (" + orbitingObject.transform.parent.localPosition.x + ", " + orbitingObject.transform.parent.localPosition.z + ")");

            // Set point at i equal to a new Vector2 of (x,y) and 0 for z value
            points[i] = new Vector3(position2D.x + orbitingObject.transform.parent.localPosition.x, 0f, position2D.y + orbitingObject.transform.parent.localPosition.z);
        }
        // Remember we have segments + 1, and are 0-indexing
        // Very last point in the array is equal to first point, completing the ellipse
        points[segments] = points[0];

        // Set LineRenderer using newer method positionCount
        lr.positionCount = segments + 1;
        // Pass in points array
        lr.SetPositions(points);
    }

    // Call-back method. If we change values in-editor while we are playing the game, we can see those
    public void OnValidate()
    {
        if (Application.isPlaying && lr != null)
        {
            CalculateEllipse();
        }
    }

    public void SetOrbitingObjectPosition()
    {
        // Pass in current orbit progress
        Vector2 orbitPos = orbitPath.Evaluate(orbitProgress);

        // Set its local position to a new vector position
        // Set z axis as orbitPos.y
        orbitingObject.localPosition = new Vector3(orbitPos.x, 0, orbitPos.y);
    }


}
