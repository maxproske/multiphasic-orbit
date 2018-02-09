﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Track events through event systems
using UnityEngine.EventSystems;

// Implement pointer interfaces programatically, instead of Scene event triggers
public class PlanetarySkillTreeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	private GameController gc;

	public GameObject macroSkillTree; // Reference to macro skill tree
	private bool mouseHover; // Tracks if mouse is hovering over panel

	// Following code is generated by right clicking IPointerEnter/ExitHandler, and selecting "Refactor"
	public void OnPointerEnter (PointerEventData eventData) {
		mouseHover = true;
	}
	public void OnPointerExit (PointerEventData eventData) {
		mouseHover = false;
	}

	void Awake() {
		gc = GameObject.Find ("Game Manager").GetComponent<GameController> ();
	}

	void Update () {
		// When player clicks on the icon
		if (Input.GetMouseButtonUp (0) && mouseHover) {
			// Toggle the panel
			macroSkillTree.SetActive (!macroSkillTree.activeSelf);

			// Switch the hint indicator
			if (macroSkillTree.activeSelf && gc.GAME_STATE == Constants.TURN_1_SKILL_TREE) {
				gc.GAME_STATE = Constants.TURN_1_PLANET_SLOT;
			}
			// Handle player clicking skill tree when skill tree is open
			else if (gc.GAME_STATE == Constants.TURN_1_PLANET_SLOT) {
				gc.GAME_STATE = Constants.TURN_1_SKILL_TREE;
			}
			else if (gc.GAME_STATE == Constants.TURN_2_SKILL_TREE) {
				gc.GAME_STATE = Constants.TURN_2_PLANET_SLOT;
			}
		}
	}
}
