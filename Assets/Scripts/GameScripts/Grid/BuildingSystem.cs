using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingSystem : MonoBehaviour
{
    public static BuildingSystem current;
    public GridLayout gridLayout; // reference to the grid where we will build
    private Grid grid; // Reference to the grid component of the grid GameObject

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
            objectToPlace = prefab1.GetComponent<PlaceableObject>();
            PlayerController aux = FindAnyObjectByType<PlayerController>();

            Vector3 rot = aux.transform.forward.normalized;
            Vector3 sum = (
                MovableObject.vectorRounded(rot) *
                current.gridLayout.cellSize.x +
                aux.transform.position
                ) ;
            //Debug.Log(sum);
            CheckDrop(sum);
        }
        
    }

    public bool DropItem(GameObject objectToDrop, Vector3 position)
    {
        objectToPlace = objectToDrop.GetComponent<PlaceableObject>();
        return CheckDrop(position);

    }
    public bool CheckDrop(Vector3 position)
    {
        //Debug.Log($"Object to place: {objectToPlace}");
        Vector3 cellPosition = SnapCoordinateToGrid(position);

        Vector3Int intPosition = new Vector3Int(
                Mathf.FloorToInt(cellPosition.x),
                0,
                Mathf.FloorToInt(cellPosition.z));

        PlacementData canBePlace = gridData.CanPlaceObjectAt(intPosition, objectToPlace.Size);
        PlayerInteractionsController player = FindAnyObjectByType<PlayerInteractionsController>();
        Debug.Log($"Object can be placed: {canBePlace}");
        //Debug.Log($"Position where it would be placed: {cellPosition}");
        if (canBePlace == null)
        {
            gridData.AddObjectAt(
                intPosition,
                objectToPlace.Size,
                objectToPlace.GetInstanceID(),
                objectToPlace.GetInstanceID());
            InitializeWithObject(objectToPlace.gameObject, cellPosition);
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
    public bool CheckDrop()
    {
        Vector3 position = objectToPlace.GetStartPosition();
        return CheckDrop(position);
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
    public void InitializeWithObject(GameObject prefab, Vector3 position)
    {
        position = SnapCoordinateToGrid(position);
        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();

        //Debug.Log($"Termine de inicializar {objectToPlace} objeto");
    }

}
