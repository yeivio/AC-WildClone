using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;
    public GridLayout gridLayout; // reference to the grid where we will build
    private Grid grid; // Reference to the grid component of the grid GameObject

    public GameObject prefab1;
    [SerializeField] private Tile tile;
    [SerializeField] private Tilemap tilemap;

    private PlaceableObject objectToPlace; // Component of the object to be placed on the grid
    private GridData gridData; // Data of the grid with the objects that are placed

    private Vector3 centerFirstCell;
    private void Awake()
    {
        current = this; // Patron Singleton
        
        
        grid = gridLayout.gameObject.GetComponent<Grid>();
        centerFirstCell = SnapCoordinateToGrid(new(0,0,0));

    }
    private void Start()
    {
        gridData = GridData.Initialize();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            objectToPlace = prefab1.GetComponent<PlaceableObject>();
            PlayerController player = FindAnyObjectByType<PlayerController>();
            //Debug.Log($"{NextPositionInGrid(player.gameObject)}, {LookingDirection(player.gameObject)}");
            CheckDrop(NextPositionInGrid(player.gameObject), LookingDirection(player.gameObject));
        }
        
    }

    public bool DropItem(GameObject objectToDrop, GameObject gObject, Vector3 direction)
    {
        objectToPlace = objectToDrop.GetComponent<PlaceableObject>();
        return CheckDrop(NextPositionInGrid(gObject), direction);

    }
    public bool CheckDrop(Vector3 position, Vector3 direction)
    {
        //Debug.Log($"Object to place: {objectToPlace}");
        Vector3 cellPosition = SnapCoordinateToGrid(position);
        PlacementData canBePlace = gridData.CanPlaceObjectAt(cellPosition, objectToPlace.Size, direction);
        PlayerInteractionsController player = FindAnyObjectByType<PlayerInteractionsController>();
        Debug.Log($"Object can be placed: {canBePlace == null}");
        if (canBePlace == null)
        {
            
            Vector3 positionToPlace = gridData.AddObjectAt(
                cellPosition,
                objectToPlace.Size,
                direction,
                objectToPlace.gameObject);
            Debug.Log($"Object to be positioned at: {positionToPlace}");
            InitializeWithObject(objectToPlace.gameObject, positionToPlace);
        }
        else if (
            objectToPlace.TryGetComponent<PlantableObject>(out PlantableObject toPlant) && // If we are trying to place a plant
            player.interactingObject != null && // And we are interacting with something
            player.interactingObject.CompareTag("Hole") // And that something is a hole
            )
        {
            //Debug.Log("Planted");
            Destroy(player.interactingObject);
        }
        else
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Dada una posici�n, devuelve el vector de la casilla m�s cercana
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public Vector3 SnapCoordinateToGrid(Vector3 position)
        // Returns the center of the grid cell of the position passed as argument
    {
        // converts the input position to a Vector3Int cell position
        Vector3Int cellpos = gridLayout.WorldToCell(position);
        // get the center position of the grid cell at the cellpos position
        position = grid.GetCellCenterWorld(cellpos);
        return position;
    }
    public Vector3 NextPositionInGrid(GameObject gObject)
        // Returns the center of the grid cell of the position in the forward
        // direction of the gameobject

        // Only works if the grid cell are squares
    {
        Vector3 rot = gObject.transform.forward.normalized;
        Vector3 sum = (
                rot *
                current.gridLayout.cellSize.x +
                gObject.transform.position
                );
        return SnapCoordinateToGrid(sum);
    }

    /// <summary>
    /// Instanciador de objetos 
    /// </summary>
    /// <param name="prefab"></param>
    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = FindAnyObjectByType<PlayerController>().transform.position;

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
    }
    public void InitializeWithObject(GameObject prefab, Vector3 position)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();

        //Debug.Log($"Termine de inicializar {objectToPlace} objeto");
    }
    public Vector3 LookingDirection(GameObject gobject)
    {
        Vector3 dif = NextPositionInGrid(gobject) - gobject.transform.position;
        return new(
            dif.x >= 0 ? 1 : -1,
            0,
            dif.z >= 0 ? 1 : -1
            ) ;
    }

}
