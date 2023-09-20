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
        for(int i = 0; i < vertices.Length; i++)
        {
            Vector3 worldPos = transform.TransformPoint(objectVertices[i]);
            vertices[i] = BuildingSystem.current.gridLayout.WorldToCell(worldPos);
        }
        Size = new Vector3Int(Mathf.Abs((vertices[0] - vertices[1]).x),
                            Mathf.Abs((vertices[0] - vertices[3]).y),
                            1);
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
    }

    /// <summary>
    /// Acci�n de colocarse en el mapa
    /// </summary>
    public virtual void Place()
    {
        ObjectDrag drag = gameObject.GetComponent<ObjectDrag>();
        Destroy(drag);

        Placed = true;

        //Invoke events of placements
    }
}
