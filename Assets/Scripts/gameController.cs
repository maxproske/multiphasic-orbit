using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class gameController : MonoBehaviour
{
    public Button nextTurnButton;
    public Planet[] planets;

    public Text planetText;
    public Text carbonText;
    public Text nitrogenText;
    public Text hydrogenText;

    private Planet planetScript;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("Planet(Clone)") != null)
        {
            planetScript = GameObject.Find("Planet(Clone)").GetComponent<Planet>();
            //Debug.Log("Planet(Clone) Carbon: " + planetScript.carbon);
            //Debug.Log("Planet(Clone) Nitrogen: " + planetScript.nitrogen);
            //Debug.Log("Planet(Clone) Hydrogen: " + planetScript.hydrogen);
            planetText.text = planetScript.name;
            carbonText.text = planetScript.carbon.ToString();
            nitrogenText.text = planetScript.nitrogen.ToString();
            hydrogenText.text = planetScript.hydrogen.ToString();
        }
    }
}
