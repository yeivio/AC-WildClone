using System.Collections;
using System.Collections.Generic;
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
    private AudioSource audioSource;

    private bool hasApples; // Tree has apples at this time
    private float existingTime; // Time passed since creation or the last time a player dropped the fruits
    public int currentLevel;   //  Current level of the tree

    private const float TREE_UPDATE = 2f; // Const for set when the tree coroutines should continue their iterations
    private const float TREE_REFILL= 5f; // Minimun Time that should pass before refilling the tree with apples

    private GameObject currentActiveObject;
    public enum TreeState{
        GROWING, FULL_GROWN
    }


    private void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
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
        if(currentActiveObject.TryGetComponent(out Animator animator)) { 
            animator.GetComponent<Animator>().Play("AppleTree_Shake");
            audioSource.Play();
        }
        if (!hasApples)
            return;
        

        if (!BuildingSystem.current.DropItem(this.fruit, this.gameObject, new(1, 0, 1), this.gameObject.transform.forward * -1))
            BuildingSystem.current.DropItem(this.fruit, this.gameObject, new(2, 0, 1), this.gameObject.transform.forward * -1);

        if (!BuildingSystem.current.DropItem(this.fruit, this.gameObject, new(1, 0, 1), this.gameObject.transform.right))
            BuildingSystem.current.DropItem(this.fruit, this.gameObject, new(1, 0, 2), this.gameObject.transform.forward * -1);

        if (!BuildingSystem.current.DropItem(this.fruit, this.gameObject, new(1, 0, 1), this.gameObject.transform.right * -1))
            BuildingSystem.current.DropItem(this.fruit, this.gameObject, new(2, 0, 1), this.gameObject.transform.forward * -1);

        this.hasApples = false;
        foreach (MeshRenderer objVector in fruitPositions)
        {
            objVector.enabled = false;
        }
        StartCoroutine(TreeRefiller());
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
    }


    public TreeState getTreeState()
    {
        if (this.currentLevel > levelUpTimers.Length - 1) {
            
            return TreeState.FULL_GROWN;
        
        }
        
        return TreeState.GROWING;
    }
}
