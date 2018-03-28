using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Missions : MonoBehaviour
{
    // used to access gc
    private GameController gc;
    private Mission m;
    private Planet p;
    public GameObject confirmationPanel;
    private ConfirmationPanel cp;

    public List<GameObject> missions; // list of missions that will be played with
    public List<GameObject> Test1Missions; // add these missions into the missions
    public List<GameObject> Test2Missions;
    public List<GameObject> Test3Missions;

    // Use this for initialization
    void Start()
    {
        gc = GameObject.Find("Game Manager").GetComponent<GameController>();
        m = GameObject.Find("Missions").GetComponent<Mission>();
        cp = confirmationPanel.GetComponent<ConfirmationPanel>();
        cp.ShowPanel("Learner's Test Begins", "Build one planet to get started!");
    }

    // use this to check if mission requirements have been fulfilled
    public void OnNotify(GameObject mission)
    {
        m = mission.GetComponent<Mission>();
        switch (m.missionName)
        {
            case "Place Planet":
                //Debug.Log("Checking Mission: " + m.missionName + " progress...");
                if (gc.planets.Count > 0)
                {
                    Complete(mission);
                    Reward(mission);
                } else
                {
                    //Debug.Log("Mission: " + m.missionName + " incomplete.");
                }
                break;
            case "Open Planet Properties Panel":
                //Debug.Log("Checking Mission: " + m.missionName + " progress...");
                if (gc.selected != null && gc.selected.CompareTag("Planet"))
                {
                    Complete(mission);
                    Reward(mission);
                }
                else
                {
                    //Debug.Log("Mission: " + m.missionName + " incomplete.");
                }
                break;
            case "Simulate":
                //Debug.Log("Checking Mission: " + m.missionName + " progress...");
                if (gc.simulate)
                {
                    Complete(mission);
                    Reward(mission);
                }
                else
                {
                    //Debug.Log("Mission: " + m.missionName + " incomplete.");
                }
                break;
            case "Assign a Planet to Learn High Energy Magnetics":
                //Debug.Log("Checking Mission: " + m.missionName + " progress...");

                foreach (var planet in gc.planets)
                {
                    p = planet.GetComponent<Planet>();

                    if (p.moreResource)
                    {
                        Complete(mission);
                        Reward(mission);
                    }
                    else
                    {
                        //Debug.Log("Mission: " + m.missionName + " incomplete.");
                    }
                }
                break;
            case "Assign a Planet to Learn a Negative Mass Mechanics":
                //Debug.Log("Checking Mission: " + m.missionName + " progress...");
                //if (gc.planets.Count > 0)
                //{
                //    Complete(mission);
                //    Reward(mission);
                //}
                //else
                //{
                //    Debug.Log("Mission: " + m.missionName + " incomplete.");
                //}
                break;
            case "Unknown Mission":
                Debug.Log("Checking Mission: " + m.missionName + " progress...");
                if (gc.linksuccessful)
                {
                    Complete(mission);
                    Reward(mission);
                }
                else
                {
                    Debug.Log("Mission: " + m.missionName + " incomplete.");
                }
                break;
        }
    }

    // used to reward
    public void Reward(GameObject mission)
    {
        m = mission.GetComponent<Mission>();
        switch (m.missionName)
        {
            case "Place Planet":
                Debug.Log("Place Planet Reward: Here's a pat on the back.");
                break;
        }
    }

    // used to remove mission from in-progress list and add to completed list
    private void Complete(GameObject mission)
    {
        m = mission.GetComponent<Mission>();
        Debug.Log("Mission: " + m.missionName + " completed!");
        m.completed = true;

        // update respective button colour
        GameObject ms = GameObject.Find(mission.name + "(Clone)"); // find the instantiated game object of the same name that is set as a child to one of the mission buttons
        Button button = ms.transform.parent.transform.Find("Mission Button").GetComponent<Button>(); // get the particular mission button
        ColorBlock cb = button.GetComponent<Button>().colors; 
        cb.normalColor = new Color(0.298f, 0.686f, 0.313f); // set the button color to same green as play button
        button.colors = cb;

        if (m.postMissionHint != "") // if there is post mission hint, show confirmation panel with message and hint - assumes when there is a hint, there is a message
        {
            cp.ShowPanel("Mission: " + m.missionName + " completed!", m.postMissionMessage, m.postMissionHint);
        } else if (m.postMissionMessage != "")// else just show confirmation panel with just message
        {
            cp.ShowPanel("Mission: " + m.missionName + " completed!", m.postMissionMessage);
        }
        
    }

    public void CheckMissions(List<GameObject> missionsList)
    {
        foreach (var mission in missionsList)
        {
            if (!mission.GetComponent<Mission>().completed)
            {
                OnNotify(mission);
            }
        }
    }
}
