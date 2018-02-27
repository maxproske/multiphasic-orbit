using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tradetest : MonoBehaviour {
	public int C=0;
	public int B=0;
	public int A=0;

	public int adda;	
	public int addb;
	public int addc;

	public List<tradetest> planets;
	public bool iflinked = true;
	private bool ifpress=false;
	public Button EndTurnButton;
	public tradetest p2;
	private tradetest p;
	private Button btn;
	private int i=0;

	public int maxresource=0;
	public int maxresourcetype=0;

	// Use this for initialization
	void Start () {

		planets = new List<tradetest> ();

		p=p2.GetComponent<tradetest> ();
		planets.Add (p);
		btn = EndTurnButton.GetComponent<Button> ();
		btn.onClick.AddListener (TaskonClick);
	}


	void TaskonClick(){
		ifpress = true;
		tradeaction ();
	}	


	// Update is called once per frame
	void Update () {

	}

	void tradeaction(){
		if (ifpress == true) {
			for (int j = 0; j < planets.Count; j++) {
				i = 0;
				if (checklink (planets [i]) && i == 0) {
					trade (planets [i]);
					i = 1;
				}

			}
			ifpress = false;
		}
	}

	bool checklink(tradetest go){
		return iflinked;
	}
	void trade(tradetest go){
		
		getMaxresource ();
		go.getMaxresource ();

		if (maxresourcetype == 1) {
			go.A += 1;
			A -= 1;
		}else if (maxresourcetype == 2) {
			go.B += 1;
			B -= 1;
		}else if (maxresourcetype == 3) {
			go.C += 1;
			C -= 1;
		}

		if (go.maxresourcetype == 1) {
			A += 1;
			go.A -= 1;
		}else if (go.maxresourcetype == 2) {
			B += 1;
			go.B -= 1;
		}else if (go.maxresourcetype == 3) {
			C += 1;
			go.C -= 1;
		}
	}

	void getMaxresource(){
		
		maxresource=Mathf.Max (A, B, C);
		if (A == maxresource) {
			maxresourcetype =1 ;
		}
		if (B == maxresource) {
			maxresourcetype =2 ;
		}
		if (C == maxresource) {
			maxresourcetype =3 ;
		}
	}

}
