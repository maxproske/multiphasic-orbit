using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class CameraController : MonoBehaviour {

	public Transform target;
    private Camera camera;
	public float distance = 0.0f;
	public float xSpeed = 120.0f;
	public float ySpeed = 120.0f;

	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;

	public float distanceMin = .5f;
	public float distanceMax = 15f;

	public float sizeMin = 40f;
	public float sizeMax = 600f;
	private int scrollDistance = 70;

	float x = 0.0f;
	float y = 0.0f;

    private UIController ui;

	// Use this for initialization
	void Start () 
	{
        camera = Camera.main;
        ui = GameObject.Find ("Canvas").GetComponent<UIController> ();

		Vector3 angles = transform.eulerAngles;
		x = angles.y;
		y = angles.x;

        // Initialize distance UI
        ui.SetDistance((int)camera.orthographicSize);
	}

	void LateUpdate () 
	{
		if (target && Input.GetMouseButton(1)) 
		{
			target.position = new Vector3 (0, 0, 0);

			x += Input.GetAxis("Mouse X") * xSpeed * distance * 0.02f;
			y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

			y = ClampAngle(y, yMinLimit, yMaxLimit);

			Quaternion rotation = Quaternion.Euler(y, x, 0);

			// Disable scroll wheel
			//distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel")*5, distanceMin, distanceMax);
			distance = 5f;

			RaycastHit hit;
			if (Physics.Linecast (target.position, transform.position, out hit)) 
			{
				distance -=  hit.distance;
			}
			Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
			Vector3 position = rotation * negDistance + (target.position);

			transform.rotation = rotation;
			// Multiply by 100 to prevent orbit path from clipping
			transform.position = position * 100;
		}
		// Scroll in and out with the mouse scroll wheel
		var scroll = Input.GetAxis ("Mouse ScrollWheel");
		if (scroll != 0) {
			// Change the orthographic camera size
			Camera.main.orthographicSize -= (scroll > 0) ? scrollDistance : -scrollDistance;

            // Clamp values if camera is too close or too far away
			if (Camera.main.orthographicSize < sizeMin) {
				Camera.main.orthographicSize = sizeMin;
			} else if (Camera.main.orthographicSize > sizeMax) {
				Camera.main.orthographicSize = sizeMax;
			}
           // Update distance UI last
            ui.SetDistance((int)camera.orthographicSize);
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		// Prevent camera view from crossing y=0
		if (angle < -20) {
			angle = -20f;
		}

		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp(angle, min, max);
	}
}