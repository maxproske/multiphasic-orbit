using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammonia : Planet {

    public Ammonia() {
        tier = 2;
        addCarbon = 2;
        addNitrogen = 6;
        addHydrogen = 2;
        turnsToBuild = 3;
        defensePower = 5;
        attackPower = 5;
        turnsToDie = 3;
    }
}
