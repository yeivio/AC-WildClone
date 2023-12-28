using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
   public InventoryItem_ScriptableObject inventorySprite;
   public BuildingSystem bS;

    public void Start()
    {
        bS = FindAnyObjectByType<BuildingSystem>();
    }
    public void takeObject()
   {
        bS.PickItem(this.gameObject);
        Destroy(this.gameObject);
   }
}
