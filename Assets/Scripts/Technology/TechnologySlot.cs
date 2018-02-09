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
						AddNewPoint (115f+10f, 115f-75f);
						AddNewPoint (45f+10f, 45f-75f);

						//AddNewPoint (this.transform.position.x, this.transform.position.y);
						//AddNewPoint (target.position.x, target.position.y);

						num++;
					}
				}
			}
		}
		/* FOR LINKING LATER
		// Find prereqs
		if (prereqs != null) {
			var num = 0;
			foreach (Transform target in prereqs) {
				if (target != null) {
					Debug.Log("childPos: (" + this.transform.position.x + ", " + this.transform.position.y + "), parentPos: (" + target.position.x + ", " + target.position.y + ")");

					// Add a Line Renderer to the GameObject
					lines.Add (this.gameObject.AddComponent<LineRenderer> ());
					// Set the width of the Line Renderer
					lines [num].widthMultiplier = 1f;
					lines [num].sortingOrder = 1;
					lines [num].material = new Material (Shader.Find ("Sprites/Default"));
					lines [num].material.color = Color.red; 
					lines [num].SetVertexCount (2);
					// Increment counter
					num++;
				}
			}
		} else {
			Debug.Log ("prereqs is null");
		}
		*/
	}
	
	// Update is called once per frame
	void Update () {

		// When player clicks on the icon
		if (Input.GetMouseButtonUp (0) && mouseHover) {
			if (gc.GAME_STATE == Constants.TURN_3_TECH_SLOT) {
				gc.GAME_STATE = Constants.TURN_3_TECH_TREE_2;
			}
		}


		/* FOR LINKING LATER
		// Avoid object reference not set to an isntance of an object
		if (prereqs != null) {
			var num = 0;
			foreach (Transform target in prereqs) {
				if (target != null) {
					// Update position of the two vertex of the Line Renderer
					//lines [num].SetPosition (0, new Vector3(0,0,0));
					//lines [num].SetPosition (1, new Vector3(2,2,0));
					lines [num].SetPosition (0, new Vector3(this.transform.position.x, this.transform.position.y, 0));
					lines [num].SetPosition (1,  new Vector3(target.position.x, target.position.y, 0));

					// Increment counter
					num++;
				}
			}
		}
		*/
	}
}