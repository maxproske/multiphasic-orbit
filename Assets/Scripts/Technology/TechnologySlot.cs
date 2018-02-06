using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TechnologySlot : MonoBehaviour {

	public Transform technologyParent; // Reference to the empty skill tree slots parent
	public List<Transform> prereqs; // Reference to the technology slots to draw lines to

	// Use this for initialization
	void Start () {
		// Set all planet slot buttons as uninteractable
		var buttons = technologyParent.GetComponentsInChildren<Button>();
		for (int i = 0; i < buttons.Length; i++)
		{
			if (buttons[i].interactable)
			{
				buttons[i].interactable = false;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
