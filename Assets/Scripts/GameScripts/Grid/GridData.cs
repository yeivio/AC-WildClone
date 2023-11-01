using UnityEngine;
using System.Collections.Generic;
using System;

public class GridData
{
    Dictionary<Vector3, PlacementData> placedObjects = new();

    public Vector3 AddObjectAt(Vector3 gridPosition,
                            Vector3 objectSize,
                            Vector3 direction,
                            GameObject objectToPlace)
    {
        /*
         * Add an object to the gridData
         */
        BuildingSystem buildingSystem = BuildingSystem.current;
        
        gridPosition = new Vector3(gridPosition.x, 0, gridPosition.z);
        List<Vector3> positionsToOccupy = CalculatePositions(gridPosition, objectSize, direction);
        
        GameObject placedObject = buildingSystem.InitializeWithObject(objectToPlace.gameObject, CentralPosition(positionsToOccupy));
        PlacementData data = new(positionsToOccupy, placedObject);
        foreach (var pos in positionsToOccupy)
        {
            //Debug.Log(pos);
            if(placedObjects.ContainsKey(pos))
            {
                GameObject.Destroy(placedObject);
                throw new Exception($"Cell in position {pos} already occupied");

            }
            placedObjects.Add(pos, data);
        }
        return CentralPosition(positionsToOccupy);
    }
    public Vector3 CentralPosition(List<Vector3> positionsToOccupy)
    {
        float minZ = float.MaxValue;
        float maxZ = float.MinValue;
        float minX = float.MaxValue;
        float maxX = float.MinValue;

        foreach (Vector3 vertex in positionsToOccupy)
        {
            minX = Mathf.Min(minX, vertex.x);
            minZ = Mathf.Min(minZ, vertex.z);
            maxX = Mathf.Max(maxX, vertex.x);
            maxZ = Mathf.Max(maxZ, vertex.z);
        }

        Vector3 p0 = new(maxX, 0, maxZ);
        Vector3 p3 = new(minX, 0, minZ);

        Vector3 center = Vector3.Lerp(p0,p3,0.5f);


        return BuildingSystem.current.gridLayout.LocalToWorld(center);



    }
    private List<Vector3> CalculatePositions(Vector3 initialPos, Vector3 objectSize, Vector3 direction)
    {
        /*
         * Return a list of Vector3 with the positions occuppied by an object
         * with an initial position at gridPosition and a size of objectSize
         * direction is a vector of the form (x,0,y) being x,y ∈ {1,-1}
         */

        List<Vector3> returnVal = new();
        for(int x = 0; x< objectSize.x; x++)
        {
            for(int z=0; z < objectSize.z; z++)
            {
                returnVal.Add(initialPos + Vector3.Scale(direction,new Vector3(x, 0, z)));
            }
        }

        
        return returnVal;
    }

    public PlacementData CanPlaceObjectAt(Vector3 gridPosition, Vector3 objectSize, Vector3 direction)
    {
        /*
         * Define wehter the object can be placed at a position given its size
         * If the object can't be placed the data of the grid position is returned
         * in other case null is returned indicating there's nothing in there.
         */
        gridPosition = new Vector3(gridPosition.x, 0, gridPosition.z);
        List<Vector3> positionToOccupy = CalculatePositions(gridPosition, objectSize, direction);
        foreach(var pos in positionToOccupy)
        {
            //Debug.Log($"Position{pos}");
            if(placedObjects.ContainsKey(pos))
            {
                //Debug.Log($"Key that is contained: {pos}\n" +
                    //$"Placed object in that position: {placedObjects[pos]}");
                return placedObjects[pos];
            }
        }
        return null;
    }
    public static GridData Initialize()
    {
        /*
         Initialize the grid data with all the elements that where created by
         the tilemap and have the tag grid

         Initially the idea is to create a map with a tilemap, delete it and
         with the remaining objects that we want to be interactive mark them
         with grid tag.
         */
        GameObject[] allGObjects = GameObject.FindGameObjectsWithTag("Grid");

        GridData gridData = new();
        foreach ( var gObject in allGObjects)
        {
            // First we correct the position of the object into the grid
            Vector3 correctedPos = BuildingSystem.current.
                    SnapCoordinateToGrid(gObject.transform.position); 
            Vector3 originalPos = gObject.transform.position;
            gObject.transform.position = new(correctedPos.x, originalPos.y, correctedPos.z);

            // Then we add the object to the gridData with the corrected position
            PlaceableObject placeableData = gObject.GetComponent<PlaceableObject>();
            Vector3 position = placeableData.transform.position;
            gridData.AddObjectAt(
                position,
                placeableData.Size,
                new(1,0,1),
                gObject);
            GameObject.Destroy(gObject);
        }
        return gridData;
    }
    public void FreeSpace(PlacementData data)
    {
        GameObject.Destroy(data.PlacedObject.gameObject);
        foreach(Vector3 position in data.occupiedPositions)
        {
            placedObjects.Remove(position);
        }
    }
    public List<PlacementData> Neighbors(Vector3 position, int howFar)
    // Return the neighbors cell data to the position
    // howFar indicate how far to look for neighbors
    // howFar = 1 indicates to look for only the sourounding neighbors
    {
        position = BuildingSystem.current.SnapCoordinateToGrid(position);
        List<PlacementData> toReturn = new();

        List<Vector3> vectors = new();
        for( int i = 1; i<=howFar; i++)
        {
            vectors.Add(Vector3.forward * i);
            vectors.Add(Vector3.back * i);
            vectors.Add(Vector3.left * i);
            vectors.Add(Vector3.right * i);
            for(int j = 1; j<=howFar; j++)
            {
                vectors.Add(Vector3.right * j + Vector3.forward * i);
                vectors.Add(Vector3.right * j + Vector3.back * i);
                vectors.Add(Vector3.left * j + Vector3.forward * i);
                vectors.Add(Vector3.left * j + Vector3.back * i);
            }

        };
        Vector3 cellSize = new(
            BuildingSystem.current.gridLayout.cellSize.x,
            0,
            BuildingSystem.current.gridLayout.cellSize.y);
        
        foreach(Vector3 vector in vectors)
        {
            Vector3 aux = vector;

            aux.Scale(cellSize);
            if (placedObjects.ContainsKey(aux + position ))
            {
                toReturn.Add(placedObjects[aux + position]);
            }
                    
        }
        

        return toReturn;
    }
   
}

public class PlacementData
    // Data that will be saved per cell in the grid
{
    public List<Vector3> occupiedPositions;
    public GameObject PlacedObject { get; private set; }

    public PlacementData(List<Vector3> occuppiedPositions, GameObject placedObject)
    {
        this.occupiedPositions = occuppiedPositions;
        PlacedObject = placedObject;
    }


}