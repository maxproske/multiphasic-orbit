using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIController : MonoBehaviour {

	/* Declare Top Panel
	   ========================================================================== */
	public Text turnText;
	public Text distanceText;

	/* Declare Right Panel
	   ========================================================================== */

	/* Declare Bottom Panel
	   ========================================================================== */

	/* Declare Left Panel
	   ========================================================================== */

	private void Start() {
		/* Initialize Top Panel
	       ====================================================================== */
		SetTurn (1);

		/* Initialize Right Panel
	       ====================================================================== */

		/* Initialize Bottom Panel
		   ====================================================================== */

		/* Initialize Left Panel
		   ====================================================================== */
	}

	/* Set Top Panel
	   ========================================================================== */
	public void SetTurn(int turn) {
		turnText.text = "Turn " + turn.ToString ();
	}

	public void SetDistance(int distance) {
		distanceText.text = distance.ToString() + " uu";
	}

	/* Set Right Panel
	   ========================================================================== */

	/* Set Bottom Panel
	   ========================================================================== */

	/* Set Left Panel
	   ========================================================================== */
}
