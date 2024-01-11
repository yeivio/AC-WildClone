using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;
    public GridLayout gridLayout; // reference to the grid where we will build
    private Grid grid; // Reference to the grid component of the grid GameObject

    public PlaceableObject hole;
    public GameObject prefab1;
    [SerializeField] private Tile tile;
    [SerializeField] private Tilemap tilemap;

    private PlaceableObject objectToPlace; // Component of the object to be placed on the grid
    private GridData gridData; // Data of the grid with the objects that are placed

    [SerializeField] private Border[] limitBorders;

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
            objectToPlace = prefab1.GetComponent<PlaceableObject>();
            PlayerController player = FindAnyObjectByType<PlayerController>();
            //Debug.Log($"{NextPositionInGrid(player.gameObject)}, {LookingDirection(player.gameObject)}");
            CheckDrop(NextPositionInGrid(player.gameObject), LookingDirection(player.gameObject));
        }        
    }

    public bool DropItem(GameObject objectToDrop, GameObject gObject, Vector3 direction)
    // Drop Item thought to take the objectToDrop and place it at the front (forward axis) of the
    // gObject ocupying the direction passed
    {
        objectToPlace = objectToDrop.GetComponent<PlaceableObject>();
        return CheckDrop(NextPositionInGrid(gObject), direction);

    }

    /// <summary>
    /// Dropea un objeto si es posible
    /// </summary>
    /// <param name="objectToDrop"> objeto a ser puesto, debe ser un PlacebleObject</param>
    /// <param name="gObject"> objeto a ser puesto, si es un PlacebleObject debe tener indicado el tamaño del mismo correctamente</param>
    /// <param name="direction"> Dirección a ser puesto un objeto, si el objeto es de tamaño 1 es un tanto irrelevante </param>
    /// <param name="forward"> Dirección en la cual empezar a colocar el objeto </param>
    /// <returns></returns> 
    public bool DropItem(GameObject objectToDrop, GameObject gObject, Vector3 direction, Vector3 forward)
    // Drop Item thought to take the objectToDrop and place it at the side specified by forward of the
    // gObject ocupying the direction passed
    {
        objectToPlace = objectToDrop.GetComponent<PlaceableObject>();
        return CheckDrop(NextPositionInGrid(gObject,forward), direction);
    }
    public void PickItem(GameObject objectToDrop)
    {
        Vector3 cellPosition = SnapCoordinateToGrid(objectToDrop.transform.position);
        gridData.FreeObject(cellPosition);
    }
    public bool CheckDrop(Vector3 position, Vector3 direction)
    {
        if (limitBorders.Any(x => x.isColliding))
            return false;

        //Debug.Log($"Object to place: {objectToPlace}");
        Vector3 cellPosition = SnapCoordinateToGrid(position);
        PlacementData canBePlace = gridData.CanPlaceObjectAt(cellPosition, objectToPlace.Size, direction);
        Debug.Log($"placing {canBePlace}");

        if (canBePlace == null)
        {
            
            Vector3 positionToPlace = gridData.AddObjectAt(
                cellPosition,
                objectToPlace.Size,
                direction,
                objectToPlace.gameObject);
            Debug.Log($"Object to be positioned at: {positionToPlace}");
        }
        else if (
            objectToPlace.TryGetComponent<PlantableObject>(out PlantableObject toPlant) && // If we are trying to place a plant
            canBePlace.PlacedObject.TryGetComponent<HoleObject>(out HoleObject hole) // And that something is a hole
            )
        {
            Debug.Log("Hola");
            objectToPlace = toPlant.treePrefab.GetComponent<PlaceableObject>();
            gridData.FreeSpace(canBePlace);
            Debug.Log("Holaaa");
            Vector3 positionToPlace = gridData.AddObjectAt(
                cellPosition,
                objectToPlace.Size,
                direction,
                objectToPlace.gameObject);

            return true;
        }
        else
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Indica si un objeto puede ser posicionado por otro
    /// </summary>
    /// <param name="objectToDrop"> objeto a ser puesto, debe ser un PlacebleObject</param>
    /// <param name="gObject"> objeto a ser puesto, si es un PlacebleObject debe tener indicado el tamaño del mismo correctamente</param>
    /// <param name="direction"> Dirección a ser puesto un objeto, si el objeto es de tamaño 1 es un tanto irrelevante </param>
    /// <param name="forward"> Dirección en la cual empezar a colocar el objeto </param>
    /// <returns></returns> 
    public bool CanPlaceObject(GameObject objectToDrop, GameObject gObject, Vector3 direction, Vector3 forward)
    {
        objectToPlace = objectToDrop.GetComponent<PlaceableObject>();
        Vector3 cellPosition = SnapCoordinateToGrid(NextPositionInGrid(gObject, forward));
        PlacementData canBePlace = gridData.CanPlaceObjectAt(cellPosition, objectToPlace.Size, direction);
        Debug.Log($"test placing {canBePlace}");
        return canBePlace == null;
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
    public Vector3 NextPositionInGrid(GameObject gObject, Vector3 forward)
    {
        Vector3 positionedElement = new(0, 0, 0); // if element is a positioned have in mind its size 
        if(gObject.TryGetComponent<PlaceableObject>(out PlaceableObject p))
        {
            if(p.Size.x != 1)
                positionedElement.x = p.Size.x/2;
            if (p.Size.z != 1)
                positionedElement.z = p.Size.z/2;
        }
            
        Vector3 rot = forward.normalized;
        positionedElement.Scale(rot);
        Vector3 sum = (
                rot *
                current.gridLayout.cellSize.x +
                gObject.transform.position +
                positionedElement
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
    public GameObject InitializeWithObject(GameObject prefab, Vector3 position)
    {
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        return obj;

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

    public bool Dig(Vector3 position, Vector3 direction)
    {
        PlacementData placementData = gridData.CanPlaceObjectAt(position, hole.Size, direction);
        if (placementData != null)
        {
            
            if (placementData.PlacedObject.CompareTag("Hole"))
            {
                gridData.FreeSpace(placementData);
                return true;
            }
                
            return false;
        }
        objectToPlace = hole;
        CheckDrop(position, direction);
        return true;
    }
}
