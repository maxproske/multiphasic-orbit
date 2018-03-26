using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UninteractiveChildren : MonoBehaviour {

    private Button myButton;
    private bool last;
    private bool current;

    void Start ()
    {
        myButton = this.GetComponent<Button>();
		//myButton.onClick.AddListener(Clicked);

        last = current = myButton.IsInteractable();
    }

    void Update()
    {
        current = myButton.IsInteractable();

        // If interactable set alpha to normal
        if (last != current)
        {
            Debug.Log("Button state changed!");
            Clicked();
        }

        last = current;
    }

    public void Clicked()
    {
        // Button was clicked
        //Debug.Log(myButton.IsInteractable());

        Text myText = myButton.GetComponentInChildren<Text>();
        if (myText != null) {
            // Button contains text
            Debug.Log("Button contains text.");
            Color color = myText.color;
            color.a = myButton.IsInteractable() ? 1f : 0.09f;
            myText.color = color;
        }
        else
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
}
