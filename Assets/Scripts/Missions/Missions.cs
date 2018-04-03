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
    private Rogue r;
    public GameObject confirmationPanel;
    private ConfirmationPanel cp;
    public bool reward;
    private int rogueDieIncrement;

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
        reward = false;
        rogueDieIncrement = 0;

        InitializeMissions();
    }

    public void CPShownAtStartOfLevel()
    {
        switch (gc.level)
        {
            case 1:
                cp.ShowPanel("Welcome to the Learner's Test", "Left Click - Navigation\r\nRight Click - Rotate Camera\r\nScroll Wheel - Zoom in and out\r\nTAB - Open Mission Log\r\n\r\nSelect a planet type to start building your solar system!");
                cp.confirmButton.onClick.AddListener(cp.CheckMissions); // change function of button to change level/scene
                break;
            case 2:
                //cp.ShowPanel("N Test", "Sorry, N Test is currently not available...\r\n\r\nClick OK to replay!");
                //cp.confirmButton.onClick.AddListener(cp.Restart); // change function of button to change level/scene
                cp.ShowPanel("Novice Test", "It's going to be a bit more difficult now. Rogue planets may appear and attack and steal from your planets.");
                break;
            case 3:
                cp.ShowPanel("Full License Test", "An unknown amount of Rogue planets will appear. Eliminate them all to prove that you are worthy of a full license.");
                break;
            default:
                cp.ShowPanel("Welcome to the Learner's Test", "Left Click - Navigation\r\nRight Click - Rotate Camera\r\nScroll Wheel - Zoom in and out\r\nTAB - Open Mission Log\r\n\r\nSelect a planet type to start building your solar system!");
                break;
        }
    }

    private void InitializeMissions()
    {
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
            case 3:
                Debug.Log("Playing Level 3");
                // add missions from Test1Missions to missions to play with list called missions
                foreach (var mission in Test3Missions)
                {
                    gc.AddMissionsToUI(mission);
                }
                break;
            default:
                Debug.Log("Playing Level 1");
                // add missions from Test1Missions to missions to play with list called missions
                foreach (var mission in Test1Missions)
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
            case "A Whole New World":
                if (gc.planets.Count > 0)
                {
                    Complete(mission);
                    Reward(mission);
                }
                break;
            case "Living in a Simulation":
                //Debug.Log("Checking Mission: " + m.missionName + " progress...");
                if (gc.simulate)
                {
                    Complete(mission);
                    Reward(mission);
                }
                break;
            case "Planet in Training":
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
            case "Interplanetary Networking":
                //Debug.Log("Checking Mission: " + m.missionName + " progress...");

                foreach (var planet in gc.planets)
                {
                    p = planet.GetComponent<Planet>();

                    if (p.iflinkactive)
                    {
                        Complete(mission);
                        Reward(mission);
                    }
                }
                break;
            case "Binary Planets":
                if (gc.planets.Count > 1)
                {
                    Complete(mission);
                    Reward(mission);
                }
                break;
            case "Social Network":
                if (gc.planets.Count > 1)
                {
                    if (p.iflinkactive)
                    {
                        Complete(mission);
                        Reward(mission);
                    }
                }
                break;
            case "Together Forever":
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
                }
                break;

            //// level 2
            //case "Link With One Planet":
            //    foreach (var planet in gc.planets)
            //    {
            //        p = planet.GetComponent<Planet>();

            //        if (p.linkedWith.Count > 0)
            //        {
            //            Complete(mission);
            //            reward = true;
            //        }
            //    }
            //    if (reward)
            //    {
            //        Reward(mission);
            //    }
            //    break;
            //case "Learn Attack Tech":
            //    foreach (var planet in gc.planets)
            //    {
            //        p = planet.GetComponent<Planet>();
            //        if (p.ifattackactive)
            //        {
            //            Complete(mission);
            //            Reward(mission);
            //        }
            //    }
            //    break;
            //case "Defeat One Rogue Planet":
            //    foreach (var rogue in gc.roguePlanets)
            //    {
            //        r = rogue.GetComponent<Rogue>();
            //        if (r.die)
            //        {
            //            Complete(mission);
            //            Reward(mission);
            //            cp.ShowPanel("Rogue Planet Defeated", "Get ready for your final test!");
            //            cp.confirmButton.onClick.AddListener(cp.Final); // change function of button to change level/scene
            //            return;
            //        }

            //    }
            //    break;
            //case "Defeat 3 Rogue Planets Before Turn 100":
            //    if (gc.turn < 100)
            //    {
            //        foreach (var rogue in gc.roguePlanets)
            //        {
            //            r = rogue.GetComponent<Rogue>();
            //            if (r.die)
            //            {
            //                rogueDieIncrement++;

            //            }
            //            if (rogueDieIncrement > 2)
            //            {
            //                Complete(mission);
            //                Reward(mission);
            //                cp.ShowPanel("Congratulations!", "You now have a full license to go on and build solar systems. Be safe!\r\n\r\nClick OK to replay!");
            //                cp.confirmButton.onClick.AddListener(cp.Restart); // change function of button to change level/scene
            //                return;
            //            }

            //        }
            //    }
            //    break;
            default:
                break;
        }
    }

    // used to reward
    public void Reward(GameObject mission)
    {

        m = mission.GetComponent<Mission>();

        switch (m.missionName)
        {
            case "A Whole New World":
                // reward first planet with 2 stone
                gc.planets[0].GetComponent<Planet>().carbon += 2;
                reward = false;
                break;
        }

    }

    // used to remove mission from in-progress list and add to completed list
    private void Complete(GameObject mission)
    {
        m = mission.GetComponent<Mission>();
        //Debug.Log("Mission: " + m.missionName + " completed!");
        if (!m.completed)
        {
            m.completed = true;

            // update respective button colour
            GameObject ms = GameObject.Find(mission.name + "(Clone)"); // find the instantiated game object of the same name that is set as a child to one of the mission buttons
            Button button = ms.transform.parent.transform.Find("Mission Button").GetComponent<Button>(); // get the particular mission button
            ColorBlock cb = button.GetComponent<Button>().colors;
            cb.disabledColor = new Color(0.298f, 0.686f, 0.313f); // set the button color to same green as play button
            button.colors = cb;

            //if (m.postMissionHint != "") // if there is post mission hint, show confirmation panel with message and hint - assumes when there is a hint, there is a message
            //{
            //    cp.ShowPanel("Mission: " + m.missionName + " completed!", m.postMissionMessage, m.postMissionHint);
            //}
            //else if (m.postMissionMessage != "")// else just show confirmation panel with just message
            //{
            if (!gc.simulate)
            {
                cp.ShowPanel("Mission: " + m.missionName + " completed!", m.postMissionMessage, m.missionReward);
            }
            
            //}

            gc.l.UpdateLogMission(m.missionName, m.missionReward);
            gc.l.LogBackLog();
        }
    }

    public void CheckMissions(List<GameObject> missionsList)
    {
        foreach (var mission in missionsList)
        {
            if (!mission.GetComponent<Mission>().completed)
            {
                OnNotify(mission);
                gc.ui.UpdateSelectedPlanet();
            }
        }
    }
}
