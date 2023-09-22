using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerController aux = FindAnyObjectByType<PlayerController>();

            Vector3 rot = aux.transform.forward;
            Vector3 sum = (vectorRounded(aux.transform.forward) * BuildingSystem.current.gridLayout.cellSize.x)
                + this.transform.position;
            this.transform.position = BuildingSystem.current.SnapCoordinateToGrid(sum);
        }
    }

    private Vector3 vectorRounded(Vector3 vector)
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
