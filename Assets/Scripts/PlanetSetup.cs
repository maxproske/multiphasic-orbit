using UnityEngine;

public class PlanetSetup : MonoBehaviour {

	public Planet planet;

	void Setup() {
		bool wasAdded = MacroSkillTree.instance.Add (planet);
		Debug.Log (wasAdded);
	}
}
