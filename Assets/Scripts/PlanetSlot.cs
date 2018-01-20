using UnityEngine;
// We are using Unity UI
using UnityEngine.UI;

public class PlanetSlot : MonoBehaviour {

	public Image icon;
	private Planet planet;

	public void AddPlanet(Planet newPlanet) {
		planet = newPlanet;
		icon.sprite = planet.icon;
		icon.enabled = true;
	}

	public void ClearSlot () {
		planet = null;
		icon.sprite = null;
		icon.enabled = false;
	}
}
