using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* In order to make this class to work, you need to assign the same number of treelevels and levelUpTimers, each one
 * will be associated base on the index, which means the first mesh will be associated with the first levelUpTimer.
 * The fruitPositions should be empty gameobjects.*/ 
public class TreeManager : MonoBehaviour
{
    [SerializeField] private Mesh[] treeLevels; // Saves the different tree meshes 
    [SerializeField] private Transform[] fruitPositions;    // Positions on the tree where the fruit should appear
    public GameObject fruit;  // @TODO Fruit the tree grows. This should be passed through an InventoryObject_SO
    [SerializeField] private int[] levelUpTimers;   // Min time that should pass for the tree to be able to upgrade into the next level

    private bool hasApples; // Tree has apples at this time
    private int existingTime; // Time passed since creation or the last time a player dropped the fruits
    private int currentLevel;   //  Current level of the tree

    private const float TREE_UPDATE = 5f; // Const for set when the tree coroutines should continue their iterations

    private void Start()
    {
        existingTime = 0;
        hasApples = false;
        this.GetComponent<MeshFilter>().mesh = treeLevels[currentLevel]; // Change into the first level mesh
        StartCoroutine(LevelUpTimer());
    }

    /// <summary>
    /// Function for when a player shakes the tree to drop the tree's items.
    /// If the tree doesn't have any apples, it won't do anything
    /// </summary>
    public void shakeTree()
    {
        if (!hasApples)
            return;
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
            existingTime += 5;  // Update timer
            if (existingTime >= levelUpTimers[currentLevel])    // Upgrade level
            {
                this.GetComponent<MeshFilter>().mesh = treeLevels[currentLevel];
                currentLevel++;
            }
            if (currentLevel >= levelUpTimers.Length)   // Last level
            {
                foreach (Transform objVector in fruitPositions)
                    Instantiate(this.fruit, objVector); 
                currentLevel = levelUpTimers.Length + 1; // Break
                yield return null;
            }
        }
    }
}
