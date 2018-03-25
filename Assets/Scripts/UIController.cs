﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// -- Create UI Controller
// private UIController ui;
// ui = GameObject.Find ("Canvas").GetComponent<UIController> ();

// -- Update the left panel
// ui.SetSelectedPlanet (Planet planet);

// -- Set play button
// ui.SetPlayInteractive (true/false)

// -- Set turn count (eg. Turn 1)
// ui.SetTurn (1);

// -- Set distance(eg. 20 uu)
// ui.SetDistance (20);

// -- Update resources
// ui.UpdateResources();

// -- Update population
// ui.UpdatePopulation();

// -- Update health
// ui.UpdateHealth();

public class UIController : MonoBehaviour 
{
	/* Declare Top Panel
	   ========================================================================== */
	// Turn Panel
	public Text topTurnText;
	// Distance Panel
	public Text topDistanceText;

	/* Declare Left Panel
	   ========================================================================== */
	// Selection Panel
	public Text leftLeftText;
	public Text leftNameText;
	public Text leftRightText;
	// Preview Panel
	public Image leftPreviewImage;
	// Resource Panel
	public Text leftCarbonText;
	public Text leftNitrogenText;
	public Text leftHydrogenText;
    public RectTransform leftResourcePanel;
	// Status Panel
	public Text leftPopulationText;
	public Text leftHealthText;
    public RectTransform leftStatusPanel;
    // Technology Panel
    public RectTransform leftTechnologyPanel;
    private Button[] _technologyButtons; // Keep private
    private Text[] _technologyText; // Keep private
    private Planet _selectedPlanet; // Keep private

	/* Declare Right Panel
	   ========================================================================== */
    // Action Panel
    public Button rightNextTurnButton;

	private void Start()
	{
		/* Initialize Top Panel
	       ====================================================================== */
		SetTurn (1);
		SetDistance (0);

		/* Initialize Left Panel
	       ====================================================================== */
		SetSelectedPlanet (null);

		/* Initialize Right Panel
		   ====================================================================== */
        SetPlayInteractive(false);
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

	/* Set Left Panel
	   ========================================================================== */
	public void SetSelectedPlanet (Planet p = null)
	{
		// Planet selected
		if (p != null) 
		{
            // Set global variable
            this._selectedPlanet = p;

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
        leftResourcePanel.gameObject.SetActive(active);
        leftStatusPanel.gameObject.SetActive(active);
        leftTechnologyPanel.gameObject.SetActive(active);
    }

	private void SetSelectedName (string name = null)
	{
		// Planet selected
		if (name != null) 
		{
			leftLeftText.text = "«";
			leftNameText.text = name;
			leftRightText.text = "»";
		}
		// No planet selected
		else 
		{
			leftLeftText.text = "";
			leftNameText.text = "No Planet Selected";
			leftRightText.text = "";
		}
	}

	private void SetSelectedPreview (Sprite sprite = null)
	{
		// Planet selected
		if (sprite != null) 
		{
			leftPreviewImage.GetComponent<Image>().sprite = sprite;
		}
		// No planet selected
		else 
		{
			// Resources.Load searches in the directory "Assets/Resources"
			sprite = (Sprite)Resources.Load<Sprite>("Default");
			leftPreviewImage.GetComponent<Image>().sprite = sprite;
		}
	}

	private void SetSelectedResources (int carbon = -1, int nitrogen = -1, int hydrogen = -1)
	{
		// Planet selected
		if (carbon > -1 && nitrogen > -1 && hydrogen > -1) 
		{
			leftCarbonText.text = carbon.ToString();
			leftNitrogenText.text = nitrogen.ToString();
			leftHydrogenText.text = hydrogen.ToString();
		}
		// No planet selected
		else 
		{
			leftCarbonText.text = leftNitrogenText.text = leftHydrogenText.text = "-";
		}
	}

	private void SetSelectedPopulation (int population = -1)
	{
		// Planet selected
		if (population > -1) 
		{
			if (population > 0) 
			{
				leftPopulationText.text = population.ToString () + " Billion";
			} 
			else 
			{
				leftPopulationText.text = "Uninhabited";
			}
		}
		// No planet selected
		else 
		{
			leftPopulationText.text = "-";
		}
	}

	private void SetSelectedHealth (int health = -1, int maxHealth = -1)
	{
		// Planet selected
		if (health > -1 && maxHealth > -1) 
		{
			if (health <= maxHealth) 
			{
				leftHealthText.text = health.ToString () + "/" + maxHealth.ToString () + " HP";
			} 
			else 
			{
				leftHealthText.text = "ERROR: health > max health";
			}
		}
		// No planet selected
		else 
		{
			leftHealthText.text = "-";
		}
	}

    private void SetSelectedTechnology (int technology = -1)
	{
		// Planet selected
		if (technology > -1) 
		{
            _technologyButtons = leftTechnologyPanel.GetComponentsInChildren<Button>();
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
            _technologyButtons = leftTechnologyPanel.GetComponentsInChildren<Button>();
            for (int i = 0; i < _technologyButtons.Length; i++)
            {
                if (_technologyButtons[i].interactable)
                {
                    _technologyButtons[i].interactable = false;
                }
            }
		}
	}

	/* Set Right Panel
	   ========================================================================== */
    public void SetPlayInteractive (bool canSimulate = false)
    {
        rightNextTurnButton.interactable = canSimulate;
    }

   	/* Update Left Panel
	   ========================================================================== */ 
    public void UpdateResources ()
    {
        // Planet selected
        if (_selectedPlanet != null)
        {
            SetSelectedResources(_selectedPlanet.carbon, _selectedPlanet.nitrogen, _selectedPlanet.hydrogen);
        }
        // No planet selected
        else
        {
            SetSelectedResources();
        }
    }

    public void UpdatePopulation ()
    {
        // Planet selected
        if (_selectedPlanet != null)
        {
            SetSelectedPopulation(_selectedPlanet.population);
        }
        // No planet selected
        else
        {
            SetSelectedPopulation();
        }
    }

    public void UpdateHealth ()
    {
        // Planet selected
        if (_selectedPlanet != null)
        {
            SetSelectedHealth(_selectedPlanet.health, _selectedPlanet.maxHealth);
        }
        // No planet selected
        else
        {
            SetSelectedHealth();
        }
    }
}