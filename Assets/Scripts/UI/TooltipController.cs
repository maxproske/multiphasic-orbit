using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // For <Button>
using UnityEngine.EventSystems; // For <IPointerEnterHandler> and <IPointerExitHandler>

public class TooltipController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	// Declare public variables
	public string myString; // Message to display
	private GameObject myParent; // Private because this can't be set in the editor
	private GameObject myPrefab;
    private RectTransform myTarget;
	private int paddingLeft = 0; // Additional extra left padding

	// Declare private variables
	private bool mouseHover;
	private Text myHiddenParentText; // Reference to text that resizes box
	private Text myVisualChildText; // Reference to visible text
	private Text myHiddenParentText2; // Reference to text that resizes box
	private Text myVisualChildText2; // Reference to visible text
	private GameObject go; // Game object to instantiate
	private GameObject go2;

	private RectTransform rt;

    // Reference to UI
	private UIController ui;

    private bool tooltipHasWidth = false;

    private RectTransform tooltipPanel;

	// Starts when TooltipController is instantiated in UIController
    void Start()
    {
        // Set flag
        tooltipHasWidth = false;
        tooltipPanel = null;

        // Set resources
        ui = GameObject.Find("Canvas").GetComponent<UIController>();
        myPrefab = (GameObject)Resources.Load<GameObject>("Prefabs/Tooltip");

        // Self
        myTarget = (RectTransform)gameObject.transform;

		// Format string
		myString = myString.Replace(";", "\n");

		// Script cannot find Tooltips empty game object when set in editor
		myParent = ui.tooltipContainer;

		// Instantiate a new tooltip prefab
		go = Instantiate(myPrefab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
		go2 = Instantiate(myPrefab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;

		go.SetActive (false);
		// Make hidden by default
		tooltipSetActive (false);

		// Make it a child of the button
		go.transform.SetParent(myTarget.transform);
		go2.transform.SetParent(myParent.transform);

		// Reset position and scale
		go.transform.localPosition = new Vector3 (0, 0, 0);
		go.transform.localScale = new Vector3 (1, 1, 1);
		go2.transform.localPosition = new Vector3 (0, 0, 0);
		go2.transform.localScale = new Vector3 (1, 1, 1);

		// Initialize text with user supplied message
		myHiddenParentText = go.GetComponent<Text> ();
		myVisualChildText = go.GetComponentsInChildren<Text> ()[1].GetComponent<Text>();
		myHiddenParentText.text = myVisualChildText.text = myString;

		myHiddenParentText2 = go2.GetComponent<Text> ();
		myVisualChildText2 = go2.GetComponentsInChildren<Text> ()[1].GetComponent<Text>();
		myHiddenParentText2.text = myVisualChildText2.text = myString;

        // Make children invisible last
        RectTransform[] children = go2.GetComponentsInChildren<RectTransform>();
        foreach (var child in children)
        {
            child.gameObject.SetActive(false);
        }
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		mouseHover = true;
        UpdateTooltip();
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		mouseHover = false;
        UpdateTooltip();
	}

    void Update ()
    {
        if (!tooltipHasWidth) 
        {
            tooltipPanel = (RectTransform)go2.transform;
            float tooltipPanelWidth = tooltipPanel.rect.width;
            if (tooltipPanelWidth > 0) 
            {
                // Make children visible
				RectTransform[] children = go2.GetComponentsInChildren<RectTransform>(true);
                foreach (var child in children)
                {
                    child.gameObject.SetActive(true);
                }

                // Update the tooltip location now that we have the width
                UpdateTooltip();

                // Set flag true
                tooltipHasWidth = true;
            }
        }
    }

	void UpdateTooltip ()
	{
		go2.transform.position = go.transform.position;

        // Set the tooltip to the right side before making active
		RectTransform buttonPanel = (RectTransform)myTarget.transform;
        float buttonPanelWidth = buttonPanel.rect.width;
        tooltipPanel = (RectTransform)go2.transform;
        float tooltipPanelWidth = tooltipPanel.rect.width;

        // Set base margin, or else tooltip will be on very edge of button
        float margin = 20;

        if (tooltipPanel.position.x > Screen.width / 2)
        {
            // Show tooltip on the left side of target
            float offset = go2.transform.localPosition.x - (buttonPanelWidth/2) - tooltipPanelWidth - margin;
            go2.transform.localPosition = new Vector3 (offset, go2.transform.localPosition.y, 0);
        }
        else
        {
            // Show tooltip on the right side of target
            float offset = go2.transform.localPosition.x + (buttonPanelWidth/2) + margin;
            go2.transform.localPosition = new Vector3 (offset, go2.transform.localPosition.y, 0);
        }

        // Prevent stuttering
        // tooltipPanel.rect.width is 0 until SetActive, causing stuttering when opening
        // the tooltip for the first time. To get around this, we need to quickly make it active but transparent.

		if (mouseHover/*&& myButton.interactable*/) 
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

	public void tooltipSetActive(bool state) 
	{
        // If tooltip is already properly positioned
        go2.SetActive (state);
	}
}
