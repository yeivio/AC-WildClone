using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] private Button firstSelectedButton;

    private void OnEnable()
    {
        this.firstSelectedButton.Select();
    }
}
