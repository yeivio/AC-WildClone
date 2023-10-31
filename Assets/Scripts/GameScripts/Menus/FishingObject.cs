using UnityEngine;
using UnityEngine.InputSystem;

public class FishingObject : MonoBehaviour
{
    [SerializeField] private FishingUI_Manager FishingUI;
    private PlayerInput playerInput;

    private void Start()
    {
        playerInput = FindAnyObjectByType<PlayerInput>();

    }

    /// <summary>
    /// When a player interacts on this object, the object will enable the fishing UI
    /// </summary>
    public void interaction()
    {
        this.FishingUI.gameObject.SetActive(true);
    }

    /// <summary>
    /// When a player wants to quit from fishing, the object will disable the UI gameobject and then switch
    /// the inputmap so the player can move again
    /// </summary>
    public void stopInteraction(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        this.FishingUI.gameObject.SetActive(false);
        playerInput.SwitchCurrentActionMap(Utils.FREEMOVE_INPUTMAP);
    }
}
