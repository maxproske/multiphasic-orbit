using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MacroSkillTree : MonoBehaviour {

	// Singleton pattern
	public static MacroSkillTree instance;

	// Called on add and remove planet
	public delegate void OnItemChanged();
	public OnItemChanged onItemChangedCallback;

	// Maximum slots in skill ttree
	public int maxSlots = 4;

	// Public list of planets
	public List<Planet> planets = new List<Planet>();


	void Awake() {
		// Make sure there is only one instance of the macro skill tree
		if (instance != null) {
			Debug.LogWarning ("More than one instance of MacroSkillTree found!");
			return;
		}
		// Create a static variable shared by all instances of the class
		// We can now use MacroSkillTree.instance.Add(item) from other classes, instead of the very expensive FindObjectOfType<Planet>().Add(item)
		instance = this;
	}

	void Setup() {
		for (int i = 0; i < maxSlots; i++) {
			bool addedPlanet = Add(new Planet());
		}
	}

	public bool Add (Planet planet) {
		bool added = false;
		Debug.Log (planets.Count);
		Debug.Log(maxSlots);
		if (planets.Count < maxSlots) {
			// We have enough room, add planet
			planets.Add (planet);

			// Call delegate
			if (onItemChangedCallback != null) {
				onItemChangedCallback.Invoke();
			}

			added = true;
		} else {
			// Not enough slots free
			Debug.Log("Not enough room.");
			added = false;
		}
		return added;
	}

	public void Remove (Planet planet) {
		planets.Remove (planet);

		// Call delegate
		if (onItemChangedCallback != null) {
			onItemChangedCallback.Invoke();
		}
	}

}
