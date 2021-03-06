﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sandbox : MonoBehaviour
{

    // planet prefabs
    public GameObject stone;
    public GameObject water;
    public GameObject gas;

    // sun transform
    public Transform sun;

    // starting amount of each planet
    public int stoneAmount;
    public int waterAmount;
    public int gasAmount;

    // starting amount of rogue planets
    public int rogueAmount;

    // starting amount of each resources
    public int resourceAmount;

    private GameController gc;
    private Planet p;
    private GameObject go;

    private int stoneIncrement;
    private int waterIncrement;
    private int gasIncrement;

    // Orbit Path
    // X axis - random(150 - 350)
    // Y axis - random(150 - 350)
    // Orbit progress - random(range: 0-1)
    // Placing coroutine - false

    // Use this for initialization
    void Start()
    {
        gc = GameObject.Find("Game Manager").GetComponent<GameController>();

        gc.planets = new List<GameObject>();
        gc.roguePlanets = new List<GameObject>();

        RandomlyPlacePlanets(stone, stoneAmount, resourceAmount);
        RandomlyPlacePlanets(water, waterAmount, resourceAmount);
        RandomlyPlacePlanets(gas, gasAmount, resourceAmount);

        gc.ui.selectedPlanet = p; //  highlight planet orbit
        gc.ui.SetSelectedPlanet(p); // populate left panel with data
        gc.ui.OpenLeftPanel(); // open the panel
        gc.ui.UpdateSelectedPlanet();

        gc.playButton.interactable = true;
    }

    void RandomlyPlacePlanets(GameObject prefab, int planetAmount, int resourceAmount)
    {
        // instantiate and randomly place each planet
        for (int i = 0; i < planetAmount; i++)
        {
            go = Instantiate(prefab, sun);

            // rename planet with increment
            if (prefab.name.Contains("Stone"))
            {
                stoneIncrement++;
                go.name = "Stone " + stoneIncrement;
            }
            else if (prefab.name.Contains("Water"))
            {
                waterIncrement++;

                go.name = "Water " + waterIncrement;
            }
            else if (prefab.name.Contains("Gas"))
            {
                gasIncrement++;
                go.name = "Gas " + gasIncrement;
            }

            // add planet to planets list
            gc.planets.Add(go);

            gc.selected = go;

            // access planet script
            p = go.GetComponent<Planet>();

            // randomly set Orbit Path x and y axes
            p.orbitPath.xAxis = Random.Range(150f, 350f);
            p.orbitPath.yAxis = Random.Range(150f, 350f);

            // randomly set orbit progress
            p.orbitProgress = Random.Range(0f, 1f);

            // set turnsToBuild to built status
            p.turnsToBuild = -1;

            // set population to show left panel at start
            p.population = 1;

            // planetPlaced to true to stop AnimateOrbit
            p.planetPlaced = true;

            // set planet starting resource amount
            p.stone = resourceAmount;
            p.water = resourceAmount;
            p.gas = resourceAmount;

            // set gc planetScript to p
            gc.planetScript = p;
        }
    }
}
