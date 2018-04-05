public static class Constants
{
    public const int LEARNERS_MISSION_1 = 0;
    public const int LEARNERS_MISSION_2 = 1;
    public const int LEARNERS_MISSION_3 = 2;
    public const int LEARNERS_MISSION_4 = 3;
    public const int LEARNERS_MISSION_5 = 4;
    public const int LEARNERS_MISSION_5_PLAY = 100; // Extra hint pointing ot play button we may remove in the future
    public const int LEARNERS_MISSION_6 = 5;
    public const int LEARNERS_MISSION_6_PLAY = 101; // Extra hint pointing ot play button we may remove in the future
    public const int LEARNERS_MISSION_7 = 6;
    public const int LEARNERS_MISSION_7_PLAY = 102;

	// Constants for hint indicator
	public const int TURN_1_SKILL_TREE = 0;
	public const int TURN_1_PLANET_SLOT = 0;
	public const int TURN_1_PLACE_PLANET = 0;
	public const int TURN_1_END_TURN = 0;
	public const int TURN_1_WATCH_SIMULATION = 0;

	public const int TURN_2_SKILL_TREE = 0;
	public const int TURN_2_PLANET_SLOT = 0;
	public const int TURN_2_PLACE_PLANET = 0;
	public const int TURN_2_END_TURN = 0;
	public const int TURN_2_WATCH_SIMULATION = 0;

	public const int TURN_3_TECH_TREE = 0;
	public const int TURN_3_TECH_SLOT = 0;
	public const int TURN_3_TECH_TREE_2 = 0;
	public const int TURN_3_TECH_SLOT_2 = 0;

    // Constants for attributes
    public const int DEFENSE = 0;
    public const int ATTACK = 1;

    // log types
    public const int PLANET = 41;
    public const int TRADE = 42;
    public const int TECHNOLOGY = 43;
    public const int MISSION = 44;

    // Constants for starting values for planets
    public const int CARBON_ADD_CARBON = 4;
    public const int CARBON_ADD_NITROGEN = 1;
    public const int CARBON_ADD_HYDROGEN = 1;
    public const int CARBON_TURNS_TO_BUILD = 1;
    public const int CARBON_DEFENSE_POWER = 3;
    public const int CARBON_ATTACK_POWER = 3;
    public const int CARBON_TURNS_TO_DIE = 2;

    public const float ANIMATE_SPEED_TEST = 1f; // use this constant for testing
    public const int ANIMATE_SPEED_PLAY = 1; // use this constant for for final 

    // Constants for missions
    public const int MISSION_1 = 31;

    public const string LEVEL_1_SCENE_NAME = "Main";
    public const string LEVEL_2_SCENE_NAME = "Level2";
    public const string LEVEL_3_SCENE_NAME = "Final";

    // Strings for tooltips
    // https://docs.unity3d.com/Manual/StyledText.html
    //
    // ;                        for new line 
    // <b></b>                  for bold
    // <color=yellow></color>   for color
    public const string TOOLTIP_RIGHT_STONE_BUTTON = "Build a <b><color=#616161FF>Stone Planet</color></b>.;;- Generates <b><color=#616161FF>4 stone</color></b>, <b><color=#2196F3FF>1 water</color></b>, <b><color=#795548FF>1 gas</color></b> per turn.;- Takes 1 turn to build.";
    public const string TOOLTIP_RIGHT_WATER_BUTTON = "Build a <b><color=#2196F3FF>Water Planet</color></b>.;;- Generates <b><color=#616161FF>2 stone</color></b>, <b><color=#2196F3FF>2 water</color></b>, <b><color=#795548FF>6 gas</color></b> per turn.;- Takes 3 turns to build.";
    public const string TOOLTIP_RIGHT_GAS_BUTTON = "Build a <b><color=#795548FF>Gas Planet</color></b>.;;- Generates <b><color=#616161FF>24 stone</color></b>, <b><color=#2196F3FF>8 water</color></b>, <b><color=#795548FF>8 gas</color></b> per turn.;- Takes 3 turns to build.";
    public const string TOOLTIP_RIGHT_NEXT_TURN_BUTTON = "Play End Turn Simulation.";
    public const string TOOLTIP_RIGHT_MISSION_1 = "Details about Mission 1;Edit me in Constants.cs";
    public const string TOOLTIP_RIGHT_MISSION_2 = "Details about Mission 2;Edit me in Constants.cs";
    public const string TOOLTIP_RIGHT_MISSION_3 = "Details about Mission 3;Edit me in Constants.cs";
    public const string TOOLTIP_LEFT_BUILDING_BUTTON = "Press Play Button to end turn.";
    public const string TOOLTIP_LEFT_TECHNOLOGY_1_BUTTON = "Learn <b>High Energy Magnetics</b>.;;- Resources Protected from Solar Storms.;- Costs <b><color=#616161FF>10 stone</color></b>.";
    public const string TOOLTIP_LEFT_TECHNOLOGY_2_BUTTON = "Learn <b>Interplanetary Networking</b>.;;- Link with another planet, allowing them to automatically trade resources.;- Costs <b><color=#616161FF>5 stone</color></b>, <b><color=#2196F3FF>5 water</color></b>, <b><color=#795548FF>5 gas</color></b>.";
    public const string TOOLTIP_LEFT_TECHNOLOGY_3_BUTTON = "Learn <b>Advanced Economics</b>.;;- Increases Chance to Link by 50%.;- Costs <b><color=#2196F3FF>10 water</color></b>, <b><color=#795548FF>15 gas</color></b>.";
    public const string TOOLTIP_LEFT_TECHNOLOGY_4_BUTTON = "Learn <b>Extraplanetary Ballistics</b>.;;- Can Attack Rogue Planets.;- Costs <b><color=#616161FF>15 stone</color></b>, <b><color=#2196F3FF>15 water</color></b>.";
    public const string TOOLTIP_LEFT_TECHNOLOGY_5_BUTTON = "Learn <b>Negative Mass Mechanics</b>.;;- Increases Attack Power by 50%.;- Costs <b><color=#616161FF>20 stone</color></b>, <b><color=#2196F3FF>20 water</color></b>, <b><color=#795548FF>20 gas</color></b>.";
    public const string TOOLTIP_LEFT_LINK_TO_BUTTON = "Select a target to link to.;;- Target must also know <b>Interplanetary Networking</b>.";
}
