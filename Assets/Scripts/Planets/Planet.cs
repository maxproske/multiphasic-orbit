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
    public bool planetPlaced = false;

    public bool coroutineFlag = false;

    // Resource Counters
    public int carbon;
    public int nitrogen;
    public int hydrogen;

    // Add how many resources per turn
    public int addCarbon;
    public int addNitrogen;
    public int addHydrogen;

	private GameController gc; // Access Game Controller script


	//add a bool to check if the planet is selected
	private bool showInformation=false;

	//add a collider for the object
	public SphereCollider sc;




    public Planet()
    {
        addCarbon = 0;
        addNitrogen = 0;
        addHydrogen = 0;
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
    public void Start()
    {
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
        StartCoroutine(AnimateOrbit());

		//add a collider for this planet
		sc=gameObject.AddComponent<SphereCollider>();
		sc.radius = 0.5f;
		sc.center = new Vector3 (0, 0, 0);

    }

    public void FixedUpdate()
    {
        if (Application.isPlaying && lr != null)
        {
            CalculateEllipse();
            //Debug.Log(fastUniverse);
        }

        float cTime = Time.time;


        
        //Debug.Log ("planetPlaced: " + planetPlaced + ", orbitActive: " + orbitActive);

        // Planet must be placed before pausing its orbit
        if (planetPlaced)
        {
            // if simuate boolean is true in GameController
            if (gc.simulate)
            {
                // same code as GoNext()
                // Don't stack button presses
                if (!orbitActive)
                {
                    //we can add resouces here
                    startTime = Time.time;
                    ifNext = true;
                }
                if (cTime - startTime < 3)
                {
                    if (ifNext == true)
                    {
                        orbitActive = true;

                        // Start coroutine ONCE
                        ifNext = false;
                        //gc.simulate = false;
                        StartCoroutine(AnimateOrbit());
                        CollectResources();
                    }
                }
                else
                {
                    orbitActive = false;
                    ifNext = false;
                    gc.simulate = false;

                }
            }
        }
    }



	//Function that check if the mouse click this object

	void OnMouseDown(){

		Debug.Log ("123");
		if (showInformation == false) {
			showInformation = true;
		} else {
			showInformation = false;
		}
	}


	//Function that can show the resource of this object

	void OnGUI(){
		if (showInformation == true&&!orbitActive) {
			GUIStyle guistyle = new GUIStyle();
	
			GUI.Label (new Rect (Screen.width - Screen.width / 5, 50, 100, 50), "Carbon: " + carbon);
			GUI.Label (new Rect (Screen.width - Screen.width / 5, 70, 100, 50), "Nitrogen: " + nitrogen);
			GUI.Label (new Rect (Screen.width - Screen.width / 5, 90, 100, 50), "Hydrogen: " + hydrogen);


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
        carbon += addCarbon;
        nitrogen += addNitrogen;
        hydrogen += addHydrogen;
    }

    public IEnumerator AnimateOrbit()
    {
        // Is the orbit really close to 0? We don't want it to move too fast.
        // Set it to a more reasonable minimum (every 1/10 of a second) so it won't divide by 0
        if (orbitPeriod < 0.1f)
        {
            orbitPeriod = 0.1f;
        }

        // If orbit is active, start orbit animation
        while (orbitActive)
        {

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
