using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadscene : MonoBehaviour {

	// Use this for initialization
	public Button yourButton;

	void Start()
	{
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(LoadLevel);
	}

	void LoadLevel()
	{
		SceneManager.LoadScene("Cutscene1");
	}
}
