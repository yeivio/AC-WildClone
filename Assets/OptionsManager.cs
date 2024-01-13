using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionsManager : MonoBehaviour
{
    public void Accept(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        // Code
    }
}
