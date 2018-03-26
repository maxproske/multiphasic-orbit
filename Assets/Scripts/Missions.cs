using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missions : MonoBehaviour
{
    // used to access gc
    private GameController gc;

    // Use this for initialization
    void Start()
    {
        gc = GameObject.Find("Game Manager").GetComponent<GameController>();
    }

    // use this to check if mission requirements have been fulfilled
    public void OnNotify(int mission)
    {
        switch (mission)
        {
            case Constants.MISSION_1:
                Debug.Log("Checking Mission " + Constants.MISSION_1 + " progress...");
                if (gc.planets.Count > 0)
                {
                    Reward(mission);
                    Complete(Constants.MISSION_1);
                }
                break;
        }
    }

    // used to reward
    public void Reward(int mission)
    {
        switch (mission)
        {
            case Constants.MISSION_1:
                
                break;
        }
    }

    // used to remove mission from in-progress list and add to completed list
    private void Complete(int mission)
    {
        Debug.Log("Mission " + mission + " completed!");
        gc.missionsInProgress.Remove(mission);
        gc.missionsCompleted.Add(mission);
    }
}
