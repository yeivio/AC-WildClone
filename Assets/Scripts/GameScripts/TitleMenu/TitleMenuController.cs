using UnityEngine;

public class TitleMenuController : MonoBehaviour
{
    [SerializeField] private Animator loadingAnimation;
    public Animation anim;
    
    

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
            loadingAnimation.Play(anim.ToString());
    }
}
