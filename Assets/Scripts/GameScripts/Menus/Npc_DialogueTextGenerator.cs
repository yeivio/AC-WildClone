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
    private PlayerInput input;

    public UnityEvent OnClose;

    private bool isActive;

    private bool isCoroutineActive;

    private void Start()
    {
        input = FindAnyObjectByType<PlayerInput>();
        foreach(TalkableObject obj in FindObjectsByType<TalkableObject>(FindObjectsSortMode.None))
            obj.OnTalk.AddListener(StartDialogueBox);
        isActive = false;
    }

    private void Update()
    {
        if(isCoroutineActive && Input.anyKeyDown)
            this.speedText = this.speedText / 5;
    }
    private void StartDialogueBox(NPCConfig_ScriptableObject config)
    {
        this.OnNPCInteract = config;
        foreach (Transform child in transform) //Activate text and backgrounds
            child.gameObject.SetActive(true);

        villagerText.text = config.dialogue;
        villagerName.text = config.name;
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
        if (isCoroutineActive)
            StopAllCoroutines();
        this.OnClose?.Invoke();
    }
    
    IEnumerator slowText()
    {
        this.isCoroutineActive = true;
        int visibleChar = 0;
        int fullTextSize = OnNPCInteract.dialogue.Length;
        while (visibleChar < fullTextSize)
        {
            villagerText.maxVisibleCharacters = visibleChar;
            yield return new WaitForSeconds(speedText);
            visibleChar++;
        }
        this.isCoroutineActive = false;
    }

}
