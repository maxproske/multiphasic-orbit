using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    public Button nextTurnButton;
    public List<GameObject> planets;

    public Text planetText;
    public Text carbonText;
    public Text nitrogenText;
    public Text hydrogenText;

    private Planet planetScript;
    private GameObject selected; // hold selected GameObject

    public bool simulate;
    private float startTime;
    private Button nextTurn;
    private EventSystem es;
    private bool esgo = false; // handles un/locking of UI
    private int i;
    // Use this for initialization
    void Start()
    {
        nextTurn = GameObject.Find("End Turn Button").GetComponent<Button>();
        nextTurn.onClick.AddListener(Simulate);

        planets = new List<GameObject>();
        es = EventSystem.current;
        simulate = false;
    }

    // Update is called once per frame
    void Update()
    {
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


    void Simulate()
    {
        simulate = true;
        startTime = Time.time;
        es.enabled = false;
        esgo = true;
    }




}
