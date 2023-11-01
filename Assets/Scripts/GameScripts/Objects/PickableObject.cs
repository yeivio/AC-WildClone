using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
   public InventoryItem_ScriptableObject inventorySprite;

   public void takeObject()
   {
        Destroy(this.gameObject);
   }
}
