using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acetylene : Planet {

    public Acetylene() {
        tier = 3;
        addCarbon = 6;
        addNitrogen = 6;
        addHydrogen = 6;
        turnsToBuild = 5;
        defensePower = 10;
        attackPower = 10;
        turnsToDie = 4;
    }
}
