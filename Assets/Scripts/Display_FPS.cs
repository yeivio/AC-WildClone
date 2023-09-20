using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Display_FPS : MonoBehaviour
{
    public TextMeshProUGUI display;
    public float deltaTime;

    // Update is called once per frame
    void Update()
    {
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        this.display.text = Mathf.Ceil(fps).ToString(); ;
    }
}
