using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class loadscene : MonoBehaviour
{
    private void Start()
    {
        this.GetComponent<Button>().onClick.AddListener(LoadScene);
    }

    private void LoadScene()
    {
        switch (this.name)
        {
            case "New Game":
                SceneManager.LoadScene("Cutscene1");
                break;
            case "Level 1":
                SceneManager.LoadScene(Constants.LEVEL_1_SCENE_NAME);
                break;
            case "Level 2":
                SceneManager.LoadScene(Constants.LEVEL_2_SCENE_NAME);
                break;
            case "Level 3":
                SceneManager.LoadScene(Constants.LEVEL_3_SCENE_NAME);
                break;
            default:
                SceneManager.LoadScene("Cutscene1");
                break;
        }
    }
}
