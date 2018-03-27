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
    public RectTransform leftPanel;
	// Selection Panel
    public RectTransform leftSelectionPanel;
	public Text leftLeftText;
	public Text leftNameText;
	public Text leftRightText;
	// Preview Panel
    public RectTransform leftPreviewPanel;
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
    public Planet selectedPlanet; // Keep private
    // Building Panel
    public RectTransform leftBuildingPanel;
    public Text leftBuildingText;
    // Helper variables
    private Planet previousPlanet; // Keep private
    public Button leftTechnology1Button;
    public Button leftTechnology2Button;
    public Button leftTechnology3Button;
    public Button leftTechnology4Button;
    public Button leftTechnology5Button;

	/* Declare Right Panel
	   ========================================================================== */
    // Action Panel
    public Button rightNextTurnButton;
    public Button rightStoneButton;
    public Button rightWaterButton;
    public Button rightGasButton;
    public Button rightMission1Button;
    public Button rightMission2Button;
    public Button rightMission3Button;

	/* Declare Tooltips
	   ========================================================================== */
    public GameObject tooltipPrefab;
    public GameObject tooltipContainer;

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

        /* Initialize External Scripts
           ====================================================================== */
        SetupTooltipController();
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
            // Set global variables
            this.previousPlanet = this.selectedPlanet;
            this.selectedPlanet = p;

			// Replace placeholder values with planet data
			SetSelectedName (p.planetname);
			SetSelectedPreview (p.planetSprite);
			SetSelectedResources (p.carbon, p.nitrogen, p.hydrogen);
			SetSelectedPopulation (p.population);
			SetSelectedHealth (p.health, p.maxHealth);
            SetSelectedTechnology (p.technologyLevel);
            SetSelectedBuilding(p.turnsToBuild);

            // Enable last
            SetSelectionPanelActive(true);
            SetPreviewPanelActive(true);
            SetResourcePanelActive(true);
            SetStatusPanelActive(true);
            SetTechnologyPanelActive(p.population > 0);
            SetBuildingPanelActive(p.population <= 0);

            // GFX
            SetSelectedOrbit();
		}
		// No planet selected
		else 
		{
            // Disable First
            SetSelectionPanelActive();
            SetPreviewPanelActive();
            SetResourcePanelActive();
            SetStatusPanelActive();
            SetTechnologyPanelActive();
            SetBuildingPanelActive();

			// Replace placeholder values with default values
			SetSelectedName ();
			SetSelectedPreview ();
			SetSelectedResources ();
			SetSelectedPopulation ();
			SetSelectedHealth ();
            SetSelectedBuilding();

            // GFX
            SetSelectedOrbit();
		}
	}

    public void SetNoPlanetSelected()
    {
        SetSelectionPanelActive(true);
        SetPreviewPanelActive(true);
        // Play animation
        OpenLeftPanel();
    }

    public void SetSelectedOrbit() 
    {
        if (previousPlanet != null) 
        {
            // Hide orbit of previous planet
            Color previousColor = Color.white;
            previousColor.a = 0.04f;
            previousPlanet.SetLineRendererTint(previousColor);

            // Highlight orbit of selected planet
            Color selectedColor = Color.white;
            selectedColor.a = 1f;
            selectedPlanet.SetLineRendererTint(selectedColor);
        }
    }

    private void SetSelectionPanelActive (bool active = false) 
    {
        leftSelectionPanel.gameObject.SetActive(active);
    }

    private void SetPreviewPanelActive (bool active = false) 
    {
        leftPreviewPanel.gameObject.SetActive(active);
    }

    private void SetResourcePanelActive (bool active = false) 
    {
        leftResourcePanel.gameObject.SetActive(active);
    }

    private void SetStatusPanelActive (bool active = false) 
    {
        leftStatusPanel.gameObject.SetActive(active);
    }

    private void SetTechnologyPanelActive (bool active = false) 
    {
        leftTechnologyPanel.gameObject.SetActive(active);
    }

    private void SetBuildingPanelActive (bool active = false) 
    {
        leftBuildingPanel.gameObject.SetActive(active);
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
			sprite = (Sprite)Resources.Load<Sprite>("Icons/delete-material-white");
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
		if (technology > -1) {

		}

		// No planet selected
		else 
		{
			SetTechnologyPanelActive(false);

            // Make uninteractive
//            _technologyButtons = leftTechnologyPanel.GetComponentsInChildren<Button>();
//            for (int i = 0; i < _technologyButtons.Length; i++)
//            {
//                if (_technologyButtons[i].interactable)
//                {
//                    _technologyButtons[i].interactable = false;
//                }
//            }
		}
	}

    private void SetSelectedBuilding (int population = 0)
	{
		// Planet selected
		if (population > -1) 
        {
            if (population > 0) 
            {
                leftBuildingText.text = selectedPlanet.turnsToBuild.ToString() + " turn" + ((selectedPlanet.turnsToBuild>1) ? "s" : "") + " left to build.";
            }
            else 
            {
                // When Play is pressed, let the text hang until the next turn begins
                leftBuildingText.text = "1 turn left to build.";
            }
		}
	}

	/* Set Right Panel
	   ========================================================================== */
    public void SetPlayInteractive (bool canSimulate = false)
    {
        rightNextTurnButton.interactable = canSimulate;
    }

	/* Set External Scripts
	   ========================================================================== */
    private void SetupTooltipController ()
    {
        // Right Panel
        SetTooltip((RectTransform)rightStoneButton.transform, Constants.TOOLTIP_RIGHT_STONE_BUTTON);
        SetTooltip((RectTransform)rightWaterButton.transform, Constants.TOOLTIP_RIGHT_WATER_BUTTON);
        SetTooltip((RectTransform)rightGasButton.transform, Constants.TOOLTIP_RIGHT_GAS_BUTTON);
        SetTooltip((RectTransform)rightNextTurnButton.transform, Constants.TOOLTIP_RIGHT_NEXT_TURN_BUTTON);
        SetTooltip((RectTransform)rightMission1Button.transform, Constants.TOOLTIP_RIGHT_MISSION_1);
        SetTooltip((RectTransform)rightMission2Button.transform, Constants.TOOLTIP_RIGHT_MISSION_2);
        SetTooltip((RectTransform)rightMission3Button.transform, Constants.TOOLTIP_RIGHT_MISSION_3);
        // Left Panel
        SetTooltip((RectTransform)leftTechnology1Button.transform, Constants.TOOLTIP_LEFT_TECHNOLOGY_1_BUTTON);
        SetTooltip((RectTransform)leftTechnology2Button.transform, Constants.TOOLTIP_LEFT_TECHNOLOGY_2_BUTTON);
        SetTooltip((RectTransform)leftTechnology3Button.transform, Constants.TOOLTIP_LEFT_TECHNOLOGY_3_BUTTON);
        SetTooltip((RectTransform)leftTechnology4Button.transform, Constants.TOOLTIP_LEFT_TECHNOLOGY_4_BUTTON);
        SetTooltip((RectTransform)leftTechnology5Button.transform, Constants.TOOLTIP_LEFT_TECHNOLOGY_5_BUTTON);
    }

    public void SetMission1()
    {

    }

    private void SetTooltip(RectTransform rt, string myString) {
        TooltipController tc = rt.gameObject.AddComponent<TooltipController>();
        tc.myString = myString;
    }


   	/* Update Left Panel
	   ========================================================================== */ 
    public void UpdateResources ()
    {
        // Planet selected
        if (selectedPlanet != null)
        {
            SetSelectedResources(selectedPlanet.carbon, selectedPlanet.nitrogen, selectedPlanet.hydrogen);
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
        if (selectedPlanet != null)
        {
            SetSelectedPopulation(selectedPlanet.population);
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
        if (selectedPlanet != null)
        {
            SetSelectedHealth(selectedPlanet.health, selectedPlanet.maxHealth);
        }
        // No planet selected
        else
        {
            SetSelectedHealth();
        }
    }

    /* Animations
    ========================================================================== */ 
    public void OpenLeftPanel() 
    {
        Animator anim = leftPanel.GetComponent<Animator>();
        anim.enabled = true;
        anim.Play("LeftPanelSlideIn");
    }

    public void CloseLeftPanel() 
    {
        Animator anim = leftPanel.GetComponent<Animator>();
        anim.enabled = true;
        anim.Play("LeftPanelSlideOut");
    }
}