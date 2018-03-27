using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rogue : Planet
{

    private Planet dominatedPlanetScript;


    public Rogue()
    {
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
        //dominatedPlanets = new List<Planet>();
    }

    // function used to steal a fraction of dominatedPlanet's resources
    // parameters are how many resources stolen per turn
    public void Steal(int sCarbon, int sNitrogen, int sHydrogen)
	{
		if (die == true){
			foreach (var dominatedPlanet in linkedWith) {
				dominatedPlanetScript = dominatedPlanet.GetComponent<Planet> ();
				// take away and add to this rogue planet's resources only if dominatedPlanet has more than 0 of that resource
				if (dominatedPlanetScript.carbon > 0) {
					dominatedPlanetScript.carbon -= sCarbon;
					carbon += sCarbon;
				}
				if (dominatedPlanetScript.nitrogen > 0) {
					dominatedPlanetScript.nitrogen -= sNitrogen;
					nitrogen += sNitrogen;
				}
				if (dominatedPlanetScript.hydrogen > 0) {
					dominatedPlanetScript.hydrogen -= sHydrogen;
					hydrogen += sHydrogen;
				}
			}
		}
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
