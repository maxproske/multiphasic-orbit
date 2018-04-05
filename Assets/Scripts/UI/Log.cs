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
    string planetColor2;
    string stoneColor;
    string waterColor;
    string gasColor;
    string rogueColor;
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
        rogueColor = "FF00001A";
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


    public void UpdateLogPopulation(string planetName)
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
        ToLog("The population of Planet ");
        ToLog(ChangeColor(planetColor, planetName));
        ToLog(" has increased 1 billion ");

    }

    public void UpdateLogTrade(string planetName1, string planetName2, int stone, int water, int gas)
    {
        if (planetName1.Contains("Stone"))
        {
            planetColor = stoneColor;
        }

        if (planetName1.Contains("Water"))
        {
            planetColor = waterColor;
        }

        if (planetName1.Contains("Gas"))
        {
            planetColor = gasColor;
        }
        if (planetName2.Contains("Stone"))
        {
            planetColor2 = stoneColor;
        }

        if (planetName2.Contains("Water"))
        {
            planetColor2 = waterColor;
        }

        if (planetName2.Contains("Gas"))
        {
            planetColor2 = gasColor;
        }

        // backlog text first and then log
        ToLog(AddNewLine());
        ToLog("Planet ");
        ToLog(ChangeColor(planetColor, planetName1));
        ToLog(" used ");
        if (stone < 0)
        {
            stone *= -1;
            ToLog(ChangeColor(stoneColor, stone.ToString() + " Stone "));
            stone *= -1;
        }
        if (water < 0)
        {
            water *= -1;
            ToLog(ChangeColor(waterColor, water.ToString() + " Water "));
            water *= -1;
        }
        if (gas < 0)
        {
            gas *= -1;
            ToLog(ChangeColor(gasColor, gas.ToString() + " Gas "));
            gas *= -1;
        }

        ToLog(" to trade ");
        if (stone > 0)
        {
            ToLog(ChangeColor(stoneColor, stone.ToString() + " Stone "));
        }
        if (water > 0)
        {
            ToLog(ChangeColor(waterColor, water.ToString() + " Water "));
        }
        if (gas > 0)
        {
            ToLog(ChangeColor(gasColor, gas.ToString() + " Gas "));
        }
        ToLog(" from planet ");
        ToLog(ChangeColor(planetColor2, planetName2));
    }


    public void UpdateLogAttack(string planetName1, string planetName2)
    {
        if (planetName1.Contains("Stone"))
        {
            planetColor = stoneColor;
        }

        if (planetName1.Contains("Water"))
        {
            planetColor = waterColor;
        }

        if (planetName1.Contains("Gas"))
        {
            planetColor = gasColor;
        }

        planetColor2 = rogueColor;

        // backlog text first and then log
        ToLog(AddNewLine());
        ToLog("Planet ");
        ToLog(ChangeColor(planetColor, planetName1));
        ToLog(" attacked planet ");
        ToLog(ChangeColor(rogueColor, planetName2));
        ToLog(" and decrease its hp by 50. ");

    }

    public void UpdateLogLinkSuccessful(string planetName1, string planetName2)
    {
        if (planetName1.Contains("Stone"))
        {
            planetColor = stoneColor;
        }

        if (planetName1.Contains("Water"))
        {
            planetColor = waterColor;
        }

        if (planetName1.Contains("Gas"))
        {
            planetColor = gasColor;
        }
        if (planetName2.Contains("Stone"))
        {
            planetColor2 = stoneColor;
        }

        if (planetName2.Contains("Water"))
        {
            planetColor2 = waterColor;
        }

        if (planetName2.Contains("Gas"))
        {
            planetColor2 = gasColor;
        }



        // backlog text first and then log
        ToLog(AddNewLine());
        ToLog("Planet ");
        ToLog(ChangeColor(planetColor, planetName1));
        ToLog(" successfully linked with planet ");
        ToLog(ChangeColor(planetColor2, planetName2));

    }

    public void UpdateLogLinkFail(string planetName1, string planetName2)
    {
        if (planetName1.Contains("Stone"))
        {
            planetColor = stoneColor;
        }

        if (planetName1.Contains("Water"))
        {
            planetColor = waterColor;
        }

        if (planetName1.Contains("Gas"))
        {
            planetColor = gasColor;
        }
        if (planetName2.Contains("Stone"))
        {
            planetColor2 = stoneColor;
        }

        if (planetName2.Contains("Water"))
        {
            planetColor2 = waterColor;
        }

        if (planetName2.Contains("Gas"))
        {
            planetColor2 = gasColor;
        }



        // backlog text first and then log
        ToLog(AddNewLine());
        ToLog("The linking between Planet ");
        ToLog(ChangeColor(planetColor, planetName1));
        ToLog(" and planet ");
        ToLog(ChangeColor(planetColor2, planetName2));
        ToLog(" failed, planet ");
        ToLog(ChangeColor(planetColor2, planetName2));
        ToLog(" became a ");
        ToLog(ChangeColor(rogueColor, "rogue planet"));
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
        Debug.Log("gc.GAME_STATE " + gc.GAME_STATE + ", " + "gc.turn " + gc.turn);
        if (GameController.level == 1 && gc.GAME_STATE == Constants.LEARNERS_MISSION_3 && gc.turn == 2) gc.GAME_STATE = Constants.LEARNERS_MISSION_4;
        //if (GameController.level == 1 && gc.GAME_STATE == Constants.LEARNERS_MISSION_3 && gc.turn == 3) gc.GAME_STATE = Constants.LEARNERS_MISSION_5;
        if (GameController.level == 1 && gc.GAME_STATE == Constants.LEARNERS_MISSION_5_PLAY && gc.turn == 4 && gc.canBuild) gc.GAME_STATE = Constants.LEARNERS_MISSION_6;
        if (GameController.level == 1 && gc.GAME_STATE == Constants.LEARNERS_MISSION_6_PLAY) gc.GAME_STATE = Constants.LEARNERS_MISSION_7;
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

//
