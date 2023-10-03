using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Npc_Dialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI villagerText;
    [SerializeField] private TextMeshProUGUI villagerName;
    [SerializeField] private NPCConfig_ScriptableObject OnNPCInteract;
    public float speedText;
    private int visibleChar = 0;
    [SerializeField] private float DEFAULT_TEXT_SPEED = 0.05f;
    [SerializeField] private float FAST_TEXT_SPEED = 0.1f;
    private PlayerInput input;

    private bool isActive;

    private void Start()
    {
        input = FindAnyObjectByType<PlayerInput>();
        foreach(TalkableObject obj in FindObjectsByType<TalkableObject>(FindObjectsSortMode.None))
            obj.TalkEvent.AddListener(StartDialogueBox);
        isActive = false;
    }
    private void StartDialogueBox(NPCConfig_ScriptableObject config)
    {
        this.OnNPCInteract = config;
        foreach (Transform child in transform) //Activate text and backgrounds
            child.gameObject.SetActive(true);

        villagerText.maxVisibleCharacters = visibleChar;
        speedText = DEFAULT_TEXT_SPEED;
        isActive = true;
        StartCoroutine(slowText());

    }

    public void CloseDialog()
    {
        if (!isActive)
            return;
        isActive = !isActive;
        input.SwitchCurrentActionMap(Utils.FREEMOVE_INPUTMAP);
        foreach (Transform child in transform) //Deactivate text and backgrounds
            child.gameObject.SetActive(false);
    }
    
    IEnumerator slowText()
    {
        int visibleChar = 0;
        int fullTextSize = OnNPCInteract.dialogue.Length;
        while (visibleChar < fullTextSize)
        {
            if(Input.anyKeyDown)
                speedText = FAST_TEXT_SPEED;
            villagerText.maxVisibleCharacters = visibleChar;
            yield return new WaitForSeconds(speedText);
            visibleChar++;
        }
    }

}
