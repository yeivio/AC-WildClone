using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;



public class Npc_Dialogue : MonoBehaviour
{
    [Header("Mandatory objects references")]
    [SerializeField] public TextMeshProUGUI villagerText;  // Text box for dialogue text
    [SerializeField] private TextMeshProUGUI villagerName;  // Text box for villager name text
    [SerializeField] private GameObject tienda;             // Menu que se activa cuando se accede a la tienda en un dialogo
    [SerializeField] private BuyMenu tiendaController;
    [SerializeField] private GameObject shellTienda;        // Menu que contiene la tienda para vender
    [SerializeField] private SellMenu sellTiendaController;
    [SerializeField] private GameObject selectionDialog;    // Dialog to be used when selecting between multiple choices
    [SerializeField] private Button[] selectionOptions;     // Options of the dialog multiple choice menu
    [SerializeField] private Wallet wallet;


    private NPCConfig_ScriptableObject OnNPCInteract;  // Villager dialogue data
    private AudioClip npcTalk_clip;  // Villager talking audio
    private PlayerInput input;
    private TalkableObject talkObj; // NPC object Ref
    private bool isActive;  // Tracks if the dialog window is active
    private bool isCoroutineActive;
    private int visibleChar = 0;    // Current visible chars
    private AudioSource audioSource;

    public bool didLastTextFinish;

    private void Start()
    {
        didLastTextFinish = false;
        audioSource = this.gameObject.GetComponent<AudioSource>();
        input = FindAnyObjectByType<PlayerInput>();
        isActive = false;
    }

    /// <summary>
    /// Given an SO of an NPC loads the text into the separated dialogBoxes and starts the coroutine for the appearing text animation.
    /// </summary>
    /// <param name="config"></param>
    public void StartDialogueBox(NPCConfig_ScriptableObject config, AudioClip npcAudio ,TalkableObject talkObj)
    {
        this.OnNPCInteract = config;
        this.talkObj = talkObj;
        this.npcTalk_clip = npcAudio;

        foreach (Transform child in transform) //Activate text and backgrounds
        {
            if (!child.gameObject.CompareTag("DialogWindow"))
                child.gameObject.SetActive(true);
        }
            
        //Load config and start displaying text
        villagerText.text = config.dialogue;
        villagerName.text = config.name;
        villagerText.maxVisibleCharacters = visibleChar;
        isActive = true;
        StartCoroutine(slowText());


        input.SwitchCurrentActionMap(Utils.NPC_TALK_INPUTMAP);
        this.talkObj.EnableCamera(); // Enable the npc camera

    }
    public void ContinueDialog(NPCConfig_ScriptableObject config)
    {
        switch(config.dialogue)
        {
            case "exit":
                 CloseDialog(new());
                break;
            case "tienda":
                selectionDialog.SetActive(true);
                talkObj.tiendaAbierta = true;
                selectionOptions[0].gameObject.GetComponent<TextMeshProUGUI>().text = "comprar";
                selectionOptions[0].gameObject.SetActive(true);
                selectionOptions[0].Select();
                selectionOptions[1].gameObject.GetComponent<TextMeshProUGUI>().text = "vender";
                selectionOptions[1].gameObject.SetActive(true);
                break;
            case "SiNo":
                selectionDialog.SetActive(true);
                talkObj.tiendaAbierta = true;
                selectionOptions[0].gameObject.GetComponent<TextMeshProUGUI>().text = "si";
                selectionOptions[0].gameObject.SetActive(true);
                selectionOptions[0].Select();
                selectionOptions[1].gameObject.GetComponent<TextMeshProUGUI>().text = "no";
                selectionOptions[1].gameObject.SetActive(true);
                break;
            case "endBS":
                talkObj.tiendaAbierta = false;
                talkObj.actualTalk += 1;
                if (talkObj.isBuyShop)
                {
                    InventoryItem_ScriptableObject item = tiendaController.GetComponent<BuyMenu>().buying.item;
                    // COMPROBAR DINERO DE LA PERSONA
                    if(wallet.CanBuy(item.BuyPrice))
                    {
                        wallet.Buy(item.BuyPrice);
                        sellTiendaController.GetComponent<SellMenu>().inventory.AddItem(item);
                    }
                    

                }
                
                else
                {
                    List<InventoryItem_ScriptableObject> items = sellTiendaController.GetComponent<SellMenu>().GetSell().ConvertAll<InventoryItem_ScriptableObject>(i=>i.item);
                    foreach (InventoryItem_ScriptableObject item in items)
                    {
                        sellTiendaController.GetComponent<SellMenu>().inventory.DeleteItem(item);
                    }
                    // AÑADIR Dinero
                    int dinero = sellTiendaController.GetComponent<SellMenu>().price;
                    wallet.Sell(dinero);
                }
                
                talkObj.Continue();
                break;

            default:
                StopAllCoroutines();
                villagerText.text = config.dialogue;
                StartCoroutine(slowText());
                break;
        }
    }
    public void ManageResultChoiceDialog(string result)
    {
        selectionDialog.SetActive(false);

        switch (result)
        {
            case "comprar":
                tienda.SetActive(true);
                talkObj.isBuyShop = true;
                talkObj.isSellShop = false;
                break;
            case "vender":
                shellTienda.SetActive(true);
                talkObj.isBuyShop = false;
                talkObj.isSellShop = true;
                break;
            case "si":
                Debug.Log("SI");
                talkObj.tiendaAbierta = false;
                talkObj.Continue();
                

                break;
            case "no":
                Debug.Log("NO");
                talkObj.actualTalk += 1;
                talkObj.tiendaAbierta = false;
                talkObj.Continue();
                

                break;
        }
    }
    public void AfterTienda(string dialog)
    {
        StopAllCoroutines();
        villagerText.text = dialog;
        StartCoroutine(slowText());

        selectionDialog.SetActive(true);
        talkObj.tiendaAbierta = true;
        selectionOptions[0].gameObject.GetComponent<TextMeshProUGUI>().text = "si";
        selectionOptions[0].gameObject.SetActive(true);
        selectionOptions[0].Select();
        selectionOptions[1].gameObject.GetComponent<TextMeshProUGUI>().text = "no";
        selectionOptions[1].gameObject.SetActive(true);
    }
    /// <summary>
    /// If the dialogbox is active, disable all the dialog UI, disable the dialogue camera and change the input map to FREEMOVE
    /// </summary>
    public void CloseDialog(InputAction.CallbackContext context)
    {
        
        shellTienda.SetActive(false);
        tienda.SetActive(false);
        if (!isActive)
            return;
        isActive = !isActive;
        input.SwitchCurrentActionMap(Utils.FREEMOVE_INPUTMAP);
        foreach (Transform child in transform) //Deactivate text and backgrounds
            child.gameObject.SetActive(false);
        if (isCoroutineActive)  // If the animation is playing, then it's forced to stop
            StopAllCoroutines();
        talkObj.tiendaAbierta = false;
        talkObj.actualTalk = 0;
        talkObj.isActive = false;
        talkObj.isBuyShop = false;
        talkObj.isSellShop = false;
        talkObj.DisableCamera(); // Disable the npc camera
    }
    /// <summary>
    /// Animation for the npc dialogue text. The Coroutine spawns the string char by char to match the length of the audio
    /// </summary>
    /// <returns></returns>
    IEnumerator slowText()
    {
        audioSource.PlayOneShot(npcTalk_clip);
        this.isCoroutineActive = true;
        int visibleChar = 0;
        int fullTextSize = OnNPCInteract.dialogue.Length;
        float audioDuration = this.npcTalk_clip.length;
        while (visibleChar <= fullTextSize)
        {
            villagerText.maxVisibleCharacters = visibleChar;
            yield return new WaitForSeconds(audioDuration / fullTextSize);
            visibleChar++;
        }
        didLastTextFinish = true;
        this.isCoroutineActive = false;
    }
    public void ContinueConversation(InputAction.CallbackContext context)
    {
        if (!didLastTextFinish)
            return;

        TalkableObject[] objects = GameObject.FindObjectsByType<TalkableObject>(FindObjectsSortMode.None);
        foreach(TalkableObject o in objects)
        {
            if (o.isActive && didLastTextFinish)
            {
                didLastTextFinish = false;
                o.Continue();
            }
        }
    }


    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
