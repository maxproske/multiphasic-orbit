using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseOnBlur : MonoBehaviour {

	// Check whether or not the mouse is hovering over the canvas panel
	private bool mouseHover;

	public void SetHover (bool hover) {
		mouseHover = hover;
	}

	// Close itself when player clicks and it is off the panel
	void Update () {
		// Player is finished clicking
		if (Input.GetMouseButtonUp (0) && !mouseHover) {
			gameObject.SetActive (false);
		}
	}
}
