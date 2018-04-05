using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is a data class. Remove MonoBehaviour inheritance.
// Serialize. When we have an ellipse in our inspector, we'll be able to populate the
// public field, instead of doing it only in code
[System.Serializable]
public class EllipseTester {

	public float xAxis;
	public float yAxis;
    public float zAxis;

	// If we are populating the axes in code, we should create a constructor
	public EllipseTester (float xAxis, float yAxis, float zAxis) {
		this.xAxis = xAxis;
		this.yAxis = yAxis;
        this.zAxis = zAxis;
	}

	// Put in the value between 0 and 1, and get the proper vector
	// position for that point in the ellipse
	// Use a Vector2 so we don't need to rework a Vector3 on the fly
	public Vector3 Evaluate(float t) 
    {
        float x, y, z;
        float angle = Mathf.Deg2Rad * 360f * t;

        if (GameController.ORBIT_4D) 
        {
            // We dont need to divide segments by i, because we already know
            // how far we are along the elipse (float t).
            // Cast both to floats, to make it a number between 0-1
            // Multiply by 360 to get its equivalent in degrees
            // Use Mathf.Deg2Rad, because Sin and Cosine work in radians, not degrees
            x = Mathf.Sin (angle) * xAxis;
            y = Mathf.Cos (angle) * yAxis;
            float flatten = 10f;
            z = Mathf.Tan (angle) * (xAxis+yAxis)/flatten; // 0 for flat orbit

            if (z < -500) z = -99999;
            if (z > 500) z = 99999;
        }
        else
        {
            x = Mathf.Sin (angle) * xAxis;
            y = Mathf.Cos (angle) * yAxis;
            z = Mathf.Sin (angle) * -(xAxis+yAxis/2)/5;
        }

		// Return Vector2
		return new Vector3 (x, y, z);
	}

}
