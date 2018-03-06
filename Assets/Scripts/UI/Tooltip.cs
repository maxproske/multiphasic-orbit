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
	private Text myHiddenParentText2; // Reference to text that resizes box
	private Text myVisualChildText2; // Reference to visible text
	private GameObject go; // Game object to instantiate
	private GameObject go2;

	private RectTransform rt;

	void Start()
	{
		setupTooltip ();
	}

	void Update() 
	{
		updateTooltip ();
	}

	void setupTooltip ()
	{
		// Script cannot find Tooltips empty game object when set in editor
		myParent = GameObject.Find ("Tooltips");

		// Instantiate a new tooltip prefab
		go = Instantiate(myPrefab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
		go2 = Instantiate(myPrefab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;

		go.SetActive (false);
		// Make hidden by default
		tooltipSetActive (false);

		// Make it a child of the button
		go.transform.SetParent(myButton.transform);
		go2.transform.SetParent(myParent.transform);

		// Get width of button for margin
		rt = (RectTransform)myButton.transform.parent.transform;
		float marginLeft = (rt.rect.width/2) + 5;

		// Reset position and scale
		go.transform.localPosition = new Vector3 (marginLeft, 0, 0);
		go.transform.localScale = new Vector3 (1, 1, 1);
		go2.transform.localPosition = new Vector3 (marginLeft, 0, 0);
		go2.transform.localScale = new Vector3 (1, 1, 1);

		// Initialize text with user supplied message
		myHiddenParentText = go.GetComponent<Text> ();
		myVisualChildText = go.GetComponentsInChildren<Text> ()[1].GetComponent<Text>();
		myHiddenParentText.text = myVisualChildText.text = myString;

		myHiddenParentText2 = go2.GetComponent<Text> ();
		myVisualChildText2 = go2.GetComponentsInChildren<Text> ()[1].GetComponent<Text>();
		myHiddenParentText2.text = myVisualChildText2.text = myString;
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		mouseHover = true;
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		mouseHover = false;
	}

	void updateTooltip ()
	{
		go2.transform.position = go.transform.position;

		if (mouseHover && myButton.interactable) 
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

	void tooltipSetActive(bool state) 
	{
		go2.SetActive (state);
	}
}
