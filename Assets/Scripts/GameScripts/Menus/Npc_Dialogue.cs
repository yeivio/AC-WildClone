using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Npc_Dialogue : MonoBehaviour
{
    [Header("Mandatory objects references")]
    [SerializeField] private TextMeshProUGUI villagerText;  // Text box for dialogue text
    [SerializeField] private TextMeshProUGUI villagerName;  // Text box for villager name text
    [SerializeField] private NPCConfig_ScriptableObject OnNPCInteract;  // Villager dialogue data
    [SerializeField] private AudioClip npcTalk_clip;  // Villager talking audio


    [Header("Dialogue config")]
    [SerializeField] private float speedText = 0.05f;   // Speed of the appearing text

    private PlayerInput input;
    private TalkableObject talkObj;
    private bool isActive;  // Tracks if the dialog window is active
    private bool isCoroutineActive;
    private int visibleChar = 0;    // Current visible chars
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        input = FindAnyObjectByType<PlayerInput>();
        isActive = false;
    }

    private void Update()
    {
        if (isCoroutineActive && Input.anyKeyDown) // Speedup the dialog appearing text
            this.speedText = this.speedText / 5;
        
    }

    /// <summary>
    /// Given an SO of an NPC loads the text into the separated dialogBoxes and starts the coroutine for the appearing text animation.
    /// </summary>
    /// <param name="config"></param>
    public void StartDialogueBox(NPCConfig_ScriptableObject config, TalkableObject talkObj)
    {
        this.OnNPCInteract = config;
        this.talkObj = talkObj;
        foreach (Transform child in transform) //Activate text and backgrounds
            child.gameObject.SetActive(true);

        villagerText.text = config.dialogue;
        villagerName.text = config.name;
        villagerText.maxVisibleCharacters = visibleChar;
        isActive = true;

        input.SwitchCurrentActionMap(Utils.NPC_TALK_INPUTMAP);
        this.talkObj.EnableCamera(); // Enable the npc camera
        StartCoroutine(slowText());


    }

    /// <summary>
    /// If the dialogbox is active, disable all the dialog UI, disable the dialogue camera and change the input map to FREEMOVE
    /// </summary>
    public void CloseDialog(InputAction.CallbackContext context)
    {
        if (!isActive)
            return;
        isActive = !isActive;
        input.SwitchCurrentActionMap(Utils.FREEMOVE_INPUTMAP);
        foreach (Transform child in transform) //Deactivate text and backgrounds
            child.gameObject.SetActive(false);
        if (isCoroutineActive)  // If the animation is playing, then it's forced to stop
            StopAllCoroutines();
        this.talkObj.DisableCamera(); // Disable the npc camera
    }
    
    /// <summary>
    /// Animation for the npc dialogue text. The Coroutine spawns the string char by char at a speedtext speed.
    /// </summary>
    /// <returns></returns>
    IEnumerator slowText()
    {
        audioSource.PlayOneShot(npcTalk_clip);
        this.isCoroutineActive = true;
        int visibleChar = 0;
        int fullTextSize = OnNPCInteract.dialogue.Length;
        float audioDuration = this.npcTalk_clip.length;
        while (visibleChar < fullTextSize)
        {
            villagerText.maxVisibleCharacters = visibleChar;
            yield return new WaitForSeconds(audioDuration / fullTextSize);
            visibleChar++;
        }
        this.isCoroutineActive = false;
    }

}