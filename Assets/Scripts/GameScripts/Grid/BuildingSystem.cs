using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;
    public GridLayout gridLayout; // reference to the grid where we will build
    private Grid grid; // Reference to the grid component of the grid GameObject
    [SerializeField] private Tilemap MainTilemap; // Reference to the tilemap inside the grid
    [SerializeField] private TileBase objectTile; // Reference to the sprite that will be drawn and looked for

    public GameObject prefab1;

    private PlaceableObject objectToPlace; // Component of the object to be placed on the grid



    private void Awake()
    {
        current = this; // Patron Singleton
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            InitializeWithObject(prefab1);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && objectToPlace)
            Destroy(objectToPlace.gameObject);
        
    }

    private void LateUpdate()
    {
        if (!objectToPlace)
            return;
        if (CanBeplaced(objectToPlace))
        {
            objectToPlace.Place();
            Vector3Int start = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
            TakeArea(start, objectToPlace.Size);
            objectToPlace = null;
        }
        else
        {
            Destroy(objectToPlace.gameObject);
        }
    }
    
    /// <summary>
    /// Dada una posici�n, devuelve el vector de la casilla m�s cercana
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        // converts the input position to a Vector3Int cell position
        Vector3Int cellpos = gridLayout.WorldToCell(position);
        // get the center position of the grid cell at the cellpos position
        position = grid.GetCellCenterWorld(cellpos);
        return position;
    }

    public static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    // Get the tiles in a given area of a given tilemap
    {
        TileBase[]array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach(var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, 0);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }

        return array;
    }

    /// <summary>
    /// Instanciador de objetos 
    /// </summary>
    /// <param name="prefab"></param>
    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(FindAnyObjectByType<PlayerController>().transform.position);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
    }

    private bool CanBeplaced(PlaceableObject placeableObject)
    // Look if there are sprites of objectTile, if there are return false
    {
        BoundsInt area = new BoundsInt(); // area where the object has been placed
        area.position = gridLayout.WorldToCell(objectToPlace.GetStartPosition());
        area.size = placeableObject.Size;
        area.size = new Vector3Int(area.size.x, area.size.y, area.size.z);

        TileBase[] baseArray = GetTilesBlock(area, MainTilemap); // array in the tile where it test if it is empty
        foreach(var b in baseArray)
        // Searching for a tile that represents that the area has been taken
        {
            if(b == objectTile)
            {
                return false;
            }
        }
        return true;
    }

    public void TakeArea(Vector3Int start, Vector3Int size)
    // Take the area specified: in this case it will be taken by painting the grid
    {
        MainTilemap.BoxFill(start, objectTile, start.x, start.y,
                            start.x + size.x - 1, start.y + size.y - 1);
    }
}
