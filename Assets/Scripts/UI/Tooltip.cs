using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For <Button>
using UnityEngine.EventSystems; // For <IPointerEnterHandler> and <IPointerExitHandler>

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	// Declare public variables
	public string myString; // Message to display
	public Button myButton; // Reference to button to check if intractable
	public GameObject myPrefab;

	// Declare private variables
	private bool mouseHover;
	private Text myHiddenParentText; // Reference to text that resizes box
	private Text myVisualChildText; // Reference to visible text
	private GameObject go; // Game object to instantiate

	void Start()
	{
		setupTooltip ();
	}

	void Update() 
	{
		updateTooltip ();
	}

	void tooltipSetActive(bool state) 
	{
		if (go != null) {
			go.SetActive (state);
		}
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		mouseHover = true;
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		mouseHover = false;
	}

	void setupTooltip ()
	{
		// Instantiate a new tooltip prefab
		go = Instantiate(myPrefab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;

		// Make hidden by default
		tooltipSetActive (false);

		// Make it a child of the button
		go.transform.parent = myButton.transform;

		// Get width of button for margin
		RectTransform rt = (RectTransform)go.transform.parent.transform;
		float marginLeft = (rt.rect.width/2) + 5;

		// Reset position and scale
		go.transform.localPosition = new Vector3 (marginLeft, 0, 0);
		go.transform.localScale = new Vector3 (1, 1, 1);

		// Initialize text with user supplied message
		myHiddenParentText = go.GetComponent<Text> ();
		myVisualChildText = go.GetComponentsInChildren<Text> ()[1].GetComponent<Text>();
		myHiddenParentText.text = myVisualChildText.text = myString;
	}
		
	void updateTooltip ()
	{
		if (mouseHover) 
		{
			// Show tooltip
			tooltipSetActive (true);
		}
		else 
		{
			// Hide tooltip
			tooltipSetActive (false);
		}
	}
}
