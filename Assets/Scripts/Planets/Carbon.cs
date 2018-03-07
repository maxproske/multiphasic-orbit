using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carbon : Planet {
    public Carbon()
    {
        tier = 1;
        addCarbon = 4;
        addNitrogen = 1;
        addHydrogen = 1;
        turnsToBuild = 1;
        defensePower = 3;
        attackPower = 3;
        turnsToDie = 2;
    }
}
