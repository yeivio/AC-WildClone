using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

/* In order to make this class to work, you need to assign the same number of treelevels and levelUpTimers, each one
 * will be associated base on the index, which means the first mesh will be associated with the first levelUpTimer.
 * The fruitPositions should be empty gameobjects.*/
public class TreeManager : MonoBehaviour
{
    [SerializeField] private GameObject[] treeLevels; // Saves the different tree meshes 
    [SerializeField] private MeshRenderer[] fruitPositions;    // Positions on the tree where the fruit should appear
    public GameObject fruit;  // Fruit the tree grows.
    [SerializeField] private int[] levelUpTimers;   // Min time that should pass for the tree to be able to upgrade into the next level
    [SerializeField] private AudioSource treeShakeAudio;
    [SerializeField] private AudioSource itemFallAudio;

    private bool hasApples; // Tree has apples at this time
    private float existingTime; // Time passed since creation or the last time a player dropped the fruits
    private int currentLevel;   //  Current level of the tree

    private const float TREE_UPDATE = 2f; // Const for set when the tree coroutines should continue their iterations
    private const float TREE_REFILL = 5f; // Minimun Time that should pass before refilling the tree with apples

    private GameObject currentActiveObject;
    public enum TreeState {
        GROWING, FULL_GROWN
    }

    private Coroutine refRestart;

    private Vector3[] copyFruitPositions; // ReadOnly save of the positions of the fruitpositions

    private void Start()
    {
        copyFruitPositions = new Vector3[fruitPositions.Length];
        int index = 0;
        foreach (MeshRenderer fruit in fruitPositions)
        {
            copyFruitPositions[index] = fruit.gameObject.transform.position;
            index++;
        }
        existingTime = 0;
        hasApples = false;
        currentActiveObject = Instantiate(treeLevels[currentLevel], this.transform); // Change into the first level mesh
        StartCoroutine(LevelUpTimer());
    }

    /// <summary>
    /// Function for when a player shakes the tree to drop the tree's items.
    /// If the tree doesn't have any apples, it won't do anything
    /// </summary>
    public void shakeTree()
    {
        if (currentActiveObject.TryGetComponent(out Animator animator)) {
            animator.GetComponent<Animator>().Play("AppleTree_Shake");
            treeShakeAudio.Play();
        }
        if (!hasApples)
            return;

        
        if (fruitPositions[0].enabled && 
            BuildingSystem.current.CanPlaceObject(this.fruit, this.gameObject, new(1, 0, 1), this.gameObject.transform.forward * -1))
        {
            StartCoroutine(AppleFallingAnimation(0, new(1, 0, 1), this.gameObject.transform.forward * -1));
        }
        
        if (fruitPositions[1].enabled &&
            BuildingSystem.current.CanPlaceObject(this.fruit, this.gameObject, new(1, 0, 1), this.gameObject.transform.right * -1))
        {
            StartCoroutine(AppleFallingAnimation(1, new(1, 0, 1), this.gameObject.transform.right * -1));
        }
        
        if (fruitPositions[2].enabled &&
            BuildingSystem.current.CanPlaceObject(this.fruit, this.gameObject, new(1, 0, 1), this.gameObject.transform.right))
        {
            StartCoroutine(AppleFallingAnimation(2, new(1, 0, 1), this.gameObject.transform.right));
        }        
    }

    private bool ExistsApplesForFalling()
    {
        bool existe = false;
        foreach (MeshRenderer aux in fruitPositions)
            if (aux.enabled)
                existe = true;
        return existe;
    }
    private IEnumerator AppleFallingAnimation(int fruitPosition, Vector3 position ,Vector3 direction)
    {

        float timer = 0f;
        float duration = 0.30f;

        Vector3 appleInitial = fruitPositions[fruitPosition].gameObject.transform.localScale;

        Vector3 initialPos = fruitPositions[fruitPosition].gameObject.transform.position;

        Vector3 finalPos = BuildingSystem.current.SnapCoordinateToGrid(
            BuildingSystem.current.NextPositionInGrid(this.gameObject, direction));
        if(!itemFallAudio.isPlaying)
            itemFallAudio.Play();
        while (timer < duration)
        {
            fruitPositions[fruitPosition].gameObject.transform.localScale = Vector3.Lerp(appleInitial, Vector3.one, timer / duration);
            fruitPositions[fruitPosition].gameObject.transform.position = Vector3.Lerp(initialPos, finalPos, timer / duration);

            timer += Time.deltaTime;
            yield return null;
        }
        fruitPositions[fruitPosition].gameObject.transform.localScale = Vector3.one;
        fruitPositions[fruitPosition].enabled = false;
        
        //Reset del objeto manzana
        fruitPositions[fruitPosition].transform.position = copyFruitPositions[fruitPosition];
        fruitPositions[fruitPosition].transform.localScale = appleInitial;

        BuildingSystem.current.DropItem(this.fruit, this.gameObject, position,direction);

        if (!ExistsApplesForFalling() && refRestart == null)
        {
            this.hasApples = false;
            refRestart = StartCoroutine(TreeRefiller());
        }
    }

    /// <summary>
    /// This coroutine starts at the moment the tree spawns and while the level of the tree is not the 
    /// highest, it's going to iterate and check every X seconds defined in constant TREE_UPDATE if it 
    /// should change based on the levelUpTimers his own mesh. The last level won't change the mesh but 
    /// will spawn the number of fruits and stops itself.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LevelUpTimer()
    {
        while (currentLevel < levelUpTimers.Length)
        {
            yield return new WaitForSeconds(TREE_UPDATE);
            existingTime += TREE_UPDATE;  // Update timer
            if (existingTime >= levelUpTimers[currentLevel])    // Upgrade level if not visible
            {
                Destroy(currentActiveObject);
                currentActiveObject = Instantiate(treeLevels[currentLevel], this.transform);
                currentLevel++;
            }
            if (currentLevel >= levelUpTimers.Length)   // Last level
            {
                foreach (MeshRenderer objVector in fruitPositions) {
                    objVector.enabled = true;
                }
                this.hasApples = true;
                currentLevel = levelUpTimers.Length + 1; // Break
                yield return null;
            }
        }
    }

    private IEnumerator TreeRefiller()
    {
        existingTime = 0;
        while (existingTime < TREE_REFILL)
        {
            yield return new WaitForSeconds(TREE_UPDATE);
            existingTime += TREE_UPDATE;  // Update 
        }

        foreach (MeshRenderer objVector in fruitPositions)
        {
            objVector.enabled = true;
        }
        this.hasApples = true;
        refRestart = null;
    }


    public TreeState getTreeState()
    {
        if (this.currentLevel > levelUpTimers.Length - 1) {
            
            return TreeState.FULL_GROWN;
        
        }
        
        return TreeState.GROWING;
    }
}
