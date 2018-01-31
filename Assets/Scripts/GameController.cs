using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Button nextTurnButton;
    public List<GameObject> planets;

    public Text planetText;
    public Text carbonText;
    public Text nitrogenText;
    public Text hydrogenText;

    private Planet planetScript;

    public bool simulate;

    private Button nextTurn;
    // Use this for initialization
    void Start()
    {
        nextTurn = GameObject.Find("End Turn Button").GetComponent<Button>();
        nextTurn.onClick.AddListener(Simulate);

        planets = new List<GameObject>();
        simulate = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameObject.Find("Carbon(Clone)") != null)
        {
            planetScript = GameObject.Find("Carbon(Clone)").GetComponent<Planet>();
            //Debug.Log("Planet(Clone) Carbon: " + planetScript.carbon);
            //Debug.Log("Planet(Clone) Nitrogen: " + planetScript.nitrogen);
            //Debug.Log("Planet(Clone) Hydrogen: " + planetScript.hydrogen);
            planetText.text = planetScript.name;
            carbonText.text = planetScript.carbon.ToString();
            nitrogenText.text = planetScript.nitrogen.ToString();
            hydrogenText.text = planetScript.hydrogen.ToString();
        }
    }

    void Simulate()
    {
        simulate = true;
    }
}
