using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// -- Create UI Controller
// UIController ui = GameObject.Find ("Canvas").GetComponent<UIController> ();

// -- Update the right panel
// ui.SetSelectedPlanet (Planet planet);

// -- Update the bottom panel
// ui.SetActionPanel (bool simulating, bool canSimulate, bool canLink);

// -- Update the turn counter (Turn 1)
// ui.SetTurn (int turn);

// -- Update the distance counter (20 uu)
// ui.SetDistance (int distance);

public class UIController : MonoBehaviour 
{
	/* Declare Top Panel
	   ========================================================================== */
	// Turn Panel
	public Text topTurnText;
	// Distance Panel
	public Text topDistanceText;

	/* Declare Right Panel
	   ========================================================================== */
	// Selection Panel
	public Text rightLeftText;
	public Text rightNameText;
	public Text rightRightText;
	// Preview Panel
	public Image rightPreviewImage;
	// Resource Panel
	public Text rightCarbonText;
	public Text rightNitrogenText;
	public Text rightHydrogenText;
    public RectTransform rightResourcePanel;
	// Status Panel
	public Text rightPopulationText;
	public Text rightHealthText;
    public RectTransform rightStatusPanel;
    // Technology Panel
    public RectTransform rightTechnologyPanel;
    private Button[] _technologyButtons; // Keep private
    private Text[] _technologyText; // Keep private

	/* Declare Bottom Panel
	   ========================================================================== */
    public RectTransform bottomActionPanel;
    private Button[] _actionButtons; // Keep private

	/* Declare Left Panel
	   ========================================================================== */

	private void Start()
	{
		/* Initialize Top Panel
	       ====================================================================== */
		SetTurn (1);
		SetDistance (20);

		/* Initialize Right Panel
	       ====================================================================== */
		SetSelectedPlanet (null);

		/* Initialize Bottom Panel
		   ====================================================================== */
        SetActionPanel(false, false, false);

		/* Initialize Left Panel
		   ====================================================================== */
	}

	/* Set Top Panel
	   ========================================================================== */
	public void SetTurn (int turn = 1)
	{
		topTurnText.text = "Turn " + turn.ToString ();
	}

	public void SetDistance (int distance = 20)
	{
		topDistanceText.text = distance.ToString() + " uu";
	}

	/* Set Right Panel
	   ========================================================================== */
	public void SetSelectedPlanet (Planet p = null)
	{
		// Planet selected
		if (p != null) 
		{
			// Replace placeholder values with planet data
			SetSelectedName (p.planetname);
			SetSelectedPreview (p.planetSprite);
			SetSelectedResources (p.carbon, p.nitrogen, p.hydrogen);
			SetSelectedPopulation (p.population);
			SetSelectedHealth (p.health, p.maxHealth);
            SetSelectedTechnology (p.technologyLevel);
            // Enable last
            SetSelectedPanelsActive(true);
		}
		// No planet selected
		else 
		{
            // Disable First
            SetSelectedPanelsActive();
			// Replace placeholder values with default values
			SetSelectedName ();
			SetSelectedPreview ();
			SetSelectedResources ();
			SetSelectedPopulation ();
			SetSelectedHealth ();
            SetSelectedTechnology ();
		}
	}

    private void SetSelectedPanelsActive (bool active = false) 
    {
        rightResourcePanel.gameObject.SetActive(active);
        rightStatusPanel.gameObject.SetActive(active);
        rightTechnologyPanel.gameObject.SetActive(active);
    }

	private void SetSelectedName (string name = null)
	{
		// Planet selected
		if (name != null) 
		{
			rightLeftText.text = "«";
			rightNameText.text = name;
			rightRightText.text = "»";
		}
		// No planet selected
		else 
		{
			rightLeftText.text = "";
			rightNameText.text = "No Planet Selected";
			rightRightText.text = "";
		}
	}

	private void SetSelectedPreview (Sprite sprite = null)
	{
		// Planet selected
		if (sprite != null) 
		{
			rightPreviewImage.GetComponent<Image>().sprite = sprite;
		}
		// No planet selected
		else 
		{
			// Resources.Load searches in the directory "Assets/Resources"
			sprite = (Sprite)Resources.Load<Sprite>("Default");
			rightPreviewImage.GetComponent<Image>().sprite = sprite;
		}
	}

	private void SetSelectedResources (int carbon = -1, int nitrogen = -1, int hydrogen = -1)
	{
		// Planet selected
		if (carbon > -1 && nitrogen > -1 && hydrogen > -1) 
		{
			rightCarbonText.text = carbon.ToString();
			rightNitrogenText.text = nitrogen.ToString();
			rightHydrogenText.text = hydrogen.ToString();
		}
		// No planet selected
		else 
		{
			rightCarbonText.text = rightNitrogenText.text = rightHydrogenText.text = "-";
		}
	}

	private void SetSelectedPopulation (int population = -1)
	{
		// Planet selected
		if (population > -1) 
		{
			if (population > 0) 
			{
				rightPopulationText.text = population.ToString () + " Billion";
			} 
			else 
			{
				rightPopulationText.text = "Uninhabited";
			}
		}
		// No planet selected
		else 
		{
			rightPopulationText.text = "-";
		}
	}

	private void SetSelectedHealth (int health = -1, int maxHealth = -1)
	{
		// Planet selected
		if (health > -1 && maxHealth > -1) 
		{
			if (health <= maxHealth) 
			{
				rightHealthText.text = health.ToString () + "/" + maxHealth.ToString () + " HP";
			} 
			else 
			{
				rightHealthText.text = "ERROR: health > max health";
			}
		}
		// No planet selected
		else 
		{
			rightHealthText.text = "-";
		}
	}

    private void SetSelectedTechnology (int technology = -1)
	{
		// Planet selected
		if (technology > -1) 
		{
            _technologyButtons = rightTechnologyPanel.GetComponentsInChildren<Button>();
            for (int i = 0; i < _technologyButtons.Length; i++)
            {
                if (_technologyButtons[i].interactable)
                {
                    _technologyButtons[i].interactable = false;
                }
            }
		}
		// No planet selected
		else 
		{
            // Make uninteractive
            _technologyButtons = rightTechnologyPanel.GetComponentsInChildren<Button>();
            for (int i = 0; i < _technologyButtons.Length; i++)
            {
                if (_technologyButtons[i].interactable)
                {
                    _technologyButtons[i].interactable = false;
                }
            }
		}
	}

	/* Set Bottom Panel
	   ========================================================================== */
    public void SetActionPanel (bool simulating = false, bool canSimulate = false, bool canLink = false)
    {
        // Get reference to buttons
        _actionButtons = bottomActionPanel.GetComponentsInChildren<Button>();

        if (simulating)
        {
            // Make uninteractive
            for (int i = 0; i < _actionButtons.Length; i++)
            {
                _actionButtons[i].interactable = false;
            }
        }
        else 
        {
            // Make interactive
            for (int i = 0; i < _actionButtons.Length; i++)
            {
                if (i == 0)
                {
                    _actionButtons[i].interactable = true;
                }
                if (i == 1)
                {
                    _actionButtons[i].interactable = canSimulate;
                }
                if (i == 2)
                {
                    _actionButtons[i].interactable = canLink;
                }
            }
        }
    }

	/* Set Left Panel
	   ========================================================================== */
}
