using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryItems_ScriptableObject", menuName = "Custom Assets/InventoryItems")]
public class InventoryItem_ScriptableObject : ScriptableObject
{
    [Header("Atributos Visuales")]
    [SerializeField] private Sprite _ItemSprite;
    [SerializeField] private GameObject _object3D;

    [Header("Atributos cuantitativos")]
    [SerializeField] private bool _IsStackable;
    [SerializeField] private int _MaxStackSize;

    [Header("Atributos cualitativos")]
    [SerializeField] private string _Name;
    [SerializeField] private bool _IsPlantable;
    [SerializeField] private bool _IsEatable;
    [SerializeField] private bool _IsEquipable;
    [SerializeField] private bool _IsDropeable;

    public int ID => GetInstanceID();
    public bool IsStackable => _IsStackable;
    public int MaxStackSize => _MaxStackSize;
    public string Name => _Name;
    public Sprite ItemSprite => _ItemSprite;
    public GameObject object3D => _object3D;
    public bool IsPlantable => _IsPlantable;
    public bool IsEatable => _IsEatable;
    public bool IsEquipable => _IsEquipable;
    public bool IsDropeable => _IsDropeable;
}
