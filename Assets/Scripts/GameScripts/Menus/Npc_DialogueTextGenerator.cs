using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Npc_Dialogue : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI villagerText;
    [SerializeField] private TextMeshProUGUI villagerName;
    [SerializeField] private NPCConfig_ScriptableObject OnNPCInteract;
    public float speedText;
    private int visibleChar = 0;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        villagerText.maxVisibleCharacters = visibleChar;
    }

    
    
    IEnumerator slowText()
    {
        int visibleChar = 0;
        int fullTextSize = villagerText.text.Length;
        while (visibleChar < fullTextSize)
        {
            villagerText.maxVisibleCharacters = visibleChar;
            yield return new WaitForSeconds(speedText);
            visibleChar++;
        }
        yield return null;
    }



}
