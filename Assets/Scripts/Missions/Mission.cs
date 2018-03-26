using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission : MonoBehaviour {

    public string missionName;
    public string missionDescription;
    public bool completed;

	// Use this for initialization
	public void Start () {
        Debug.Log("Started");
        completed = false;
	}
}


