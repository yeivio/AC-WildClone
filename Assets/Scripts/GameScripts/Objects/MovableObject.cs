using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovableObject : MonoBehaviour
{   
    public void moveObject(Vector2 input)
    {
        Vector3 rot = new Vector3(input.x, input.y, 0);
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
}
