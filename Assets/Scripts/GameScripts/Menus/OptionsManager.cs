using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private SelectionOptionMenuDialog first;
    [SerializeField] private SelectionOptionMenuDialog second;

    public void Accept(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (!gameObject.activeSelf)
            return;

        first.OnSubmit(null);
        second.OnSubmit(null);
    }
}
