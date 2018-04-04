using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndOfMission : MonoBehaviour
{
    public Text title;
    public Text message;
    public Text buttonText;

    public Button nextLevelButton;

    public GameObject panel;

    private void Start()
    {
        panel.SetActive(false);
        SetPanelProperties();
    }

    public void ShowPanel()
    {
        panel.SetActive(true);
    }

    private void SetPanelProperties()
    {
        nextLevelButton.onClick.AddListener(NextLevel);
        switch (GameController.level)
        {
            case 1:
                Debug.Log("Level 1");
                title.text = "Learner's Test Complete!";
                message.text = "Congratulations on passing your Learner's Test! You are rewarded with Learner's certification, and are now eligible to take your Novice test.";
                buttonText.text = "Start Novice Test!";
                break;
            case 2:
                title.text = "Novice Test Complete!";
                message.text = "You have defended your planet and the solar system is in peace again. You have passed the Novice test, and have earned Novice certification. You are now eligible to take your full license. Remember to link different planet types together to avoid creating hostile Rogue planets.";
                buttonText.text = "Start FINAL Test!";
                break;
            case 3:
                break;
            default:
                Debug.Log("Default");
                title.text = "Learner's Test Complete!";
                message.text = "Congratulations on passing your Learner's Test! You are rewarded with Learner's certification, and are now eligible to take your Novice test.";
                buttonText.text = "Start Novice Test!";
                break;
        }

    }

    private void NextLevel()
    {
        switch (GameController.level)
        {
            case 1:
                SceneManager.LoadScene("Level2");
                break;
            case 2:
                SceneManager.LoadScene("Final");
                break;
            case 3:
                break;
            default:
                break;
        }
        
    }
}
