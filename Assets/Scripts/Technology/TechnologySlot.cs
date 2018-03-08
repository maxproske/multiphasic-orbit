using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
// Track events through event systems
using UnityEngine.EventSystems;

public class TechnologySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	private GameController gc;
	private bool mouseHover; // Tracks if mouse is hovering over panel
	public GameObject microSkillTree; // Reference to micro skill tree

	public Transform technologyParent; // Reference to the empty skill tree slots parent
	public List<Transform> prereqs = new List<Transform>(); // Reference to the technology slots to draw lines to
	public UnityEngine.UI.Extensions.UILineRenderer LineRenderer; // Assign Line Renderer in editor

	public void OnPointerEnter (PointerEventData eventData) {
		mouseHover = true;
	}
	public void OnPointerExit (PointerEventData eventData) {
		mouseHover = false;
	}

	void Awake() {
		gc = GameObject.Find ("Game Manager").GetComponent<GameController> ();
	}

	// Use this for initialization
	public void AddNewPoint (float xValue, float yValue) {
		var point = new Vector2() { x = xValue, y = yValue };
		var pointlist = new List<Vector2>(LineRenderer.Points);
		pointlist.Add(point);
		LineRenderer.Points = pointlist.ToArray();
	}

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
		// Enable math at the start
		buttons [0].interactable = true;

		// NOTE: The LineRenderer component is not suitable for drawing lines in the new UI
		//this.gameObject.AddComponent<UILineRenderer> ();
		if (LineRenderer != null) {
			LineRenderer.LineThickness = 4;

			if (prereqs != null) {
				var num = 0;
				foreach (Transform target in prereqs) {
					if (target != null) {

                        // M->ASD
                        //AddNewPoint(115f + 10f, 115f - 75f);
                        //AddNewPoint(45f + 10f, 45f - 75f);


                        num++;
					}
				}
			}
		}
	}

	void getButtonName() {
		var currentEventSystem = EventSystem.current;
		if(currentEventSystem == null) { return; }

		var currentSelectedGameObject = currentEventSystem.currentSelectedGameObject;
		if(currentSelectedGameObject == null) { return; }

		Debug.Log(currentSelectedGameObject.name);
	}

	// Update is called once per frame
	void Update () {

		// When player clicks on the icon
		if (Input.GetMouseButtonUp (0) && mouseHover) {
			// Get tech tree slot clicked
			var clicked = this.GetComponent<Button> ().transform.parent.name;
			// Get script
			var tech = microSkillTree.GetComponent<TechnologySkillTree>();

			// Unlock the next one.
			if (clicked == "Mathematics Technology Slot") {
				tech.mathematics = 1;
			}
			if (clicked == "Interplanetary Networking Technology Slot") {
				tech.interplanetaryNetworking = 1;
			}
			if (clicked == "Mass Particle Displacement Technology Slot") {
				tech.massParticleDisplacement = 1;
			}

			if (gc.GAME_STATE == Constants.TURN_3_TECH_SLOT) {
				gc.GAME_STATE = Constants.TURN_3_TECH_TREE_2;
			}
		}
	}
}