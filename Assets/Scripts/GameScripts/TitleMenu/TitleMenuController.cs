using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class TitleMenuController : MonoBehaviour
{
    [SerializeField] private GameObject panelLoading;
    [SerializeField] private AudioSource theme_song;


    void Update()
    {
        if (Input.anyKey) {
            panelLoading.GetComponent<Animator>().enabled = true;
            StartCoroutine(Countdown());
        }
    }

    void eventoFinalAnimation()
    {
        SceneManager.LoadScene(1);
    }

    IEnumerator Countdown()
    {
        float timeElapsed = 0;
        float objetiveTime = 1.5f;    // Time to turn down the volume
        float initialVolume = this.theme_song.volume;

        while (timeElapsed < objetiveTime)
        {
            this.theme_song.volume = Mathf.Lerp(initialVolume, 0f, timeElapsed/objetiveTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        this.theme_song.Stop();
        this.theme_song.volume = initialVolume;
    }

}
