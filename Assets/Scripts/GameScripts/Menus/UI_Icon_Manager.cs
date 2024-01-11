using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_Icon_Manager : MonoBehaviour
{
    [SerializeField] private string texto;
    [SerializeField] private Sprite keyBoard_Icon;
    [SerializeField] private Sprite xboxController_Icon;
    [SerializeField] private Image buttonDisplay;
    [SerializeField] private TextMeshProUGUI textDisplay;
    public static ControllerType activeController;
    

    public  enum ControllerType { KEYBOARD, XBOX}

    private void Start()
    {
        textDisplay.text = texto;
        activeController = ControllerType.KEYBOARD;
    }

    private void Update()
    {
        switch (activeController)
        {
            case ControllerType.KEYBOARD:
                this.buttonDisplay.sprite = keyBoard_Icon;
                break;
            case ControllerType.XBOX:
                this.buttonDisplay.sprite = xboxController_Icon;
                break;
        }
    }
}
