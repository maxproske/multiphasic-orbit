using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour {

    private Button myButton;
    private bool last;
    private bool current;

    void Start ()
    {
        myButton = this.GetComponent<Button>();
        last = current = myButton.IsInteractable();
        Clicked();
    }

    void Update()
    {
        current = myButton.IsInteractable();
        if (last != current)
        {
            Clicked();
        }
        last = current;
    }

    public void Clicked()
    {
        Image[] myImage = myButton.GetComponentsInChildren<Image>();
        if (myImage != null)
        {
            // Button contains an image
            Color color = myImage[1].color;
            color.a = myButton.IsInteractable() ? 1f : 0.09f;
            myImage[1].color = color;
        }
    }
}