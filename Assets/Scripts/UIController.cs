using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
	// Status Panel
	public Text rightPopulationText;
	public Text rightHealthText;

	/* Declare Bottom Panel
	   ========================================================================== */

	/* Declare Left Panel
	   ========================================================================== */

	private void Awake()
	{
		/* Initialize Top Panel
	       ====================================================================== */
		SetTurn (1);
		SetDistance (20);

		/* Initialize Right Panel
	       ====================================================================== */
		SetSelected (null);

		/* Initialize Bottom Panel
		   ====================================================================== */

		/* Initialize Left Panel
		   ====================================================================== */
	}

	/* Set Top Panel
	   ========================================================================== */
	public void SetTurn(int turn)
	{
		topTurnText.text = "Turn " + turn.ToString ();
	}

	public void SetDistance(int distance)
	{
		topDistanceText.text = distance.ToString() + " uu";
	}

	/* Set Right Panel
	   ========================================================================== */
	public void SetSelected(Planet p)
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
		}
		// No planet selected
		else 
		{
			// Replace placeholder values with default values
			SetSelectedName ();
			SetSelectedPreview ();
			SetSelectedResources ();
			SetSelectedPopulation ();
			SetSelectedHealth ();
		}
	}

	private void SetSelectedName(string name = null)
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

	private void SetSelectedPreview(Sprite sprite = null)
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

	private void SetSelectedResources(int carbon = -1, int nitrogen = -1, int hydrogen = -1)
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

	private void SetSelectedPopulation(int population = -1)
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

	private void SetSelectedHealth(int health = -1, int maxHealth = -1)
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

	/* Set Bottom Panel
	   ========================================================================== */

	/* Set Left Panel
	   ========================================================================== */
}
