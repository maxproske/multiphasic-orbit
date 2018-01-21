using UnityEngine;
// We are using Unity UI
using UnityEngine.UI;

public class PlanetSlot : MonoBehaviour {

	public Image background;
	public Button button;
	private Planet planet;

	public void Enable (Planet newPlanet) {
		planet = newPlanet;
		button.interactable = true;
		button.image = background;
	}

	public void Disable () {
		planet = null;
		button.interactable = false;
		button.image = background;
	}
}
