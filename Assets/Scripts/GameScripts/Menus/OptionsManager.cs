using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;

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
        first.GetComponent<SelectionOptionMenuDialog>().permitirInput();
        second.GetComponent<SelectionOptionMenuDialog>().permitirInput();
        first.Submit();
        second.Submit();
    }
}
