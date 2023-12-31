using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TitleMenuController : MonoBehaviour
{
    [SerializeField] private GameObject panelLoading;
    [SerializeField] private AudioSource theme_song;

    [SerializeField] private AudioSource boatSound;
    [SerializeField] private AudioSource boatWater;


    public bool isOntitle; // This same script is used on the title menu and the start of the game. this will distinguish between the versions

    private void Start()
    {
        if (!isOntitle)
        {
            this.gameObject.GetComponent<Animator>().enabled = true;
            this.gameObject.GetComponent<Animator>().Play("FadeOut");
        }
    }

    void Update()
    {
        if (Input.anyKey && isOntitle) {
            panelLoading.GetComponent<Animator>().enabled = true;
            StartCoroutine(Countdown());
            boatSound.Play();
            boatWater.Play();
        }
    }

    public void eventoFinalAnimation()
    {
        SceneManager.LoadScene(1);
    }

    public void stopFinalAnimation()
    {
        this.gameObject.SetActive(false);
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
