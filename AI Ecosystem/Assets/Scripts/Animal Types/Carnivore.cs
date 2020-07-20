using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carnivore : Animal, Animals
{
    // Declare additional closestFood objects
    public GameObject closestFood2;
    
    // This one will be chosen
    public GameObject actualClosestFood;

    // Create a list of closestFoods that we have found
    public List<GameObject> actualClosestFoods = new List<GameObject>();

    // This routine finds the closest Herbivore gameObject to the animal
    public override GameObject ChooseNearestFood()
    {
        // Variables used for finding closest food
        float distToClosestFood = Mathf.Infinity;
        GameObject closestFood = null;

        // Makes sure the we have access to the AnimalCounters script
        if (AnimalsCounter == null)
            AnimalsCounter = FindObjectOfType<AnimalsCounter>();


        // Go through each HerbivoreObject in the HerbivoresCounter List
        foreach (GameObject HerbivoreObject in AnimalsCounter.HerbivoresCounter)
        {
            if (HerbivoreObject != null)
            {
                // Work out the distance from each Herbivore item to the animal
                float distToFood = (HerbivoreObject.transform.position - this.transform.position).sqrMagnitude;
                // If the distance is smaller than the distance to the current closest Herbivore
                if (distToFood < distToClosestFood)
                {
                    // Overwite the closest Herbivore 
                    distToClosestFood = distToFood;
                    // Set the closestFood GameObject to this Herbivore
                    closestFood = HerbivoreObject;
                }
            }
        }

        // Return the closestFood GameObject
        return closestFood;
    }

    // This routine finds the closest Omnivore gameObject to the animal
    public GameObject ChooseNearestFood2()
    {
        // Variables used for finding closest food
        float distToClosestFood2 = Mathf.Infinity;
        GameObject closestFood2 = null;

        // Makes sure the we have access to the AnimalCounters script
        if (AnimalsCounter == null)
            AnimalsCounter = FindObjectOfType<AnimalsCounter>();


        // Go through each OmnivoreObject in the OmnivoresCounter List
        foreach (GameObject OmnivoreObject in AnimalsCounter.OmnivoresCounter)
        {
            if (OmnivoreObject != null)
            {
                // Work out the distance from each Omnivore item to the animal
                float distToFood2 = (OmnivoreObject.transform.position - this.transform.position).sqrMagnitude;
                // If the distance is smaller than the distance to the current closest Omnivore
                if (distToFood2 < distToClosestFood2)
                {
                    // Overwite the closest Omnivore 
                    distToClosestFood2 = distToFood2;
                    // Set the closestFood GameObject to this Omnivore
                    closestFood2 = OmnivoreObject;
                }
            }
        }

        // Return the closestFood GameObject
        return closestFood2;
    }


    // This routine finds the actualNearestFood gameObject to the animal
    public GameObject ChooseActualNearestFood()
    {
        // Variables used for finding actual closest food
        float distToActualClosestFood = Mathf.Infinity;
        GameObject actualClosestFood = null;


        // Go through each GameObject in the actualClosestFoods List
        foreach (GameObject T in actualClosestFoods)
        {
            if (T != null)
            {
                // Work out the distance from each Food/Animal item to the animal
                float distToActualFood = (T.transform.position - this.transform.position).sqrMagnitude;
                // If the distance is smaller than the distance to the current closest Food/Animal
                if (distToActualFood < distToActualClosestFood)
                {
                    // Overwite the closest Food/Animal item
                    distToActualClosestFood = distToActualFood;
                    // Set the closestFood GameObject to this Food/Animal
                    actualClosestFood = T;
                }
            }
        }

        // Return the actualClosestFood GameObject
        return actualClosestFood;
    }

    void Start()
    {
        // The agent's age begins at 0 for obvious reasons
        age = 0f;

        // The agent's hunger begins at 0
        hunger = 0f;

        // The agent's powerPoints begin at 0
        powerPoints = 0f;

        // The agent's reproductionPoints begin at 0
        reproductionPoints = 0f;
    }


    void Update()
    {
        // Increase the agent's age over time
        age += (ageRate * Time.deltaTime);

        // Animal (sadly) dies when they reach a certain age
        if (age >= ageLimit || hunger >= hungerLimit)
            Destroy(this.gameObject);

        // Increase the agent's hunger over time
        hunger += (hungerRate * Time.deltaTime);

        // Tells the animal to find it's closest Food item
        FindFood();

        // Tells the animal to move to it's closest Food item
        Move();

        // Check if the animal has met reproduction requirements
        if (reproductionPoints >= 3)
        {
            // Create a baby animal of this type
            Reproduce();
        }

        // Constantly remove all eaten Foods/Animals from the actualClosestFoods Lists (if they are null, they have been eaten)
        actualClosestFoods.RemoveAll(item => item == null);
    }


    // Sets up and OnTriggerEnter method for the animal
    void OnTriggerEnter(Collider other)
    {
        // Delete any Herbivore the animal comes into contact with
        if (other.tag == "Herbivore" || other.tag == "Omnivore")
        {
            if (other.tag == "Herbivore")
            // Destroy the Food item
            Destroy(other.gameObject);

            // Decide who wins via who owns the most powerPoints
            if (other.tag == "Omnivore")
            {
                if (this.powerPoints >= other.GetComponent<Animal>().powerPoints)
                {
                    Destroy(other.gameObject);
                }
                else
                    Destroy(this.gameObject);
            }
            // Call for growth of the animal
            Grow();
        }
    }


    // Override the FindFood method to find the nearest out of the 3 food choices
    // The FindFood method calls for the closestFood item using the ChooseNearestFood routine
    public override void FindFood()
    {
        // Get the distances from each 'closest' food
        closestFood = ChooseNearestFood();
        closestFood2 = ChooseNearestFood2();

        // Add the closestFoods to the actualClosestFood List
        // If they are not already contained
        if (!actualClosestFoods.Contains(closestFood))
            actualClosestFoods.Add(closestFood);
        if (!actualClosestFoods.Contains(closestFood2))
            actualClosestFoods.Add(closestFood2);

        // Set the actual closest food to the CLOSEST of the closest foods!
        actualClosestFood = ChooseActualNearestFood();
    }


    // The Move method does as expected, moves the animal to the actualClosestFood item found by FindFood()
    public override void Move()
    {
        // If the animal has a closestFood item, move towards it
        // This prevents errors when there is no Food/Animal items available (or when the game is launched)
        if (actualClosestFood != null)
            transform.position = Vector3.MoveTowards(transform.position, actualClosestFood.transform.position, speed * Time.deltaTime);
    }


    // Override the Reproduce function to spawn a Carnivore and add it to the List of Carnivores
    public override void Reproduce()
    {
        // Reset reproductionPoints
        reproductionPoints = 0;

        // Spawn a child animal just to the right of this animal
        GameObject childAnimal = Instantiate(CarnivoreObject, new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), transform.rotation);

        // Call the 'AddCarnivoreToList' method from the 'AnimalsCounter' script
        GameObject.Find("Animals Counter").GetComponent<AnimalsCounter>().AddCarnivoreToList(childAnimal);
    }
}
