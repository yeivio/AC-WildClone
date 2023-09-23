using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private bool canMove;

    private void moveObject()
    {
        PlayerController aux = FindAnyObjectByType<PlayerController>();
        Vector3 rot = aux.transform.forward;
        Vector3 sum = (vectorRounded(rot) * BuildingSystem.current.gridLayout.cellSize.x)
            + this.transform.position;
        this.transform.position = BuildingSystem.current.SnapCoordinateToGrid(sum);
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
        if (Input.GetKeyDown(KeyCode.P) && canMove)
        {
            moveObject();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        canMove = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        canMove = true;
    }
    private void OnTriggerExit(Collider other)
    {
        canMove = false;
    }


}
