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
    private GridData gridData; // Data of the grid with the objects that are placed


    private void Awake()
    {
        current = this; // Patron Singleton
        
        
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }
    private void Start()
    {
        gridData = GridData.Initialize();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            InitializeWithObject(prefab1);
        }
        else if (Input.GetKeyUp(KeyCode.J))
        {
            Vector3 position = objectToPlace.GetStartPosition();

            Vector3Int intPosition = new Vector3Int(
                    Mathf.FloorToInt(position.x),
                    Mathf.FloorToInt(position.y),
                    0);

            bool canBePlace = gridData.CanPlaceObjectAt(intPosition, objectToPlace.Size);
            Debug.Log($"Object can be placed: { canBePlace}");
            Debug.Log($"Position where it would be placed: {position}");
            if (! canBePlace)
                Destroy(objectToPlace.gameObject);
            else
            {
                gridData.AddObjectAt(
                    intPosition,
                    objectToPlace.Size,
                    objectToPlace.GetInstanceID(),
                    objectToPlace.GetInstanceID());
            }
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

}
