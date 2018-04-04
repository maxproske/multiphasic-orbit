using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildButtons : MonoBehaviour {

    private GameController gc;
    private Button myButton;
    private bool last;
    private bool current;

    void Start ()
    {
        gc = GameObject.Find ("Game Manager").GetComponent<GameController> ();
        myButton = this.GetComponent<Button>();
		myButton.onClick.AddListener(OnClick);

        last = myButton.IsInteractable();
        current = myButton.IsInteractable();
    }

    void Update()
    {
        current = myButton.IsInteractable();

        // If interactable set alpha to normal
        if (last != current)
        {
            Clicked();
        }

        last = current;
    }

    // Made another onclick function that is only triggered for the purpose of MISSIONS
    private void OnClick()
    {
        //Debug.Log("Clicked");

        // Dismiss tooltip when clicking Tech 1
        if(GameController.level == 1 && gc.turn == 2 && gameObject.name == "Tech 1")
        {
            //Debug.Log("Set Constants.LEARNERS_MISSION_3");
            gc.GAME_STATE = Constants.LEARNERS_MISSION_3; // Point at end turn button
        }
        // Dismiss tooltip when clicking Tech 1
        else if(GameController.level == 1 && gc.turn >= 3 && gc.GAME_STATE == Constants.LEARNERS_MISSION_5 && gameObject.name == "Tech 2")
        {
            //Debug.Log("Constants.LEARNERS_MISSION_5_PLAY");
            gc.GAME_STATE = Constants.LEARNERS_MISSION_5_PLAY; // Point at end turn button
            gc.numInterplanetaryNetworking++; // Flag to help Learner's test know when to tell the player to start linking
        }
        // Set this condition so numInterplanetaryNetworking doesn't increment past 2
        if (GameController.level == 1 && gameObject.name == "Tech 2" && gc.GAME_STATE == Constants.LEARNERS_MISSION_6) 
        {
            gc.GAME_STATE = Constants.LEARNERS_MISSION_6_PLAY;
            gc.numInterplanetaryNetworking++; // Flag to help Learner's test know when to tell the player to start linking
        }

        //Debug.Log("gc.numInterplanetaryNetworking: " + gc.numInterplanetaryNetworking);

        // Button was clicked
        //Debug.Log(myButton.IsInteractable());
    }

    public void Clicked()
    {
        Text myText = myButton.GetComponentInChildren<Text>();
        if (myText != null) {
            // Button contains text
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
