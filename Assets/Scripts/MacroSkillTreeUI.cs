using UnityEngine;

public class MacroSkillTreeUI : MonoBehaviour {

	public Transform planetsParent;

	// Reference to macro skill tree
	MacroSkillTree macroSkillTree;

	PlanetSlot[] slots;

	// Use this for initialization
	void Start () {
		Debug.Log("== Entering MacroSkillTreeUI.Start() ==");
		macroSkillTree = MacroSkillTree.instance;
		// Trigger whenever a new planet has been added
		macroSkillTree.onItemChangedCallback += UpdateUI;

		slots = planetsParent.GetComponentsInChildren<PlanetSlot> ();

		UpdateUI ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateUI () {
		Debug.Log("== UPDATING UI ==");
		// Iterate through slots
		for (int i = 0; i < slots.Length; i++) {
			if (i < macroSkillTree.planets.Count) {
				// Pass the corresponding planet into that slot
				slots [i].AddPlanet (macroSkillTree.planets [i]);
			} else {
				slots [i].ClearSlot ();
			}
		}
	}
}
