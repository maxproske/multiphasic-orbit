using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Germanium : Planet {

    public Germanium() {
        tier = 3;
        addCarbon = 0;
        addNitrogen = 15;
        addHydrogen = 0;
        turnsToBuild = 5;
        defensePower = 10;
        attackPower = 10;
        turnsToDie = 4;
    }
}
