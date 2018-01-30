//https://www.youtube.com/watch?v=lKfqi52PqHk



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class OrbitMotion : MonoBehaviour {

	public Transform orbitingObject;
	public EllipseTester orbitPath;
	public int resource = 0;
	[Range(0f,1f)]
	public float orbitProgress;
	public float orbitPeriod;
	public bool orbitActive =false;
//	public Button nextButton;
	public bool ifNext=false;
	private float startTime;
	private Button btn;
	// Use this for initialization
	void Start () {
		if(orbitingObject ==null){
			orbitActive = false;
			return;
		}
		btn = GameObject.Find ("NextTurn").GetComponent<Button>();
		btn.onClick.AddListener (goNext);
		SetOrbitingObjectPosition ();

	}

	void FixedUpdate(){
		
		float ctime = Time.time;
		if (ctime - startTime < 3) {
			
			if (ifNext == true ) {
				orbitActive = true;
				StartCoroutine (AnimateOrbit ());


			}

		} else {
			orbitActive = false;
			ifNext = false;
		}
	}
	void SetOrbitingObjectPosition(){
		Vector2 orbitPos = orbitPath.Evaluate(orbitProgress);
		orbitingObject.localPosition = new Vector3 (orbitPos.x, 0, orbitPos.y);


	}

	void goNext(){
		resource++;
		startTime = Time.time;
		ifNext = true;
		
	}
	IEnumerator AnimateOrbit(){
		if (orbitPeriod < 0.1f) {
			orbitPeriod = 0.1f;
		}
		float orbitSpeed = 1f / orbitPeriod;
	
			orbitProgress += Time.deltaTime * orbitSpeed;
			orbitProgress %= 1f;
			SetOrbitingObjectPosition ();
			yield return null;
	
	}
	

}
