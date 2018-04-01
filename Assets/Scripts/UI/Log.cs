﻿using System.Collections;
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
    }

    public void ToggleLog()
    {
        myScrollRect.verticalNormalizedPosition = 0f; // scroll to bottom

        if (log.activeSelf)
        {
            log.SetActive(false);
        }
        else
        {
            log.SetActive(true);
        }
    }

    public void UpdateLogPlanet(string planetName, string message)
    {
        //AddNewLine();

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

        //AddTurnToLog();
        //LogText.text += "Planet ";
        //LogText.text += ChangeColor(planetColor, planetName);
        //LogText.text += " " + message;

        ToLog(AddNewLine());
        ToLog(AddTurnToLog());
        ToLog("Planet ");
        ToLog(ChangeColor(planetColor, planetName));
        ToLog(" " + message);
    }

    public void UpdateLogPlanetRes(string planetName, int stone, int water, int gas)
    {
        //AddNewLine();

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

        //AddTurnToLog();
        //LogText.text += "Planet ";
        //LogText.text += ChangeColor(planetColor, planetName);
        //LogText.text += " " + message;

        ToLog(AddNewLine());
        ToLog(AddTurnToLog());
        ToLog("Planet ");
        ToLog(ChangeColor(planetColor, planetName));
        ToLog(" has collected: ");
        ToLog(ChangeColor(stoneColor, stone.ToString() + " Stone ")) ;
        ToLog(ChangeColor(waterColor, water.ToString() + " Water "));
        ToLog(ChangeColor(gasColor, gas.ToString() + " Gas "));
        //ToLog(" " + message);
    }

    public void UpdateLogTrade(string planetName1, string planetName2, string message)
    {

    }

    public void UpdateLogTech(string planetName, string tech, string effect)
    {
        //AddNewLine();

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

        color = "#2fbf18";

        //AddTurnToLog();
        //LogText.text += "Planet ";
        //LogText.text += ChangeColor(planetColor, planetName);
        //LogText.text += " has learned ";
        //LogText.text += ChangeColor(color, tech) + "!";
        //LogText.text += " " + "<b>" + effect + "</b>";

        // backlog text first and then log
        ToLog(AddNewLine());
        ToLog(AddTurnToLog());
        ToLog("Planet ");
        ToLog(ChangeColor(planetColor, planetName));
        ToLog(" has learned ");
        ToLog(ChangeColor(color, tech) + "!");
        ToLog(" " + "<b>" + effect + "</b>");
    }

    public void UpdateLogMission(string missionName, string missionReward)
    {
        //AddNewLine();

        color = "#f2dd54";
        //AddTurnToLog();
        //LogText.text += "Mission ";
        //LogText.text += ChangeColor(color, missionName);
        //LogText.text += " completed!";

        //if (missionReward != "")
        //{
        //    LogText.text += "<b>" + " Reward: " + missionReward + "</b>";
        //}

        // backlog text first and then log

        ToLog(AddNewLine());
        ToLog(AddTurnToLog());
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

    //private void AddTurnToLog()
    //{
    //    LogText.text += "[Turn " + gc.turn + "] ";
    //}

    private string AddTurnToLog()
    {
        return "[Turn " + gc.turn + "] ";
    }

    //private void AddNewLine()
    //{
    //    if (LogText.text != "")
    //    {
    //        LogText.text += "\r\n";
    //    }
    //}

    private string AddNewLine()
    {
        if (LogText.text != "")
        {
            return "\r\n";
        } else
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
        myScrollRect.verticalNormalizedPosition = 0f; // scroll to bottom
        LogText.text += textToLog;
        textToLog = ""; // reset textToLog
    }

    // mission
    // trading
    // technology
    // planet
    // highlight planets, technology
    // log turn
}