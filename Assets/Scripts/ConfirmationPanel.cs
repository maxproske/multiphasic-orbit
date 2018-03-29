using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ConfirmationPanel : MonoBehaviour
{
    private GameController gc;
    public GameObject panel;
    public Text titleText;
    public Text messageText;
    public Button confirmButton;

    private void Awake()
    {
        // GameController
        gc = GameObject.Find("Game Manager").GetComponent<GameController>();
        gc.cp = this;
    }

    private void Start()
    {
        confirmButton.onClick.AddListener(ClosePanel);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Escape))
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
        panel.SetActive(false);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("Level2");
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main");
    }

    public void Final()
    {
        SceneManager.LoadScene("Final");
    }
}
