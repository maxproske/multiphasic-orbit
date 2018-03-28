using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPanel : MonoBehaviour
{
    public GameObject panel;
    public Text titleText;
    public Text messageText;
    public Button confirmButton;

    private void Start()
    {
        confirmButton.onClick.AddListener(ClosePanel);
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
}
