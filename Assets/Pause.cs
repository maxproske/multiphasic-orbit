using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour {

	void ifPause(){
		if (!gameObject.activeSelf) {
			UnPause ();
		}
	}

	IEnumerator UnPause(){
		yield return new WaitForSeconds(3);
		gameObject.SetActive (true);
	}
}
