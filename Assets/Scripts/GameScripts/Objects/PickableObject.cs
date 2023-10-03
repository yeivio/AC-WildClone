using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
   [SerializeField] private InventoryItem_ScriptableObject inventorySprite;

   public InventoryItem_ScriptableObject takeObject()
   {
        Destroy(gameObject);
        return inventorySprite;
   }

}
