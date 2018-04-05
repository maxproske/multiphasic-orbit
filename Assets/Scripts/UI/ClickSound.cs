using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ClickSound : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private AudioClip clickAudioClip;
    private AudioClip hoverAudioClip;
    private Button button { get { return GetComponent<Button>(); } }
    private AudioSource audioSource { get { return GetComponent<AudioSource>(); } }

    // Use this for initialization
    void Start () 
    {
        // Add audio source
        gameObject.AddComponent<AudioSource>();

        // Load resources
        hoverAudioClip = (AudioClip)Resources.Load<AudioClip>("Audio/menu-fx-02");
        
        clickAudioClip = (AudioClip)Resources.Load<AudioClip>("Audio/menu-fx-03");
        button.onClick.AddListener(() => PlaySoud());

        audioSource.volume = 0.4f;
        audioSource.playOnAwake = false;
    }

	public void OnPointerEnter (PointerEventData eventData)
	{
        if (button.IsInteractable())
        {
            audioSource.PlayOneShot (hoverAudioClip);
        }
	}
	public void OnPointerExit (PointerEventData eventData)
	{

	}

    void PlaySoud ()
    {
        audioSource.PlayOneShot (clickAudioClip);
    }
}