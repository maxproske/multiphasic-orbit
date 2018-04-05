using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ToMenu : MonoBehaviour
{

    public Button yourButton;

    void Start()
    {

        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(LoadLevel);
    }

    void LoadLevel()
    {
        SceneManager.LoadScene("title");
    }
}