using System;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour, Animals
{
    //variables 
    public float size;
    public float speed = 2f;
    public float hunger;
    public float hungerLimit = 15f;
    public float hungerRate = 2f;
    public float age;
    public float ageRate = 2f;
    public float ageLimit = 100f;
    public float reproductionPoints;
    public float powerPoints;

    // Allows us to see the animal's Food target
    public GameObject closestFood;

    // Allows us to access the Foods List
    public FoodSpawner FoodSpawnerScript;

    // Allows us to access the AnimalsCounter List
    public AnimalsCounter AnimalsCounter;

    // Allows us to reproduce each animal type
    public GameObject HerbivoreObject;
    public GameObject CarnivoreObject;
    public GameObject OmnivoreObject;

    // This routine finds the closest Food gameObject to the animal
    public virtual GameObject ChooseNearestFood()
    {
        // Variables used for finding closest food
        float distToClosestFood = Mathf.Infinity;
        GameObject closestFood = null;

        // Makes sure the we have access to the Food Spawner's script
        if (FoodSpawnerScript == null)
            FoodSpawnerScript = FindObjectOfType<FoodSpawner>();


        // Go through each Food item in the Foods List
        foreach (GameObject Food in FoodSpawnerScript.Foods)
        {
            if (Food != null)
            {
                // Work out the distance from each Food item to the animal
                float distToFood = (Food.transform.position - this.transform.position).sqrMagnitude;
                // If the distance is smaller than the distance to the current closest Food
                if (distToFood < distToClosestFood)
                {
                    // Overwite the closest Food item
                    distToClosestFood = distToFood;
                    // Set the closestFood GameObject to this Food
                    closestFood = Food;
                }
            }
        }

        // Return the closestFood GameObject
        return closestFood;
    }


    // The FindFood method calls for the closestFood item using the ChooseNearestFood routine
    public virtual void FindFood()
    {
        // Set the closestFood item using the ChooseNearestFood routine
        closestFood = ChooseNearestFood();
    }


    // The Move method does as expected, moves the animal to the closestFood item found by FindFood()
    public virtual void Move()
    {
        // If the animal has a closestFood item, move towards it
        // This prevents errors when there is no Food items available (or when the game is launched)
        if (closestFood != null)
            transform.position = Vector3.MoveTowards(transform.position, closestFood.transform.position, speed * Time.deltaTime);
    }

    // The Grow method simply increases the animal's localscale
    public void Grow()
    {
        // Set the animal's hunger back to 0 before growing
        hunger = 0f;

        // Give the animal an additional powerPoint
        powerPoints++;

        // Give the animal an additional reproductionPoint
        reproductionPoints++;

        // Amounts to grow by:
        float growthX = 0.05f;
        float growthY = 0.05f;
        float growthZ = 0.05f;

        // Increase localScale by growth amounts
        transform.localScale += new Vector3(growthX, growthY, growthZ);
    }

    // The reproduce method will create a duplicate of the original animal next to the parent
    public virtual void Reproduce()
    {
        // Reset reproductionPoints
        reproductionPoints = 0;

        // Spawn a child animal just to the right of this animal
        GameObject childAnimal = Instantiate(HerbivoreObject, new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z), transform.rotation);
    }
}


