using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneController : MonoBehaviour {

    public Image currentImage;
    public List<Sprite> intro;
    public int currentImageNumber;

	// Use this for initialization
	void Start () {
        currentImageNumber = 0;
        currentImage.sprite = intro[currentImageNumber];
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("left"))
        {
            if (currentImageNumber != 0)
            {
                currentImage.canvasRenderer.SetAlpha(0.0f);
                currentImage.CrossFadeAlpha(1.0f, 1f, false);
                currentImageNumber--;
                currentImage.sprite = intro[currentImageNumber];
            }
        }
        if (Input.GetKeyDown("right"))
        {
            if (currentImageNumber < intro.Count - 1)
            {
                currentImage.canvasRenderer.SetAlpha(0.0f);
                currentImage.CrossFadeAlpha(1.0f, 1f, false);
                currentImageNumber++;
                currentImage.sprite = intro[currentImageNumber];
            }
        }
    }
}
