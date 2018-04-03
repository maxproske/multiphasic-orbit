using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Log : MonoBehaviour
{
    private GameController gc;
    public Text LogText;
    string color;
    string planetColor;
    string stoneColor;
    string waterColor;
    string gasColor;
    public string textToLog;
    public ScrollRect myScrollRect;
    public GameObject log;

    // Use this for initialization
    void Start()
    {
        gc = GameObject.Find("Game Manager").GetComponent<GameController>();
        gc.l = this;
        LogText.text = "";
        color = "#fff";
        stoneColor = "#616161FF";
        waterColor = "#2196F3FF";
        gasColor = "#795548FF";
        textToLog = "";
        ToggleLog();

        ToLog("<b>Turn 1</b>");
        LogBackLog();
    }

    public void ToggleLog()
    {
        if (log.activeSelf)
        {
            log.SetActive(false);
        }
        else
        {
            log.SetActive(true);
        }

        myScrollRect.verticalNormalizedPosition = 0f; // scroll to bottom
    }

    public void UpdateLogPlanet(string planetName, string message)
    {
        if (planetName.Contains("Carbon"))
        {
            planetColor = stoneColor;
        }

        if (planetName.Contains("Nitrogen"))
        {
            planetColor = waterColor;
        }

        if (planetName.Contains("Hydrogen"))
        {
            planetColor = gasColor;
        }

        // backlog text first and then log
        ToLog(AddNewLine());
        ToLog("Planet ");
        ToLog(ChangeColor(planetColor, planetName));
        ToLog(" " + message);
    }

    public void UpdateLogPlanetRes(string planetName, int stone, int water, int gas)
    {
        if (planetName.Contains("Stone"))
        {
            planetColor = stoneColor;
        }

        if (planetName.Contains("Water"))
        {
            planetColor = waterColor;
        }

        if (planetName.Contains("Gas"))
        {
            planetColor = gasColor;
        }

        // backlog text first and then log
        ToLog(AddNewLine());
        ToLog("Planet ");
        ToLog(ChangeColor(planetColor, planetName));
        ToLog(" has collected: ");
        ToLog(ChangeColor(stoneColor, stone.ToString() + " Stone "));
        ToLog(ChangeColor(waterColor, water.ToString() + " Water "));
        ToLog(ChangeColor(gasColor, gas.ToString() + " Gas "));
    }

    public void UpdateLogTrade(string planetName1, string planetName2, string message)
    {

    }

    public void UpdateLogTech(string planetName, string tech, string effect)
    {
        if (planetName.Contains("Stone"))
        {
            planetColor = stoneColor;
        }

        if (planetName.Contains("Water"))
        {
            planetColor = waterColor;
        }

        if (planetName.Contains("Gas"))
        {
            planetColor = gasColor;
        }

        color = "#2fbf18";
        ToLog(AddNewLine());
        ToLog("Planet ");
        ToLog(ChangeColor(planetColor, planetName));
        ToLog(" has learned ");
        ToLog(ChangeColor(color, tech) + "!");
        ToLog(" " + "<b>" + effect + "</b>");
    }

    public void UpdateLogMission(string missionName, string missionReward)
    {
        color = "#f2dd54";
        // backlog text first and then log
        ToLog(AddNewLine());
        ToLog("Mission ");
        ToLog(ChangeColor(color, missionName));
        ToLog(" completed!");

        if (missionReward != "")
        {
            ToLog("<b>" + " Reward: " + missionReward + "</b>");
        }
    }

    private string ChangeColor(string color, string text)
    {
        return "<color=" + color + ">" + text + "</color>";
    }

    public string AddTurnToLog()
    {

        return "\r\n\r\n<b>Turn " + gc.turn + "</b>";
    }

    private string AddNewLine()
    {
        if (LogText.text != "")
        {
            return "\r\n";
        }
        else
        {
            return null;
        }
    }

    public void ToLog(string text)
    {
        textToLog += text;
    }

    public void LogBackLog()
    {
        LogText.text += textToLog;
        textToLog = ""; // reset textToLog
        myScrollRect.verticalNormalizedPosition = 0f; // scroll to bottom
    }
}
