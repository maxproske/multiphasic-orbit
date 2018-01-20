using UnityEngine;

public class MacroSkillTreeUI : MonoBehaviour {

	// Reference to macro skill tree
	MacroSkillTree macrost;

	// Use this for initialization
	void Start () {
		macrost = MacroSkillTree.instance;
		// Trigger whenever a new planet has been added
		macrost.onItemChangedCallback += UpdateUI;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateUI () {
		Debug.Log("== UPDATING UI ==");
	}
}
