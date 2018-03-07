using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silicon : Planet {

    public Silicon() {
        tier = 2;
        addCarbon = 6;
        addNitrogen = 2;
        addHydrogen = 2;
        turnsToBuild = 3;
        defensePower = 5;
        attackPower = 5;
        turnsToDie = 3;
    }
}
