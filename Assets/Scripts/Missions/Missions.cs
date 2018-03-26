using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missions : MonoBehaviour
{
    // used to access gc
    private GameController gc;
    private Mission m;

    // Use this for initialization
    void Start()
    {
        gc = GameObject.Find("Game Manager").GetComponent<GameController>();
        m = GameObject.Find("Missions").GetComponent<Mission>();
    }

    // use this to check if mission requirements have been fulfilled
    public void OnNotify(GameObject mission)
    {
        m = mission.GetComponent<Mission>();
        switch (m.missionName)
        {
            case "Place Planet":
                Debug.Log("Checking Mission: " + m.missionName + " progress...");
                if (gc.planets.Count > 0)
                {
                    Complete(mission);
                    Reward(mission);
                } else
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
    }
}
