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
    private PlayerInputController playerInput;

    public static ControllerType activeController;
    

    public enum ControllerType { KEYBOARD, XBOX}

    private void Start()
    {
        playerInput = FindAnyObjectByType<PlayerInputController>();
        textDisplay.text = texto;
        activeController = ControllerType.KEYBOARD;
    }
    private void OnEnable()
    {
        playerInput = FindAnyObjectByType<PlayerInputController>();
        if(playerInput.activeController == ControllerType.KEYBOARD) { this.buttonDisplay.sprite = keyBoard_Icon; }
        else { this.buttonDisplay.sprite = xboxController_Icon; }
    }
    private void Update()
    {
        switch (this.playerInput.activeController)
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
