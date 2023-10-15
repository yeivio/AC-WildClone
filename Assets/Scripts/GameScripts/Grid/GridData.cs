using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition,
                            Vector3Int objectSize,
                            int ID,
                            int placedObjectIndex)
    {
        /*
         * Add an object to the gridData
         */

        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new(positionToOccupy, ID, placedObjectIndex);
        foreach (var pos in positionToOccupy)
        {
            Debug.Log($"Position to be occuppied: {pos}");
            if(placedObjects.ContainsKey(pos))
            {
                throw new Exception($"Cell in position {pos} already occupied");
            }
            placedObjects[pos] = data;
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector3Int objectSize)
    {
        /*
         * Return a list of Vector3Int with the positions occuppied by an object
         * with an initial position at gridPosition and a size of objectSize
         */

        List<Vector3Int> returnVal = new();
        for(int x = 0; x< objectSize.x; x++)
        {
            for(int y=0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, y, 0));
            }
        }
        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector3Int objectSize)
    {
        /*
         * Define wehter the object can be placed at a position given its size
         */
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach(var pos in positionToOccupy)
        {
            if(placedObjects.ContainsKey(pos))
            {
                Debug.Log($"Key that is contained: {pos}\n" +
                    $"Placed object in that position: {placedObjects[pos]}");
                return false;
            }
        }
        return true;
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
            
            PlaceableObject placeableData = gObject.GetComponent<PlaceableObject>();
            Vector3 position = placeableData.GetStartPosition();
            Debug.Log($"Object added during initialization of the grid: {gObject}\n" +
                $"At position: {position}");
            gridData.AddObjectAt(
                new Vector3Int(
                    Mathf.FloorToInt(position.x),
                    Mathf.FloorToInt(position.y),
                    0),
                placeableData.Size,
                gObject.GetInstanceID(),
                gObject.GetInstanceID());
        }
        return gridData;
    }
}

public class PlacementData
{
    public List<Vector3Int> occupiedPositions;
    public int ID { get; private set; }
    public int PlacedObjectIndex { get; private set; }

    public PlacementData(List<Vector3Int> occuppiedPositions, int iD, int placedObjectIndex)
    {
        this.occupiedPositions = occuppiedPositions;
        ID = iD;
        PlacedObjectIndex = placedObjectIndex;
    }


}