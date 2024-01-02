using UnityEngine;
using System.Collections;

public class Border : MonoBehaviour
{
    public bool isColliding;
    private void Start()
    {
        isColliding = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
            isColliding = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
            isColliding = false;
    }
}

