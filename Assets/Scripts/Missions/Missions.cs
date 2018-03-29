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

        gc.m = this;

        switch (gc.level)
        {
            case 1:
                cp.ShowPanel("Learner's Test Begins", "Left Click - Navigation\r\nRight Click - Rotate Camera\r\nScroll Wheel - Zoom in and out\r\nTAB - Open Mission Log\r\n\r\n Build a planet to start!");
                break;
            case 2:
                cp.ShowPanel("N Test", "Sorry, N Test is currently not available...\r\n\r\nClick OK to replay!");
                cp.confirmButton.onClick.AddListener(cp.Restart); // change function of button to change level/scene
                break;
            default:
                cp.ShowPanel("Learner's Test Begins", "Build one planet to get started!");
                break;
        }

        switch (gc.level)
        {
            case 1:
                Debug.Log("Playing Level 1");
                // add missions from Test1Missions to missions to play with list called missions
                foreach (var mission in Test1Missions)
                {
                    gc.AddMissionsToUI(mission);
                }
                break;
            case 2:
                Debug.Log("Playing Level 2");
                // add missions from Test1Missions to missions to play with list called missions
                foreach (var mission in Test2Missions)
                {
                    gc.AddMissionsToUI(mission);
                }
                break;

        }

        // reset all missions to incompleted
        foreach (var mission in missions)
        {
            mission.GetComponent<Mission>().completed = false;
        }

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
                }
                else
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
            case "Assign a planet to learn Interplanetary Networking":
                //Debug.Log("Checking Mission: " + m.missionName + " progress...");

                foreach (var planet in gc.planets)
                {
                    p = planet.GetComponent<Planet>();

                    if (p.iflinkactive)
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
            case "Assign another planet to learn Interplanetary Networking":
                //Debug.Log("Checking Mission: " + m.missionName + " progress...");
                int planetsLearnIP = 0;
                foreach (var planet in gc.planets)
                {
                    p = planet.GetComponent<Planet>();

                    if (p.iflinkactive)
                    {
                        planetsLearnIP++;

                    }
                }
                if (planetsLearnIP > 1)
                {
                    Complete(mission);
                    Reward(mission);
                }
                else
                {
                    //Debug.Log("Mission: " + m.missionName + " incomplete.");
                }
                break;
            case "Successfully Link Two Planets":
                //Debug.Log("Checking Mission: " + m.missionName + " progress...");
                foreach (var planet in gc.planets)
                {
                    p = planet.GetComponent<Planet>();

                    if (p.linkedWith.Count > 0)
                    {
                        Complete(mission);
                        Reward(mission);
                        cp.ShowPanel("Link Successful!", "You're ready for the next test. Click OK to advance. Good luck...");
                        cp.confirmButton.onClick.AddListener(cp.NextLevel); // change function of button to change level/scene
                    }
                    else
                    {
                        //Debug.Log("Mission: " + m.missionName + " incomplete.");
                    }
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
            //case "Place Planet":
            //    Debug.Log("Place Planet Reward: Here's a pat on the back.");
            //    break;
        }
    }

    // used to remove mission from in-progress list and add to completed list
    private void Complete(GameObject mission)
    {
        m = mission.GetComponent<Mission>();
        //Debug.Log("Mission: " + m.missionName + " completed!");
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
        }
        else if (m.postMissionMessage != "")// else just show confirmation panel with just message
        {
            cp.ShowPanel("Mission: " + m.missionName + " completed!", m.postMissionMessage);
        }

        gc.l.UpdateLogMission(m.missionName, m.missionReward);
        gc.l.LogBackLog();

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
