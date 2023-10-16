using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using System.Xml.Linq;
using UnityEngine.UIElements;

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

        Debug.Log($"Object to be added at position: {gridPosition}");
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new(positionToOccupy, ID, placedObjectIndex);
        foreach (var pos in positionToOccupy)
        {
            Debug.Log($"Position to be occuppied: {pos}");
            if(placedObjects.ContainsKey(pos))
            {
                throw new Exception($"Cell in position {pos} already occupied");
            }
            placedObjects.Add(pos, data);
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
            for(int z=0; z < objectSize.z; z++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, z));
            }
        }
        return returnVal;
    }

    public PlacementData CanPlaceObjectAt(Vector3Int gridPosition, Vector3Int objectSize)
    {
        /*
         * Define wehter the object can be placed at a position given its size
         * If the object can't be placed the data of the grid position is returned
         * in other case null is returned indicating there's nothing in there.
         */
        List<Vector3Int> positionToOccupy = CalculatePositions(gridPosition, objectSize);
        foreach(var pos in positionToOccupy)
        {
            Debug.Log($"Position{pos}");
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
            Vector3 position = placeableData.GetStartPosition();
            gridData.AddObjectAt(
                new Vector3Int(
                    Mathf.FloorToInt(position.x),
                    0,
                    Mathf.FloorToInt(position.z)),
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