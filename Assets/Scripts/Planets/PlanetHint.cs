using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Track events through event systems
using UnityEngine.EventSystems;

public class PlanetHint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public bool animateHalo = false;
	public bool pause = false;
	Light light;
	private bool mouseHover; // Tracks if mouse is hovering over panel
	private GameController gc; // Access Game Controller script

	// Use this for initialization
	void Start () {
		light = gameObject.GetComponent<Light>();
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		mouseHover = true;
		Debug.Log ("hover detected!");
	}
	public void OnPointerExit (PointerEventData eventData)
	{
		mouseHover = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (animateHalo) {
			if (mouseHover) {
				pause = true;
			} else {
				pause = false;
			}
			if (!pause) {
				Debug.Log ("animating!");
				light.range = Mathf.Abs (Mathf.Sin (Time.time * 4) * 3);
				light.intensity = Mathf.Abs (Mathf.Sin (Time.time * 4) * 2);
			} else {
				light.range = 0;
				light.intensity = 0;
			}
		} else {
			light.range = 0;
			light.intensity = 0;
		}
	}
}
