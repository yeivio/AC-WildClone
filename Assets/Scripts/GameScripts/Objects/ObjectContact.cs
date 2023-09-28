using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectContact : MonoBehaviour
{
    [SerializeField] private bool canBeMove;
    [SerializeField] private bool canBeTaken;

    public void moveObject()
    {

        if (!canBeMove)
            return;
        PlayerController aux = FindAnyObjectByType<PlayerController>();
        Vector3 rot = aux.transform.forward;
        Vector3 sum = (vectorRounded(rot) * BuildingSystem.current.gridLayout.cellSize.x)
            + this.transform.position;
        this.transform.position = BuildingSystem.current.SnapCoordinateToGrid(sum);
    }

    public void takeObject()
    {
        if (!canBeTaken)
            return;
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
}
