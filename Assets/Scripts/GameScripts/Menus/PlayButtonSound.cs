using UnityEngine;
using UnityEngine.EventSystems;

public class PlayButtonSound : MonoBehaviour, ISelectHandler, ISubmitHandler
{
    public AudioClip focus_clip;
    public AudioClip select_clip;
    public AudioSource audiosource; 

    public void OnSelect(BaseEventData eventData)
    {
        this.audiosource.PlayOneShot(focus_clip);
    }

    public void OnSubmit(BaseEventData eventData)
    {
        this.audiosource.PlayOneShot(select_clip);
    }
}
