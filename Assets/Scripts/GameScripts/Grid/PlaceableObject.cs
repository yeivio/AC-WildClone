using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableObject : MonoBehaviour
{
    public bool Placed { get; private set; }
    public Vector3Int Size { get; private set; }
    private Vector3[] objectVertices;

    /// <summary>
    /// Calcular los vertices de cada objeto, para eso se utiliza el punto central del objeto y se dividepor la mitad
    /// y dependiendo de qu� vertice se busque se suma o se resta la mitad de la arista
    /// </summary>
    private void GetColliderVertexPositionsLocal() { 
    
        BoxCollider b = gameObject.GetComponent<BoxCollider>();
        objectVertices = new Vector3[4];
        objectVertices[0] = b.center + new Vector3(-b.size.x, -b.size.y, -b.size.z) * 0.5f;
        objectVertices[1] = b.center + new Vector3(b.size.x, -b.size.y, -b.size.z) * 0.5f;
        objectVertices[2] = b.center + new Vector3(b.size.x, -b.size.y, b.size.z) * 0.5f;
        objectVertices[3] = b.center + new Vector3(-b.size.x, -b.size.y, b.size.z) * 0.5f;
    }

    /// <summary>
    /// Calcula el tama�o absoluto del objeto usando los vertices
    /// </summary>

    private void CalculateSizeInCells()
    {
        Vector3Int[] vertices = new Vector3Int[objectVertices.Length];
        Vector3Int[] cellVertices = new Vector3Int[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(objectVertices[i]);
            cellVertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPos);
        }

        int minX = int.MaxValue;
        int minY = int.MaxValue;
        int minZ = int.MaxValue;
        int maxX = int.MinValue;
        int maxY = int.MinValue;
        int maxZ = int.MinValue;

        foreach (Vector3Int vertex in cellVertices)
        {
            minX = Mathf.Min(minX, vertex.x);
            minY = Mathf.Min(minY, vertex.y);
            minZ = Mathf.Min(minZ, vertex.z);
            maxX = Mathf.Max(maxX, vertex.x);
            maxY = Mathf.Max(maxY, vertex.y);
            maxZ = Mathf.Max(maxZ, vertex.z);
        }

        Size = new Vector3Int(maxX - minX + 1, maxY - minY + 1, maxZ - minZ + 1);
        // As long as grid is 2d Z will be always 1
    }

    /// <summary>
    /// Obtiene el primer vertice del objeto y lo convierte a la posici�n en el mapa
    /// </summary>
    /// <returns></returns>
    public Vector3 GetStartPosition()
    {
        return transform.TransformPoint(objectVertices[0]);
    }

    private void Start()
    {
        GetColliderVertexPositionsLocal();
        CalculateSizeInCells();
        PlayerController aux = FindAnyObjectByType<PlayerController>();

        Vector3 rot = aux.transform.forward;
        Vector3 sum = (ObjectContact.vectorRounded(rot) * BuildingSystem.current.gridLayout.cellSize.x)*2
            + this.transform.position;
        this.transform.position = BuildingSystem.current.SnapCoordinateToGrid(sum);

    }

    /// <summary>
    /// Acci�n de colocarse en el mapa
    /// </summary>
    public virtual void Place()
    {

        Placed = true;

        //Invoke events of placements
    }
}
