using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue : Planet {

    private Planet dominatedPlanetScript;

    public List<GameObject> dominatedPlanets;

    public Rogue() {
        addCarbon = 0;
        addNitrogen = 0;
        addHydrogen = 0;
        turnsToBuild = 0;
        defensePower = 0;
        attackPower = 0;
        turnsToDie = 0;
    }

    public override void Start()
    {
        base.Start();
        dominatedPlanets = new List<GameObject>();
    }

    // stores the planet that attempted to link with rogue planet
    public GameObject dominatedPlanet;

    // function used to steal a fraction of dominatedPlanet's resources
    // parameters are how many resources stolen per turn
    public void Steal(int carbon, int nitrogen, int hydrogen)
    {
        dominatedPlanetScript = dominatedPlanet.GetComponent<Planet>();
        dominatedPlanetScript.carbon -= carbon;
        dominatedPlanetScript.nitrogen -= nitrogen;
        dominatedPlanetScript.hydrogen -= hydrogen;

    }

    // function used to attack another planet anywhere that has been built
    public void Attack()
    {
        dominatedPlanetScript.turnsToDie--;
    }

    // function used to increase attribute value
    public void IncreaseAttribute(int attributeConst, int amount)
    {
        switch (attributeConst)
        {
            case Constants.DEFENSE:
                defensePower += amount;
                break;
            case Constants.ATTACK:
                attackPower += amount;
                break;
            default:
                break;
        }
    }
}
