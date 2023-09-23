using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectContact : MonoBehaviour
{
    private bool active;
    [SerializeField] private bool canBeMove;
    [SerializeField] private bool canBeTaken;

    private void moveObject()
    {
        PlayerController aux = FindAnyObjectByType<PlayerController>();
        Vector3 rot = aux.transform.forward;
        Vector3 sum = (vectorRounded(rot) * BuildingSystem.current.gridLayout.cellSize.x)
            + this.transform.position;
        this.transform.position = BuildingSystem.current.SnapCoordinateToGrid(sum);
    }

    private void takeObject()
    // Take the object to the inventary
    // TODO: Add to the inventary
    {
        Destroy(gameObject);
    }

    public static Vector3 vectorRounded(Vector3 vector)
    {
        Vector3 roundedVector = new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
        if (roundedVector.x == roundedVector.z) {
            return Vector3.zero;
        }
        else {
            return new Vector3(Mathf.Round(vector.x), Mathf.Round(vector.y), Mathf.Round(vector.z));
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && canBeMove && active)
        {
            moveObject();
        }
        else if (Input.GetKeyDown(KeyCode.T) && canBeTaken && active)
            takeObject();
    }

    private void OnCollisionEnter(Collision collision)
    {
        active = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        active = true;
    }
    private void OnTriggerExit(Collider other)
    {
        active = false;
    }
    private void OnCollisionExit(Collision collision)
    {
        active = false;
    }


}
