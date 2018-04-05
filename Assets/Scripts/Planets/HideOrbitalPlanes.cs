using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOrbitalPlanes : MonoBehaviour 
{

    private GameController gc;

	void Start () 
    {
		gc = GameObject.Find("Game Manager").GetComponent<GameController>();

	}
	
	void Update () 
    {
        Debug.Log(gc.placing);
        // Make orbital plane active while placing
		if (gc.placing)
        {
            
        }
        else
        {

        }
	}
}
