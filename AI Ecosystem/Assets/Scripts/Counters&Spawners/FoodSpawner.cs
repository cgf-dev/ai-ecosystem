using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class FoodSpawner : MonoBehaviour
{
    // Variables
    // Spawn rate
    public float foodSpawnRate = 2f;
    // Time until next spawn
    private float nextFoodSpawn = 1f;
    // The maximum amount of Food 
    private float maxFood = 30f;

    // Food prefab I want to spawn
    public GameObject Food;

    // Create a list of all the 'Food's
    public List<GameObject> Foods = new List<GameObject>();



    void Update()
    {
        // If it is time to spawn Food
        if (ShouldSpawnFood() && Foods.Count < maxFood)
        {
            SpawnFood();
        }

        // Constantly remove all eaten Food items from the Foods list (if they are null, they have been eaten)
        Foods.RemoveAll(item => item == null);
    }


    // This method returns if time has surpassed the next Food spawn
    private bool ShouldSpawnFood()
    {
        return Time.time > nextFoodSpawn;
    }


    // This function spawns the Food
    private void SpawnFood()
    {
        // Instantiate the Food
        // Call it 'thisFood' upon instantiation
        GameObject thisFood = Instantiate(Food, new Vector3(Random.Range(-5, 5), transform.position.y, Random.Range(-5, 5)), transform.rotation);

        // If the Food that has just been instantiated is not already in the list then add it
        if (!Foods.Contains(thisFood))
            Foods.Add(thisFood);

        // Reset the nextFoodSpawn to the foodSpawnRate
        nextFoodSpawn = Time.time + foodSpawnRate;  
    }

    


}
