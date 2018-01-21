using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// New blueprint for planets that can be created in Unity editor
[CreateAssetMenu(fileName = "New Planet", menuName = "Macro Skill Tree/Planet")]
public class Planet : ScriptableObject {

	// "name" is already take. Override using "new"
	new public string name = "New Planet";

	public Sprite icon = null;
}
