using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    private GameController gc;
    public Text LogText;
    string color;

    // Use this for initialization
    void Start()
    {
        gc = GameObject.Find("Game Manager").GetComponent<GameController>();
        LogText.text = "";
        color = "#fff";
    }

    public void ToggleLog()
    {
        if (gc.log.activeSelf)
        {
            gc.log.SetActive(false);
        } else
        {
            gc.log.SetActive(true);
        }
    }

    public void UpdateLogPlanet(string planetName, string message)
    {
        
        if (planetName.Contains("Carbon"))
        {
            color = "#616161FF";
        }

        if (planetName.Contains("Nitrogen"))
        {
            color = "#2196F3FF";
        }

        if (planetName.Contains("Hydrogen"))
        {
            color = "#795548FF";
        }

        AddTurnToLog();
        LogText.text += "Planet ";
        LogText.text += ChangeColor(color, planetName);
        LogText.text += " " + message;

        AddNewLine();
    }

    public void UpdateLogTrade(string planetName1, string planetName2, string message)
    {

    }

    public void UpdateLogTech(string planetName, string message)
    {

    }

    public void UpdateLogMission(string missionName, string missionReward)
    {
        color = "#f2dd54";
        AddTurnToLog();
        LogText.text += "Mission ";
        LogText.text += ChangeColor(color, missionName);
        LogText.text += " completed!";

        if (missionReward != "")
        {
            LogText.text += "<b>" + " Reward: " + missionReward + "</b>";
        }

        AddNewLine();
    }

    private string ChangeColor(string color, string text)
    {
        return "<color=" + color + ">" + text + "</color>";
    }

    private void AddTurnToLog()
    {
        LogText.text += "[Turn " + gc.turn + "] ";
    }

    private void AddNewLine()
    {
        LogText.text += "\r\n";
    }

    // mission
    // trading
    // technology
    // planet
    // highlight planets, technology
    // log turn
}
