using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Methane : Planet {

    public Methane() {
        addCarbon = 2;
        addNitrogen = 2;
        addHydrogen = 6;
        turnsToBuild = 3;
        defensePower = 5;
        attackPower = 5;
        turnsToDie = 3;
    }
}
