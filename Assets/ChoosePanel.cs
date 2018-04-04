using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChoosePanel : MonoBehaviour
{
	private GameController gc;
	public GameObject panel;
	public Text titleText;
	public Text messageText;
	public Button yesButton;
	public Button noButton;
	public int yesorno=0;

	private void Awake()
	{
		// GameController
		gc = GameObject.Find("Game Manager").GetComponent<GameController>();
		gc.cp2 = this;
	}

	private void Start()
	{
		yesButton.onClick.AddListener(ClosePanely);
		noButton.onClick.AddListener(ClosePaneln);
	
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ClosePanel();
		}
	}

	public void ShowPanel(string title, string message)
	{
		panel.SetActive(true);
		titleText.text = title;
		messageText.text = message;
	}

	public void ShowPanel(string title, string message, string hint)
	{
		panel.SetActive(true);
		titleText.text = title;
		messageText.text = message;
		messageText.text += "\r\n" + hint;
	}

	private void ClosePanel()
	{
		yesorno =0;
		panel.SetActive(false);

		// Update game state on panel close
		//Debug.Log(gc.GAME_STATE);

		if (GameController.level == 1 && gc.GAME_STATE == Constants.LEARNERS_MISSION_1) gc.GAME_STATE = Constants.LEARNERS_MISSION_2;
	}


	private void ClosePanely()
	{
		yesorno = 1;
		panel.SetActive(false);

		// Update game state on panel close
		//Debug.Log(gc.GAME_STATE);

		if (GameController.level == 1 && gc.GAME_STATE == Constants.LEARNERS_MISSION_1) gc.GAME_STATE = Constants.LEARNERS_MISSION_2;
	}
	private void ClosePaneln()
	{
		yesorno = -1;
		panel.SetActive(false);

		// Update game state on panel close
		//Debug.Log(gc.GAME_STATE);

		if (GameController.level == 1 && gc.GAME_STATE == Constants.LEARNERS_MISSION_1) gc.GAME_STATE = Constants.LEARNERS_MISSION_2;
	}


}
