using UnityEngine;
using System.Collections;

public class Border : MonoBehaviour
{
    public bool isColliding;
    private void Start()
    {
        isColliding = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
            isColliding = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
            isColliding = false;
    }
}

