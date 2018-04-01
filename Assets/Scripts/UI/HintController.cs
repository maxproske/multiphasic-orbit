using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintController : MonoBehaviour 
{
	public string myString; // Message to display
	private GameObject myParent; // Private because this can't be set in the editor
	private GameObject myPrefab;
    private RectTransform myTarget;
	private int paddingLeft = 0; // Additional extra left padding
	private GameObject go; // Game object to instantiate
	private GameObject go2;
    private UIController ui;
	private float speed = 0.4f; // Closer to zero = faster
	private float bounceDistance = 0.333f; // Pixels to bounce left
	private bool rotated = false;
    private RectTransform hintPanel;
    private bool hintHasWidth;
    public bool active = false;

    void Start () 
    {
        hintHasWidth = false;

        // Set resources
        ui = GameObject.Find("Canvas").GetComponent<UIController>();
        myPrefab = (GameObject)Resources.Load<GameObject>("Prefabs/Hint");

        // Self
        myTarget = (RectTransform)gameObject.transform;

        // Script cannot find Hints empty game object when set in editor
		myParent = ui.hintContainer;

		// Instantiate a new hint prefab
		go = Instantiate(myPrefab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
		go2 = Instantiate(myPrefab, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
   		
        go.SetActive (false);
		// Make hidden by default
		SetHintActive (active);

        // Make it a child of the button
		go.transform.SetParent(myTarget.transform);
		go2.transform.SetParent(myParent.transform);

        // Reset position and scale
		go.transform.localPosition = new Vector3 (0, 0, 0);
		go.transform.localScale = new Vector3 (1, 1, 1);
		go2.transform.localPosition = new Vector3 (0, 0, 0);
		go2.transform.localScale = new Vector3 (1, 1, 1);

        // Spawn hint in Hint Container at position of hint attached to game object
        go2.transform.position = go.transform.position;

        // Set base margin, or else tooltip will be on very edge of button
        float margin = 10f;

        // Set the tooltip to the right side before making active
		RectTransform buttonPanel = (RectTransform)myTarget.transform;
        float buttonPanelWidth = buttonPanel.rect.width;
        hintPanel = (RectTransform)go2.transform;
        float hintPanelWidth = hintPanel.rect.width;

        if (hintPanel.position.x > Screen.width / 2)
        {
            // Show tooltip on the left side of target
            float offset = go2.transform.localPosition.x - (buttonPanelWidth/2) - hintPanelWidth - margin;
            go2.transform.localPosition = new Vector3 (offset, go2.transform.localPosition.y, 0);
            go2.transform.localScale = new Vector3(-1, 1, 1); // Point right
        }
        else
        {
            // Show tooltip on the right side of target
            float offset = go2.transform.localPosition.x + (buttonPanelWidth/2) + margin;
            go2.transform.localPosition = new Vector3 (offset, go2.transform.localPosition.y, 0);
            go2.transform.localScale = new Vector3(1, 1, 1); // Point left
        }

        // Enable
       StartCoroutine (Play ());
    }

    void Update ()
    {
        if (!hintHasWidth) 
        {
            hintPanel = (RectTransform)go2.transform;
        }
    }

	IEnumerator Play () 
    {
		Vector3 pointA = new Vector3 (0, 0, 0);
		Vector3 pointB = new Vector3 (0, 0, 0);
		// IT'S PURE MAGIC
		if (!rotated) 
        {
            // Left-right
			pointA = new Vector3 (bounceDistance / 2, 0, 0);
			pointB = pointA + new Vector3 (-bounceDistance, 0, 0);
		} 
        else
         {
            // Up-down
			pointA = new Vector3 (0, bounceDistance / 2, 0);
			pointB = pointA + new Vector3 (0, -bounceDistance, 0);
		}
		while (true) 
        {
			yield return StartCoroutine(MoveObject(go2.transform, pointA, pointB, speed));
			yield return StartCoroutine(MoveObject(go2.transform, pointB, pointA, speed));
		}
	}

	IEnumerator MoveObject (Transform thisTransform, Vector3 startPos, Vector3 endPos, float time) 
    {
		float i = 0.0f;
		float rate = 1.0f / time;
		while (i < 1.0f) {
			i += Time.deltaTime * rate;
            // Update x position of Hint attached to Hint Container
			thisTransform.position = Vector3.Lerp(startPos + thisTransform.position, endPos + thisTransform.position, i);
            yield return null;
		}
	}

    public void SetHintActive(bool state) 
	{
        // If tooltip is already properly positioned
        go2.SetActive (state);

        // Because PolygonUI is dumb, we need to mark the graphics
        // as dirty to redraw. Calling SetActive(true) is not enough.
        StartCoroutine(redrawHintAfterDelay());
	}

	IEnumerator redrawHintAfterDelay()
	{
		// Wait...
		yield return new WaitForSeconds(0.05f); // The parameter is the number of seconds to wait
		// Do something...
		UnityEngine.UI.Extensions.PolygonUI pui = go2.GetComponent<UnityEngine.UI.Extensions.PolygonUI>();
		pui.SetAllDirty();
	}
}
