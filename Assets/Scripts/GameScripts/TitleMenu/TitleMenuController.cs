using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleMenuController : MonoBehaviour
{
    [SerializeField] private GameObject panelLoading;

    void Update()
    {
        if (Input.anyKey) {
            panelLoading.GetComponent<Animator>().enabled = true;

        }
    }

    void eventoFinalAnimation()
    {
        SceneManager.LoadScene(1);
    }
}
