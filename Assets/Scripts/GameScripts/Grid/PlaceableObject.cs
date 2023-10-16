using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlaceableObject : MonoBehaviour
{
    public Vector3Int Size;


    /// <summary>
    /// Calcula el tama�o absoluto del objeto usando los vertices
    /// </summary>

    private void CalculateSizeInCells()
    {
        Vector3Int[] objectVertices = CalculateVertices();

        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int minZ = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;
        int maxZ = int.MinValue;

        foreach (Vector3Int vertex in objectVertices)
        {
            minX = Mathf.Min(minX, vertex.x);
            minY = Mathf.Min(minY, vertex.y);
            minZ = Mathf.Min(minZ, vertex.z);
            maxX = Mathf.Max(maxX, vertex.x);
            maxY = Mathf.Max(maxY, vertex.y);
            maxZ = Mathf.Max(maxZ, vertex.z);
        }

        Size = new Vector3Int(maxX - minX + 1, maxY - minY + 1, maxZ - minZ + 1);
        //Debug.Log($"Size of the placeableObject {Size}");
    }

    public Vector3Int[] CalculateVertices()
    {


        Vector3Int[] objectVertices = new Vector3Int[4];
        GridLayout currentGrid = BuildingSystem.current.gridLayout;
        Vector3Int cellPosition = currentGrid.WorldToCell(gameObject.transform.position);
        Vector3Int cellSize = new(
            // Size of half of a cell, used to calculate from the center the sides
            Mathf.FloorToInt(currentGrid.cellSize.x/2),
            0,
            Mathf.FloorToInt(currentGrid.cellSize.z/2));
        objectVertices[0] = cellPosition - cellSize;
        objectVertices[1] = cellPosition - new Vector3Int(cellSize.x,0,0);
        objectVertices[2] = cellPosition - new Vector3Int(0, 0, cellSize.z);
        objectVertices[3] = cellPosition + cellSize;
        return objectVertices;

    }
    private void Start()
    {
        CalculateVertices();
    }
    /// <summary>
    /// Obtiene el primer vertice del objeto y lo convierte a la posici�n en el mapa
    /// </summary>
    /// <returns></returns>
    public Vector3 GetStartPosition()
    {
        Vector3Int[] objectVertices = CalculateVertices();

        return transform.TransformPoint(objectVertices[0]);
    }
}
